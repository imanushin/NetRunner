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
        internal IsolatedReference([CanBeNull]TType value)
            :base(value, typeof(TType))
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
    }
}
