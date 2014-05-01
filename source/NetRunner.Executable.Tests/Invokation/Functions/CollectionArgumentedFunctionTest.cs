
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

namespace NetRunner.Executable.Tests.Invokation.Functions
{
    partial class CollectionArgumentedFunctionTest
    {
        private static IEnumerable<CollectionArgumentedFunction> GetInstancesOfCurrentType()
        {
            foreach (var columnNames in new[] { new[] { "1", "2" }.ToReadOnlyList(), new[] { "2", "3" }.ToReadOnlyList() })
            {
                foreach (var rows in new[] { HtmlRowTest.objects.Objects.Skip(1).ToReadOnlyList(), HtmlRowTest.objects.Objects.Take(2).ToReadOnlyList() })
                {
                    foreach (FunctionHeader function in FunctionHeaderTest.objects.Objects)
                    {
                        foreach (TestFunctionReference testFunctionReference in TestFunctionReferenceTest.objects.Objects)
                        {
                            yield return new CollectionArgumentedFunction(columnNames, rows, function, testFunctionReference);
                           
                        }
                    }
                }
            }
        }
    }
}

