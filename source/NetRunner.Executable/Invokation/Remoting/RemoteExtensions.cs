using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Remoting
{
    internal static class RemoteExtensions
    {
        private static class GenericHelper<TData, TValueData>
            where TData : class
        {
            private static readonly Dictionary<string, TData> cache = new Dictionary<string, TData>();

            public static TData GetData(IDataCreation<TData, TValueData> reference)
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

            public static void Save<TReference>(IEnumerable<Tuple<TReference, TData>> referenceToData)
                where TReference : IDataCreation<TData, TValueData>
            {
                lock (cache)
                {
                    foreach (var item in referenceToData)
                    {
                        cache[item.Item1.StrongIdentity] = item.Item2;
                    }
                }
            }
        }

        public static TData GetData<TData, TValueData>(this IDataCreation<TData, TValueData> reference)
            where TData : class
        {
            return GenericHelper<TData, TValueData>.GetData(reference);
        }


        [CanBeNull]
        public static PropertyReference GetProperty(this TypeData type, string propertyName)
        {
            return type
                    .Properties
                    .FirstOrDefault(p => String.Equals(p.GetData().Name, propertyName, StringComparison.OrdinalIgnoreCase));

        }

        public static void Cache(TestContainersMetaData data)
        {
            GenericHelper<TypeData, Type>.Save(data.Types.Select(kv => Tuple.Create(kv.Key,kv.Value)));
            GenericHelper<PropertyData, PropertyInfo>.Save(data.Properties.Select(kv => Tuple.Create(kv.Key, kv.Value)));
            GenericHelper<MethodData, MethodInfo>.Save(data.Methods.Select(kv => Tuple.Create(kv.Key, kv.Value)));
            GenericHelper<ParameterInfoData, ParameterInfo>.Save(data.Parameters.Select(kv => Tuple.Create(kv.Key, kv.Value)));
        }
    }
}
