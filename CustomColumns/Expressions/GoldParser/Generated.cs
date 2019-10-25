enum SymbolConstants : int
{
    
   /// <c> (EOF) </c>
   SYMBOL_EOF           = 0,     
   /// <c> (Error) </c>
   SYMBOL_ERROR         = 1,     
   /// <c> (Whitespace) </c>
   SYMBOL_WHITESPACE    = 2,     
   /// <c> '-' </c>
   SYMBOL_MINUS         = 3,     
   /// <c> '!' </c>
   SYMBOL_EXCLAM        = 4,     
   /// <c> '&amp;' </c>
   SYMBOL_AMP           = 5,     
   /// <c> '(' </c>
   SYMBOL_LPARAN        = 6,     
   /// <c> '()' </c>
   SYMBOL_LPARANRPARAN  = 7,     
   /// <c> ')' </c>
   SYMBOL_RPARAN        = 8,     
   /// <c> '*' </c>
   SYMBOL_TIMES         = 9,     
   /// <c> ',' </c>
   SYMBOL_COMMA         = 10,     
   /// <c> '.' </c>
   SYMBOL_DOT           = 11,     
   /// <c> '/' </c>
   SYMBOL_DIV           = 12,     
   /// <c> '\' </c>
   SYMBOL_BACKSLASH     = 13,     
   /// <c> '^' </c>
   SYMBOL_CARET         = 14,     
   /// <c> '+' </c>
   SYMBOL_PLUS          = 15,     
   /// <c> '&lt;' </c>
   SYMBOL_LT            = 16,     
   /// <c> '&lt;=' </c>
   SYMBOL_LTEQ          = 17,     
   /// <c> '&lt;&gt;' </c>
   SYMBOL_LTGT          = 18,     
   /// <c> '=' </c>
   SYMBOL_EQ            = 19,     
   /// <c> '&gt;' </c>
   SYMBOL_GT            = 20,     
   /// <c> '&gt;=' </c>
   SYMBOL_GTEQ          = 21,     
   /// <c> AndAlso </c>
   SYMBOL_ANDALSO       = 22,     
   /// <c> Id </c>
   SYMBOL_ID            = 23,     
   /// <c> Like </c>
   SYMBOL_LIKE          = 24,     
   /// <c> Mod </c>
   SYMBOL_MOD           = 25,     
   /// <c> Not </c>
   SYMBOL_NOT           = 26,     
   /// <c> NumberLiteral </c>
   SYMBOL_NUMBERLITERAL = 27,     
   /// <c> OrElse </c>
   SYMBOL_ORELSE        = 28,     
   /// <c> StringLiteral </c>
   SYMBOL_STRINGLITERAL = 29,     
   /// <c> &lt;Add Exp&gt; </c>
   SYMBOL_ADDEXP        = 30,     
   /// <c> &lt;Arg&gt; </c>
   SYMBOL_ARG           = 31,     
   /// <c> &lt;Expression&gt; </c>
   SYMBOL_EXPRESSION    = 32,     
   /// <c> &lt;FId&gt; </c>
   SYMBOL_FID           = 33,     
   /// <c> &lt;Field&gt; </c>
   SYMBOL_FIELD         = 34,     
   /// <c> &lt;FieldFunction&gt; </c>
   SYMBOL_FIELDFUNCTION = 35,     
   /// <c> &lt;Function&gt; </c>
   SYMBOL_FUNCTION      = 36,     
   /// <c> &lt;Mult Exp&gt; </c>
   SYMBOL_MULTEXP       = 37,     
   /// <c> &lt;Negate Exp&gt; </c>
   SYMBOL_NEGATEEXP     = 38,     
   /// <c> &lt;Shift Exp&gt; </c>
   SYMBOL_SHIFTEXP      = 39,     
   /// <c> &lt;Statements&gt; </c>
   SYMBOL_STATEMENTS    = 40,     
   /// <c> &lt;Value&gt; </c>
   SYMBOL_VALUE         = 41      
};

