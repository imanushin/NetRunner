using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public sealed class ExecutionResult : GeneralReferenceObject
    {
        public static readonly ExecutionResult Empty = new ExecutionResult(GeneralIsolatedReference.Empty, new ParameterValue[0]);

        internal ExecutionResult([NotNull]GeneralIsolatedReference result, IEnumerable<ParameterValue> outParameters)
        {
            Result = result;
            OutParameters = outParameters.ToArray();
        }

        private ExecutionResult(string exceptionMessage, string exceptionType, string exceptionToString)
        {
            ExceptionMessage = exceptionMessage;
            ExceptionType = exceptionType;
            ExceptionToString = exceptionToString;
        }

        [NotNull]
        public GeneralIsolatedReference Result
        {
            get;
            private set;
        }

        [NotNull]
        public ParameterValue[] OutParameters
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
