using com.calitha.goldparser;
using System;
using System.Collections.Generic;
using System.Globalization;
using Linq = System.Linq.Expressions;

namespace CustomColumns.Expressions.Internal
{
    internal static class SymbolRules
    {
        internal static SortedList<SymbolConstants, Func<Parser, TerminalToken, object>> Handlers
            = new SortedList<SymbolConstants, Func<Parser, TerminalToken, object>>(6)
            {
               {SymbolConstants.SYMBOL_STRINGLITERAL, (p,t) => Linq.Expression.Constant(t.Text.Trim('"'))},
               {SymbolConstants.SYMBOL_NUMBERLITERAL, RuleSymbolNumberHandler},
               {SymbolConstants.SYMBOL_ID, (p,t) => t.Text},
            };

        private static object RuleSymbolNumberHandler(Parser p, TerminalToken t)
        {
            //Known to be a valid number or integer already
            //Culture has to be invariant because grammar has '.' hard-coded
            if (t.Text.IndexOf('.') >= 0) return Linq.Expression.Constant(double.Parse(t.Text, CultureInfo.InvariantCulture));
            return Linq.Expression.Constant(int.Parse(t.Text, CultureInfo.InvariantCulture));
        }
    }
}
