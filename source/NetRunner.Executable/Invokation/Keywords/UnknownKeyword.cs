using System.Collections.Generic;
using System.Linq;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Keywords
{
    internal sealed class UnknownKeyword : AbstractKeyword
    {
        private ReadOnlyList<HtmlCell> cells;

        public UnknownKeyword(IReadOnlyCollection<HtmlCell> cells)
        {
            this.cells = cells.ToReadOnlyList();
        }

        public override ReadOnlyList<HtmlCell> PatchedCells
        {
            get
            {
                return cells.Skip(1).ToReadOnlyList();
            }
        }
    }
}
