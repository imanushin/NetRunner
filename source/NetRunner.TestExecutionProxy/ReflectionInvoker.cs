using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    public sealed class ReflectionInvoker : MarshalByRefObject
    {
        private string[] assemblyFolders = new string[0];
        private readonly List<Assembly> testAssemblies = new List<Assembly>();

        private static readonly string[] ignoredFunctions =
        {
            "ToString", "GetHashCode", "Equals", "GetType"
        };

        private static readonly ConsoleTraceListener consoleTraceListener = new ConsoleTraceListener();

        public ReflectionInvoker()
        {
            if (!Trace.Listeners.Contains(consoleTraceListener))
            {
                Trace.Listeners.Add(consoleTraceListener);
                Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var loadLog = new StringBuilder();

                var assemblyName = new AssemblyName(args.Name).Name;

                loadLog.AppendFormat("Try to find assembly '{0}' from the custom locations: {1}", assemblyName, string.Join(", ", assemblyFolders));
                loadLog.AppendLine();

                var targetFileName = assemblyName + ".dll";

                var filesCandidates = assemblyFolders.Select(f => Path.Combine(f, targetFileName)).Where(File.Exists).ToArray();

                var joinedCandidates = string.Join(", ", filesCandidates);

                loadLog.AppendFormat("Existing files with '{0}': {1}", assemblyName, joinedCandidates);
                loadLog.AppendLine();

                foreach (var candidate in filesCandidates)
                {
                    try
                    {
                        return Assembly.LoadFile(candidate);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Unable to load assembly {0} from file {1}: {2}.", args.Name, candidate, ex);
                    }
                }

                Trace.TraceInformation(loadLog.ToString());

                
                Trace.TraceError("Unable to load assembly {0}. Files candidates: {1}, ", args.Name, joinedCandidates);
            }
            catch (Exception ex)
            {
                Trace.TraceError("General assembly found error: {0}", ex);
            }

            return null;
        }

        public void AddAssemblyLoadFolders(string[] newAssemblyFolders)
        {
            assemblyFolders = assemblyFolders.Concat(newAssemblyFolders).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }

        public void LoadTestAssembly(string assemblyPath)
        {
            if (!testAssemblies.Any())
            {
                testAssemblies.Add(BaseParserType.Assembly);
            }

            if (!File.Exists(assemblyPath))
            {
                var assemblyName = Path.GetFileName(assemblyPath);

                var otherPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName);

                if (File.Exists(otherPath))
                {
                    assemblyPath = otherPath;
                }
            }

            if (!File.Exists(assemblyPath))
            {
                Trace.TraceError("Unable to load assembly from the path '{0}'. Current domain directory: '{1}'", assemblyPath, AppDomain.CurrentDomain.BaseDirectory);

                return;
            }

            testAssemblies.Add(Assembly.LoadFrom(assemblyPath));
        }

        private static Type BaseParserType
        {
            get
            {
                return typeof(BaseParser);
            }
        }

        private static Type TestContainerType
        {
            get
            {
                return typeof(BaseTestContainer);
            }
        }

        private TypeReference[] FindAllTypes(Type baseType)
        {
            try
            {
                Trace.TraceInformation("Start type finding (all types which derrived from type {0} in {1} assemblies)", baseType.Name, testAssemblies.Count);

                var nonAbstractTypes = testAssemblies.SelectMany(a => a.GetTypes()).Where(CanBeConstructed).ToArray();

                Trace.TraceInformation("Non-abstract types: {0}", string.Join(", ", nonAbstractTypes.Select(t => t.Name)));

                var subclasses = nonAbstractTypes.Where(t => t.IsSubclassOf(baseType)).ToArray();

                Trace.TraceInformation("Subclasses of {1}: {0}", string.Join(", ", subclasses.Select(t => t.Name)), baseType.Name);

                return subclasses.Select(TypeReference.GetType).ToArray();
            }
            catch (ReflectionTypeLoadException ex)
            {
                Trace.TraceError(
                    "Unable to retrieve some types from assemblies {0} because of error {1}. Inner exceptions: {2}",
                    string.Join(", ", testAssemblies),
                    ex.Message,
                    string.Join(Environment.NewLine, ex.LoaderExceptions.Cast<Exception>()));
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to retrieve some types from assemblies {0} because of error {1}", string.Join(", ", testAssemblies), ex);
            }

            return new TypeReference[0];
        }

        public TypeReference[] FindParsersAvailable()
        {
            return FindAllTypes(BaseParserType);
        }

        public TypeReference[] FindTestTypes()
        {
            return FindAllTypes(TestContainerType);
        }

        private static bool CanBeConstructed(Type t)
        {
            return !(t.IsGenericType || t.IsAbstract || t.IsValueType);
        }

        public IsolatedParser[] CreateParsers(TypeReference[] types)
        {
            return types.Select(IsolatedParser.CreateOrNull).Where(p=>p!=null).ToArray();
        }
        
        public LazyIsolatedReference<T>[] CreateTypeInstances<T>(TypeReference[] targetTypes)
        {
            return targetTypes.Select(t => new LazyIsolatedReference<T>(t)).ToArray();
        }

        public FunctionMetaData[] FindFunctionsAvailable(LazyIsolatedReference<BaseTestContainer> testContainer)
        {
            var functions = new List<FunctionMetaData>();
            
            var targetType = testContainer.Type.TargetType;

            try
            {
                var availableTests = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                               .Where(f => !ignoredFunctions.Contains(f.Name));

                var availableFunctions = availableTests.Select(t => new FunctionMetaData(t, testContainer));

                Trace.TraceInformation("Type {0} contains following public functions: {1}", targetType.Name, availableFunctions);

                functions.AddRange(availableFunctions);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to list functions of type {0} because of errors: {1}", targetType.Name, ex);
                Trace.Flush();
            }

            return functions.ToArray();
        }

        public IsolatedReference<TType> CreateOnTestDomain<TType>(TType value)
        {
            return new IsolatedReference<TType>(value);
        }

        public TypeReference CreateTypeOnTestDomain(Type type)
        {
            return TypeReference.GetType(type);
        }

        public void SendStatistic(TestCounts counts)
        {
            TestStatistic.GlobalStatisticInternal = counts.ToTestStatistic();
        }
    }
}
