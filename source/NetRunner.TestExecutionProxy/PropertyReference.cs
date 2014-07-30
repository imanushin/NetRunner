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
    public sealed class PropertyReference : GeneralReferenceObject, IReference<PropertyData>
    {
        private readonly PropertyInfo property;

        internal PropertyReference(PropertyInfo property, TypeReference typeReference)
        {
            this.property = property;

            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            StrongIdentity = typeReference.StrongIdentity + "." + property.Name;
        }

        public string StrongIdentity
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
