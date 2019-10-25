using com.calitha.goldparser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Linq = System.Linq.Expressions;

namespace CustomColumns.Expressions
{
	/// <summary>
	/// Parser: only one needed per thread
	/// </summary>
	public class Parser
	{
        /// <summary>
        /// Any errors: never null
        /// </summary>
		public readonly List<string> Errors;
        /// <summary>
        /// The fields needed as a map between name and offset
        /// </summary>
		public readonly SortedList<string, int> FieldsNeeded;
		
		readonly LALRParser _Parser;
		internal readonly Linq.ParameterExpression ArrayParameter;
		internal IDictionary<string, Type> AvailableFields { get; private set; }

        /// <summary>
        /// The final compiled delegate
        /// </summary>
		public Func<object[], object> CompiledDelegate { get; internal set; }
        /// <summary>
        /// Debugging description of expression
        /// </summary>
		public string Description { get; internal set; }
        /// <summary>
        /// The final result of the compiled delegate
        /// </summary>
		public Type ResultType { get; internal set; }

		/// <summary>
		/// Only one needed per thread
		/// </summary>
		public Parser()
		{
			Errors = new List<string>();
			FieldsNeeded = new SortedList<string, int>(32);
			ArrayParameter = Linq.Expression.Parameter(typeof(object[]), "Fields");

			_Parser = _Reader.CreateNewParser();
			_Parser.TrimReductions = true;
			_Parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;
			_Parser.OnReduce += _Parser_OnReduce;
			_Parser.OnTokenRead += _Parser_OnTokenRead;
			_Parser.OnTokenError += _Parser_OnTokenError;
			_Parser.OnParseError += _Parser_OnParseError;
		}

		void Reset(IDictionary<string, Type> fields)
		{
			AvailableFields = fields;
			Description = null;
			CompiledDelegate = null;
			ResultType = null;
			Errors.Clear();
			FieldsNeeded.Clear();
		}

		/// <summary>
		/// Parses the given input expression allowing the available fields.
        /// Sets Errors, ResultType, Description and CompiledDelegate properties depending on success/failure (true/false return result)
		/// </summary>
		public bool Parse(string input, IDictionary<string, Type> availableFields)
		{
            var expression = _Parse(input, availableFields);
            if (expression == null) return false;

			//Box if necessary
			if (expression.Type.IsValueType) expression = Linq.Expression.Convert(expression, typeof(object));
			CompiledDelegate = (Func<object[], object>)Linq.Expression.Lambda(expression, ArrayParameter).Compile();
			return true;
		}

        /// <summary>
        /// Advanced: see Parse() for the more common boxed-result.
        /// Parses the given input expression allowing the available fields.
        /// Sets Errors, ResultType, Description and CompiledDelegate properties depending on success/failure (true/false return result)
        /// </summary>
        public Func<object[], T> ParseTyped<T>(string input, IDictionary<string, Type> availableFields)
        {
            var expression = _Parse(input, availableFields);
            if (expression == null) return null;

            if (expression.Type != typeof(T))
            {
                Errors.Add("Result type expected to be: " + typeof(T).Name);
                return null;
            }

            CompiledDelegate = null;
            return (Func<object[], T>)Linq.Expression.Lambda(expression, ArrayParameter).Compile();
        }

        Linq.Expression _Parse(string input, IDictionary<string, Type> availableFields)
        {
            Reset(availableFields);

            var token = _Parser.Parse(input);
            Debug.Assert(token != null || Errors.Count > 0);

            //Failed
            if (token == null) return null;

            var expression = (Linq.Expression)token.UserObject;
            ResultType = expression.Type;
            Description = expression.ToString();
            return expression;
        }

        static readonly MethodInfo _CompareOrdinal = typeof(string).GetMethod("CompareOrdinal", new[] { typeof(string), typeof(string) });
        
