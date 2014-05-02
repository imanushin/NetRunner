using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetRunner.InternalTests
{
    internal sealed class TestResults
    {
        private readonly List<SingleTest> innerTests = new List<SingleTest>();

        public TestResults(string inputXml)
        {
            var xmlDocument = new XmlDocument();

            xmlDocument.LoadXml(inputXml);

            var rootNode = xmlDocument.ChildNodes.Cast<XmlNode>().First(n => string.Equals(n.Name, "testResults", StringComparison.Ordinal));

            var tests = rootNode.ChildNodes.Cast<XmlNode>().Where(n => string.Equals(n.Name, "rootPath", StringComparison.Ordinal)).ToArray();
            var results = rootNode.ChildNodes.Cast<XmlNode>().Where(n => string.Equals(n.Name, "result", StringComparison.Ordinal)).ToArray();

            for (int i = 0; i < tests.Length; i++)
            {
                var name = tests[i].InnerText;
                var content = results[i].ChildNodes.Cast<XmlNode>().First(n => string.Equals(n.Name, "content", StringComparison.Ordinal)).InnerText;
                var counts = results[i].ChildNodes.Cast<XmlNode>().First(n => string.Equals(n.Name, "counts", StringComparison.Ordinal));

                Dictionary<string, int> countDictionary = counts.ChildNodes.Cast<XmlNode>().ToDictionary(n => n.Name, n => int.Parse(n.InnerText));

                innerTests.Add(new SingleTest(name, countDictionary, content));
            }
        }

        public ReadOnlyCollection<SingleTest> Tests
        {
            get
            {
                return innerTests.AsReadOnly();
            }
        }
    }
}
