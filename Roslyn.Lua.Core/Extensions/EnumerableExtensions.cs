using System.Collections.Generic;

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
    }
}
