using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace NetRunner.InternalTests
{
    internal sealed class SingleTest
    {
        private readonly HtmlDocument document;

        public SingleTest(string testName, IReadOnlyDictionary<string, int> counts, string content)
        {
            TestName = testName;
            Counts = counts;
            Content = content;

            document = new HtmlDocument();

            document.LoadHtml(content);

            Tables = FindAllTables(document.DocumentNode).ToArray();
        }

        public string TestName
        {
            get;
            private set;
        }

        public IReadOnlyDictionary<string, int> Counts
        {
            get;
            private set;
        }

        public string Content
        {
            get;
            private set;
        }

        public HtmlNode[] Tables
        {
            get;
            private set;
        }

        private static IEnumerable<HtmlNode> FindAllTables(HtmlNode currentNode)
        {
            var tablesFound = new List<HtmlNode>();

            foreach (HtmlNode childNode in currentNode.ChildNodes)
            {
                if (String.Equals(childNode.Name, "table", StringComparison.OrdinalIgnoreCase) && !childNode.Attributes.Any())
                {
                    tablesFound.Add(childNode);

                    continue;
                }

                tablesFound.AddRange(FindAllTables(childNode));
            }

            return tablesFound;
        }
    }
}
