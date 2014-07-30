using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public sealed class ParameterValue : GeneralReferenceObject
    {
        public ParameterValue(string name, GeneralIsolatedReference value)
        {
            Name = name;
            Value = value;
        }

        [NotNull]
        public string Name
        {
            get;
            private set;
        }

        [NotNull]
        public GeneralIsolatedReference Value
        {
            get;
            private set;
        }
    }
}
