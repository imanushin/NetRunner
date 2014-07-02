using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    public sealed class IsolatedParser : IsolatedReference<BaseParser>
    {
        private static readonly MethodInfo tryParseMethod = typeof(BaseParser).GetMethod("TryParse");

        internal IsolatedParser(BaseParser value)
            : base(value)
        {
        }

        public bool TryParse(string inputData, TypeReference expectedType, out ExecutionResult result)
        {
            try
            {
                MethodInfo generic = tryParseMethod.MakeGenericMethod(expectedType.TargetType);

                var args = new object[]
                    {
                        inputData, null
                    };

                var parsed = (bool)generic.Invoke(Value, args);

                result = new ExecutionResult(new IsolatedReference<object>(args[1]), Enumerable.Empty<ParameterData>());

                return parsed;
            }
            catch (Exception ex)
            {
                result = ExecutionResult.FromException(ex);

                Trace.TraceError("Unable to parse value '{0}' to type {1} using parser {2}: {3}", inputData, expectedType.FullName, Value.GetType().Name, result.ExceptionToString);
                
                return false;
            }
        }
    }
}
