﻿using System;
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
    public sealed class TypeReference : GeneralReferenceObject, IDataCreation<TypeData, Type>
    {
        private static readonly object syncRoot = new object();

        private static readonly Dictionary<Type, TypeReference> references = new Dictionary<Type, TypeReference>();

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
                    return r;
                });
            }
        }

        private TypeReference(Type targetType)
        {
            TargetType = targetType;

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
    }
}
