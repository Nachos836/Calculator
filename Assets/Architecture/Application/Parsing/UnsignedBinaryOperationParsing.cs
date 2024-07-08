using System.Collections.Generic;
using System.Linq;

namespace Calc.Application.Parsing
{
    using Calculations;
    using Domain.Calculations;

    internal static class UnsignedBinaryOperationParsing
    {
        private static readonly IEnumerable<UnsignedBinaryOperator> AvailableOperators = new UnsignedBinaryOperator[]
        {
            new UnsignedBinaryAddOperator()
        };

        public static UnsignedBinaryOperator? Parse(string input)
        {
            return AvailableOperators.SingleOrDefault(@operator => input == @operator.ToString());
        }
    }
}
