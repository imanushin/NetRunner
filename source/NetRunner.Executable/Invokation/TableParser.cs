using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation
{
    internal static class TableParser
    {
        public static AbstractTestFunction ParseTable(HtmlTable table)
        {
            Validate.CollectionArgumentHasElements(table.Rows, "table");

            Validate.ArgumentCondition(table.Rows.Count == 1, "table", "Table should contain only one row");

            HtmlRow row = table.Rows.First();

            string functionName = string.Concat(row.Cells.Where(c => c.IsBold).Select(c => c.CleanedContent));
            string[] arguments = row.Cells.Where(c => !c.IsBold).Select(c => c.CleanedContent).ToArray();

            if (string.IsNullOrWhiteSpace(functionName))
                return null;

            return new SimpleTestFunction(functionName, arguments);
        }
    }
}
