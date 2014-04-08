using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal abstract class AbstractCellChange : AbstractTableChange
    {
        private readonly HtmlRowReference row;
        private readonly int column;

        protected AbstractCellChange(HtmlRowReference row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public override void PatchHtmlTable(HtmlNode node)
        {
            var rowNode = row.GetRow(node);

            var allColumns = rowNode.ChildNodes.Where(n => string.Equals(n.Name, HtmlParser.TableCellNodeName, StringComparison.OrdinalIgnoreCase)).ToReadOnlyList();

            var targetCell = allColumns.Skip(column).FirstOrDefault();

            Validate.IsNotNull(targetCell, "Unable to find column with position {0}. Columns count: {1}", column, allColumns.Count);

            PatchCell(targetCell);
        }

        protected abstract void PatchCell(HtmlNode htmlCell);

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return row;
            yield return column;
        }
    }
}
