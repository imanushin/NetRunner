using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.RawData
{
    internal sealed class HtmlRow : BaseReadOnlyObject
    {
        public HtmlRow(IEnumerable<HtmlCell> cellsEntries)
        {
            Cells = cellsEntries.ToReadOnlyList();
        }

        public ReadOnlyList<HtmlCell> Cells
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            return Cells;
        }
    }
}
