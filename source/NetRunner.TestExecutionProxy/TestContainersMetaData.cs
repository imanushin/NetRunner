using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    [Serializable]
    public sealed class TestContainersMetaData
    {
        internal TestContainersMetaData()
        {
            Types = new Dictionary<TypeReference, TypeData>();
            Methods = new Dictionary<MethodReference, MethodData>();
            Properties = new Dictionary<PropertyReference, PropertyData>();
            Parameters = new Dictionary<ParameterInfoReference, ParameterInfoData>();
        }

        public Dictionary<TypeReference, TypeData> Types
        {
            get;
            private set;
        }

        public Dictionary<MethodReference, MethodData> Methods
        {
            get;
            private set;
        }

        public Dictionary<PropertyReference, PropertyData> Properties
        {
            get;
            private set;
        }

        public Dictionary<ParameterInfoReference, ParameterInfoData> Parameters
        {
            get;
            private set;
        }

        private void AddType(TypeReference type)
        {
            Types[type] = ReferenceCache.Get(type);
        }

        private void AddMethods(IEnumerable<MethodReference> methods)
        {
            foreach (var method in methods)
            {
                MethodData methodData = ReferenceCache.Get(method);

                Methods[method] = methodData;

                foreach (var parameter in methodData.Parameters)
                {
                    var parameterInfoData = ReferenceCache.Get(parameter);

                    Parameters[parameter] = parameterInfoData;

                    Types[parameterInfoData.ParameterType] = ReferenceCache.Get(parameterInfoData.ParameterType);
                }

                Types[methodData.ReturnType] = ReferenceCache.Get(methodData.ReturnType);
            }
        }

        public static TestContainersMetaData GetMetaData(LazyIsolatedReference<BaseTestContainer>[] testContainers)
        {
            var result = new TestContainersMetaData();

            foreach (var testContainer in testContainers)
            {
                result.AddType(testContainer.Type);

                result.AddMethods(testContainer.AvailableMethods.Select(m => m.MethodReference));
            }

            return result;
        }
    }
}
