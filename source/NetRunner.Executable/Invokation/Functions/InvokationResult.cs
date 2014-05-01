using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class InvokationResult
    {
        public InvokationResult([CanBeNull]object result, [CanBeNull] Exception exception)
        {
            Result = result;
            Exception = exception;
        }

        [CanBeNull]
        public object Result
        {
            get;
            private set;
        }

        [CanBeNull]
        public Exception Exception
        {
            get;
            private set;
        }
    }
}
