
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
    partial class FunctionExecutionResultTest
    {
        private static IEnumerable<FunctionExecutionResult> GetInstancesOfCurrentType()
        {
            foreach (FunctionExecutionResult.FunctionRunResult result in Enum.GetValues(typeof(FunctionExecutionResult.FunctionRunResult)))
            {
                foreach (var changes in new[] { AbstractTableChangeTest.objects.Objects.Skip(1), AbstractTableChangeTest.objects.Take(2) })
                {
                    yield return new FunctionExecutionResult(result, changes);
                }
            }
        }
    }
}

