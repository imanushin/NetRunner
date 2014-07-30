using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    [Serializable]
    public sealed class ParameterInfoData : IHelpIdentity
    {
        public const string ParameterFormat = "Par:{0}.{1}";

        public ParameterInfoData(ParameterInfo parameter, MethodReference method)
        {
            Name = parameter.Name;
            IsOut = parameter.IsOut;
            PrepareMode = ReflectionHelpers.FindAttribute(parameter, ArgumentPrepareAttribute.Default).Mode;
            TrimInputCharacters = ReflectionHelpers.FindAttribute(parameter, StringTrimAttribute.Default).TrimInputString;
            ParameterType = TypeReference.GetType(parameter.ParameterType);
            
            Owner = method;
            HelpIdentity = string.Format(ParameterFormat, ReferenceCache.Get(Owner).HelpIdentity, parameter.Name);
        }

        public bool TrimInputCharacters
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public bool IsOut
        {
            get;
            private set;
        }

        public ArgumentPrepareAttribute.ArgumentPrepareMode PrepareMode
        {
            get;
            private set;
        }

        public TypeReference ParameterType
        {
            get;
            private set;
        }

        public MethodReference Owner
        {
            get;
            private set;
        }

        public string HelpIdentity
        {
            get;
            private set;
        }
    }
}
