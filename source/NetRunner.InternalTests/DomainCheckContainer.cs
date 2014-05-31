using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.InternalTests
{
    internal sealed class DomainCheckContainer : BaseTestContainer
    {
        public bool CurrentDomainDoesNotContainLibrary(string assemblyName)
        {
            var assemblyFound = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.FullName.IndexOf(assemblyName, StringComparison.OrdinalIgnoreCase) >= 0);

            return ReferenceEquals(null, assemblyFound);
        }

        public bool CurrentDomainIdentityNotEqual(int identity)
        {
            return AppDomain.CurrentDomain.Id != identity;
        }
    }
}
