using System;

namespace Calc.Application.Calculations
{
    using Domain.Calculations;
    using Domain.Functional;

    public sealed record UnsignedAddOperator : UnsignedBinaryOperator
    {
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
