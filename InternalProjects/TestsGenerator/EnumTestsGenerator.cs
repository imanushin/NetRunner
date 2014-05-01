using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonObjectsGenerator;
using System.Text;

namespace TestsGenerator
{
    internal sealed class EnumTestsGenerator : ITestGenerator
    {
        private const string FileTemplate =
@"{0}

{1}";

        private const string NamespaceTemplate =
@"
namespace {0}
{{
{1}
}}
";

        private const string TypeTemplate =
@"
    [TestClass]
    public sealed class {0}Test
    {{
        [TestMethod]
        public void {0}_CheckResources()
        {{
            foreach ({0} item in EnumHelper<{0}>.Values)
            {{
                string description = item.GetValueDescription();

                Assert.IsNotNull(description);
            }}
        }}
    }}
";

        public string FileName
        {
            get
            {
                return "EnumGeneratedTests.cs";
            }
        }

        public string GetFileEntry(Assembly targetAssembly)
        {
            var namespaces = new HashSet<string>(CommonNamespaces());

            List<Type> enumTypes = targetAssembly.GetTypes().Where(type => type.IsEnum).Where(HasProperlyProvider).ToList();

            if (!enumTypes.Any())
                return string.Empty;

            var result = new StringBuilder();

            ILookup<string, Type> items = enumTypes.ToLookup(type => type.Namespace);

            foreach (IGrouping<string, Type> typesInNamespace in items)
            {
                namespaces.Add(typesInNamespace.Key);

                var namespaceResult = new StringBuilder();

                foreach (Type type in typesInNamespace)
                {
                    namespaceResult.AppendFormat(TypeTemplate, type.Name);
                }

                result.AppendFormat(NamespaceTemplate, ReadonlyClassesFinder.ConvertToTestNamespace(typesInNamespace.Key), namespaceResult);
            }

            string joinedNamespaces = string.Join(Environment.NewLine, namespaces.Select(item => string.Format("using {0};", item)));

            return string.Format(FileTemplate, joinedNamespaces, result);
        }

        private static IEnumerable<string> CommonNamespaces()
        {
            yield return "Microsoft.VisualStudio.TestTools.UnitTesting";
            yield return "LeakBlocker.Libraries.Common.Resources";
            yield return "LeakBlocker.Libraries.Common";
        }

        private static bool HasProperlyProvider(Type type)
        {
            Attribute targetProvider = type.GetCustomAttributes().FirstOrDefault(attribute => attribute.GetType().Name == "EnumerationDescriptionProviderAttribute");

            return targetProvider != null;
        }
    }
}
