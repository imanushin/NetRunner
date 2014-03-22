using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.Common
{
    public abstract class BaseReadOnlyObject : IEquatable<BaseReadOnlyObject>
    {
        private int? hashCode;
        private string cachedString;

        protected abstract IEnumerable<object> GetInnerObjects();

        protected virtual string GetString()
        {
            var result = new StringBuilder();

            result.Append("{Current type: ");
            result.Append(GetType().Name);
            result.Append("; Inner objects : ");

            foreach (object innerObject in GetInnerObjects())
            {
                result.Append(innerObject);
            }

            result.Append("}");

            return result.ToString();
        }

        public bool Equals(BaseReadOnlyObject other)
        {
            if (other == null)
                return false;

            if (other.GetType() != GetType())
                return false;

            if (GetHashCode() != other.GetHashCode())
                return false;

            if (ReferenceEquals(this, other))
                return true;

            var currentObjects = GetInnerObjects().ToArray();
            var otherObjects = other.GetInnerObjects().ToArray();

            if (currentObjects.Length != otherObjects.Length)
                return false;

            for (int i = 0; i < currentObjects.Length; i++)
            {
                if (currentObjects[i] != otherObjects[i])
                    return false;
            }

            return true;
        }

        public sealed override string ToString()
        {
            if (cachedString != null)
                return cachedString;

            cachedString = GetString();

            return cachedString;
        }

        public sealed override int GetHashCode()
        {
            if (hashCode.HasValue)
                return hashCode.Value;

            var hash = 0;

            foreach (object innerObject in GetInnerObjects())
            {
                hash ^= innerObject.GetHashCode();

                hash <<= 3;
            }

            hashCode = hash;

            return hash;
        }

        public sealed override bool Equals(object obj)
        {
            var other = obj as BaseReadOnlyObject;

            return Equals(other);
        }
    }
}
