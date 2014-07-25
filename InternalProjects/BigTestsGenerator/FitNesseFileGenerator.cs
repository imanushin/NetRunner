using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigTestsGenerator
{
    internal static class FitNesseFileGenerator
    {
        private static readonly Random random = new Random(684664646);

        public static string Generate(IReadOnlyDictionary<string, int> availableFunctions)
        {
            var result = new StringBuilder();

            for (int itemIndex = 0; itemIndex < 100; itemIndex++)
            {
                var linesCount = random.Next(1, 100);

                result.AppendLine();

                var functionNumber = random.Next(0, availableFunctions.Count - 1);

                var function = availableFunctions.Skip(functionNumber).First();

                for (int lineIndex = 0; lineIndex < linesCount; lineIndex++)
                {
                    result.AppendFormat("| ''' !-{0}-!''' |", function.Key);

                    for (int argumentIndex = 0; argumentIndex < function.Value; argumentIndex++)
                    {
                        result.AppendFormat("'' {0} '' |", RandomUtils.RandomString(random.Next(0, 50)));
                    }

                    result.AppendLine();
                }

                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
