using System;
using System.Reflection;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

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
            var actualTypes = new object[expectedTypes.Count];

            for (int i = 0; i < expectedTypes.Count; i++)
            {
                actualTypes[i] = ParametersConverter.ConvertParameter(inputArguments[i].CleanedContent, expectedTypes[i].ParameterType);
            }

            try
            {
                var result = functionReference.Invoke(actualTypes);

                return new InvokationResult(result);
            }
            catch (TargetInvocationException ex)
            {
                var targetException = ex.InnerException;

                Validate.IsNotNull(targetException, "Internal error: {0} should have inner exception", ex.GetType());

                var errorData = new AddCellExpandableException(firstFunctionCell, targetException, "Function '{0}' execution was failed with error: {1}", functionReference.DisplayName, targetException.GetType().Name);

                return new InvokationResult(null, true, new[] { errorData });
            }
        }
    }
}
