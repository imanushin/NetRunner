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

        public ReflectionInvoker()
        {
            if (Trace.Listeners.Count == 0)
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
            }
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

            var assemblyContent = File.ReadAllBytes(assemblyPath);

            testAssemblies.Add(Assembly.Load(assemblyContent));
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
                return testAssemblies.SelectMany(a => a.GetTypes())
                                     .Where(CanBeConstructed)
                                     .Where(t => t.IsSubclassOf(baseType))
                                     .Select(t => new TypeReference(t))
                                     .ToArray();
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

        public IsolatedReference<T>[] CreateTypeInstances<T>(TypeReference[] targetTypes)
        {
            var result = new List<IsolatedReference<T>>();

            foreach (var typeReference in targetTypes)
            {
                try
                {
                    var constructor = typeReference.TargetType.GetConstructor(new Type[0]);

                    if (constructor == null)
                    {
                        Trace.TraceError("Unable to create type {0}: unable to find any constructor without parameters", typeReference.TargetType);

                        continue;
                    }

                    var newObject = (T)constructor.Invoke(new object[0]);

                    result.Add(new IsolatedReference<T>(newObject));
                }
                catch (TargetInvocationException ex)
                {
                    Trace.TraceError("Unable to create instance of type {0} because of error: {1}", typeReference.TargetType, ex.InnerException);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to create instance of type {0} because of error: {1}", typeReference.TargetType, ex);
                }
            }

            return result.ToArray();
        }

        public FunctionMetaData[] FindFunctionsAvailable(IsolatedReference<BaseTestContainer> testContainer)
        {
            var functions = new List<FunctionMetaData>();

            if (testContainer.IsNull)
            {
                Trace.TraceError("Internal error: unable to get items from container, because it is null");

                return functions.ToArray();
            }

            BaseTestContainer localContainer = testContainer.Value;

            var targetType = localContainer.GetType();

            try
            {
                var availableTests = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                               .Where(f => !ignoredFunctions.Contains(f.Name));

                var availableFunctions = availableTests.Select(t => new FunctionMetaData(t));

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
    }
}
