using System;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal abstract class AbstractTestFunction : BaseReadOnlyObject
    {
        public abstract FunctionExecutionResult Invoke(ReflectionLoader loader);

        protected object InvokeFunction(
            ReflectionLoader loader,
            TestFunctionReference functionReference,
            ReadOnlyList<string> inputArguments,
            out Exception executionException)
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

            executionException = null;

            try
            {
                return functionReference.Invoke(actualTypes);
            }
            catch (Exception ex)
            {
                executionException = ex;

                return null;
            }
        }
    }
}
