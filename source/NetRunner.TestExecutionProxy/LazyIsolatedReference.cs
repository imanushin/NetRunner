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
        private readonly TypeReference typeReference;
        private readonly Lazy<IsolatedReference<TType>> lazyInstance;

        internal LazyIsolatedReference(TypeReference typeReference)
            :base(typeReference)
        {
            this.typeReference = typeReference;
            lazyInstance = new Lazy<IsolatedReference<TType>>(CreateItem);
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
    }
}
