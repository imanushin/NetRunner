using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class ChangeCellColor : AbstractCellChange
    {
        public enum Color
        {
            Red,
            Green,
            Yellow
        }

        private readonly Color color;

        private ChangeCellColor(int row, int column, Color color)
            : base(row, column)
        {
            this.color = color;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return color;

            foreach (var innerObject in base.GetInnerObjects())
            {
                yield return innerObject;
            }
        }

        protected override void PatchCell(HtmlNode htmlCell)
        {
            /**/
        }
    }
}
