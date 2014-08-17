using System;
using System.Collections.Concurrent;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TruthTableGenerator.Logic.ExpressionParser;
using TruthTableGenerator.Logic.ExpressionParser.Exceptions;

namespace TruthTableGenerator.Logic.Tests
{
    [TestClass]
    public class TruthTableTests
    {
        [TestMethod]
        public void Constant()
        {
            LogicalExpression logicalExpression = new LogicalExpression("p");

            Assert.AreEqual(2, logicalExpression.TruthTable.Length);

            Assert.AreEqual("0", logicalExpression.TruthTable[0]);
            Assert.AreEqual("1", logicalExpression.TruthTable[1]);
        }

        [TestMethod]
        public void Negation()
        {
            LogicalExpression logicalExpression = new LogicalExpression("-p");

            Assert.AreEqual(2, logicalExpression.TruthTable.Length);

            Assert.AreEqual("10", logicalExpression.TruthTable[0]);
            Assert.AreEqual("01", logicalExpression.TruthTable[1]);
        }

        [TestMethod]
        public void Implication()
        {
            LogicalExpression logicalExpression = new LogicalExpression("p>q");

            Assert.AreEqual(4, logicalExpression.TruthTable.Length);

            Assert.AreEqual("010", logicalExpression.TruthTable[0]);
            Assert.AreEqual("011", logicalExpression.TruthTable[1]);
            Assert.AreEqual("100", logicalExpression.TruthTable[2]);
            Assert.AreEqual("111", logicalExpression.TruthTable[3]);
        }

        [TestMethod]
        public void Conjunction()
        {
            LogicalExpression logicalExpression = new LogicalExpression("p&q");

            Assert.AreEqual(4, logicalExpression.TruthTable.Length);

            Assert.AreEqual("000", logicalExpression.TruthTable[0]);
            Assert.AreEqual("001", logicalExpression.TruthTable[1]);
            Assert.AreEqual("100", logicalExpression.TruthTable[2]);
            Assert.AreEqual("111", logicalExpression.TruthTable[3]);
        }

        [TestMethod]
        public void Disjunction()
        {
            LogicalExpression logicalExpression = new LogicalExpression("p|q");

            Assert.AreEqual(4, logicalExpression.TruthTable.Length);

            Assert.AreEqual("000", logicalExpression.TruthTable[0]);
            Assert.AreEqual("011", logicalExpression.TruthTable[1]);
            Assert.AreEqual("110", logicalExpression.TruthTable[2]);
            Assert.AreEqual("111", logicalExpression.TruthTable[3]);
        }

        [TestMethod]
        public void Expression_With_Implication_Negation_And_Brackets()
        {
            LogicalExpression logicalExpression = new LogicalExpression("p>(-p>q)>-r");

            Assert.AreEqual(8, logicalExpression.TruthTable.Length);

            Assert.AreEqual("01 1000 110", logicalExpression.TruthTable[0]);
            Assert.AreEqual("01 1000 001", logicalExpression.TruthTable[1]);
            Assert.AreEqual("01 1011 110", logicalExpression.TruthTable[2]);
            Assert.AreEqual("01 1011 001", logicalExpression.TruthTable[3]);
            Assert.AreEqual("11 0110 110", logicalExpression.TruthTable[4]);
            Assert.AreEqual("11 0110 001", logicalExpression.TruthTable[5]);
            Assert.AreEqual("11 0111 110", logicalExpression.TruthTable[6]);
            Assert.AreEqual("11 0111 001", logicalExpression.TruthTable[7]);
        }

