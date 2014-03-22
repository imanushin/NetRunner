using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.RawData
{
    internal sealed class HtmlTable
    {
        public HtmlTable(IEnumerable<HtmlRow> rows)
        {
            Rows = rows.ToReadOnlyList();
        }

        public ReadOnlyList<HtmlRow> Rows
        {
            get;
            private set;
        }
    }
}
