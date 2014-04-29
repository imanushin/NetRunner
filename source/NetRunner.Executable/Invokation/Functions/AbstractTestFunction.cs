using System;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal abstract class AbstractTestFunction : BaseReadOnlyObject
    {
        public abstract FunctionExecutionResult Invoke(ReflectionLoader loader);

        protected InvokationResult InvokeFunction(
            ReflectionLoader loader,
            TestFunctionReference functionReference,
            FunctionHeader originalFunction)
        {
            var keyword = originalFunction.Keyword;

            return keyword.InvokeFunction(() => InvokeFunction(loader, functionReference, originalFunction.Arguments));
        }

        protected InvokationResult InvokeFunction(
            ReflectionLoader loader,
            TestFunctionReference functionReference,
            ReadOnlyList<string> inputArguments)
        {
            Validate.ArgumentIsNotNull(loader, "loader");
            Validate.ArgumentIsNotNull(functionReference, "functionReference");
            Validate.ArgumentIsNotNull(inputArguments, "inputArguments");

            var expectedTypes = functionReference.ArgumentTypes;
            var actualTypes = new object[expectedTypes.Count];

            for (int i = 0; i < expectedTypes.Count; i++)
            {
                actualTypes[i] = ParametersConverter.ConvertParameter(inputArguments[i], expectedTypes[i].ParameterType, loader);
            }
            
            try
            {
                var result = functionReference.Invoke(actualTypes);

                return new InvokationResult(result, null);
            }
            catch (Exception ex)
            {
                return new InvokationResult(null, ex);
            }
        }
    }
}
