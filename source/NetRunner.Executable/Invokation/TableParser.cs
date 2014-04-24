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
        public static AbstractTestFunction ParseTable(HtmlTable table, ReflectionLoader loader, List<AbstractTableChange> tableParseInformation)
        {
            Validate.CollectionArgumentHasElements(table.Rows, "table");

            var header = ParseHeader(table.Rows.First());

            if (header == null)
                return EmptyTestFunction.Instance;

            var functionToExecute = loader.FindFunction(header.FunctionName, header.Arguments.Count);

            if (table.Rows.Count == 1)
            {
                return new SimpleTestFunction(header, functionToExecute);
            }

            Validate.IsNotNull(functionToExecute, "Unable to find function {0}", header.FunctionName);

            if (!functionToExecute.HasStrongResult)
            {
                var parsedRows = table.Rows.Select(row =>
                {
                    try
                    {
                        return ParseSimpleTestFunction(row, loader);
                    }
                    catch (Exception ex)
                    {
                        tableParseInformation.Add(new AddExceptionLine("Unable to parse row", ex, row.RowReference));
                    }

                    return null;

                }).SkipNulls().ToReadOnlyList();

                return new TestFunctionsSequence(parsedRows);
            }

            return ParseTableValuedFunction(table, header, functionToExecute);
        }

        private static AbstractTestFunction ParseTableValuedFunction(HtmlTable table, FunctionHeader header, TestFunctionReference functionToExecute)
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

            return new CollectionArgumentedFunction(headers, table.Rows.Skip(2), header, functionToExecute);
        }

        private static AbstractTestFunction ParseSimpleTestFunction(HtmlRow row, ReflectionLoader loader)
        {
            var header = ParseHeader(row);

            if (header == null)
                return EmptyTestFunction.Instance;

            var functionReference = loader.FindFunction(header.FunctionName, header.Arguments.Count);

            Validate.IsNotNull(functionReference, "Unable to find function {0}", header.FunctionName);

            return new SimpleTestFunction(header, functionReference);
        }

        [CanBeNull]
        private static FunctionHeader ParseHeader(HtmlRow row)
        {
            var cells = row.Cells;

            var firstCell = cells.First();

            AbstractKeyword keyword = null;

            if (!firstCell.IsBold)//first non-bold cell could be keyword only
            {
                keyword = KeywordManager.Parse(firstCell);

                cells = keyword.PatchCells(cells);
            }

            string functionName = string.Concat(row.Cells.Where(c => c.IsBold).Select(c => c.CleanedContent));
            string[] arguments = cells.Where(c => !c.IsBold).Select(c => c.CleanedContent).ToArray();

            if (string.IsNullOrWhiteSpace(functionName))
                return null;

            return new FunctionHeader(functionName, arguments, row.RowReference, keyword);
        }

    }
}
