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
        private readonly HtmlRowReference rowReference;
        private readonly string targetCssClass;

        public AddRowCssClass(HtmlRowReference rowReference, string targetCssClass)
        {
            Validate.ArgumentStringIsMeanful(targetCssClass, "targetCssClass");
            Validate.ArgumentIsNotNull(rowReference, "rowReference");

            this.rowReference = rowReference;
            this.targetCssClass = targetCssClass;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return rowReference;
            yield return targetCssClass;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var targetRow = rowReference.GetRow(table);

            var document = table.OwnerDocument;

            //Highlight multiple cells, because FitNesse is unable to navigate of tr items
            foreach (HtmlNode cellNode in targetRow.SelectNodesWithName(HtmlParser.TableCellNodeName))
            {
                var oldAttributes = cellNode.Attributes.AttributesWithName(HtmlParser.ClassAttributeName).FirstOrDefault();

                if (oldAttributes == null)
                {
                    var newAttribute = document.CreateAttribute(HtmlParser.ClassAttributeName, targetCssClass);

                    cellNode.Attributes.Add(newAttribute);
                }
                else
                {
                    var oldClasses = oldAttributes.Value.Split(new[]
                    {
                        ' '
                    }, StringSplitOptions.RemoveEmptyEntries).Concat(new[]{targetCssClass}).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

                    oldAttributes.Value = string.Join(" ", oldClasses);
                }
            }
        }
    }
}