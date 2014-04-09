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
    internal sealed class ExecutionFailedMessage : AbstractTableChange
    {
        private readonly HtmlRowReference rowReference;
        private readonly string message;
        private readonly string header;

        public ExecutionFailedMessage(HtmlRowReference rowReference, string message, string header)
        {
            this.rowReference = rowReference;
            this.message = message;
            this.header = header;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return rowReference;
            yield return message;
            yield return header;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var functionRow = rowReference.GetRow(table);

            var firstCell = functionRow.ChildNodes.FirstBoldCellOrNull();

            firstCell = firstCell ?? functionRow.ChildNodes.FirstCell();

            AddExpandableDivToCell(header, message, firstCell);
        }
    }
}
