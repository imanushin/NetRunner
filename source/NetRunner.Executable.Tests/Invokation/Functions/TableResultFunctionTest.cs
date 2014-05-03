
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace NetRunner.Executable.Tests.Invokation.Functions
{
    partial class TableResultFunctionTest
    {
        private static IEnumerable<TableResultFunction> GetInstancesOfCurrentType()
        {
            foreach (var functionHeader in FunctionHeaderTest.objects.Objects.Take(3))
            {
                foreach (var functionReference in TestFunctionReferenceTest.objects.Objects.Take(3))
                {
                    foreach (var header in HtmlRowTest.objects.Objects.Take(3))
                    {
                        foreach (var rows in HtmlRowTest.CreateNonEmptyObjectsArrays())
                        {
                            yield return new TableResultFunction(header, rows, functionHeader, functionReference);
                        }
                    }
                }
            }
        }
    }
}

