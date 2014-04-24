using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation
{
    internal sealed class UnknownKeyword : AbstractKeyword
    {
        public override ReadOnlyList<HtmlCell> PatchCells(IReadOnlyCollection<HtmlCell> originalCells)
        {
            return originalCells.Skip(1).ToReadOnlyList();
        }
    }
}
