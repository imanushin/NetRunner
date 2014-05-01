
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
    partial class HtmlTableTest
    {
        private static IEnumerable<HtmlTable> GetInstancesOfCurrentType()
        {
            var document = new HtmlDocument();

            var first = document.CreateElement(HtmlParser.TableRowNodeName);
            first.InnerHtml = "check";

            var second = document.CreateElement(HtmlParser.TableRowNodeName);
            second.InnerHtml = "<b>function name</b>";

            foreach (var htmlNode in new[] { first, second })
            {
                foreach (var rows in new[] { HtmlRowTest.objects.Objects.Skip(1).ToReadOnlyList(), HtmlRowTest.objects.Objects.Take(2).ToReadOnlyList() })
                {
                    foreach (var suffix in new[] { "suffix1", "" })
                    {
                        yield return new HtmlTable(rows, htmlNode, suffix);
                    }
                }
            }

        }
    }
}

