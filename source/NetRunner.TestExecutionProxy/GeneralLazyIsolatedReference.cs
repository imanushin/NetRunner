using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public class GeneralLazyIsolatedReference : GeneralReferenceObject
    {
        private readonly TypeReference typeReference;
        private readonly Lazy<GeneralIsolatedReference> lazyInstance;

        internal GeneralLazyIsolatedReference(TypeReference typeReference)
        {
            this.typeReference = typeReference;
            lazyInstance = new Lazy<GeneralIsolatedReference>(CreateItem);
        }

        private GeneralIsolatedReference CreateItem()
        {
            var constructor = 
                typeReference
                    .TargetType
                    .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(c => !c.GetParameters().Any());

            if (constructor == null)
            {
                throw new InvalidOperationException(string.Format("Unable to create type {0}: unable to find any constructor without parameters", typeReference.Name));
            }

            return new GeneralIsolatedReference(constructor.Invoke(new object[0]), typeReference);
        }

        public virtual GeneralIsolatedReference Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        public TypeReference Type
        {
            get
            {
                return typeReference;
            }
        }
    }
}
