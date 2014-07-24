using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal static class EngineInfo
    {
        private const string howToAddHelpLinkFormat = "Help does not available for type <b>{0}</b>. See <a href=\"https://github.com/imanushin/NetRunner/wiki/Help-and-hints\">this tutorial</a> about the NetRunner help configuring.";
        private const string dialogContent = @"	<table style=""display: inline-block; max-width: 800px; max-height: 90%; position: relative; overflow: hidden; margin: 50px; background-color: #fff; border: 1px solid #000; padding: 1px"">
	<thead style=""border-bottom: 1px;margin:0px;padding: 1px;"">
	
	<tr style=""padding: 1px; margin:0px;white-space: normal"">
		<th style=""border: 0px;padding: 1px; margin:0px;"">
			<h3 style=""border: 0px;padding: 1px; margin:0px; text-align: center;"">Functions help</h3>
		</th>
		
		<td style=""border: 0px;margin:0px;padding: 1px;white-space: normal"">
			<a href=""#"" onclick=""closeHelpDialog()"" style=""padding: 1px; margin:0px"">
				<h3 style=""border: 0px;padding: 1px; margin:0px"">Close</h3>
			</a>
		</td>		
	</tr>
	</thead>
	<tbody>

	<tr>
		<td style=""text-align: start; border: 0px;vertical-align:top;white-space: normal"" colspan=""2"">
			<div id=""helpDialogContent""/>
	</td>
	</tr>
	</tbody>
	</table>
";
        private const string dialogStyle = @"#helpDialog {
    visibility: hidden;
    position: absolute;
    left: 0px;
    top: 0px;
    width: 100%;
    text-align: center;
    z-index: 1000;
}
";
        private const string dialogFunctions = @"function openHelpDialog(helpKey) {
    var el = document.getElementById(""helpDialog"");
    el.style.visibility = ""visible"";

    var contentPane = document.getElementById(""helpDialogContent"");
    var helpContent = document.getElementById(helpKey);

    var helpHtml = helpContent.innerHTML;
    contentPane.innerHTML = helpHtml;
}

