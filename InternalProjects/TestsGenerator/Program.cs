using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonObjectsGenerator;
using TestsGenerator.ReadonlyObjectGenerators;

namespace TestsGenerator
{
    internal static class Program
    {
        private static readonly StringBuilderTraceListener listener = new StringBuilderTraceListener();

        private static int Main(string[] args)
        {
            try
            {
                Trace.Listeners.Add(listener);

                if (args.Length < 1)
                {
                    const string error = "Wrong command line arguments. Please add first argument with the path to the project folder";
                    Trace.WriteLine(error);

                    throw new ArgumentException(error, "args");
                }

                string inputPath = args[0];

                Trace.WriteLine(string.Format("Input path: {0}", inputPath));

                string pathToTargetProject = new DirectoryInfo(inputPath).FullName;

                Trace.WriteLine(string.Format("Path to project: {0}", pathToTargetProject));

                string projectName = new DirectoryInfo(inputPath).Name;

                Trace.WriteLine(string.Format("Project name: {0}", projectName));

                // ReSharper disable PossibleNullReferenceException
                string pathToProjects = new DirectoryInfo(pathToTargetProject).Parent.FullName;
                // ReSharper restore PossibleNullReferenceException

                Trace.WriteLine(string.Format("Path to projects folder: {0}", pathToProjects));

                string pathToBinaries = Path.Combine(pathToProjects, "..\\binary\\currentBuild\\");

                Trace.WriteLine(string.Format("Path to binaries folder: {0}", pathToBinaries));

                string pathToDll = Path.Combine(pathToBinaries, projectName + ".dll");

                if (!File.Exists(pathToDll))
                {
                    Trace.WriteLine(string.Format("File {0} doesn't exists.", pathToDll));

                    pathToDll = Path.Combine(pathToBinaries, projectName + ".exe");
                }

                Trace.WriteLine(string.Format("Path to target binary: {0}", pathToDll));

                Assembly targetAssembly = Assembly.LoadFrom(pathToDll);

                string testProjectName = projectName + ".Tests";

                var generators = new ITestGenerator[] { new ReadonlyObjectTestGenerator() };

                foreach (ITestGenerator generator in generators)
                {
                    string resultFileEntry = generator.GetFileEntry(targetAssembly);

                    string testsProject = pathToTargetProject.Replace(projectName, testProjectName);

                    string resultFilePath = Path.Combine(testsProject, generator.FileName);

                    Trace.WriteLine("Result file: " + resultFilePath);

                    if (File.Exists(resultFilePath))
                    {
                        string oldData = File.ReadAllText(resultFilePath);

                        if (oldData == resultFileEntry)
                            continue;

                        File.Delete(resultFilePath);
                    }

                    File.WriteAllText(resultFilePath, resultFileEntry);
                }
                return 0;
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (Exception loaderException in ex.LoaderExceptions)
                {
                    Trace.WriteLine("Loader exception: " + loaderException);
                }

                Trace.WriteLine("Loaded types: " + string.Join(",", ex.Types.Where(type => type != null).Select(type => type.Name)));

                Console.Out.WriteLine(listener.ToString());

                return -1;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Unhandled exception: " + ex);

                Console.Out.WriteLine(listener.ToString());

                return -1;
            }
        }
    }
}
