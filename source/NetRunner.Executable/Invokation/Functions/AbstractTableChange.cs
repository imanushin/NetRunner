using System;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation.Functions
{
    internal abstract class AbstractTableChange : BaseReadOnlyObject
    {
        public abstract void PatchHtmlTable(HtmlNode table);

        protected static HtmlNode AddExpandableRow(HtmlNode table, string header, HtmlAttribute customAttribute = null)
        {
            var document = table.OwnerDocument;

            var node = document.CreateElement("tr");

            if (customAttribute != null)
                node.Attributes.Append(customAttribute);

            var cellContainer = document.CreateElement("td");

            var spanSttribute = document.CreateAttribute("colspan");

            spanSttribute.Value = table
                .ChildNodes.First(n => string.Equals(n.Name, "tr", StringComparison.OrdinalIgnoreCase))
                .ChildNodes.Count(n => string.Equals(n.Name, "td", StringComparison.OrdinalIgnoreCase)).ToString(CultureInfo.InvariantCulture);

            cellContainer.Attributes.Append(spanSttribute);

            var expandableDiv = document.CreateElement("div");

            cellContainer.AppendChild(expandableDiv);

            node.AppendChild(cellContainer);

            table.AppendChild(node);

            return expandableDiv;
        }
    }
}
