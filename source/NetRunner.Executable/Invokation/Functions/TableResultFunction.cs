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

        protected override FunctionExecutionResult ProcessResult(GeneralIsolatedReference mainFunctionResult)
        {
            var tableResult = mainFunctionResult.AsTableResultReference();

            var changes = new SequenceExecutionStatus();

            var functionExecutionResult = CheckTableFunctionResult(mainFunctionResult, tableResult, changes);

            if (functionExecutionResult != null)
                return functionExecutionResult;
            
            var functionToExecute = ReflectionLoader.FindFunction(CleanedColumnNames, tableResult);

            if (functionToExecute == null)
            {
                return FormatFunctionNotFoundMesage(changes);
            }

            var notificationResult = tableResult.ExecuteBeforeAllFunctionsCallMethod(functionToExecute.Method);

            AddExceptionLineIfNeeded(notificationResult, changes);

            foreach (var row in Rows)
            {
                var rowResult = InvokeFunction(
                    functionToExecute,
                    row.Cells.First(),
                    row.Cells);

                AnalyseResult(rowResult, changes, row, functionToExecute);
            }

            notificationResult = tableResult.ExecuteAfterAllFunctionsCallMethod(functionToExecute.Method);
            
            AddExceptionLineIfNeeded(notificationResult, changes);

            return FormatResult(changes);
        }

        private void AnalyseResult(InvokationResult rowResult, SequenceExecutionStatus changes, HtmlRow row, TestFunctionReference functionToExecute)
        {
            changes.MergeWith(rowResult.Changes);

            if (rowResult.Changes.WereExceptions)
            {
                changes.Changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.ErrorCssClass));
            }

            if (ReflectionLoader.FalseResult.Equals(rowResult.Result))
            {
                changes.Changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.FailCssClass));

                changes.AllIsOk = false;
            }
            else if (ReflectionLoader.TrueResult.Equals(rowResult.Result))
            {
                changes.Changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.PassCssClass));
            }

            CompareOutParameters(rowResult, row, functionToExecute, changes);
        }

        private void CompareOutParameters(InvokationResult rowResult, HtmlRow row, TestFunctionReference functionToExecute, SequenceExecutionStatus changes)
        {
            foreach (var outParameter in rowResult.OutParametersResult)
            {
                var outParameterIndex = CleanedColumnNames.IndexOf(outParameter.Name, StringComparer.OrdinalIgnoreCase);

                if (!outParameterIndex.HasValue)
                {
                    continue;
                }

                var targetCell = row.Cells[outParameterIndex.Value];

                CheckOutParameter(functionToExecute, changes, outParameter, targetCell);
            }
        }

        private FunctionExecutionResult FormatFunctionNotFoundMesage(SequenceExecutionStatus changes)
        {
            changes.Changes.Add(new ExecutionFailedMessage(
                ColumnsRow.RowReference,
                string.Format("Unable to find function with these parameters: {0}", CleanedColumnNames),
                "Unable to find function"));

            changes.Changes.Add(new AddRowCssClass(ColumnsRow.RowReference, HtmlParser.FailCssClass));

            return FormatResult(changes);
        }

        private void AddExceptionLineIfNeeded(ExecutionResult notificationException, SequenceExecutionStatus changes)
        {
            changes.MergeWith(notificationException);

            if (!notificationException.HasError)
            {
                return;
            }

            changes.Changes.Add(new ExecutionFailedMessage(
                ColumnsRow.RowReference,
                string.Format("Exception during handler invokation: {0}", notificationException.ExceptionType),
                "Argument handler executed with error: {0}",
                notificationException.ExceptionToString));

            changes.Changes.Add(new AddRowCssClass(ColumnsRow.RowReference, HtmlParser.ErrorCssClass));
        }

        [CanBeNull]
        private FunctionExecutionResult CheckTableFunctionResult(GeneralIsolatedReference mainFunctionResult, TableResultReference tableResult, SequenceExecutionStatus changes)
        {
            if (tableResult.IsNull)
            {
                if (mainFunctionResult.IsNull)
                {
                    changes.Changes.Add(new ExecutionFailedMessage(
                        Function.RowReference,
                        string.Format("Unable to check table: function '{0}' return null", Function.FunctionName),
                        "Unable to build table"));
                }
                else
                {
                    changes.Changes.Add(new ExecutionFailedMessage(
                        Function.RowReference,
                        string.Format("Unable to check table: function '{0}' return object '{1}' instead of '{2}'", Function.FunctionName, mainFunctionResult.GetType(), typeof(BaseTableArgument)),
                        "Unable to build table"));
                }

                changes.Changes.Add(new AddRowCssClass(Function.RowReference, HtmlParser.FailCssClass));

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Fail, changes.Changes);
            }

            return null;
        }
    }
}
