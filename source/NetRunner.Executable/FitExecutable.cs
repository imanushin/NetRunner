﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation;
using NetRunner.Executable.RawData;

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
            ReflectionLoader.Initialize(assemblylist.Split(new[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToReadOnlyList());
            
            for (string document = communicator.ReceiveDocument(); document.Any(); document = communicator.ReceiveDocument())
            {
                Trace.WriteLine("Processing document of size: " + document.Length);

                try
                {
                    var counts = new TestCounts();

                    var parsedDocument = HtmlParser.Parse(document);

                    communicator.SendDocument(parsedDocument.TextBeforeFirstTable);

                    foreach (var table in parsedDocument.Tables)
                    {
                        var result = RootInvoker.InvokeTable(table, counts);

                        communicator.SendDocument(result + table.TextAfterTable);
                    }

                    communicator.SendDocument(HtmlParser.LineBreak);

                    communicator.SendCounts(counts);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Test execution exception: {0}", e);
                }
            }

            Trace.WriteLine("Completion signal received");
        }
    }
}
