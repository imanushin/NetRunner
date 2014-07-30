using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    [Serializable]
    public sealed class TypeReference : IDataCreation<TypeData, Type>
    {
        private static readonly object syncRoot = new object();

        private static readonly Dictionary<Type, TypeReference> references = new Dictionary<Type, TypeReference>();
        private static readonly Dictionary<TypeReference, Type> types = new Dictionary<TypeReference, Type>();

        public static TypeReference GetType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            lock (syncRoot)
            {
                return references.GetOrCreate(type, t =>
                {
                    var r = new TypeReference(t);
                    ReferenceCache.Save(r, type);
                    types[r] = t;
                    return r;
                });
            }
        }

        private TypeReference(Type targetType)
        {
            StrongIdentity = targetType.FullName;
        }

        public string StrongIdentity
        {
            get;
            private set;
        }

        public Type TargetType
        {
            get
            {
                lock (syncRoot)
                {
                    return types[this];
                }
            }
        }

        public TypeData Create(Type targetItem)
        {
            return new TypeData(targetItem);
        }

        public override string ToString()
        {
            return StrongIdentity;
        }

        internal bool IsAssignableFrom(TypeReference otherType)
        {
            var myType = TargetType;
            var secondType = otherType.TargetType;

            return myType.IsAssignableFrom(secondType);
        }

        public override bool Equals(object obj)
        {
            var other = obj as TypeReference;

            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return string.Equals(StrongIdentity, other.StrongIdentity);
        }

        public override int GetHashCode()
        {
            return StrongIdentity.GetHashCode();
        }
    }
}
