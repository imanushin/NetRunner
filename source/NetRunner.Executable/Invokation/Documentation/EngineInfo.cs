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
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal static class EngineInfo
    {
        private const string howToAddHelpLinkFormat = "Help does not available for type <b>{0}</b>. See <a href=\"https://github.com/imanushin/NetRunner/wiki/Help-and-hints\">this tutorial</a> about the NetRunner help configuring.";
        private const string dialogContent = @"<div style=""max-width: 800px; max-height: 90%; position: relative; overflow: hidden; margin: 100px auto; background-color: #fff; border: 1px solid #000; padding: 15px;"">    
	<div style=""max-height: calc(100% - 300px);position: relative; overflow: auto;padding: 5px;"">
        <div id=""helpDialogContent"" style=""text-align: start;""  />
    </div>   

    <a href='#' onclick='closeHelpDialog()' style=""bottom: 0;position:relative"">close</a>
</div>";
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

            // innerHtml.Append("--------------");
            innerHtml.Append("<br/>");

            var rawHelp = DocumentationStore.GetRaw(function);

            if (!string.IsNullOrWhiteSpace(rawHelp))
            {
                innerHtml.Append(ReplaceTags(rawHelp));
            }

            if (function.AvailableFunctionNames.Count > 1)
            {
                innerHtml.Append("Available names:<br/>");

                function.AvailableFunctionNames.ForEach(name => innerHtml.AppendFormat("<i>{0}</i><br/>", ReplaceTags(name)));
            }

            innerHtml.Append(function.Method.SystemName);

            innerHtml.AppendFormat("({0})", string.Join(", ", function.Arguments.Select(a =>
                (a.IsOut ? "out " : string.Empty) + a.ParameterType.Name + " " + a.Name)));

            functionNode.InnerHtml = innerHtml.ToString();
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
