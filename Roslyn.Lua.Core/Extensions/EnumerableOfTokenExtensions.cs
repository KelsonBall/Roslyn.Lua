using System;
using System.Linq;
using TokenSource = System.Collections.Generic.IEnumerable<Roslyn.Lua.Core.Token>;

namespace Roslyn.Lua.Core
{
    public static class TokenSourceExtensions
    {
        public static TokenSource TokenRange(this TokenSource source, TokenType start, TokenType end)
        {
            if (!source.Any() || source.First().Type != start)
                yield break;

            source = source.Skip(1);

            int stack = 1;
            while (stack > 0)
            {
                if (!source.Any())
                    throw new InvalidOperationException("Missing block end token");
                if (source.First().Type == start)
                    stack++;
                if (source.First().Type == end)
                    stack--;

                yield return source.First();

                source = source.Skip(1);
            }
        }
    }
}
