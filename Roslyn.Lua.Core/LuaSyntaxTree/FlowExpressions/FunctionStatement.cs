using Roslyn.Lua.Core.Extensions;
using Roslyn.Lua.Core.LuaSyntaxTree.OperationExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowExpressions
{
    public class FunctionStatement : Statement
    {
        protected FunctionStatement(IEnumerable<Token> source, IdentifierExpression name, IEnumerable<IdentifierExpression> parameters, IEnumerable<Statement> statements) : base(source)
        {
            Name = name;
            Parameters = parameters;
            Statements = statements;            
        }

        public static FunctionStatement Create(IEnumerable<Token> source)
        {
            List<Token> tokens = new List<Token>();

            if (source.First().Source != "function")
                return null;

            tokens.Add(source.First());
            source = source.Skip(1);

            if (source.First().Type != TokenType.Identifier)
                return null;

            var identifier = IdentifierExpression.Create(source.Take(1));

            tokens.Add(source.First());
            source = source.Skip(1);

            if (source.First().Type != TokenType.ParenStart)
                return null;

            var paramRange = source.TokenRange(TokenType.ParenStart, TokenType.ParenEnd);
            tokens.AddRange(paramRange);
            source = source.Skip(paramRange.Count());
            paramRange = paramRange.Crop(1);

            List<IdentifierExpression> parameters = new List<IdentifierExpression>();
            IdentifierExpression parameter = null;            
            while (paramRange.Any() && (parameter = IdentifierExpression.Create(paramRange.Take(1))) != null)
            {
                parameters.Add(parameter);
                paramRange = paramRange.Skip(1);
                if (!paramRange.Any() || paramRange.First().Type != TokenType.Delimiter)
                    break;
                paramRange = paramRange.Skip(1);
            }

            List<Statement> statements = new List<Statement>();
            Statement statement = null;
            while (source.Any() 
                && source.First().Type != TokenType.BlockEnd 
                &&(statement = source.Create<Statement>()) != null)
            {
                statements.Add(statement);
                tokens.AddRange(statement.Source);
                source = source.Skip(statement.Source.Count());     
            }

            if (source.Any() && source.First().Type == TokenType.BlockEnd)
            {
                tokens.Add(source.First());
                return new FunctionStatement(tokens, identifier, parameters, statements);
            }

            return null;
        }

        public IdentifierExpression Name { get; }

        public IEnumerable<IdentifierExpression> Parameters { get; private set; }

        public DotsExpression Dots { get; }

        public IEnumerable<Statement> Statements { get; private set; }

        public override StringBuilder SerializeToXml(StringBuilder text, int depth = 0)
        {
            text = text.AppendLine($"{depth.Of(" ")}<Function>")
                       .AppendLine($"{(depth + 1).Of(" ")}<Name>")
                       .AppendLine($"{(depth + 2).Of(" ")}{Name.Identifier}")
                       .AppendLine($"{(depth + 1).Of(" ")}</Name>")
                       .AppendLine($"{(depth + 1).Of(" ")}<Parameters>");

            foreach (var parameter in Parameters)
                text = parameter.SerializeToXml(text, depth + 2);

            text = text.AppendLine($"{(depth + 1).Of(" ")}</Parameters>")
                       .AppendLine($"{(depth + 1).Of(" ")}<Statements>");

            foreach (var statements in Statements)
                text = statements.SerializeToXml(text, depth + 2);

            text = text.AppendLine($"{(depth + 1).Of(" ")}</Statements>")
                       .AppendLine($"{depth.Of(" ")}</Function>");

            return text;
        }
    }
}
