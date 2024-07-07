using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using ThirdParty.Functional;

namespace Calc.Domain.Tests.UnitTests
{
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
    }
}
