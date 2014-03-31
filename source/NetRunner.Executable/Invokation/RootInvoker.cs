using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
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
                var functionToInvoke = TableParser.ParseTable(table, loader);

                Trace.TraceInformation("Execute function {0}", functionToInvoke);

                result = functionToInvoke.Invoke(loader);

                Trace.TraceInformation("Execution result: {0}", result);
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

            var additionalText = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(result.AdditionalHtmlText))
            {
                additionalText.AppendLine(result.AdditionalHtmlText);
                additionalText.AppendLine("<br/>");
            }

            var traceData = TestExecutionLog.ExtractLogged();

            if (!string.IsNullOrWhiteSpace(traceData))
            {
                additionalText.AppendLine(traceData);
            }

            var resultTable = table.GetClonedNode();

            var additionalLines = additionalText.ToString();

            if (!string.IsNullOrWhiteSpace(additionalLines))
            {
                var node = resultTable.OwnerDocument.CreateElement("tr");

                var cellContainer = resultTable.OwnerDocument.CreateElement("td");

                var expandableDiv = resultTable.OwnerDocument.CreateElement("div");

                expandableDiv.InnerHtml = additionalLines;

                cellContainer.AppendChild(expandableDiv);

                node.AppendChild(cellContainer);

                resultTable.AppendChild(node);
            }

            return resultTable.OuterHtml;
        }
    }
}
