using System;
using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public abstract class Statement : LuaSyntaxNode
    {
        public Statement(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }
    }

    public class DoStatement : Statement
    {
        public DoStatement(IEnumerable<Token> source) : base(source)
        {
            var local = source;
            while (local.Any())
            {
                throw new NotImplementedException();
            }
        }

        public static DoStatement Specifies(IEnumerable<Token> source)
        {
            if (source.First().Source == "do")
                return new DoStatement(source.TokenRange(TokenType.BlockStart, TokenType.BlockEnd));
            return null;
        }

        public IEnumerable<Statement> Statements { get; }
    }

    public class AssignmentStatement : Statement
    {
        public AssignmentStatement(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Expression> SetTo { get; }

        public IEnumerable<Expression> SetFrom { get; }
    }
}
