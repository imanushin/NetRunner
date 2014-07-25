using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigTestsGenerator
{
    internal sealed class CSharpTestFileContent
    {
        private static readonly Random random = new Random(951369416);

        private const int classCount = 10;

        public string FileContent
        {
            get;
            private set;
        }

        public ReadOnlyCollection<string> AvailableFunctions
        {
            get;
            private set;
        }

        public static CSharpTestFileContent Generate()
        {
            var result = new StringBuilder();

            result.AppendLine("using System;");
            result.AppendLine("using NetRunner.ExternalLibrary;");
            result.AppendLine("// ReSharper disable UnusedMember.Global");
            result.AppendLine("// ReSharper disable InconsistentNaming");

            result.AppendLine();
            result.AppendLine("namespace NetRunner.GeneratedTests");
            result.AppendLine("{");

            var functions = new List<string>();

            for (int i = 0; i < classCount; i++)
            {
                var className = RandomString(random.Next(10, 20));

                result.AppendLine("    /// <summary>");
                result.AppendFormat("    /// {0}", RandomSentence(5, 20));
                result.AppendLine();
                result.AppendLine("    /// </summary>");
                result.AppendFormat("    internal sealed class {0} : BaseTestContainer", className);
                result.AppendLine();
                result.AppendLine("    {");

                var functionCount = random.Next(20, 1000);

                for (int f = 0; f < functionCount; f++)
                {
                    var functionName = RandomString(random.Next(10, 20));

                    functions.Add(functionName);

                    var resultType = GetRandomType();

                    var argumentsCount = random.Next(0, 10);

                    var argumentNames = Enumerable
                        .Range(0, argumentsCount)
                        .Select(a => RandomString(random.Next(4, 10)))
                        .ToArray();

                    var arguments = argumentNames
                        .Select(n => string.Format("{0} {1}", GetRandomType().Name, n))
                        .ToArray();

                    result.AppendLine("        /// <summary>");
                    result.AppendFormat("        /// {0}", RandomSentence(3, 10));
                    result.AppendLine();
                    result.AppendLine("        /// </summary>");

                    foreach (var argumentName in argumentNames)
                    {
                        result.AppendFormat("        /// <param name=\"{0}\">{1}</param>", argumentName, RandomSentence(1, 3));
                        result.AppendLine();
                    }

                    result.AppendFormat("        internal {0} Generated{1}(", resultType.Name, functionName);

                    result.AppendFormat("{0})", string.Join(", ", arguments));

                    result.AppendLine();
                    result.AppendLine("        {");
                    result.AppendFormat("            return ({0})Convert.ChangeType(123, typeof ({0}));", resultType.Name);
                    result.AppendLine();
                    result.AppendLine("        }");
                    result.AppendLine();

                }

                result.AppendLine("    }");
                result.AppendLine();
            }

            result.AppendLine("}");
            result.AppendLine("// ReSharper restore InconsistentNaming");
            result.AppendLine("// ReSharper restore UnusedMember.Global");

            return new CSharpTestFileContent
            {
                AvailableFunctions = functions.AsReadOnly(),
                FileContent = result.ToString()
            };
        }

        private static Type GetRandomType()
        {
            var typeId = random.Next(0, 5);

            switch (typeId)
            {
                case 0:
                    return typeof(bool);
                case 1:
                    return typeof(int);
                case 2:
                    return typeof(string);
                case 3:
                    return typeof(Uri);
                case 4:
                    return typeof(GCCollectionMode);
                case 5:
                    return typeof(MidpointRounding);
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Type id {0} is not supported", typeId));
            }
        }

        private static string RandomString(int size)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private static string RandomSentence(int minWords, int maxWords)
        {
            var wordsCount = random.Next(minWords, maxWords);

            var words = Enumerable.Range(0, wordsCount).Select(i => RandomString(random.Next(3, 8))).ToArray();

            return string.Join(" ", words);
        }

    }
}
