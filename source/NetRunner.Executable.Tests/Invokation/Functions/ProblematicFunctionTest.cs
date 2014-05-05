
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
    partial class ProblematicFunctionTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<ProblematicFunction> GetInstancesOfCurrentType()
        {
            foreach (AbstractTableChange[] tableChanges in AbstractTableChangeTest.CreateNonEmptyObjectsArrays())
            {
                foreach (HtmlRow[] rows in HtmlRowTest.CreateNonEmptyObjectsArrays().Take(3))
                {
                    yield return new ProblematicFunction(tableChanges, rows);
                }
            }
        }
    }
}

