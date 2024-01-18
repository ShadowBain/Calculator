namespace CalculatorLib
{
    public class CalculatorEngine
    {
        public double ProcessCalculation(string calculationString)
        {
            //bedmas

            if (!IsComplex(calculationString)) return HandleCalculation(calculationString);

            if (calculationString.Contains('('))
            {
                calculationString = HandleBracket(calculationString);

                return ProcessCalculation(calculationString);
            }

            return HandleMultipleSymbol(calculationString);
        }

        private double HandleMultipleSymbol(string calculationString)
        {
            if (calculationString.Contains('-'))
            {
                var minusIndex = calculationString.LastIndexOf('-');

                var value1 = ProcessCalculation(calculationString.Substring(0, minusIndex).Trim());
                var value2 = ProcessCalculation(calculationString.Substring(minusIndex + 1).Trim());

                return HandleCalculation($"{value1} - {value2}");
            }

            if (calculationString.Contains("+"))
            {
                var plusIndex = calculationString.LastIndexOf('+');

                var value1 = ProcessCalculation(calculationString.Substring(0, plusIndex).Trim());
                var value2 = ProcessCalculation(calculationString.Substring(plusIndex + 1).Trim());

                return HandleCalculation($"{value1} + {value2}");
            }

            if (calculationString.Contains("*"))
            {
                var timesIndex = calculationString.LastIndexOf('*');

                var value1 = ProcessCalculation(calculationString.Substring(0, timesIndex).Trim());
                var value2 = ProcessCalculation(calculationString.Substring(timesIndex + 1).Trim());

                return HandleCalculation($"{value1} * {value2}");
            }

            if (calculationString.Contains('/'))
            {
                var divideIndex = calculationString.LastIndexOf('/');

                var value1 = ProcessCalculation(calculationString.Substring(0, divideIndex).Trim());
                var value2 = ProcessCalculation(calculationString.Substring(divideIndex + 1).Trim());

                return HandleCalculation($"{value1} / {value2}");
            }

            return 0d;
        }

        private string HandleBracket(string calculationString)
        {
            var startIndex = calculationString.LastIndexOf('(');
            var length = calculationString.IndexOf(")", startIndex) - startIndex;

            var subString = calculationString.Substring(startIndex + 1, length - 1);

            var bracketResult = ProcessCalculation(subString);

            return calculationString.Replace($"({subString})", $"{bracketResult}");
        }

        private double HandleCalculation(string calculationString)
        {
            if (string.IsNullOrWhiteSpace(calculationString)) return 0d;
            if (double.TryParse(calculationString, out var result)) return result;

            if (calculationString.Contains('+'))
            {
                var values = calculationString.Split('+');

                var hasValue1 = double.TryParse(values[0], out var value1);
                var hasValue2 = double.TryParse(values[1], out var value2);

                if (!hasValue1 && hasValue2) return value2;
                if (hasValue1 && !hasValue2) return value1;

                return value1 + value2;
            }

            if (calculationString.Contains('-'))
            {
                var values = calculationString.Split('-');
                var value1Index = 0;

                if (string.IsNullOrWhiteSpace(values[0]) && values.Length > 2)
                {
                    value1Index++;
                    values[value1Index] = $"-{values[value1Index]}";
                }

                var hasValue1 = double.TryParse(values[value1Index], out var value1);
                var hasValue2 = double.TryParse(values[value1Index + 1], out var value2);

                if (!hasValue1 && hasValue2) return value2;
                if (hasValue1 && !hasValue2) return value1;

                return value1 - value2;
            }

            if (calculationString.Contains('*'))
            {
                var values = calculationString.Split('*');

                var hasValue1 = double.TryParse(values[0], out var value1);
                var hasValue2 = double.TryParse(values[1], out var value2);

                if (!hasValue1 && hasValue2) return value2;
                if (hasValue1 && !hasValue2) return value1;

                return value1 * value2;
            }

            if (calculationString.Contains('/'))
            {
                var values = calculationString.Split('/');

                var hasValue1 = double.TryParse(values[0], out var value1);
                var hasValue2 = double.TryParse(values[1], out var value2);

                if (!hasValue1 && hasValue2) return value2;
                if (hasValue1 && !hasValue2) return value1;

                return (value1 / value2);
            }

            throw new InvalidOperationException("Unsupported calculation");
        }

        private bool IsComplex(string calculationString)
        {
            if (calculationString.Contains("(")) return true;

            var symbolCount = calculationString.Count(x => x == '+' || x == '-' || x == '/' || x == '*');

            return symbolCount > 1;
        }
    }
}