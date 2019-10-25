using System;
using System.Collections.Generic;
using System.Linq;
using Linq = System.Linq.Expressions;

namespace CustomColumns.Expressions.Internal
{
	public class Functions
	{
		public static SortedList<string, Func<List<Linq.Expression>, object>> Handlers
			= new SortedList<string, Func<List<Linq.Expression>, object>>(64)
            {
               //Math
               {"Math.Abs", a => MathHandler("Abs", a)},
               {"Math.Ceiling", a => MathHandler("Ceiling", a)},
               {"Math.Floor", a => MathHandler("Floor", a)},
               {"Math.Exp", a => MathHandler("Exp", a)},
               {"Math.Log", a => MathHandler("Log", a)},
               {"Math.Log10", a => MathHandler("Log10", a)},
               {"Math.Sign", a => MathHandler("Sign", a)},
               {"Math.Sqrt", a => MathHandler("Sqrt", a)},
               {"Math.Truncate", a => MathHandler("Truncate", a)},
               {"Math.Pow", MathPowHandler},
               {"Math.Round", MathRoundHandler},
               //CType
               {"CStr", CStrHandler},
               {"CInt", CIntHandler},
               {"CDbl", CDblHandler},
               {"CBool", CBoolHandler},
               {"CDate", CDateHandler},
               //Formatting
               {"Format", FormatHandler},
               //Logic
               {"IIf", IIfHandler},
               {"Choose", ChooseHandler},
			   {"IsNull", IsNullHandler},
			   //Constants
			   {"Today", TodayHandler},
			   //String
			   {"Substring", SubstringHandler},
			   {"StringLength", StringLengthHandler}
            };

		static object TodayHandler(List<Linq.Expression> a)
		{
			if (a != null) throw new ArgumentException("Today expects no arguments");
			return Linq.Expression.Constant(DateTime.Now.Date);
		}

		static object SubstringHandler(List<Linq.Expression> a)
		{
			if (a == null || (a.Count != 3)) throw new ArgumentException("Substring expects three arguments, first string, second startIndex, third length, will return the type of first arg.");

			var exp0 = a[0].Type == typeof(string) ? a[0] : Linq.Expression.Convert(a[0], typeof(string));
			var exp1 = a[1].Type == typeof(int) ? a[1] : Linq.Expression.Convert(a[1], typeof(int));
			var exp2 = a[2].Type == typeof(int) ? a[2] : Linq.Expression.Convert(a[2], typeof(int));

			return Linq.Expression.Call(exp0,
				typeof(string).GetMethod("Substring", new[] { typeof(int), typeof(int) }),
				exp1, exp2);
		}

		static object StringLengthHandler(List<Linq.Expression> a)
		{
			if (a == null || (a.Count != 1)) throw new ArgumentException("StringLength expects one argument, returns the number of characters in the current string arg.");
			var exp0 = a[0].Type == typeof(string) ? a[0] : Linq.Expression.Convert(a[0], typeof(string));
			return Linq.Expression.Property(exp0, typeof(string).GetProperty("Length"));
		}

		static object IsNullHandler(List<Linq.Expression> a)
		{
			if (a == null || a.Count != 2) throw new ArgumentException("IsNull expects two arguments");
			return Linq.Expression.Coalesce(a[0], a[1]);
		}

		static object ChooseHandler(List<Linq.Expression> a)
		{
			if (a == null || (a.Count < 2)) throw new ArgumentException("Choose expects at least two arguments");
			var exp1 = a[0].Type == typeof(int) ? a[0] : Linq.Expression.Convert(a[0], typeof(int));
			var array = Linq.Expression.NewArrayInit(a[1].Type, a.Skip(1));
			return Linq.Expression.ArrayAccess(array, exp1);
		}

		static object IIfHandler(List<Linq.Expression> a)
		{
			if (a == null || a.Count != 3) throw new ArgumentException("IIf expects three arguments");
			var exp1 = a[0].Type == typeof(bool) ? a[0] : Linq.Expression.Convert(a[0], typeof(bool));
			var b = Helpers.Promote(a[1], a[2], true);
			return Linq.Expression.Condition(exp1, b[0], b[1]);
		}

		static object FormatHandler(List<Linq.Expression> a)
		{
			if (a == null || a.Count != 2
				|| a[1].Type != typeof(string))
				throw new ArgumentException("Format expects two arguments, the second of which is a string");

