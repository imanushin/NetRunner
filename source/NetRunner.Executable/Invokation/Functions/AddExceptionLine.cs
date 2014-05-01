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
    internal sealed class AddExceptionLine : AbstractTableChange
    {
        private readonly HtmlRowReference rowReference;
        private readonly string header;
        private readonly ReadOnlyList<Exception> exceptions;

        public AddExceptionLine(string header, Exception exception, HtmlRowReference rowReference)
            : this(header, new[] { exception }, rowReference)
        {
            Validate.ArgumentIsNotNull(exception, "exception");
        }

        public AddExceptionLine(string header, IReadOnlyCollection<Exception> exceptions, HtmlRowReference rowReference)
        {
            Validate.ArgumentIsNotNull(rowReference, "rowReference");
            Validate.ArgumentStringIsMeanful(header, "header");
            Validate.CollectionArgumentHasElements(exceptions, "exceptions");

            this.header = header;
            this.exceptions = exceptions.ToReadOnlyList();
            this.rowReference = rowReference;

            Validate.CollectionArgumentHasElements(this.exceptions, "exceptions");
        }

        public AddExceptionLine(Exception exception, HtmlRowReference rowReference)
            : this(new[] { exception }, rowReference)
        {
            Validate.ArgumentIsNotNull(exception, "exception");
        }

        public AddExceptionLine(IReadOnlyCollection<Exception> exceptions, HtmlRowReference rowReference)
            : this("Test failed with exceptions", exceptions, rowReference)
        {
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return rowReference;
            yield return header;
            yield return exceptions;
        }

        public override void PatchHtmlTable(HtmlNode node)
        {
            AddExpandableRow(node, rowReference, header, string.Join("<br/>", exceptions), HtmlParser.ErrorCssClass);
        }

        public static string FormatExceptionHeader(Exception exception)
        {
            return string.Format("Function execution failed with error {0}", exception.GetType().Name);
        }
    }
}
