using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public sealed class ExecutionResult
    {
        public ExecutionResult(IsolatedReference<object> result)
        {
            Result = result;
        }

        public ExecutionResult(string exceptionMessage, string exceptionType, string exceptionToString)
        {
            ExceptionMessage = exceptionMessage;
            ExceptionType = exceptionType;
            ExceptionToString = exceptionToString;
        }

        public IsolatedReference<object> Result
        {
            get;
            private set;
        }

        public string ExceptionMessage
        {
            get;
            private set;
        }

        public string ExceptionType
        {
            get;
            private set;
        }

        public string ExceptionToString
        {
            get;
            private set;
        }
    }
}
