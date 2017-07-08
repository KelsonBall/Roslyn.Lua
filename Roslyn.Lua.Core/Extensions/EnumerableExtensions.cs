using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> And<T>(this IEnumerable<T> source, params IEnumerable<T>[] next)
        {
            foreach (var item in source)
                yield return item;
            foreach (var collection in next)
                foreach (var item in collection)
                    yield return item;
        }

        public static IEnumerable<T> And<T>(this IEnumerable<T> source, params T[] items)
        {
            foreach (var item in source)
                yield return item;
            foreach (var item in items)               
                    yield return item;
        }

        public static IEnumerable<T> Crop<T>(this IEnumerable<T> source, int count)
        {
            source = source.Skip(count);
            int length = source.Count();
            return source.Take(length - count);
        }
    }
}
