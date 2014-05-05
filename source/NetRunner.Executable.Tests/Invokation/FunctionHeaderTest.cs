
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
    partial class FunctionHeaderTest
    {
        private static IEnumerable<FunctionHeader> GetInstancesOfCurrentType()
        {
            foreach (var name in new[] { "name1", "name2" })
            {
                foreach (var args in HtmlCellTest.CreateNonEmptyObjectsArrays())
                {
                    foreach (HtmlRowReference htmlRowReference in HtmlRowReferenceTest.objects.Objects.Take(3))
                    {
                        foreach (AbstractKeyword keyword in AbstractKeywordTest.objects.Objects.Take(3))
                        {
                            foreach (var firstFunctionCell in HtmlCellTest.objects.Objects.Take(3))
                            {
                                yield return new FunctionHeader(name, args, htmlRowReference, firstFunctionCell, keyword);
                            }

                        }
                    }
                }
            }
        }
    }
}

