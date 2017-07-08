using System.Linq;

namespace Roslyn.Lua.Core.Extensions
{
    public static class IntegerExtensions
    {
        public static string Of(this int n, string s)
        {
            return string.Join("", Enumerable.Range(0, n).Select(i => s));
        }
    }
}
