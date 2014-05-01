

using System.Collections.Generic;
using System.Linq;
using NetRunner.Executable.Invokation.Keywords;
using NetRunner.Executable.RawData;
using NetRunner.Executable.Tests.RawData;

namespace NetRunner.Executable.Tests.Invokation.Keywords
{
    public sealed partial class EmptyKeywordTest
    {
        private static IEnumerable<EmptyKeyword> GetInstancesOfCurrentType()
        {
            yield return new EmptyKeyword(HtmlCellTest.objects.Objects.Take(2).ToArray());
        }
    }
}
