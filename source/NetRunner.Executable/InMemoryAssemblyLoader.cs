using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetRunner.Executable.Properties;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable
{
    public sealed class InMemoryAssemblyLoader : MarshalByRefObject
    {
        public static readonly InMemoryAssemblyLoader Instance = new InMemoryAssemblyLoader();

        public InMemoryAssemblyLoader()
        {
        }

        public void SubscribeDomain(AppDomain domain)
        {
            domain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            domain.UnhandledException += domain_UnhandledException;
        }

        private static void domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject;

            Trace.TraceError("Unhandled exception in the domain {0}: {1}", AppDomain.CurrentDomain.FriendlyName, exception);

            Trace.Flush();

            Thread.Sleep(1000);//wait until fitnesse recieve text
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
                return Assembly.Load(Resources.NetRunner_TestExecutionProxy, Resources.NetRunner_TestExecutionProxyPdb);
            }

            return null;
        }
    }
}
