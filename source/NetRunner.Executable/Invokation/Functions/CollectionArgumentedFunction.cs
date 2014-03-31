using System;
using System.Collections.Generic;
using NetRunner.Executable.Common;

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
            throw new NotImplementedException();
        }
    }
}
