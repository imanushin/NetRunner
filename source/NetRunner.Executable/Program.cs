using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetRunner.Executable
{
    internal static class Program
    {
        private const int assemblylist = 0;
        private const int host = 1;
        private const int port = 2;
        private const int socketToken = 3;

        private static int Main(string[] args)
        {
            try
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
                Trace.AutoFlush = true;

                Debugger.Launch();
                
                var settings = ParseArguments(args);

                FitExecutable.Execute(settings);

                return 0;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unhandled exception: {0}", ex);

                return 1;
            }
        }

        private static ApplicationSettings ParseArguments(IList<string> args)
        {
            try
            {
                string assemblies = args[assemblylist];
                string hostValue = args[host];
                int portValue = int.Parse(args[port]);
                string socketTokenValue = args[socketToken];

                return new ApplicationSettings(assemblies, hostValue, portValue, socketTokenValue);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(@"Command pattern example: !define COMMAND_PATTERN {%m %p}", ex);
            }
        }
    }
}
