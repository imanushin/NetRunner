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

        public static void Save<TData, TValueData>(IDataCreation<TData, TValueData> reference, TValueData data)
            where TData : class
        {
            lock (syncRoot)
            {
                var typeDictionary = cache.GetOrAdd(reference.GetType());

                typeDictionary[reference.StrongIdentity] = data;
            }
        }

        public static Tuple<TReference, TData>[] Get<TData, TValueData, TReference>(IEnumerable<TReference> references)
            where TData : class
            where TReference : IDataCreation<TData, TValueData>
        {
            var refArray = references.ToArray();

            if (!refArray.Any())
            {
                return new Tuple<TReference, TData>[0];
            }

            lock (syncRoot)
            {
                var typeDictionary = cache.GetOrAdd(refArray.First().GetType());

                return refArray.Select(r => Tuple.Create(r, r.Create((TValueData)typeDictionary[r.StrongIdentity]))).ToArray();
            }
        }

        public static TData Get<TData, TValueData>(IDataCreation<TData, TValueData> reference)
            where TData : class
        {
            lock (syncRoot)
            {
                var typeDictionary = cache.GetOrAdd(reference.GetType());

                var value = (TValueData)typeDictionary[reference.StrongIdentity];

                return reference.Create(value);
            }
        }
    }
}
