using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public sealed class ReflectionInvoker : MarshalByRefObject
    {
        private string[] assemblyFolders = new string[0];

        public ReflectionInvoker()
        {
            if (Trace.Listeners.Count == 0)
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
            }
        }

        private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var loadLog = new StringBuilder();

                var assemblyName = new AssemblyName(args.Name).Name;

                loadLog.AppendFormat("Try to find assembly '{0}' from the custom locations: {1}", assemblyName, assemblyFolders);
                loadLog.AppendLine();

                var targetFileName = assemblyName + ".dll";

                var filesCandidates = assemblyFolders.Select(f => Path.Combine(f, targetFileName)).Where(File.Exists).ToArray();

                loadLog.AppendFormat("Existing files with '{0}': {1}", assemblyName, string.Join(", ", filesCandidates));
                loadLog.AppendLine();

                foreach (var candidate in filesCandidates)
                {
                    try
                    {
                        return Assembly.ReflectionOnlyLoad(candidate);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Unable to load assembly {0} from file {1}: {2}.", args.Name, candidate, ex);
                    }
                }

                Trace.TraceInformation(loadLog.ToString());

                Trace.TraceError("Unable to load assembly {0}. Files candidates: {1}, ", args.Name, filesCandidates);
            }
            catch (Exception ex)
            {
                Trace.TraceError("General assembly found error: {0}", ex);
            }

            return null;
        }

        public void AddAssemblyLoadFolders(string[] newAssemblyFolders)
        {
            assemblyFolders = assemblyFolders.Concat(newAssemblyFolders).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }
    }
}
