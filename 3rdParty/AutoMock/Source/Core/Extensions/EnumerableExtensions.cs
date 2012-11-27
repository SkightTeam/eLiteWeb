using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.Specifications.AutoMocking.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }
    }
}
