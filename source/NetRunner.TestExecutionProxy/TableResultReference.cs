using System;
using System.Collections.Generic;
using System.Linq;
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

        public ExecutionResult ExecuteAfterFunctionCallMethod(string displayName)
        {
            return Execute(displayName, baseTableArgument.NotifyAfterFunctionCall);
        }

        public ExecutionResult ExecuteBeforeFunctionCallMethod(string displayName)
        {
            return Execute(displayName, baseTableArgument.NotifyBeforeFunctionCall);
        }

        private ExecutionResult Execute(string displayName, Func<string, Exception> function)
        {
            var result = function(displayName);

            if (result != null)
            {
                return ExecutionResult.FromException(result);
            }

            return new ExecutionResult(new IsolatedReference<object>(null));
        }
    }
}
