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

            return keyword.InvokeFunction(() => InvokeFunction(functionReference, originalFunction.Arguments), functionReference);
        }

        protected InvokationResult InvokeFunction(
            TestFunctionReference functionReference,
            ReadOnlyList<string> inputArguments)
        {
            Validate.ArgumentIsNotNull(functionReference, "functionReference");
            Validate.ArgumentIsNotNull(inputArguments, "inputArguments");

            var expectedTypes = functionReference.ArgumentTypes;
            var actualTypes = new object[expectedTypes.Count];

            for (int i = 0; i < expectedTypes.Count; i++)
            {
                actualTypes[i] = ParametersConverter.ConvertParameter(inputArguments[i], expectedTypes[i].ParameterType);
            }

            try
            {
                var result = functionReference.Invoke(actualTypes);

                return new InvokationResult(result, null);
            }
            catch (TargetInvocationException ex)
            {
                return new InvokationResult(null, ex.InnerException);
            }
        }
    }
}
