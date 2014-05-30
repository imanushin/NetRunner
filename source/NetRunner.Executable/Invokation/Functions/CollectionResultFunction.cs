using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class CollectionResultFunction : BaseComplexArgumentedFunction
    {
        public CollectionResultFunction(
            HtmlRow columnsRow,
            IEnumerable<HtmlRow> rows,
            FunctionHeader function,
            TestFunctionReference functionToExecute) :
            base(columnsRow, rows, function, functionToExecute)
        {
        }

        private static TableChangeCollection CompareItems(IsolatedReference<object> resultObject, HtmlCell expectedResult, string propertyName)
        {
            TypeReference propertyType;

            GeneralIsolatedReference resultValue;

            var resultIsOkChange = new CssClassCellChange(expectedResult, HtmlParser.PassCssClass);

            if (!ReflectionLoader.TryReadPropery(resultObject, propertyName, out propertyType, out resultValue))
            {
                return new TableChangeCollection(false, false, new ShowActualValueCellChange(expectedResult, resultObject));
            }

            if (ReferenceEquals(null, resultValue))
            {
                return new TableChangeCollection(true, false, resultIsOkChange);
            }

            Validate.IsNotNull(propertyType, "Internal error: type of property '{0}' is undefined", propertyName);

            string errorHeader = string.Format("Unable to convert value to type '{0}'", propertyType.Name);

            var expectedObject = ParametersConverter.ConvertParameter(expectedResult, propertyType, errorHeader);

            if (!expectedObject.Changes.AllWasOk)
            {
                return expectedObject.Changes;
            }

            var conversionSucceeded = resultValue.Equals(expectedObject.Result);
            var cellChange = conversionSucceeded
                 ? resultIsOkChange
                 : new ShowActualValueCellChange(expectedResult, resultValue);

            return new TableChangeCollection(conversionSucceeded, false, expectedObject.Changes.Changes.Concat(cellChange));
        }


        protected override FunctionExecutionResult ProcessResult(IsolatedReference<object> mainFunctionResult)
        {
            var collectionResult = mainFunctionResult.As<IEnumerable>();

            collectionResult = collectionResult.IsNull ?
                ReflectionLoader.CreateOnTestDomain((IEnumerable)ReadOnlyList<object>.Empty) :
                collectionResult;

            var orderedResult = collectionResult.ToArray();

            var allRight = true;
            var exceptionOccurred = false;

            var tableChanges = new List<AbstractTableChange>();

            for (int rowIndex = 0; rowIndex < orderedResult.Length && rowIndex < Rows.Count; rowIndex++)
            {
                var resultObject = orderedResult[rowIndex];

                var currentRow = Rows[rowIndex];

                for (int columnIndex = 0; columnIndex < CleanedColumnNames.Count; columnIndex++)
                {
                    var expectedResult = currentRow.Cells[columnIndex];

                    var changes = CompareItems(resultObject, expectedResult, CleanedColumnNames[columnIndex]);

                    tableChanges.AddRange(changes.Changes);

                    allRight &= changes.AllWasOk;
                    exceptionOccurred |= changes.WereExceptions;
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

        private static string ReadProperty(string propertyName, IsolatedReference<object> resultObject)
        {
            GeneralIsolatedReference resultValue;
            TypeReference propertyType;

            if (!ReflectionLoader.TryReadPropery(resultObject, propertyName, out propertyType, out resultValue))
                return string.Format("Unable to read property '{0}'", propertyName);

            return (resultValue.IsNull ? string.Empty : resultValue.ToString()).ToString();
        }
    }
}
