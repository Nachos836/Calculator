using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Calc.Domain.Tests.UnitTests
{
    using Calculations;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class Operand_UnitTests
    {
        [Test]
        public void OperandWithValue1_0mDecimalCreated_Passes()
        {
            var value = 1.0m;
            var candidate = UnsignedOperand.Create(value);

            candidate.Match
            (
                success: stored => Assert.AreEqual(value, stored.Raw),
                error: static exception => Assert.Fail(exception.Message)
            );
        }

        [Test]
        public void OperandWithValue1_9999999mDecimalCreated_Fails()
        {
            var value = 1.999999m;
            var candidate = UnsignedOperand.Create(value);

            candidate.Match
            (
                success: static _ => Assert.Fail("Should not happen!"),
                error: static exception => Assert.Pass(exception.Message)
            );
        }

        [Test]
        public void OperandWithValueMinus1_0mDecimalCreated_Fails()
        {
            var value = decimal.MinusOne;
            var candidate = UnsignedOperand.Create(value);

            candidate.Match
            (
                success: static _ => Assert.Fail("Should not happen!"),
                error: static exception => Assert.Pass(exception.Message)
            );
        }

        [Test]
        public void OperandWithMaxDecimalValueCreated_Passes()
        {
            var value = decimal.MaxValue;
            var candidate = UnsignedOperand.Create(value);

            candidate.Match
            (
                success: stored => Assert.AreEqual(value, stored.Raw),
                error: static exception => Assert.Fail(exception.Message)
            );
        }

        [Test]
        public void OperandWithMinDecimalValueCreated_Fails()
        {
            var value = decimal.MinValue;
            var candidate = UnsignedOperand.Create(value);

            candidate.Match
            (
                success: static _ => Assert.Fail("Should not happen!"),
                error: static exception => Assert.Pass(exception.Message)
            );
        }
    }
}