function closeHelpDialog() {
            var el = document.getElementById(""helpDialog"");
            el.style.visibility = ""hidden"";
        }";

        

        private static readonly Dictionary<string, string> typesToCut = new Dictionary<string, string>
        {
            {"Boolean", "bool" },
            {"Int32", "int" },
            {"Double", "double" },
            {"String", "string" },
            {"Float", "float" },
            {"Void", "void" },
        };

        private static void AppendModalWindowTags(HtmlDocument document)
        {
            var modalDialog = document.DocumentNode.AppendChild(document.CreateElement("div"));

            modalDialog.SetAttributeValue("id", "helpDialog");

            modalDialog.InnerHtml = dialogContent;

            var style = document.DocumentNode.AppendChild(document.CreateElement("style"));

            style.InnerHtml = dialogStyle;

            document.DocumentNode.AppendChild(document.CreateElement("script")).InnerHtml = dialogFunctions;
        }

        public static string PrintTestEngineInformation()
        {
            var document = new HtmlDocument();

            AppendModalWindowTags(document);

            document.DocumentNode.AppendChild(document.CreateElement("br"));

            var expandableDiv = document.DocumentNode.AppendChild(document.CreateElement("div"));

            expandableDiv.Attributes.Add(HtmlParser.ClassAttributeName, "collapsible closed");

            var titleNode = document.CreateElement("p");
            titleNode.InnerHtml = "Engine information";
            titleNode.Attributes.Add(HtmlParser.ClassAttributeName, "title");

            expandableDiv.AppendChild(titleNode);

            var textNode = document.CreateElement("div");
            
            AddTitle(textNode, "Test containers:");
            TestContainersToSequence(textNode, ReflectionLoader.TestContainers);

            AddTitle(textNode, "Parsers:");
            ParsersToSequence(textNode);

            AddTitle(textNode, "Test assemblies:");
            StringsToSequence(textNode, ReflectionLoader.AssemblyPathes);

            AddTextTag(textNode, "h5", "Current executable path: " + Process.GetCurrentProcess().MainModule.FileName);

            AddTextTag(textNode, "h5", "Working folder: " + Environment.CurrentDirectory);

            textNode.AppendChild(document.CreateElement("br"));
            AddTextTag(textNode, "h3", "Available functions:");
            AllFunctionsToSequence(textNode, ReflectionLoader.TestContainers);

            expandableDiv.AppendChild(textNode);

            document.DocumentNode.AppendChild(document.CreateElement("br"));

            return document.DocumentNode.OuterHtml;
        }

        private static void ParsersToSequence(HtmlNode textNode)
        {
            var ownerDocument = textNode.OwnerDocument;

            var rootTable = textNode.AppendChild(ownerDocument.CreateElement("table"));

            rootTable.SetAttributeValue("style", "border:0px; padding:0px;  border-spacing: 0px;");

            foreach (var parser in ReflectionLoader.Parsers)
            {
                var tableRow = rootTable.AppendChild(ownerDocument.CreateElement("tr"));
                var tableCell = tableRow.AppendChild(ownerDocument.CreateElement("td"));
                var parserElement = tableCell.AppendChild(ownerDocument.CreateElement("p"));

                tableRow.SetAttributeValue("style", "border:0px; padding:0px;border-spacing: 0px;");
                tableCell.SetAttributeValue("style", "border:0px; padding:0px;border-spacing: 0px;");
                parserElement.SetAttributeValue("style", "display: inline-block; border:0px; padding:0px;border-spacing: 0px;margin: 3px");

                var parserType = parser.GetType();
                var text = string.Format("{0}; priority: {1}", parserType.Name, parser.ExecuteProperty<int>("Priority"));
                
                parserElement.InnerHtml = text;

                var parserHint = DocumentationHtmlHelpers.GetHintAttributeValue(parserType);

                if (!string.IsNullOrWhiteSpace(parserHint))
                {
                    parserElement.SetAttributeValue(DocumentationHtmlHelpers.AttributeName, parserHint);
                }
            }
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

        private static string GetHtmlIdentity(this TypeReference type)
        {
            return type.Identity.Replace('.', '_');
        }

        private static void TestContainersToSequence(HtmlNode textNode, ReadOnlyList<LazyIsolatedReference<BaseTestContainer>> inputContainers)
        {
            var ownerDocument = textNode.OwnerDocument;

            foreach (var testContainer in inputContainers)
            {
                var testContainerType = testContainer.Type;

                var helpDivContent = GetTypeDocumentation(testContainerType);

                var helpElement = textNode.AppendChild(ownerDocument.CreateElement("div"));

                var key = string.Format("type_{0}", testContainerType.GetHtmlIdentity());

                helpElement.SetAttributeValue("id", key);
                helpElement.SetAttributeValue("style", "display: none;");

                var containerParagraph = helpElement.AppendChild(ownerDocument.CreateElement("p"));

                containerParagraph.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = helpDivContent;

                var methods = ReflectionLoader.GetMethodFor(testContainer);

                methods.ForEach(m => AppendMethod(m, containerParagraph));
                
                textNode.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = testContainerType.Name;
            }
        }

        private static string GetTypeDocumentation(TypeReference testContainerType)
        {
            var rawDocumentation = DocumentationStore.GetFor(testContainerType);

            return string.IsNullOrWhiteSpace(rawDocumentation)
                ? string.Format(howToAddHelpLinkFormat, testContainerType.Name)
                : rawDocumentation;
        }

        private static void AllFunctionsToSequence(HtmlNode textNode, ReadOnlyList<LazyIsolatedReference<BaseTestContainer>> inputContainers)
        {
            var ownerDocument = textNode.OwnerDocument;

            foreach (var testContainer in inputContainers)
            {
                var testContainerType = testContainer.Type;

                textNode.AppendChild(ownerDocument.CreateElement("h5")).InnerHtml = string.Format("Container {0}:", testContainerType.Name);
                textNode.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = GetTypeDocumentation(testContainerType);

                var containerParagraph = textNode.AppendChild(ownerDocument.CreateElement("p"));
                
                var methods = ReflectionLoader.GetMethodFor(testContainer);

                methods.ForEach(m => AppendMethod(m, containerParagraph));

                textNode.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = testContainerType.Name;
            }
        }

        private static void AppendMethod(TestFunctionReference function, HtmlNode container)
        {
            var document = container.OwnerDocument;

            var functionNode = container.AppendChild(document.CreateElement("p"));

            var innerHtml = new StringBuilder();

            innerHtml.Append("---");
            innerHtml.Append("<br/>");

            var rawHelp = DocumentationStore.GetFor(function);

            if (!string.IsNullOrWhiteSpace(rawHelp))
            {
                innerHtml.AppendFormat("<p><i>{0}</i>  </p>", rawHelp);
            }

            string systemName = function.Method.SystemName;

            if (function.AvailableFunctionNames.Any() &&
                (function.AvailableFunctionNames.Count > 1 ||
                !string.Equals(function.AvailableFunctionNames.First(), systemName, StringComparison.OrdinalIgnoreCase)))
            {
                var names = string.Join(", ", function.AvailableFunctionNames
                    .Select(name => string.Format("<i>{0}</i>", HtmlParser.ReplaceUnknownTags(name))));

                innerHtml.AppendFormat("<p>Available names: {0}</p>", names);
            }
            
            innerHtml.AppendFormat(
                "<b>{0} {1}({2})</b>", 
                CutType(function.ResultType),
                systemName, 
                string.Join(", ", function.Arguments.Select(GetFormatterParameter)));

            functionNode.InnerHtml = innerHtml.ToString();
        }

        private static string GetFormatterParameter(ParameterInfoReference parameter)
        {
            var hint = DocumentationHtmlHelpers.GetHintAttributeValue(parameter);

            return string.Format(
                "<b {0}=\"{1}\">{2}{3} {4}</b>",
                DocumentationHtmlHelpers.AttributeName,
                hint,
                parameter.IsOut ? "out " : string.Empty,
                CutType(parameter.ParameterType),
                parameter.Name
                );
        }

        [NotNull]
        private static string CutType(TypeReference type)
        {
            var name = type.Name;

            string result;

            if (typesToCut.TryGetValue(name, out result))
            {
                return result;
            }

            return name;
        }

        private static void StringsToSequence(HtmlNode textNode, IEnumerable<string> inputStrings)
        {
            var ownerDocument = textNode.OwnerDocument;

            foreach (string testContainerName in inputStrings)
            {
                textNode.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = testContainerName;
            }
        }

    }
}
