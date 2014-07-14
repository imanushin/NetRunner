using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class ShowActualValueCellChange : CssClassCellChange
    {
        [NotNull]
        private readonly string actualValue;

        public ShowActualValueCellChange(HtmlCell cell, [NotNull] string actualValue)
            : base(cell, HtmlParser.FailCssClass)
        {
            Validate.ArgumentIsNotNull(actualValue, "actualValue");

            this.actualValue = actualValue;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            base.PatchHtmlTable(table);

            var htmlCell = Cell.FindMyself(table);

            var clonedNodes = htmlCell.ChildNodes.Select(n => n.Clone()).ToList();

            htmlCell.ChildNodes.Clear();
            
            var expect = htmlCell.InnerText;
            htmlCell.InnerHtml = string.Empty;
            var expectBlock = htmlCell.OwnerDocument.CreateElement("i");
            expectBlock.Attributes.Add("class", "code");
            expectBlock.InnerHtml = "expect: ";

            var actualBlock = expectBlock.Clone();
            actualBlock.InnerHtml = "actual: ";

            htmlCell.AppendChild(expectBlock);
            clonedNodes.ForEach(htmlCell.ChildNodes.Add);
            htmlCell.InnerHtml += expect + "</br>";
            htmlCell.AppendChild(actualBlock);
            htmlCell.InnerHtml += actualValue;
        }
    }
}