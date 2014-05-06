using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Keywords;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class ProblematicFunction : AbstractTestFunction
    {
        private readonly ReadOnlyList<AbstractTableChange> tableChanges;
        private readonly IReadOnlyCollection<HtmlRow> rows;
        private readonly AbstractKeyword keyword;

        public ProblematicFunction(IReadOnlyCollection<AbstractTableChange> tableChanges, IReadOnlyCollection<HtmlRow> rows, AbstractKeyword keyword)
        {
            Validate.CollectionArgumentHasElements(tableChanges, "tableChanges");
            Validate.CollectionArgumentHasElements(rows, "rows");

            var rowsWithErrorsMarks = rows.Select(r => new AddRowCssClass(r.RowReference, HtmlParser.ErrorCssClass)).ToReadOnlyList();

            this.tableChanges = tableChanges.Concat(rowsWithErrorsMarks).ToReadOnlyList();
            this.rows = rows;
            this.keyword = keyword;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return tableChanges;
            yield return rows;
            yield return keyword;
        }

        public override FunctionExecutionResult Invoke()
        {
            return new FunctionExecutionResult(FunctionExecutionResult.FunctionRunResult.Exception, tableChanges.Concat(keyword.ParsingErrors));
        }
    }
}
