using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class AddTraceLine: AbstractTableChange
    {
        private readonly string text;

        public AddTraceLine(string text)
        {
            this.text = text;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return text;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var expandableDiv = AddExpandableRow(table, "Trace captured");

            expandableDiv.InnerHtml = text;
        }
    }
}
