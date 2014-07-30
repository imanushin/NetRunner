using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Remoting;
using NetRunner.Executable.RawData;
using NetRunner.TestExecutionProxy;

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
            titleNode.InnerHtml = "Execution plan";
            titleNode.Attributes.Add(HtmlParser.ClassAttributeName, "title");

            expandableDiv.AppendChild(titleNode);

            var textNode = document.CreateElement("div");

            foreach (var table in FunctionsSequence)
            {
                foreach (var function in table.TestFunction.GetInnerFunctions())
                {
                    var functionNode = textNode.AppendChild(document.CreateElement("div"));
                    textNode.AppendChild(document.CreateElement("br"));

                    functionNode.SetAttributeValue(HtmlHintManager.AttributeName, HtmlHintManager.GetHintAttributeValue(function));
                    functionNode.SetAttributeValue("style", HtmlParser.ItemSequenceStyle);

                    var argumentsString = string.Join(", ", function.Arguments.Select(t => t.GetData().ParameterType.GetData().Name + ' ' + t.GetData().Name).ToReadOnlyList());

                    functionNode.InnerHtml = string.Format("{0}({1})", function.DisplayName, argumentsString);
                }
            }

            expandableDiv.AppendChild(textNode);

            document.DocumentNode.AppendChild(document.CreateElement("br"));

            return document.DocumentNode.OuterHtml;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return FunctionsSequence;
        }
    }
}
