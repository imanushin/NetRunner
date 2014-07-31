using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    [Serializable]
    public sealed class ParameterInfoReference : IDataCreation<ParameterInfoData, ParameterInfo>
    {
        private static readonly Dictionary<ParameterInfo, ParameterInfoReference> parameters = new Dictionary<ParameterInfo, ParameterInfoReference>();

        [CanBeNull]
        [NonSerialized]
        private readonly MethodReference localMethod;

        private ParameterInfoReference(ParameterInfo parameter, MethodReference method)
        {
            var methodIdentity = method.StrongIdentity;
            StrongIdentity = methodIdentity + ":" + parameter.Name;
            localMethod = method;
        }

        public string StrongIdentity
        {
            get;
            private set;
        }

        public static ParameterInfoReference GetParameter(ParameterInfo parameter, MethodReference method)
        {
            lock (parameters)
            {
                return parameters.GetOrCreate(parameter, m =>
                {
                    var result = new ParameterInfoReference(parameter, method);

                    ReferenceCache.Save(result, parameter);

                    return result;
                });
            }
        }

        public ParameterInfoData Create(ParameterInfo targetItem)
        {
            var method = localMethod ?? MethodReference.GetMethod((MethodInfo)targetItem.Member);

            return new ParameterInfoData(targetItem, method);
        }
    }
}
