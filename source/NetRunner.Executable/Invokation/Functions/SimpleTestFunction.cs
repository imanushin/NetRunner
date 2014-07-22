using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Documentation;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class SimpleTestFunction : AbstractTestFunction
    {
        private readonly HtmlRow targetRow;
        private readonly TestFunctionReference functionReference;
        private readonly FunctionHeader function;

        public SimpleTestFunction([NotNull] FunctionHeader header, [NotNull] TestFunctionReference functionToExecute, [NotNull] HtmlRow targetRow)
        {
            Validate.ArgumentIsNotNull(targetRow, "targetRow");
            Validate.ArgumentIsNotNull(header, "header");
            Validate.ArgumentIsNotNull(functionToExecute, "functionToExecute");

            function = header;
            functionReference = functionToExecute;
            this.targetRow = targetRow;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return function;
            yield return functionReference;
            yield return targetRow;
        }

        public override FunctionExecutionResult Invoke()
        {
            try
            {
                var status = new SequenceExecutionStatus();

                var result = InvokeFunction(functionReference, function);

                AddFunctionHelp(status);

                status.MergeWith(result.Changes);

                if (result.Changes.WereExceptions)
                {
                    var rowCss = new AddRowCssClass(function.RowReference, HtmlParser.ErrorCssClass);

                    status.Changes.Add(rowCss);
                }

                if (result.Result.IsFalse)
                {
                    status.AllIsOk = false;
                    status.Changes.Add(new AddRowCssClass(function.RowReference, HtmlParser.FailCssClass));
                }

                foreach (var parameterData in result.OutParametersResult)
                {
                    CheckOutParameter(functionReference, status, parameterData, GetParameterCell(parameterData));
                }

                if (result.Result.IsTrue && status.AllIsOk)
                {
                    var trueResultMark = new AddRowCssClass(function.RowReference, HtmlParser.PassCssClass);

                    status.Changes.Add(trueResultMark);
                }

                return FormatResult(status);
            }
            catch (InternalException ex)
            {
                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, new[]
                {
                    new AddExceptionLine(ex.Message, ex.InnerException, function.RowReference)
                });
            }
            catch (Exception ex)
            {
                var errorChange = new AddCellExpandableException(targetRow.Cells.First(), ex, "Internal function execution error");
                var markCellAsError = new AddRowCssClass(targetRow.RowReference, HtmlParser.ErrorCssClass);
                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, new AbstractTableChange[]
                {
                    errorChange,
                    markCellAsError
                });
            }
        }

        private void AddFunctionHelp(SequenceExecutionStatus status)
        {
            var argumentsHelp = function.Arguments
                .Select((arg, index) => new
                {
                    Argument = arg,
                    Index = index
                })
                .Where(item => item.Index < functionReference.Arguments.Count)
                .Select(item => new AddCellParameterHelp(item.Argument, functionReference.Arguments[item.Index]));

            status.Changes.AddRange(argumentsHelp);

            status.Changes.Add(new AddFunctionHelp(function.FunctionCells, functionReference));
        }

        private HtmlCell GetParameterCell(ParameterData parameterData)
        {
            var parameterIndex = functionReference.Arguments.IndexOf(p => string.Equals(p.Name, parameterData.Name, StringComparison.Ordinal));

            Validate.IsNotNull(
                parameterIndex,
                "Internal error: unable to find parameter '{0}' of '{1}'. Parameters available: {2}. Please send this issue to {3}",
                parameterData.Name,
                function.FunctionName,
                functionReference.Arguments.JoinToStringLazy(", "),
                GlobalConstants.IssuesPath
            );

            return function.Arguments[parameterIndex.Value];
        }

        public override ReadOnlyList<TestFunctionReference> GetInnerFunctions()
        {
            return new[]
            {
                functionReference
            }.ToReadOnlyList();
        }

        protected override string GetString()
        {
            return GetType().Name + ": " + function;
        }
    }
}
