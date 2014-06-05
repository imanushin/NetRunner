using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public TypeReference PropertyType
        {
            get
            {
                return TypeReference.GetType(property.PropertyType);
            }
        }

        public GeneralIsolatedReference GetValue(GeneralIsolatedReference targetObject)
        {
            return new GeneralIsolatedReference(property.GetValue(targetObject.Value));
        }
    }
}
