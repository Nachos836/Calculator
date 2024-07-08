using System;
using ThirdParty.Functional;
using ThirdParty.Functional.Outcome;

namespace Calc.Domain.Calculations
{
    public readonly record struct UnsignedOperation
    {
        private readonly UnsignedOperand _first;
        private readonly UnsignedOperand _second;
        private readonly UnsignedBinaryOperator _operator;

        public UnsignedOperation(UnsignedOperand first, UnsignedOperand second, UnsignedBinaryOperator @operator)
        {
            _first = first;
            _second = second;
            _operator = @operator;
        }

        public RichResult<UnsignedOperand, string> Execute()
        {
            var first = _first;
            var second = _second;
            var @operator = _operator;

            return _operator.Calculate(_first, _second)
                .Match
                (
                    success: result => RichResult<UnsignedOperand, string>.FromResult
                    (
                        result, $"{first}{@operator}{second}={result}"
                    ),
                    error: exception =>
                    {
                        return exception switch
                        {
                            OverflowException => new Expected.Failure($"{first}{@operator}{second}=ERROR"),
                            _ => RichResult<UnsignedOperand, string>.Impossible,
                        };
                    }
                );
        }
    }
}
