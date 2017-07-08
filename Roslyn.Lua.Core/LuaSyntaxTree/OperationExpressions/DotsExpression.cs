using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree.OperationExpressions
{
    public class DotsExpression : Expression
    {
        public DotsExpression(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public static DotsExpression Create(IEnumerable<Token> token)
        {
            return null;
        }
    }
}
