﻿using System;
using System.Collections;
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
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation
{
    internal static class ReflectionLoader
    {
        private static readonly Type testContainerType = typeof(BaseTestContainer);
        private static readonly Type reflectionInvokerType = typeof(ReflectionInvoker);

        private static AppDomain currentTestDomain;

        [NotNull]
        private static ReadOnlyList<string> assemblyList = ReadOnlyList<string>.Empty;

        [NotNull]
        private static ReadOnlyList<string> assemblyFolders = ReadOnlyList<string>.Empty;

        [NotNull]
        private static ReadOnlyList<TestFunctionReference> functions = ReadOnlyList<TestFunctionReference>.Empty;

        [NotNull]
        private static ReadOnlyList<IsolatedReference<BaseTestContainer>> testContainers = ReadOnlyList<IsolatedReference<BaseTestContainer>>.Empty;

        [CanBeNull]
        private static ReflectionInvoker reflectionInvoker;

        public static void AddAssemblies(IReadOnlyCollection<string> assemblyPathes)
        {
            var allAssemblyFiles = assemblyList.Concat(assemblyPathes).Distinct(StringComparer.OrdinalIgnoreCase).Where(af => !String.IsNullOrWhiteSpace(af)).ToReadOnlyList();

            assemblyList = allAssemblyFiles.Where(File.Exists).ToReadOnlyList();

            var missingFiles = allAssemblyFiles.Where(af => !File.Exists(af)).ToReadOnlyList();

            if (missingFiles.Any())
            {
                Trace.TraceWarning("Unable to find some of assembly files: {0}", missingFiles);
            }

            assemblyFolders = assemblyList.Select(Path.GetDirectoryName).Distinct(StringComparer.OrdinalIgnoreCase).ToReadOnlyList();

            Trace.TraceInformation("Additional folder for assembly loading: {0}", assemblyFolders.JoinToStringLazy("; "));
        }

        private static void ReloadAssemblies()
        {
            Trace.TraceInformation("Start assembly loading from list: {0}", assemblyList);

            Validate.IsNotNull(reflectionInvoker, "Test domain was not initialized");

            reflectionInvoker.AddAssemblyLoadFolders(assemblyFolders.ToArray());

            assemblyList.ForEach(reflectionInvoker.LoadTestAssembly);

            var testTypes = reflectionInvoker.FindTestTypes().ToReadOnlyList();
            var parserTypes = reflectionInvoker.FindParsersAvailable().ToReadOnlyList();

            testContainers = reflectionInvoker.CreateTypeInstances<BaseTestContainer>(testTypes.ToArray()).ToReadOnlyList();

            functions = testContainers
                .SelectMany(tc => reflectionInvoker.FindFunctionsAvailable(tc).Select(f => new TestFunctionReference(f, tc.CastToFunctionContainer())))
                .ToReadOnlyList();

            var parsersFound = reflectionInvoker.CreateParsers(parserTypes.ToArray()).ToList();

            parsersFound.Sort((first, second) => second.ExecuteProperty<int>("Priority") - first.ExecuteProperty<int>("Priority"));

            Parsers = parsersFound.ToReadOnlyList();

            Trace.TraceInformation("All available functions: {0}", functions.JoinToStringLazy(Environment.NewLine));

            TrueResult = reflectionInvoker.CreateOnTestDomain(true);
            FalseResult = reflectionInvoker.CreateOnTestDomain(false);
            EnumerableType = reflectionInvoker.CreateTypeOnTestDomain(typeof(IEnumerable));
            TableArgumentType = reflectionInvoker.CreateTypeOnTestDomain(typeof(BaseTableArgument));
        }

        private static string LoadConfigurationIfNeeded()
        {
            const string configSuffix = ".config";

            var configurationsAvailable = assemblyList.Select(path => new FileInfo(path + configSuffix)).Where(f => f.Exists).ToReadOnlyList();

            if (!configurationsAvailable.Any())
                return String.Empty;

            if (configurationsAvailable.Count > 1)
            {
                Trace.TraceInformation("Load configuration step skipped because more than one (actually - {0}) configuration files are available: {1}", configurationsAvailable.Count, configurationsAvailable);

                return String.Empty;
            }

            var configurationFile = configurationsAvailable.First().FullName;
            string targetAssemblyFile = configurationFile.Substring(0, configurationFile.Length - configSuffix.Length);

            Trace.TraceInformation("Configuration will be loaded for assembly '{0}' (file '{1}')", targetAssemblyFile, configurationFile);

            return configurationFile;
        }

        public static ReadOnlyList<IsolatedParser> Parsers
        {
            get;
            private set;
        }

        public static ReadOnlyList<string> TestContainerNames
        {
            get
            {
                return testContainers.Select(tc => tc.GetTypeFullName()).ToReadOnlyList();
            }
        }

        public static ReadOnlyList<string> AssemblyPathes
        {
            get
            {
                return assemblyList;
            }
        }

        [CanBeNull]
        public static TestFunctionReference FindFunction(ReadOnlyList<string> argumentNames, TableResultReference targetObject)
        {
            var allMethods = targetObject.GetMethods();

            var methodCandidates =
                allMethods.Where(m => m.GetParameters().Select(p => p.Name)
                    .SequenceEqual(argumentNames, StringComparer.OrdinalIgnoreCase));

            string joinedNames = TestFunctionReference.CleanFunctionName(string.Concat(argumentNames));

            methodCandidates = methodCandidates.Concat(
                allMethods
                .Where(m => m.ParametersCount == argumentNames.Count)
                .Where(m => m.AvailableFunctionNames.Any(fn => string.Equals(fn, joinedNames, StringComparison.OrdinalIgnoreCase))));

            var firstCandidate = methodCandidates.FirstOrDefault();

            if (firstCandidate == null)
                return null;

            return new TestFunctionReference(firstCandidate, targetObject);
        }

        [CanBeNull]
        public static TestFunctionReference FindFunction(string name, int argumentCount)
        {
            //ToDo: Change to dictionary to avoid multiple enumerations
            return functions.FirstOrDefault(f =>
                f.ArgumentTypes.Count == argumentCount &&
                f.AvailableFunctionNames.Any(fn => string.Equals(fn, name, StringComparison.OrdinalIgnoreCase)));
        }

        public static bool TryReadPropery(GeneralIsolatedReference targetObject, string propertyName, [CanBeNull]  out TypeReference propertyType, [CanBeNull] out GeneralIsolatedReference resultValue)
        {
#warning change to Table changes
            var targetType = targetObject.GetType();

            var property = targetType.GetProperty(propertyName);

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
                ParametersConverter.ResetParsers();
            }

            var evidence = AppDomain.CurrentDomain.Evidence;

            var setupInformation = AppDomain.CurrentDomain.SetupInformation;

            var configFileCandidate = LoadConfigurationIfNeeded();

            if (!String.IsNullOrWhiteSpace(configFileCandidate))
            {
                setupInformation.ConfigurationFile = configFileCandidate;
            }

            setupInformation.ApplicationBase = GetFolderRoot();

            currentTestDomain = AppDomain.CreateDomain("Test execution domain", evidence, setupInformation);

            var currentAssembly = Assembly.GetExecutingAssembly();

            var loadingType = typeof(InMemoryAssemblyLoader);

            Validate.Condition(loadingType.Assembly == currentAssembly, "Assembly of type '{0}' is '{1}' which is not equal with current '{2}'", loadingType, loadingType.Assembly, currentAssembly);

            var createdInstance = (InMemoryAssemblyLoader)currentTestDomain.CreateInstanceFrom(currentAssembly.Location, loadingType.FullName).Unwrap();

            Validate.IsNotNull(createdInstance, "Unable to create instance of type {0} in the test domain", loadingType);

            createdInstance.SubscribeDomain(currentTestDomain);

            reflectionInvoker = (ReflectionInvoker)currentTestDomain.CreateInstanceAndUnwrap(reflectionInvokerType.Assembly.FullName, reflectionInvokerType.FullName);

            ReloadAssemblies();
        }

        private static string GetFolderRoot()
        {
            var existingAssembly = assemblyList.Where(File.Exists)
                .Select(a => new FileInfo(a).Directory)
                .SkipNulls()
                .Select(d => d.FullName)
                .FirstOrDefault();

            return existingAssembly ?? Environment.CurrentDirectory;
        }

        public static IsolatedReference<bool> TrueResult
        {
            get;
            private set;
        }

        public static IsolatedReference<bool> FalseResult
        {
            get;
            private set;
        }

        public static TypeReference EnumerableType
        {
            get;
            private set;
        }

        public static TypeReference TableArgumentType
        {
            get;
            private set;
        }

        public static IsolatedReference<TType> CreateOnTestDomain<TType>([CanBeNull] TType value)
        {
            return reflectionInvoker.CreateOnTestDomain(value);
        }

        public static void UpdateCounts(TestCounts counts)
        {
            Validate.ArgumentIsNotNull(counts, "counts");
            Validate.IsNotNull(reflectionInvoker, "Test domain does not initialized");

            reflectionInvoker.SendStatistic(counts);
        }
    }
}
