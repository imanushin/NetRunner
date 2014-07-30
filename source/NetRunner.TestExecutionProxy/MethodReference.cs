using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    [Serializable]
    public sealed class MethodReference : IDataCreation<MethodData, MethodInfo>
    {
        private static readonly Dictionary<MethodInfo, MethodReference> methods = new Dictionary<MethodInfo, MethodReference>();

        private MethodReference(MethodBase method)
        {
            var parent = TypeReference.GetType(method.DeclaringType);

            StrongIdentity = parent + ":" + method.Name + "(" + string.Join(",", method.GetParameters().Select(p => p.ParameterType.FullName)) + ")";
        }

        internal static MethodReference GetMethod(MethodInfo method)
        {
            lock (methods)
            {
                return methods.GetOrCreate(method, m =>
                {
                    var result = new MethodReference(m);

                    ReferenceCache.Save(result, method);

                    return result;
                });
            }
        }

        public string StrongIdentity
        {
            get;
            private set;
        }

        public MethodData Create(MethodInfo targetItem)
        {
            return new MethodData(targetItem, this);
        }
    }
}
