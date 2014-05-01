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

            this.rowReference = rowReference;
            this.targetCssClass = targetCssClass;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return rowReference;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var targetRow = rowReference.GetRow(table);

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
                    oldAttributes.Value = oldAttributes.Value + " " + targetCssClass;
                }
            }
        }
    }
}