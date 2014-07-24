using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation;

namespace NetRunner.Executable.RawData
{
    internal static class HtmlParser
    {
        internal const string FailCssClass = "fail";
        internal const string ErrorCssClass = "error";
        internal const string PassCssClass = "pass";
        internal const string IgnoreCssClass = "ignore";
        internal const string ClassAttributeName = "class";

        internal const string LineBreak = "<br/>";
        internal const string BoldNodeName = "b";
        internal const string TableCellNodeName = "td";
        internal const string TableRowNodeName = "tr";
        internal const string TableNodeName = "table";

        internal const string ItemSequenceStyle = "display: inline-block;margin:0.3em";

        private static readonly Dictionary<string, string> replaceDictionary = CreateReplaceDictionary();

        public static FitnesseHtmlDocument Parse(string htmlDocument)
        {
            var document = new HtmlDocument();

            document.LoadHtml(htmlDocument);

            var allTables = FindAllTables(document.DocumentNode).ToReadOnlyList();

            if (!allTables.Any())
            {
                return new FitnesseHtmlDocument(htmlDocument, ReadOnlyList<HtmlTable>.Empty);
            }

            var parsedTables = new List<HtmlTable>();

            for (int tableIndex = 0; tableIndex < allTables.Count; tableIndex++)
            {
                var table = allTables[tableIndex];

                int tableEndPosition = table.NextSibling.StreamPosition;
                int nextTableStart = (tableIndex + 1 == allTables.Count) ? htmlDocument.Length - 1 : allTables[tableIndex + 1].StreamPosition;

                var textBetweenTables = htmlDocument.Substring(tableEndPosition, nextTableStart - tableEndPosition);

                var parsedTable = ParseTable(table, textBetweenTables);

                parsedTables.Add(parsedTable);
            }

            var firstTable = allTables.FirstOrDefault();

            Validate.IsNotNull(firstTable, "Test should have at least one table for execution");

            var textBeforeFirst = htmlDocument.Substring(0, firstTable.StreamPosition);

            return new FitnesseHtmlDocument(textBeforeFirst, parsedTables);
        }

        private static IEnumerable<HtmlNode> FindAllTables(HtmlNode currentNode)
        {
            var tablesFound = new List<HtmlNode>();

            foreach (HtmlNode childNode in currentNode.ChildNodes)
            {
                if (String.Equals(childNode.Name, TableNodeName, StringComparison.OrdinalIgnoreCase))
                {
                    tablesFound.Add(childNode);

                    continue;
                }

                tablesFound.AddRange(FindAllTables(childNode));
            }

            return tablesFound;
        }

        private static HtmlTable ParseTable(HtmlNode tableNode, string textAfterTable)
        {
            var allRows = tableNode.ChildNodes.Where(cn => String.Equals(cn.Name, TableRowNodeName, StringComparison.OrdinalIgnoreCase));

            var parsedRows = allRows.Select(ParseRow).ToArray();

            return new HtmlTable(parsedRows, tableNode, textAfterTable);
        }

        private static HtmlRow ParseRow(HtmlNode rowNode)
        {
            var allCells = rowNode.ChildNodes.Where(cn => String.Equals(cn.Name, TableCellNodeName, StringComparison.OrdinalIgnoreCase));

            return new HtmlRow(allCells.Select(cell => new HtmlCell(cell)).ToReadOnlyList(), HtmlRowReference.MarkRowAndGenerateReference(rowNode));
        }

        private static Dictionary<string, string> CreateReplaceDictionary()
        {
            var result = new Dictionary<string, string>();

          //  result.Add("\"", "&quot;");
          //  result.Add("'", "&#39;");

            foreach (var tag in new[] { "applet", "script", "style", "link", "iframe", })
            {
                result.Add( "<" + tag + ">", "&lt;" + tag + "&gt;");
                result.Add( "</" + tag + ">", "&lt;/" + tag + "&gt;");
                result.Add( "<" + tag + "/>", "&lt;" + tag + "/&gt;");
                result.Add( "<" + tag + " >", "&lt;" + tag + " &gt;");
                result.Add( "</" + tag + " >", "&lt;" + tag + " &gt;");
                result.Add( "<" + tag + " />", "&lt;" + tag + " /&gt;");
            }

            return result;
        }

        public static string ReplaceUnknownTags(string rawData)
        {
            foreach (var replacement in replaceDictionary)
            {
                rawData = rawData.Replace(replacement.Key, replacement.Value);
            }

            return rawData;
        }

    }
}
