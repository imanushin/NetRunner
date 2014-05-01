
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.Invokation.Keywords;
using NetRunner.Executable.RawData;
using NetRunner.Executable.Tests;
using NetRunner.Executable.Tests.Invokation;
using NetRunner.Executable.Tests.Invokation.Functions;
using NetRunner.Executable.Tests.Invokation.Keywords;
using NetRunner.Executable.Tests.RawData;
using NetRunner.ExternalLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetRunner.Executable.Tests.Invokation
{
    partial class TestFunctionReferenceTest
    {
        private static IEnumerable<TestFunctionReference> GetInstancesOfCurrentType()
        {
            foreach (var methodInfo in typeof(TestFunctionReferenceTest).GetMethods())
            {
                foreach (var container in new[] { new FakeFunctionContainer(1), new FakeFunctionContainer(1) })
                {
                    yield return new TestFunctionReference(methodInfo, container);
                }
            }
        }
    }
}

