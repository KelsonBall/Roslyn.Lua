using Roslyn.Lua.Core.LuaSyntaxTree.LiteralExpressions;
using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowStatements
{
    public class WhileStatement : Statement
    {
        public WhileStatement(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public Expression Condition { get; }

        public Block Body { get; }
    }

    public class RepeatStatement : Statement
    {
        public RepeatStatement(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public Expression Condition { get; }

        public Block Body { get; }
    }

    public class ForNumStatement : Statement
    {
        public ForNumStatement(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public StringLiteralExpression Identifier { get; }

        public Expression From { get; }

        public Expression To { get; }

        public Expression Step { get; }

        public Block Body { get; }
    }

    public class ForInStatement : Statement
    {
        public ForInStatement(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StringLiteralExpression> Identifiers { get; }

        public IEnumerable<Expression> Iterators { get; }

        public Block Body { get; }
    }
}
