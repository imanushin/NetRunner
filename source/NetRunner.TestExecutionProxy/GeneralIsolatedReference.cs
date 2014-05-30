using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public class GeneralIsolatedReference : MarshalByRefObject
    {
        public GeneralIsolatedReference(object value)
        {
            this.Value = value;
        }

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

        public override int GetHashCode()
        {
            return ReferenceEquals(null, Value) ? 0 : Value.GetHashCode();
        }

        public FunctionMetaData[] GetMethods()
        {
            return Value.GetType().GetMethods().Select(m => new FunctionMetaData(m, Value)).ToArray();
        }

        public new TypeReference GetType()
        {
            return new TypeReference(Value.GetType());
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
    }
}
