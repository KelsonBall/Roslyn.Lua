using Roslyn.Lua.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roslyn.Lua.Core.LuaSyntaxTree.OperationExpressions
{
    public class BinaryOperandExpression : Expression
    {
        public BinaryOperandExpression(IEnumerable<Token> source) : base(source)
        {
            LeftOperand =
                source
                    .TakeWhile(t => t.Type != TokenType.BinaryOperator)
                    .Create<Expression>();
            source = source.Skip(LeftOperand.Source.Count());
            Operation = source.First().Source;
            source = source.Skip(1);
            RightOperand = source.Create<Expression>();

        }

        public static BinaryOperandExpression Create(IEnumerable<Token> source)
        {
            if (!source.Any(t => t.Type == TokenType.BinaryOperator))
                return null;

            var leftSource = source.TakeWhile(t => t.Type != TokenType.BinaryOperator);

            var left = leftSource.Create<Expression>();
            if (left == null || leftSource.Count() < left.Source.Count())
                return null;

            source = source.Skip(left.Source.Count());
            var operand = source.First();
            if (operand.Type != TokenType.BinaryOperator)
                return null;

            source = source.Skip(1);

            var right = source.Create<Expression>();
            if (left != null && right != null)
                return new BinaryOperandExpression(left.Source.And(operand).And(right.Source));
            return null;
        }

        public string Operation { get; }

        public Expression LeftOperand { get; }

        public Expression RightOperand { get; }

        public override StringBuilder SerializeToXml(StringBuilder text, int depth = 0)
        {
            text = text.AppendLine($"{depth.Of(" ")}<BinaryOperandExpression>")
                       .AppendLine($"{(depth + 1).Of(" ")}<Left>");
            text = LeftOperand.SerializeToXml(text, depth + 2)
                        .AppendLine($"{(depth + 1).Of(" ")}</Left>");
            text = text.AppendLine($"{(depth + 1).Of(" ")}<Operand>{Operation}</Operand>");                    
            text = text.AppendLine($"{(depth + 1).Of(" ")}<Right>");
            text = RightOperand.SerializeToXml(text, depth + 2)
                        .AppendLine($"{(depth + 1).Of(" ")}</Right>");
            text = text.AppendLine($"{depth.Of(" ")}</BinaryOperandExpression>");
            return text;
        }
    }
}
