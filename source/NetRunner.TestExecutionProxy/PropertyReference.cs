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
    public sealed class PropertyReference : GeneralReferenceObject, IHelpIdentity
    {
        private const string propertyFormat = "P:{0}.{1}";

        private readonly PropertyInfo property;

        internal PropertyReference(PropertyInfo property, TypeReference typeReference)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            Owner = typeReference;
            this.property = property;
            ArgumentPrepareMode = ReflectionHelpers.FindAttribute(property, ArgumentPrepareAttribute.Default).Mode;
            TrimInputCharacters = ReflectionHelpers.FindAttribute(property, StringTrimAttribute.Default).TrimInputString;
            HelpIdentity = string.Format(propertyFormat, Owner.FullName, property.Name);

        }

        public TypeReference Owner
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return property.Name;
            }
        }

        public bool HasGet
        {
            get
            {
                return property.GetMethod != null;
            }
        }

        [NotNull]
        public TypeReference PropertyType
        {
            get
            {
                return TypeReference.GetType(property.PropertyType);
            }
        }

        public ArgumentPrepareAttribute.ArgumentPrepareMode ArgumentPrepareMode
        {
            get;
            private set;
        }

        public bool TrimInputCharacters
        {
            get;
            private set;
        }

        public string HelpIdentity
        {
            get;
            private set;
        }

        public ExecutionResult SetValue(GeneralIsolatedReference targetObject, GeneralIsolatedReference value)
        {
            try
            {
                property.SetValue(targetObject.Value, value.Value);

                return new ExecutionResult(GeneralIsolatedReference.Empty, Enumerable.Empty<ParameterData>());
            }
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }

        public ExecutionResult GetValue(GeneralIsolatedReference targetObject)
        {
            try
            {
                var result = new GeneralIsolatedReference(property.GetValue(targetObject.Value), property.PropertyType);

                return new ExecutionResult(result, Enumerable.Empty<ParameterData>());
            }
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }
    }
}
