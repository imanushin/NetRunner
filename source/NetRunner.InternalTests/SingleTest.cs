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

        private readonly HtmlNode[] tables;

        public SingleTest(string testName, IReadOnlyDictionary<string, int> counts, string content)
        {
            TestName = testName;
            Counts = counts;
            Content = content;

            document = new HtmlDocument();

            document.LoadHtml(content);

            tables = FindAllTables(document.DocumentNode).ToArray();
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

        public HtmlNode GetTable(int tableIndex)
        {
            return tables[tableIndex - 1];
        }

        private static IEnumerable<HtmlNode> FindAllTables(HtmlNode currentNode)
        {
            var tablesFound = new List<HtmlNode>();

            foreach (HtmlNode childNode in currentNode.ChildNodes)
            {
                if (String.Equals(childNode.Name, "table", StringComparison.OrdinalIgnoreCase))
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
