using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;
using NetRunner.Executable.Tests.RawData;

namespace NetRunner.Executable.Tests.Invokation.Functions
{
    partial class AddCellExpandableInfoTest
    {
        private static IEnumerable<AddCellExpandableInfo> GetInstancesOfCurrentType()
        {
            foreach (var header in new[] { "header1", "header2" })
            {
                foreach (var info in new[] { "info1", "info2" })
                {
                    foreach (HtmlCell htmlCell in HtmlCellTest.objects.Objects)
                    {
                        yield return new AddCellExpandableInfo(htmlCell, header, info);
                    }
                }
            }
        }
    }
}
