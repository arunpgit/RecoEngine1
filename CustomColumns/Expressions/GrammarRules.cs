using com.calitha.goldparser;
using System;
using System.Collections.Generic;
using Linq = System.Linq.Expressions;

namespace CustomColumns.Expressions.Internal
{
    internal static class GrammarRules
    {
        internal static SortedList<RuleConstants, Func<Parser, NonterminalToken, object>> Handlers
            = new SortedList<RuleConstants, Func<Parser, NonterminalToken, object>>(64)
            {
                //<Statements> ::= '=' <Expression>
               {RuleConstants.RULE_STATEMENTS_EQ, (p,t) => t.Tokens[1].UserObject},
                //<Value> ::= '(' <Expression> ')'
               {RuleConstants.RULE_VALUE_LPARAN_RPARAN, (p,t) => t.Tokens[1].UserObject},
               //<Expression> ::= <Expression> '>' <Shift Exp>
               {RuleConstants.RULE_EXPRESSION_GT, RuleExpressionGtHandler},
               //<Expression> ::= <Expression> '<' <Shift Exp>
               {RuleConstants.RULE_EXPRESSION_LT, RuleExpressionLtHandler},
               //<Expression> ::= <Expression> '>=' <Shift Exp>
               {RuleConstants.RULE_EXPRESSION_GTEQ, RuleExpressionGtEqHandler},
               //<Expression> ::= <Expression> '<=' <Shift Exp>
               {RuleConstants.RULE_EXPRESSION_LTEQ, RuleExpressionLtEqHandler},
               //<Expression> ::= <Expression> '=' <Shift Exp>
               {RuleConstants.RULE_EXPRESSION_EQ, RuleExpressionEqHandler},
                //<Expression> ::= <Expression> '<>' <Shift Exp>
               {RuleConstants.RULE_EXPRESSION_LTGT, RuleExpressionLtGtHandler},
                //<Shift Exp> ::= Not <Add Exp>
               {RuleConstants.RULE_SHIFTEXP_NOT, (p,t) => Linq.Expression.IsFalse(
                   Helpers.CType(t.Tokens[1].UserObject, typeof(bool)))},
                //<Shift Exp> ::= <Shift Exp> AndAlso <Add Exp>
               {RuleConstants.RULE_SHIFTEXP_ANDALSO, (p,t) => Linq.Expression.AndAlso(
                   Helpers.CType(t.Tokens[0].UserObject, typeof(bool)),
                   Helpers.CType(t.Tokens[2].UserObject, typeof(bool))) },
                //<Shift Exp> ::= <Shift Exp> OrElse <Add Exp>
               {RuleConstants.RULE_SHIFTEXP_ORELSE, (p,t) => Linq.Expression.OrElse(
                   Helpers.CType(t.Tokens[0].UserObject, typeof(bool)),
                   Helpers.CType(t.Tokens[2].UserObject, typeof(bool))) },
                //<Add Exp> ::= <Add Exp> '+' <Mult Exp>
               {RuleConstants.RULE_ADDEXP_PLUS, RuleExpressionPlusHandler},
                //<Add Exp> ::= <Add Exp> '-' <Mult Exp>
               {RuleConstants.RULE_ADDEXP_MINUS, RuleExpressionMinusHandler},
                //<Add Exp> ::= <Add Exp> '&' <Mult Exp>
               {RuleConstants.RULE_ADDEXP_AMP, RuleExpressionAmpHandler},
                //<Add Exp> ::= <Add Exp> Mod <Mult Exp>
               {RuleConstants.RULE_ADDEXP_MOD, RuleExpressionModHandler},
                //<Mult Exp> ::= <Mult Exp> '*' <Negate Exp>
               {RuleConstants.RULE_MULTEXP_TIMES, RuleExpressionTimesHandler},
                //<Mult Exp> ::= <Mult Exp> '/' <Negate Exp>
               {RuleConstants.RULE_MULTEXP_DIV, RuleExpressionDivHandler},
                //<Mult Exp> ::= <Mult Exp> '\' <Negate Exp>
               {RuleConstants.RULE_MULTEXP_BACKSLASH, RuleExpressionRemainderHandler},
                //<Mult Exp> ::= <Mult Exp> '^' <Negate Exp>
               {RuleConstants.RULE_MULTEXP_CARET, RuleExpressionPowHandler},
                //<Negate Exp> ::= '-' <Function>
               {RuleConstants.RULE_NEGATEEXP_MINUS, (p,t)=>Linq.Expression.Negate((Linq.Expression)t.Tokens[1].UserObject)},
                //<FId> ::= <FId> '.' Id
               {RuleConstants.RULE_FID_DOT_ID, (p,t)=>(string)t.Tokens[0].UserObject+'.'+(string)t.Tokens[2].UserObject},
                //<Function> ::= <FId> '()'
               {RuleConstants.RULE_FUNCTION_LPARANRPARAN, RuleExpressionFunctionHandler},
                //<Function> ::= <FId> '(' <Arg> ')'
               {RuleConstants.RULE_FUNCTION_LPARAN_RPARAN, RuleExpressionFunctionHandler},
                //<FieldFunction> ::= Id '()'
               {RuleConstants.RULE_FIELDFUNCTION_ID_LPARANRPARAN, RuleExpressionFunctionHandler},
                //<FieldFunction> ::= Id '(' <Arg> ')'
               {RuleConstants.RULE_FIELDFUNCTION_ID_LPARAN_RPARAN, RuleExpressionFunctionHandler},
                //<Arg> ::= <Arg>
               {RuleConstants.RULE_ARG, RuleExpressionArgHandler},
                //<Arg> ::= <Arg> ',' <Expression>
               {RuleConstants.RULE_ARG_COMMA, RuleExpressionArgHandler},
                //<Field> ::= Id '!' Id
               {RuleConstants.RULE_FIELD_ID_EXCLAM_ID, RuleExpressionFieldHandler},
                //<Value> ::= <Field> '.' <FieldFunction>
               {RuleConstants.RULE_VALUE_DOT, RuleExpressionFieldFunctionHandler},

            };

