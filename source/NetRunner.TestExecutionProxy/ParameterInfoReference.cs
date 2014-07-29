using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    public sealed class ParameterInfoReference : GeneralReferenceObject, IHelpIdentity
    {
        public const string ParameterFormat = "Par:{0}.{1}";

        internal ParameterInfoReference(ParameterInfo parameter, FunctionMetaData method)
        {
            Parameter = parameter;
            PrepareMode = ReflectionHelpers.FindAttribute(parameter, ArgumentPrepareAttribute.Default).Mode;
            TrimInputCharacters = ReflectionHelpers.FindAttribute(parameter, StringTrimAttribute.Default).TrimInputString;
            Owner = method;

            HelpIdentity = string.Format(ParameterFormat, method.HelpIdentity, parameter.Name);
        }

        public bool TrimInputCharacters
        {
            get;
            private set;
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

        public bool IsOut
        {
            get
            {
                return Parameter.IsOut;
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

        public FunctionMetaData Owner
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
