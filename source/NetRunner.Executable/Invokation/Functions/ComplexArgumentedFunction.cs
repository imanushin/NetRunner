using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Functions
{
    internal abstract class BaseComplexArgumentedFunction : AbstractTestFunction
    {
        private readonly TestFunctionReference functionReference;

        protected BaseComplexArgumentedFunction(HtmlRow columnsRow, IEnumerable<HtmlRow> rows, FunctionHeader function, TestFunctionReference functionToExecute)
        {
            Validate.ArgumentIsNotNull(function, "function");
            Validate.ArgumentIsNotNull(rows, "rows");
            Validate.ArgumentIsNotNull(functionToExecute, "functionToExecute");
            Validate.ArgumentIsNotNull(columnsRow, "columnsRow");

            Function = function;
            Rows = rows.ToReadOnlyList();
            functionReference = functionToExecute;
            ColumnsRow = columnsRow;
            CleanedColumnNames = columnsRow.Cells.Select(c => c.CleanedContent).Select(TestFunctionReference.CleanFunctionName).ToReadOnlyList();
        }

        protected HtmlRow ColumnsRow
        {
            get;
            private set;
        }

        protected ReadOnlyList<string> CleanedColumnNames
        {
            get;
            private set;
        }

        protected FunctionHeader Function
        {
            get;
            private set;
        }

        protected ReadOnlyList<HtmlRow> Rows
        {
            get;
            private set;
        }

        protected sealed override IEnumerable<object> GetInnerObjects()
        {
            yield return Function;
            yield return ColumnsRow;
            yield return Rows;
            yield return functionReference;
        }

        public sealed override FunctionExecutionResult Invoke()
        {
            var inputDataProblems = CheckInputData();

            if (inputDataProblems != null)
            {
                return inputDataProblems;
            }

            var result = InvokeFunction(functionReference, Function);

            if (result.Changes.WereExceptions)
            {
                var rowCss = new AddRowCssClass(Function.RowReference, HtmlParser.ErrorCssClass);

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, result.Changes.Changes.Concat(rowCss));
            }

            Validate.IsNotNull(result.Result, "Function result should not be null");

            Validate.Condition(!result.Result.IsNull, "Null result is not supported yet");

            var actualResult = ProcessResult(result.Result);

            return new FunctionExecutionResult(actualResult.ResultType, actualResult.TableChanges.Concat(result.Changes.Changes));
        }

        protected abstract FunctionExecutionResult ProcessResult([NotNull] GeneralIsolatedReference mainFunctionResult);

        [CanBeNull]
        private FunctionExecutionResult CheckInputData()
        {
            var errors = new List<AbstractTableChange>();

            if (!ColumnsRow.Cells.Any())
            {
                errors.Add(new ExecutionFailedMessage(
                    ColumnsRow.RowReference,
                    string.Format("Wrong header: second row should have at least one column. They are used to retrieve result property names"),
                    "There are no any values in the second row in function {0}. Please add header row to match table values and function result/input",
                    Function.FunctionName));

                errors.Add(new AddRowCssClass(ColumnsRow.RowReference, HtmlParser.FailCssClass));
            }

            if (!ColumnsRow.Cells.All(c => c.IsBold))
            {
                errors.Add(new ExecutionFailedMessage(
                    ColumnsRow.RowReference,
                    string.Format("Wrong header: all elements should be bold"),
                    "All elements on second row (named 'headers') should be bold. These values are used in the function {0} to match internal field name and table column name.",
                    Function.FunctionName));

                errors.Add(new AddRowCssClass(ColumnsRow.RowReference, HtmlParser.FailCssClass));
            }

            foreach (HtmlRow htmlRow in Rows)
            {
                if (CleanedColumnNames.Count != htmlRow.Cells.Count)
                {
                    errors.Add(new ExecutionFailedMessage(
                        htmlRow.RowReference,
                        string.Format("Wrong column count: {0} expected, however {1} actual", CleanedColumnNames.Count, htmlRow.Cells.Count),
                        "Current contain different count values (count: {0}) than header row (count: {1}).",
                        htmlRow.Cells.Count,
                        CleanedColumnNames.Count));

                    errors.Add(new AddRowCssClass(htmlRow.RowReference, HtmlParser.FailCssClass));
                }

                var boldCells = htmlRow.Cells.Where(c => c.IsBold).Select(c => "'" + c.CleanedContent + "'").ToReadOnlyList();

                if (boldCells.Any())
                {
                    errors.Add(new ExecutionFailedMessage(
                        htmlRow.RowReference,
                        string.Format("{0} cells are bold. All cells should be non-bold", boldCells.Count),
                        "All rows except first two should have non-bold entry, because bold type means metadata, non-bold type means test value. Current bold contains bold cells: {0}",
                        boldCells));

                    errors.Add(new AddRowCssClass(htmlRow.RowReference, HtmlParser.FailCssClass));
                }
            }

            if (errors.Any())
            {
                errors.Add(new ExecutionFailedMessage(
                    Function.RowReference,
                    "Row invokation was skipped because of parser errors",
                    "Row invokation was skipped because of parser errors"));

                errors.Add(new AddRowCssClass(Function.RowReference, HtmlParser.FailCssClass));

                return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Fail, errors);
            }

            return null;
        }

        protected FunctionExecutionResult FormatResult(bool exceptionsOccurred, bool allIsOk, IEnumerable<AbstractTableChange> changes)
        {
            var resultType = exceptionsOccurred
                ? FunctionExecutionResult.FunctionRunResult.Exception
                : (allIsOk
                    ? FunctionExecutionResult.FunctionRunResult.Success
                    : FunctionExecutionResult.FunctionRunResult.Fail);

            var innerChanges = changes.ToList();

            if (exceptionsOccurred)
            {
                innerChanges.Add(new AddRowCssClass(Function.RowReference, HtmlParser.ErrorCssClass));
            }
            else if (allIsOk)
            {
                innerChanges.Add(new AddRowCssClass(Function.RowReference, HtmlParser.PassCssClass));
            }
            else
            {
                innerChanges.Add(new AddRowCssClass(Function.RowReference, HtmlParser.FailCssClass));
            }

            return new FunctionExecutionResult(resultType, innerChanges);
        }


        protected sealed override string GetString()
        {
            return string.Format("Type: {0}, FunctionReference: {1}, Function: {2}, CleanedColumnNames: {3}, Rows: {4}",
                GetType().Name,
                functionReference,
                Function,
                CleanedColumnNames,
                Rows);
        }

        [CanBeNull]
        public static BaseComplexArgumentedFunction GetFunction(HtmlRow columnNames, IEnumerable<HtmlRow> cells, FunctionHeader header, TestFunctionReference functionToExecute)
        {
            var resultType = functionToExecute.ResultType;

            if (ReflectionLoader.EnumerableType.IsAssignableFrom(resultType))
            {
                return new CollectionResultFunction(columnNames, cells, header, functionToExecute);
            }

            if (ReflectionLoader.TableArgumentType.IsAssignableFrom(resultType))
            {
                return new TableResultFunction(columnNames, cells, header, functionToExecute);
            }

            return null;
            /*
            throw new InvalidOperationException(string.Format(
                "Result type {0} does not supported. Table function should inherit one of the following types: {1}, {2}",
                resultType, 
                CollectionResultFunction.BaseType, 
                TableResultFunction.BaseType));*/
        }

        public override ReadOnlyList<TestFunctionReference> GetInnerFunctions()
        {
            return new[]
            {
                functionReference
            }.ToReadOnlyList();
        }
    }
}
