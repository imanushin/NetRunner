using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    public sealed class FunctionMetaData : MarshalByRefObject
    {
        public FunctionMetaData(MethodInfo method)
        {
            Method = method;
        }

        internal MethodInfo Method
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return Method.Name;
            }
        }

        public TypeReference ReturnType
        {
            get
            {
                return new TypeReference(Method.ReturnType);
            }
        }

        public ParameterInfoReference[] GetParameters()
        {
            return Method.GetParameters().Select(p => new ParameterInfoReference(p)).ToArray();
        }

        public ExecutionResult Invoke(IsolatedReference<FunctionContainer> targetObject, IsolatedReference<object>[] parameters)
        {
            try
            {
                var result = Method.Invoke(targetObject.Value, parameters.Select(p => p.Value).ToArray());

                return new ExecutionResult(new IsolatedReference<object>(result));
            }
            catch (Exception ex)
            {
                return new ExecutionResult(ex.Message, ex.GetType().Name, ex.ToString());
            }
        }
    }
}
