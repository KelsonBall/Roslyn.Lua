using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree.LiteralExpressions
{
    public class TableExpression : Expression
    {
        public TableExpression(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<Expression, Expression>> Assignments { get; }

        public IEnumerable<Expression> Expressions { get; }
    }
}
