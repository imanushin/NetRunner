using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using NetRunner.Executable.Properties;

namespace NetRunner.Executable
{
    internal static class Program
    {
        private const int assemblylist = 0;
        private const int host = 1;

        private static int Main(string[] args)
        {
            try
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
                Trace.AutoFlush = true;

                //Debugger.Launch();

                InMemoryAssemblyLoader.Instance.SubscribeDomain(AppDomain.CurrentDomain, false);

                var settings = ParseArguments(args);

                FitExecutable.Execute(settings);

                return 0;
            }
            catch (Exception ex)
            {
                Trace.Listeners.Add(new EventLogTraceListener());
                Trace.TraceError("Unhandled exception: {0}", ex);

                Trace.Flush();

                Thread.Sleep(1000); //wait for data transfer

                return 1;
            }
        }

        private static bool IsFitnesseParent()
        {
            return Process.GetCurrentProcess().Parent().ProcessName.Contains("java");
        }

        private static ApplicationSettings ParseArguments(IList<string> args)
        {
            try
            {
                string assemblies = args[assemblylist];
                string hostValue = args[host];
                int portValue = int.Parse(args[args.Count - 2]);
                string socketTokenValue = args[args.Count - 1];

                return new ApplicationSettings(assemblies, hostValue, portValue, socketTokenValue);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(@"Command pattern example: !define COMMAND_PATTERN {%m %p}", ex);
            }
        }

        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexdName;
                }
            }

            return processIndexdName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int)parentId.NextValue());
        }

        public static Process Parent(this Process process)
        {
            return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
        }
    }
}
