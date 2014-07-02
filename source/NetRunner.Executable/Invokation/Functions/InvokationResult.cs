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
        private InvokationResult(
            [NotNull]GeneralIsolatedReference result,
            [NotNull]TableChangeCollection changes,
            [NotNull]IEnumerable<ParameterData> outParametersResult)
        {
            Validate.ArgumentIsNotNull(result, "result");
            Validate.ArgumentIsNotNull(changes, "changes");
            Validate.ArgumentIsNotNull(outParametersResult, "outParametersResult");

            Result = result;
            Changes = changes;
            OutParametersResult = outParametersResult.ToReadOnlyList();
        }

        public InvokationResult(GeneralIsolatedReference result, [NotNull]TableChangeCollection changes)
            : this(result, changes, ReadOnlyList<ParameterData>.Empty)
        {
        }

        public InvokationResult(GeneralIsolatedReference result)
            : this(result, TableChangeCollection.AllIsOkNoChanges, ReadOnlyList<ParameterData>.Empty)
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

        public ReadOnlyList<ParameterData> OutParametersResult
        {
            get;
            private set;
        }

        public static InvokationResult CreateSuccessResult(ExecutionResult result)
        {
            return new InvokationResult(result.Result, TableChangeCollection.AllIsOkNoChanges, result.OutParameters);
        }

        public static InvokationResult CreateErrorResult(TableChangeCollection changes)
        {
            return new InvokationResult(ReflectionLoader.NullResult, changes, ReadOnlyList<ParameterData>.Empty);
        }
    }
}
