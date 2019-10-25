using System.Collections.Generic;
using System.Diagnostics;

namespace Collections
{
    /// <summary>
    /// Single implementation of a comparer that works in both hash/equality and ordered collection types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICollectionComparer<in T> : IEqualityComparer<T>, IComparer<T>
    {
    }

    #region Comparer Examples
    #region Example: show's the necessary parent class Boilerplate code
    /*
	public class Test : IComparable<Test>
	{
		readonly string _Value;

		public Test(string value)
		{
			_Value = value;
		}

		public override string ToString()
		{
			return _Value;
		}

		public override int GetHashCode()
		{
			return _Value.GetHashCode();
		}

		#region Comparer
		class _Comparer : ComparerBase<Test>
		{
			protected override int CompareUniqueNonNull(Test x, Test y)
			{
				return string.CompareOrdinal(x._Value, y._Value);
			}

			protected override bool EqualsUniqueNonNull(Test x, Test y)
			{
				return string.Equals(x._Value, y._Value, StringComparison.Ordinal);
			}
		}
		#endregion

		#region Boilerplate
		/// <summary>
		/// Comparer to use in collections
		/// </summary>
		public readonly static ICollectionComparer<Test> Comparer = new _Comparer();

		int IComparable<Test>.CompareTo(Test other)
		{
			return Comparer.Compare(this, other);
		}
		public override bool Equals(object obj)
		{
			return Comparer.Equals(this, obj as Test);
		}
		public static bool operator ==(Test lhs, Test rhs)
		{
			return Comparer.Equals(lhs, rhs);
		}
		public static bool operator >=(Test lhs, Test rhs)
		{
			return Comparer.Compare(lhs, rhs) >= 0;
		}
		public static bool operator <=(Test lhs, Test rhs)
		{
			return Comparer.Compare(lhs, rhs) <= 0;
		}
		public static bool operator !=(Test lhs, Test rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
	}
	*/
    #endregion

    /// <summary>
    /// Helper base for all Comparers - inherit off this using a nested class and paste the boilerplate code into the parent
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerStepThrough]
    public abstract class ComparerBase<T> : ICollectionComparer<T> where T : class
    {
        protected abstract int CompareUniqueNonNull(T x, T y);

        /// <summary>
        /// Override if there is a more efficient way to compare for equality only
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected virtual bool EqualsUniqueNonNull(T x, T y)
        {
            return CompareUniqueNonNull(x, y) == 0;
        }

        #region IComparer<T> Members
        public int Compare(T x, T y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(x, null)) return -1;
            if (ReferenceEquals(y, null)) return 1;
            return CompareUniqueNonNull(x, y);
        }
        #endregion

        #region IEqualityComparer<T> Members
        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(y, null)) return false;
            if (ReferenceEquals(x, null)) return false;

            return EqualsUniqueNonNull(x, y);
        }

        public virtual int GetHashCode(T obj)
        {
            if (ReferenceEquals(obj, null)) return 0;
            return obj.GetHashCode();
        }
        #endregion
    }
    #endregion

    #region Comparers
    public static class Comparers
    {
        public readonly static ICollectionComparer<string> StringComparer = new _StringEqualityComparer();
        public readonly static ICollectionComparer<char> CharComparer = new _CharEqualityComparer();
        public readonly static ICollectionComparer<int> IntComparer = new _IntEqualityComparer();

        class _CharEqualityComparer : ICollectionComparer<char>
        {
            public bool Equals(char x, char y)
            {
                return x == y;
            }

            public int GetHashCode(char obj)
            {
                return obj.GetHashCode();
            }

            public int Compare(char x, char y)
            {
                return x - y;
            }
        }
        class _StringEqualityComparer : ICollectionComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return string.CompareOrdinal(x, y) == 0;
            }

            public int GetHashCode(string obj)
            {
                return obj != null ? obj.GetHashCode() : 0;
            }

            public int Compare(string x, string y)
            {
                return string.CompareOrdinal(x, y);
            }
        }
        class _IntEqualityComparer : ICollectionComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }

            public int Compare(int x, int y)
            {
                return x - y;
            }
        }
    }
    #endregion
}
