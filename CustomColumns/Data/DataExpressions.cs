using CustomColumns.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CustomColumns.Data
{
	/// <summary>
	/// The expression associated with a column
	/// </summary>
	[DebuggerDisplay("{Expression}, {Error}")]
	public class DataColumnExpression
	{
		/// <summary>
		/// The user-entered formula, starting with '='
		/// </summary>
		public readonly string Expression;

		/// <summary>
		/// The fixed Error that occurred during Parsing or Compilation
		/// (Check this first)
		/// </summary>
		public string Error { get; internal set; }

		/// <summary>
		/// The friendly version of the parsed Expression
		/// </summary>
		public string Description { get; internal set; }

		/// <summary>
		/// Non-null only if formula needs other columns
		/// </summary>
		public readonly ISet<int> ColumnsNeeded;

		/// <summary>
		/// The order of the columns needed or null
		/// </summary>
		public readonly int[] DelegateArguments;

		/// <summary>
		/// The compiled delegate - may be null if in error or derived class doesn't use it
		/// </summary>
        public Func<object[], object> CompiledDelegate { get; protected set; }

		/// <summary>
		/// How many row(s) were in error during last recalculation
		/// </summary>
		public int LastErrorCount { get; internal set; }

		//Internal use only
		internal bool HasNeverBeenCalculated;

		internal DataColumnExpression(Parser parser, string expression,
			IDictionary<string, Type> fields, Func<string, int> nameToIndexMapper)
		{
			Expression = expression;
			//Requires subsequent recalculation
			HasNeverBeenCalculated = true;
            if (Parse(parser, expression, fields))
			{
				Description = parser.Description;
				if (parser.FieldsNeeded.Count > 0)
				{
					//Cache columns and arguments using the immutable column indicies
                    ColumnsNeeded = new HashSet<int>();
					foreach (var pair in parser.FieldsNeeded)
					{
						ColumnsNeeded.Add(nameToIndexMapper(pair.Key));
					}
					DelegateArguments = new int[parser.FieldsNeeded.Count];
					foreach (var pair in parser.FieldsNeeded)
						DelegateArguments[pair.Value] = nameToIndexMapper(pair.Key);
				}
			}
			else
			{
				Error = string.Join("\n", parser.Errors);
			}
		}

        /// <summary>
        /// Derived: must call a variant of parser.Parse(expression, fields) and set CompiledDelegate
        /// </summary>
        protected virtual bool Parse(Parser parser, string expression, IDictionary<string, Type> fields)
        {
            if (parser.Parse(expression, fields))
            {
                CompiledDelegate = parser.CompiledDelegate;
                return true;
            }
            return false;
        }
	}

    /// <summary>
    /// The expression associated with a boolean value like a column filter
    /// </summary>
    public class DataBooleanExpression : DataColumnExpression
    {
        /// <summary>
		/// The compiled delegate - may be null if in error
        /// </summary>
        public new Func<object[], bool> CompiledDelegate { get; protected set; }

        internal DataBooleanExpression(Parser parser, string expression,
            IDictionary<string, Type> fields, Func<string, int> nameToIndexMapper)
            : base(parser, expression, fields, nameToIndexMapper)
        {
        }

        protected override bool Parse(Parser parser, string expression, IDictionary<string, Type> fields)
        {
            CompiledDelegate = parser.ParseTyped<bool>(expression, fields);
            return CompiledDelegate != null;
        }
    }
}
