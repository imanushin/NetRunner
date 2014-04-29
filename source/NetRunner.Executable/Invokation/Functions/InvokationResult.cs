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
        public InvokationResult(object result, Exception exception)
            : this(result, exception, ReadOnlyList<AbstractTableChange>.Empty)
        {
        }

        public InvokationResult([CanBeNull]object result, [CanBeNull] Exception exception, [NotNull] IReadOnlyCollection<AbstractTableChange> changes)
        {
            Validate.ArgumentIsNotNull(changes, "changes");

            Changes = changes.ToReadOnlyList();
            Result = result;
            Exception = exception;
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

        public ReadOnlyList<AbstractTableChange> Changes
        {
            get;
            private set;
        }
    }
}
