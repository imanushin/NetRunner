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
        public static TestExecutionPlan GenerateTestExecutionPlan(FitnesseHtmlDocument document)
        {
            var parsedTables = document.Tables.Select(ParseTable).ToReadOnlyList();

            var textBeforeFirstTable = document.TextBeforeFirstTable;

            return new TestExecutionPlan(textBeforeFirstTable, parsedTables);
        }

        private static ParsedTable ParseTable(HtmlTable table)
        {
            var tableParseInformation = new List<AbstractTableChange>();

            Validate.CollectionArgumentHasElements(table.Rows, "table");

            var headerRow = table.Rows.First();

            var header = ParseHeader(headerRow);

            if (header == null)
                return new ParsedTable(table, EmptyTestFunction.Instance, ReadOnlyList<AbstractTableChange>.Empty);

            var functionToExecute = ReflectionLoader.FindFunction(header.FunctionName, header.Arguments.Count);

            if (functionToExecute == null)
            {
                return new ParsedTable(table, CreateMissingFunction(headerRow, header, table.Rows), ReadOnlyList<AbstractTableChange>.Empty);
            }

            if (table.Rows.Count == 1)
            {
                return new ParsedTable(table, new SimpleTestFunction(header, functionToExecute, headerRow), ReadOnlyList<AbstractTableChange>.Empty);
            }

            var result = ParseTableValuedFunction(table, header, functionToExecute);

            if (result != null)
                return new ParsedTable(table, result, ReadOnlyList<AbstractTableChange>.Empty);

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

            return new ParsedTable(table, new TestFunctionsSequence(parsedRows), tableParseInformation);
        }

        private static AbstractTestFunction CreateMissingFunction(HtmlRow headerRow, FunctionHeader header, IReadOnlyCollection<HtmlRow> otherRows)
        {
            var startCell = headerRow.FirstBold;

            var errorHeader = string.Format("Unable to find function {0} with {1} parameter(s)", header.FunctionName, header.Arguments.Count);
            var errorInfo = string.Format("Unable to find function {0}", header.FunctionName);

            var tableChange = new AddCellExpandableInfo(startCell, errorHeader, errorInfo);

            return new ProblematicFunction(new[]
            {
                tableChange
            }, otherRows, header.Keyword);
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

            var functionReference = ReflectionLoader.FindFunction(header.FunctionName, header.Arguments.Count);

            if (functionReference == null)
            {
                return CreateMissingFunction(row, header, new[] { row });
            }

            return new SimpleTestFunction(header, functionReference, row);
        }

        [CanBeNull]
        private static FunctionHeader ParseHeader(HtmlRow row)
        {
            var cells = row.Cells;

            AbstractKeyword keyword = KeywordManager.Parse(cells);

            cells = keyword.PatchedCells;

            var functionCells = cells.Where(c => c.IsBold).ToReadOnlyList();
            string functionName = string.Concat(functionCells.Select(c => c.CleanedContent));
            var arguments = cells.Where(c => !c.IsBold).ToReadOnlyList();

            if (string.IsNullOrWhiteSpace(functionName))
                return null;

            return new FunctionHeader(functionName, arguments, row.RowReference, functionCells, keyword);
        }

    }
}
