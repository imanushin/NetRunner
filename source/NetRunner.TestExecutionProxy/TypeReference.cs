using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public sealed class TypeReference : MarshalByRefObject
    {
        internal TypeReference(Type targetType)
        {
            if(targetType == null)
                throw new ArgumentNullException("targetType");

            TargetType = targetType;
        }

        internal Type TargetType
        {
            get;
            private set;
        }

        public string TypeName
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return TargetType.Name;
            }
        }

        public string FullName
        {
            get
            {
                return TargetType.FullName;
            }
        }

        public PropertyReference[] GetProperties
        {
            get
            {
                return TargetType.GetProperties().Select(p => new PropertyReference(p)).ToArray();
            }
        }

        public override string ToString()
        {
            return TargetType.ToString();
        }

        public bool IsAssignableFrom(TypeReference otherType)
        {
            return TargetType.IsAssignableFrom(otherType.TargetType);
        }

        public override bool Equals(object obj)
        {
            var other = obj as TypeReference;

            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return TargetType == other.TargetType;
        }

        public override int GetHashCode()
        {
            return TargetType.GetHashCode();
        }

        public PropertyReference GetProperty(string propertyName)
        {
            var result =
                TargetType
                    .GetProperties()
                    .FirstOrDefault(p => String.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

            if (result != null)
            {
                return new PropertyReference(result);
            }

            return null;
        }
    }
}
