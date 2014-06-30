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
        private SingleTest currentTest;

        public MainTestContainer()
        {
        }

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

        public void ExecutePageOnPort(string pageLocalUrl, int port)
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

                        InitTest("1");
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

        public bool InitTest(string testNameOrIndex)
        {
            testNameOrIndex = testNameOrIndex.Trim();

            if (testNameOrIndex.StartsWith("."))
                testNameOrIndex = testNameOrIndex.Substring(1);

            int testIndex;
            if (int.TryParse(testNameOrIndex, out testIndex))
            {
                currentTest = testResult.Tests[testIndex - 1];

                return true;
            }

            currentTest = testResult.Tests.FirstOrDefault(t => string.Equals(testNameOrIndex, t.TestName, StringComparison.OrdinalIgnoreCase));

            if (currentTest == null)
            {
                Trace.TraceError("Unable to find test {0}. Tests available: {1}", testNameOrIndex, string.Join(", ", testResult.Tests.Select(t => t.TestName)));
            }

            return currentTest != null;
        }

        public IEnumerable CurrentTestResults()
        {
            return currentTest.Counts.Select(c => new
            {
                Type = c.Key,
                Count = c.Value
            });
        }

        public TableAnalyser AnalyseTable(int tableIndex)
        {
            return new TableAnalyser(new[] { currentTest.Tables[tableIndex - 1] });
        }

        public TableAnalyser AnalyseAllTablesInAllTests()
        {
            var nodes = new HtmlNode[0];

            if (testResult != null)
            {
                nodes = testResult.Tests.SelectMany(t => t.Tables).ToArray();
            }

            return new TableAnalyser(nodes);
        }

        public int RowCountOfTableIs(int tableIndex)
        {
            return currentTest.Tables[tableIndex - 1].ChildNodes.Count(n => string.Equals(n.Name, "tr", StringComparison.OrdinalIgnoreCase));
        }

        public int ExceptionCountIs()
        {
            return TestStatistic.GlobalStatistic.Errors;
        }

        public int FailCountIs()
        {
            return TestStatistic.GlobalStatistic.Wrong;
        }

        public int IgnoresCountIs()
        {
            return TestStatistic.GlobalStatistic.Skipped;
        }

        public bool SuccessGreaterThanZero()
        {
            return TestStatistic.GlobalStatistic.Right > 0;
        }

        [StringTrim]
        public int TrimmedStringLengthOfIs(string inputData)
        {
            return inputData.Length;
        }

        [StringTrim]
        public int NonTrimmedStringLengthOfIs([StringTrim(false)] string inputData)
        {
            return inputData.Length;
        }
    }
}
