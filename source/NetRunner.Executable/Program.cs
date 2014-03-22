using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable
{
    internal static class Program
    {
        private const int assemblylist = 0;
        private const int host = 1;
        private const int port = 2;
        private const int socketToken = 3;
        private const int done = 4;

        private static int Main(string[] args)
        {
            try
            {
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

                string assemblies = args[0];
                string hostValue = args[1];
                int portValue = int.Parse(args[2]);
                string socketTokenValue = args[3];

                return new ApplicationSettings(assemblies, hostValue, portValue, socketTokenValue);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(@"Command pattern example: !define COMMAND_PATTERN {%m %p}", ex);
            }
        }
    }
}
