using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public sealed class TypeReference : MarshalByRefObject
    {
        public TypeReference(Type targetType)
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
    }
}
