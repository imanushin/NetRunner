using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.RawData
{
    [ImmutableObject(true)]
    internal sealed class HtmlCell : BaseReadOnlyObject
    {
        private const string globalAttributeIndexName = "GlobalCellIndex";

        private readonly HtmlNode tableCell;

        private const int cellAbsenteeIndex = -1;
        private static int cellGlobalIndex = cellAbsenteeIndex + 1;

        public HtmlCell(HtmlNode tableCell)
        {
            this.tableCell = tableCell;

            Validate.ArgumentIsNotNull(tableCell, "tableCell");

            CellIndex = cellGlobalIndex++;

            Validate.ArgumentCondition(!tableCell.Attributes.Contains(globalAttributeIndexName), "tableCell", "Input cell has already had attribute {0}: {1}", globalAttributeIndexName, tableCell.OuterHtml);

            tableCell.Attributes.Append(globalAttributeIndexName, CellIndex.ToString(CultureInfo.InvariantCulture));
        }

        public int CellIndex
        {
            get;
            private set;
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
            [Pure]
            get
            {
                return tableCell.InnerText;
            }
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return CellIndex;
        }

        protected override string GetString()
        {
            return string.Format("Cell Index: {0}; html: {1}", CellIndex, HttpUtility.HtmlEncode(tableCell.OuterHtml));
        }

        [NotNull, Pure]
        public HtmlNode FindMyself([NotNull] HtmlNode node)
        {
            Validate.ArgumentIsNotNull(node, "node");

            //do find because our node is from original html, however input node is from result html text
            var result = node.Descendants()
                .FirstOrDefault(n => n.GetAttributeValue(globalAttributeIndexName, cellAbsenteeIndex) == CellIndex);

            Validate.IsNotNull(result, "Unable to cell {0} in the table {1}", this, HttpUtility.HtmlEncode(node.InnerHtml));

            return result;
        }
    }
}
