using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class TableResultFunction : BaseComplexArgumentedFunction
    {
        public static readonly Type BaseType = typeof(BaseTableArgument);

        public TableResultFunction(
            HtmlRow columnsRow,
            IEnumerable<HtmlRow> rows,
            FunctionHeader function,
            TestFunctionReference functionToExecute) :
            base(columnsRow, rows, function, functionToExecute)
        {
        }

        protected override FunctionExecutionResult ProcessResult(object mainFunctionResult)
        {
            var tableResult = mainFunctionResult as BaseTableArgument;

            var changes = new List<AbstractTableChange>();

            var functionExecutionResult = CheckTableFunctionResult(mainFunctionResult, tableResult, changes);

            if (functionExecutionResult != null)
                return functionExecutionResult;

            bool allIsOk = true;
            bool exceptionsOccurred = false;

            foreach (var row in Rows)
            {
                var functionToExecute = ReflectionLoader.Instance.FindFunction(CleanedColumnNames, tableResult);

                if (functionToExecute == null)
                {
                    changes.Add(new ExecutionFailedMessage(
                        row.RowReference,
                        string.Format("Unable to find function with these parameters: {0}", CleanedColumnNames),
                        "Unable to find function"));

                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.FailCssClass));

                    allIsOk = false;

                    continue;
                }

                var rowResult = InvokeFunction(
                    functionToExecute,
                    row.Cells.First(),
                    row.Cells);

                changes.AddRange(rowResult.TableChanges);

                if (rowResult.ExecutedWithErrors)
                {
                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.ErrorCssClass));

                    exceptionsOccurred = true;
                }

                if (Equals(false, rowResult.Result))
                {
                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.FailCssClass));

                    allIsOk = false;
                }
                else if (Equals(true, rowResult.Result))
                {
                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.PassCssClass));
                }
            }

            return FormatResult(exceptionsOccurred, allIsOk, changes);
        }

        [CanBeNull]
        private FunctionExecutionResult CheckTableFunctionResult(object mainFunctionResult, BaseTableArgument tableResult, List<AbstractTableChange> changes)
        {
            if (tableResult == null)
            {
                if (ReferenceEquals(null, mainFunctionResult))
                {
                    changes.Add(new ExecutionFailedMessage(
                        Function.RowReference,
                        string.Format("Unable to check table: function {0} return null", Function.FunctionName),
                        "Unable to build table"));
                }
                else
                {
                    changes.Add(new ExecutionFailedMessage(
                        Function.RowReference,
                        string.Format("Unable to check table: function {0} return object {1} instead of {2}", Function.FunctionName, mainFunctionResult.GetType(), typeof(BaseTableArgument)),
                        "Unable to build table"));
                }

                changes.Add(new AddRowCssClass(Function.RowReference, HtmlParser.FailCssClass));

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Fail, changes);
            }

            return null;
        }


    }
}
