using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class CollectionArgumentedFunction : AbstractTestFunction
    {
        public CollectionArgumentedFunction(ReadOnlyList<string> columnNames, IEnumerable<ReadOnlyList<string>> rows, FunctionHeader function, TestFunctionReference functionToExecute)
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

        public ReadOnlyList<ReadOnlyList<string>> Rows
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

            var missingLines = new List<ReadOnlyList<string>>();
            var surplusLines = new List<ReadOnlyList<string>>();

            var allRight = true;

            var convertExceptions = new List<ConversionException>();

            var tableChanges = new List<AbstractTableChange>();

            for (int rowIndex = 0; rowIndex < orderedResult.Length && rowIndex < Rows.Count; rowIndex++)
            {
                var resultObject = orderedResult[rowIndex];

                for (int columnIndex = 0; columnIndex < ColumnNames.Count; columnIndex++)
                {
                    try
                    {
                        allRight &= CompareItems(resultObject, Rows[rowIndex][columnIndex], ColumnNames[rowIndex], loader, tableChanges);
                    }
                    catch (ConversionException ex)
                    {
                        convertExceptions.Add(ex);
                    }
                }
            }

            var resultType = FunctionExecutionResult.FunctionRunResult.Success;

            if (!allRight)
                resultType = FunctionExecutionResult.FunctionRunResult.Fail;

            if (convertExceptions.Any())
            {
                resultType = FunctionExecutionResult.FunctionRunResult.Exception;
                tableChanges.Add(new AddExceptionLine(convertExceptions, Function.RowReference));
            }

            return new FunctionExecutionResult(resultType, tableChanges);
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
