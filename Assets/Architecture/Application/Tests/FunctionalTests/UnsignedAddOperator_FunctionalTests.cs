using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Calc.Application.Tests.FunctionalTests
{
    using Calculations;
    using Domain.Calculations;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class UnsignedAddOperator_FunctionalTests
    {
        [Test]
        [TestCase(0.0, 0.0, ExpectedResult = 0.0)]
        [TestCase(1.0, 0.0, ExpectedResult = 1.0)]
        [TestCase(0.0, 1.0, ExpectedResult = 1.0)]
        [TestCase(1.0, 1.0, ExpectedResult = 2.0)]
        public decimal UnsignedAddOperator_Calculate_Passes(decimal first, decimal second)
        {
            var firstResult = UnsignedOperand.Create(first);
            var secondResult = UnsignedOperand.Create(second);

            var candidate = firstResult.Combine(secondResult)
                .Run(static (first, second) =>
                {
                    return new UnsignedBinaryAddOperator()
                        .Calculate(first, second);
                });

            return candidate.Match
            (
                success: static provided => provided.Raw,
                error: static exception => throw exception
            );
        }

        [Test]
        public void UnsignedAddOperator_Calculate_EdgeCaseWithBothMaxValue_Fails()
        {
            var firstResult = UnsignedOperand.Create(decimal.MaxValue);
            var secondResult = UnsignedOperand.Create(decimal.MaxValue);

            var candidate = firstResult.Combine(secondResult)
                .Run(static (first, second) =>
                {
                    return new UnsignedBinaryAddOperator()
                        .Calculate(first, second);
                });

            candidate.Match
            (
                success: static _ => Assert.Fail("Should not happen!"),
                error: static exception => Assert.IsTrue(exception is OverflowException)
            );
        }
    }
}
