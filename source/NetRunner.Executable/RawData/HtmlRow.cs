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
        internal const string GlobalAttributeIndexName = "GlobalRowIndex";

        public HtmlRow(IEnumerable<HtmlCell> cellsEntries, int rowGlobalIndex)
        {
            Cells = cellsEntries.ToReadOnlyList();
            RowGlobalIndex = rowGlobalIndex;
        }

        public int RowGlobalIndex
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
            yield return RowGlobalIndex;
            yield return Cells;
        }
    }
}
