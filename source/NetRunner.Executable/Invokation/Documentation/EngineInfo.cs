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

	<tr style=""overflow:hidden;max-height: calc(90% - 30px)"">
		<td id=""helpDialogContent"" style=""max-height: calc(90% - 30px); text-align: start; border: 0px;vertical-align:top;white-space: normal"" colspan=""2"">
			<div style=""overflow:auto;height:calc(90% - 100px)"">
            NetRunnerTestContainer help
            <b>bold test</b><br><i>italic test</i><br>&lt;script&gt;injection&lt;/script&gt;</p><p>---<br></p><p>
            Summ function
            </p><b>int SummAndWillBe(int first, int second)</b><p></p><p>---<br></p><p>
            Very positive help
            </p><b>bool IsPositive(int value)</b><p></p><p>---<br></p><p>
            Usage: <br>
            Right: | '''try parse string ''' | 123 | <br>
            Wrong: | '''try parse string ''' | 123 |
            </p><b>bool TryParseString(string value)</b><p></p><p>---<br><b>bool PingSite(string url)</b></p><p>---<br><b>int StringLengthOfIs(string inputLine)</b></p><p>---<br><b>int RawStringLengthOfIs(string inputLine)</b></p><p>---<br><b>IEnumerable ListNumbersFromTo(int start, int finish)</b></p><p>---<br><b>bool PingSiteWithoutExceptions(string url)</b></p><p>---<br><b>IEnumerable ListFilesIn(string path)</b></p><p>---<br><b>CreateFolderArgument CreateSubfoldersIn(string baseDirectoryPath)</b></p><p>---<br><b>RemoveFolderArgument RemoveSubfoldersFrom(string baseDirectoryPath)</b></p><p>---<br><b>IEnumerable`1 ListInOutObjects(int count)</b></p><p>---<br><b>int PlusIs(int a, int b, out Int32&amp; c)</b></p><p>---<br><b>IncorrectEqualityClass GetIncorrectEqualityClass()</b></p><p>---<br><b>IncorrectToStringClass GetIncorrectToStringClass()</b></p><p>---<br><b>IEnumerable ListWrongEquality()</b></p><p>---<br><b>IEnumerable ListWrongToString()</b></p><p></p>
			</div>
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
    height: 100%;
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

            var helpKeyValues = new Dictionary<string, string>();

            AddTitle(textNode, "Test containers:");
            TestContainersToSequence(textNode, ReflectionLoader.TestContainers, helpKeyValues);

            AddTitle(textNode, "Parsers:");
            StringsToSequence(textNode, ReflectionLoader.Parsers.Select(p => string.Format("{0}; priority: {1}", p.GetTypeName(), p.ExecuteProperty<int>("Priority"))));

            AddTitle(textNode, "Test assemblies:");
            StringsToSequence(textNode, ReflectionLoader.AssemblyPathes);

            AddTextTag(textNode, "h5", "Current executable path: " + Process.GetCurrentProcess().MainModule.FileName);

            AddTextTag(textNode, "h5", "Working folder: " + Environment.CurrentDirectory);

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

        private static void TestContainersToSequence(HtmlNode textNode, ReadOnlyList<LazyIsolatedReference<BaseTestContainer>> inputStrings, Dictionary<string, string> helpKeyValues)
        {
            var ownerDocument = textNode.OwnerDocument;

            foreach (var testContainer in inputStrings)
            {
                var testContainerType = testContainer.Type;

                var rawDocumentation = DocumentationStore.GetRaw(testContainerType);

                var helpElement = textNode.AppendChild(ownerDocument.CreateElement("div"));

                var key = string.Format("type_{0}", testContainerType.FullName);

                helpElement.SetAttributeValue("id", key);
                helpElement.SetAttributeValue("style", "display: none;");

                var helpDivContent = string.IsNullOrWhiteSpace(rawDocumentation) ?
                    string.Format(howToAddHelpLinkFormat, testContainerType.Name) :
                    ReplaceTags(rawDocumentation);

                var containerParagraph = helpElement.AppendChild(ownerDocument.CreateElement("p"));

                containerParagraph.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = helpDivContent;

                var methods = ReflectionLoader.GetMethodFor(testContainer);

                methods.ForEach(m => AppendMethod(m, containerParagraph));

                helpKeyValues[key] = helpDivContent;

                var linkElement = textNode.AppendChild(ownerDocument.CreateElement("a"));
                linkElement.SetAttributeValue("href", "#");
                linkElement.SetAttributeValue("onclick", string.Format("openHelpDialog('{0}')", key));

                linkElement.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = testContainerType.Name;
            }
        }

        private static void AppendMethod(TestFunctionReference function, HtmlNode container)
        {
            var document = container.OwnerDocument;

            var functionNode = container.AppendChild(document.CreateElement("p"));

            var innerHtml = new StringBuilder();

            innerHtml.Append("---");
            innerHtml.Append("<br/>");

            var rawHelp = DocumentationStore.GetRaw(function);

            if (!string.IsNullOrWhiteSpace(rawHelp))
            {
                innerHtml.AppendFormat("<p>{0}</p>", ReplaceTags(rawHelp));
            }

            string systemName = function.Method.SystemName;

            if (function.AvailableFunctionNames.Any() &&
                (function.AvailableFunctionNames.Count > 1 ||
                !string.Equals(function.AvailableFunctionNames.First(), systemName, StringComparison.OrdinalIgnoreCase)))
            {
                var names = string.Join(", ", function.AvailableFunctionNames.Select(name => string.Format("<i>{0}</i>", ReplaceTags(name))));

                innerHtml.AppendFormat("<p>Available names: {0}</p>", names);
            }

            innerHtml.AppendFormat("<b>{0} {1}({2})</b>", CutType(function.ResultType), systemName, string.Join(", ", function.Arguments.Select(a =>
                (a.IsOut ? "out " : string.Empty) + CutType(a.ParameterType) + " " + a.Name)));

            functionNode.InnerHtml = innerHtml.ToString();
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

        private static string ReplaceTags(string rawData)
        {
            return rawData
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;")
                .Replace("&lt;p&gt;", "<p>")
                .Replace("&lt;/p&gt;", "</p>")
                .Replace("&lt;b&gt;", "<b>")
                .Replace("&lt;/b&gt;", "</b>")
                .Replace("&lt;i&gt;", "<i>")
                .Replace("&lt;/i&gt;", "</i>")
                .Replace("&lt;u&gt;", "<u>")
                .Replace("&lt;/u&gt;", "</u>")
                .Replace("&lt;br /&gt;", "<br/>")
                .Replace("&lt;br/&gt;", "<br/>");
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
