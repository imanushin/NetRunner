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

            public static void Save(IEnumerable<KeyValuePair<IDataCreation<TData, TValueData>, TData>> referenceToData)
            {
                lock (cache)
                {
                    foreach (var item in referenceToData)
                    {
                        cache[item.Key.StrongIdentity] = item.Value;
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
            GenericHelper<TypeReference, Type>.Save(data.Types.Cast<KeyValuePair<IDataCreation<TypeReference, Type>, TypeReference>>());
            GenericHelper<PropertyReference, PropertyInfo>.Save(data.Properties.Cast<KeyValuePair<IDataCreation<PropertyReference, PropertyInfo>, PropertyReference>>());
            GenericHelper<MethodReference, MethodInfo>.Save(data.Methods.Cast<KeyValuePair<IDataCreation<MethodReference, MethodInfo>, MethodReference>>());
            GenericHelper<ParameterInfoReference, ParameterInfo>.Save(data.Parameters.Cast<KeyValuePair<IDataCreation<ParameterInfoReference, ParameterInfo>, ParameterInfoReference>>());
        }
    }
}