        [TestMethod]
        public void Complex_Expression_With_All_Operators()
        {
            LogicalExpression logicalExpression = new LogicalExpression("p>(-q>-p&-r)>p>q|-q&-p|(r>s|p&-q)&-r");

            Assert.AreEqual(16, logicalExpression.TruthTable.Length);

            Assert.AreEqual("01 10110110 00101101101 01000010 110", logicalExpression.TruthTable[0]);
            Assert.AreEqual("01 10110110 00101101101 01110010 110", logicalExpression.TruthTable[1]);
            Assert.AreEqual("01 10010001 00101101101 10000010 001", logicalExpression.TruthTable[2]);
            Assert.AreEqual("01 10010001 00101101101 11110010 001", logicalExpression.TruthTable[3]);
            Assert.AreEqual("01 01110110 00111010101 01000001 110", logicalExpression.TruthTable[4]);
            Assert.AreEqual("01 01110110 00111010101 01110001 110", logicalExpression.TruthTable[5]);
            Assert.AreEqual("01 01110001 00111010101 10000001 001", logicalExpression.TruthTable[6]);
            Assert.AreEqual("01 01110001 00111010101 11110001 001", logicalExpression.TruthTable[7]);
            Assert.AreEqual("10 10001010 11100100011 01011110 110", logicalExpression.TruthTable[8]);
            Assert.AreEqual("10 10001010 11100100011 01111110 110", logicalExpression.TruthTable[9]);
            Assert.AreEqual("10 10001001 11000100010 11011110 001", logicalExpression.TruthTable[10]);
            Assert.AreEqual("10 10001001 11000100010 11111110 001", logicalExpression.TruthTable[11]);
            Assert.AreEqual("11 01101010 11111010011 01001001 110", logicalExpression.TruthTable[12]);
            Assert.AreEqual("11 01101010 11111010011 01111001 110", logicalExpression.TruthTable[13]);
            Assert.AreEqual("11 01101001 11111010011 10001001 001", logicalExpression.TruthTable[14]);
            Assert.AreEqual("11 01101001 11111010011 11111001 001", logicalExpression.TruthTable[15]);
        }

        [TestMethod]
        public void Initial_Token_Exception()
        {
            try
            {
                LogicalExpression logicalExpression = new LogicalExpression(")p");
                Assert.Fail("Exception expected");
            }
            catch (ParsingException e)
            {
                Assert.AreEqual(ParserExceptionType.IllegalInitialToken, e.ExceptionType);
                Assert.AreEqual(TokenType.CloseBracket, e.Token.Type);
            }
        }

        [TestMethod]
        public void End_Token_Exception()
        {
            try
            {
                LogicalExpression logicalExpression = new LogicalExpression("p>");
                Assert.Fail("Exception expected");
            }
            catch (ParsingException e)
            {
                Assert.AreEqual(ParserExceptionType.IllegalEndToken, e.ExceptionType);
                Assert.AreEqual(TokenType.Implication, e.Token.Type);
            }
        }

        [TestMethod]
        public void Expected_Token_Exception()
        {
            try
            {
                LogicalExpression logicalExpression = new LogicalExpression("p>->p");
                Assert.Fail("Exception expected");
            }
            catch (ParsingException e)
            {
                Assert.AreEqual(ParserExceptionType.ExpectedToken, e.ExceptionType);
                Assert.AreEqual(TokenType.Implication, e.Token.Type);
            }
        }

        [TestMethod]
        public void Missing_Open_Bracket_Exception()
        {
            try
            {
                LogicalExpression logicalExpression = new LogicalExpression("p>q)");
                Assert.Fail("Exception expected");
            }
            catch (ParsingException e)
            {
                Assert.AreEqual(ParserExceptionType.MissingOpenBracket, e.ExceptionType);
                Assert.AreEqual(TokenType.CloseBracket, e.Token.Type);
            }
        }

        [TestMethod]
        public void Missing_Close_Bracket_Exception()
        {
            try
            {
                LogicalExpression logicalExpression = new LogicalExpression("(p>q");
                Assert.Fail("Exception expected");
            }
            catch (ParsingException e)
            {
                Assert.AreEqual(ParserExceptionType.MissingCloseBracket, e.ExceptionType);
                Assert.AreEqual(TokenType.OpenBracket, e.Token.Type);
            }
        }

        [TestMethod]
        public void Unknown_Token_Exception()
        {
            try
            {
                LogicalExpression logicalExpression = new LogicalExpression("p>.");
                Assert.Fail("Exception expected");
            }
            catch (ParsingException e)
            {
                Assert.AreEqual(ParserExceptionType.UnknownToken, e.ExceptionType);
                Assert.AreEqual(TokenType.Unknown, e.Token.Type);
            }
        }
    }
}