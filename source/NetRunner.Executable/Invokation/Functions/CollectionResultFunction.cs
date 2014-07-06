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

        private static TableChangeCollection CompareItems(GeneralIsolatedReference resultObject, HtmlCell expectedResult, string propertyName)
        {
            var resultIsOkChange = new CssClassCellChange(expectedResult, HtmlParser.PassCssClass);

            var objectType = resultObject.GetType();

            var property = objectType.GetProperty(propertyName);

            if (property == null)
            {
                const string propertyNotFoundFormat = "Type '{0}' does not contain property '{1}'. Available properties: {2}";
                string header = string.Format("Property {0} was not found", propertyName);
                string info = string.Format(propertyNotFoundFormat, objectType, propertyName, string.Join(", ", objectType.GetProperties.Select(p => p.Name)));

                var propertyNotFoundChange = new AddCellExpandableInfo(expectedResult, header, info);
                var exceptionClassChange  = new CssClassCellChange(expectedResult, HtmlParser.ErrorCssClass);

                return new TableChangeCollection(false, true, propertyNotFoundChange, exceptionClassChange);
            }

            var propertyValue = property.GetValue(resultObject);

            if (propertyValue.HasError)
            {
                var changes = new AddCellExpandableException(expectedResult, propertyValue, "Unable to read '{0}'", propertyName);

                return new TableChangeCollection(false, true, changes);
            }

            var propertyType = property.PropertyType;

            string errorHeader = string.Format("Unable to convert value to type '{0}'", propertyType.Name);

            var cellInfo = new CellParsingInfo(expectedResult, propertyType);

            var expectedObject = ParametersConverter.ConvertParameter(cellInfo, errorHeader);

            if (!expectedObject.Changes.AllWasOk)
            {
                return expectedObject.Changes;
            }

            var resultValue = propertyValue.Result;

            var conversionSucceeded = resultValue.Equals(expectedObject.Result);

            var cellChange = conversionSucceeded
                 ? resultIsOkChange
                 : new ShowActualValueCellChange(expectedResult, resultValue);

            return new TableChangeCollection(conversionSucceeded, false, expectedObject.Changes.Changes.Concat(cellChange));
        }


        protected override FunctionExecutionResult ProcessResult(GeneralIsolatedReference mainFunctionResult)
        {
            var collectionResult = mainFunctionResult.AsIEnumerable();

            collectionResult = collectionResult.IsNull ?
                ReflectionLoader.CreateOnTestDomain((IEnumerable)ReadOnlyList<object>.Empty) :
                collectionResult;

            var orderedResult = collectionResult.ToArray();

            var tableChanges = new SequenceExecutionStatus();

            for (int rowIndex = 0; rowIndex < orderedResult.Length && rowIndex < Rows.Count; rowIndex++)
            {
                var resultObject = orderedResult[rowIndex];

                var currentRow = Rows[rowIndex];

                for (int columnIndex = 0; columnIndex < CleanedColumnNames.Count; columnIndex++)
                {
                    var expectedResult = currentRow.Cells[columnIndex];

                    var changes = CompareItems(resultObject, expectedResult, CleanedColumnNames[columnIndex]);

                    tableChanges.MergeWith(changes);
                }
            }

            for (int rowIndex = orderedResult.Length; rowIndex < Rows.Count; rowIndex++)
            {
                var currentRow = Rows[rowIndex];

                var cells = currentRow.Cells.Select(c => c.CleanedContent + "<br/> <i class=\"code\">missing</i>").ToReadOnlyList();

                tableChanges.Changes.Add(new AppendRowWithCells(HtmlParser.FailCssClass, cells));

                tableChanges.AllIsOk = false;
            }

            for (int rowIndex = Rows.Count; rowIndex < orderedResult.Length; rowIndex++)
            {
                var resultObject = orderedResult[rowIndex];

                var cells = CleanedColumnNames.Select(name => ReadProperty(name, resultObject) + "<br/> <i class=\"code\">surplus</i>").ToReadOnlyList();

                tableChanges.Changes.Add(new AppendRowWithCells(HtmlParser.FailCssClass, cells));

                tableChanges.AllIsOk = false;
            }

            return FormatResult(tableChanges);
        }

        private static string ReadProperty(string propertyName, GeneralIsolatedReference resultObject)
        {
            var type = resultObject.GetType();

            var property = type.GetProperty(propertyName);

            if (property == null)
            {
                return string.Format("Unable to find property '{0}'", propertyName);
            }

            var propertyReadValue = property.GetValue(resultObject);

            if (propertyReadValue.HasError)
            {
                return string.Format("Unable to read property '{0}' because of: {1}", propertyName, propertyReadValue.ExceptionToString);
            }

            var resultValue = propertyReadValue.Result;

            return (resultValue.IsNull ? string.Empty : resultValue.ToString()).ToString();
        }
    }
}
