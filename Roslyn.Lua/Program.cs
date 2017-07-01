using Roslyn.Lua.Core;
using System;
using System.Linq;

namespace Roslyn.Lua
{
    class Program
    {
        const string exampleSource =
@"x = 4
local s = 'this is some text'
local t = { { 0 }, { { 3.14 } }, { { { 'hahaha' } } } }

function add(a, b)
    return b, a + -b
end
c, d = add(2, x)
print(c) 
";
        static void Main(string[] args)
        {
            var tokens = exampleSource.Tokens().Where(t => t.Type != TokenType.Whitespace);
            Console.WriteLine(string.Join("\r\n", tokens.Select(t => $"[{t.Source}, {t.Type}]")));
        }
    }
}
