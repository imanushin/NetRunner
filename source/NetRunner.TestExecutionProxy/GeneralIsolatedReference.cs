using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public class GeneralIsolatedReference : MarshalByRefObject
    {
        private readonly object value;

        public GeneralIsolatedReference(object value)
        {
            this.value = value;
        }

        public override int GetHashCode()
        {
            return ReferenceEquals(null, value) ? 0 : value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as GeneralIsolatedReference;

            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(value, other.value))
            {
                return true;
            }

            if (ReferenceEquals(null, other.value) || ReferenceEquals(null, value))
            {
                return false;
            }

            return value.Equals(other.value);
        }
    }
}
