using CustomColumns.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace CustomColumns.Data
{
	/// <summary>
	/// An optimised column-based DataTable equivalent with associated metadata and Key lookups
	/// </summary>
	/// <typeparam name="R">Must be a struct with any additional row metadata as fields</typeparam>
	/// <typeparam name="C">Can be a class or struct with any additional column metadata as fields</typeparam>
	[DebuggerDisplay("Rows = {RowCount}, Cols = {ColumnCount}, Expressions = {ExpressionsCount}")]
	[DebuggerTypeProxy(typeof(DataSource_DebuggerView<,>))]
    public class DataSource<R, C> : IBindingList, ITypedList
		where R : struct
		where C : new()
	{
		readonly List<DataRow<R, C>.MetaDataContainer<R>> _RowMetaData = new List<DataRow<R, C>.MetaDataContainer<R>>(DataColumn<C>.MinimumRowCapacity);
		readonly List<DataRow<R, C>> _Rows = new List<DataRow<R, C>>(DataColumn<C>.MinimumRowCapacity);
		readonly IDictionary<string, int> _LookupColumnByName = new Dictionary<string, int>(DataRow<R, C>.MaxColumns);
		readonly IDictionary<Key, int> _LookupRowByKey = new Dictionary<Key, int>(DataColumn<C>.MinimumRowCapacity, Key.Comparer);
		List<DataColumn<C>> _Columns = new List<DataColumn<C>>(DataRow<R, C>.MaxColumns);

		public delegate void MetaDataChangerDelegate(ref R metaData);

		/// <summary>
		/// Runs the action over all metadata allowing changes to be made conditionally
		/// </summary>
		/// <param name="action">The action.</param>
		public void ApplyChangeToAllMetaData(MetaDataChangerDelegate action)
		{
			for (int i = 0; i < _RowMetaData.Count; i++)
			{
				var copy = _RowMetaData[i];
				action(ref copy.Data);
				_RowMetaData[i] = copy;
			}
		}

		/// <summary>
		/// Deletes the row and any associated metadata if the key is found
        /// This does not notify/recalculate expressions/filter etc - this must be done manually
        /// NOTE: Does not resize the underlying arrays so space is not reclaimed
		/// </summary>
		/// <param name="key"></param>
		public void DeleteRowAndMetaData(Key key)
		{
			var index = TryGetRowIndex(key);
			if (index >= 0) DeleteRowAndMetaData(index);
		}

		/// <summary>
		/// Deletes the row and any associated metadata and/or Key mappings
        /// This does not notify/recalculate expressions/filter etc - this must be done manually
		/// NOTE: Does not resize the underlying arrays so space is not reclaimed
		/// </summary>
		/// <param name="rowIndexToDelete">The row index to delete.</param>
		public void DeleteRowAndMetaData(int rowIndexToDelete)
		{
			if (_Columns.Count == 0 || rowIndexToDelete >= _RowMetaData.Count) return;

			//1. Remove row metadata
			var lastRowIndex = _RowMetaData.Count - 1;
			var key = _RowMetaData[rowIndexToDelete].Key;
			//If not last row, swap first
			if (rowIndexToDelete < lastRowIndex)
			{
				_RowMetaData[rowIndexToDelete] = _RowMetaData[lastRowIndex];
				var swappedKey = _RowMetaData[lastRowIndex].Key;
				//Update any key to index mapping
				if (swappedKey != null) _LookupRowByKey[swappedKey] = rowIndexToDelete;
			}
			//And delete last row
			_RowMetaData.RemoveAt(lastRowIndex);

			//2. Remove any key
			if (key != null) _LookupRowByKey.Remove(key);

			//3. Remove from the columns
			foreach (var col in _Columns)
				col.DeleteRowBySwappingWithLast(rowIndexToDelete);

			//4. Remove the row
			_Rows.RemoveAt(lastRowIndex);
		}

		/// <summary>
		/// Gets or sets the object at the specific row index and column index
		/// NOTE: It is more efficient to get the column and then set multiple row-values on it
		/// Debug only: Set has type checking
		/// </summary>
		/// <value></value>
		public object this[int row, int col]
		{
			get
			{
				try
				{
					return col < _Columns.Count ? _Columns[col][row] : null;
				}
				catch
				{
					return null;
				}
			} 
			set { _Columns[col][row] = value; }
		}

		#region Expressions
		const int StackLimit = DataRow<R, C>.MaxColumns;
        const int ExpressionFilterIndex = -2;
		internal readonly List<int> _ColumnsWithExpressions = new List<int>(DataRow<R, C>.MaxColumns + 1);
        readonly List<int> _CalculationOrder = new List<int>(DataRow<R, C>.MaxColumns + 1);
        ulong[] _Filter;

        /// <summary>
        /// The expression for any filter
        /// </summary>
        public DataBooleanExpression ExpressionFilter { get; private set; }

        /// <summary>
        /// Only true if there is a fully valid and calculated filter present
        /// </summary>
        public bool HasValidFilter { get; private set; }

        /// <summary>
        /// The expression filter name to optionally use with ResetExpressions to set up the filter
        /// </summary>
        public readonly string ExpressionFilterName = "Filter.DataSource";

		/// <summary>
		/// The number of columns containing expressions including optional filter
		/// </summary>
		public int ExpressionsCount { get { return _ColumnsWithExpressions.Count; } }

		/// <summary>
		/// If there are valid non-constant expressions that can be calculated
		/// via Recalculate()
		/// </summary>
		public bool HasValidExpressions { get { return _ColumnsWithExpressions.Count != 0 && _CalculationOrder.Count != 0; } }

        /// <summary>
        /// Callback to be notified when ExpressionFilter changes: arguments are (ExpressionFilter, HaveValidFilter).
        /// If expression itself is reset or modified is fired with !HaveValidFilter.
        /// Every time expression is calculated is fired with HaveValidFilter.
        /// </summary>
        public Action<DataBooleanExpression, bool> OnFilterChanged;

        /// <summary>
        /// Callback to be notified whenever calculation occurs: arguments are (ColumnsThatChanged, RowsThatChanged) 
        /// and either can be null to indicate all.
        /// </summary>
        public Action<ICollection<int>, ICollection<Key>> OnCalculation;

        /// <summary>
        /// Modifies or clears the filter expression, leaving any other expressions untouched
        /// Recalculation needs to be done separately
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="newFilterExpression">The expression or null to clear</param>
        public void EditFilterExpression(Parser parser, string newFilterExpression)
        {
            var bClearingExpression = string.IsNullOrEmpty(newFilterExpression);

            //Nothing to do if no expression assigned and none passed in
            if (ExpressionsCount == 0 && bClearingExpression) return;

            if (bClearingExpression)
            {
                if (ExpressionFilter != null)
                {
                    ExpressionFilter = null;
                    HasValidFilter = false;
                    _ColumnsWithExpressions.Remove(ExpressionFilterIndex);
                    _CalculationOrder.Remove(ExpressionFilterIndex);

                    _FireOnFilterChanged();
                }
                return;
            }

            //Try to re-use expression where possible
            if (ExpressionFilter != null)
            {
                if (ExpressionFilter.Error == null && ExpressionFilter.Expression == newFilterExpression)
                {
                    //Currently assigned expression is error free and matches exactly the new expression
                    return;
                }
            }
            else
            {
                //Note that because any filter is added last it will always be the last item in the calculation order
                _ColumnsWithExpressions.Add(ExpressionFilterIndex);
                ExpressionFilter = null;
            }

            HasValidFilter = false;
            _CalculationOrder.Clear();

            //Will need to re-parse so build the column name mapping
            var dict = new Dictionary<string, Type>(_Columns.Count);
            foreach (var col in _Columns)
            {
                dict.Add(col.Name, col.Type);
            }

            var processed = new HashSet<int>();
            var exp = new DataBooleanExpression(parser, newFilterExpression, dict, name => _LookupColumnByName[name]);
            //Handle constant columns (parse errors are handled later)
            if (exp.Error == null && exp.ColumnsNeeded == null)
            {
                processed.Add(ExpressionFilterIndex);
                exp.Error = "Filter must not be constant";
            }
            ExpressionFilter = exp;

            _DetermineCalculationOrder(processed);
            _FireOnFilterChanged();
        }

		/// <summary>
		/// Clears and sets the expressions given a list of column names and expression texts
		/// Note that any other column expressions will be removed including any filter. 
        /// Recalculation needs to be done separately
        /// 
        /// Advanced:
        /// If the name/value matches exactly, the expression will be re-used, something that only works if the columns have not changed type or name/index in the mean time.
        /// When re-using expressions, the recalculation call should not pass any columns to force a complete recalculation unless you are very sure that this is not needed.
        /// E.g. you know only one expression is being edited and you know the other existing expressions do not depend up on it so you can just say to recalculate the one column.
		/// </summary>
		/// <param name="parser">The parser.</param>
		/// <param name="expressions">The expressions or null to clear all; use ExpressionFilter as the name of any desired filter</param>
		public void ResetExpressions(Parser parser, ICollection<KeyValuePair<string, string>> expressions)
		{
            var bClearingExpressions = expressions == null || expressions.Count == 0;

			//Nothing to do if no expressions assigned and none passed in
			if (ExpressionsCount == 0 && bClearingExpressions) return;

            var originalFilter = ExpressionFilter;
            HashSet<int> processed = null;
            Dictionary<string, Type> dict = null;
            if (!bClearingExpressions)
            {
                //Try to re-use expressions where possible
                processed = new HashSet<int>();
                foreach (var pair in expressions)
                {
                    if (object.ReferenceEquals(pair.Key, ExpressionFilterName))
                    {
                        if (ExpressionFilter != null)
                        {
                            if (ExpressionFilter.Error == null && ExpressionFilter.Expression == pair.Value)
                            {
                                //Currently assigned expression is error free and matches exactly the new expression
                                processed.Add(ExpressionFilterIndex);
                            }
                        }
                        continue;
                    }
                    var idx = _LookupColumnByName[pair.Key];
                    var col = _Columns[idx];
                    var exp = col.Expression;
                    if (exp != null)
                    {
                        if (exp.Error == null && exp.Expression == pair.Value)
                        {
                            //Currently assigned expression is error free and matches exactly the new expression
                            processed.Add(idx);
                        }
                    }
                }

                dict = new Dictionary<string, Type>(_Columns.Count);
			    //Clear all changed or new expressions and create field dictionary
                foreach (var col in _Columns)
                {
                    if (!processed.Contains(col.Index)) col.Expression = null;
                    dict.Add(col.Name, col.Type);
                }
            }
            else
            {
			    //Clear all expressions
                foreach (var col in _Columns)
                {
                    col.Expression = null;
                }
                //And the filter
                ExpressionFilter = null;
            }

			_ColumnsWithExpressions.Clear();
            _CalculationOrder.Clear();
            HasValidFilter = false;

			//Nothing more to do if no expressions passed in
            if (bClearingExpressions)
            {
                if (!ReferenceEquals(originalFilter, ExpressionFilter)) _FireOnFilterChanged();
                return;
            }

            //Only clear filter if not re-using it
            if (!processed.Contains(ExpressionFilterIndex)) ExpressionFilter = null;

            processed.Clear();
            string filter = null;

			//Parse, compile and assign expressions
			foreach (var pair in expressions)
			{
                if (object.ReferenceEquals(pair.Key, ExpressionFilterName))
                {
                    filter = pair.Value;
                    continue;
                }
				var idx = _LookupColumnByName[pair.Key];
				_ColumnsWithExpressions.Add(idx);
				var col = _Columns[idx];
                //Re-use expression if possible
                if (col.Expression != null) continue;

				//Marks column as IsDirty
				var exp = new DataColumnExpression(parser, pair.Value, dict, name => _LookupColumnByName[name]);
				if (exp.Error == null)
				{
					col.ResetTypeOnly(parser.ResultType);
					dict[pair.Key] = parser.ResultType;
				}
				col.Expression = exp;
				//Handle constant columns (parse errors are handled later)
				if (exp.Error == null && exp.ColumnsNeeded == null)
				{
					processed.Add(idx);
					object constant = null;
					try
					{
						constant = exp.CompiledDelegate(null);
                        exp.HasNeverBeenCalculated = false;
                    }
					catch (Exception ex)
					{
						exp.Error = ex.Message;
						//Errors handled later
						continue;
					}
					col.SetConstantValue(constant);
				}
			}

            //Note that because any filter is added last it will always be the last item in the calculation order
            if (filter != null)
            {
                _ColumnsWithExpressions.Add(ExpressionFilterIndex);
                //Only create if not re-using
                if (ExpressionFilter == null)
                {
                    var exp = new DataBooleanExpression(parser, filter, dict, name => _LookupColumnByName[name]);
                    //Handle constant columns (parse errors are handled later)
                    if (exp.Error == null && exp.ColumnsNeeded == null)
                    {
                        processed.Add(ExpressionFilterIndex);
                        exp.Error = "Filter must not be constant";
                    }
                    ExpressionFilter = exp;
                }
            }

            _DetermineCalculationOrder(processed);
            if (!ReferenceEquals(originalFilter, ExpressionFilter)) _FireOnFilterChanged();
        }

        #region Private helpers

        #region Determine calculation order
        void _DetermineCalculationOrder(HashSet<int> processed)
        {
            _CalculationOrder.Clear();
            var order = new List<int>(DataRow<R, C>.MaxColumns);

            //Recursively determine calculation order, watching out for loops
            foreach (var idx in _ColumnsWithExpressions)
            {
                var col = idx >= 0 ? _Columns[idx] : null;
                var expression = idx >= 0 ? col.Expression : ExpressionFilter;

                if (!processed.Contains(idx))
                {
                    if (expression.Error == null)
                    {
                        var stackDepth = StackLimit;
                        order.Clear();
                        _RecurseExpressionDependencies(expression, processed, order, ref stackDepth);

                        //If no error occurred, watch out for stack overflow
                        if (expression.Error == null)
                        {
                            if (stackDepth >= 0)
                            {
                                _CalculationOrder.AddRange(order);
                                _CalculationOrder.Add(idx);
                            }
                            else expression.Error = "Circular references detected";
                        }
                    }
                    processed.Add(idx);
                }
                //Clear error columns (propagation handled by recursion)
                if (expression.Error != null && col != null) col.SetConstantValue(null);
            }
        }

        void _RecurseExpressionDependencies(DataColumnExpression expression, ISet<int> processed, List<int> order, ref int stackDepth)
        {
            Debug.Assert(expression.Error == null);
            stackDepth--;
            if (stackDepth < 0) return;
            if (expression.ColumnsNeeded != null)
            {
                string errors = null;
                foreach (var idx in expression.ColumnsNeeded)
                {
                    if (processed.Contains(idx)) continue;
                    var col = _Columns[idx];
                    if (col.Expression != null)
                    {
                        if (col.Expression.Error == null)
                        {
                            _RecurseExpressionDependencies(col.Expression, processed, order, ref stackDepth);
                            //Expression dependencies have been calculated
                            order.Add(col.Index);
                        }
                        //Stop on first error
                        if (col.Expression.Error != null)
                        {
                            processed.Add(idx);
                            errors = col.Expression.Error;
                            break;
                        }
                    }
                    processed.Add(idx);
                }
                expression.Error = errors;
            }
        }
        #endregion

        #region Apply filter
        void _FireOnFilterChanged()
        {
            if (OnFilterChanged != null) OnFilterChanged(ExpressionFilter, HasValidFilter);
        }

        void _DoFilter(ICollection<Key> rows, int errorThreshold, ref int res)
        {
            HasValidFilter = false;
            var expression = ExpressionFilter;
            var error = expression.Error;
            //Valid and non-constant column
            if (error == null && expression.ColumnsNeeded != null)
            {
                expression.LastErrorCount = 0;
                expression.HasNeverBeenCalculated = false;
                if (RowCount == 0) 
                {
                    HasValidFilter = true;
                    _FireOnFilterChanged();
                    return;
                }

                var longs = RowCount / 64 + 2;
                if (_Filter == null || _Filter.Length < longs) _Filter = new ulong[longs];

                var args = new object[expression.DelegateArguments.Length];
                var cols = new DataColumn<C>[args.Length];
                for (int a = 0; a < args.Length; a++)
                {
                    cols[a] = _Columns[expression.DelegateArguments[a]];
                }
                var block = 0ul;
                if (rows == null)
                {
                    //Finally loop over all rows in target column
                    for (int i = 0; i < RowCount; i++)
                    {
                        var mod = i % 64;
                        if (mod == 0)
                        {
                            var bidx = i / 64;
                            //Commit previous and acquire new block by block to minimise array churn
                            if (bidx > 0) _Filter[bidx - 1] = block;
                            block = 0ul;
                        }

                        bool value;
                        if (_DoOneRow(i, expression, cols, args, out value, errorThreshold))
                        {
                            if (value) block |= 1ul << (i % 64);
                            continue;
                        }
                        //No point in clearing the rest of the flags
                        expression.LastErrorCount = 1;
                        res++;
                        break;
                    }
                    //Possible partial last block
                    _Filter[(RowCount - 1) / 64] = block;
                }
                else
                {
                    foreach (var r in rows)
                    {
                        var i = _LookupRowByKey[r];
                        bool value;
                        if (_DoOneRow(i, expression, cols, args, out value, errorThreshold))
                        {
                            var bit = 1ul << (i % 64);
                            var bidx = i / 64;
                            block = _Filter[bidx];

                            if (value) block |= bit;
                            else block &= ~bit;

                            _Filter[bidx] = block;
                            continue;
                        }
                        //No point in clearing the rest of the flags
                        expression.LastErrorCount = 1;
                        res++;
                        break;
                    }
                }
                HasValidFilter = expression.LastErrorCount == 0;
                _FireOnFilterChanged();
            }
        }
        #endregion

        #endregion

        #endregion

        #region Calculation

        /// <summary>
        /// Recalculates filter and any other non-calculated expressions
        /// NOTE: This also signifies what data has changed, notifying any listener
        /// <remarks>The results are not cloned. Do not alter</remarks>
        /// </summary>
        /// <param name="rows">The rows to recalculate or null for all.</param>
        /// <param name="errorThreshold">The error threshold for the non-filter columns. Set on exit to count of columns that reached it. Use -1 for none.</param>
        public void RecalculateFilter(ICollection<Key> rows, ref int errorThreshold)
        {
            Recalculate(new[] { ExpressionFilterIndex }, rows, ref errorThreshold);
        }

        /// <summary>
        /// Recalculates any expressions given the specified changed columns.
        /// NOTE: This also signifies what data has changed, notifying any listener
        /// <remarks>The results are not cloned. Do not alter</remarks>
        /// </summary>
        /// <param name="changedColumns">The changed columns or null for all</param>
        /// <param name="rows">The rows to recalculate or null for all.</param>
        /// <param name="errorThreshold">The error threshold. Set on exit to count of columns that reached it. Use -1 for none.</param>
        /// <returns>
        /// Union of changedColumns and calculated columns - may be used for optimal UI refreshing
        /// </returns>
        public ICollection<int> Recalculate(ICollection<int> changedColumns, ICollection<Key> rows, ref int errorThreshold)
        {
            if (!HasValidExpressions)
            {
                _FireOnCalculation(changedColumns, rows);
                errorThreshold = 0;
                return changedColumns;
            }

            var res = 0;
            var order = _CalculationOrder;

            //Calculate reduced list based on changed columns
            if (changedColumns != null && changedColumns.Count > 0)
            {
                order = new List<int>(_CalculationOrder.Count);
                var columnsToCalculate = new SortedSet<int>(changedColumns);
                foreach (var idx in _CalculationOrder)
                {
                    var exp = idx >= 0 ? _Columns[idx].Expression : ExpressionFilter;
                    if (!exp.HasNeverBeenCalculated && !changedColumns.Contains(idx))
                    {
                        //Has been calculated at least once and user is not forcing a recalculation then only 
                        //recalculate if any of its dependencies are being recalculated.
                        var neededFields = exp.ColumnsNeeded;
                        //Constant and error expressions have already been set
                        if (neededFields == null) continue;
                        if (!columnsToCalculate.Overlaps(neededFields)) continue;
                    }
                    columnsToCalculate.Add(idx);
                    order.Add(idx);
                }
            }

            //Nothing to calculate
            if (order.Count == 0)
            {
                _FireOnCalculation(changedColumns, rows);
                errorThreshold = 0;
                return changedColumns;
            }

            var bDoFilter = false;

            foreach (var idx in order)
            {
                if (idx < 0)
                {
                    bDoFilter = true;
                    continue;
                }

                var col = _Columns[idx];
                var expression = col.Expression;
                var error = expression.Error;
                //Valid and non-constant column
                if (error == null && expression.ColumnsNeeded != null)
                {
                    var args = new object[expression.DelegateArguments.Length];
                    var cols = new DataColumn<C>[args.Length];
                    for (int a = 0; a < args.Length; a++)
                    {
                        cols[a] = _Columns[expression.DelegateArguments[a]];
                    }
                    expression.LastErrorCount = 0;
                    expression.HasNeverBeenCalculated = false;
                    if (rows == null)
                    {
                        //Finally loop over all rows in target column
                        for (int i = 0; i < RowCount; i++)
                        {
                            object value;
                            if (_DoOneRow(i, expression, cols, args, out value, errorThreshold))
                            {
                                col[i] = value;
                                continue;
                            }
                            //And clear rest of column
                            col.SetConstantValue(null, i);
                            res++;
                            break;
                        }
                    }
                    else
                    {
                        foreach (var r in rows)
                        {
                            var i = _LookupRowByKey[r];
                            object value;
                            if (_DoOneRow(i, expression, cols, args, out value, errorThreshold))
                            {
                                col[i] = value;
                                continue;
                            }
                            //And clear rest of column
                            col.SetConstantValue(null, i);
                            res++;
                            break;
                        }
                    }
                }
            }

            if (bDoFilter)
            {
                //Assume one error stops the filter
                errorThreshold = 0;
                _DoFilter(rows, errorThreshold, ref res);
            }

            var combined = new HashSet<int>();
            if (order.Count > 0) foreach (var o in order) combined.Add(o);
            if (changedColumns != null) foreach (var c in changedColumns) combined.Add(c);

            if (OnCalculation != null)
            {
                if (!(bDoFilter && combined.Count == 1 && combined.Contains(ExpressionFilterIndex)))
                {
                    //Only fire if it's not a pure filter event
                    _FireOnCalculation(combined, rows);
                }
            }

            errorThreshold = res;
            return combined;
        }

        #region Private helpers
        void _FireOnCalculation(ICollection<int> changedColumns, ICollection<Key> changedRows)
        {
            if (OnCalculation != null) OnCalculation(changedColumns, changedRows);
        }
        
        static bool _DoOneRow(int i, DataColumnExpression expression, DataColumn<C>[] cols,
            object[] args, out object res, int errorThreshold)
        {
            int nullColumns = 0;
            for (int a = 0; a < cols.Length; a++)
            {
                var arg = cols[a][i];
                args[a] = arg;
                if (arg == null) nullColumns++;
            }

            //Force to null if all inputs are null
            //TODO: Support if any column is null - i.e. there is currently no support for nullable types
            //if (nullColumns == cols.Length)
            if (nullColumns > 0)
            {
                res = null;
                return true;
            }

            //And call
            try
            {
                res = expression.CompiledDelegate(args);
                return true;
            }
            catch (Exception)
            {
                //TODO: show error somewhere but not in column due to type
                res = null;
                expression.LastErrorCount++;
                if ((errorThreshold >= 0) && (expression.LastErrorCount > errorThreshold))
                {
                    return false;
                }
                return true;
            }
        }

        static bool _DoOneRow(int i, DataBooleanExpression expression, DataColumn<C>[] cols,
            object[] args, out bool res, int errorThreshold)
        {
            int nullColumns = 0;
            for (int a = 0; a < cols.Length; a++)
            {
                var arg = cols[a][i];
                args[a] = arg;
                if (arg == null) nullColumns++;
            }

            //Force to false if all inputs are null
            //TODO: Support if any column is null - i.e. there is currently no support for nullable types
            //if (nullColumns == cols.Length)
            if (nullColumns > 0)
            {
                res = false;
                return true;
            }

            //And call
            try
            {
                res = expression.CompiledDelegate(args);
                return true;
            }
            catch (Exception)
            {
                //TODO: show error somewhere but not in column due to type
                res = false;
                expression.LastErrorCount++;
                if ((errorThreshold >= 0) && (expression.LastErrorCount > errorThreshold))
                {
                    return false;
                }
                return true;
            }
        }
        #endregion 

        #endregion

        #region Filter accessors
        /// <summary>
        /// Accessor for any filter - will return an empty enumerator if no filter, or filter is in error.
        /// See HaveValidFilter property.
        /// </summary>
        public IEnumerable<bool> FilterEnumerator()
        {
            var count = HasValidFilter ? RowCount : 0;
            return new _FilterEnumerator(_Filter, count);
        }

        class _FilterEnumerator : IEnumerator<bool>, IEnumerable<bool>
        {
            readonly int Rows;
            readonly ulong[] Blocks;
            int _current;
            ulong _block;

            internal _FilterEnumerator(ulong[] blocks, int rows)
            {
                Rows = rows;
                Blocks = blocks;
                Reset();
            }

            public bool MoveNext()
            {
                if (_current < Rows - 1)
                {
                    _current++;
                    var mod = _current % 64;
                    if (mod == 0)
                    {
                        var bidx = _current / 64;
                        _block = Blocks[bidx];
                    }
                    var bit = 1ul << mod;
                    Current = (_block & bit) != 0ul;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _current = -1;
            }

            public bool Current { get; set; }

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get { throw new NotImplementedException(); }
            }


            public IEnumerator<bool> GetEnumerator()
            {
                return this;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        /// <summary>
		/// Gets the metadata for the given row
		/// Do not use if only the Key is required
		/// </summary>
		/// <param name="row">The row.</param>
		/// <returns></returns>
		public DataRow<R, C>.MetaDataContainer<R> GetMetaDataContainer(int row) { return _RowMetaData[row]; }

		/// <summary>
		/// Gets the metadata for the given row
		/// Do not use if the Key is also required
		/// </summary>
		/// <param name="row">The row.</param>
		/// <returns></returns>
		public R GetMetaData(int row) { return _RowMetaData[row].Data; }

		/// <summary>
		/// Gets the key for row without getting the entire MetaData
		/// </summary>
		/// <param name="row">The row.</param>
		/// <returns></returns>
		public Key GetKeyForRow(int row) { return _RowMetaData[row].Key; }

		/// <summary>
		/// Sets the metadata for the given row
		/// T is expected to be a structure so must be provided
		/// If key is non-null, it updates the key/row mappings;
		/// if null, it removes the mapping.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="newData">The new data.</param>
		/// <param name="key">The key. Use null to remove</param>
		public void SetMetaDataContainer(int row, R newData, Key key)
		{
			var copy = _RowMetaData[row];
			copy.Data = newData;
			copy.Key = key;
			_RowMetaData[row] = copy;
			if (key != null) _LookupRowByKey[key] = row;
			else if (copy.Key != null) _LookupRowByKey.Remove(copy.Key);
		}

		/// <summary>
		/// Updates the meta data for the given row without touching the key
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="newData">The new data.</param>
		public void UpdateRowMetaData(int row, R newData)
		{
			var copy = _RowMetaData[row];
			copy.Data = newData;
			_RowMetaData[row] = copy;
		}

		/// <summary>
		/// Gets the column count.
		/// </summary>
		/// <value>The column count.</value>
		public int ColumnCount { get { return _Columns.Count; } }

		/// <summary>
		/// Gets the row count.
		/// </summary>
		/// <value>The row count.</value>
		public int RowCount { get { return _Rows.Count; } }

		/// <summary>
		/// Allows enumeration of the columns (always sequential)
		/// Use DataColumn.Index as a convenience to retrieve the index
		/// </summary>
		/// <value>The columns.</value>
		public IList<DataColumn<C>> Columns { get { return _Columns; } }

		/// <summary>
		/// Get index of column by name or negative if not found
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public int TryGetColumnIndex(string name)
		{
			int idx;
			if (name != null && _LookupColumnByName.TryGetValue(name, out idx)) return idx;
			return -1;
		}

        /// <summary>
        /// Get index of column by binding name or negative if not found
        /// </summary>
        /// <param name="name">The binding name.</param>
        /// <returns></returns>
        public int TryGetColumnIndexByBindingName(string name)
        {
            return Array.BinarySearch(_PropertiesByName, name);
        }

		/// <summary>
		/// Gets the column by name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public DataColumn<C> GetColumn(string name)
		{
            int idx = GetColumnIndex(name);
	  	    return Columns[idx];
		}

        /// <summary>
        /// Get index of column by name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public int GetColumnIndex(string name)
        {
            var idx = TryGetColumnIndex(name);
            if (idx >= 0) return idx;
            throw new ArgumentOutOfRangeException(string.Format("Column '{0}' not found.", name));
        }


		/// <summary>
		/// Get the column name by the datasource column index or null if not found
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public string TryGetColumnBindingName(int index)
		{
			if (index >= 0 && index < _Properties.Count) return _Properties[index].Name;
			return null;
		}

		/// <summary>
		/// Get index of the row by key or negative if not found
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public int TryGetRowIndex(Key key)
		{
			int idx;
			if (key != null && _LookupRowByKey.TryGetValue(key, out idx)) return idx;
			return -1;
		}

		/// <summary>
		/// Adds the new rows. After doing this call, get the metadata for each row
		/// and update the Key for e.g.
        /// This does not notify/recalculate expressions/filter etc - this must be done manually
        /// NOTE: The default value is not deep copied/cloned so don't set the Key or any other reference types
		/// </summary>
		/// <param name="count">The count.</param>
		/// <param name="defaultValue">The default value. It is not cloned!</param>
		/// <returns></returns>
		public void AddNewRows(int count, DataRow<R, C>.MetaDataContainer<R> defaultValue = default(DataRow<R,C>.MetaDataContainer<R>))
		{
			Debug.Assert(_Columns.Count > 0, "Must add columns first");
			var newRows = new DataRow<R, C>[count];
			for (int i = 0; i < count; i++)
			{
				newRows[i] = new DataRow<R, C>(this, RowCount + i);
			}

			var existingCapacity = _Rows.Capacity;
			var targetCapacity = _Rows.Count + count;
			if (existingCapacity < targetCapacity)
			{
				_Rows.Capacity = targetCapacity;
				_RowMetaData.Capacity = targetCapacity;
			}

			_Rows.AddRange(newRows);

			foreach (var dataColumn in _Columns)
				dataColumn.AddNewRows(count);

			while (count > 0)
			{
				_RowMetaData.Add(defaultValue);
				count--;
			}
		}

		/// <summary>
		/// Reserves capacity but does not add new rows
		/// </summary>
		/// <param name="estimatedTotalRows"></param>
		public void ReserveCapacity(int estimatedTotalRows)
		{
			Debug.Assert(_Columns.Count > 0, "Must add columns first");
			if (_Rows.Count >= estimatedTotalRows) return;
			_Rows.Capacity = estimatedTotalRows;
			foreach (var dataColumn in _Columns)
				dataColumn.ReserveCapacity(estimatedTotalRows);
			_RowMetaData.Capacity = estimatedTotalRows;
		}

		/// <summary>
		/// Trims the excess rows.
		/// </summary>
		/// <param name="newRowCount">The new row count.</param>
		public void TrimExcessRows(int newRowCount)
		{
			Debug.Assert(ColumnCount > 0, "Must add columns first");
			if (newRowCount == _Rows.Count) return;
			var existingCount = _Rows.Count;
			if (newRowCount > existingCount) throw new ArgumentOutOfRangeException("newRowCount");
			_Rows.RemoveRange(newRowCount, existingCount - newRowCount);
			_Rows.TrimExcess();

			foreach (var dataColumn in _Columns)
				dataColumn.TrimExcessRows(newRowCount);

			for (int i = newRowCount; i < _RowMetaData.Count; i++)
			{
				var key = _RowMetaData[i].Key;
				if (key != null) _LookupRowByKey.Remove(key);
			}

			_RowMetaData.RemoveRange(newRowCount, existingCount - newRowCount);
			_RowMetaData.TrimExcess();

			Debug.Assert(_Rows.Count == newRowCount);
			Debug.Assert(_RowMetaData.Count == newRowCount);
			Debug.Assert(_LookupRowByKey.Count <= newRowCount);
			Debug.Assert(_RowMetaData.Count == newRowCount);
		}

		/// <summary>
		/// Removes all columns, clearing all row metadata etc
		/// </summary>
		public void RemoveAllColumns()
		{
			_Columns.Clear();
			_LookupColumnByName.Clear();
			_LookupRowByKey.Clear();
			_RowMetaData.Clear();
			_Rows.Clear();
            _ClearExpressions();
        }

        void _ClearExpressions()
        {
            _CalculationOrder.Clear();
            _ColumnsWithExpressions.Clear();
            ExpressionFilter = null;
            HasValidFilter = false;
        }


		/// <summary>
		/// Trims the columns off end. Does not touch row metadata until all
		/// columns are removed.
        /// This does not notify/recalculate expressions/filter etc - this must be done manually
        /// </summary>
		/// <param name="toRemove">To remove.</param>
		public void TrimColumnsOffEnd(int toRemove)
		{
			var newCount = _Columns.Count - toRemove;
			if (newCount < 0 || _Columns.Count == newCount) return;
			if (newCount == 0)
			{
				RemoveAllColumns();
				return;
			}
			_Columns.RemoveRange(_Columns.Count - newCount, newCount);
			//And reindex
			_LookupColumnByName.Clear();
			for (int i = 0; i < _Columns.Count; i++)
			{
				_LookupColumnByName.Add(_Columns[i].Name, i);
			}
            _ClearExpressions();
		}

		/// <summary>
		/// Appends columns with given name and type to the end of any existing columns
        /// This does not notify/recalculate expressions/filter etc - this must be done manually
        /// Debug: Type is checked in debug only
		/// </summary>
		/// <param name="columns">The new columns to append.</param>
		public void AppendColumns(IList<KeyValuePair<string, Type>> columns)
		{
			var rowCount = _Rows.Count;
			for (int i = 0; i < columns.Count; i++)
			{
				var pair = columns[i];
				var idx = _Columns.Count;
				_LookupColumnByName.Add(pair.Key, idx);
				_Columns.Add(new DataColumn<C>(idx, rowCount, pair.Key, pair.Value));
			}
		}

		/// <summary>
		/// Adjusts (any) existing columns to match the set provided,
		/// adding, removing or replacing as necessary and removing all expressions including any filter.
		/// Order is ignored: the name is used when matching.
        /// This does not notify/recalculate expressions/filter etc - this must be done manually
        /// NOTE: If adding new ones only, it is more efficient to use AppendColumns instead
		/// Debug: Type is checked in debug only
		/// </summary>
		/// <param name="columns">The complete final columns ignoring order.</param>
		public void AdjustColumns(IList<KeyValuePair<string, Type>> columns)
		{
			if (columns == null || columns.Count == 0)
			{
				TrimColumnsOffEnd(_Columns.Count);
				return;
			}
			//Must be sorted in column order
			var existingIdxToKeep = new SortedSet<int>();
			var newIdxToAdd = new Queue<int>(columns.Count);
			int idx;
			for (int i = 0; i < columns.Count; i++)
			{
				var name = columns[i].Key;
				if (!_LookupColumnByName.TryGetValue(name, out idx))
				{
					newIdxToAdd.Enqueue(i);
					continue;
				}
				existingIdxToKeep.Add(idx);
				//For now, clear all expressions
				_Columns[idx].Expression = null;

				if (columns[i].Value != _Columns[idx].Type)
				{
					_Columns[idx].SetTypeAndClear(name, columns[i].Value);
				}
			}
			//For now, clear all expressions
            _ClearExpressions();

			//Clear mapping for now
			_LookupColumnByName.Clear();
			//Attempt to reuse any columns that are to be deleted
			if (newIdxToAdd.Count > 0 && existingIdxToKeep.Count != _Columns.Count)
			{
				for (int i = 0; i < _Columns.Count; i++)
				{
					if (existingIdxToKeep.Contains(i)) continue;
					var newColumn = columns[newIdxToAdd.Dequeue()];
					//This column is to be deleted so reuse instead
					existingIdxToKeep.Add(i);
					var columnToReuse = _Columns[i];
					columnToReuse.SetTypeAndClear(newColumn.Key, newColumn.Value);
					//No more to add
					if (newIdxToAdd.Count == 0) break;
				}
			}
			//Delete any columns not needed
			if (existingIdxToKeep.Count != _Columns.Count)
			{
				var columnsToKeep = new List<DataColumn<C>>(existingIdxToKeep.Count);
				idx = 0;
				foreach (var toKeep in existingIdxToKeep)
				{
					//Keeping this column
					var col = _Columns[toKeep];
					//Update index on column being kept
					col.Index = idx;
					columnsToKeep.Add(col);
					idx++;
				}
				_Columns = columnsToKeep;
			}
			//Add any new columns needed
			if (newIdxToAdd.Count > 0)
			{
				_Columns.Capacity = _Columns.Count + newIdxToAdd.Count;
				do
				{
					var pair = columns[newIdxToAdd.Dequeue()];
					_Columns.Add(new DataColumn<C>(_Columns.Count, _Rows.Count, pair.Key, pair.Value));
				} while (newIdxToAdd.Count > 0);
			}
			//And reindex
			for (int i = 0; i < _Columns.Count; i++)
			{
				_LookupColumnByName.Add(_Columns[i].Name, i);
			}
		}

		static readonly PropertyDescriptorCollection _Properties;
        static readonly string[] _PropertiesByName;

		static DataSource()
		{
			//Get the 'shape' of the list
			//Only get the public properties
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(typeof(DataRow<R, C>));

			//Sort the properties
			_Properties = pdc.Sort();
            //Store name mapping
            _PropertiesByName = new string[_Properties.Count];
            for (int i=0; i<_PropertiesByName.Length; i++)
            {
                _PropertiesByName[i] = _Properties[i].Name;
            }
		}

        #region ITypedList Members

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return _Properties;
		}

		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			return typeof(DataRow<R, C>).Name;
		}

		#endregion ITypedList Members

		#region IBindingList Members

		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			throw new NotImplementedException();
		}

		object IBindingList.AddNew()
		{
			throw new NotImplementedException();
		}

		bool IBindingList.AllowEdit
		{
			get { return false; }
		}

		bool IBindingList.AllowNew
		{
			get { return false; }
		}

		bool IBindingList.AllowRemove
		{
			get { return false; }
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw new NotImplementedException();
		}

		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw new NotImplementedException();
		}

		bool IBindingList.IsSorted
		{
			get { return false; }
		}

		event ListChangedEventHandler IBindingList.ListChanged
		{
			add { }
			remove { }
		}

		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			throw new NotImplementedException();
		}

		void IBindingList.RemoveSort()
		{
			throw new NotImplementedException();
		}

		ListSortDirection IBindingList.SortDirection
		{
			get { throw new NotImplementedException(); }
		}

		PropertyDescriptor IBindingList.SortProperty
		{
			get { throw new NotImplementedException(); }
		}

		bool IBindingList.SupportsChangeNotification
		{
			get { return false; }
		}

		bool IBindingList.SupportsSearching
		{
			get { return false; }
		}

		bool IBindingList.SupportsSorting
		{
			get { return false; }
		}

		#endregion IBindingList Members

		#region IList Members

		int System.Collections.IList.Add(object value)
		{
			throw new NotImplementedException();
		}

		void System.Collections.IList.Clear()
		{
			throw new NotImplementedException();
		}

		bool System.Collections.IList.Contains(object value)
		{
            //WPF only: <DataGrid AutoGenerateColumns="True" ItemsSource="{Binding DataSource}" />
            return ((DataRow<R, C>)value).RowIndex < RowCount;
		}

		int System.Collections.IList.IndexOf(object value)
		{
			//WPF only: <DataGrid AutoGenerateColumns="True" ItemsSource="{Binding DataSource}" />
            return ((DataRow<R, C>)value).RowIndex;
		}

		void System.Collections.IList.Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		bool System.Collections.IList.IsFixedSize
		{
            //WPF only: ListCollectionView uses it
			get { return true; }
		}

		bool System.Collections.IList.IsReadOnly
		{
			get { return true; }
		}

		void System.Collections.IList.Remove(object value)
		{
			throw new NotImplementedException();
		}

		void System.Collections.IList.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		object System.Collections.IList.this[int index]
		{
			get
			{
				return _Rows[index];
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion IList Members

		#region ICollection Members

		void System.Collections.ICollection.CopyTo(Array array, int index)
		{
			//WPF only: <DataGrid AutoGenerateColumns="True" ItemsSource="{Binding DataSource}" />
            for (int i = 0; i < _Rows.Count; i++)
            {
                array.SetValue(_Rows[i], i + index);
            }
		}

		int System.Collections.ICollection.Count
		{
			get { return _Rows.Count; }
		}

		bool System.Collections.ICollection.IsSynchronized
		{
			get { return false; }
		}

		object System.Collections.ICollection.SyncRoot
		{
			get
			{
				//WPF only: <DataGrid AutoGenerateColumns="True" ItemsSource="{Binding DataSource}" />
				return null;
			}
		}

		#endregion ICollection Members

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _Rows.GetEnumerator();
		}

		#endregion IEnumerable Members

        #region INotifyCollectionChanged - not implemented
        //The following is not needed because the underlying View's Refresh() method does the same thing
        //However, it shows how to extend this DataSource should it be necessary

        //event NotifyCollectionChangedEventHandler _CollectionChanged;
        //readonly NotifyCollectionChangedEventArgs _CollectionResetArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        ////WPF only: <DataGrid AutoGenerateColumns="True" ItemsSource="{Binding DataSource}" />
        ////Needed in v4.5 because WPF now checks that the items source count is always consistent with any notifications
        ////so by not doing any notifications it seems impossible to actually add/remove items without triggering the check.
        //event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        //{
        //    add { _CollectionChanged += value; }
        //    remove { _CollectionChanged -= value; }
        //}

        ///// <summary>
        ///// Needed in v4.5 because WPF now checks that the items source count is always consistent with any notifications
        ///// so by not doing any notifications it seems impossible to actually add/remove items without triggering the check.
        ///// </summary>
        //public void NotifyCollectionReset()
        //{
        //    if (_CollectionChanged != null) _CollectionChanged(this, _CollectionResetArgs);
        //}
        #endregion
    }

	#region Debugger View

	internal class DataSource_DebuggerView<R1, C1>
		where R1 : struct
		where C1 : new()
	{
		private readonly DataSource<R1, C1> _Source;

		public DataSource_DebuggerView(DataSource<R1, C1> source)
		{
			_Source = source;
		}

		public ICollection<DataColumnExpression> Expressions
		{
			get
			{
				var res = new List<DataColumnExpression>(_Source.ExpressionsCount);
				if (_Source._ColumnsWithExpressions != null)
				{
					foreach (var idx in _Source._ColumnsWithExpressions)
					{
                        if (idx >= 0)
                        {
                            var col = _Source.Columns[idx];
                            res.Add(col.Expression);
                        }
					}
                    if (_Source.ExpressionFilter != null) res.Add(_Source.ExpressionFilter);
				}
				return res;
			}
		}

		public DataTable Data
		{
			get
			{
				var res = new DataTable(_Source.GetType().Name);
				foreach (var col in _Source.Columns)
				{
					var name = col.Name;
					var type = col.Type;
					res.Columns.Add(name, type);
				}
				for (int row = 0; row < _Source.RowCount; row++)
				{
					var data = res.NewRow();
					for (int col = 0; col < _Source.ColumnCount; col++)
					{
						data[col] = _Source[row, col] ?? DBNull.Value;
					}
					res.Rows.Add(data);
				}
				return res;
			}
		}
	}

	#endregion Debugger View
}