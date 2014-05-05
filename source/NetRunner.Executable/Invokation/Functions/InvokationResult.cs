using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class InvokationResult
    {
        public InvokationResult([CanBeNull] object result, bool executedWithErrors, AbstractTableChange tableChange)
            : this(result, executedWithErrors, new[] { tableChange })
        {

        }

        public InvokationResult([CanBeNull] object result, bool executedWithErrors, [NotNull]IEnumerable<AbstractTableChange> tableChanges)
        {
            Validate.ArgumentIsNotNull(tableChanges, "tableChanges");

            Result = result;
            TableChanges = tableChanges.ToReadOnlyList();
            ExecutedWithErrors = executedWithErrors;
        }

        public InvokationResult([CanBeNull]object result)
            : this(result, false, ReadOnlyList<AbstractTableChange>.Empty)
        {
        }

        public ReadOnlyList<AbstractTableChange> TableChanges
        {
            get;
            private set;
        }


        [CanBeNull]
        public object Result
        {
            get;
            private set;
        }

        public bool ExecutedWithErrors
        {
            get;
            private set;
        }
    }
}
