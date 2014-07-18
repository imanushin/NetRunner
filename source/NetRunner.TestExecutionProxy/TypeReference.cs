using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public sealed class TypeReference : GeneralReferenceObject
    {
        private static readonly object syncRoot = new object();

        private static readonly Dictionary<Type, TypeReference> references = new Dictionary<Type, TypeReference>();

        public static TypeReference GetType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            TypeReference result;

            lock (syncRoot)
            {
                if (!references.TryGetValue(type, out result))
                {
                    result = new TypeReference(type);
                    references[type] = result;
                }
            }

            return result;
        }

        private TypeReference(Type targetType)
        {
            TargetType = targetType;

            Identity = targetType.FullName;
        }

        internal Type TargetType
        {
            get;
            private set;
        }

        public TypeReference GetElementType()
        {
            return new TypeReference(TargetType.GetElementType());
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
                return TargetType.GetProperties().Select(p => new PropertyReference(p, this)).ToArray();
            }
        }

        public string Identity
        {
            get;
            private set;
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

        [CanBeNull]
        public PropertyReference GetProperty(string propertyName)
        {
            var result =
                TargetType
                    .GetProperties()
                    .FirstOrDefault(p => String.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

            if (result != null)
            {
                return new PropertyReference(result, this);
            }

            return null;
        }
    }
}
