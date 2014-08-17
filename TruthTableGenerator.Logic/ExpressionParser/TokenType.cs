namespace TruthTableGenerator.Logic.ExpressionParser
{
    public enum TokenType
    {
        Constant,
        Negation,
        OpenBracket,
        CloseBracket,
        Implication,
        Conjunction,
        Disjunction,
        Unknown,
    }
}