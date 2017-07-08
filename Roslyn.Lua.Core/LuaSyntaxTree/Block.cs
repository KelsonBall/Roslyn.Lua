using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree
{
    public class Block : Statement
    {
        public Block(IEnumerable<Token> source) : base(source)
        {
            source = source.Crop(1);
            
            Statement statement = null;
            while ((statement = source.Create<Statement>()) != null)
            {
                _statements.Add(statement);
                source = source.Skip(statement.Source.Count());
            }
        }

        public static Block Create(IEnumerable<Token> source)
        {                    
            if (source.First().Source == "do")            
                return new Block(source.TokenRange(TokenType.BlockStart, TokenType.BlockEnd));            
            return null;
        }

        private readonly List<Statement> _statements = new List<Statement>();
        public IEnumerable<Statement> Statements => _statements.AsEnumerable();
    }
}
 