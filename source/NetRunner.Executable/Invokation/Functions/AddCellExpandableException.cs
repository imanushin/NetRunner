using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class AddCellExpandableException : AddCellExpandableInfo
    {
        public AddCellExpandableException(HtmlCell cell, Exception exception, string headerFormat, params object[] args)
            : base(cell, string.Format(headerFormat, args), CreateInfo(exception))
        {
            Validate.ArgumentIsNotNull(exception, "exception");
        }

        public AddCellExpandableException(HtmlCell cell, ExecutionResult executionResult, string headerFormat, params object[] args)
            : base(cell, string.Format(headerFormat, args), CreateInfo(executionResult))
        {
            Validate.ArgumentIsNotNull(executionResult, "executionResult");
            Validate.ArgumentCondition(executionResult.HasError, "executionResult", "Execition result should be failed");
        }

        private static string CreateInfo(ExecutionResult exception)
        {
            return string.Format("Exception:{0}{1}", Environment.NewLine, exception.ExceptionToString);
        }

        private static string CreateInfo(Exception exception)
        {
            return string.Format("Exception:{0}{1}", Environment.NewLine, exception);
        }
    }
}
