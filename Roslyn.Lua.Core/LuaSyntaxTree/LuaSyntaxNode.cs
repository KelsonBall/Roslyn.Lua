using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public abstract class LuaSyntaxNode
    {
        public IEnumerable<Token> Source { get; private set; }

        protected LuaSyntaxNode(IEnumerable<Token> source)
        {
            Source = source;
        }
    }
}
