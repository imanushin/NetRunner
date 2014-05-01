﻿using HtmlAgilityPack;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class ShowActualValueCellChange : CssClassCellChange
    {
        private readonly object actualValue;

        public ShowActualValueCellChange(HtmlCell cell, object actualValue)
            : base(cell, HtmlParser.FailCssClass)
        {
            this.actualValue = actualValue;
        }

        public override void PatchHtmlTable(HtmlNode table)
        {
            base.PatchHtmlTable(table);

            var htmlCell = Cell.FindMyself(table);

            var expect = htmlCell.InnerText;
            htmlCell.InnerHtml = string.Empty;
            var expectBlock = htmlCell.OwnerDocument.CreateElement("i");
            expectBlock.Attributes.Add("class", "code");
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