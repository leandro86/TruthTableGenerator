using System.Text.RegularExpressions;

namespace TruthTableGenerator.Logic.Lexer
{
    public class Rule<T>
    {
        public Regex Regex { get; set; }
        public T TokenType { get; set; }

        public static Rule<T> CreateRule(string regex, T tokenType)
        {
            /* we need to append the anchor \G so the tokenizer can start to match exactly at the 
             * end of the previous match */
            return new Rule<T>(regex.Insert(0, @"\G"), tokenType);
        }

        private Rule(string regex, T tokenType)
        {
            Regex = new Regex(regex);
            TokenType = tokenType;
        }
    }
}
