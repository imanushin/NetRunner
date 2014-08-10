using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
        private Uri pathToFitNesseRoot;

        private TestResults testResult;
        private SingleTest currentTest;
        private string rawResult = string.Empty;

        public MainTestContainer()
        {
        }

        public void ExecutePage(string pageLocalUrl)
        {
            var currentAssemblyLocation = new FileInfo(Assembly.GetExecutingAssembly().Location);

            var folderRoot = currentAssemblyLocation.Directory.Parent.Parent;

            var fitNesseRoot = folderRoot.GetDirectories("fitnesse").First();

            pageLocalUrl = pageLocalUrl.Trim();

            if (pageLocalUrl.StartsWith("."))
                pageLocalUrl = pageLocalUrl.Substring(1);

            var resultXmlFile = new FileInfo(Path.Combine(folderRoot.FullName, "localTestResults.xml"));

            if (resultXmlFile.Exists)
            {
                resultXmlFile.Delete();
            }

            var executionLine = string.Format("-jar fitnesse-standalone.jar -c \"{0}?suite&format=xml\" -b \"{1}\"", pageLocalUrl, resultXmlFile.FullName);

            Trace.TraceInformation("Execution line: {0}", executionLine);

            var processStartInfo = new ProcessStartInfo("java.exe", executionLine);

            processStartInfo.WorkingDirectory = fitNesseRoot.FullName;

            var newProcess = Process.Start(processStartInfo);

            newProcess.WaitForExit();

            rawResult = File.ReadAllText(resultXmlFile.FullName);

            testResult = new TestResults(rawResult);

            InitTest("1");
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

        /// <summary>
        /// Checks how often target text exists
        /// </summary>
        /// <returns></returns>
        public RawResultAnalyzer AnalyzeHtml()
        {
            return new RawResultAnalyzer(rawResult);
        }

        public IEnumerable CurrentTestResults()
        {
            return currentTest.Counts.Select(c => new
            {
                Type = c.Key,
                Count = c.Value
            });
        }

        public TableAnalyser AnalyzeTable(int tableIndex)
        {
            return new TableAnalyser(new[] { currentTest.Tables[tableIndex - 1] });
        }

        public TableAnalyser AnalyzeAllTablesInAllTests()
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
            return TestStatistic.CurrentTestStatistic.Errors;
        }

        public int FailCountIs()
        {
            return TestStatistic.CurrentTestStatistic.Wrong;
        }

        public int IgnoresCountIs()
        {
            return TestStatistic.CurrentTestStatistic.Skipped;
        }

        public bool SuccessGreaterThanZero()
        {
            return TestStatistic.CurrentTestStatistic.Right > 0;
        }

        public bool SuiteResultsGreaterThatLocalResults()
        {
            var suiteResults = TestStatistic.SuiteStatistic;
            var testResults = TestStatistic.CurrentTestStatistic;

            return
                suiteResults.Errors >= testResults.Errors &&
                suiteResults.Right >= testResults.Right &&
                suiteResults.Skipped >= testResults.Skipped &&
                suiteResults.Wrong >= testResults.Wrong;

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
