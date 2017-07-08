using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree.OperationExpressions
{
    public class CallExpression : Expression
    {
        protected CallExpression(IEnumerable<Token> source, IdentifierExpression identifier, IEnumerable<IdentifierExpression> arguments) : base(source)
        {
            Identifier = identifier;
            Arguments = arguments;
        }

        public static CallExpression Create(IEnumerable<Token> source)
        {            
            var identifier = source.Create<IdentifierExpression>();
            if (identifier == null)
                return null;
            var tokens = new List<Token>();
            tokens.AddRange(identifier.Source);
            source = source.Skip(identifier.Source.Count());
            if (source.First().Type != TokenType.ParenStart)
                return null;

            var paramRange = source.TokenRange(TokenType.ParenStart, TokenType.ParenEnd);
            source = source.Skip(paramRange.Count());
            tokens.AddRange(paramRange);
            paramRange = paramRange.Crop(1);
            var arguments = new List<IdentifierExpression>();
            IdentifierExpression argument = null;           
            while (paramRange.Any() && (argument = paramRange.Create<IdentifierExpression>()) != null)
            {
                arguments.Add(argument);
                paramRange = paramRange.Skip(argument.Source.Count());
                if (!paramRange.Any() || paramRange.First().Type != TokenType.Delimiter)
                    break;
                paramRange = paramRange.Skip(1);
            }
            if (paramRange.Any())
                return null;

            return new CallExpression(tokens, identifier, arguments);
        }

        public IdentifierExpression Identifier { get; private set; }

        public IEnumerable<IdentifierExpression> Arguments { get; private set; }
    }
}
