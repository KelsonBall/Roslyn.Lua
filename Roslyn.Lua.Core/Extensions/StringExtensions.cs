using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core
{
    public static class StringExtensions
    {
        public static bool StartsWithAnItemOf(this string source, IEnumerable<string> items)
        {
            foreach (string item in items)
                if (source.StartsWith(item))
                    return true;
            return false;
        }

        public static int IndexOfAny(this string source, IEnumerable<string> items)
        {
            var indecies = items.Select(i => source.IndexOf(i)).Where(i => i >= 0);
            if (indecies.Any())
                return indecies.Min();
            return -1;
        }

        public static IEnumerable<Token> Tokens(this string source)
        {
            source = source + System.Environment.NewLine;
            var result = Token.PopToken(source);
            while (result.token != null)
            {
                yield return result.token;
                result = Token.PopToken(result.remainder);
            }
        }
    }
}
