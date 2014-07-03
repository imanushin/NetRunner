using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class MarkAsBoldCellChange : AbstractTableChange
    {
        private readonly HtmlCell cell;

        public MarkAsBoldCellChange(HtmlCell cell)
        {
            this.cell = cell;
        }


        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return cell;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var node = cell.FindMyself(table);
            
            var oldHtml = node.InnerHtml;

            node.InnerHtml = string.Empty;

            var boldNode = node.OwnerDocument.CreateElement("b");

            node.AppendChild(boldNode);

            boldNode.InnerHtml = oldHtml;
        }
    }
}
