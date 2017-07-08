using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowStatements
{
    public class WhileStatement : Statement
    {
        protected WhileStatement(IEnumerable<Token> source, Expression condition, Block block) : base(source)
        {
            Condition = condition;
            Body = block;
        }

        public static WhileStatement Create(IEnumerable<Token> source)
        {
            var local = source;
            if (local.First().Source == "while")
            {
                local = local.Skip(1);
                var expr = local.Create<Expression>();
                if (expr != null)
                {
                    local = local.Skip(expr.Source.Count());
                    if (local.First().Source == "do")
                    {
                        var block = local.Create<Block>();
                        if (block != null)
                        {
                            return new WhileStatement(source.Take(expr.Source.Count() + block.Source.Count() + 1), expr, block);
                        }
                    }
                }
            }
            return null;
        }

        public Expression Condition { get; }

        public Block Body { get; }
    }

}
