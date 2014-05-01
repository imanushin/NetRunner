
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

namespace NetRunner.Executable.Tests.RawData
{
    partial class HtmlCellTest
    {
        private static IEnumerable<HtmlCell> GetInstancesOfCurrentType()
        {
            var document = new HtmlDocument();

            var first = document.CreateElement("td");
            first.InnerHtml = "check";

            var second = document.CreateElement("td");
            second.InnerHtml = "<b>function name</b>";

            var third = document.CreateElement("td");
            third.InnerHtml = "<i>res</i>";

            yield return new HtmlCell(first.Clone());
            yield return new HtmlCell(second.Clone());
            yield return new HtmlCell(third.Clone());
        }
    }
}

