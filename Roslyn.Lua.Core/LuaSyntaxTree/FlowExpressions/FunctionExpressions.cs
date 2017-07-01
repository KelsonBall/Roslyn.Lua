using Roslyn.Lua.Core.LuaSyntaxTree.LiteralExpressions;
using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowExpressions
{
    public class FunctionExpression : Expression
    {
        public FunctionExpression(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StringLiteralExpression> Parameters { get; }

        public DotsExpression Dots { get; }

        public Block Body { get; }       
    }

    public class DotsExpression : Expression
    {
        public DotsExpression(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }
    }
}
