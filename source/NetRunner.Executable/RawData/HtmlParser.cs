using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.RawData
{
    internal static class HtmlParser
    {
        public static FitnesseHtmlDocument Parse(string htmlDocument)
        {
            var document = new HtmlDocument();

            document.LoadHtml(htmlDocument);

            var allChildNodes = document.DocumentNode.ChildNodes.ToArray();

            var tables = allChildNodes.Where(cn => string.Equals(cn.Name, "table", StringComparison.OrdinalIgnoreCase)).ToArray();

            var parsedTables = tables.Select(ParseTable).ToReadOnlyList();

            var firstTable = tables.First();

            var nodesBeforeFirst = allChildNodes.TakeWhile(n => n != firstTable).ToArray();

            var textBeforeFirst = string.Join(Environment.NewLine, nodesBeforeFirst.Select(n => n.OuterHtml));

            return new FitnesseHtmlDocument(textBeforeFirst, parsedTables);
        }

        private static HtmlTable ParseTable(HtmlNode tableNode)
        {
            var allRows = tableNode.ChildNodes.Where(cn => string.Equals(cn.Name, "tr", StringComparison.OrdinalIgnoreCase));

            var parsedRows = allRows.Select(ParseRow).ToArray();

            return new HtmlTable(parsedRows, tableNode);
        }

        private static HtmlRow ParseRow(HtmlNode rowNode)
        {
            var allCells = rowNode.ChildNodes.Where(cn => string.Equals(cn.Name, "tr", StringComparison.OrdinalIgnoreCase));

            var cellsEntries = allCells.Select(cell => cell.InnerText).ToArray();

            return new HtmlRow(cellsEntries);
        }
    }
}
