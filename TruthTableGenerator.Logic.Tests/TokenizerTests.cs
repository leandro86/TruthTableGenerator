using Microsoft.VisualStudio.TestTools.UnitTesting;
using TruthTableGenerator.Logic.Lexer;
using TruthTableGenerator.Logic.Lexer.Exceptions;

namespace TruthTableGenerator.Logic.Tests
{
    [TestClass]
    public class TokenizerTest
    {
        private enum Tokentype
        {
            Identifier,
            Number
        };

        private Rule<Tokentype>[] _rules;

        [TestInitialize]
        public void Initialize()
        {
            _rules = CreateRules();
        }

        [TestMethod]
        public void Simple_Identifier()
        {
            string expression = "anIdentifier";

            Tokenizer<Tokentype> tokenizer = new Tokenizer<Tokentype>(expression, _rules);
            Token<Tokentype>[] tokens = tokenizer.Tokenize();

            Assert.AreEqual(1, tokens.Length);
            Assert.AreEqual(Tokentype.Identifier, tokens[0].Type);
            Assert.AreEqual(0, tokens[0].Position);
            Assert.AreEqual("anIdentifier", tokens[0].Value);
        }

        [TestMethod]
        public void Simple_Number()
        {
            string expression = "59842";

            Tokenizer<Tokentype> tokenizer = new Tokenizer<Tokentype>(expression, _rules);
            Token<Tokentype>[] tokens = tokenizer.Tokenize();

            Assert.AreEqual(1, tokens.Length);
            Assert.AreEqual(Tokentype.Number, tokens[0].Type);
            Assert.AreEqual(0, tokens[0].Position);
            Assert.AreEqual("59842", tokens[0].Value);
        }

        [TestMethod]
        public void Simple_Expression()
        {
            string expression = "abc 564 a b";

            Tokenizer<Tokentype> tokenizer = new Tokenizer<Tokentype>(expression, _rules);
            Token<Tokentype>[] tokens = tokenizer.Tokenize();

            Assert.AreEqual(4, tokens.Length);

            Assert.AreEqual(Tokentype.Identifier, tokens[0].Type);
            Assert.AreEqual(0, tokens[0].Position);
            Assert.AreEqual("abc", tokens[0].Value);

            Assert.AreEqual(Tokentype.Number, tokens[1].Type);
            Assert.AreEqual(4, tokens[1].Position);
            Assert.AreEqual("564", tokens[1].Value);

            Assert.AreEqual(Tokentype.Identifier, tokens[2].Type);
            Assert.AreEqual(8, tokens[2].Position);
            Assert.AreEqual("a", tokens[2].Value);

            Assert.AreEqual(Tokentype.Identifier, tokens[3].Type);
            Assert.AreEqual(10, tokens[3].Position);
            Assert.AreEqual("b", tokens[3].Value);
        }

        [TestMethod]
        public void No_Matching_Rule_Exception()
        {
            string expression = "abc 123 ? a";

            Tokenizer<Tokentype> tokenizer = new Tokenizer<Tokentype>(expression, _rules);

            try
            {
                Token<Tokentype>[] tokens = tokenizer.Tokenize();
                Assert.Fail("Exception expected");
            }
            catch (MatchingRuleException e)
            {
                Assert.AreEqual(8, e.Position);
            }
        }

        private Rule<Tokentype>[] CreateRules()
        {
            return new Rule<Tokentype>[]
            {
                Rule<Tokentype>.CreateRule(@"[a-zA-Z]+", Tokentype.Identifier),
                Rule<Tokentype>.CreateRule(@"\d+", Tokentype.Number),
            };
        }
    }
}