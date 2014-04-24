using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation
{
    internal abstract class AbstractKeyword
    {
        public abstract ReadOnlyList<HtmlCell> PatchCells(IReadOnlyCollection<HtmlCell> originalCells);
    }
}
