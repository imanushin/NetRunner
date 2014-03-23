using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.RawData
{
    internal sealed class HtmlTable : BaseReadOnlyObject
    {
        private readonly HtmlNode tableNode;

        public HtmlTable(IEnumerable<HtmlRow> rows, HtmlNode tableNode, string textAfterTable)
        {
            TextAfterTable = textAfterTable;
            this.tableNode = tableNode;
            Rows = rows.ToReadOnlyList();
        }
        public ReadOnlyList<HtmlRow> Rows
        {
            get;
            private set;
        }

        public string TextAfterTable
        {
            get;
            private set;
        }

        public HtmlNode GetClonedNode()
        {
            return tableNode.Clone();
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Rows;
            yield return TextAfterTable;
        }
    }
}
