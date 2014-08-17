using System;
using System.Collections.Generic;
using System.Linq;
using TruthTableGenerator.Logic.ExpressionParser.Exceptions;
using TruthTableGenerator.Logic.Lexer;
using TruthTableGenerator.Logic.Lexer.Exceptions;

namespace TruthTableGenerator.Logic.ExpressionParser
{
    public class Parser
    {
        private static readonly Rule<TokenType>[] TokenRules =
        {
            Rule<TokenType>.CreateRule(@"[a-zA-Z]", TokenType.Constant),
            Rule<TokenType>.CreateRule(@"-", TokenType.Negation),
            Rule<TokenType>.CreateRule(@">", TokenType.Implication),
            Rule<TokenType>.CreateRule(@"\(", TokenType.OpenBracket),
            Rule<TokenType>.CreateRule(@"\)", TokenType.CloseBracket),
            Rule<TokenType>.CreateRule(@"&", TokenType.Conjunction),
            Rule<TokenType>.CreateRule(@"\|", TokenType.Disjunction),
        };

        // Every possible token type has an array which indicates what are the only valid tokens that can follow it
        private static readonly Dictionary<TokenType, TokenType[]> ParsingRules = new Dictionary<TokenType, TokenType[]>()
        {
            {TokenType.CloseBracket, new[] {TokenType.Implication, TokenType.CloseBracket, TokenType.Conjunction, TokenType.Disjunction}},
            {TokenType.Constant, new[] {TokenType.Implication, TokenType.CloseBracket, TokenType.Conjunction, TokenType.Disjunction}},
            {TokenType.Implication, new[] {TokenType.Constant, TokenType.OpenBracket, TokenType.Negation}},
            {TokenType.Negation, new[] {TokenType.Constant, TokenType.OpenBracket, TokenType.Negation}},
            {TokenType.OpenBracket, new[] {TokenType.Negation, TokenType.OpenBracket, TokenType.Constant}},
            {TokenType.Conjunction, new[] {TokenType.Constant, TokenType.OpenBracket, TokenType.Negation}},
            {TokenType.Disjunction, new[] {TokenType.Constant, TokenType.OpenBracket, TokenType.Negation}},
        };

        private static readonly Dictionary<TokenType, int> OperatorsPrecedences = new Dictionary<TokenType, int>()
        {
            {TokenType.Negation, 1},
            {TokenType.Conjunction, 2},
            {TokenType.Disjunction, 3},
            {TokenType.Implication, 4},
            {TokenType.OpenBracket, 5},
            {TokenType.CloseBracket, 5}
        };

        private readonly string _infixExpression;
        private Token<TokenType>[] _infixTokens;
        private Token<TokenType>[] _postfixTokens;

        public Parser(string expression)
        {
            _infixExpression = expression;
        }

        public void Parse()
        {
            Tokenizer<TokenType> tokenizer = new Tokenizer<TokenType>(_infixExpression, TokenRules);

            try
            {
                _infixTokens = tokenizer.Tokenize();
            }
            catch (MatchingRuleException e)
            {
                throw new ParsingException(ParserExceptionType.UnknownToken,
                                           new Token<TokenType>(_infixExpression[e.Position].ToString(), e.Position, TokenType.Unknown),
                                           ParsingRules.Keys.ToArray());
            }

            Stack<Token<TokenType>> openBrackets = new Stack<Token<TokenType>>();

            Token<TokenType> currentToken = _infixTokens[0];

            if (currentToken.Type == TokenType.CloseBracket || currentToken.Type == TokenType.Implication)
            {
                throw new ParsingException(ParserExceptionType.IllegalInitialToken, currentToken,
                                           new[] {TokenType.Constant, TokenType.Negation, TokenType.OpenBracket});
            }

            if (currentToken.Type == TokenType.OpenBracket)
            {
                openBrackets.Push(currentToken);
            }

            Token<TokenType> previousToken = currentToken;

            for (int i = 1; i < _infixTokens.Length; i++)
            {
                currentToken = _infixTokens[i];

                if (!ParsingRules[previousToken.Type].Contains(currentToken.Type))
                {
                    throw new ParsingException(ParserExceptionType.ExpectedToken, currentToken, ParsingRules[previousToken.Type]);
                }

                if (currentToken.Type == TokenType.OpenBracket)
                {
                    openBrackets.Push(currentToken);
                }
                else if (currentToken.Type == TokenType.CloseBracket)
                {
                    if (openBrackets.Count == 0)
                    {
                        throw new ParsingException(ParserExceptionType.MissingOpenBracket, currentToken, new[] {TokenType.OpenBracket});
                    }

                    openBrackets.Pop();
                }

                previousToken = currentToken;
            }

            if (currentToken.Type != TokenType.Constant && currentToken.Type != TokenType.CloseBracket)
            {
                throw new ParsingException(ParserExceptionType.IllegalEndToken, currentToken, ParsingRules[currentToken.Type]);
            }

            if (openBrackets.Count != 0)
            {
                throw new ParsingException(ParserExceptionType.MissingCloseBracket, openBrackets.Peek(), new[] {TokenType.CloseBracket});
            }
        }

        public IEnumerable<Token<TokenType>> GetInfixTokens()
        {
            if (_infixTokens == null)
            {
                throw new Exception("Parse must be called first.");
            }

            return _infixTokens;
        }

        public IEnumerable<Token<TokenType>> GetPostfixTokens()
        {
            if (_infixTokens == null)
            {
                throw new Exception("Parse must be called first.");
            }

            if (_postfixTokens == null)
            {
                _postfixTokens = ConvertToPostfix(_infixTokens).ToArray();
            }

            return _postfixTokens;
        }

        private IEnumerable<Token<TokenType>> ConvertToPostfix(IEnumerable<Token<TokenType>> infixTokens)
        {
            Stack<Token<TokenType>> operators = new Stack<Token<TokenType>>();
            List<Token<TokenType>> postfixTokens = new List<Token<TokenType>>();

            foreach (Token<TokenType> token in infixTokens)
            {
                switch (token.Type)
                {
                    case TokenType.Constant:
                    {
                        postfixTokens.Add(token);
                        break;
                    }
                    case TokenType.Negation:
                    {
                        operators.Push(token);
                        break;
                    }
                    case TokenType.OpenBracket:
                    {
                        operators.Push(token);
                        break;
                    }
                    case TokenType.Implication:
                    case TokenType.Conjunction:
                    case TokenType.Disjunction:
                    {
                        while (operators.Count != 0 && (OperatorsPrecedences[operators.Peek().Type] <= OperatorsPrecedences[token.Type]))
                        {
                            postfixTokens.Add(operators.Pop());
                        }
                        operators.Push(token);
                        break;
                    }
                    case TokenType.CloseBracket:
                    {
                        while (operators.Peek().Type != TokenType.OpenBracket)
                        {
                            postfixTokens.Add(operators.Pop());
                        }
                        operators.Pop();
                        break;
                    }
                }
            }

            while (operators.Count != 0)
            {
                postfixTokens.Add(operators.Pop());
            }

            return postfixTokens;
        }
    }
}