using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public abstract class Expression : LuaSyntaxNode
    {
        public Expression(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }
    }
}
