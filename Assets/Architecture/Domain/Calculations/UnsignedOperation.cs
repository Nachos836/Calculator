namespace Calc.Domain.Calculations
{
    using Functional;

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

        public Result<UnsignedOperand, string> Execute()
        {
            var first = _first;
            var second = _second;
            var @operator = _operator;

            return _operator.Calculate(_first, _second)
                .Attach<string>(result => $"{first}{@operator}{second}={result}");
        }
    }
}
