using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation
{
    internal static class RootInvoker
    {
        public static string InvokeTable(HtmlTable table, TestCounts currentStatistic)
        {
            var functionToInvoke = TableParser.ParseTable(table);

            currentStatistic.IncrementSuccessCount();

            return table.GetClonedNode().OuterHtml;
        }
    }
}
