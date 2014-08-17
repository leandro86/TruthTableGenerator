namespace TruthTableGenerator.Logic.Lexer
{
    public class Token<T>
    {
        public string Value { get; set; }
        public int Position { get; set; }
        public T Type { get; set; }

        public Token(string value, int position, T type)
        {
            Value = value;
            Position = position;
            Type = type;
        }
    }
}
