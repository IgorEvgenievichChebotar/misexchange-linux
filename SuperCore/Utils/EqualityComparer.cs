using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore
{
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        Func<T, T, bool> _funcEq;
        Func<T, int> _funcHash;
        public EqualityComparer(Func<T, T, bool> funcEq, Func<T, int> funcHash)
        {
            _funcEq = funcEq;
            _funcHash = funcHash;
        }
        public bool Equals(T first, T second)
        {
            return _funcEq(first, second);
        }
        public int GetHashCode(T obj)
        {
            return _funcHash(obj);
        }

    }
}
