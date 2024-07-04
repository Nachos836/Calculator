namespace Calc.Domain.Calculations
{
    using Functional;

    public abstract record UnsignedBinaryOperator
    {
        public abstract Result<UnsignedOperand> Calculate(UnsignedOperand first, UnsignedOperand second);
    }
}
