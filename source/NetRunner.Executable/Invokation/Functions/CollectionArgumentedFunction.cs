using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class CollectionArgumentedFunction : AbstractTestFunction
    {
        public CollectionArgumentedFunction(ReadOnlyList<string> columnNames, IEnumerable<HtmlRow> rows, FunctionHeader function, TestFunctionReference functionToExecute)
        {
            Validate.ArgumentIsNotNull(function, "function");
            Validate.ArgumentIsNotNull(rows, "rows");
            Validate.ArgumentIsNotNull(functionToExecute, "functionToExecute");
            Validate.CollectionArgumentHasElements(columnNames, "columnNames");

            Function = function;
            ColumnNames = columnNames;
            Rows = rows.ToReadOnlyList();
            FunctionReference = functionToExecute;
        }

        public TestFunctionReference FunctionReference
        {
            get;
            private set;
        }

        public FunctionHeader Function
        {
            get;
            private set;
        }

        public ReadOnlyList<string> ColumnNames
        {
            get;
            private set;
        }

        public ReadOnlyList<HtmlRow> Rows
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Function;
            yield return ColumnNames;
            yield return Rows;
            yield return FunctionReference;
        }

        public override FunctionExecutionResult Invoke(ReflectionLoader loader)
        {
            var resultType = FunctionReference.ResultType;

            if (resultType.IsAssignableFrom(typeof(IEnumerable)))
            {
                return InvokeCollection(loader);
            }

            if (resultType.IsAssignableFrom(typeof(BaseTableResult)))
            {
                return InvokeTable(loader);
            }

            throw new InvalidOperationException("Result type {0} does not supported");
        }

        private FunctionExecutionResult InvokeTable(ReflectionLoader loader)
        {
            throw new NotImplementedException();
        }

        private FunctionExecutionResult InvokeCollection(ReflectionLoader loader)
        {
            var result = (IEnumerable)InvokeFunction(loader, FunctionReference, Function);

            result = result ?? new object[0];

            var orderedResult = result.Cast<object>().ToArray();

            var allRight = true;

            var tableChanges = new List<AbstractTableChange>();

            bool isInputDataCorrect = CheckInputData(tableChanges);

            for (int rowIndex = 0; rowIndex < orderedResult.Length && rowIndex < Rows.Count; rowIndex++)
            {
                var resultObject = orderedResult[rowIndex];

                var currentRow = Rows[rowIndex];

                for (int columnIndex = 0; columnIndex < ColumnNames.Count; columnIndex++)
                {
                    try
                    {
                        var expectedResult = currentRow.Cells[columnIndex].CleanedContent;

                        var currentIsOk = CompareItems(resultObject, expectedResult, ColumnNames[rowIndex], loader, tableChanges);

                        var cellChange = new ChangeCellCssClass(currentRow.RowReference, columnIndex, currentIsOk ? HtmlParser.PassCssClass : HtmlParser.FailCssClass);

                        tableChanges.Add(cellChange);

                        allRight &= currentIsOk;
                    }
                    catch (ConversionException ex)
                    {
                        tableChanges.Add(new ChangeCellCssClass(currentRow.RowReference, columnIndex, HtmlParser.ErrorCssClass));

                        tableChanges.Add(new AddCellExpandableInfo(currentRow.RowReference, columnIndex, "Unable to parse cell", ex.ToString()));

                        allRight = false;
                    }
                }
            }

            for (int rowIndex = orderedResult.Length; rowIndex < Rows.Count; rowIndex++)
            {
                var currentRow = Rows[rowIndex];

                var cells = currentRow.Cells.Select(c => c.CleanedContent + "<br/> missing").ToReadOnlyList();

                tableChanges.Add(new AppendRowWithCells(HtmlParser.FailCssClass, cells));

                allRight = false;
            }

            for (int rowIndex = Rows.Count; rowIndex < orderedResult.Length; rowIndex++)
            {
                var resultObject = orderedResult[rowIndex];

                var cells = ColumnNames.Select(name => ReadProperty(name, resultObject, loader) + "<br/> <i>surplus</i>").ToReadOnlyList();

                tableChanges.Add(new AppendRowWithCells(HtmlParser.FailCssClass, cells));

                allRight = false;
            }

            var resultType = FunctionExecutionResult.FunctionRunResult.Success;

            if (!allRight)
                resultType = FunctionExecutionResult.FunctionRunResult.Fail;

            return new FunctionExecutionResult(resultType, tableChanges);
        }

        private string ReadProperty(string propertyName, object resultObject, ReflectionLoader loader)
        {
            object resultValue;

            if (!loader.TryReadPropery(resultObject, propertyName, out resultValue))
                return string.Format("Unable to read property {0}", propertyName);

            return resultValue.ToString();
        }

        private bool CheckInputData(List<AbstractTableChange> tableChanges)
        {
            //ToDo: fill table changes
            bool allIsOk = true;

            foreach (HtmlRow htmlRow in Rows)
            {
                Validate.Condition(
                    ColumnNames.Count == htmlRow.Cells.Count,
                    "Row {0} contain less values ({1}) than header row ({2}).", htmlRow, htmlRow.Cells.Count, ColumnNames.Count);

                Validate.Condition(
                    htmlRow.Cells.All(c => !c.IsBold),
                    "Some of cells of row '{0}' are bold. All rows except first two should have non-bold entry, because bold type means metadata, non-bold type means test value", htmlRow);
            }

            return allIsOk;
        }

        private bool CompareItems(object resultObject, string expectedResult, string propertyName, ReflectionLoader loader, List<AbstractTableChange> cellChanges)
        {
            object resultValue;

            if (!loader.TryReadPropery(resultObject, propertyName, out resultValue))
                return false;

            if (ReferenceEquals(null, resultObject))
                return string.IsNullOrEmpty(expectedResult);

            var expectedObject = ParametersConverter.ConvertParameter(expectedResult, resultObject.GetType(), loader);

            return resultObject.Equals(expectedObject);
        }
    }
}
