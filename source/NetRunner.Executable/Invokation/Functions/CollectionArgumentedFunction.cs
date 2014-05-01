using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

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
            Rows = rows.ToReadOnlyList();
            FunctionReference = functionToExecute;
            CleanedColumnNames = columnNames.Select(c => c.Replace(" ", string.Empty)).ToReadOnlyList();
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

        public ReadOnlyList<string> CleanedColumnNames
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
            yield return CleanedColumnNames;
            yield return Rows;
            yield return FunctionReference;
        }

        public override FunctionExecutionResult Invoke()
        {
            var resultType = FunctionReference.ResultType;

            if (typeof(IEnumerable).IsAssignableFrom(resultType))
            {
                return InvokeCollection();
            }

            if (typeof(BaseTableArgument).IsAssignableFrom(resultType))
            {
                return InvokeTable();
            }

            throw new InvalidOperationException(string.Format("Result type {0} does not supported", resultType));
        }

        private FunctionExecutionResult InvokeTable()
        {
            var changes = new List<AbstractTableChange>();

            bool exceptionsOccurred = false;
            bool allIsOk = true;

            var result = InvokeFunction(FunctionReference, Function);

            if (result.Exception != null)
            {
                var errorChange = new AddExceptionLine("Function execution failed with error", result.Exception, Function.RowReference);

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, result.TableChanges.Concat(errorChange));
            }

            var tableResult = result.Result as BaseTableArgument;

            var functionExecutionResult = CheckTableFunctionResult(result, tableResult, changes);

            if (functionExecutionResult != null)
                return functionExecutionResult;

            foreach (var row in Rows)
            {
                var functionToExecute = ReflectionLoader.Instance.FindFunction(CleanedColumnNames, tableResult);

                if (functionToExecute == null)
                {
                    changes.Add(new ExecutionFailedMessage(
                        row.RowReference,
                        string.Format("Unable to find function with these parameters: {0}", CleanedColumnNames),
                        "Unable to find function"));

                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.FailCssClass));

                    allIsOk = false;

                    continue;
                }

                var rowResult = InvokeFunction(
                    functionToExecute, 
                    row.Cells.Select(c => c.CleanedContent).ToReadOnlyList());

                changes.AddRange(rowResult.TableChanges);

                if (rowResult.Exception != null)
                {
                    exceptionsOccurred = true;
                    allIsOk = false;

                    changes.Add(new AddExceptionLine( "Unable to execute function", rowResult.Exception, row.RowReference));
                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.ErrorCssClass));
                }

                if (Equals(false, rowResult.Result))
                {
                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.FailCssClass));

                    allIsOk = false;
                }
                else if (Equals(true, rowResult.Result))
                {
                    changes.Add(new AddRowCssClass(row.RowReference, HtmlParser.PassCssClass));
                }
            }

            var resultType = exceptionsOccurred
                ? FunctionExecutionResult.FunctionRunResult.Exception
                : (allIsOk
                    ? FunctionExecutionResult.FunctionRunResult.Success
                    : FunctionExecutionResult.FunctionRunResult.Fail);

            return new FunctionExecutionResult(resultType, changes);
        }

        [CanBeNull]
        private FunctionExecutionResult CheckTableFunctionResult(object result, BaseTableArgument tableResult, List<AbstractTableChange> changes)
        {
            if (tableResult == null)
            {
                if (ReferenceEquals(null, result))
                {
                    changes.Add(new ExecutionFailedMessage(
                        Function.RowReference,
                        string.Format("Unable to check table: function {0} return null", Function.FunctionName),
                        "Unable to build table"));
                }
                else
                {
                    changes.Add(new ExecutionFailedMessage(
                        Function.RowReference,
                        string.Format("Unable to check table: function {0} return object {1} instead of {2}", Function.FunctionName, result.GetType(), typeof(BaseTableArgument)),
                        "Unable to build table"));
                }

                changes.Add(new AddRowCssClass(Function.RowReference, HtmlParser.FailCssClass));

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Fail, changes);
            }

            return null;
        }

        private FunctionExecutionResult InvokeCollection()
        {
            var result = InvokeFunction(FunctionReference, Function);
            var collectionResult = (IEnumerable)result.Result;

            if (result.Exception != null)
            {
                var errorChange = new AddExceptionLine("Function execution failed with error", result.Exception, Function.RowReference);

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, new[] { errorChange });
            }

            collectionResult = collectionResult ?? new object[0];

            var orderedResult = collectionResult.Cast<object>().ToArray();

            var allRight = true;

            var tableChanges = new List<AbstractTableChange>();

            CheckInputData();

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

                        tableChanges.Add(new AddCellExpandableInfo(currentRow.Cells[columnIndex], "Unable to parse cell", ex.ToString()));

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

            var resultType = FunctionExecutionResult.FunctionRunResult.Success;

            if (!allRight)
                resultType = FunctionExecutionResult.FunctionRunResult.Fail;

            return new FunctionExecutionResult(resultType, tableChanges);
        }

        private string ReadProperty(string propertyName, object resultObject)
        {
            object resultValue;
            Type propertyType;

            if (!ReflectionLoader.Instance.TryReadPropery(resultObject, propertyName, out propertyType, out resultValue))
                return string.Format("Unable to read property {0}", propertyName);

            return (resultValue ?? string.Empty).ToString();
        }

        private bool CheckInputData()
        {
            //ToDo: fill table changes

            foreach (HtmlRow htmlRow in Rows)
            {
                Validate.Condition(
                    CleanedColumnNames.Count == htmlRow.Cells.Count,
                    "Row {0} contain less values ({1}) than header row ({2}).", htmlRow, htmlRow.Cells.Count, CleanedColumnNames.Count);

                Validate.Condition(
                    htmlRow.Cells.All(c => !c.IsBold),
                    "Some of cells of row '{0}' are bold. All rows except first two should have non-bold entry, because bold type means metadata, non-bold type means test value", htmlRow);
            }

            return true;
        }

        private bool CompareItems(object resultObject, string expectedResult, string propertyName,  out object resultValue)
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
    }
}
