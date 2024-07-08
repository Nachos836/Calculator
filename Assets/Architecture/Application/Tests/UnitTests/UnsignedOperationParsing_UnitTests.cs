using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Calc.Application.Tests.UnitTests
{
    using Parsing;
    using Calculations;
    using Domain.Calculations;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class UnsignedOperationParsing_UnitTests
    {
        [Test]
        [TestCase(0.0, 0.0, ExpectedResult = "0+0=0")]
        [TestCase(1.0, 0.0, ExpectedResult = "1+0=1")]
        [TestCase(0.0, 1.0, ExpectedResult = "0+1=1")]
        [TestCase(1.0, 1.0, ExpectedResult = "1+1=2")]
        public string UnsignedOperationParsing_ExecuteRawResult_Passes(decimal first, decimal second)
        {
            var firstResult = UnsignedOperand.Create(first);
            var secondResult = UnsignedOperand.Create(second);

            var candidate = firstResult.Combine(secondResult)
                .Run(static (first, second) =>
                {
                    var @operator = new UnsignedBinaryAddOperator();

                    return new UnsignedOperation(first, second, @operator)
                        .Execute();
                });

            return candidate.Match
            (
                success: static (_, raw) => raw,
                failure: static failure => failure.Message,
                error: static exception => throw exception
            );
        }

        [Test]
        public void UnsignedOperationParsing_NullStringOnInput_Fails()
        {
            string input = null!;
            var candidate = UnsignedOperationParsing.Parse(ArraySegment<UnsignedBinaryOperator>.Empty, input);

            candidate.Match
            (
                success: static _ => Assert.Fail("Should not happen!"),
                failure: static _ => Assert.Fail("Should not happen!"),
                error: static exception => Assert.Pass(exception.Message)
            );
        }

        [Test]
        [TestCase("0+0", ExpectedResult = "0+0=0")]
        public string UnsignedOperationParsing_Input_Passes(string input)
        {
            var operators = new UnsignedBinaryOperator[] { new UnsignedBinaryAddOperator() };
            var candidate = UnsignedOperationParsing.Parse(operators, input);

            return candidate.Match<string>
            (
                success: static operation => operation.Execute().Match
                (
                    success: static (_, raw) => raw,
                    failure: static failure => failure.Message,
                    error: static exception => exception.Message
                ),
                failure: static failure => failure.Message,
                error: static exception => exception.Message
            );
        }
    }
}
