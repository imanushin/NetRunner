
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
    partial class ExecutionFailedMessageTest
    {
        private static IEnumerable<ExecutionFailedMessage> GetInstancesOfCurrentType()
        {
            foreach (HtmlRowReference rowReference in HtmlRowReferenceTest.objects.Objects)
            {
                foreach (var header in new[] { "header1", "header2" })
                {
                    yield return new ExecutionFailedMessage(rowReference, header, "test");
                    yield return new ExecutionFailedMessage(rowReference, header, "test {0}", "123");
                }
            }
        }
    }
}

