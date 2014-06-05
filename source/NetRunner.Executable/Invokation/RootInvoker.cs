using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation
{
    internal static class RootInvoker
    {
        public static string InvokeTable(ParsedTable table, TestCounts currentStatistic)
        {
            var changes = new List<AbstractTableChange>();

            FunctionExecutionResult result;
            try
            {
                Trace.TraceInformation("Execute function {0}", table.TestFunction);

                result = table.TestFunction.Invoke();

                Trace.TraceInformation("Execution result: {0}", result);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to execute function because of error: {0}", ex);

                var firstRow = table.Table.Rows.FirstOrDefault();

                if (firstRow != null)
                {
                    changes.Add(new AddExceptionLine(string.Format("Internal execution error: {0}", ex.GetType().Name), ex, firstRow.RowReference));
                }

                result = new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, ReadOnlyList<AbstractTableChange>.Empty);
            }

            return FormatResult(table.Table, result, currentStatistic, changes);
        }

        private static string FormatResult(HtmlTable table, FunctionExecutionResult result, TestCounts currentStatistic, IReadOnlyCollection<AbstractTableChange> tableParseInformation)
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

            var tableChanges = result.TableChanges.Concat(tableParseInformation).ToList();

            var traceData = TestExecutionLog.ExtractLogged();

            if (!string.IsNullOrWhiteSpace(traceData))
            {
                var lastRow = table.Rows.LastOrDefault();

                Validate.IsNotNull(lastRow, "Internal error: target table does not contain any rows. Execution should be skipped.");

                tableChanges.Add(new AddTraceLine(traceData, lastRow.RowReference));
            }

            var resultTable = table.GetClonedNode();

            foreach (var tableChange in tableChanges)
            {
                try
                {
                    tableChange.PatchHtmlTable(resultTable);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to patch table {0} with changes {1} because of error {2}", HttpUtility.HtmlEncode(resultTable.OuterHtml), tableChange, ex);
                }
            }

            return resultTable.OuterHtml;
        }
    }
}
