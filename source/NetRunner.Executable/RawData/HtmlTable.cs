using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.RawData
{
    internal sealed class HtmlTable
    {
        public HtmlTable(IEnumerable<HtmlRow> rows)
        {
            Rows = rows.ToList().AsReadOnly();
        }

        public IReadOnlyCollection<HtmlRow> Rows
        {
            get;
            private set;
        }
    }
}
