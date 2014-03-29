using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation
{
    internal abstract class AbstractTestFunction : BaseReadOnlyObject
    {
        public abstract FunctionRunResult Invoke(ReflectionLoader loader);

        public enum FunctionRunResult
        {
            Fail = 0,
            Success,
            Ignore,
            Exception
        }
    }
}
