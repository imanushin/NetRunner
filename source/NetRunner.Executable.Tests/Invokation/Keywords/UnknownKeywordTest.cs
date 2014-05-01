using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Keywords;
using NetRunner.Executable.RawData;
using NetRunner.Executable.Tests.RawData;

namespace NetRunner.Executable.Tests.Invokation.Keywords
{
    partial class UnknownKeywordTest
    {
        private static IEnumerable<UnknownKeyword> GetInstancesOfCurrentType()
        {
            yield return new UnknownKeyword(HtmlCellTest.objects.Objects.Take(1).ToReadOnlyList());
            yield return new UnknownKeyword(HtmlCellTest.objects.Objects.Skip(1).ToReadOnlyList());
        }
    }
}
