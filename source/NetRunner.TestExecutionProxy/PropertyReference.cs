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

        private static readonly Dictionary<PropertyInfo, PropertyReference> properties = new Dictionary<PropertyInfo, PropertyReference>(); 

        internal static PropertyReference GetPropertyReference(PropertyInfo property)
        {
            lock (properties)
            {
                return properties.GetOrCreate(property, p => new PropertyReference(p));
            }
        }

        private PropertyReference(PropertyInfo property)
        {
            this.property = property;

            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            StrongIdentity = TypeReference.GetType(property.DeclaringType) + "." + property.Name;

            ReferenceCache.Save(this, new PropertyData(property));
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

                return new ExecutionResult(GeneralIsolatedReference.Empty, Enumerable.Empty<ParameterValue>());
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

                return new ExecutionResult(result, Enumerable.Empty<ParameterValue>());
            }
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }
    }
}
