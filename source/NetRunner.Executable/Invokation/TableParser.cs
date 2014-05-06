using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.Invokation.Keywords;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    internal static class TableParser
    {
        public static AbstractTestFunction ParseTable(HtmlTable table, List<AbstractTableChange> tableParseInformation)
        {
            Validate.CollectionArgumentHasElements(table.Rows, "table");

            var headerRow = table.Rows.First();

            var header = ParseHeader(headerRow);

            if (header == null)
                return EmptyTestFunction.Instance;

            var functionToExecute = ReflectionLoader.Instance.FindFunction(header.FunctionName, header.Arguments.Count);

            if (functionToExecute == null)
            {
                return CreateProblematicFunction(headerRow, header, table.Rows);
            }

            if (table.Rows.Count == 1)
            {
                return new SimpleTestFunction(header, functionToExecute, headerRow);
            }

            var result = ParseTableValuedFunction(table, header, functionToExecute);

            if (result != null)
                return result;

            var parsedRows = table.Rows.Select(row =>
                {
                    try
                    {
                        return ParseSimpleTestFunction(row);
                    }
                    catch (Exception ex)
                    {
                        tableParseInformation.Add(new AddExceptionLine("Unable to parse row", ex, row.RowReference));
                    }

                    return null;

                }).SkipNulls().ToReadOnlyList();

            return new TestFunctionsSequence(parsedRows);
        }

        private static AbstractTestFunction CreateProblematicFunction(HtmlRow headerRow, FunctionHeader header, IReadOnlyCollection<HtmlRow> otherRows)
        {
            var startCell = headerRow.FirstBold;

            var errorHeader = string.Format("Unable to find function {0}", header.FunctionName);
            var errorInfo = string.Format("Unable to find function {0}", header.FunctionName);

            var tableChange = new AddCellExpandableInfo(startCell, errorHeader, errorInfo);

            return new ProblematicFunction(new[]
            {
                tableChange
            }, otherRows);
        }

        [CanBeNull]
        private static AbstractTestFunction ParseTableValuedFunction(HtmlTable table, FunctionHeader header, TestFunctionReference functionToExecute)
        {
            return BaseComplexArgumentedFunction.GetFunction(table.Rows.Second(), table.Rows.Skip(2), header, functionToExecute);
        }

        private static AbstractTestFunction ParseSimpleTestFunction(HtmlRow row)
        {
            var header = ParseHeader(row);

            if (header == null)
                return EmptyTestFunction.Instance;

            var functionReference = ReflectionLoader.Instance.FindFunction(header.FunctionName, header.Arguments.Count);

            if (functionReference == null)
            {
                return CreateProblematicFunction(row, header, new[] { row });
            }

            return new SimpleTestFunction(header, functionReference, row);
        }

        [CanBeNull]
        private static FunctionHeader ParseHeader(HtmlRow row)
        {
            var cells = row.Cells;

            AbstractKeyword keyword = KeywordManager.Parse(cells);

            cells = keyword.PatchedCells;

            var functionCells = row.Cells.Where(c => c.IsBold).ToReadOnlyList();
            string functionName = string.Concat(functionCells.Select(c => c.CleanedContent));
            var arguments = cells.Where(c => !c.IsBold).ToReadOnlyList();

            if (string.IsNullOrWhiteSpace(functionName))
                return null;

            return new FunctionHeader(functionName, arguments, row.RowReference, functionCells.First(), keyword);
        }

    }
}
