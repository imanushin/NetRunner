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
                var result = InvokeFunction(functionReference, function);

                if (result.Changes.WereExceptions)
                {
                    var rowCss = new AddRowCssClass(function.RowReference, HtmlParser.ErrorCssClass);

                    return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, result.Changes.Changes.Concat(rowCss));
                }

                if (Equals(false, result.Result))
                {
                    var falseResultMark = new AddRowCssClass(function.RowReference, HtmlParser.FailCssClass);

                    return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Fail, result.Changes.Changes.Concat(falseResultMark));
                }

                var trueResultMark = new AddRowCssClass(function.RowReference, HtmlParser.PassCssClass);

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Success, result.Changes.Changes.Concat(trueResultMark));
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
                var errorChange = new AddCellExpandableException(targetRow.Cells.First(), ex, "Internal error function execution error");
                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, new[]
                {
                    errorChange
                });
            }
        }

        protected override string GetString()
        {
            return GetType().Name + ": " + function;
        }
    }
}
