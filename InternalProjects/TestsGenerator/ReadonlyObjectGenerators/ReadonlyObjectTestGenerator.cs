using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using CommonObjectsGenerator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;

namespace TestsGenerator.ReadonlyObjectGenerators
{
    internal sealed class ReadonlyObjectTestGenerator : ITestGenerator
    {
        private const string fileName = "ReadOnlyObjectTestsGenerated.cs";

        private static int indexer = 135346;
        private static readonly DateTime startDate = new DateTime(2012, 12, 21);

        private const string totalTemplate =
@"
// ReSharper disable RedundantUsingDirective

{0}

// ReSharper restore RedundantUsingDirective

// ReSharper disable ConvertToConstant.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable InconsistentNaming
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable UnusedVariable
// ReSharper disable RedundantCast
// ReSharper disable UnusedMember.Global
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantExplicitArrayCreation
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantNameQualifier

#pragma warning disable 67
#pragma warning disable 219
#pragma warning disable 168

{1}

#pragma warning restore 219
#pragma warning restore 67
#pragma warning restore 168

// ReSharper restore RedundantNameQualifier
// ReSharper restore MemberCanBePrivate.Global
// ReSharper restore AssignNullToNotNullAttribute
// ReSharper restore RedundantExplicitArrayCreation
// ReSharper restore PartialTypeWithSinglePart
// ReSharper restore RedundantCast
// ReSharper restore UnusedVariable
// ReSharper restore ConditionIsAlwaysTrueOrFalse
// ReSharper restore ObjectCreationAsStatement
// ReSharper restore ConvertToConstant.Local
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Global

";

        private const string namespaceTemplate =
@"
namespace {0}
{{
{1}
}}
";


        private const string commonTestClassStart =
            @"
    [TestClass]
    public sealed partial class {0}Test : ReadOnlyObjectTest
    {{
        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        internal static readonly ObjectsCache<{0}> objects = new ObjectsCache<{0}>(GetInstances);

        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        internal static IEnumerable<{0}[]> CreateNonEmptyObjectsArrays()
        {{
            return new[]
            {{
                {0}Test.objects.Objects.Skip(1).ToArray(),
                {0}Test.objects.Objects.Take(2).ToArray(),
                {0}Test.objects.Objects.Take(1).ToArray()
            }};
        }}

        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        internal static {0} First
        {{
            get
            {{
                return objects.Objects.First();
            }}
        }}

        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        internal static {0} Second
        {{
            get
            {{
                return objects.Objects.Skip(1).First();
            }}
        }}

        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        internal static {0} Third
        {{
            get
            {{
                return objects.Objects.Skip(2).First();
            }}
        }}

        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        private static IEnumerable<{0}> GetInstances()
        {{
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }}

";

        private const string commonTestClassEnd =
            @"
        [TestMethod]
        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        public void {0}_GetHashCodeTest()
        {{
            BaseGetHashCodeTest(objects);
        }}

        [TestMethod]
        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        public void {0}_EqualsTest()
        {{
            BaseEqualsTest(objects);
        }}

        [TestMethod]
        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        public void {0}_ToStringTest()
        {{
            BaseToStringTest(objects);
        }}
    }}
";

        private const string sealedClassTemplate =
@"
        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        private static IEnumerable<{0}>GetApparents()
        {{
            return ReadOnlyList<{0}>.Empty;
        }}
";

        private const string abstractClassTemplate = @"
        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        private static IEnumerable<{0}> GetInstancesOfCurrentType()
        {{
            return ReadOnlyList<{0}>.Empty;
        }}
";

        private const string getApparentsTemplate =
@"
        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        private static IEnumerable<{0}>GetApparents()
        {{
            return
            new {0}[0].Union(
{1});
        }}
";

