using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation;
using NetRunner.ExternalLibrary.Properties;

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

        [NotNull]
        public HtmlCell FirstBold
        {
            [Pure]
            get
            {
                var result = Cells.FirstOrDefault(c => c.IsBold);

                Validate.IsNotNull(result, "Unable to find bold cell: row contains only non-bold children: {0}", Cells);

                return result;
            }
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return RowReference;
            yield return Cells;
        }
    }
}
