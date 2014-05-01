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
    internal sealed class AppendRowWithCells : AbstractTableChange
    {
        private readonly string cellClass;
        private readonly ReadOnlyList<string> cellHtmlDatas;

        public AppendRowWithCells(string cellClass, IReadOnlyCollection<string> cellHtmlDatas)
        {
            Validate.ArgumentStringIsMeanful(cellClass, "cellClass");
            Validate.CollectionArgumentHasElements(cellHtmlDatas, "cellHtmlDatas");

            this.cellClass = cellClass;
            this.cellHtmlDatas = cellHtmlDatas.ToReadOnlyList();
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return cellClass;
            yield return cellHtmlDatas;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var document = table.OwnerDocument;

            var newRow = document.CreateElement(HtmlParser.TableRowNodeName);

            foreach (string cellData in cellHtmlDatas)
            {
                var newCell = document.CreateElement(HtmlParser.TableCellNodeName);

                newCell.Attributes.Append(document.CreateAttribute(HtmlParser.ClassAttributeName, cellClass));
                newCell.InnerHtml = cellData;

                newRow.ChildNodes.Append(newCell);
            }

            table.ChildNodes.Append(newRow);
        }
    }
}
