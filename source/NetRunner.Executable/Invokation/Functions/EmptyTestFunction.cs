using System.Collections.Generic;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class EmptyTestFunction : AbstractTestFunction
    {
        public static EmptyTestFunction Instance = new EmptyTestFunction();
        private readonly FunctionExecutionResult functionExecutionResult = new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Ignore, ReadOnlyList<AbstractTableChange>.Empty);

        private EmptyTestFunction()
        {
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            return new object[0];
        }

        public override FunctionExecutionResult Invoke(ReflectionLoader loader)
        {
            return functionExecutionResult;
        }
    }
}
