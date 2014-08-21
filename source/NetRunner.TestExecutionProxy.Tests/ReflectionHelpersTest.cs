using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetRunner.TestExecutionProxy.Tests
{
    [TestClass]
    public sealed class ReflectionHelpersTest
    {
        [TestMethod]
        public void GetTypeNameWithoutGenericsTest1()
        {
            var type = typeof(string);

            var result = ReflectionHelpers.GetTypeNameWithoutGenerics(type);

            Assert.AreEqual(type.FullName, result);
        }

        [TestMethod]
        public void GetTypeNameWithoutGenericsTest2()
        {
            var type = typeof(List<string>);

            var result = ReflectionHelpers.GetTypeNameWithoutGenerics(type);

            Assert.AreEqual("System.Collections.Generic.List", result);
        }

        [TestMethod]
        public void GetTypeNameWithoutGenericsTest3()
        {
            var type = typeof(Dictionary<string, int>);

            var result = ReflectionHelpers.GetTypeNameWithoutGenerics(type);

            Assert.AreEqual("System.Collections.Generic.Dictionary", result);
        }
    }
}
