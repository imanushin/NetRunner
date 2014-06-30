using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    public sealed class ParameterInfoReference : MarshalByRefObject
    {
        internal ParameterInfoReference(ParameterInfo parameter)
        {
            Parameter = parameter;
            PrepareMode = ReflectionHelpers.FindAttribute(parameter, ArgumentPrepareAttribute.Default).Mode;
        }

        internal ParameterInfo Parameter
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return Parameter.Name;
            }
        }

        public ArgumentPrepareAttribute.ArgumentPrepareMode PrepareMode
        {
            get;
            private set;
        }

        public TypeReference ParameterType
        {
            get
            {
                return TypeReference.GetType(Parameter.ParameterType);
            }
        }
    }
}
