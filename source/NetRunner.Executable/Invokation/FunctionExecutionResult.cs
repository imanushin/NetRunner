using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    [ImmutableObject(true)]
    internal sealed class FunctionExecutionResult : BaseReadOnlyObject
    {
        public FunctionExecutionResult(FunctionRunResult resultType, IEnumerable<AbstractTableChange> tableChanges)
        {
            Validate.ArgumentIsNotNull(tableChanges, "tableChanges");
            Validate.ArgumentEnumerationValueIsDefined(resultType, "resultType");

            ResultType = resultType;
            TableChanges = tableChanges.ToReadOnlyList();
        }

        public FunctionRunResult ResultType
        {
            get;
            private set;
        }

        public ReadOnlyList<AbstractTableChange> TableChanges
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return ResultType;
            yield return TableChanges;
        }

        protected override string GetString()
        {
            return string.Format("Type: {0}; Table changes: {1}", ResultType, TableChanges);
        }

        public enum FunctionRunResult
        {
            Fail = 0,
            Success,
            Ignore,
            Exception
        }
    }
}
