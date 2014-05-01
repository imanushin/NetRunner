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
        public InvokationResult([CanBeNull] object result, [CanBeNull] Exception exception, [NotNull]IEnumerable<AbstractTableChange> tableChanges)
        {
            Validate.ArgumentIsNotNull(tableChanges, "tableChanges");

            Result = result;
            Exception = exception;
            TableChanges = tableChanges.ToReadOnlyList();
        }

        public InvokationResult([CanBeNull]object result, [CanBeNull] Exception exception)
            : this(result, exception, ReadOnlyList<AbstractTableChange>.Empty)
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

        [CanBeNull]
        public Exception Exception
        {
            get;
            private set;
        }
    }
}
