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
        public static string InvokeTable(HtmlTable table, TestCounts currentStatistic, ReflectionLoader loader)
        {
            var functionToInvoke = TableParser.ParseTable(table);

            var result = functionToInvoke.Invoke(loader);

            switch (result)
            {
                case AbstractTestFunction.FunctionRunResult.Fail:
                    currentStatistic.IncrementFailCount();
                    break;
                case AbstractTestFunction.FunctionRunResult.Success:
                    currentStatistic.IncrementSuccessCount();
                    break;
                case AbstractTestFunction.FunctionRunResult.Ignore:
                    currentStatistic.IncrementIgnoreCount();
                    break;
                case AbstractTestFunction.FunctionRunResult.Exception:
                    currentStatistic.IncrementExceptionCount();
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown result: {0}", result));
            }

            return table.GetClonedNode().OuterHtml;
        }
    }
}
