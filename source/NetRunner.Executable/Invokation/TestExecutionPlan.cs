using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation
{
    internal sealed class TestExecutionPlan : BaseReadOnlyObject
    {
        public TestExecutionPlan([StringCanBeEmpty] string textBeforeFirstTable, IEnumerable<ParsedTable> functionsSequence)
        {
            Validate.ArgumentIsNotNull(textBeforeFirstTable, "textBeforeFirstTable");
            Validate.ArgumentIsNotNull(functionsSequence, "functionsSequence");

            TextBeforeFirstTable = textBeforeFirstTable;
            FunctionsSequence = functionsSequence.ToReadOnlyList();
        }

        public string TextBeforeFirstTable
        {
            get;
            private set;
        }

        public ReadOnlyList<ParsedTable> FunctionsSequence
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return FunctionsSequence;
            yield return TextBeforeFirstTable;
        }
    }
}
