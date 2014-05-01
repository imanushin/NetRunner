using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class AddCellExpandableInfo : AbstractTableChange
    {
        private readonly HtmlCell cell;
        private readonly string header;
        private readonly string info;

        public AddCellExpandableInfo(HtmlCell cell, string header, string info)
        {
            this.cell = cell;
            this.header = header;
            this.info = info;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return cell;
            yield return header;
            yield return info;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var targetNode = cell.FindMyself(table);

            AddExpandableDivToCell(header, info, targetNode);
        }
    }
}
