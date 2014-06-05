using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    public sealed class TableResultReference : GeneralIsolatedReference
    {
        private readonly BaseTableArgument baseTableArgument;

        internal TableResultReference(BaseTableArgument baseTableArgument)
            : base(baseTableArgument)
        {
            this.baseTableArgument = baseTableArgument;
        }

        public ExecutionResult ExecuteBeforeAllFunctionsCallMethod(FunctionMetaData method)
        {
            return method.ExecuteHandler<MethodInfo, BaseTableArgument>((ta, m) => ta.NotifyBeforeAllFunctionsCall(method.Method), method.Method);
        }

        public ExecutionResult ExecuteAfterAllFunctionsCallMethod(FunctionMetaData method)
        {
            return method.ExecuteHandler<MethodInfo, BaseTableArgument>((ta, m) => ta.NotifyAfterAllFunctionsCall(method.Method), method.Method);
        }
    }
}
