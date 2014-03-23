using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.RawData
{
    internal sealed class FitnesseHtmlDocument : BaseReadOnlyObject
    {
        public FitnesseHtmlDocument(string textBeforeFirstTable, ReadOnlyList<HtmlTable> tables)
        {
            TextBeforeFirstTable = textBeforeFirstTable;
            Tables = tables;
        }

        public string TextBeforeFirstTable
        {
            get;
            private set;
        }

        public ReadOnlyList<HtmlTable> Tables
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return TextBeforeFirstTable;
            yield return Tables;
        }
    }
}
