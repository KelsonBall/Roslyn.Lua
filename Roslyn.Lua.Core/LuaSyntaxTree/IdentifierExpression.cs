using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public class IdentifierExpression : Expression
    {
        public IdentifierExpression(IEnumerable<Token> source) : base(source)
        {
            Identifier = source.First();
            source = source.Skip(1);
            if (source.Any())
            {
                while (source.First().Type == TokenType.IndexerStart)
                {
                    var range = source.TokenRange(TokenType.IndexerStart, TokenType.IndexerEnd);
                    source = source.Skip(range.Count());
                    IndexExpression.Add(range.Crop(1).Create<Expression>());
                }
                if (source.Any())
                {
                    Next = IdentifierExpression.Create(source);
                }
            }
        }

        private readonly List<Token> _tokens = new List<Token>();
        public IEnumerable<Token> Tokens => _tokens.AsEnumerable();

        public Token Identifier { get; }
        public List<Expression> IndexExpression { get; } = new List<Expression>();
        public IdentifierExpression Next { get; }

        public static IdentifierExpression Create(IEnumerable<Token> source)
        {
            List<Token> tokens = new List<Token>();

            Identifier:
            if (source.First().Type == TokenType.Identifier)
            {
                tokens.Add(source.First());
                source = source.Skip(1);
                if (source.Any())
                if (source.First().Source == "." && source.Skip(1).First().Type == TokenType.Identifier)
                {
                    tokens.Add(source.First());
                    source = source.Skip(1);
                    var child = IdentifierExpression.Create(source);
                    tokens.AddRange(child.Source);
                    source = source.Skip(child.Source.Count());                    
                }
                else if (source.First().Source == ":" && source.Skip(1).First().Type == TokenType.Identifier)
                {
                    tokens.AddRange(source.Take(2));
                    source = source.Skip(2);
                }
                else if (source.Count() > 1 && source.Skip(1).First().Type == TokenType.IndexerStart)
                {
                    var range = source.Skip(1).TokenRange(TokenType.IndexerStart, TokenType.IndexerEnd);
                    tokens.AddRange(range);
                    source = source.Skip(range.Count() + 1);
                    goto Identifier;
                }
            }

            if (tokens.Any())
                return new IdentifierExpression(tokens);

            return null;
        }

    }
}
