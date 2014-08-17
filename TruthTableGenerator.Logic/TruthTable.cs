using System;
using System.Collections.Generic;
using System.Linq;
using TruthTableGenerator.Logic.ExpressionParser;
using TruthTableGenerator.Logic.Lexer;

namespace TruthTableGenerator.Logic
{
    public class TruthTable
    {
        // Size of the truth table.
        public int Length { get; private set; }

        // Position in the expression string of the final result.
        public int ResultPosition { get; set; }

        private readonly string _expression;
        private readonly Token<TokenType>[] _postfixTokens;

        // Contains all the distinct constants in the expression.
        private readonly char[] _constants;

        // Maps a constant (an ascii char) with its corresponding value for a particular row.
        private readonly Dictionary<char, bool> _constantsValues;

        public TruthTable(string expression, Token<TokenType>[] postfixTokens)
        {
            ResultPosition = postfixTokens.Last().Position;

            _expression = expression;
            _postfixTokens = postfixTokens;

            // I know the Token value would be a single letter representing a constant, so its length would be 1.
            _constants = _postfixTokens.Where(t => t.Type == TokenType.Constant).Select(t => t.Value[0]).Distinct().ToArray();

            _constantsValues = new Dictionary<char, bool>();
            foreach (char constant in _constants)
            {
                _constantsValues.Add(constant, false);
            }

            Length = (int)Math.Pow(2, _constants.Length);
        }

        public string this[int row]
        {
            get
            {
                if (row >= Length)
                {
                    throw new IndexOutOfRangeException(string.Format("The row number must be between 0 and {0}", Length - 1));
                }
                return EvaluateRow(row);
            }
        }

        private string EvaluateRow(int row)
        {
            Stack<bool> operands = new Stack<bool>();
            char[] result = new char[_expression.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ' ';
            }

            SetConstantsValuesForRow(_constantsValues, row);

            foreach (Token<TokenType> token in _postfixTokens)
            {
                bool value = false;

                switch (token.Type)
                {
                    case TokenType.Constant:
                    {
                        value = _constantsValues[token.Value[0]];

                        operands.Push(value);
                        break;
                    }
                    case TokenType.Negation:
                    {
                        value = !operands.Pop();

                        operands.Push(value);
                        break;
                    }
                    case TokenType.Implication:
                    {
                        bool secondOperand = operands.Pop();
                        bool firstOperand = operands.Pop();
                        value = !(firstOperand && !secondOperand);

                        operands.Push(value);
                        break;
                    }
                    case TokenType.Conjunction:
                    {
                        bool secondOperand = operands.Pop();
                        bool firstOperand = operands.Pop();
                        value = firstOperand && secondOperand;

                        operands.Push(value);
                        break;                        
                    }
                    case TokenType.Disjunction:
                    {
                        bool secondOperand = operands.Pop();
                        bool firstOperand = operands.Pop();
                        value = firstOperand || secondOperand;

                        operands.Push(value);
                        break;
                    }
                }
                result[token.Position] = value ? '1' : '0';
            }
            return new string(result);
        }

        private void SetConstantsValuesForRow(Dictionary<char, bool> constants, int row)
        {
            /* The binary representation of the row number gives me the values
             * that constants should have (0 or 1) for this particulary row. A little
             * bit shifting allows me to extract the values for every constant. */
            for (int i = 0; i < _constants.Length; i++)
            {
                constants[_constants[_constants.Length - i - 1]] = ((row >> i) & 1) == 1;
            }
        }
    }
}