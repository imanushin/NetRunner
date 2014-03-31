using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    internal static class TableParser
    {
        public static AbstractTestFunction ParseTable(HtmlTable table, ReflectionLoader loader)
        {
            Validate.CollectionArgumentHasElements(table.Rows, "table");

            var header = ParseHeader(table.Rows.First());

            if (header == null)
                return EmptyTestFunction.Instance;

            if (table.Rows.Count == 1)
            {
                return new SimpleTestFunction(header);
            }

            var functionToExecute = loader.FindFunction(header.FunctionName, header.Arguments.Count);

            Validate.IsNotNull(functionToExecute, "Unable to find function {0}", header.FunctionName);

            if (!functionToExecute.HasStrongResult)
            {
                var parsedRows = table.Rows.Select(ParseSimpleTestFunction).ToReadOnlyList();

                return new TestFunctionsSequence(parsedRows);
            }

            return ParseTableValuedFunction(table, header);
        }

        private static AbstractTestFunction ParseTableValuedFunction(HtmlTable table, FunctionHeader header)
        {
            Validate.Condition(
                table.Rows.Second().Cells.All(c => c.IsBold),
                "All elements on second row (named 'headers') should be bold. These values are used in the function {0} to match internal field name and table column name.",
                header.FunctionName);

            Validate.Condition(
                table.Rows.Second().Cells.Any(),
                "There are no any values in the second row in function {0}. Please add header row to match table values and function result/input",
                header.FunctionName);

            var headers = table.Rows.Second().Cells.Select(c => c.CleanedContent).ToReadOnlyList();

            var values = new List<ReadOnlyList<string>>();

            foreach (HtmlRow htmlRow in table.Rows.Skip(2))
            {
                Validate.Condition(
                    headers.Count == htmlRow.Cells.Count,
                    "Row {0} contain less values ({1}) than header row ({2}).", htmlRow, htmlRow.Cells.Count, headers.Count);

                Validate.Condition(
                    htmlRow.Cells.All(c => !c.IsBold),
                    "Some of cells of row '{0}' are bold. All rows except first two should have non-bold entry, because bold type means metadata, non-bold type means test value", htmlRow);

                values.Add(htmlRow.Cells.Select(c => c.CleanedContent).ToReadOnlyList());
            }

            return new CollectionArgumentedFunction(headers, values, header);
        }

        private static AbstractTestFunction ParseSimpleTestFunction(HtmlRow row)
        {
            var header = ParseHeader(row);

            if (header == null)
                return EmptyTestFunction.Instance;

            return new SimpleTestFunction(header);
        }

        [CanBeNull]
        private static FunctionHeader ParseHeader(HtmlRow row)
        {
            string functionName = string.Concat(row.Cells.Where(c => c.IsBold).Select(c => c.CleanedContent));
            string[] arguments = row.Cells.Where(c => !c.IsBold).Select(c => c.CleanedContent).ToArray();

            if (string.IsNullOrWhiteSpace(functionName))
                return null;

            return new FunctionHeader(functionName, arguments);
        }

    }
}
