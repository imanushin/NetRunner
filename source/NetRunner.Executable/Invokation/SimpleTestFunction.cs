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

        public override FunctionRunResult Invoke(ReflectionLoader loader)
        {
            try
            {
                var functionsAvailable = loader.FindFunctions(FunctionName, Arguments.Count);

                Validate.CollectionHasElements(functionsAvailable, "Unable to find function '{0}'", this);

                var firstFoundFunction = functionsAvailable.First();

                var expectedTypes = firstFoundFunction.ArgumentTypes;
                var actualTypes = new object[expectedTypes.Count];

                for (int i = 0; i < expectedTypes.Count; i++)
                {
                    actualTypes[i] = Convert.ChangeType(Arguments[i], expectedTypes[i]);
                }

                var result = firstFoundFunction.Invoke(actualTypes);

                if(Equals(false, result))
                    return FunctionRunResult.Fail;

                return FunctionRunResult.Success;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to executio function {0} because of error {1}", this, ex);

                return FunctionRunResult.Exception;
            }
        }
    }
}
