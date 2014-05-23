using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Properties;

namespace NetRunner.Executable
{
    internal static class InMemoryAssemblyLoader
    {
        public static void SubscribeDomain(AppDomain domain)
        {
            domain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name;

            if (string.Equals(assemblyName, "HtmlAgilityPack", StringComparison.OrdinalIgnoreCase))
            {
                return Assembly.Load(Resources.HtmlAgilityPack);
            }

            if (string.Equals(assemblyName, "NetRunner.TestExecutionProxy", StringComparison.OrdinalIgnoreCase))
            {
                return Assembly.Load(Resources.NetRunner_TestExecutionProxy);
            }

            return null;
        }
    }
}
