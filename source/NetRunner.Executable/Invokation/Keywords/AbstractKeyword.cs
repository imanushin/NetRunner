using System.Collections.Generic;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Keywords
{
    internal abstract class AbstractKeyword
    {
        public abstract ReadOnlyList<HtmlCell> PatchedCells
        {
            get;
        }
    }
}
