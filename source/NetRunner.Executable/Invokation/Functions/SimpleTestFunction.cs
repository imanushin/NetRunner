using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class SimpleTestFunction : AbstractTestFunction
    {
        public SimpleTestFunction([NotNull] FunctionHeader header, [NotNull] TestFunctionReference functionToExecute)
        {
            Validate.ArgumentIsNotNull(header, "header");
            Validate.ArgumentIsNotNull(functionToExecute, "functionToExecute");

            Function = header;
            FunctionReference = functionToExecute;
        }

        public TestFunctionReference FunctionReference
        {
            get;
            private set;
        }

        public FunctionHeader Function
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Function;
            yield return FunctionReference;
        }

        public override FunctionExecutionResult Invoke()
        {
            try
            {
                var result = InvokeFunction(FunctionReference, Function);

                if (result.Exception != null)
                {
                    var errorChange = new AddExceptionLine("Function execution failed with error", result.Exception, Function.RowReference);

                    return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, new[] { errorChange });
                }


                if (Equals(false, result.Result))
                {
                    return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Fail, new[] { new AddRowCssClass(Function.RowReference, HtmlParser.FailCssClass) });
                }

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Success, new[] { new AddRowCssClass(Function.RowReference, HtmlParser.PassCssClass) });
            }
            catch (Exception ex)
            {
                //                TestExecutionLog.Trace("Unable to execute function {0} because of error {1}", this, ex);

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, new[] { new AddExceptionLine(ex, Function.RowReference) });
            }
        }

        protected override string GetString()
        {
            return GetType().Name + ": " + Function;
        }
    }
}
