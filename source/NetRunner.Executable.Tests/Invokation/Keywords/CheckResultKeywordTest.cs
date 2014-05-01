using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Keywords;
using NetRunner.Executable.RawData;
using NetRunner.Executable.Tests.RawData;

namespace NetRunner.Executable.Tests.Invokation.Keywords
{
    public sealed partial class CheckResultKeywordTest
    {
        private static IEnumerable<CheckResultKeyword> GetInstancesOfCurrentType()
        {
            var document = new HtmlDocument();

            var first = document.CreateElement("td");
            first.InnerHtml = "check";

            var second = document.CreateElement("td");
            second.InnerHtml = "<b>function name</b>";

            var last1 = document.CreateElement("td");
            last1.InnerHtml = "<i>res</i>";

            var last2 = document.CreateElement("td");
            last2.InnerHtml = "res";

            yield return CheckResultKeyword.TryParse(new[] { new HtmlCell(first), new HtmlCell(second), new HtmlCell(last1) });
            yield return CheckResultKeyword.TryParse(new[] { new HtmlCell(first), new HtmlCell(second), new HtmlCell(last2) });
        }
    }
}
