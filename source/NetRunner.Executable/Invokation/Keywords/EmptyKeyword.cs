using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Keywords
{
    internal sealed class EmptyKeyword : AbstractKeyword
    {
        private readonly ReadOnlyList<HtmlCell> cells;

        public EmptyKeyword(IReadOnlyCollection<HtmlCell> cells)
        {
            Validate.ArgumentIsNotNull(cells, "cells");

            this.cells = cells.ToReadOnlyList();
        }


        public override ReadOnlyList<HtmlCell> PatchedCells
        {
            get
            {
                return cells;
            }
        }
    }
}
