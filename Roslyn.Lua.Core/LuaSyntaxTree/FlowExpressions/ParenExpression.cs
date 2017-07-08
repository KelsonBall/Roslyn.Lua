using System;
using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowExpressions
{
    public class ParenExpression : Expression
    {
        public ParenExpression(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public static ParenExpression Create(IEnumerable<Token> source)
        {
            if (source.First().Type == TokenType.ParenStart)
            {
                source = source.Skip(1);
            }

            return null;
        }

        public Expression ChildExpression { get; }
    }
}
