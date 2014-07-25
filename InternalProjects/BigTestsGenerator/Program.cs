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

                WriteIfNeeded(resultCsharpFile, charpFile.FileContent);

                var fitNesseData = FitNesseFileGenerator.Generate(charpFile.AvailableFunctions);

                WriteIfNeeded(resultFitNesseFile, fitNesseData);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }

        private static void WriteIfNeeded(string filePath, string fileContent)
        {
            if (!File.Exists(filePath) || !string.Equals(File.ReadAllText(filePath), fileContent, StringComparison.OrdinalIgnoreCase))
            {
                File.WriteAllText(filePath, fileContent);
            }

        }
    }
}
