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
        private const int classCount = 10;

        public string FileContent
        {
            get;
            private set;
        }

        public IReadOnlyDictionary<string, int> AvailableFunctions
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

            var functions = new Dictionary<string, int>();

            for (int i = 0; i < classCount; i++)
            {
                var className = RandomUtils.RandomString(RandomUtils.random.Next(10, 20));

                result.AppendLine("    /// <summary>");
                result.AppendFormat("    /// {0}", RandomUtils.RandomSentence(5, 20));
                result.AppendLine();
                result.AppendLine("    /// </summary>");
                result.AppendFormat("    internal sealed class {0} : BaseTestContainer", className);
                result.AppendLine();
                result.AppendLine("    {");

                var functionCount = RandomUtils.random.Next(20, 1000);

                for (int f = 0; f < functionCount; f++)
                {
                    var functionName = RandomUtils.RandomString(RandomUtils.random.Next(10, 20));

                    var resultType = RandomUtils.GetRandomType();

                    var argumentsCount = RandomUtils.random.Next(0, 10);

                    functions[functionName] = argumentsCount;

                    var argumentNames = Enumerable
                        .Range(0, argumentsCount)
                        .Select(a => RandomUtils.RandomString(RandomUtils.random.Next(4, 10)))
                        .ToArray();

                    var arguments = argumentNames
                        .Select(n => string.Format("{0} {1}", RandomUtils.GetRandomType().Name, n))
                        .ToArray();

                    result.AppendLine("        /// <summary>");
                    result.AppendFormat("        /// {0}", RandomUtils.RandomSentence(3, 10));
                    result.AppendLine();
                    result.AppendLine("        /// </summary>");

                    foreach (var argumentName in argumentNames)
                    {
                        result.AppendFormat("        /// <param name=\"{0}\">{1}</param>", argumentName, RandomUtils.RandomSentence(1, 3));
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
                AvailableFunctions = functions,
                FileContent = result.ToString()
            };
        }
    }
}
