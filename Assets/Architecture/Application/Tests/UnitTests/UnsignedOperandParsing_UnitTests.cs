using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Calc.Application.Tests.UnitTests
{
    using Parsing;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class UnsignedOperandParsing_UnitTests
    {
        [Test]
        [TestCase("0", "", ExpectedResult = "0")]
        [TestCase("123", "", ExpectedResult = "123")]
        public string UnsignedOperandParsing_Input_Passes(string input, string expectedLeftover)
        {
            var candidate = UnsignedOperandParsing.Parse(in input, out var leftover);

            Assert.AreEqual(expectedLeftover, leftover, $"Expected leftover was: \"{expectedLeftover}\", but got: \"{leftover}\"");

            return candidate.Match<string>
            (
                success: static operand => operand.ToString(),
                failure: static failure => failure.Message,
                error: static exception => exception.Message
            );
        }
    }
}
