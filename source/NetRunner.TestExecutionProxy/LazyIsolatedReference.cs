using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.TestExecutionProxy
{
    public class LazyIsolatedReference<TType> : GeneralLazyIsolatedReference
    {
        private static readonly string[] ignoredFunctions =
        {
            "ToString", "GetHashCode", "Equals", "GetType"
        };

        private readonly TypeReference typeReference;
        private readonly Lazy<IsolatedReference<TType>> lazyInstance;

        private Lazy<FunctionMetaData[]> availableMethods;

        internal LazyIsolatedReference(TypeReference typeReference)
            :base(typeReference)
        {
            this.typeReference = typeReference;
            lazyInstance = new Lazy<IsolatedReference<TType>>(CreateItem);
            availableMethods = new Lazy<FunctionMetaData[]>(GetAvailableMethods);
        }

        private IsolatedReference<TType> CreateItem()
        {
            return base.Instance.Cast<TType>();
        }

        public override GeneralIsolatedReference Instance
        {
            get
            {
                return TypedInstance;
            }
        }

        public IsolatedReference<TType> TypedInstance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        public FunctionMetaData[] AvailableMethods
        {
            get
            {
                return availableMethods.Value.ToArray();
            }
        }

        private FunctionMetaData[] GetAvailableMethods()
        {
            var targetType = Type.TargetType;

            var availableTests = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                           .Where(f => !ignoredFunctions.Contains(f.Name) && !f.IsSpecialName);

            return availableTests.Select(t => new FunctionMetaData(t, this)).ToArray();
        }
    }
}
