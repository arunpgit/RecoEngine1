using System;
using System.Reflection;
using Linq = System.Linq.Expressions;

namespace CustomColumns.Expressions.Internal
{
    static class Helpers
    {
        static readonly MethodInfo _toStringMethod = typeof(object).GetMethod("ToString");

        internal static Linq.Expression CStr(object userObject)
        {
            var exp = (Linq.Expression)userObject;
            if (exp.Type != typeof(string))
            {
                var toString = Linq.Expression.Call(
                    exp,
                    _toStringMethod
                    );

                return toString;
            }
            return exp;
        }

        internal static Linq.Expression CType(object userObject, Type target)
        {
            var exp = (Linq.Expression)userObject;
            if (exp.Type != target)
            {
                exp = Linq.Expression.Convert(exp, target);
            }
            return exp;
        }

		internal static Linq.Expression[] Promote(object userObject1, object userObject2, bool promoteToStringFirst = false)
		{
			var res = new Linq.Expression[2] 
            {
                (Linq.Expression)userObject1,
                (Linq.Expression)userObject2 
            };

			if (promoteToStringFirst)
			{
				//if either is string, ensure both are string and return
				if (res[0].Type == typeof(string))
				{
					res[1] = CStr(userObject2);
					return res;
				}
				if (res[1].Type == typeof(string))
				{
					res[0] = CStr(userObject1);
					return res;
				}
			}

			//if any of the arguments are dateTime, then handle them in a custom fashion
			if (res[0].Type == typeof(DateTime) || res[1].Type == typeof(DateTime))
			{
				if (PromoteTypesForDateTime(res)) return res;
			}

			//1. If either is double, ensure both are double
			if (res[0].Type == typeof(double))
			{
				if (res[1].Type != typeof(double)) res[1] = Linq.Expression.Convert(res[1], typeof(double));
			}
			else if (res[1].Type == typeof(double))
			{
				if (res[0].Type != typeof(double)) res[0] = Linq.Expression.Convert(res[0], typeof(double));
			}
			//2. If either is decimal, ensure both are decimal
			else if (res[0].Type == typeof(decimal))
			{
				if (res[1].Type != typeof(decimal)) res[1] = Linq.Expression.Convert(res[1], typeof(decimal));
			}
			else if (res[1].Type == typeof(decimal))
			{
				if (res[0].Type != typeof(decimal)) res[0] = Linq.Expression.Convert(res[0], typeof(decimal));
			}
			return res;
		}

		static readonly long _ticksInADay = new TimeSpan(1, 0, 0, 0).Ticks;
		static readonly ConstructorInfo _timespanCtor = typeof(TimeSpan).GetConstructor(new[] { typeof(long) });
		static readonly PropertyInfo _ticksProperty = typeof(DateTime).GetProperty("Ticks");
		static readonly PropertyInfo _totalDaysProperty = typeof(TimeSpan).GetProperty("TotalDays");

		static bool PromoteTypesForDateTime(Linq.Expression[] res)
		{
			//if both are dates
			if (res[0].Type == res[1].Type)
			{
				//DateTime d1;
				//DateTime d2;
				//var double1 = new TimeSpan(d1.Ticks).TotalDays;
				//var double2 = new TimeSpan(d2.Ticks).TotalDays;

				var ticksExp = Linq.Expression.Property(res[0], _ticksProperty);
				var timespanExp = Linq.Expression.New(_timespanCtor, ticksExp);
				var totalDaysExp = Linq.Expression.Property(timespanExp, _totalDaysProperty);
				res[0] = totalDaysExp;

				ticksExp = Linq.Expression.Property(res[1], _ticksProperty);
				timespanExp = Linq.Expression.New(_timespanCtor, ticksExp);
				totalDaysExp = Linq.Expression.Property(timespanExp, _totalDaysProperty);
				res[1] = totalDaysExp;

				return true;
			}
			//at least one of them is a date
			else
			{
				if (res[0].Type == typeof(int) || res[0].Type == typeof(double) || res[0].Type == typeof(decimal))
				{
					res[0] = ConvertNumberToTimeSpan(res[0]);
					return true;
				}
				if (res[1].Type == typeof(int) || res[1].Type == typeof(double) || res[1].Type == typeof(decimal))
				{
					res[1] = ConvertNumberToTimeSpan(res[1]);
					return true;
				}
			}
			return false;
		}

		static Linq.Expression ConvertNumberToTimeSpan(Linq.Expression expression)
		{
			//double days = (double)input;
			//var totalTicks = days * _ticksInADay
			//var timespan = new TimeSpan(totalTicks);
			var daysExp = expression.Type != typeof(double) ? Linq.Expression.Convert(expression, typeof(double)) : expression;
			var ticksInDayExp = Linq.Expression.Convert(Linq.Expression.Constant(_ticksInADay), typeof(double));
			var totalTicksExp = Linq.Expression.Multiply(daysExp, ticksInDayExp);
			var timeSpanExp = Linq.Expression.New(_timespanCtor, Linq.Expression.Convert(totalTicksExp, typeof(long)));
			return timeSpanExp;
		}
         
        static MethodInfo GetStaticMethod(string methodName, Linq.Expression left, Linq.Expression right)
        {
            return left.Type.GetMethod(methodName, new[] { left.Type, right.Type });
        }

        internal static Linq.Expression GenerateStaticMethodCall(string methodName, Linq.Expression left, Linq.Expression right)
        {
            return Linq.Expression.Call(null, GetStaticMethod(methodName, left, right), new[] { left, right });
        }
    }
}
