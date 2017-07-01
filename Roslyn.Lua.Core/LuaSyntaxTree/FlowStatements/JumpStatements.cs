using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowStatements
{
    public class ReturnStatement : Statement
    {
        public ReturnStatement(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Expression> Returns { get; }
    }

    public class BreakStatement : Statement
    {
        public BreakStatement(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }
    }

    public class ContinueStatement : Statement
    {
        public ContinueStatement(IEnumerable<Token> source) : base(source)
        {
            throw new NotImplementedException();
        }
    }
}
