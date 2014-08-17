using System;
using System.Runtime.Serialization;

namespace TruthTableGenerator.Logic.Lexer.Exceptions
{
    [Serializable]
    public class MatchingRuleException : Exception
    {
        public int Position { get; set; }

        public MatchingRuleException(int position)
        {
            Position = position;
        }

        public MatchingRuleException(string message) : base(message)
        {
        }

        public MatchingRuleException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MatchingRuleException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}