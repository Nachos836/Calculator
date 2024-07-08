using System;
using System.Collections.Generic;
using ThirdParty.Functional;

namespace Calc.Application.Parsing
{
    using Domain.Calculations;

    internal static class UnsignedOperationParsing
    {
        public static RichResult<UnsignedOperation> Parse(IEnumerable<UnsignedBinaryOperator> operators, string rawExpression)
        {
            if (string.IsNullOrEmpty(rawExpression) || string.IsNullOrWhiteSpace(rawExpression))
            {
                return RichResult<UnsignedOperation>.FromException
                (
                    new ArgumentException($"Input: \"{rawExpression}\" is invalid!")
                );
            }

            foreach (var @operator in operators)
            {
                var operands = rawExpression.Split(@operator.ToString());
                if (operands.Length != 2) continue;

                var first = UnsignedOperandParsing.Parse(operands[0], out _);
                var second = UnsignedOperandParsing.Parse(operands[1], out _);

                return first.Combine(second).Run<UnsignedOperation>((left, right) =>
                {
                    return new UnsignedOperation(left, right, @operator);
                });
            }

            return RichResult<UnsignedOperation>.Impossible;
        }
    }
}
