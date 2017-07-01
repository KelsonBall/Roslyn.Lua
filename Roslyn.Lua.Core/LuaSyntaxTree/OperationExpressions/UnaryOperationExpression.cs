using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree.OperationExpressions
{
    public class UnaryOperandExpression : Expression
    {
        public UnaryOperandExpression(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public enum OpId
        {
            Unm, Len, Not, bNot
        }

        public OpId Operation { get; }

        public Expression Operand { get; }
    }
}
