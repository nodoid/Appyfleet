using System;
using System.Collections.Generic;

namespace mvvmframework
{
    public static class LinqHelper
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
                yield return item;
            }
        }
    }
}

