using System;
using ThirdParty.Functional;

namespace Calc.Application.Calculations
{
    using Domain.Calculations;

    internal sealed record UnsignedBinaryAddOperator : UnsignedBinaryOperator
    {
        protected override string RawRepresentation => "+";

        public override Result<UnsignedOperand> Calculate(UnsignedOperand first, UnsignedOperand second)
        {
            try
            {
                var candidate = first.Raw + second.Raw;
                return UnsignedOperand.Create(candidate);
            }
            catch (Exception exception)
            {
                return exception;
            }
        }
    }
}
