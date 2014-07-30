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
    [Serializable]
    public sealed class TypeReference : GeneralReferenceObject, IHelpIdentity, IDataCreation<TypeData, Type>
    {
        private static readonly object syncRoot = new object();

        private static readonly Dictionary<Type, TypeReference> references = new Dictionary<Type, TypeReference>();

        private const string typeIdentityFormat = "T:{0}";

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
                    ReferenceCache.Save(result, type);
                }
            }

            return result;
        }

        private TypeReference(Type targetType)
        {
            TargetType = targetType;

            HelpIdentity = string.Format(typeIdentityFormat, targetType.FullName);
            StrongIdentity = targetType.FullName;
        }

        public string StrongIdentity
        {
            get;
            private set;
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

        public PropertyReference[] GetProperties
        {
            get
            {
                return TargetType.GetProperties().Select(PropertyReference.GetPropertyReference).ToArray();
            }
        }

        public string HelpIdentity
        {
            get;
            private set;
        }

        public TypeData Create(Type targetItem)
        {
            return new TypeData(targetItem);
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
                return PropertyReference.GetPropertyReference(result);
            }

            return null;
        }
    }
}
