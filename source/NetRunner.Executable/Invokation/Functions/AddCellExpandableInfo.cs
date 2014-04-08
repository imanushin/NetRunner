using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class AddCellExpandableInfo : AbstractCellChange
    {
        private readonly string header;
        private readonly string info;

        public AddCellExpandableInfo(HtmlRowReference row, int column, string header, string info)
            : base(row, column)
        {
            this.header = header;
            this.info = info;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            return base.GetInnerObjects().Concat(new[]{info, header});
        }

        protected override void PatchCell(HtmlNode htmlCell)
        {
            AddExpandableDivToCell(header, info, htmlCell);
        }
    }
}
