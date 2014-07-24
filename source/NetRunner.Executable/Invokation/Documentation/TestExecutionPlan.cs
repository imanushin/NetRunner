using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal sealed class TestExecutionPlan : BaseReadOnlyObject
    {
        public TestExecutionPlan(IEnumerable<ParsedTable> functionsSequence)
        {
            Validate.ArgumentIsNotNull(functionsSequence, "functionsSequence");

            FunctionsSequence = functionsSequence.ToReadOnlyList();
        }

        public ReadOnlyList<ParsedTable> FunctionsSequence
        {
            get;
            private set;
        }

        public string FormatExecutionPlan()
        {
            var document = new HtmlDocument();

            var expandableDiv = document.DocumentNode.AppendChild(document.CreateElement("div"));

            expandableDiv.Attributes.Add(HtmlParser.ClassAttributeName, "collapsible closed");

            var titleNode = document.CreateElement("p");
            titleNode.InnerHtml = "Execution information";
            titleNode.Attributes.Add(HtmlParser.ClassAttributeName, "title");

            expandableDiv.AppendChild(titleNode);

            var textNode = document.CreateElement("div");

            AddTitle(textNode, "Execution plan:");

            foreach (var table in FunctionsSequence)
            {
                foreach (var function in table.TestFunction.GetInnerFunctions())
                {
                    var functionNode = textNode.AppendChild(document.CreateElement("div"));
                    textNode.AppendChild(document.CreateElement("br"));

                    functionNode.SetAttributeValue(HtmlHintManager.AttributeName, HtmlHintManager.GetHintAttributeValue(function));
                    functionNode.SetAttributeValue("style", HtmlParser.ItemSequenceStyle);

                    var argumentsString = string.Join(", ", function.Arguments.Select(t => t.ParameterType.Name + ' ' + t.Name).ToReadOnlyList());

                    functionNode.InnerHtml = string.Format("{0}({1})", function.DisplayName, argumentsString);
                }
            }

            expandableDiv.AppendChild(textNode);

            document.DocumentNode.AppendChild(document.CreateElement("br"));

            return document.DocumentNode.OuterHtml;
        }

        private static void AddTextTag(HtmlNode textNode, string tagName, string titleText)
        {
            var title = textNode.AppendChild(textNode.OwnerDocument.CreateElement(tagName));

            title.InnerHtml = titleText;
        }

        private static void AddTitle(HtmlNode textNode, string titleText)
        {
            AddTextTag(textNode, "h4", titleText);
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return FunctionsSequence;
        }
    }
}
