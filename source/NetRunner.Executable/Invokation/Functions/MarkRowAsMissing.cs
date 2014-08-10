using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class MarkRowAsMissing : AbstractTableChange
    {
        private readonly HtmlRowReference row;

        public MarkRowAsMissing(HtmlRowReference row)
        {
            this.row = row;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return row;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var targetRow = row.GetRow(table);

            var document = table.OwnerDocument;

            foreach (var node in targetRow.SelectNodesWithName(HtmlParser.TableCellNodeName))
            {
                node.AppendChild(document.CreateElement("br"));
                var missingWord = node.AppendChild(node.OwnerDocument.CreateElement("i"));
                missingWord.SetAttributeValue(HtmlParser.ClassAttributeName, "code");
                missingWord.InnerHtml = "missing";

                node.Attributes.Append(document.CreateAttribute(HtmlParser.ClassAttributeName, HtmlParser.FailCssClass));
            }
        }
    }
}
