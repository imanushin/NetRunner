using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class InvokationResult
    {
        public InvokationResult(GeneralIsolatedReference result, [NotNull]TableChangeCollection changes)
        {
            Validate.ArgumentIsNotNull(changes, "changes");

            Result = result;
            Changes = changes;
        }

        public InvokationResult(GeneralIsolatedReference result)
            : this(result, TableChangeCollection.AllIsOkNoChanges)
        {
        }

        public TableChangeCollection Changes
        {
            get;
            private set;
        }

        public GeneralIsolatedReference Result
        {
            get;
            private set;
        }
    }
}
