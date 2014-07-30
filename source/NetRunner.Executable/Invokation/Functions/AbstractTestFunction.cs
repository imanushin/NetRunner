using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Documentation;
using NetRunner.Executable.RawData;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Functions
{
    internal abstract class AbstractTestFunction : BaseReadOnlyObject
    {
        public abstract FunctionExecutionResult Invoke();

        protected InvokationResult InvokeFunction(
            TestFunctionReference functionReference,
            FunctionHeader originalFunction)
        {
            Validate.ArgumentIsNotNull(functionReference, "functionReference");
            Validate.ArgumentIsNotNull(originalFunction, "originalFunction");

            var keyword = originalFunction.Keyword;

            return keyword.InvokeFunction(() => InvokeFunction(functionReference, originalFunction.FunctionCells.First(), originalFunction.Arguments), functionReference);
        }

        protected InvokationResult InvokeFunction(
            TestFunctionReference functionReference,
            HtmlCell firstFunctionCell,
            ReadOnlyList<HtmlCell> inputArguments)
        {
            Validate.ArgumentIsNotNull(functionReference, "functionReference");
            Validate.ArgumentIsNotNull(inputArguments, "inputArguments");

            var expectedTypes = functionReference.Arguments;
            var actualTypes = new List<ParameterValue>();

            var conversionErrors = new List<AbstractTableChange>();
            var helpChanges = new List<AbstractTableChange>();

            for (int i = 0; i < expectedTypes.Count; i++)
            {
                var inputArgument = inputArguments[i];
                var parameterInfo = expectedTypes[i];

                helpChanges.Add(new AddCellParameterHelp(inputArgument, parameterInfo));

                if (parameterInfo.IsOut)
                {
                    continue;
                }

                var conversionErrorHeader = string.Format(
                    "Unable to convert value '{2}' of parameter '{0}' of function '{1}'",
                    parameterInfo.Name,
                    functionReference.DisplayName,
                    inputArgument.CleanedContent);

                var cellInfo = new CellParsingInfo(parameterInfo, inputArgument);
                var conversionResult = ParametersConverter.ConvertParameter(cellInfo, conversionErrorHeader);
                var parameterData = new ParameterValue(parameterInfo.Name, conversionResult.Result);

                actualTypes.Add(parameterData);

                conversionErrors.AddRange(conversionResult.Changes.Changes);
            }

            if (conversionErrors.Any())
            {
                return InvokationResult.CreateErrorResult(new TableChangeCollection(false, true, conversionErrors));
            }

            var result = functionReference.Invoke(actualTypes);

            if (result.HasError)
            {
                var errorData = new AddCellExpandableException(
                    firstFunctionCell,
                    result,
                    "Function '{0}' execution was failed with error: {1}",
                    functionReference.DisplayName,
                    result.ExceptionType);

                helpChanges.Add(errorData);

                return InvokationResult.CreateErrorResult(new TableChangeCollection(false, true, helpChanges));
            }

            return InvokationResult.CreateSuccessResult(result, helpChanges);
        }

        public abstract ReadOnlyList<TestFunctionReference> GetInnerFunctions();

        protected static void CheckOutParameter(TestFunctionReference functionToExecute, SequenceExecutionStatus changes, ParameterValue outParameter, HtmlCell targetCell)
        {
            Validate.ArgumentIsNotNull(functionToExecute, "functionToExecute");
            Validate.ArgumentIsNotNull(changes, "changes");
            Validate.ArgumentIsNotNull(outParameter, "outParameter");
            Validate.ArgumentIsNotNull(targetCell, "targetCell");

            var targetParameter = functionToExecute.Method.GetParameter(outParameter.Name);

            var cellInfo = new CellParsingInfo(targetParameter, targetCell);

            const string conversionErrorHeader = "Unable to convert value";

            var parsedValue = ParametersConverter.ConvertParameter(cellInfo, conversionErrorHeader);

            changes.MergeWith(parsedValue.Changes);

            if (!parsedValue.Changes.AllWasOk)
            {
                return;
            }
            
            if (changes.CompareAndMerge(parsedValue.Result, outParameter.Value, targetCell))
            {
                changes.Changes.Add(new CssClassCellChange(targetCell, HtmlParser.PassCssClass));
            }
            else
            {
                changes.Changes.Add(new ShowActualValueCellChange(targetCell, outParameter.Value.ToString()));
                changes.AllIsOk = false;
            }
        }

        protected FunctionExecutionResult FormatResult(SequenceExecutionStatus changes)
        {
            var resultType = changes.WereExceptions
                ? FunctionExecutionResult.FunctionRunResult.Exception
                : (changes.AllIsOk
                    ? FunctionExecutionResult.FunctionRunResult.Success
                    : FunctionExecutionResult.FunctionRunResult.Fail);

            var innerChanges = changes.Changes;

            return new FunctionExecutionResult(resultType, innerChanges);
        }

        protected void MarkRootFunction(SequenceExecutionStatus changes, HtmlRowReference rowReference)
        {
            var innerChanges = changes.Changes;

            if (changes.WereExceptions)
            {
                innerChanges.Add(new AddRowCssClass(rowReference, HtmlParser.ErrorCssClass));
            }
            else if (changes.AllIsOk)
            {
                innerChanges.Add(new AddRowCssClass(rowReference, HtmlParser.PassCssClass));
            }
            else
            {
                innerChanges.Add(new AddRowCssClass(rowReference, HtmlParser.FailCssClass));
            }
        }
    }
}
