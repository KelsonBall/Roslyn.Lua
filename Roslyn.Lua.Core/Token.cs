using System;
using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core
{
    public class Token
    {
        public TokenType Type { get; }
        public string Source { get; }

        private Token(TokenType type, string source)
        {
            Type = type;
            Source = source.Trim();
        }

        private static readonly string[] whitespace = new[] { " ", "\t", "\r", "\n" };

        private static readonly string[] endpunctuation = new[] { ",", "}", ")", "]" };

        private static string integerLiteral(string s)
        {
            int last = s.IndexOfAny(whitespace.And(endpunctuation));
            if (last < 0)
                return null;
            string relevant = s.Trim().Substring(0, last);
            if (int.TryParse(relevant, out int _))
                return relevant;
            return null;
        }

        private static string numberLiteral(string s)
        {
            int last = s.IndexOfAny(whitespace.And(endpunctuation));
            if (last < 0)
                return null;
            string relevant = s.Trim().Substring(0, last);
            if (double.TryParse(relevant, out double _))
                return relevant;
            return null;
        }

        private static string stringLiteral(string s)
        {
            if (!(s.StartsWith("'") || s.StartsWith("\"") || s.StartsWith("[[")))
                return null;

            string ender = "\"";
            if (s.StartsWith("'"))
                ender = "'";
            else if (s.StartsWith("[["))
                ender = "]]";

            int stretch = 1;
            while (!s.Substring(stretch).StartsWith(ender))
                stretch++;

            return s.Substring(0, stretch + 1);
        }

        private static string booleanLiteral(string s)
        {
            return s.StartsWith("false") ? "false" : (s.StartsWith("true") ? "true" : null);
        }

        private static Func<string, string> matcher(string v) => s => s.StartsWith(v) ? v : null;

        private static Func<string, string> wordMatcher(string v) => s => s.StartsWith(v + " ")
                                                                       || s.StartsWith(v + '\r')
                                                                       || s.StartsWith(v + '\n')
                                                                       || s.StartsWith(v + '\t')
                                                                       ? v : null;
        
        private static readonly (Func<string, string> pattern, TokenType type)[] tokenSource =
        new(Func<string, string> pattern, TokenType type)[]
            {
                (wordMatcher("function"), TokenType.BlockStart),
                (wordMatcher("elseif"), TokenType.Keyword),
                (wordMatcher("repeat"), TokenType.BlockStart),
                (wordMatcher("return"), TokenType.Return),
                (wordMatcher("break"), TokenType.Keyword),                
                (wordMatcher("local"), TokenType.Keyword),                                                                                
                (wordMatcher("until"), TokenType.BlockEnd),
                (wordMatcher("while"), TokenType.Keyword),
                (wordMatcher("then"), TokenType.BlockStart),
                (wordMatcher("else"), TokenType.Keyword),
                (wordMatcher("for"), TokenType.Keyword),
                (wordMatcher("end"), TokenType.BlockEnd),
                (wordMatcher("not"), TokenType.UnaryOperator),
                (wordMatcher("in"), TokenType.Keyword),
                (wordMatcher("or"), TokenType.BinaryOperator),
                (wordMatcher("do"), TokenType.BlockStart),
                (wordMatcher("if"), TokenType.Keyword),
                (matcher("nil"), TokenType.NilLiteral),
                (matcher(","), TokenType.Delimiter),
                (matcher("."), TokenType.AccessOperator),
                (matcher(":"), TokenType.AccessOperator),
                (matcher(".."), TokenType.BinaryOperator),
                (matcher("..."), TokenType.Identifier),
                (matcher("{"), TokenType.TableLiteralStart),
                (matcher("}"), TokenType.TableLiteralEnd),
                (matcher("("), TokenType.ParenStart),
                (matcher(")"), TokenType.ParenEnd),
                (matcher("["), TokenType.IndexerStart),
                (matcher("]"), TokenType.IndexerEnd),
                (matcher("~"), TokenType.UnaryOperator),
                (matcher("="), TokenType.AssignmentOperator),
                (matcher("~="), TokenType.BinaryOperator),
                (matcher("=="), TokenType.BinaryOperator),
                (matcher(">="), TokenType.BinaryOperator),
                (matcher("<="), TokenType.BinaryOperator),
                (matcher("*"), TokenType.BinaryOperator),
                (matcher("/"), TokenType.BinaryOperator),
                (matcher("+"), TokenType.BinaryOperator),
                (matcher("-"), TokenType.MinusOperator),
                (matcher("#"), TokenType.UnaryOperator),
                (matcher(">"), TokenType.BinaryOperator),
                (matcher("<"), TokenType.BinaryOperator),
                (integerLiteral, TokenType.IntegerLiteral),
                (numberLiteral, TokenType.NumberLiteral),
                (stringLiteral, TokenType.StringLiteral),
                (booleanLiteral, TokenType.BooleanLiteral),
            };

        public static (Token token, string remainder) PopToken(string source)
        {
            int index = 0;
            string remaining(int plus = 0) => source.Substring(index + plus);

            while (remaining().StartsWithAnItemOf(whitespace))
                index++;

            if (index > 0)
                return (new Token(TokenType.Whitespace, " "), remaining());

            if (string.IsNullOrEmpty(source))
                return (null, null);

            Func<string, IEnumerable<(Func<string, string> pattern, TokenType type)>> tokens = 
                s =>
                    tokenSource
                        .Select(t => (t.pattern(s)?.Length ?? 0, t))
                        .Where(tt => tt.Item1 > 0)
                        .OrderByDescending(tt => tt.Item1)
                        .Select(tt => tt.Item2);

            int stretch = 0;
            if (!tokens(source).Any())
            {                
                do                
                    stretch++;                
                while (stretch < source.Length && !tokens(source.Substring(stretch)).Any());
                return (new Token(TokenType.Identifier, source.Substring(0, stretch)), source.Substring(stretch));
            }

            var token = tokens(source).FirstOrDefault();
            if (token.pattern != null)
                return (new Token(token.type, token.pattern(source)), remaining(token.pattern(source).Length));
            else            
                if (token.type == TokenType.BooleanLiteral)                    
                    return (new Token(TokenType.BooleanLiteral, token.pattern(source)), source.Substring(token.pattern(source).Length));                    
                else                
                    return (new Token(token.type, source.Substring(0, token.pattern(source).Length)), remaining(token.pattern(source).Length));            
        }

        public override string ToString()
        {
            return $"[{Source},{Type}]";
            
        }
    }
}
