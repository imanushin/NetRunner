using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Remoting
{
    internal static class RemoteExtensions
    {
        private static class GenericHelper<TData>
            where TData : class
        {
            private static readonly Dictionary<string, TData> cache = new Dictionary<string, TData>();

            public static TData GetData(IReference<TData> reference)
            {
                lock (cache)
                {
                    TData data;

                    string strongIdentity = reference.StrongIdentity;

                    if (!cache.TryGetValue(strongIdentity, out data))
                    {
                        data = ReflectionLoader.LoadData(reference);

                        cache[strongIdentity] = data;
                    }

                    return data;
                }
            }

        }

        public static TData GetData<TData>(this IReference<TData> reference)
            where TData : class
        {
            return GenericHelper<TData>.GetData(reference);
        }
    }
}
