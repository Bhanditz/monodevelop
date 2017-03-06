using System;
using System.Collections.Generic;

namespace CorApi2.Metadata
{
    class AscendingValueComparer<K, V> : IComparer<KeyValuePair<K, V>> where V:IComparable
    {
        public int Compare(KeyValuePair<K,V> p1, KeyValuePair<K, V> p2)
        {
            return p1.Value.CompareTo(p2.Value);
        }

        public bool Equals(KeyValuePair<K, V> p1, KeyValuePair<K, V> p2)
        {
            return Compare(p1,p2) == 0;
        }

        public int GetHashCode(KeyValuePair<K, V> p)
        {
            return p.Value.GetHashCode();
        }
    }
}