using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.Invokation
{
    public sealed class Isolated<T> : IDisposable where T : MarshalByRefObject
    {
        private readonly T value;

        public Isolated(AppDomain domain)
        {
            Type type = typeof(T);

            value = (T)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public T Value
        {
            get
            {
                return value;
            }
        }

        public void Dispose()
        {
            
        }
    }
}