        static object RuleExpressionFieldHandler(Parser p, NonterminalToken t)
        {
            //<Field> ::= Id '!' Id
            var field = (string)t.Tokens[0].UserObject + '.' + (string)t.Tokens[2].UserObject;
            Type fieldType;
            if (!p.AvailableFields.TryGetValue(field, out fieldType)) 
                throw new ArgumentException("Unknown field: " + field.Replace('.', '!'));
            
            int idx;
            if (!p.FieldsNeeded.TryGetValue(field, out idx))
            {
                idx = p.FieldsNeeded.Count;
                p.FieldsNeeded.Add(field, idx);
            }

            var asObject = Linq.Expression.ArrayAccess(p.ArrayParameter, Linq.Expression.Constant(idx));
            var cast = Linq.Expression.Convert(asObject, fieldType);
            return cast;
        }
        static object RuleExpressionArgHandler(Parser p, NonterminalToken t)
        {
            //<Arg> ::= <Arg>
            //<Arg> ::= <Arg> ',' <Expression>
            var list = t.Tokens[0].UserObject as List<Linq.Expression>;
            if (list == null) list = new List<Linq.Expression>() { (Linq.Expression)t.Tokens[0].UserObject };
            if (t.Tokens.Length > 1) list.Add((Linq.Expression)t.Tokens[2].UserObject);
            return list;
        }
        static object RuleExpressionFunctionHandler(Parser p, NonterminalToken t)
        {
            //<Function> ::= <FId> '()'
            //<Function> ::= Id '()'
            //<Function> ::= <FId> '(' <Arg> ')'
            //<Function> ::= Id '(' <Arg> ')'
            var fn = (string)t.Tokens[0].UserObject;
            List<Linq.Expression> args;
            if (t.Tokens.Length == 2) args = null;
            else
            {
                args = t.Tokens[2].UserObject as List<Linq.Expression>;
                if (args == null)
                {
                    args = new List<Linq.Expression>()
                    {
                        (Linq.Expression)t.Tokens[2].UserObject
                    };
                }
            }

            return Functions.Process(fn, args);
        }
        static object RuleExpressionFieldFunctionHandler(Parser p, NonterminalToken t)
        {
            //<Value> ::= <Field> '.' <FieldFunction>
            var fieldIndex = (int)t.Tokens[0].UserObject;

            throw new NotImplementedException("Not implemented yet");
            //TODO: Confirm function applies to field/type
            //E.g. You could do Field.ToString() for example
            //But error handling and verification are a little tricky
        }
        static object RuleExpressionGtHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.GreaterThan(r[0], r[1]);
        }
        static object RuleExpressionLtHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.LessThan(r[0], r[1]);
        }
        static object RuleExpressionGtEqHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.GreaterThanOrEqual(r[0], r[1]);
        }
        static object RuleExpressionLtEqHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.LessThanOrEqual(r[0], r[1]);
        }
        static object RuleExpressionEqHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.Equal(r[0], r[1]);
        }
        static object RuleExpressionLtGtHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.NotEqual(r[0], r[1]);
        }
        static object RuleExpressionPlusHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject, true);
            if (r[0].Type != typeof(string)) return Linq.Expression.Add(r[0], r[1]);
            return Helpers.GenerateStaticMethodCall("Concat", r[0], r[1]);
        }
        static object RuleExpressionMinusHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.Subtract(r[0], r[1]);
        }
        static object RuleExpressionAmpHandler(Parser p, NonterminalToken t)
        {
            return Helpers.GenerateStaticMethodCall("Concat",
                Helpers.CStr(t.Tokens[0].UserObject),
                Helpers.CStr(t.Tokens[2].UserObject)
                );
        }
        static object RuleExpressionModHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.Modulo(r[0], r[1]);
        }
        static object RuleExpressionTimesHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.Multiply(r[0], r[1]);
        }
        static object RuleExpressionDivHandler(Parser p, NonterminalToken t)
        {
            var r = Helpers.Promote(t.Tokens[0].UserObject, t.Tokens[2].UserObject);
            return Linq.Expression.Divide(r[0], r[1]);
        }
        static object RuleExpressionRemainderHandler(Parser p, NonterminalToken t)
        {
            return Linq.Expression.Divide(
                Helpers.CType(t.Tokens[0].UserObject, typeof(int)),
                Helpers.CType(t.Tokens[2].UserObject, typeof(int))
                );
        }
        static object RuleExpressionPowHandler(Parser p, NonterminalToken t)
        {
            return Linq.Expression.Power(
                Helpers.CType(t.Tokens[0].UserObject, typeof(double)),
                Helpers.CType(t.Tokens[2].UserObject, typeof(double))
                );
        }
    }
}
