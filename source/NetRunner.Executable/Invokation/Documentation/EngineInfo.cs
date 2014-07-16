﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.RawData;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal static class EngineInfo
    {
        private const string howToAddHelpLinkFormat = "Help does not available for type <b>{0}</b>. See <a href=\"https://github.com/imanushin/NetRunner/wiki/Help-and-hints\">this tutorial</a> about the NetRunner help configuring.";

        private static void AppendModalWindowTags(HtmlDocument document)
        {
            var modalDialog = document.DocumentNode.AppendChild(document.CreateElement("div"));

            modalDialog.SetAttributeValue("id", "helpDialog");

            modalDialog.InnerHtml =
@"<div>
    <p id=""helpDialogContent"">Content you want the user to see goes here.</p>
    
    <a href='#' onclick='closeHelpDialog()'>close</a>
</div>";

            var style = document.DocumentNode.AppendChild(document.CreateElement("style"));

            style.InnerHtml =
@"#helpDialog {
    visibility: hidden;
    position: absolute;
    left: 0px;
    top: 0px;
    width: 100%;
    height: 100%;
    text-align: center;
    z-index: 1000;
}

#helpDialog div {
    max-width: 90%;
    max-height: 90%;
    min-width: 10%;
    min-height: 10%;
    overflow: hidden;
    margin: 100px auto;
    background-color: #fff;
    border: 1px solid #000;
    padding: 15px;
    text-align: center;
}";

            document.DocumentNode.AppendChild(document.CreateElement("script")).InnerHtml =
@"function openHelpDialog(helpKey) {
    var el = document.getElementById(""helpDialog"");
    el.style.visibility = ""visible"";

    var contentPane = document.getElementById(""helpDialogContent"");
    var helpContent = document.getElementById(helpKey);

    var helpHtml = helpContent.innerHTML;
    contentPane.innerHTML = helpHtml;
}";

            document.DocumentNode.AppendChild(document.CreateElement("script")).InnerHtml =
@"function closeHelpDialog() {
            var el = document.getElementById(""helpDialog"");
            el.style.visibility = ""hidden"";
        }";
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
            TestContainersToSequence(textNode, ReflectionLoader.TestContainerTypes, helpKeyValues);

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

        private static void TestContainersToSequence(HtmlNode textNode, IEnumerable<TypeReference> inputStrings, Dictionary<string, string> helpKeyValues)
        {
            var ownerDocument = textNode.OwnerDocument;

            foreach (var testContainer in inputStrings)
            {
                var rawDocumentation = DocumentationStore.GetForType(testContainer);

                var helpDivContent = string.IsNullOrWhiteSpace(rawDocumentation) ?
                    string.Format(howToAddHelpLinkFormat, testContainer.Name) :
                    ReplaceTags(rawDocumentation);

                var key = string.Format("type_{0}", testContainer.FullName);

                var helpElement = textNode.AppendChild(ownerDocument.CreateElement("div"));

                helpElement.InnerHtml = helpDivContent;
                helpElement.SetAttributeValue("id", key);
                helpElement.SetAttributeValue("style", "display: none;");

                helpKeyValues[key] = helpDivContent;

                var linkElement = textNode.AppendChild(ownerDocument.CreateElement("a"));
                linkElement.SetAttributeValue("href", "#");
                linkElement.SetAttributeValue("onclick", string.Format("openHelpDialog('{0}')", key));

                linkElement.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = testContainer.Name;
            }
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
