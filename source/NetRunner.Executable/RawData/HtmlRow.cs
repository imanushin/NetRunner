using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation;

namespace NetRunner.Executable.RawData
{
    internal sealed class HtmlRow : BaseReadOnlyObject
    {
        public HtmlRow(IEnumerable<HtmlCell> cellsEntries, HtmlRowReference rowReference)
        {
            Cells = cellsEntries.ToReadOnlyList();
            RowReference = rowReference;
        }

        public HtmlRowReference RowReference
        {
            get;
            private set;
        }

        public ReadOnlyList<HtmlCell> Cells
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return RowReference;
            yield return Cells;
        }
    }
}
