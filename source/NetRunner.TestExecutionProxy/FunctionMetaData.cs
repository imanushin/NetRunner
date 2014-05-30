using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public sealed class FunctionMetaData : MarshalByRefObject
    {
        private readonly BaseTestContainer targetObject;

        public FunctionMetaData(MethodInfo method)
        {
            this.targetObject = targetObject;
            Method = method;
        }

        public IsolatedReference<BaseTestContainer> TargetObject
        {
            get
            {
                return new IsolatedReference<BaseTestContainer>(targetObject);
            }
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

        public ExecutionResult Invoke( IsolatedReference<object>[] parameters)
        {
            try
            {
                var result = Method.Invoke(targetObject, parameters.Select(p => p.Value).ToArray());

                return new ExecutionResult(new IsolatedReference<object>(result));
            }
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }
    }
}
