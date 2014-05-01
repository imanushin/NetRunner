using System;
using System.Collections.Generic;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Keywords
{
    internal abstract class AbstractKeyword : BaseReadOnlyObject
    {
        public abstract ReadOnlyList<HtmlCell> PatchedCells
        {
            get;
        }

        public virtual InvokationResult InvokeFunction(Func<InvokationResult> func, TestFunctionReference targetFunction)
        {
            return func();
        }
    }
}
