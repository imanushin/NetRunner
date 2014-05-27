using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public sealed class IsolatedReference<TType> : MarshalByRefObject
    {
        public IsolatedReference(TType value)
        {
            Value = value;
        }

        [CanBeNull]
        internal TType Value
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

        public TResult ExecuteProperty<TResult>(string propertyName)
        {
            var targetProperty = Value.GetType().GetProperty(propertyName);

            return (TResult)targetProperty.GetValue(Value);
        }

        public FunctionMetaData[] GetMethods()
        {
            return Value.GetType().GetMethods().Select(m => new FunctionMetaData(m)).ToArray();
        }

        public IsolatedReference<object>[] ToArray()
        {
            var enumerable = Value as IEnumerable;

            if (ReferenceEquals(null, enumerable))
                throw new InvalidOperationException(string.Format("Unable to convert value '{0}' of type '{1}' to array", Value, typeof(TType)));

            return enumerable.Cast<object>().Select(o => new IsolatedReference<object>(o)).ToArray();
        }


        [NotNull]
        public IsolatedReference<T> As<T>()
            where T : class
        {
            return new IsolatedReference<T>(Value as T);
        }

        [CanBeNull]
        public string ExecuteMethod(string methodName, string displayName)
        {
            var targetMethod = Value.GetType().GetMethod(methodName);

            targetMethod.Invoke(Value, new object[] { displayName });

            return string.Empty;
        }

        public IsolatedReference<T> Cast<T>()
            where T : class 
        {
            if (ReferenceEquals(null, Value))
            {
                return new IsolatedReference<T>(null);
            }

            var result = As<T>();

            if (result.IsNull)
            {
                throw new InvalidCastException(string.Format("Unable to convert type {0} to {1}", typeof(TType), typeof(T)));
            }

            return result;
        }
    }
}
