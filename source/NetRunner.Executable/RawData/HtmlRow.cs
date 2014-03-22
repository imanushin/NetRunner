using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.RawData
{
    internal sealed class HtmlRow
    {
        public HtmlRow(IEnumerable<string> cellsEntries)
        {
            Cells = cellsEntries.ToReadOnlyList();
        }

        public ReadOnlyList<string> Cells
        {
            get;
            private set;
        }
    }
}
