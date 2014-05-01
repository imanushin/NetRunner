using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class ExecutionFailedMessage : AbstractTableChange
    {
        private readonly HtmlRowReference rowReference;
        private readonly string message;
        private readonly string header;

        [StringFormatMethod("messageFormat")]
        public ExecutionFailedMessage(HtmlRowReference rowReference, string header, string messageFormat, params object[] args)
        {
            Validate.ArgumentIsNotNull(rowReference, "rowReference");
            Validate.ArgumentStringIsMeanful(header, "header");
            Validate.ArgumentStringIsMeanful(messageFormat, "messageFormat");

            this.rowReference = rowReference;
            this.message = string.Format(messageFormat, args);
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