        /// <summary>
        /// Creates a row comparer for one or more columns 
        /// </summary>
        /// <param name="columns">One type for each column, with true for asc and false for desc sort order</param>
        /// <returns></returns>
        public Func<object[][], int, int, int> CreateComparer(params KeyValuePair<Type, bool>[] columns)
        {
            if (columns == null || columns.Length == 0) throw new ArgumentNullException("columns");
            var arg0 = Linq.Expression.Parameter(typeof(object[][]), "columns");
            var arg1 = Linq.Expression.Parameter(typeof(int), "firstRowIdx");
            var arg2 = Linq.Expression.Parameter(typeof(int), "secondRowIdx");
            var nullConstant = Linq.Expression.Constant(null);
            var equalConstant = Linq.Expression.Constant(0);
            var returnLabel = Linq.Expression.Label(typeof(int), "Result");

            var returnGreater = Linq.Expression.Return(returnLabel, Linq.Expression.Constant(1), returnLabel.Type);
            var returnLess = Linq.Expression.Return(returnLabel, Linq.Expression.Constant(-1), returnLabel.Type);
            
            //Gotos to move to next comparison block on equality
            var labels = new Linq.GotoExpression[columns.Length + 1];
            for (int i=0; i<columns.Length; i++)
            {
                labels[i] = Linq.Expression.Goto(Linq.Expression.Label("Column" + i));
            }
            //Return equality
            labels[labels.Length - 1] = Linq.Expression.Return(returnLabel, equalConstant, returnLabel.Type);

            var comparisons = new Linq.Expression[columns.Length + 1];
            for (int i = 0; i < columns.Length; i++)
            {
                var type = columns[i].Key;
                var isAscending = columns[i].Value;

                var thisLabel = labels[i].Target;
                var valuesAreEqual = labels[i + 1];

                var column = Linq.Expression.ArrayAccess(arg0, Linq.Expression.Constant(i));
                var firstValue = Linq.Expression.ArrayAccess(column, arg1);
                var secondValue = Linq.Expression.ArrayAccess(column, arg2);
                var typedFirstValue = type.IsValueType ? Linq.Expression.Unbox(firstValue, type) : Linq.Expression.Convert(firstValue, typeof(string));
                var typedSecondValue = type.IsValueType ? Linq.Expression.Unbox(secondValue, type) : Linq.Expression.Convert(secondValue, typeof(string));

                var firstValueIsNull = Linq.Expression.ReferenceEqual(firstValue, nullConstant);
                var secondValueIsNull = Linq.Expression.ReferenceEqual(secondValue, nullConstant);
                var bothValuesAreSameInstance = Linq.Expression.ReferenceEqual(firstValue, secondValue);

                var returnFirstIsLess = isAscending ? returnLess : returnGreater;
                var returnFirstIsGreater = isAscending ? returnGreater : returnLess;

                Linq.Expression typedFinalCheck;
                if (type.IsValueType)
                {
                    //Value types have less than operator
                    typedFinalCheck = Linq.Expression.IfThenElse(
                        Linq.Expression.LessThan(typedFirstValue, typedSecondValue),
                        returnFirstIsLess,
                        returnFirstIsGreater);
                }
                else
                {
                    //Strings require one of the static string.CompareXXX() calls
                    Linq.Expression stringCompare = Linq.Expression.Call(_CompareOrdinal, typedFirstValue, typedSecondValue);
                    if (!isAscending) stringCompare = Linq.Expression.Negate(stringCompare);
                    typedFinalCheck = Linq.Expression.Return(returnLabel, stringCompare);
                }

                var block = Linq.Expression.Block(
                    Linq.Expression.Label(thisLabel),
                    //if both null or same string instance then are equal so try next column (or return equal)
                    Linq.Expression.IfThen(bothValuesAreSameInstance, valuesAreEqual),
                    //if first is null then less
                    Linq.Expression.IfThen(firstValueIsNull, returnFirstIsLess),
                    //if second is null then greater
                    Linq.Expression.IfThen(secondValueIsNull, Linq.Expression.Return(returnLabel, returnFirstIsGreater)),
                    //if unboxed values are equal try next column (or return equal)
                    Linq.Expression.IfThen(Linq.Expression.Equal(typedFirstValue, typedSecondValue), valuesAreEqual),
                    //final typed check
                    typedFinalCheck
                    );

                comparisons[i] = block;
            }
            
            //Put in return label
            comparisons[comparisons.Length - 1] = Linq.Expression.Label(returnLabel, equalConstant);

            //And form the final expression
            var res = Linq.Expression.Lambda(Linq.Expression.Block(comparisons), arg0, arg1, arg2);

            return (Func<object[][], int, int, int>)res.Compile();
        }

		#region Internal parsing events
		void _Parser_OnParseError(LALRParser parser, ParseErrorEventArgs args)
		{
			Errors.Add("Parse error: '" + args.UnexpectedToken.ToString() + "'");
			args.Continue = ContinueMode.Stop;
		}

		void _Parser_OnTokenError(LALRParser parser, TokenErrorEventArgs args)
		{
			Errors.Add("Token error: '" + args.Token.ToString() + "'");
			args.Continue = false;
		}

		void _Parser_OnTokenRead(LALRParser parser, TokenReadEventArgs args)
		{
			var token = args.Token;
			Func<Parser, TerminalToken, object> handler;
			if (Internal.SymbolRules.Handlers.TryGetValue((SymbolConstants)token.Symbol.Id, out handler))
			{
				try
				{
					token.UserObject = handler(this, token);
				}
				catch (Exception ex)
				{
					Errors.Add(string.Format("{0}: {1}", args.Token.Symbol, ex.Message));
					args.Continue = false;
				}
			}
		}

		void _Parser_OnReduce(LALRParser parser, ReduceEventArgs args)
		{
			Func<Parser, NonterminalToken, object> handler;
			if (Internal.GrammarRules.Handlers.TryGetValue((RuleConstants)args.Rule.Id, out handler))
			{
				try
				{
					args.Token.UserObject = handler(this, args.Token);
				}
				catch (Exception ex)
				{
					Errors.Add(string.Format("{0}: {1}", args.Rule.ToString(), ex.Message));
					args.Continue = false;
				}
				return;
			}
			//This may happen for things that are not implemented yet because they 
			//"unbalance" the AST
			if (args.Token.Tokens.Length != 1)
			{
				Errors.Add(string.Format("{0}: Internal problem", args.Rule.ToString()));
				args.Continue = false;
				return;
			}
			args.Token.UserObject = args.Token.Tokens[0].UserObject;
		}


		private readonly static CGTReader _Reader;

		static Parser()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Parser), "GoldParser.Generated.cgt");
			_Reader = new CGTReader(stream);
		}
		#endregion
	}
}
