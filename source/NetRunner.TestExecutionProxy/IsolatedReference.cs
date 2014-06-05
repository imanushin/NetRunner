using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public class IsolatedReference<TType> : GeneralIsolatedReference
    {
        public IsolatedReference([CanBeNull]TType value)
            :base(value)
        {
            Value = value;
        }

        [CanBeNull]
        internal TType Value
        {
            get;
            private set;
        }

        public TResult ExecuteProperty<TResult>(string propertyName)
        {
            var targetProperty = Value.GetType().GetProperty(propertyName);

            return (TResult)targetProperty.GetValue(Value);
        }

        public IsolatedReference<object>[] ToArray()
        {
            var enumerable = Value as IEnumerable;

            if (ReferenceEquals(null, enumerable))
                throw new InvalidOperationException(string.Format("Unable to convert value '{0}' of type '{1}' to array", Value, typeof(TType)));

            return enumerable.Cast<object>().Select(o => new IsolatedReference<object>(o)).ToArray();
        }


        public ExecutionResult ExecuteMethod<TResult>(Func<TType, TResult> method)
        {
            try
            {
                var result = method(Value);

                return new ExecutionResult(new IsolatedReference<object>(result));
            }
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }

        public ExecutionResult ExecuteMethod(string methodName, string displayName)
        {
            try
            {
                var targetMethod = Value.GetType().GetMethod(methodName);

                var result = targetMethod.Invoke(Value, new object[] { displayName });

                return new ExecutionResult(new IsolatedReference<object>(result));
            }
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }

        private Type ValueType
        {
            get
            {
                return ReferenceEquals(null, Value) ? typeof(TType) : Value.GetType();
            }
        }

        public string GetTypeName()
        {
            return ValueType.Name;
        }

        public string GetTypeFullName()
        {
            return ValueType.FullName;
        }
    }
}
