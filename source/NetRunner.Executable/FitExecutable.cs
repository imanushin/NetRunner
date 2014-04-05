using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Invokation;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable
{
    internal static class FitExecutable
    {
        private const string suiteSetupIdentifier = "suitesetup";

        internal static void Execute(ApplicationSettings settings)
        {
            var communicator = new FitnesseCommunicator(settings.Host, settings.Port, settings.SocketToken);

            ProcessTestDocuments(communicator, settings.Assemblylist);
        }

        public static void ProcessTestDocuments(FitnesseCommunicator communicator, string assemblylist)
        {
            var loader = new ReflectionLoader(assemblylist.Split(new[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()));

            bool suiteIsAbandoned = false;
            bool maybeProcessingSuiteSetup = true;

            for (string document = communicator.ReceiveDocument(); document.Any() && !suiteIsAbandoned; document = communicator.ReceiveDocument())
            {
                Trace.WriteLine("Processing document of size: " + document.Length);

                try
                {
                    var counts = new TestCounts();

                    var parsedDocument = HtmlParser.Parse(document);

                    communicator.SendDocument(parsedDocument.TextBeforeFirstTable);

                    /*init test context*/

                    Trace.WriteLine("test...");

                    if ( /*is sute setup?*/false || maybeProcessingSuiteSetup)
                    {
                        /*execute setup*/
                    }
                    else
                    {
                        /*execute test*/
                    }

                    foreach (var table in parsedDocument.Tables)
                    {
                        var result = RootInvoker.InvokeTable(table, counts, loader);

                        communicator.SendDocument(result + table.TextAfterTable);
                    }

                    communicator.SendCounts(counts);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Test execution exception: {0}", e);
                    /*var testStatus = new TestStatus();
                    var parse = new CellBase(parseError, "div");
                    parse.SetAttribute(CellAttribute.Body, parseError);
                    testStatus.MarkException(parse, e);
                    writer.WriteTable(new CellTree(parse));
                    writer.WriteTest(new CellTree().AddBranchValue(parse), testStatus.Counts);*/
                }

                maybeProcessingSuiteSetup = false;
            }

            Trace.WriteLine("Completion signal received");

            if (suiteIsAbandoned)
                throw new InvalidOperationException("Suite is abadoned by caller");
        }

        private static void ProcessTestDocument(string document)
        {
        }

    }
}
