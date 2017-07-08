using System;
using System.Collections.Generic;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowStatements
{
    public class RepeatStatement : Statement
    {
        protected RepeatStatement(IEnumerable<Token> source, Expression condition, Block block) : base(source)
        {
            throw new NotImplementedException();
        }

        public static RepeatStatement Create(IEnumerable<Token> source)
        {            
            return null;
        }

        public Expression Condition { get; }

        public Block Body { get; }
    }
}
