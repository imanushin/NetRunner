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
    public sealed class PropertyReference : GeneralReferenceObject
    {
        private readonly PropertyInfo property;

        internal PropertyReference(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.property = property;
            ArgumentPrepareMode = ReflectionHelpers.FindAttribute(property, ArgumentPrepareAttribute.Default).Mode;
            TrimInputCharacters = ReflectionHelpers.FindAttribute(property, StringTrimAttribute.Default).TrimInputString;
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
