
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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetRunner.Executable.Tests.Invokation.Functions
{
    partial class AddCellExpandableExceptionTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddCellExpandableException> GetInstancesOfCurrentType()
        {
            foreach (var htmlCell in HtmlCellTest.objects.Objects)
            {
                foreach (var exception in new[] { new Exception("1"), new Exception("2") })
                {
                    foreach (var headerFormat in new[] { "123 {0}", "{0} 123" })
                    {
                        foreach (var argument in new[] { 567, 890 })
                        {
                            yield return new AddCellExpandableException(htmlCell, exception, headerFormat, argument);
                        }
                    }
                }
            }
        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
        }
    }
}

