using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    internal static class ReferenceCache
    {
        private static readonly Dictionary<Type, Dictionary<string, object>> cache = new Dictionary<Type, Dictionary<string, object>>();

        private static readonly object syncRoot = new object();

        public static void Save<TData>(IReference<TData> reference, TData data)
            where TData : class
        {
            lock (syncRoot)
            {
                var typeDictionary = cache.GetOrAdd(reference.GetType());

                typeDictionary[reference.StrongIdentity] = data;
            }
        }

        public static TData Get<TData>(IReference<TData> reference)
            where TData : class
        {
            lock (syncRoot)
            {
                var typeDictionary = cache.GetOrAdd(reference.GetType());

                return (TData)typeDictionary[reference.StrongIdentity];
            }
        }
    }
}
