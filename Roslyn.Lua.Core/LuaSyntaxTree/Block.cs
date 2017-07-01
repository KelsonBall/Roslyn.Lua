using System;
using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public class Block : LuaSyntaxNode
    {
        public Block(IEnumerable<Token> source) : base(source)
        {
            var tokens = source.SkipWhile(t => t.Type == TokenType.Whitespace);            
        }

        public IEnumerable<Statement> Statements { get; }
    }
}
 