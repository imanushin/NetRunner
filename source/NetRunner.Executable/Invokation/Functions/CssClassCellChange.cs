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
    internal class CssClassCellChange : AbstractTableChange
    {
        private readonly string newClass;

        public CssClassCellChange(HtmlCell cell, string newClass)
        {
            Validate.ArgumentIsNotNull(cell, "cell");
            Validate.ArgumentStringIsMeanful(newClass, "newClass");

            this.Cell = cell;
            this.newClass = newClass;
        }

        public HtmlCell Cell
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return newClass;
            yield return Cell;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            var htmlCell = Cell.FindMyself(table);

            var attribute = htmlCell.Attributes.AttributesWithName(HtmlParser.ClassAttributeName).FirstOrDefault();

            if (attribute == null)
            {
                attribute = htmlCell.OwnerDocument.CreateAttribute(HtmlParser.ClassAttributeName);

                htmlCell.Attributes.Append(attribute);
            }

            attribute.Value = newClass;
        }
    }
}
