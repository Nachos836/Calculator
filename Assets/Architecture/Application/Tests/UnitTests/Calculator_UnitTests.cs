using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace Calc.Application.Tests.UnitTests
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class Calculator_UnitTests
    {
        [Test]
        public void Calculator_BuildWithPlusProvided_Passes()
        {
            var candidate = Calculator.Build("+");

            Assert.IsNotNull(candidate);
        }

        [Test]
        [TestCase(arg1: new object []{ "-" }, "")]
        [TestCase(arg1: new object []{ "-", "%" }, "")]
        [TestCase(arg1: new object []{ "wuifgsdbgsdbgfjihu3247" }, "")]
        [TestCase(arg1: new object []{ " +" }, "")]
        [TestCase(arg1: new object []{ "+", "-" }, "")]
        [TestCase(arg1: new object []{ "+", "!!!" }, "")]
        public void Calculator_BuildWithWrongArgs_Fails(object[] raw, string _)
        {
            var input = raw.Cast<string>().ToArray();

            Assert.Throws<ArgumentException>(() => Calculator.Build(input));
        }
    }
}
