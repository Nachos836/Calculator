using System;
using System.Globalization;
using Calc.Domain.Calculations;
using ThirdParty.Functional;
using ThirdParty.Functional.Outcome;

namespace Calc.Application.Parsing
{
    internal static class UnsignedOperandParsing
    {
        public static RichResult<UnsignedOperand> Parse(in string input, out string leftover)
        {
            if (input.Trim() is { } trimmed && trimmed != input)
            {
                leftover = input;

                return RichResult<UnsignedOperand>.FromException
                (
                    new ArgumentException("Input: can not have whitespace at the start: ")
                );
            }

            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedDecimal) is false)
            {
                leftover = input;

                return RichResult<UnsignedOperand>.FromFailure
                (
                    new Expected.Failure("Input doesn't contain number")
                );
            }

            var decimalString = parsedDecimal.ToString(CultureInfo.InvariantCulture);
            var lengthOfDecimalInString = input.IndexOf(decimalString, StringComparison.InvariantCultureIgnoreCase)
                                          + decimalString.Length;
            leftover = input[lengthOfDecimalInString..];

            return UnsignedOperand.Create(parsedDecimal).Match
            (
                success: static operand => RichResult<UnsignedOperand>.FromResult(operand),
                error: static exception => RichResult<UnsignedOperand>.FromFailure(new Expected.Failure(exception.Message))
            );
        }
    }
}
