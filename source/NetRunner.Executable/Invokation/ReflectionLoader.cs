﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary;

namespace NetRunner.Executable.Invokation
{
    internal sealed class ReflectionLoader
    {
        private static readonly Type testContainer = typeof(BaseTestContainer);
        private ReadOnlyList<TestFunctionReference> functions;

        public ReflectionLoader(IEnumerable<string> assemblyPathes)
        {
            var pathes = assemblyPathes.ToReadOnlyList();

            Trace.TraceInformation("Start assembly loading from list: {0}", pathes);

            var loadedAssemblies = LoadAssemblies(pathes);

            var assemblyFolders = loadedAssemblies
                .Select(a => Path.GetDirectoryName(a.Location))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToReadOnlyList();

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => LoadFrom(assemblyFolders, args);

            Trace.TraceInformation("Additional folder for assembly loading: {0}", assemblyFolders);

            var testTypes = FindTestTypes(loadedAssemblies);

            var testContainers = CreateTestContainers(testTypes);

            functions = FindFunctionsAvailable(testContainers);

            Trace.TraceInformation("All available functions: {0}", functions.JoinToStringLazy(Environment.NewLine));
        }

        public ReadOnlyList<TestFunctionReference> FindFunctions(string name, int argumentCount)
        {
            return functions
                .Where(f => f.ArgumentTypes.Count == argumentCount && string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase))
                .ToReadOnlyList();
        }

        private static ReadOnlyList<TestFunctionReference> FindFunctionsAvailable(ReadOnlyList<BaseTestContainer> testContainers)
        {
            var functions = new List<TestFunctionReference>();

            foreach (BaseTestContainer container in testContainers)
            {
                var targetType = container.GetType();

                var availableTests = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                functions.AddRange(availableTests.Select(t => new TestFunctionReference(t, container)));
            }

            return functions.ToReadOnlyList();
        }

        private static ReadOnlyList<BaseTestContainer> CreateTestContainers(ReadOnlyList<Type> testTypes)
        {
            var testContainers = new List<BaseTestContainer>();

            foreach (Type testType in testTypes)
            {
                try
                {
                    var constructor = testType.GetConstructors().FirstOrDefault(ct => !ct.GetParameters().Any());

                    Validate.IsNotNull(constructor, "Unable to find constructor without parameters for type {0}", testType.Name);

                    var targetObject = (BaseTestContainer)constructor.Invoke(new object[0]);

                    testContainers.Add(targetObject);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to create instance of type {0} because of error: {1}", testType, ex);
                }
            }
            return testContainers.ToReadOnlyList();
        }

        private static Assembly LoadFrom(ReadOnlyList<string> assemblyFolders, ResolveEventArgs args)
        {
            var targetLocation = args.RequestingAssembly.Location;

            var targetFileName = Path.GetFileNameWithoutExtension(targetLocation) + ".dll";

            var filesCandidates = assemblyFolders.Select(f => Path.Combine(f, targetFileName)).Where(File.Exists);

            foreach (var candidate in filesCandidates)
            {
                try
                {
                    return Assembly.LoadFrom(candidate);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to load assembly {0} from file {1}: {2}", args.Name, candidate, ex);
                }
            }

            return null;
        }

        private static ReadOnlyList<Type> FindTestTypes(List<Assembly> loadedAssemblies)
        {
            var testContainers = new List<Type>();

            foreach (Assembly assembly in loadedAssemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => !(t.IsGenericType || t.IsAbstract || t.IsValueType))
                    .Where(t => t.IsSubclassOf(testContainer)).ToReadOnlyList();

                Trace.TraceInformation("Test containers from assembly {1}: {0}", types, assembly);

                testContainers.AddRange(types);
            }

            var result = testContainers.Distinct().ToReadOnlyList();

            Trace.TraceInformation("All test containers: {0}", string.Join(", ", result.Select(t => t.Name)));
            return result;
        }

        private static List<Assembly> LoadAssemblies(ReadOnlyList<string> pathes)
        {
            var loadedAssemblies = new List<Assembly>();

            foreach (string assemblyPath in pathes)
            {
                try
                {
                    if (!File.Exists(assemblyPath))
                    {
                        Trace.TraceError("Unable to load assembly because it does not exist: '{0}'", assemblyPath);

                        continue;
                    }

                    var loadedAssembly = Assembly.LoadFile(assemblyPath);

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
    }
}
