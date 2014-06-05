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

        public ExecutionResult ExecuteAfterFunctionCallMethod(FunctionMetaData function)
        {
            return Execute(function.Method, baseTableArgument.NotifyAfterFunctionCall);
        }

        public ExecutionResult ExecuteBeforeFunctionCallMethod(FunctionMetaData function)
        {
            return Execute(function.Method, baseTableArgument.NotifyBeforeFunctionCall);
        }

        private ExecutionResult Execute(MethodInfo functionReference, Func<MethodInfo, Exception> function)
        {
            var result = function(functionReference);

            if (result != null)
            {
                return ExecutionResult.FromException(result);
            }

            return new ExecutionResult(new IsolatedReference<object>(null));
        }
    }
}
