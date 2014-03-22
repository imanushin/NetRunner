using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace NetRunner.Executable.RawData
{
    internal static class HtmlParser
    {
        public static IReadOnlyCollection<HtmlTable> Parse(string htmlDocument)
        {
            var document = new HtmlDocument();

            document.LoadHtml(htmlDocument);

            var allChildNodes = document.DocumentNode.ChildNodes;

            var tables = allChildNodes.Where(cn => string.Equals(cn.Name, "table", StringComparison.OrdinalIgnoreCase));

            return tables.Select(ParseTable).ToArray();
        }

        private static HtmlTable ParseTable(HtmlNode tableNode)
        {
            var allRows = tableNode.ChildNodes.Where(cn => string.Equals(cn.Name, "tr", StringComparison.OrdinalIgnoreCase));

            var parsedRows = allRows.Select(ParseRow).ToArray();

            return new HtmlTable(parsedRows);
        }

        private static HtmlRow ParseRow(HtmlNode rowNode)
        {
            var allCells = rowNode.ChildNodes.Where(cn => string.Equals(cn.Name, "tr", StringComparison.OrdinalIgnoreCase));

            var cellsEntries = allCells.Select(cell=>cell.InnerText).ToArray();

            return new HtmlRow(cellsEntries);
        }
    }
}
