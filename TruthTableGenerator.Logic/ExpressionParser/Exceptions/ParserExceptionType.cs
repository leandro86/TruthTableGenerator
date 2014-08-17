namespace TruthTableGenerator.Logic.ExpressionParser.Exceptions
{
    public enum ParserExceptionType
    {
        IllegalInitialToken,
        IllegalEndToken,
        ExpectedToken,
        MissingOpenBracket,
        MissingCloseBracket,
        UnknownToken,
    }
}