        private const string checkNullArgConstructorTestTemplate =
@"
        [TestMethod]
        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        public void {2}_CheckNullArg_{0}{4}()
        {{
{1}

            try
            {{
                new {2}({3});
            }}
            catch(ArgumentNullException ex)
            {{
                CheckArgumentExceptionParameter( ""{0}"", ex.ParamName );

                return;
            }}

            Assert.Fail(""Argument '{0}' isn't checked for null inputs"");
        }}
";

        private const string testFileTemplate =
            @"
{0}

namespace {1}
{{
    partial class {2}Test
    {{
        [GeneratedCode(""TestGenerator"", ""1.0.0.0"")]
        private static IEnumerable<{2}> GetInstancesOfCurrentType()
        {{
            #error Implement test class {2}Test to retrieve possible objects for type {2}
        }}
    }}
}}

";

        private const string UnionPartTemplate = @"{1}Test.objects";


        public OutFile[] GetFileEntries(Assembly targetAssembly, ConfigFile configFile)
        {
            ISet<Type> types = new HashSet<Type>(ReadonlyClassesFinder.FindTypes(targetAssembly));

            var typesByNamespace = types.ToLookup(type => type.Namespace);

            var importedNamespaces = GetNamespaces(types).ToList();

            importedNamespaces.AddRange(configFile.GetSectionItems("namespaces"));

            importedNamespaces.Sort();

            importedNamespaces = importedNamespaces.Select(name => string.Format("using {0};", name)).ToList();

            string namespaces = string.Join(Environment.NewLine, importedNamespaces);

            var otherFiles = GenerateFileTemplates(types, namespaces);

            var result = new StringBuilder();

            foreach (IGrouping<string, Type> grouping in typesByNamespace)
            {
                var namespaceEntry = new StringBuilder();

                foreach (Type type in grouping.Where(type => !type.IsInterface))
                {
                    namespaceEntry.AppendFormat(commonTestClassStart, type.Name);

                    if (type.IsSealed)
                    {
                        namespaceEntry.AppendFormat(sealedClassTemplate, type.Name);
                    }
                    else
                    {
                        GenerateAbstractClassTemplate(namespaceEntry, type, types);
                    }

                    if (!type.IsAbstract)
                    {
                        string constructorNullArgumetnsCheck = GenerateTestForConstructors(type, configFile);
                        namespaceEntry.Append(constructorNullArgumetnsCheck);
                    }
                    else
                    {
                        namespaceEntry.AppendFormat(abstractClassTemplate, type.Name);
                    }

                    namespaceEntry.AppendFormat(commonTestClassEnd, type.Name);
                }

                result.AppendFormat(namespaceTemplate, ReadonlyClassesFinder.ConvertToTestNamespace(grouping.Key), namespaceEntry);
            }

            var entry = string.Format(totalTemplate, namespaces, result);

            otherFiles.Add(new OutFile(fileName, entry, true));

            return otherFiles.ToArray();
        }

        private List<OutFile> GenerateFileTemplates(ISet<Type> types, string namespaces)
        {
            var result = new List<OutFile>();

            foreach (Type type in types)
            {
                if (type.IsAbstract)
                    continue;

                var localPath = Path.Combine(
                    string.Join(Path.DirectorySeparatorChar.ToString(), type.Namespace.Split('.').Skip(2).ToArray()),
                    type.Name + "Test.cs");

                string fileEntry = string.Format(
                    testFileTemplate,
                    namespaces,
                    ReadonlyClassesFinder.ConvertToTestNamespace(type.Namespace),
                    type.Name);

                result.Add(new OutFile(localPath, fileEntry, false));
            }

            return result;
        }

        private static IEnumerable<string> GetNamespaces(ISet<Type> types)
        {
            IEnumerable<string> importedNamespaces = new[] {
                "System",
                "System.Collections.Generic",
                "System.Linq",
                "Microsoft.VisualStudio.TestTools.UnitTesting",
                "NetRunner.Executable.Tests",
                "System.CodeDom.Compiler"
            };

            importedNamespaces =
                importedNamespaces.Union(
                    types.Union(
                    types.SelectMany(type => type.GetConstructors()
                    .SelectMany(ctor => ctor.GetParameters())
                    .SelectMany(GetTypesOfParameter)))
                    .Distinct()
                    .SelectMany(RequiredNamespaces))
                    .Distinct();

            return importedNamespaces;
        }

        private static IEnumerable<string> RequiredNamespaces(Type type)
        {
            yield return type.Namespace;

            if (ReadonlyClassesFinder.IsTypeReadOnly(type))
                yield return ReadonlyClassesFinder.ConvertToTestNamespace(type.Namespace);
        }

        private static IEnumerable<Type> GetTypesOfParameter(ParameterInfo parameter)
        {
            yield return parameter.ParameterType;

            foreach (Type type in parameter.ParameterType.GetGenericArguments())
            {
                yield return type;
            }
        }

        private string GenerateTestForConstructors(Type targetType, ConfigFile configFile)
        {
            var result = new StringBuilder();

            bool skipOverloading = targetType.GetConstructors().Length == 1;

            foreach (ConstructorInfo constructor in targetType.GetConstructors())
            {
                if (!constructor.IsPublic)
                    continue;

                var parameters = constructor.GetParameters();

                IEnumerable<string> initializers = parameters.Select(parameter => GenerateParameterInitialization(parameter, configFile));

                string initialization = string.Join(Environment.NewLine, initializers);

                string argumentsList = string.Join(", ", parameters.Select(parameter => parameter.Name));

                foreach (ParameterInfo param in parameters)
                {
                    if (param.ParameterType.IsEnum)
                        result.Append(CheckEnumTestGenerator.CreateWrongEnumConstructor(param, argumentsList, initialization));

                    if (typeof(string) == param.ParameterType && !CanBeEmpty(param))
                        result.Append(StringTestGenerator.CreateEmptyStringConstructor(skipOverloading, param, argumentsList, initialization));

                    if (typeof(DateTime) == param.ParameterType && targetType.Name != "Time")
                        result.Append(DateTimeTestGenerator.CreateWrongDateTimeConstructor(param));

                    if (param.ParameterType.IsValueType
                        || param.Attributes.HasFlag(ParameterAttributes.Optional)
                        || param.GetCustomAttributes(true).Any(a => string.Equals(a.GetType().Name, "CanBeNullAttribute"))) //resharper attribute
                        continue;

                    string ctor = CreateNullArgConstructor(skipOverloading, param, argumentsList, initialization);

                    result.Append(ctor);
                }
            }

            return result.ToString();
        }

        private static bool CanBeEmpty(ParameterInfo parameterInfo)
        {
            if (parameterInfo.Attributes.HasFlag(ParameterAttributes.Optional))
                return true;

            return parameterInfo.GetCustomAttributes().Any(attr => string.Equals(attr.GetType().Name, "StringCanBeEmptyAttribute", StringComparison.Ordinal));
        }

        private static string CreateNullArgConstructor(
            bool skipOverloading,
            ParameterInfo param,
            string argumentsList,
            string initialization)
        {
            string currentArgsReplacement;
            string methodSuffix;

            if (skipOverloading)
            {
                currentArgsReplacement = "null";
                methodSuffix = string.Empty;
            }
            else
            {
                currentArgsReplacement = string.Format("({0})null", GetFullName(param.ParameterType));
                methodSuffix = "_" + indexer++;
            }

            var currentArgs = string.Join(", ", argumentsList.Split(',').Select(s => s.Trim())
                .Select(s => string.Equals(s, param.Name, StringComparison.Ordinal) ? currentArgsReplacement : s));

            Type targetType = param.Member.DeclaringType;

            return string.Format(checkNullArgConstructorTestTemplate, param.Name, initialization, targetType.Name, currentArgs, methodSuffix);
        }

        private static string GetFullName(Type t)
        {
            //http://stackoverflow.com/questions/1533115/get-generictype-name-in-good-format-using-reflection-on-c-sharp
            if (!t.IsGenericType)
                return t.Name;

            var sb = new StringBuilder();

            sb.Append(t.Name.Substring(0, t.Name.LastIndexOf("`", StringComparison.InvariantCultureIgnoreCase)));
            sb.Append(t.GetGenericArguments().Aggregate("<",
                                                        (aggregate, type) =>
                                                        aggregate + (aggregate == "<" ? "" : ",") + GetFullName(type)
                          ));

            sb.Append(">");

            return sb.ToString();
        }

        private string GenerateParameterInitialization(ParameterInfo parameter, ConfigFile configFile)
        {
            const string template = "            var {0} = {1};";

            Type parameterType = parameter.ParameterType;

            string initializer = CreateTypeInitializer(parameterType, parameter.Member, configFile);

            return string.Format(template, parameter.Name, initializer);
        }

        private string CreateTypeInitializer(Type enumType, MemberInfo owner, ConfigFile configFile)
        {
            if (ReadonlyClassesFinder.IsTypeReadOnly(enumType))
            {
                const string initializerTemplate = "{0}Test.First";

                return string.Format(initializerTemplate, enumType.Name);
            }
            if (typeof(int) == enumType)
            {
                return (indexer++).ToString(CultureInfo.InvariantCulture);
            }
            if (typeof(byte) == enumType)
            {
                return ((byte)(indexer++)).ToString(CultureInfo.InvariantCulture);
            }
            if (typeof(bool) == enumType)
            {
                return ((indexer++) % 2 == 0).ToString();
            }
            if (typeof(string) == enumType)
            {
                return string.Format(@"""text {0}""", indexer++);
            }
            if (typeof(long) == enumType)
            {
                return string.Format((indexer++).ToString());
            }
            if (typeof(Guid) == enumType)
            {
                return @"new Guid(""{{C556EA1B-20B8-4E2D-BB3F-8C1A9A691C73}}"")";
            }
            if (enumType.Name.StartsWith("IDictionary`") || enumType.Name.StartsWith("Dictionary`"))
            {
                return string.Format("new Dictionary<{0},{1}>()", GetFullName(enumType.GenericTypeArguments[0]), GetFullName(enumType.GenericTypeArguments[1]));
            }
            if (enumType.Name.StartsWith("ReadOnlyDictionary`"))
            {
                return string.Format("new Dictionary<{0},{1}>().ToReadOnlyDictionary()", GetFullName(enumType.GenericTypeArguments[0]), GetFullName(enumType.GenericTypeArguments[1]));
            }
            if (enumType.Name.StartsWith("Func`"))
            {
                string parameters = string.Join(", ", Enumerable.Range(1, enumType.GenericTypeArguments.Length - 1).Select(num => "arg" + num));

                string resultType = enumType.GenericTypeArguments.Last().Name;

                string allArguments = string.Join(", ", enumType.GenericTypeArguments.Select(type => type.Name));

                return string.Format("new Func<{2}>(({0})=>default({1}))", parameters, resultType, allArguments);
            }
            if (typeof(DateTime) == enumType)//Any value because expected fail result will be because of null argument
            {
                DateTime result = startDate.AddSeconds(indexer++).AddMinutes(indexer++).AddHours(indexer++);

                return string.Format(
                    "new DateTime({0}, {1}, {2}, {3}, {4}, {5}, DateTimeKind.Utc)",
                    result.Year,
                    result.Month,
                    result.Day,
                    result.Hour,
                    result.Minute,
                    result.Second);
            }
            if (enumType.IsEnum)
            {
                const string initializerTemplate = "Enum.GetValues(typeof({0})).Cast<{0}>().First()";

                var name = EnumTestsGenerator.GetEnumName(enumType);

                return string.Format(initializerTemplate, name);
            }

            if (typeof(byte[]) == enumType)
            {
                return string.Format("new byte[]{{ {0}, {1}, {2} }}", (byte)indexer++, (byte)indexer++, (byte)indexer++);
            }

            if (typeof(Exception) == enumType)
            {
                return string.Format("new Exception(\"Text exception {0}\")", indexer++);
            }

            if (typeof(Object) == enumType)
            {
                return string.Format("new object()");
            }

            if (enumType.IsArray)
            {
                const string initializerTemplate = "new {0}[] {{ {1}, {1} }}";

                var innerType = Type.GetType(enumType.FullName.Replace("[]", string.Empty), true);

                string innerInitializer = CreateTypeInitializer(innerType, enumType, configFile);

                return string.Format(initializerTemplate, innerType.Name, innerInitializer);
            }

            if (IsCollection(enumType))
            {
                const string initializerTemplate = "new List<{0}>{{ {1} }}.ToReadOnlyList()";

                var genericArguments = enumType.GetGenericArguments();

                Type genericArg = genericArguments[0];
                string innerInitializer = CreateTypeInitializer(genericArg, enumType, configFile);

                return string.Format(initializerTemplate, genericArg.Name, innerInitializer);
            }

            var propertyString = configFile.GetSectionItems("fakeTypes").FirstOrDefault(s => s.StartsWith(enumType.FullName + "=>"));

            if (!string.IsNullOrWhiteSpace(propertyString))
            {
                int start = propertyString.IndexOf("=>", StringComparison.OrdinalIgnoreCase) + "=>".Length;

                return propertyString.Substring(start);
            }

            throw new InvalidOperationException(string.Format("Unable to generate initializer for the type {0} (member of {1}.{2}). Try to add fake implementation to the file {3}", enumType, owner.DeclaringType, owner.Name, ConfigFileName));
        }

        private static bool IsCollection(Type classType)
        {
            return classType.GetInterface("IEnumerable") != null;
        }

        private static void GenerateAbstractClassTemplate(StringBuilder namespaceEntry, Type type, IEnumerable<Type> allTypes)
        {
            IEnumerable<Type> derrivedTypes = allTypes.Where(otherType => otherType.IsSubclassOf(type)).ToList();

            IEnumerable<string> unions = derrivedTypes.Select(otherType => string.Format(UnionPartTemplate, type.Name, otherType.Name));

            string resultUnion = string.Join(").Union(" + Environment.NewLine, unions);

            namespaceEntry.AppendFormat(getApparentsTemplate, type.Name, resultUnion);
        }

        public string ConfigFileName
        {
            get
            {
                return "TestsGenerationConfig.txt";
            }
        }
    }
}
