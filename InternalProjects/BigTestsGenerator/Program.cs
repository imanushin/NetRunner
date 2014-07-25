using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigTestsGenerator
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            if (args.Length < 2)
            {
                Trace.TraceError("Unable to generate text: please specify path to the csharp file and to the FitNesse content file");
            }

            try
            {
                var resultCsharpFile = args[0];
                var resultFitNesseFile = args[1];

                var charpFile = CSharpTestFileContent.Generate();

                if (!File.Exists(resultCsharpFile) || !string.Equals(File.ReadAllText(resultCsharpFile), charpFile.FileContent, StringComparison.OrdinalIgnoreCase))
                {
                    File.WriteAllText(resultCsharpFile, charpFile.FileContent);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
    }
}
