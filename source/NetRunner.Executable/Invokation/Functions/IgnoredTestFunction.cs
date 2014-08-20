using System.Collections.Generic;
using System.Linq;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class IgnoredTestFunction : AbstractTestFunction
    {
        private readonly FunctionExecutionResult functionExecutionResult;

        public IgnoredTestFunction(HtmlRow row)
            : this(new[] { row })
        {
        }

        public IgnoredTestFunction(IReadOnlyCollection<HtmlRow> rows)
        {
            var tableChanges = rows.Select(r => new AddRowCssClass(r.RowReference, HtmlParser.IgnoreCssClass)).ToReadOnlyList();

            functionExecutionResult = new FunctionExecutionResult(
                FunctionExecutionResult.FunctionRunResult.Ignore,
                tableChanges);
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return functionExecutionResult;
        }

        public override FunctionExecutionResult Invoke()
        {
            return functionExecutionResult;
        }

        public override ReadOnlyList<TestFunctionReference> GetInnerFunctions()
        {
            return ReadOnlyList<TestFunctionReference>.Empty;
        }
    }
}
