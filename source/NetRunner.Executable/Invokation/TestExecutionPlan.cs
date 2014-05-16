using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation
{
    internal sealed class TestExecutionPlan : BaseReadOnlyObject
    {
        public TestExecutionPlan([StringCanBeEmpty] string textBeforeFirstTable, IEnumerable<ParsedTable> functionsSequence)
        {
            Validate.ArgumentIsNotNull(textBeforeFirstTable, "textBeforeFirstTable");
            Validate.ArgumentIsNotNull(functionsSequence, "functionsSequence");

            TextBeforeFirstTable = textBeforeFirstTable;
            FunctionsSequence = functionsSequence.ToReadOnlyList();
        }

        public string TextBeforeFirstTable
        {
            get;
            private set;
        }

        public ReadOnlyList<ParsedTable> FunctionsSequence
        {
            get;
            private set;
        }

        public string FormatExecutionPlan()
        {
            var document = new HtmlDocument();

            document.DocumentNode.AppendChild(document.CreateElement("br"));

            var expandableDiv = document.DocumentNode.AppendChild(document.CreateElement("div"));

            expandableDiv.Attributes.Add(HtmlParser.ClassAttributeName, "collapsible closed");

            var titleNode = document.CreateElement("p");
            titleNode.InnerHtml = "Test execution plan";
            titleNode.Attributes.Add(HtmlParser.ClassAttributeName, "title");

            expandableDiv.AppendChild(titleNode);

            var textNode = document.CreateElement("div");

            foreach (var table in FunctionsSequence)
            {
                foreach (var function in table.TestFunction.GetInnerFunctions())
                {
                    var functionNode = textNode.AppendChild(document.CreateElement("p"));

                    var argumentsString = string.Join(", ", function.ArgumentTypes.Select(t => t.ParameterType.Name + ' ' + t.Name).ToReadOnlyList());

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
            yield return TextBeforeFirstTable;
        }
    }
}
