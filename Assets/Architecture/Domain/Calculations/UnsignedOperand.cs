using System;
using System.Globalization;
using ThirdParty.Functional;

namespace Calc.Domain.Calculations
{
    public readonly record struct UnsignedOperand
    {
        public decimal Raw { get; }

        public static Result<UnsignedOperand> Create(decimal income)
        {
            if (income < decimal.Zero)
            {
                return Result<UnsignedOperand>
                    .FromException(new Exception($"Value should not be negative! Value is {income}"));
            }

            var candidate = decimal.Round(income);
            if (candidate != income)
            {
                return Result<UnsignedOperand>
                    .FromException(new Exception($"Value should not be fractional! Value is {income}"));
            }

            return new UnsignedOperand(income);
        }

        private UnsignedOperand(decimal income)
        {
            Raw = income;
        }

        public override string ToString()
        {
            return decimal.Truncate(Raw)
                .ToString(CultureInfo.InvariantCulture);
        }
    }
}
