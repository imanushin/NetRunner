using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class ProblematicFunction : AbstractTestFunction
    {
        private readonly ReadOnlyList<AbstractTableChange> tableChanges;
        private readonly IReadOnlyCollection<HtmlRow> rows;

        public ProblematicFunction(IReadOnlyCollection<AbstractTableChange> tableChanges, IReadOnlyCollection<HtmlRow> rows)
        {
            Validate.CollectionArgumentHasElements(tableChanges, "tableChanges");
            Validate.CollectionArgumentHasElements(rows, "rows");

            var rowsWithErrorsMarks = rows.Select(r => new AddRowCssClass(r.RowReference, HtmlParser.ErrorCssClass)).ToReadOnlyList();

            this.tableChanges = tableChanges.Concat(rowsWithErrorsMarks).ToReadOnlyList();
            this.rows = rows;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return tableChanges;
            yield return rows;
        }

        public override FunctionExecutionResult Invoke()
        {
            return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, tableChanges);
        }
    }
}
