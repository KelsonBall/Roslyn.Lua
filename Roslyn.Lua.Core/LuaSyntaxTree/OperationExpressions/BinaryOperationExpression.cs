using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree.OperationExpressions
{
    public class BinaryOperandExpression : Expression
    {
        public BinaryOperandExpression(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public enum OpId
        {
            // arithmetic
            Add, Sub, Mul, Div, IDiv, Mod, Pow,
            // binary
            bAnd, bOr, bXor, bNot, Shl, Shr,
            // logical
            And, Or,
            Eq, Ne, Lt, Gt, Le, Ge,
            // string
            Concat,
        }

        public OpId Operation { get; }

        public Expression LeftOperand { get; }

        public Expression RightOperand { get; }
    }
}
