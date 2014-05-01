
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
    partial class HtmlRowReferenceTest
    {
        private static IEnumerable<HtmlRowReference> GetInstancesOfCurrentType()
        {
            var document = new HtmlDocument();

            var first = document.CreateElement(HtmlParser.TableRowNodeName);
            first.InnerHtml = "check";

            var second = document.CreateElement(HtmlParser.TableRowNodeName);
            second.InnerHtml = "<b>function name</b>";

            var third = document.CreateElement(HtmlParser.TableRowNodeName);
            third.InnerHtml = "<i>res</i>";

            yield return HtmlRowReference.MarkRowAndGenerateReference(first);
            yield return HtmlRowReference.MarkRowAndGenerateReference(second);
            yield return HtmlRowReference.MarkRowAndGenerateReference(third);
        }
    }
}

