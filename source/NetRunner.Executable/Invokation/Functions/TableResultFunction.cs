using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class TableResultFunction : BaseComplexArgumentedFunction
    {
        public TableResultFunction(
            HtmlRow columnsRow,
            IEnumerable<HtmlRow> rows,
            FunctionHeader function,
            TestFunctionReference functionToExecute) :
            base(columnsRow, rows, function, functionToExecute)
        {
        }

        protected override FunctionExecutionResult ProcessResult(IsolatedReference<object> mainFunctionResult)
        {
            var tableResult = mainFunctionResult.AsTableResultReference();

            var changes = new List<AbstractTableChange>();

            var functionExecutionResult = CheckTableFunctionResult(mainFunctionResult, tableResult, changes);

            if (functionExecutionResult != null)
                return functionExecutionResult;

            bool allIsOk = true;
            bool exceptionsOccurred = false;

            var functionToExecute = ReflectionLoader.FindFunction(CleanedColumnNames, tableResult);

            if (functionToExecute == null)
            {
                changes.Add(new ExecutionFailedMessage(
                    ColumnsRow.RowReference,
                    string.Format("Unable to find function with these parameters: {0}", CleanedColumnNames),
                    "Unable to find function"));

                changes.Add(new AddRowCssClass(ColumnsRow.RowReference, HtmlParser.FailCssClass));

                return FormatResult(false, false, changes);
            }

            var notificationResult = tableResult.ExecuteBeforeFunctionCallMethod(functionToExecute.DisplayName);

            AddExceptionLineIfNeeded(notificationResult, changes);

            foreach (var row in Rows)
            {
                var rowResult = InvokeFunction(
                    functionToExecute,
                    row.Cells.First(),
                    row.Cells);

                changes.AddRange(rowResult.Changes.Changes);

                if (rowResult.Changes.WereExceptions)
                {
                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.ErrorCssClass));

                    exceptionsOccurred = true;
                }

                if (ReflectionLoader.FalseResult.Equals(rowResult.Result))
                {
                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.FailCssClass));

                    allIsOk = false;
                }
                else if (ReflectionLoader.TrueResult.Equals(rowResult.Result))
                {
                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.PassCssClass));
                }
            }

            notificationResult = tableResult.ExecuteAfterFunctionCallMethod(functionToExecute.DisplayName);

            AddExceptionLineIfNeeded(notificationResult, changes);

            return FormatResult(exceptionsOccurred, allIsOk, changes);
        }

        private void AddExceptionLineIfNeeded(ExecutionResult notificationException, List<AbstractTableChange> changes)
        {
            if (!notificationException.HasError)
            {
                return;
            }

            changes.Add(new ExecutionFailedMessage(
                ColumnsRow.RowReference,
                string.Format("Exception during handler invokation"),
                "Argument handler executed with error: {0}",
                notificationException));

            changes.Add(new AddRowCssClass(ColumnsRow.RowReference, HtmlParser.FailCssClass));
        }

        [CanBeNull]
        private FunctionExecutionResult CheckTableFunctionResult(GeneralIsolatedReference mainFunctionResult, TableResultReference tableResult, List<AbstractTableChange> changes)
        {
            if (tableResult.IsNull)
            {
                if (mainFunctionResult.IsNull)
                {
                    changes.Add(new ExecutionFailedMessage(
                        Function.RowReference,
                        string.Format("Unable to check table: function '{0}' return null", Function.FunctionName),
                        "Unable to build table"));
                }
                else
                {
                    changes.Add(new ExecutionFailedMessage(
                        Function.RowReference,
                        string.Format("Unable to check table: function '{0}' return object '{1}' instead of '{2}'", Function.FunctionName, mainFunctionResult.GetType(), typeof(BaseTableArgument)),
                        "Unable to build table"));
                }

                changes.Add(new AddRowCssClass(Function.RowReference, HtmlParser.FailCssClass));

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Fail, changes);
            }

            return null;
        }
    }
}
