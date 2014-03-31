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
        public SimpleTestFunction(string functionName, IEnumerable<string> arguments)
        {
            Validate.ArgumentStringIsMeanful(functionName, "functionName");

            FunctionName = functionName.Replace(" ", string.Empty);
            Arguments = arguments.ToReadOnlyList();
        }

        public string FunctionName
        {
            get;
            private set;
        }

        public ReadOnlyList<string> Arguments
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return FunctionName;
            yield return Arguments;
        }

        public override FunctionExecutionResult Invoke(ReflectionLoader loader)
        {
            try
            {
                var functionsAvailable = loader.FindFunctions(FunctionName, Arguments.Count);

                if (!functionsAvailable.Any())
                    return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, string.Format("Unable to find function {0}. See above listing of all functions available.", this));
                
                var firstFoundFunction = functionsAvailable.First();

                var expectedTypes = firstFoundFunction.ArgumentTypes;
                var actualTypes = new object[expectedTypes.Count];

                for (int i = 0; i < expectedTypes.Count; i++)
                {
                    actualTypes[i] = Convert.ChangeType(Arguments[i], expectedTypes[i]);
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
            return string.Format("{0}: name - '{1}', arguments: {2}", GetType().Name, FunctionName, Arguments.JoinToStringLazy(";"));
        }
    }
}
