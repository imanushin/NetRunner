using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using NetRunner.ExternalLibrary;

namespace NetRunner.InternalTests
{
    internal sealed class MainTestContainer : BaseTestContainer
    {
        private Uri pathToFitnesseRoot;

        private TestResults testResult;

        public bool SetFitnessePath(Uri pathToRoot)
        {
            try
            {
                var request = WebRequest.CreateHttp(pathToRoot);

                using (var responce = request.GetResponse())
                {
                    using (var stream = responce.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to access to {0}: {1}", pathToRoot, ex);

                return false;
            }

            pathToFitnesseRoot = pathToRoot;

            return true;
        }

        public void ExecutePage(string pageLocalUrl)
        {
            pageLocalUrl = pageLocalUrl.Trim();

            if (pageLocalUrl.StartsWith("."))
                pageLocalUrl = pageLocalUrl.Substring(1);

            var testUri = new Uri(pathToFitnesseRoot, pageLocalUrl + "?test&format=xml");

            Trace.TraceInformation("Accessing url: {0}", testUri);

            var request = WebRequest.CreateHttp(testUri);

            using (var responce = request.GetResponse())
            {
                using (var stream = responce.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var result = reader.ReadToEnd();

                        Trace.TraceInformation("Test execution result: {1}{0}{1}-------------------------", result, Environment.NewLine);

                        testResult = new TestResults(result);
                    }
                }
            }
        }

        public int TestsCount()
        {
            if (testResult == null)
            {
                Trace.TraceError("No tests were executed");

                return -1;
            }

            return testResult.Tests.Count;
        }
    }
}
