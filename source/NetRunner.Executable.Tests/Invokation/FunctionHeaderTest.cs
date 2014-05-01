
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
                foreach (var args in new[] { new[] { "arg1", "arg2" }, new string[0] })
                {
                    foreach (HtmlRowReference htmlRowReference in HtmlRowReferenceTest.objects.Objects)
                    {
                        foreach (AbstractKeyword keyword in AbstractKeywordTest.objects.Objects)
                        {
                            yield return new FunctionHeader(name, args, htmlRowReference, keyword);

                        }
                    }
                }
            }
        }
    }
}

