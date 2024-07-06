namespace Calc.Domain.Calculations
{
    using Functional;

    public abstract record UnsignedBinaryOperator
    {
        protected abstract string RawRepresentation { get; }
        public abstract Result<UnsignedOperand> Calculate(UnsignedOperand first, UnsignedOperand second);
        public sealed override string ToString() => RawRepresentation;
    }
}
