using System;
using System.Collections.Generic;
using System.Linq;

namespace Roslyn.Lua.Core.LuaSyntaxTree.LiteralExpressions
{
    public class StringLiteralExpression : Expression
    {
        public StringLiteralExpression(IEnumerable<Token> source) : base(source)
        {
            Value = source.Single().Source;

        }
        
        public static StringLiteralExpression Specifies(IEnumerable<Token> source)
        {
            if (source.First().Type == TokenType.StringLiteral)
                return new StringLiteralExpression(source.Take(1));
            return null;
        }

        public string Value { get; }
    }

    public class IntegerLiteralExpression : Expression
    {
        public IntegerLiteralExpression(IEnumerable<Token> source) : base(source)
        {
            Value = int.Parse(source.Single().Source);
        }

        public static IntegerLiteralExpression Specifies(IEnumerable<Token> source)
        {
            if (source.First().Type == TokenType.IntegerLiteral)
                return new IntegerLiteralExpression(source.Take(1));
            return null;
        }

        public int Value { get; }
    }

    public class NumberLiteralExpression : Expression
    {
        public NumberLiteralExpression(IEnumerable<Token> source) : base(source)
        {
            Value = double.Parse(source.Single().Source);
        }

        public static NumberLiteralExpression Specifies(IEnumerable<Token> source)
        {
            if (source.First().Type == TokenType.NumberLiteral)
                return new NumberLiteralExpression(source.Take(1));
            return null;
        }

        public double Value { get; }
    }

    public class BooleanLiteralExpression : Expression
    {
        public BooleanLiteralExpression(IEnumerable<Token> source) : base(source)
        {
            Value = source.Single().Source.StartsWith("true");
        }

        public static BooleanLiteralExpression Specifies(IEnumerable<Token> source)
        {
            if (source.First().Type == TokenType.BooleanLiteral)
                return new BooleanLiteralExpression(source.Take(1));
            return null;
        }

        public bool Value { get; }
    }

    public class NilExpression : Expression
    {
        public NilExpression(IEnumerable<Token> source) : base(source)
        {            
        }

        public static NilExpression Specifies(IEnumerable<Token> source)
        {
            if (source.First().Type == TokenType.NilLiteral)
                return new NilExpression(source.Take(1));
            return null;
        }
    }
}
