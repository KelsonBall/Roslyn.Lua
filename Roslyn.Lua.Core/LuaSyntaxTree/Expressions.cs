using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public abstract class Expression : LuaSyntaxNode
    {
        protected Expression(IEnumerable<Token> source) : base(source) { }
    }

    public abstract class LiteralExpression : Expression
    {
        protected LiteralExpression(IEnumerable<Token> source) : base(source) { }
    }

    
}
