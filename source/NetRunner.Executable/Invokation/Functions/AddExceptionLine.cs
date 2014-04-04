using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class AddExceptionLine : AbstractTableChange
    {
        private readonly string header;
        private readonly ReadOnlyList<Exception> exceptions;

        public AddExceptionLine(string header, Exception exception)
            : this(header, new[] { exception })
        {
        }

        public AddExceptionLine(string header, IEnumerable<Exception> exceptions)
        {
            this.header = header;
            this.exceptions = exceptions.ToReadOnlyList();
        }

        public AddExceptionLine(Exception exception)
            : this(new[] { exception })
        {
        }

        public AddExceptionLine(IEnumerable<Exception> exceptions)
            : this("Test failed with exceptions", exceptions)
        {
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return header;
            yield return exceptions;
        }

        public override void PatchHtmlTable(HtmlNode node)
        {
            var redColoredAttribute = node.OwnerDocument.CreateAttribute("bgcolor");

            redColoredAttribute.Value = "#FFAAAA";

            var dataContainer = AddExpandableRow(node, header, redColoredAttribute);

            dataContainer.InnerHtml = string.Join(Environment.NewLine, exceptions);
        }
    }
}
