using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public sealed class ExecutionResult : MarshalByRefObject
    {
        public static readonly ExecutionResult Empty = new ExecutionResult(GeneralIsolatedReference.Empty);

        internal ExecutionResult([CanBeNull]GeneralIsolatedReference result)
        {
            Result = result;
        }

        public ExecutionResult(string exceptionMessage, string exceptionType, string exceptionToString)
        {
            ExceptionMessage = exceptionMessage;
            ExceptionType = exceptionType;
            ExceptionToString = exceptionToString;
        }

        public GeneralIsolatedReference Result
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

        public bool HasError
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ExceptionToString);
            }
        }

        public static ExecutionResult FromException(Exception ex)
        {
            var targetInvocationException = ex as TargetInvocationException;

            if (targetInvocationException != null && targetInvocationException.InnerException != null)
            {
                return FromException(targetInvocationException.InnerException);
            }

            return new ExecutionResult(ex.Message, ex.GetType().Name, ex.ToString());
        }
    }
}