enum RuleConstants : int
{
   /// <c> &lt;Statements&gt; ::= '=' &lt;Expression&gt; </c>
   RULE_STATEMENTS_EQ                  = 0,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&gt;' &lt;Shift Exp&gt; </c>
   RULE_EXPRESSION_GT                  = 1,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&lt;' &lt;Shift Exp&gt; </c>
   RULE_EXPRESSION_LT                  = 2,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&lt;=' &lt;Shift Exp&gt; </c>
   RULE_EXPRESSION_LTEQ                = 3,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&gt;=' &lt;Shift Exp&gt; </c>
   RULE_EXPRESSION_GTEQ                = 4,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '=' &lt;Shift Exp&gt; </c>
   RULE_EXPRESSION_EQ                  = 5,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; Like &lt;Shift Exp&gt; </c>
   RULE_EXPRESSION_LIKE                = 6,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&lt;&gt;' &lt;Shift Exp&gt; </c>
   RULE_EXPRESSION_LTGT                = 7,    
   /// <c> &lt;Expression&gt; ::= &lt;Shift Exp&gt; </c>
   RULE_EXPRESSION                     = 8,    
   /// <c> &lt;Shift Exp&gt; ::= Not &lt;Add Exp&gt; </c>
   RULE_SHIFTEXP_NOT                   = 9,    
   /// <c> &lt;Shift Exp&gt; ::= &lt;Shift Exp&gt; AndAlso &lt;Add Exp&gt; </c>
   RULE_SHIFTEXP_ANDALSO               = 10,    
   /// <c> &lt;Shift Exp&gt; ::= &lt;Shift Exp&gt; OrElse &lt;Add Exp&gt; </c>
   RULE_SHIFTEXP_ORELSE                = 11,    
   /// <c> &lt;Shift Exp&gt; ::= &lt;Add Exp&gt; </c>
   RULE_SHIFTEXP                       = 12,    
   /// <c> &lt;Add Exp&gt; ::= &lt;Add Exp&gt; '+' &lt;Mult Exp&gt; </c>
   RULE_ADDEXP_PLUS                    = 13,    
   /// <c> &lt;Add Exp&gt; ::= &lt;Add Exp&gt; '-' &lt;Mult Exp&gt; </c>
   RULE_ADDEXP_MINUS                   = 14,    
   /// <c> &lt;Add Exp&gt; ::= &lt;Add Exp&gt; '&amp;' &lt;Mult Exp&gt; </c>
   RULE_ADDEXP_AMP                     = 15,    
   /// <c> &lt;Add Exp&gt; ::= &lt;Add Exp&gt; Mod &lt;Mult Exp&gt; </c>
   RULE_ADDEXP_MOD                     = 16,    
   /// <c> &lt;Add Exp&gt; ::= &lt;Mult Exp&gt; </c>
   RULE_ADDEXP                         = 17,    
   /// <c> &lt;Mult Exp&gt; ::= &lt;Mult Exp&gt; '*' &lt;Negate Exp&gt; </c>
   RULE_MULTEXP_TIMES                  = 18,    
   /// <c> &lt;Mult Exp&gt; ::= &lt;Mult Exp&gt; '/' &lt;Negate Exp&gt; </c>
   RULE_MULTEXP_DIV                    = 19,    
   /// <c> &lt;Mult Exp&gt; ::= &lt;Mult Exp&gt; '\' &lt;Negate Exp&gt; </c>
   RULE_MULTEXP_BACKSLASH              = 20,    
   /// <c> &lt;Mult Exp&gt; ::= &lt;Mult Exp&gt; '^' &lt;Negate Exp&gt; </c>
   RULE_MULTEXP_CARET                  = 21,    
   /// <c> &lt;Mult Exp&gt; ::= &lt;Negate Exp&gt; </c>
   RULE_MULTEXP                        = 22,    
   /// <c> &lt;Negate Exp&gt; ::= '-' &lt;Function&gt; </c>
   RULE_NEGATEEXP_MINUS                = 23,    
   /// <c> &lt;Negate Exp&gt; ::= &lt;Function&gt; </c>
   RULE_NEGATEEXP                      = 24,    
   /// <c> &lt;FId&gt; ::= &lt;FId&gt; '.' Id </c>
   RULE_FID_DOT_ID                     = 25,    
   /// <c> &lt;FId&gt; ::= Id </c>
   RULE_FID_ID                         = 26,    
   /// <c> &lt;Function&gt; ::= &lt;FId&gt; '()' </c>
   RULE_FUNCTION_LPARANRPARAN          = 27,    
   /// <c> &lt;Function&gt; ::= &lt;FId&gt; '(' &lt;Arg&gt; ')' </c>
   RULE_FUNCTION_LPARAN_RPARAN         = 28,    
   /// <c> &lt;Function&gt; ::= &lt;Value&gt; </c>
   RULE_FUNCTION                       = 29,    
   /// <c> &lt;FieldFunction&gt; ::= Id </c>
   RULE_FIELDFUNCTION_ID               = 30,    
   /// <c> &lt;FieldFunction&gt; ::= Id '()' </c>
   RULE_FIELDFUNCTION_ID_LPARANRPARAN  = 31,    
   /// <c> &lt;FieldFunction&gt; ::= Id '(' &lt;Arg&gt; ')' </c>
   RULE_FIELDFUNCTION_ID_LPARAN_RPARAN = 32,    
   /// <c> &lt;Arg&gt; ::= &lt;Arg&gt; ',' &lt;Expression&gt; </c>
   RULE_ARG_COMMA                      = 33,    
   /// <c> &lt;Arg&gt; ::= &lt;Expression&gt; </c>
   RULE_ARG                            = 34,    
   /// <c> &lt;Field&gt; ::= Id '!' Id </c>
   RULE_FIELD_ID_EXCLAM_ID             = 35,    
   /// <c> &lt;Value&gt; ::= &lt;Field&gt; </c>
   RULE_VALUE                          = 36,    
   /// <c> &lt;Value&gt; ::= &lt;Field&gt; '.' &lt;FieldFunction&gt; </c>
   RULE_VALUE_DOT                      = 37,    
   /// <c> &lt;Value&gt; ::= StringLiteral </c>
   RULE_VALUE_STRINGLITERAL            = 38,    
   /// <c> &lt;Value&gt; ::= NumberLiteral </c>
   RULE_VALUE_NUMBERLITERAL            = 39,    
   /// <c> &lt;Value&gt; ::= '(' &lt;Expression&gt; ')' </c>
   RULE_VALUE_LPARAN_RPARAN            = 40     
};
