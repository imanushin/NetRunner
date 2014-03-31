using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.Invokation
{
    internal static class TestExecutionLog
    {
        private static readonly StringBuilder builder = new StringBuilder();

        public static void Trace(string format, params object[] args)
        {
            lock (builder)
            {
                builder.AppendFormat(format, args);
                builder.AppendLine();
            }
        }

        public static string ExtractLogged()
        {
            lock (builder)
            {
                var result = builder.ToString();

                builder.Clear();

                return result;
            }
        }
    }
}
