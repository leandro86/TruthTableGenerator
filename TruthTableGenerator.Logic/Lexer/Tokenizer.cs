using System.Collections.Generic;
using System.Text.RegularExpressions;
using TruthTableGenerator.Logic.Lexer.Exceptions;

namespace TruthTableGenerator.Logic.Lexer
{
    public class Tokenizer<T>
    {
        private readonly string _expression;
        private readonly Rule<T>[] _rules;

        public Tokenizer(string expression, Rule<T>[] rules)
        {
            _expression = expression;
            _rules = rules;
        }

        public Token<T>[] Tokenize()
        {
            List<Token<T>> tokens = new List<Token<T>>();

            int pos = 0;
            bool foundInvalidToken = false;

            while (pos < _expression.Length && !foundInvalidToken)
            {
                // skip all whitespaces
                if (char.IsWhiteSpace(_expression[pos]))
                {
                    pos++;
                    continue;
                }

                Rule<T> matchingRule = null;
                Match match = Match.Empty;

                // Try to match any regex at the current position.
                int i = 0;
                while (i < _rules.Length && matchingRule == null)
                {
                    match = _rules[i].Regex.Match(_expression, pos);

                    if (match.Success)
                    {
                        matchingRule = _rules[i];
                    }

                    i += 1;
                }

                if (matchingRule != null)
                {
                    tokens.Add(new Token<T>(match.Value, match.Index, matchingRule.TokenType));
                    pos += match.Length;
                }
                else
                {
                    foundInvalidToken = true;
                }
            }

            if (foundInvalidToken)
            {
                throw new MatchingRuleException(pos);
            }

            return tokens.ToArray();
        }
    }
}