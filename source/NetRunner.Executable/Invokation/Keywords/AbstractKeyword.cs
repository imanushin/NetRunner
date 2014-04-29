using System;
using System.Collections.Generic;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Keywords
{
    internal abstract class AbstractKeyword
    {
        public abstract ReadOnlyList<HtmlCell> PatchedCells
        {
            get;
        }

        public virtual InvokationResult InvokeFunction(Func<InvokationResult> func)
        {
            return func();
        }
    }
}
