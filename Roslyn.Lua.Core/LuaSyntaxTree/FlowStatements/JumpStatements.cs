using Roslyn.Lua.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roslyn.Lua.Core.LuaSyntaxTree.FlowStatements
{
    public class ReturnStatement : Statement
    {
        public ReturnStatement(IEnumerable<Token> source, IEnumerable<Expression> expressions) : base(source)
        {
            Returns = expressions;
        }

        public static ReturnStatement Create(IEnumerable<Token> source)
        {
            if (source.First().Source != "return")
                return null;

            var tokens = new List<Token>() { source.First() };
            source = source.Skip(1);
            var expressions = new List<Expression>();
            Expression expression = null;
            while ((expression = source.Create<Expression>()) != null)
            {
                expressions.Add(expression);
                tokens.AddRange(expression.Source);
                source = source.Skip(expression.Source.Count());
                if (!source.Any() || source.First().Type != TokenType.Delimiter)
                    break;
                source = source.Skip(1);
            }

            return new ReturnStatement(tokens, expressions);            
        }
        
        public IEnumerable<Expression> Returns { get; private set; }

        public override StringBuilder SerializeToXml(StringBuilder text, int depth = 0)
        {
            text = text.AppendLine($"{depth.Of(" ")}<Return>");
            foreach (var expr in Returns)
                text = expr.SerializeToXml(text, depth + 1);
            text = text.AppendLine($"{depth.Of(" ")}</Return>");
            return text;
        }
    }

    public class BreakStatement : Statement
    {
        public BreakStatement(IEnumerable<Token> source) : base(source)
        {            
        }

        public static BreakStatement Create(IEnumerable<Token> source)
        {
            if (source.First().Source == "break")
                return new BreakStatement(source.Take(1));
            return null;
        }

        public override StringBuilder SerializeToXml(StringBuilder text, int depth = 0)
        {
            return text.AppendLine($"{depth.Of(" ")}<Break />");
        }
    }

    public class ContinueStatement : Statement
    {
        public ContinueStatement(IEnumerable<Token> source) : base(source)
        {
        }

        public static ContinueStatement Create(IEnumerable<Token> source)
        {
            if (source.First().Source == "continue")
                return new ContinueStatement(source.Take(1));
            return null;
        }

        public override StringBuilder SerializeToXml(StringBuilder text, int depth = 0)
        {
            return text.AppendLine($"{depth.Of(" ")}<Continue />");
        }
    }
}
