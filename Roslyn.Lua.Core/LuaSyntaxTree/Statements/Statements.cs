using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public abstract class Statement : LuaSyntaxNode
    {
        protected Statement(IEnumerable<Token> source) : base(source)
        {            
        }
    }

    public class DoStatement : Statement
    {
        public DoStatement(IEnumerable<Token> source) : base(source)
        {
            var local = source;
            while (local.Any())
            {
                var statement = source.Create<Statement>();
                local = local.Skip(statement.Source.Count());
                _statements.Add(statement);
            }
        }

        public static DoStatement Create(IEnumerable<Token> source)
        {
            if (source.First().Source == "do")
                return new DoStatement(source.TokenRange(TokenType.BlockStart, TokenType.BlockEnd));
            return null;
        }

        private readonly List<Statement> _statements = new List<Statement>();
        public IEnumerable<Statement> Statements => _statements.AsEnumerable();
    }    
}
