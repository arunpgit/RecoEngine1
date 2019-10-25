using System;
using System.Diagnostics;

namespace CustomColumns.Data
{
	/// <summary>
	/// Represents an entire column within a DataSource
	/// </summary>
	/// <typeparam name="C">Match that used on DataSource</typeparam>
	public class DataColumn<C>
		where C : new()
	{
		public const int MinimumRowCapacity = 10;
		const int ReallocChunkSize = 10;

		object[] _Rows;
		int _RowCount;

		/// <summary>
		/// Gets the index of the column for convenience during enumeration
		/// </summary>
		public int Index { get; internal set; }

		/// <summary>
		/// Gets the name of the column
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the type of the column
		/// </summary>
		/// <value>The type.</value>
		public Type Type { get; private set; }

		/// <summary>
		/// The expression, if this column has one
        /// Use DataSource.ResetExpressions() to make this
		/// </summary>
		public DataColumnExpression Expression;

		/// <summary>
		/// Optional metadata to describe the DataColumn
		/// </summary>
		public C MetaData;

		/// <summary>
		/// Gets the object at the specific row index
		/// Sets the object at the specific row index 
		/// Debug only: Set has type checking
		/// </summary>
		/// <value></value>
		public object this[int idx]
		{
			get { return _Rows[idx]; }
			set
			{
				Debug.Assert(value == null || Type == value.GetType(), "Type mismatch: expected column type is: " + Type.Name);
				_Rows[idx] = value;
			}
		}

		internal void SetTypeAndClear(string name, Type newType)
		{
			Name = name;
			Type = newType;
			MetaData = default(C);
			Expression = null;
			Array.Clear(_Rows, 0, _Rows.Length);
		}

		internal void ResetTypeOnly(Type newType)
		{
			Type = newType;
			Expression = null;
		}

		/// <summary>
		/// Used to clear down the values without resetting the type
		/// </summary>
		public void ClearValues()
		{
			Array.Clear(_Rows, 0, _Rows.Length);
		}

        /// <summary>
        /// Advanced: provides access to the raw object array for things like fast sorting
        /// NOTE: The length may be bigger than the public RowCount
        /// </summary>
        public static explicit operator object[](DataColumn<C> rhs)
        {
            return rhs._Rows;
        }

		internal void SetConstantValue(object value, int startingIdx = 0)
		{
			Debug.Assert(value == null || Type == value.GetType(), "Type mismatch: expected column type is: " + Type.Name);
			if (value == null) Array.Clear(_Rows, startingIdx, _Rows.Length - startingIdx);
			else for (int i = startingIdx; i < _Rows.Length; i++) _Rows[i] = value;
		}

		internal DataColumn(int index, int rowCount, string name, Type type)
		{
			Index = index;
			Name = name;
			Type = type;
			_RowCount = rowCount;
			_Rows = new object[rowCount < MinimumRowCapacity ? MinimumRowCapacity : rowCount];
		}

		internal void AddNewRows(int rowsToAdd)
		{
			var free = _Rows.Length - _RowCount;
			if (free < rowsToAdd)
			{
				var diff = (int)Math.Ceiling((double)rowsToAdd / (double)ReallocChunkSize) * ReallocChunkSize;
				//Allocate increase in range [ReallocChunkSize, ReallocChunkSize*2]
				diff += ReallocChunkSize;
				var increased = new object[diff + _Rows.Length];
				Array.Copy(_Rows, 0, increased, 0, _RowCount);
				_Rows = increased;
			}
			_RowCount += rowsToAdd;
		}

		internal void ReserveCapacity(int estimatedTotalRows)
		{
			Debug.Assert(estimatedTotalRows > _RowCount);
			if (_Rows.Length >= estimatedTotalRows) return;
			var increased = new object[estimatedTotalRows];
			Array.Copy(_Rows, 0, increased, 0, _RowCount);
			_Rows = increased;
		}

		internal void TrimExcessRows(int newRowCount)
		{
			Debug.Assert(newRowCount < _RowCount, "Trimming must reduce the row count");
			Debug.Assert(newRowCount < _Rows.Length);

			if (newRowCount != _Rows.Length)
			{
				var reduced = new object[newRowCount];
				Array.Copy(_Rows, 0, reduced, 0, newRowCount);
				_Rows = reduced;
			}
			_RowCount = newRowCount;
		}

		internal void DeleteRowBySwappingWithLast(int rowIndexToDelete)
		{
			Debug.Assert(rowIndexToDelete >= 0 && rowIndexToDelete < _RowCount, "Out of range");
			//If not last row, swap first
			var lastRowIndex = _RowCount - 1;
			if (rowIndexToDelete < lastRowIndex)
			{
				_Rows[rowIndexToDelete] = _Rows[lastRowIndex];
			}
			//Clear row in case it gets re-used but also to free all the object references
			_Rows[lastRowIndex] = null;
			//And delete last row
			_RowCount--;
		}
	}
}
