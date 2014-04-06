using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.RawData
{
    internal static class HtmlParser
    {
        private static int rowGlobalIndex = 0;

        internal const string FailCssClass = "fail";
        internal const string ErrorCssClass = "error";
        internal const string PassCssClass = "pass";
        internal const string IgnoreCssClass = "ignore";
        internal const string ClassAttributeName = "class";

        public static FitnesseHtmlDocument Parse(string htmlDocument)
        {
            var document = new HtmlDocument();

            document.LoadHtml(htmlDocument);

            var allChildNodes = document.DocumentNode.ChildNodes.ToArray();

            var tables = allChildNodes.Where(IsTableNode).ToArray();

            var parsedTables = tables
                .Select(t => (
                ParseTable(t,
                    String.Join(Environment.NewLine,
                        allChildNodes
                        .SkipWhile(n => n != t)
                        .Skip(1)
                        .TakeWhile(n => !IsTableNode(n))
                        .Select(n => n.OuterHtml)))))
                .ToReadOnlyList();

            var firstTable = tables.First();

            var nodesBeforeFirst = allChildNodes.TakeWhile(n => n != firstTable).ToArray();

            var textBeforeFirst = String.Join(Environment.NewLine, nodesBeforeFirst.Select(n => n.OuterHtml));

            return new FitnesseHtmlDocument(textBeforeFirst, parsedTables);
        }

        private static bool IsTableNode(HtmlNode cn)
        {
            return String.Equals(cn.Name, "table", StringComparison.OrdinalIgnoreCase);
        }

        private static HtmlTable ParseTable(HtmlNode tableNode, string textAfterTable)
        {
            var allRows = tableNode.ChildNodes.Where(cn => String.Equals(cn.Name, "tr", StringComparison.OrdinalIgnoreCase));

            var parsedRows = allRows.Select(ParseRow).ToArray();

            return new HtmlTable(parsedRows, tableNode, textAfterTable);
        }

        private static HtmlRow ParseRow(HtmlNode rowNode)
        {
            var allCells = rowNode.ChildNodes.Where(cn => String.Equals(cn.Name, "td", StringComparison.OrdinalIgnoreCase));

            var newIndex = rowGlobalIndex++;

            rowNode.Attributes.Append(HtmlRow.GlobalAttributeIndexName, newIndex.ToString(CultureInfo.InvariantCulture));

            return new HtmlRow(allCells.Select(cell => new HtmlCell(cell)), newIndex);
        }

    }
}
