using ThirdParty.Functional;

namespace Calc.Domain.Calculations
{
    public abstract record UnsignedBinaryOperator
    {
        protected abstract string RawRepresentation { get; }
        public abstract Result<UnsignedOperand> Calculate(UnsignedOperand first, UnsignedOperand second);
        public sealed override string ToString() => RawRepresentation;
    }
}
