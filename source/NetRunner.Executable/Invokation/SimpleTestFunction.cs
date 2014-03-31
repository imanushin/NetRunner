using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation
{
    internal sealed class SimpleTestFunction : AbstractTestFunction
    {
        public SimpleTestFunction(FunctionHeader header)
        {
            Validate.ArgumentIsNotNull(header, "header");

            Function = header;
        }

        public FunctionHeader Function
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Function;
        }

        public override FunctionExecutionResult Invoke(ReflectionLoader loader)
        {
            try
            {
                var functionsAvailable = loader.FindFunctions(Function.FunctionName, Function.Arguments.Count);

                if (!functionsAvailable.Any())
                    return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, string.Format("Unable to find function {0}. See above listing of all functions available.", this));

                var firstFoundFunction = functionsAvailable.First();

                var expectedTypes = firstFoundFunction.ArgumentTypes;
                var actualTypes = new object[expectedTypes.Count];

                for (int i = 0; i < expectedTypes.Count; i++)
                {
                    actualTypes[i] = Convert.ChangeType(Function.Arguments[i], expectedTypes[i]);
                }

                var result = firstFoundFunction.Invoke(actualTypes);

                if (Equals(false, result))
                    return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Fail, string.Empty);

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Success, string.Empty);
            }
            catch (Exception ex)
            {
                TestExecutionLog.Trace("Unable to execute function {0} because of error {1}", this, ex);

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, ex.ToString());
            }
        }

        protected override string GetString()
        {
            return GetType().Name + ": " + Function;
        }
    }
}
