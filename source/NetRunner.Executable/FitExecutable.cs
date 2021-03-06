﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation;
using NetRunner.Executable.Invokation.Documentation;
using NetRunner.Executable.Properties;
using NetRunner.Executable.RawData;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable
{
    internal static class FitExecutable
    {
        private const string netRunnerExternallibrary = "NetRunner.ExternalLibrary";

        internal static void Execute(ApplicationSettings settings)
        {
            using (var communicator = new FitnesseCommunicator(settings.Host, settings.Port, settings.SocketToken))
            {
                try
                {
                    ProcessTestDocuments(communicator, settings.Assemblylist);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unhandled exception: " + ex);
                }
            }
        }

        private static void ProcessTestDocuments(FitnesseCommunicator communicator, string assemblylist)
        {
            var assemblyPathes = assemblylist.Split(new[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToReadOnlyList();

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var assemblyName = new AssemblyName(args.Name).Name;

                if (!string.Equals(assemblyName, netRunnerExternallibrary, StringComparison.Ordinal))
                    return null;

                var externalLibraryFile = assemblyPathes
                    .Select(Path.GetDirectoryName)
                    .Select(d => Path.Combine(d, netRunnerExternallibrary + ".dll"))
                    .FirstOrDefault(File.Exists);

                if (string.IsNullOrEmpty(externalLibraryFile))
                {
                    return Assembly.Load(Resources.NetRunner_ExternalLibrary);
                }

                return Assembly.LoadFrom(externalLibraryFile);
            };

            ReflectionLoader.AddAssemblies(assemblyPathes);

            ReflectionLoader.CreateNewApplicationDomain();

            ReflectionLoader.UpdateDatas();

            DocumentationStore.LoadForAssemblies(ReflectionLoader.LoadedAssemblies);

            communicator.SendDocument(EngineInfo.PrintTestEngineInformation());

            var globalCounts = new TestCounts();

            for (string document = communicator.ReceiveDocument(); document.Any(); document = communicator.ReceiveDocument())
            {
                communicator.SendDocument(HtmlHintManager.TestHeader);
                
                Trace.WriteLine("Processing document of size: " + document.Length);

                var localCounts = new TestCounts();

                try
                {
                    var parsedDocument = HtmlParser.Parse(document);

                    var executionPlan = TableParser.GenerateTestExecutionPlan(parsedDocument);

                    communicator.SendDocument(executionPlan.FormatExecutionPlan());

                    communicator.SendDocument(parsedDocument.TextBeforeFirstTable);

                    foreach (var test in executionPlan.FunctionsSequence)
                    {
                        var testCounts = new TestCounts();

                        var result = RootInvoker.InvokeTable(test, testCounts);

                        localCounts.Merge(testCounts);
                        globalCounts.Merge(testCounts);

                        ReflectionLoader.UpdateCounts(globalCounts, localCounts);

                        communicator.SendDocument(result + test.Table.TextAfterTable);
                    }

                    communicator.SendDocument(HtmlParser.LineBreak);

                    communicator.SendDocument(HtmlHintManager.GetTestFooter());

                    communicator.SendCounts(localCounts);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Test execution exception: {0}", e);

                    try
                    {
                        localCounts.IncrementExceptionCount();

                        communicator.SendCounts(localCounts);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Unable to send counts '{0}': {1}", localCounts, ex);
                    }
                }
            }

            Trace.WriteLine("Execution completed");
        }
    }
}
