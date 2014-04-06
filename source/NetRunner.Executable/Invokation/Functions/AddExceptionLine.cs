﻿using System;
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

            Validate.CollectionArgumentHasElements(this.exceptions, "exceptions");
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
            AddExpandableRow(node, header, string.Join("<br/>", exceptions), HtmlParser.ErrorCssClass);
        }
    }
}
