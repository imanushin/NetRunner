using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public sealed class PropertyReference : MarshalByRefObject
    {
        private readonly PropertyInfo property;

        internal PropertyReference(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.property = property;
        }

        public string Name
        {
            get
            {
                return property.Name;
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

        public ExecutionResult GetValue(GeneralIsolatedReference targetObject)
        {
            try
            {
                var result =  new GeneralIsolatedReference(property.GetValue(targetObject.Value));

                return new ExecutionResult(result, Enumerable.Empty<ParameterData>());
            }
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }
    }
}
