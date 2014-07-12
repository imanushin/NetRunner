using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public class GeneralIsolatedReference : MarshalByRefObject
    {
        public static readonly GeneralIsolatedReference Empty = new GeneralIsolatedReference(null, typeof(object));

        private readonly TypeReference objectType;

        internal GeneralIsolatedReference(object value, Type objectType)
            :this(value, TypeReference.GetType(objectType))
        {
        }

        internal GeneralIsolatedReference(object value, TypeReference objectType)
        {
            Value = value;
            this.objectType = objectType;

            if (!ReferenceEquals(null, value))
            {
                this.objectType = TypeReference.GetType(value.GetType());
            }
        }

        [CanBeNull]
        internal object Value
        {
            get;
            private set;
        }

        public bool IsNull
        {
            get
            {
                return ReferenceEquals(null, Value);
            }
        }

        public TableResultReference AsTableResultReference()
        {
            return new TableResultReference(Value as BaseTableArgument);
        }

        public IsolatedReference<IEnumerable> AsIEnumerable()
        {
            return new IsolatedReference<IEnumerable>(Value as IEnumerable);
        }

        public IsolatedReference<FunctionContainer> CastToFunctionContainer()
        {
            return new IsolatedReference<FunctionContainer>((FunctionContainer)Value);
        }

        public override int GetHashCode()
        {
            return ReferenceEquals(null, Value) ? 0 : Value.GetHashCode();
        }

        public FunctionMetaData[] GetMethods()
        {
            return objectType.TargetType.GetMethods().Select(m => new FunctionMetaData(m, this)).ToArray();
        }

        public new TypeReference GetType()
        {
            return objectType;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GeneralIsolatedReference;

            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(Value, other.Value))
            {
                return true;
            }

            if (ReferenceEquals(null, other.Value) || ReferenceEquals(null, Value))
            {
                return false;
            }

            return Value.Equals(other.Value);
        }

        public override string ToString()
        {
            return ReferenceEquals(Value, null) ? string.Empty : Value.ToString();
        }

        internal IsolatedReference<T> Cast<T>()
        {
            return new IsolatedReference<T>((T)Value);
        }
    }
}