			return Linq.Expression.Call(a[0],
				a[0].Type.GetMethod("ToString", new[] { typeof(string) }),
				a[1]);
		}

		static object MathRoundHandler(List<Linq.Expression> a)
		{
			if (a == null || (a.Count != 1 && a.Count != 2)
				|| !a[0].Type.IsValueType
				|| a[0].Type == typeof(DateTime)
				)
				throw new ArgumentException("Math.Round expects one or two numeric arguments");

			var exp1 = a[0].Type == typeof(double) ? a[0] : Linq.Expression.Convert(a[0], typeof(double));
			if (a.Count == 1) return Linq.Expression.Call(null,
				typeof(Math).GetMethod("Round", new[] { typeof(double) }),
				exp1);

			var exp2 = a[1].Type == typeof(int) ? a[1] : Linq.Expression.Convert(a[1], typeof(int));

			return Linq.Expression.Call(null,
				typeof(Math).GetMethod("Round", new[] { typeof(double), typeof(int) }),
				exp1, exp2);
		}


		static object MathPowHandler(List<Linq.Expression> a)
		{
			if (a == null || a.Count != 2
				|| !a[0].Type.IsValueType
				|| a[0].Type == typeof(DateTime)
				|| !a[1].Type.IsValueType
				|| a[1].Type == typeof(DateTime))
				throw new ArgumentException("Math.Pow expects two numeric arguments");

			var exp1 = a[0].Type == typeof(double) ? a[0] : Linq.Expression.Convert(a[0], typeof(double));
			var exp2 = a[1].Type == typeof(double) ? a[1] : Linq.Expression.Convert(a[1], typeof(double));


			return Linq.Expression.Call(null,
				typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) }),
				exp1, exp2);
		}

		static object MathHandler(string method, List<Linq.Expression> a)
		{
			if (a == null || a.Count != 1 || !a[0].Type.IsValueType || a[0].Type == typeof(DateTime))
				throw new ArgumentException("Math." + method + " expects one numeric argument");

			var exp = a[0];
			if (a[0].Type != typeof(double)) exp = Linq.Expression.Convert(exp, typeof(double));

			return Linq.Expression.Call(null,
				typeof(Math).GetMethod(method, new[] { typeof(double) }),
				exp);
		}

		static object CStrHandler(List<Linq.Expression> a)
		{
			if (a == null || a.Count != 1) throw new ArgumentException("CStr expects one argument");
            return Helpers.CStr(a[0]);
		}

		static object CIntHandler(List<Linq.Expression> a)
		{
			if (a == null || a.Count != 1) throw new ArgumentException("CInt expects one argument");
            if (a[0].Type != typeof(string)) return Helpers.CType(a[0], typeof(int));

			var methodInfo = typeof(int).GetMethod("Parse", new[] { typeof(string) });
			return Linq.Expression.Call(methodInfo, a[0]);
		}

		static object CDblHandler(List<Linq.Expression> a)
		{
			if (a == null || a.Count != 1) throw new ArgumentException("CDbl expects one argument");
            if (a[0].Type != typeof(string)) return Helpers.CType(a[0], typeof(double));

			var methodInfo = typeof(double).GetMethod("Parse", new[] { typeof(string) });
			return Linq.Expression.Call(methodInfo, a[0]);
		}

		static object CBoolHandler(List<Linq.Expression> a)
		{
			if (a == null || a.Count != 1) throw new ArgumentException("CBool expects one argument");
            if (a[0].Type != typeof(string)) return Helpers.CType(a[0], typeof(bool));

			var methodInfo = typeof(bool).GetMethod("Parse", new[] { typeof(string) });
			return Linq.Expression.Call(methodInfo, a[0]);
		}

		static object CDateHandler(List<Linq.Expression> a)
		{
			if (a == null || a.Count != 1) throw new ArgumentException("CDate expects one argument");
			if (a[0].Type == typeof(DateTime)) return a[0];
            if (a[0].Type != typeof(string)) throw new ArgumentException("CDate expects a string argument");

			var methodInfo = typeof(DateTime).GetMethod("Parse", new[] { typeof(string) });
			return Linq.Expression.Call(methodInfo, a[0]);
		}

		internal static object Process(string fn, List<Linq.Expression> args)
		{
			Func<List<Linq.Expression>, object> del;
			if (!Handlers.TryGetValue(fn, out del)) throw new ArgumentException("Unknown function: " + fn);
			return del(args);
		}
	}
}
