using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation.Functions
{
    internal abstract class AbstractTestFunction : BaseReadOnlyObject
    {
        public abstract FunctionExecutionResult Invoke(ReflectionLoader loader);

        protected object InvokeFunction(ReflectionLoader loader, TestFunctionReference functionReference, FunctionHeader userData)
        {
            var expectedTypes = functionReference.ArgumentTypes;
            var actualTypes = new object[expectedTypes.Count];

            for (int i = 0; i < expectedTypes.Count; i++)
            {
                actualTypes[i] = ParametersConverter.ConvertParameter(userData.Arguments[i], expectedTypes[i].ParameterType, loader);
            }

            return functionReference.Invoke(actualTypes);
        }
    }
}
