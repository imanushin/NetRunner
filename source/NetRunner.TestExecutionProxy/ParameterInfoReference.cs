using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public sealed class ParameterInfoReference
    {
        public ParameterInfoReference(ParameterInfo parameter)
        {
            Parameter = parameter;
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

        public TypeReference ParameterType
        {
            get
            {
                return new TypeReference(Parameter.ParameterType);
            }
        }
    }
}
