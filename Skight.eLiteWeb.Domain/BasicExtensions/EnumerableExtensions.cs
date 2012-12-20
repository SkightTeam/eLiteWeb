using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Domain.BasicExtensions
{
    public static class EnumerableExtensions
    {
        public static void each<T>(this IEnumerable<T> items, Action<T> action) {
            foreach (T item in items) {
                action(item);
            }
        }    
    }
}