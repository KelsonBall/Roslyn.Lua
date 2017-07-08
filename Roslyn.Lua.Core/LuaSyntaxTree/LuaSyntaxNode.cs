using Roslyn.Lua.Core.Extensions;
using Roslyn.Lua.Core.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public abstract class LuaSyntaxNode
    {
        public IEnumerable<Token> Source { get; private set; }

        protected LuaSyntaxNode(IEnumerable<Token> source)
        {
            Source = source;
        }

        public virtual StringBuilder SerializeToXml(StringBuilder text, int depth = 0)
        {
            return text.AppendLine($"{depth.Of(" ")}<{this.GetType().Name}>{string.Join(",", Source)}</{this.GetType().Name}>");
        }

        public virtual void Execute(LuaState state)
        {
            throw new NotImplementedException();
        }
    }
}
