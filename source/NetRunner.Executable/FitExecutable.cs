using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable
{
    internal static class FitExecutable
    {
        private const string suiteSetupIdentifier = "suitesetup";

        internal static void Execute(ApplicationSettings settings)
        {
            var communicator = new FitnesseCommunicator(settings.Host, settings.Port);

            Trace.WriteLine("Host: {0}; Port: {1}");

            communicator.EstablishConnection(settings.SocketToken);
            /*
            var writer = new StoryTestStringWriter(service)
                .ForTables(WriteTables)
                .ForCounts(WriteCounts);*/

            ProcessTestDocuments(communicator);
        }

        public static void ProcessTestDocuments(FitnesseCommunicator communicator)
        {
            string document;

            bool suiteIsAbandoned = false;
            bool maybeProcessingSuiteSetup = true;

            while ((document = communicator.ReceiveDocument()).Length > 0 && !suiteIsAbandoned)
            {
                Trace.WriteLine("Processing document of size: " + document.Length);

                try
                {
                    var counts = new TestCounts();

                    var tables = HtmlParser.Parse(document);

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

                    communicator.SendDocument(@"<br/> TEST !!!!");

                    foreach (var table in tables)
                    {
                        counts.IncrementSuccessCount();    
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
