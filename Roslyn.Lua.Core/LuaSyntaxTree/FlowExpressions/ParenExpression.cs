using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowExpressions
{
    public class ParenExpression : Expression
    {
        public ParenExpression(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public Expression ChildExpression { get; }
    }
}
