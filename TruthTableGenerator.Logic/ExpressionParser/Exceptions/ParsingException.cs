using System;
using TruthTableGenerator.Logic.Lexer;

namespace TruthTableGenerator.Logic.ExpressionParser.Exceptions
{
    [Serializable]
    public class ParsingException : Exception
    {
        public ParserExceptionType ExceptionType { get; set; }
        public Token<TokenType> Token { get; set; }
        public TokenType[] ExpectedTokenTypes { get; set; }

        public ParsingException(ParserExceptionType exceptionType, Token<TokenType> token, TokenType[] expectedTokenTypes)
        {
            ExceptionType = exceptionType;
            Token = token;
            ExpectedTokenTypes = expectedTokenTypes;
        }

        public ParsingException()
        {
        }

        public ParsingException(string message) : base(message)
        {
        }

        public ParsingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ParsingException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}