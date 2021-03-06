﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class AddTraceLine : AbstractTableChange
    {
        private readonly string text;
        private readonly HtmlRowReference rowReference;

        public AddTraceLine(string text, HtmlRowReference rowReference)
        {
            Validate.ArgumentStringIsMeanful(text, "text");
            Validate.ArgumentIsNotNull(rowReference, "rowReference");

            this.text = text;
            this.rowReference = rowReference;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return rowReference;
            yield return text;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            AddExpandableRow(table, rowReference, "Trace captured", text);
        }
    }
}
