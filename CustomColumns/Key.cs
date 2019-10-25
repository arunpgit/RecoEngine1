using Collections;
using System;

namespace CustomColumns
{
    public class Key : IComparable<Key>
    {
        public readonly string Value;

        public Key(string key)
        {
            Value = key;
        }

        public override string ToString()
        {
            return Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #region Comparer
        class _Comparer : ComparerBase<Key>
        {
            protected override int CompareUniqueNonNull(Key x, Key y)
            {
                return string.CompareOrdinal(x.Value, y.Value);
            }

            protected override bool EqualsUniqueNonNull(Key x, Key y)
            {
                return string.Equals(x.Value, y.Value, StringComparison.Ordinal);
            }
        }
        #endregion

        #region Boilerplate
        /// <summary>
        /// Comparer to use in collections
        /// </summary>
        public readonly static ICollectionComparer<Key> Comparer = new _Comparer();

        int IComparable<Key>.CompareTo(Key other)
        {
            return Comparer.Compare(this, other);
        }
        public override bool Equals(object obj)
        {
            return Comparer.Equals(this, obj as Key);
        }
        public static bool operator ==(Key lhs, Key rhs)
        {
            return Comparer.Equals(lhs, rhs);
        }
        public static bool operator >=(Key lhs, Key rhs)
        {
            return Comparer.Compare(lhs, rhs) >= 0;
        }
        public static bool operator <=(Key lhs, Key rhs)
        {
            return Comparer.Compare(lhs, rhs) <= 0;
        }
        public static bool operator !=(Key lhs, Key rhs)
        {
            return !(lhs == rhs);
        }
        #endregion
    }
}
