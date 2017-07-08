using Roslyn.Lua.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public class Program : LuaSyntaxNode
    {
        public Program(IEnumerable<Token> source) : base(source)
        {
            Statement statement = null;
            while ((statement = source.Create<Statement>()) != null)
            {
                _statements.Add(statement);
                source = source.Skip(statement.Source.Count());
            }
        }

        public static Program Create(IEnumerable<Token> source)
        {
            int count = 0;
            Statement statement = null;
            while ((statement = source.Skip(count).Create<Statement>()) != null)
                count += statement.Source.Count();
            
            return new Program(source.Take(count));
        }

        private readonly List<Statement> _statements = new List<Statement>();
        public IEnumerable<Statement> Statements => _statements.AsEnumerable();

        public override StringBuilder SerializeToXml(StringBuilder text, int depth = 0)
        {
            text = text.AppendLine($"{depth.Of(" ")}<Program>");
            foreach (var statement in Statements)
                text = statement.SerializeToXml(text, depth + 1);
            text = text.AppendLine($"{depth.Of(" ")}</Program>");
            return text;
        }
    }
}
