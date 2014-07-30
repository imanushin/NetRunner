using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Remoting
{
    internal static class RemoteExtensions
    {
        private static readonly Dictionary<string, TypeData> types = new Dictionary<string, TypeData>();

        public static TypeData GetData(this TypeReference reference)
        {
            var identity = reference.StrongIdentity;

            lock (types)
            {
                TypeData data;

                if (!types.TryGetValue(identity, out data))
                {
                    data = ReflectionLoader.LoadData(reference);

                    types[identity] = data;
                }

                return data;
            }
        }

    }
}
