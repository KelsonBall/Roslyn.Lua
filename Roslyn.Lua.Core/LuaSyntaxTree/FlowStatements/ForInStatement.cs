using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowStatements
{
    public class ForInStatement : Statement
    {
        protected ForInStatement(IEnumerable<Token> source, IEnumerable<IdentifierExpression> identifiers, IEnumerable<Expression> iterators, Block body) : base(source)
        {
            Identifiers = identifiers;
            Iterators = iterators;
            Body = body;
        }

        public static ForInStatement Create(IEnumerable<Token> source)
        {
            var local = source;
            if (local.First().Source == "for")
            {
                local = local.Skip(1);
                var identifiers = new List<IdentifierExpression>();
                IdentifierExpression identifier = null;
                while ((identifier = IdentifierExpression.Create(local)) != null)
                {
                    identifiers.Add(identifier);
                    local = local.Skip(identifier.Source.Count());
                    if (!local.Any() || local.First().Type != TokenType.Delimiter)
                        break;
                    local = local.Skip(1);
                }

                if (local.First().Source != "in")
                    return null;

                local = local.Skip(1);
                var iterators = new List<Expression>();
                Expression iterator = null;
                while ((iterator = local.Create<Expression>()) != null)
                {
                    iterators.Add(iterator);
                    local = local.Skip(iterator.Source.Count());
                    if (!local.Any() || local.First().Type != TokenType.Delimiter)
                        break;
                    local = local.Skip(1);
                }

                if (local.First().Source != "do")
                    return null;

                var body = Block.Create(local);

                if (body != null)
                    return new ForInStatement(source.Take(source.Count() - local.Count()), identifiers, iterators, body);
            }
            return null;
        }

        public IEnumerable<IdentifierExpression> Identifiers { get; }

        public IEnumerable<Expression> Iterators { get; }

        public Block Body { get; }
    }
}