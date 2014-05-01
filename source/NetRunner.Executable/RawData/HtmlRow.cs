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
        public HtmlRow(IReadOnlyCollection<HtmlCell> cellsEntries, HtmlRowReference rowReference)
        {
            Validate.CollectionArgumentHasElements(cellsEntries, "cellsEntries");
            Validate.ArgumentIsNotNull(rowReference, "rowReference");

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
