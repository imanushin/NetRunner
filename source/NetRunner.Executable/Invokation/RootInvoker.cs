using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation
{
    internal static class RootInvoker
    {
        public static string InvokeTable(HtmlTable table, TestCounts currentStatistic, ReflectionLoader loader)
        {
            FunctionExecutionResult result;
            try
            {
                var functionToInvoke = TableParser.ParseTable(table);

                if (functionToInvoke == null) //table does not contain function declaration
                {
                    result = new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Ignore, "First row should contain bold cells (use syntax like |'''test function name'''|). These cells will be unioned to the function name.");
                }
                else
                {
                    result = functionToInvoke.Invoke(loader);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to execute function because of error: {0}", ex);

                result = new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, "Internal execution error: " + ex.ToString());
            }

            return FormatResult(table, result, currentStatistic);
        }

        private static string FormatResult(HtmlTable table, FunctionExecutionResult result, TestCounts currentStatistic)
        {
            switch (result.ResultType)
            {
                case FunctionExecutionResult.FunctionRunResult.Fail:
                    currentStatistic.IncrementFailCount();
                    break;
                case FunctionExecutionResult.FunctionRunResult.Success:
                    currentStatistic.IncrementSuccessCount();
                    break;
                case FunctionExecutionResult.FunctionRunResult.Ignore:
                    currentStatistic.IncrementIgnoreCount();
                    break;
                case FunctionExecutionResult.FunctionRunResult.Exception:
                    currentStatistic.IncrementExceptionCount();
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown result: {0}", result));
            }

            return table.GetClonedNode().OuterHtml;
        }
    }
}
