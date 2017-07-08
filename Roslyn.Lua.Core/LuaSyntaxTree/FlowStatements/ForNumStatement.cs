using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowStatements
{
    public class ForNumStatement : Statement
    {
        protected ForNumStatement(IEnumerable<Token> source, IdentifierExpression identifer, Expression from, Expression to, Expression step, Block body) : base(source)
        {
            Identifier = identifer;
            From = from;
            To = to;
            Step = step;
            Body = body;
        }

        public static ForNumStatement Create(IEnumerable<Token> source)
        {
            var local = source;
            if (local.First().Source == "for")
            {
                if (local.Skip(1).First().Type == TokenType.Identifier)
                {
                    var identifier = IdentifierExpression.Create(local.Skip(1).Take(1));
                    local = local.Skip(1 + identifier.Source.Count());
                    if (local.First().Type == TokenType.AssignmentOperator)
                    {
                        local = local.Skip(1);
                        var from = local.Create<Expression>();
                        local = local.Skip(from.Source.Count());
                        if (local.First().Type == TokenType.Delimiter)
                        {
                            local = local.Skip(1);
                            var to = local.Create<Expression>();
                            local = local.Skip(to.Source.Count());
                            Expression step = null;
                            if (local.First().Type == TokenType.Delimiter)
                            {
                                local = local.Skip(1);
                                step = local.Create<Expression>();
                                local = local.Skip(step.Source.Count());
                            }
                            var body = Block.Create(local);
                            if (body != null)
                            {
                                return new ForNumStatement(source.Take(source.Count() - local.Count()), identifier, from, to, step, body);
                            }
                        }
                    }
                }
            }
            return null;
        }

        public IdentifierExpression Identifier { get; }

        public Expression From { get; }

        public Expression To { get; }

        public Expression Step { get; }

        public Block Body { get; }
    }
}
