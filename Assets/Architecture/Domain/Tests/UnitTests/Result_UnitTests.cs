using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Calc.Architecture.Domain.Tests.UnitTests
{
    using Calc.Domain.Functional;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class Result_UnitTests
    {
        [Test]
        public void ResultWithValue1_0mDecimalCreated_Passes()
        {
            var value = 1.0m;
            var candidate = Result<decimal>.FromResult(value);

            candidate.Match
            (
                success: storedValue => Assert.AreEqual(value, storedValue),
                error: static exception => Assert.Fail(exception.Message)
            );
        }

        [Test]
        public void ResultWithValue0_99999mDecimalCreated_Fails()
        {
            var value = 0.99999m;
            var candidate = Result<decimal>.FromResult(value);

            candidate.Match
            (
                success: static _ => Assert.Fail("Should not happen!"),
                error: static exception => Assert.Pass(exception.Message)
            );
        }
    }
}
