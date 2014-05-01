
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
    partial class AppendRowWithCellsTest
    {
        private static IEnumerable<AppendRowWithCells> GetInstancesOfCurrentType()
        {
            foreach (string cssClass in new[] { "1", "2" })
            {
                foreach (var cells in new[] { new[] { "1", "2" }, new[] { "2", "1" }, new[] { "2", "3" } })
                {
                    yield return new AppendRowWithCells(cssClass, cells);
                }
            }
        }
    }
}

