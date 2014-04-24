using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.RawData
{
    internal sealed class HtmlCell : BaseReadOnlyObject
    {
        private readonly HtmlNode tableCell;

        public HtmlCell(HtmlNode tableCell)
        {
            this.tableCell = tableCell;
            Validate.ArgumentIsNotNull(tableCell, "tableCell");
        }

        public bool IsBold
        {
            get
            {
                if (tableCell.ChildNodes.Count != 1)
                    return false;

                var rootChild = tableCell.FirstChild;

                return string.Equals(rootChild.Name, "b", StringComparison.OrdinalIgnoreCase);
            }
        }

        public string CleanedContent
        {
            get
            {
                return tableCell.InnerText;
            }
        }
        
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return tableCell.OuterHtml;
        }

        protected override string GetString()
        {
            return tableCell.OuterHtml;
        }
    }
}
