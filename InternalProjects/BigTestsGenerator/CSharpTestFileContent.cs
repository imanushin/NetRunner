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
            result.AppendLine("using System.Collections.Generic;");
            result.AppendLine("using System.Linq;");
            result.AppendLine("using System.Text;");
            result.AppendLine("using System.Threading.Tasks;");

            result.AppendLine("namespace GeneratedLib");
            result.AppendLine("{");

            var functions = new List<string>();

            for (int i = 0; i < classCount; i++)
            {
                var className = RandomString(random.Next(10, 20));

                result.AppendFormat("    public class {0}", className);
                result.AppendLine();
                result.AppendLine("    {");

                var functionCount = random.Next(20, 1000);

                for (int f = 0; f < functionCount; f++)
                {
                    var functionName = RandomString(random.Next(10, 20));

                    functions.Add(functionName);

                    result.AppendFormat("        public bool Generated{0}(", functionName);

                    var argumentsCount = random.Next(0, 10);

                    var arguments = Enumerable
                        .Range(0, argumentsCount)
                        .Select(a => string.Format("{0} {1}", GetRandomType().Name, RandomString(random.Next(4, 10))))
                        .ToArray();

                    result.AppendFormat("{0})", string.Join(", ", arguments));

                    result.AppendLine();
                    result.AppendLine("        {");
                    result.AppendFormat("            return {0};", (random.Next() % 2 == 0).ToString().ToLowerInvariant());
                    result.AppendLine();
                    result.AppendLine("        }");
                    result.AppendLine();

                }

                result.AppendLine("    }");
                result.AppendLine();
            }

            result.AppendLine("}");

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
                    return typeof(UriPartial);
                case 5:
                    return typeof(TypeCode);
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

    }
}
