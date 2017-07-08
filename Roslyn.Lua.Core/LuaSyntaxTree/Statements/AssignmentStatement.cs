using Roslyn.Lua.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public class AssignmentStatement : Statement
    {
        public AssignmentStatement(IEnumerable<Token> source) : base(source)
        {
            IdentifierExpression currentIdentifier = null;
            while ((currentIdentifier = source.Create<IdentifierExpression>()) != null)
            {
                _idenfiers.Add(currentIdentifier);
                source = source.Skip(currentIdentifier.Source.Count());
                if (!source.Any() || source.First().Type != TokenType.Delimiter)
                    break;
                source = source.Skip(1);
            }
            source = source.SkipWhile(t => t.Type != TokenType.AssignmentOperator).Skip(1);
            Expression currentExpression = null;
            while ((currentExpression = source.Create<Expression>()) != null)
            {
                _expressions.Add(currentExpression);
                source = source.Skip(currentExpression.Source.Count());
                if (!source.Any() || source.First().Type != TokenType.Delimiter)
                    break;
                source = source.Skip(1);
            }
        }

        public static AssignmentStatement Create(IEnumerable<Token> source)
        {
            if (!source.Any(t => t.Type == TokenType.AssignmentOperator))
                return null;

            var tokens = new List<Token>();
            tokens.AddRange(source.TakeWhile(t => t.Type != TokenType.AssignmentOperator));
            source = source.SkipWhile(t => t.Type != TokenType.AssignmentOperator);
            tokens.Add(source.First());
            source = source.Skip(1);

            Expression expr = null;
            while ((expr = source.Create<Expression>()) != null)
            {
                source = source.Skip(expr.Source.Count());
                tokens.AddRange(expr.Source);
                if (!source.Any() || source.First().Type != TokenType.Delimiter)
                    break;
                tokens.Add(source.First());
                source = source.Skip(1);
            }
            return new AssignmentStatement(tokens);
        }

        private readonly List<IdentifierExpression> _idenfiers = new List<IdentifierExpression>();
        public IEnumerable<IdentifierExpression> SetTo => _idenfiers.AsEnumerable();
        private readonly List<Expression> _expressions = new List<Expression>();
        public IEnumerable<Expression> SetFrom => _expressions.AsEnumerable();

        public override StringBuilder SerializeToXml(StringBuilder text, int depth = 0)
        {
            text = text.AppendLine($"{depth.Of(" ")}<Assignment>");
            text = text.AppendLine($"{(depth + 1).Of(" ")}<Sets>");
            foreach (var ident in SetTo)
                text = ident.SerializeToXml(text, depth + 2);
            text = text.AppendLine($"{(depth + 1).Of(" ")}</Sets>");
            text = text.AppendLine($"{(depth + 1).Of(" ")}<To>");
            foreach (var expr in SetFrom)
                text = expr.SerializeToXml(text, depth + 2);
            text = text.AppendLine($"{(depth + 1).Of(" ")}</To>");
            text = text.AppendLine($"{depth.Of(" ")}</Assignment>");
            return text;
        }
    }
}
