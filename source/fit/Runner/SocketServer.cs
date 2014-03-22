// Copyright © 2012 Syterra Software Inc. Includes work by Object Mentor, Inc., © 2002 Cunningham & Cunningham, Inc.
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License version 2.
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

using System;
using fitSharp.Fit.Engine;
using fitSharp.Fit.Exception;
using fitSharp.Fit.Fixtures;
using fitSharp.Fit.Model;
using fitSharp.IO;
using fitSharp.Machine.Model;

namespace fit.Runner {
    public class SocketServer {
        private const string parseError = "Unable to parse input. Input ignored.";
        private static readonly IdentifierName suiteSetupIdentifier = new IdentifierName("suitesetup");

        private readonly FitSocket socket;
        private readonly CellProcessor service;
        private readonly IProgressReporter reporter;

	    private bool maybeProcessingSuiteSetup;

        public SocketServer(FitSocket socket, CellProcessor service, IProgressReporter reporter, bool suiteSetUpIsAnonymous) {
            this.service = service;
            this.reporter = reporter;
            this.socket = socket;
            maybeProcessingSuiteSetup = suiteSetUpIsAnonymous;
        }

		public void ProcessTestDocuments(StoryTestWriter writer) {
			string document;

            while ((document = socket.ReceiveDocument()).Length > 0 && !suiteIsAbandoned) {
                reporter.WriteLine("processing document of size: " + document.Length);
                ProcessTestDocument(document, writer);
		        maybeProcessingSuiteSetup = false;
			}
		    reporter.WriteLine("\ncompletion signal received");
		    socket.Close();

            if (suiteIsAbandoned) throw new AbandonSuiteException();
		}

        private void ProcessTestDocument(string document, StoryTestWriter writer) {
			try {
			    var storyTest = new StoryTest(service, writer)
                    .WithInput(document)
                    .OnAbandonSuite(() => { suiteIsAbandoned = true; });
			    reporter.WriteLine(storyTest.Leader);
			    if (suiteSetupIdentifier.IsStartOf(storyTest.Leader) || maybeProcessingSuiteSetup)
                    storyTest.Execute();
                else
                    storyTest.Execute(new Service.Service(service));
			}
            catch (Exception e)
            {
			    var testStatus = new TestStatus();
			    var parse = new CellBase(parseError, "div");
                parse.SetAttribute(CellAttribute.Body, parseError );
			    testStatus.MarkException(parse, e);
                writer.WriteTable(new CellTree(parse));
			    writer.WriteTest(new CellTree().AddBranchValue(parse), testStatus.Counts); 
			}
		}

        bool suiteIsAbandoned;
    }
}
