using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class AddRowCssClass : AbstractTableChange
    {
        private readonly int rowGlobalIndex;
        private readonly string targetCssClass;

        public AddRowCssClass(int rowGlobalIndex, string targetCssClass)
        {
            Validate.ArgumentStringIsMeanful(targetCssClass, "targetCssClass");

            this.rowGlobalIndex = rowGlobalIndex;
            this.targetCssClass = targetCssClass;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return rowGlobalIndex;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var stringIndex = rowGlobalIndex.ToString(CultureInfo.InvariantCulture);

            var targetRow = table.ChildNodes.FirstOrDefault(
                row =>
                    row.Attributes
                    .AttributesWithName(HtmlRow.GlobalAttributeIndexName)
                    .Any(a => string.Equals(a.Value, stringIndex, StringComparison.Ordinal)));

            Validate.IsNotNull(targetRow, "Unable to find row with index {0} in current table", rowGlobalIndex);

            var document = table.OwnerDocument;

            //Highlight multiple cells, because fitnesse is unable to navigate of tr items
            foreach (HtmlNode cellNode in targetRow.ChildNodes.Where(n => string.Equals(n.Name, HtmlParser.TableCellNodeName, StringComparison.OrdinalIgnoreCase)))
            {
                var oldAttributes = cellNode.Attributes.AttributesWithName(HtmlParser.ClassAttributeName).FirstOrDefault();

                if (oldAttributes == null)
                {
                    var newAttribute = document.CreateAttribute(HtmlParser.ClassAttributeName, targetCssClass);

                    cellNode.Attributes.Add(newAttribute);
                }
                else
                {
                    oldAttributes.Value = oldAttributes.Value + "," + targetCssClass;
                }
            }
        }
    }
}