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
        public InvokationResult([CanBeNull] object result, [NotNull]TableChangeCollection changes)
        {
            Validate.ArgumentIsNotNull(changes, "changes");

            Result = result;
            Changes = changes;
        }

        public InvokationResult([CanBeNull]object result)
            : this(result, TableChangeCollection.AllIsOkNoChanges)
        {
        }

        public TableChangeCollection Changes
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
    }
}
