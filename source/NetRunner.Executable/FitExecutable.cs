using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation;
using NetRunner.Executable.RawData;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable
{
    internal static class FitExecutable
    {
        internal static void Execute(ApplicationSettings settings)
        {
            using (var communicator = new FitnesseCommunicator(settings.Host, settings.Port, settings.SocketToken))
            {
                ProcessTestDocuments(communicator, settings.Assemblylist);
            }
        }

        private static void ProcessTestDocuments(FitnesseCommunicator communicator, string assemblylist)
        {
            ReflectionLoader.AddAssemblies(assemblylist.Split(new[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToReadOnlyList());

            ReflectionLoader.CreateNewApplicationDomain();

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

            Trace.WriteLine("Completion signal received");
        }
    }
}
