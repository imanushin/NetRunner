using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Properties;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    internal static class ReflectionLoader
    {
        private static readonly Type testContainerType = typeof(BaseTestContainer);

        private static AppDomain currentTestDomain;

        [NotNull]
        private static ReadOnlyList<string> assemblyList = ReadOnlyList<string>.Empty;

        [NotNull]
        private static ReadOnlyList<string> assemblyFolders = ReadOnlyList<string>.Empty;

        [NotNull]
        private static ReadOnlyList<TestFunctionReference> functions = ReadOnlyList<TestFunctionReference>.Empty;

        [NotNull]
        private static ReadOnlyList<BaseTestContainer> testContainers = ReadOnlyList<BaseTestContainer>.Empty;

        private static readonly string[] ignoredFunctions =
        {
            "ToString", "GetHashCode", "Equals", "GetType"
        };

        static ReflectionLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var loadLog = new StringBuilder();

                var assemblyName = new AssemblyName(args.Name).Name;

                loadLog.AppendFormat("Try to find assembly '{0}' from the custom locations: {1}", assemblyName, assemblyFolders);
                loadLog.AppendLine();

                var targetFileName = assemblyName + ".dll";

                var filesCandidates = assemblyFolders.Select(f => Path.Combine(f, targetFileName)).Where(File.Exists).ToReadOnlyList();

                loadLog.AppendFormat("Existing files with '{0}': {1}", assemblyName, filesCandidates);
                loadLog.AppendLine();

                foreach (var candidate in filesCandidates)
                {
                    try
                    {
                        return Assembly.LoadFrom(candidate);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Unable to load assembly {0} from file {1}: {2}.", args.Name, candidate, ex);
                    }
                }

                if (string.Equals(assemblyName, "HtmlAgilityPack", StringComparison.OrdinalIgnoreCase))
                {
                    return Assembly.Load(Resources.HtmlAgilityPack);
                }

                Trace.TraceInformation(loadLog.ToString());

                Trace.TraceError("Unable to load assembly {0}. Files candidates: {1}, ", args.Name, filesCandidates);
            }
            catch (Exception ex)
            {
                Trace.TraceError("General assembly found error: {0}", ex);
            }

            return null;
        }

        public static void AddAssemblies(IReadOnlyCollection<string> assemblyPathes)
        {
            var allAssemblyFiles = assemblyList.Concat(assemblyPathes).Distinct(StringComparer.OrdinalIgnoreCase).Where(af => !string.IsNullOrWhiteSpace(af)).ToReadOnlyList();

            assemblyList = allAssemblyFiles.Where(File.Exists).ToReadOnlyList();

            var missingFiles = allAssemblyFiles.Where(af => !File.Exists(af)).ToReadOnlyList();

            if (missingFiles.Any())
            {
                Trace.TraceWarning("Unable to find some of assembly files: {0}", missingFiles);
            }

            assemblyFolders = assemblyList.Select(Path.GetDirectoryName).Distinct(StringComparer.OrdinalIgnoreCase).ToReadOnlyList();

            Trace.TraceInformation("Additional folder for assembly loading: {0}", assemblyFolders.JoinToStringLazy("; "));

            //ToDo: inject them into the test domain
        }

        private static void ReloadAssemblies()
        {
            Trace.TraceInformation("Start assembly loading from list: {0}", assemblyList);

            var loadedAssemblies = LoadAssemblies(assemblyList);

            var testTypes = FindTestTypes(loadedAssemblies);
            var parserTypes = FindParsersAvailable(loadedAssemblies).ToReadOnlyList();
            
            testContainers = CreateTypeInstances<BaseTestContainer>(testTypes).ToReadOnlyList();

            functions = FindFunctionsAvailable(testContainers.ToReadOnlyList());

            var parsersFound = CreateTypeInstances<BaseParser>(parserTypes);

            parsersFound.Sort((first, second) => second.Priority - first.Priority);

            Parsers = parsersFound.ToReadOnlyList();

            Trace.TraceInformation("All available functions: {0}", functions.JoinToStringLazy(Environment.NewLine));
        }

        private static string LoadConfigurationIfNeeded()
        {
            const string configSuffix = ".config";

            var configurationsAvailable = assemblyList.Select(path => path + configSuffix).Where(File.Exists).ToReadOnlyList();

            if (!configurationsAvailable.Any())
                return string.Empty;

            if (configurationsAvailable.Count > 1)
            {
                Trace.TraceInformation("Load configuration step skipped because more than one (actually - {0}) configuration files are available: {1}", configurationsAvailable);

                return string.Empty;
            }

            string configurationFile = configurationsAvailable.First();
            string targetAssemblyFile = configurationFile.Substring(0, configurationFile.Length - configSuffix.Length);

            Trace.TraceInformation("Configuration will be loaded for assembly '{0}' (file '{1}')", targetAssemblyFile, configurationFile);

            return configurationFile;
        }

        public static ReadOnlyList<BaseParser> Parsers
        {
            get;
            private set;
        }

        public static ReadOnlyList<string> TestContainerNames
        {
            get
            {
                return testContainers.Select(tc => tc.GetType().Name).ToReadOnlyList();
            }
        }

        public static ReadOnlyList<string> AssemblyPathes
        {
            get
            {
                return assemblyList;
            }
        }

        private static IEnumerable<Type> FindParsersAvailable(List<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.GetTypes())
                        .Where(CanBeConstructed)
                        .Where(t => t.IsSubclassOf(typeof(BaseParser))).ToReadOnlyList();
        }

        [CanBeNull]
        public static TestFunctionReference FindFunction(ReadOnlyList<string> argumentNames, BaseTableArgument targetObject)
        {
            var targetType = targetObject.GetType();

            var allMethods = targetType.GetMethods();

            var methodCandidates =
                allMethods.Where(m => m.GetParameters().Select(p => p.Name).SequenceEqual(argumentNames, StringComparer.OrdinalIgnoreCase));

            var firstCandidate = methodCandidates.FirstOrDefault();

            if (firstCandidate == null)
                return null;

            return new TestFunctionReference(firstCandidate, targetObject);
        }

        [CanBeNull]
        public static TestFunctionReference FindFunction(string name, int argumentCount)
        {
            return functions.FirstOrDefault(f => f.ArgumentTypes.Count == argumentCount && string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        private static ReadOnlyList<TestFunctionReference> FindFunctionsAvailable(ReadOnlyList<BaseTestContainer> testContainers)
        {
            var functions = new List<TestFunctionReference>();

            foreach (BaseTestContainer container in testContainers)
            {
                var targetType = container.GetType();

                var availableTests = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => !ignoredFunctions.Contains(f.Name));

                BaseTestContainer localContainer = container;

                var availableFunctions = availableTests.Select(t => new TestFunctionReference(t, localContainer)).ToReadOnlyList();

                Trace.TraceInformation("Type {0} contains following public functions: {1}", targetType.Name, availableFunctions);

                functions.AddRange(availableFunctions);
            }

            return functions.ToReadOnlyList();
        }

        private static List<TResultType> CreateTypeInstances<TResultType>(ReadOnlyList<Type> testTypes)
        {
            var result = new List<TResultType>();

            foreach (Type testType in testTypes)
            {
                try
                {
                    var constructor = testType.GetConstructors().FirstOrDefault(ct => !ct.GetParameters().Any());

                    Validate.IsNotNull(constructor, "Unable to find constructor without parameters for type {0}", testType.Name);

                    var targetObject = (TResultType)constructor.Invoke(new object[0]);

                    result.Add(targetObject);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to create instance of type {0} because of error: {1}", testType, ex);
                }
            }

            return result;
        }

        private static ReadOnlyList<Type> FindTestTypes(List<Assembly> loadedAssemblies)
        {
            var testContainers = new List<Type>();

            foreach (Assembly assembly in loadedAssemblies)
            {
                try
                {
                    var types = assembly.GetTypes()
                        .Where(CanBeConstructed)
                        .Where(t => t.IsSubclassOf(testContainerType)).ToReadOnlyList();

                    Trace.TraceInformation("Test containers from assembly {1}: {0}", types, assembly);

                    testContainers.AddRange(types);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Trace.TraceError(
                        "Unable to retrieve types from assembly {0} because of error {1}. Inner exceptions: {2}",
                        assembly,
                        ex.Message,
                        string.Join(Environment.NewLine, ex.LoaderExceptions.Cast<Exception>()));
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to retrieve types from assembly {0} because of error {1}", assembly, ex);
                }
            }

            var result = testContainers.Distinct().ToReadOnlyList();

            Trace.TraceInformation("All test containers: {0}", string.Join(", ", result.Select(t => t.Name)));
            return result;
        }

        private static bool CanBeConstructed(Type t)
        {
            return !(t.IsGenericType || t.IsAbstract || t.IsValueType);
        }

        private static List<Assembly> LoadAssemblies(ReadOnlyList<string> pathes)
        {
            Validate.IsNotNull(currentTestDomain, "Test domain was not initialized");

            var loadedAssemblies = new List<Assembly>() { testContainerType.Assembly };

            foreach (string assemblyPath in pathes)
            {
                try
                {
                    if (!File.Exists(assemblyPath))
                    {
                        Trace.TraceError("Unable to load assembly because it does not exist: '{0}'", assemblyPath);

                        continue;
                    }

                    var loadedAssembly = currentTestDomain.Load(File.ReadAllBytes(assemblyPath));

                    Trace.TraceInformation("Assembly {0} was loaded", loadedAssembly);

                    loadedAssemblies.Add(loadedAssembly);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to load assembly {0} because of error: {1}", assemblyPath, ex);
                }
            }
            return loadedAssemblies;
        }

        public static bool TryReadPropery(object targetObject, string propertyName, [CanBeNull]  out Type propertyType, [CanBeNull] out object resultValue)
        {
#warning change to Table changes
            var targetType = targetObject.GetType();

            var property = targetType.GetProperties().FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

            if (property == null)
            {
                Trace.TraceError("Unable to find property with name {0} for type {1} ({2})", propertyName, targetType, targetObject);
                resultValue = null;
                propertyType = null;
                return false;
            }

            resultValue = property.GetValue(targetObject);
            propertyType = property.PropertyType;

            return true;
        }

        public static void CreateNewApplicationDomain()
        {
            if (currentTestDomain != null)
            {
                AppDomain.Unload(currentTestDomain);
            }

            var evidence = new Evidence();

            var setupInformation = new AppDomainSetup();

            var configFileCandidate = LoadConfigurationIfNeeded();

            if (!string.IsNullOrWhiteSpace(configFileCandidate))
            {
                setupInformation.ConfigurationFile = configFileCandidate;
            }

            currentTestDomain = AppDomain.CreateDomain("Test execution domain", evidence, setupInformation);

            ReloadAssemblies();
        }
    }
}
