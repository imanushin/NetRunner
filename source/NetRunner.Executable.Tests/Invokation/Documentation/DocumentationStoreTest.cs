using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Documentation;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Tests.Invokation.Documentation
{
    [TestClass]
    public sealed class DocumentationStoreTest
    {
        [TestMethod]
        public void GenericItemsTest()
        {
            LoadAssemblies();

            var method = typeof(NetRunnerTestContainer)
                .GetMethods()
                .First(m => string.Equals(m.Name, "CompareEnvironmentVariableTarget", StringComparison.Ordinal));

            var document = DocumentationStore.GetFor(new MethodData(method, MethodReference.GetMethod(method)));

            Assert.IsNotNull(document);
            Assert.IsFalse(string.IsNullOrWhiteSpace(document));
        }

        internal static void LoadAssemblies()
        {
            var fitSampleAssembly = typeof (InOutObject).Assembly.Location;

            DocumentationStore.LoadForAssemblies(new[]
            {
                fitSampleAssembly
            }.ToReadOnlyList());
        }
    }
}
