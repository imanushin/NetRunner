using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.RawData
{
    internal sealed class HtmlRow
    {
        public HtmlRow(IEnumerable<string> cellsEntries)
        {
            Cells = cellsEntries.ToList().AsReadOnly();
        }

        public IReadOnlyCollection<string> Cells
        {
            get;
            private set;
        }
    }
}
