using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class CollectionResultFunction : BaseComplexArgumentedFunction
    {
        public static readonly Type BaseType = typeof (IEnumerable);

        public CollectionResultFunction(
            HtmlRow columnsRow,
            IEnumerable<HtmlRow> rows,
            FunctionHeader function,
            TestFunctionReference functionToExecute) :
            base(columnsRow, rows, function, functionToExecute)
        {
        }

        private static bool CompareItems(object resultObject, string expectedResult, string propertyName, out object resultValue)
        {
            Type propertyType;

            if (!ReflectionLoader.Instance.TryReadPropery(resultObject, propertyName, out propertyType, out resultValue))
            {
                return false;
            }

            if (ReferenceEquals(null, resultValue))
            {
                return string.IsNullOrEmpty(expectedResult);
            }

            var expectedObject = ParametersConverter.ConvertParameter(expectedResult, propertyType);

            return resultValue.Equals(expectedObject);
        }


        protected override FunctionExecutionResult ProcessResult(object mainFunctionResult)
        {
            var collectionResult = (IEnumerable)mainFunctionResult;

            collectionResult = collectionResult ?? new object[0];

            var orderedResult = collectionResult.Cast<object>().ToArray();

            var allRight = true;
            var exceptionOccurred = false;

            var tableChanges = new List<AbstractTableChange>();

            for (int rowIndex = 0; rowIndex < orderedResult.Length && rowIndex < Rows.Count; rowIndex++)
            {
                var resultObject = orderedResult[rowIndex];

                var currentRow = Rows[rowIndex];

                for (int columnIndex = 0; columnIndex < CleanedColumnNames.Count; columnIndex++)
                {
                    try
                    {
                        var expectedResult = currentRow.Cells[columnIndex].CleanedContent;

                        object actualValue;

                        var currentIsOk = CompareItems(resultObject, expectedResult, CleanedColumnNames[columnIndex], out actualValue);

                        var cellChange = currentIsOk
                            ? new CssClassCellChange(currentRow.Cells[columnIndex], HtmlParser.PassCssClass)
                            : new ShowActualValueCellChange(currentRow.Cells[columnIndex], actualValue);

                        tableChanges.Add(cellChange);

                        allRight &= currentIsOk;
                    }
                    catch (ConversionException ex)
                    {
                        tableChanges.Add(new CssClassCellChange(currentRow.Cells[columnIndex], HtmlParser.ErrorCssClass));

                        tableChanges.Add(new AddCellExpandableException(currentRow.Cells[columnIndex], ex, "Unable to parse cell"));

                        exceptionOccurred = true;

                        allRight = false;
                    }
                }
            }

            for (int rowIndex = orderedResult.Length; rowIndex < Rows.Count; rowIndex++)
            {
                var currentRow = Rows[rowIndex];

                var cells = currentRow.Cells.Select(c => c.CleanedContent + "<br/> <i class=\"code\">missing</i>").ToReadOnlyList();

                tableChanges.Add(new AppendRowWithCells(HtmlParser.FailCssClass, cells));

                allRight = false;
            }

            for (int rowIndex = Rows.Count; rowIndex < orderedResult.Length; rowIndex++)
            {
                var resultObject = orderedResult[rowIndex];

                var cells = CleanedColumnNames.Select(name => ReadProperty(name, resultObject) + "<br/> <i class=\"code\">surplus</i>").ToReadOnlyList();

                tableChanges.Add(new AppendRowWithCells(HtmlParser.FailCssClass, cells));

                allRight = false;
            }

            return FormatResult(exceptionOccurred, allRight, tableChanges);
        }

        private static string ReadProperty(string propertyName, object resultObject)
        {
            object resultValue;
            Type propertyType;

            if (!ReflectionLoader.Instance.TryReadPropery(resultObject, propertyName, out propertyType, out resultValue))
                return string.Format("Unable to read property {0}", propertyName);

            return (resultValue ?? string.Empty).ToString();
        }

    }
}
