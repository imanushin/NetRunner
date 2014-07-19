using System;
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
                ProcessTestDocuments(communicator, settings.Assemblylist);
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
                    return null;
                }

                return Assembly.LoadFrom(externalLibraryFile);
            };

            ReflectionLoader.AddAssemblies(assemblyPathes);

            ReflectionLoader.CreateNewApplicationDomain();

            DocumentationStore.LoadForAssemblies(ReflectionLoader.LoadedAssemblies);

            communicator.SendDocument(DocumentationHtmlHelpers.HtmlHeader);
            communicator.SendDocument(EngineInfo.PrintTestEngineInformation());

            for (string document = communicator.ReceiveDocument(); document.Any(); document = communicator.ReceiveDocument())
            {
                Trace.WriteLine("Processing document of size: " + document.Length);

                var counts = new TestCounts();

                try
                {
                    var parsedDocument = HtmlParser.Parse(document);

                    var executionPlan = TableParser.GenerateTestExecutionPlan(parsedDocument);

                    communicator.SendDocument(executionPlan.FormatExecutionPlan());

                    communicator.SendDocument(parsedDocument.TextBeforeFirstTable);

                    foreach (var test in executionPlan.FunctionsSequence)
                    {
                        var result = RootInvoker.InvokeTable(test, counts);

                        ReflectionLoader.UpdateCounts(counts);

                        communicator.SendDocument(result + test.Table.TextAfterTable);
                    }

                    communicator.SendDocument(HtmlParser.LineBreak);

                    communicator.SendDocument(DocumentationHtmlHelpers.HtmlFooter);

                    communicator.SendCounts(counts);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Test execution exception: {0}", e);

                    try
                    {
                        counts.IncrementExceptionCount();

                        communicator.SendCounts(counts);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Unable to send counts '{0}': {1}", counts, ex);
                    }
                }
            }

            Trace.WriteLine("Execution completed");
        }
    }
}
