using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Calc.Application.Tests.FunctionalTests
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class Calculator_FunctionalTests
    {
        [Test]
        [TestCase("0+0", ExpectedResult = "0+0=0")]
        [TestCase("1+0", ExpectedResult = "1+0=1")]
        [TestCase("0+1", ExpectedResult = "0+1=1")]
        [TestCase("1+1", ExpectedResult = "1+1=2")]
        public string UnsignedOperationParsing_EvaluateWithExpectedInput_Passes(string input)
        {
            var calculator = Calculator.Build("+");

            return calculator.Evaluate(input).Match
            (
                success: static (_, raw) => raw,
                failure: static failure => failure.Message,
                error: static exception => throw exception
            );
        }

        [Test]
        [TestCase("0+-1", ExpectedResult = "0+-1=ERROR")]
        public string UnsignedOperationParsing_EvaluateWithIncorrectInput_Passes(string input)
        {
            var calculator = Calculator.Build("+");

            return calculator.Evaluate(input).Match
            (
                success: static (_, raw) => raw,
                failure: static failure => failure.Message,
                error: static exception => throw exception
            );
        }

        [Test]
        [TestCase("0-0")]
        [TestCase("1*0")]
        public void UnsignedOperationParsing_EvaluateWithWrongInput_Fails(string input)
        {
            var calculator = Calculator.Build("+");

            calculator.Evaluate(input).Match
            (
                success: static (_, _) => Assert.Fail("Should not happen!"),
                failure: static _ => Assert.Fail("Should not happen!"),
                error: static exception => Assert.IsTrue(exception is ArgumentException)
            );
        }

        [Test]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public void UnsignedOperationParsing_EvaluateWithBlankInput_Fails(string input)
        {
            var calculator = Calculator.Build("+");

            calculator.Evaluate(input).Match
            (
                success: static (_, _) => Assert.Fail("Should not happen!"),
                failure: static _ => Assert.Fail("Should not happen!"),
                error: static exception => Assert.IsTrue(exception is ArgumentException)
            );
        }
    }
}
