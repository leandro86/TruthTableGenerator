using System;
using System.Collections.Generic;
using System.Linq;
using TruthTableGenerator.Logic.ExpressionParser;
using TruthTableGenerator.Logic.Lexer;

namespace TruthTableGenerator.Logic
{
    public class LogicalExpression
    {
        public TruthTable TruthTable { get; private set; }
        public string Expression { get; private set; }

        private readonly Token<TokenType>[] _infixTokens;
        private readonly Token<TokenType>[] _postfixTokens;

        public LogicalExpression(string expression)
        {
            Expression = expression.ToLower();
            Parser parser = new Parser(Expression);

            try
            {
                parser.Parse();
            }
            catch (Exception e)
            {
                throw;
            }

            _infixTokens = parser.GetInfixTokens().ToArray();
            _postfixTokens = parser.GetPostfixTokens().ToArray();

            TruthTable = new TruthTable(Expression, _postfixTokens);
        }

        public IEnumerable<Tuple<TokenType, char, int>> GetInfixTokens()
        {
            foreach (Token<TokenType> token in _infixTokens)
            {
                yield return new Tuple<TokenType, char, int>(token.Type, token.Value[0], token.Position);
            }
        }

        public BinaryNode GenerateExpressionTree()
        {
            Stack<BinaryNode> operands = new Stack<BinaryNode>();

            foreach (Token<TokenType> token in _postfixTokens)
            {
                switch (token.Type)
                {
                    case TokenType.Constant:
                    {
                        BinaryNode node = new BinaryNode() {Value = token.Value[0]};
                        operands.Push(node);
                        break;
                    }
                    case TokenType.Negation:
                    {
                        BinaryNode node = new BinaryNode() {Value = token.Value[0], Left = operands.Pop()};
                        operands.Push(node);
                        break;
                    }
                    case TokenType.Implication:
                    case TokenType.Conjunction:
                    case TokenType.Disjunction:
                    {
                        BinaryNode node = new BinaryNode() {Value = token.Value[0], Right = operands.Pop(), Left = operands.Pop()};
                        operands.Push(node);
                        break;
                    }
                }
            }
            return operands.Peek();
        }
    }
}