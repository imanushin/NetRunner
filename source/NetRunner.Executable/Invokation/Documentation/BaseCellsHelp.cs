using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal abstract class BaseCellsHelp : AbstractTableChange
    {
        private readonly ReadOnlyList<HtmlCell> cells;
        private readonly string helpIdentity;

        protected BaseCellsHelp(ReadOnlyList<HtmlCell> functionCells, string helpIdentity)
        {
            Validate.CollectionArgumentHasElements(functionCells, "functionCells");
            Validate.ArgumentIsNotNull(helpIdentity, "helpIdentity");

            this.cells = functionCells.ToReadOnlyList();
            this.helpIdentity = helpIdentity;
        }

        public sealed override void PatchHtmlTable(HtmlNode table)
        {
            if (string.IsNullOrEmpty(helpIdentity))
            {
                return;
            }

            foreach (HtmlCell htmlCell in cells)
            {
                var targetCell = htmlCell.FindMyself(table);

                targetCell.SetAttributeValue(HtmlHintManager.AttributeName, helpIdentity);
            }
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return cells;
            yield return helpIdentity;
        }
    }
}
