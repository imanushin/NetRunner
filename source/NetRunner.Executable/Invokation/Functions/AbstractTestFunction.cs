using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
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

            return keyword.InvokeFunction(() => InvokeFunction(functionReference, originalFunction.FirstFunctionCell, originalFunction.Arguments), functionReference);
        }

        protected InvokationResult InvokeFunction(
            TestFunctionReference functionReference,
            HtmlCell firstFunctionCell,
            ReadOnlyList<HtmlCell> inputArguments)
        {
            Validate.ArgumentIsNotNull(functionReference, "functionReference");
            Validate.ArgumentIsNotNull(inputArguments, "inputArguments");

            var expectedTypes = functionReference.ArgumentTypes;
            var actualTypes = new IsolatedReference<object>[expectedTypes.Count];

            var conversionErrors = new List<AbstractTableChange>();

            for (int i = 0; i < expectedTypes.Count; i++)
            {
                var inputArgument = inputArguments[i];

                var conversionErrorHeader = string.Format(
                    "Unable to convert value '{2}' of parameter '{0}' of function '{1}'",
                    functionReference.ArgumentTypes[i].Name,
                    functionReference.DisplayName,
                    inputArgument.CleanedContent);


                var conversionResult = ParametersConverter.ConvertParameter(inputArgument, expectedTypes[i].ParameterType, conversionErrorHeader);
                actualTypes[i] = conversionResult.Result;

                conversionErrors.AddRange(conversionResult.Changes.Changes);
            }

            if (conversionErrors.Any())
            {
                return new InvokationResult(null, new TableChangeCollection(false, true, conversionErrors));
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

                return new InvokationResult(null, new TableChangeCollection(false, true, errorData));
            }

            return new InvokationResult(result.Result);
        }

        public abstract ReadOnlyList<TestFunctionReference> GetInnerFunctions();
    }
}
