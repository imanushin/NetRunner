using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.Invokation
{
    internal sealed class EmptyTestFunction : AbstractTestFunction
    {
        public static EmptyTestFunction Instance = new EmptyTestFunction();
        private readonly FunctionExecutionResult functionExecutionResult = new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Ignore, string.Empty);

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
