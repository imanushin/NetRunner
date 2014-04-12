using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal class CssClassCellChange : AbstractCellChange
    {
        private readonly string newClass;

        public CssClassCellChange(HtmlRowReference row, int column, string newClass)
            : base(row, column)
        {
            this.newClass = newClass;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return newClass;

            foreach (var innerObject in base.GetInnerObjects())
            {
                yield return innerObject;
            }
        }

        protected override void PatchCell(HtmlNode htmlCell)
        {
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
