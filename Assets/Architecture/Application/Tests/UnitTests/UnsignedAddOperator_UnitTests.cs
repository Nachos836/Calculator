using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Calc.Application.Tests.UnitTests
{
    using Calculations;
    using Domain.Calculations;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class UnsignedAddOperation_UnitTests
    {
        [Test]
        public void UnsignedAddOperator_Created_Passes()
        {
            var candidate = new UnsignedBinaryAddOperator();

            Assert.IsNotNull(candidate);
        }

        [Test]
        public void UnsignedAddOperator_WithDefaultData_Passes()
        {
            var first = default(UnsignedOperand);
            var second = default(UnsignedOperand);
            var operation = new UnsignedBinaryAddOperator();

            var candidate = operation.Calculate(first, second);
            candidate.Match
            (
                success: static _ => Assert.Pass(),
                error: static exception => Assert.Fail(exception.Message)
            );
        }
    }
}
