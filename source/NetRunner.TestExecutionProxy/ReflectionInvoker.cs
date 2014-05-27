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
        private static readonly Type testContainerType = typeof(BaseTestContainer);

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
            var assemblyContent = File.ReadAllBytes(assemblyPath);

            testAssemblies.Add(Assembly.Load(assemblyContent));
        }

        public TypeReference[] FindParsersAvailable()
        {
            return testAssemblies.SelectMany(a => a.GetTypes())
                        .Where(CanBeConstructed)
                        .Where(t => t.IsSubclassOf(typeof(BaseParser)))
                        .Select(t => new TypeReference(t))
                        .ToArray();
        }

        public TypeReference[] FindTestTypes()
        {
            var containers = new List<Type>();

            foreach (Assembly assembly in testAssemblies)
            {
                try
                {
                    var types = assembly.GetTypes()
                        .Where(CanBeConstructed)
                        .Where(t => t.IsSubclassOf(testContainerType)).ToArray();

                    Trace.TraceInformation("Test containers from assembly {1}: {0}", string.Join(", ", types.Select(t => t.Name)), assembly);

                    containers.AddRange(types);
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

            var result = containers.Distinct().Select(t => t.FullName).ToArray();

            Trace.TraceInformation("All test containers: {0}", string.Join(", ", result));

            return containers.Select(t => new TypeReference(t)).ToArray();
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
