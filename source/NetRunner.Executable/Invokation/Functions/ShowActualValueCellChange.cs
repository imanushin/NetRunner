using HtmlAgilityPack;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class ShowActualValueCellChange : CssClassCellChange
    {
        private readonly object actualValue;

        public ShowActualValueCellChange(HtmlRowReference row, int column, object actualValue)
            : base(row, column, HtmlParser.FailCssClass)
        {
            this.actualValue = actualValue;
        }

        protected override void PatchCell(HtmlNode htmlCell)
        {
            base.PatchCell(htmlCell);
            var expect = htmlCell.InnerText;
            htmlCell.InnerHtml = string.Empty;
            var expectBlock = htmlCell.OwnerDocument.CreateElement("i");
            expectBlock.Attributes.Add("class","code");
            expectBlock.InnerHtml = "expect: ";

            var actualBlock = expectBlock.Clone();
            actualBlock.InnerHtml = "actual: ";

            htmlCell.AppendChild(expectBlock);
            htmlCell.InnerHtml += expect + "</br>";
            htmlCell.AppendChild(actualBlock);
            htmlCell.InnerHtml += actualValue;
        }
    }
}