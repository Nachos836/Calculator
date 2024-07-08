using System;
using System.Collections.Generic;
using System.Linq;
using ThirdParty.Functional;
using ThirdParty.Functional.Outcome;

namespace Calc.Application
{
    using Parsing;
    using Domain.Calculations;

    public sealed class Calculator
    {
        private readonly IEnumerable<UnsignedBinaryOperator> _operators;
        private readonly string _validOperators;

        public static Calculator Build(params string[] operators)
        {
            var inputs = operators.Select(UnsignedBinaryOperationParsing.Parse)
                .Where(static input => input is not null)
                .Distinct()
                .ToArray();

            if (inputs.Length is 0 || inputs.Length != operators.Length)
            {
                throw new ArgumentException("Provided input doesn't have all valid operators!", nameof(operators));
            }

            return new Calculator
            (
                inputs!,
                validOperators: inputs.Select(static @operator => @operator!.ToString())
                    .Aggregate(static (first, second) => first + " " + second)
                    .ToString()
                );
        }

        private Calculator(IEnumerable<UnsignedBinaryOperator> operators, string validOperators)
        {
            _operators = operators;
            _validOperators = validOperators;
        }

        public RichResult<decimal, string> Evaluate(string expression)
        {
            if (string.IsNullOrEmpty(expression) || string.IsNullOrWhiteSpace(expression))
            {
                return new ArgumentException("Input: is empty!");
            }

            foreach (var @operator in _operators)
            {
                var operands = expression.Split(@operator.ToString());
                if (operands.Length < 2) return new ArgumentException($"At least two arguments and valid operator \"{ _validOperators }\" between should be provided!");
                if (operands.Length > 2) return new ArgumentException("More than two arguments not supported!");

                var first = UnsignedOperandParsing.Parse(operands[0], out _);
                var second = UnsignedOperandParsing.Parse(operands[1], out _);

                var operation = first.Combine(second)
                    .Run((left, right) => new UnsignedOperation(left, right, @operator)
                        .Execute());

                return operation.Match<RichResult<decimal, string>>
                (
                    success: static (result, resultedString) => (result.Raw, resultedString),
                    failure: _ => new Expected.Failure($"{expression}=ERROR"),
                    error: static exception => exception
                );
            }

            return new Expected.Failure("There is no matched operations available!");
        }
    };
}
