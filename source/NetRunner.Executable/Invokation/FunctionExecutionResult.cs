using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation
{
    internal sealed class FunctionExecutionResult : BaseReadOnlyObject
    {
        public FunctionExecutionResult(FunctionRunResult resultType, string additionalHtmlText)
        {
            ResultType = resultType;
            AdditionalHtmlText = additionalHtmlText;
        }

        public FunctionRunResult ResultType
        {
            get;
            private set;
        }

        public string AdditionalHtmlText
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return ResultType;
            yield return AdditionalHtmlText;
        }

        protected override string GetString()
        {
            return string.Format("Type: {0}; Additional info: {1}", ResultType, AdditionalHtmlText);
        }

        public enum FunctionRunResult
        {
            Fail = 0,
            Success,
            Ignore,
            Exception
        }
    }
}
