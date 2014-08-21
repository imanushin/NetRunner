using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Remoting;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal static class EngineInfo
    {
        private const string howToAddHelpLinkFormat = "Help does not available for type <b>{0}</b>. See <a href=\"https://github.com/imanushin/NetRunner/wiki/Help-and-hints\">this tutorial</a> about the NetRunner help configuring.";
       

        private static readonly Dictionary<string, string> typesToCut = new Dictionary<string, string>
        {
            {"Boolean", "bool" },
            {"Int32", "int" },
            {"Double", "double" },
            {"String", "string" },
            {"Float", "float" },
            {"Void", "void" },
        };

        public static string PrintTestEngineInformation()
        {
            var document = new HtmlDocument();
            
            document.DocumentNode.AppendChild(document.CreateElement("br"));

            var expandableDiv = document.DocumentNode.AppendChild(document.CreateElement("div"));

            expandableDiv.Attributes.Add(HtmlParser.ClassAttributeName, "collapsible closed");

            var titleNode = document.CreateElement("p");
            titleNode.InnerHtml = "Engine information";
            titleNode.Attributes.Add(HtmlParser.ClassAttributeName, "title");

            expandableDiv.AppendChild(titleNode);

            var textNode = document.CreateElement("div");
            
            AddTitle(textNode, "Test containers:");
            TestContainersToSequence(textNode, ReflectionLoader.TestContainers.Select(t=>t.Type));

            AddTitle(textNode, "Parsers:");
            ParsersToSequence(textNode);

            AddTitle(textNode, "Test assemblies:");
            StringsToSequence(textNode, ReflectionLoader.AssemblyPathes);

            AddTextTag(textNode, "h5", "Current executable path: " + Process.GetCurrentProcess().MainModule.FileName);

            AddTextTag(textNode, "h5", "Working folder: " + Environment.CurrentDirectory);

            textNode.AppendChild(document.CreateElement("br"));
            textNode.AppendChild(document.CreateElement("br"));
            textNode.AppendChild(document.CreateElement("br"));
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
                var parserType = parser.GetType();

                var itemNode = CreateAndAddNodeForType(textNode, parserType);

                var text = string.Format("{0}; priority: {1}", parserType.GetData().Name, parser.ExecuteProperty<int>("Priority"));

                itemNode.InnerHtml = text;
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
        
        private static void TestContainersToSequence(HtmlNode textNode, IEnumerable<TypeReference> inputTypes)
        {
            foreach (var type in inputTypes)
            {
                var testContainerNode = CreateAndAddNodeForType(textNode, type);

                testContainerNode.InnerHtml = type.GetData().Name;
            }
        }

        private static HtmlNode CreateAndAddNodeForType(HtmlNode textNode, TypeReference type)
        {
            var ownerDocument = textNode.OwnerDocument;

            var testContainerNode = textNode.AppendChild(ownerDocument.CreateElement("div"));
            textNode.AppendChild(ownerDocument.CreateElement("br"));

            testContainerNode.SetAttributeValue(HtmlHintManager.AttributeName, HtmlHintManager.GetHintAttributeValue(type.GetData()));
            testContainerNode.SetAttributeValue("style", HtmlParser.ItemSequenceStyle);
            return testContainerNode;
        }

        private static string GetTypeDocumentation(TypeReference testContainerType)
        {
            var rawDocumentation = DocumentationStore.GetFor(testContainerType.GetData());

            return string.IsNullOrWhiteSpace(rawDocumentation)
                ? string.Format(howToAddHelpLinkFormat, testContainerType.GetData().Name)
                : rawDocumentation;
        }

        private static void AllFunctionsToSequence(HtmlNode textNode, ReadOnlyList<LazyIsolatedReference<BaseTestContainer>> inputContainers)
        {
            var ownerDocument = textNode.OwnerDocument;

            foreach (var testContainer in inputContainers)
            {
                var testContainerType = testContainer.Type;

                textNode.AppendChild(ownerDocument.CreateElement("h4")).InnerHtml = string.Format("Container {0}:", testContainerType.GetData().Name);
                textNode.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = GetTypeDocumentation(testContainerType);

                var containerParagraph = textNode.AppendChild(ownerDocument.CreateElement("p"));
                
                var methods = ReflectionLoader.GetMethodFor(testContainer);

                methods.ForEach(m => AppendMethod(m, containerParagraph));

                textNode.AppendChild(ownerDocument.CreateElement("p")).InnerHtml = testContainerType.GetData().Name;
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

            string systemName = function.MethodData.SystemName;

            if (function.AvailableFunctionNames.Any() &&
                (function.AvailableFunctionNames.Count > 1 ||
                !string.Equals(function.AvailableFunctionNames.First(), systemName, StringComparison.OrdinalIgnoreCase)))
            {
                var names = string.Join(", ", function.AvailableFunctionNames
                    .Select(name => string.Format("<i>{0}</i>", HtmlParser.ReplaceUnknownTags(name))));

                innerHtml.AppendFormat("<p>Available names: {0}</p>", names);
            }

            innerHtml.AppendFormat("<b>{0} {1}(", CutType(function.ResultType), systemName);

            innerHtml.AppendJoin(", ", AppendParameter, function.Arguments.Select(p=>p.GetData()));

            innerHtml.AppendFormat(")</b>");

            functionNode.InnerHtml = innerHtml.ToString();
        }

        private static void AppendParameter(StringBuilder builder, ParameterInfoData parameter)
        {
            var hint = HtmlHintManager.GetHintAttributeValue(parameter);
            
            if (string.IsNullOrEmpty(hint))
            {
                builder.AppendFormat(
                    "<b>{0}{1} {2}</b>",
                    parameter.IsOut ? "out " : string.Empty,
                    CutType(parameter.ParameterType),
                    parameter.Name);

                return;
            }

            builder.AppendFormat(
                "<b {0}=\"{1}\">{2}{3} {4}</b>",
                HtmlHintManager.AttributeName,
                hint,
                parameter.IsOut ? "out " : string.Empty,
                CutType(parameter.ParameterType),
                parameter.Name);
        }

        [NotNull]
        private static string CutType(TypeReference type)
        {
            var name = type.GetData().Name;

            string result;

            if (typesToCut.TryGetValue(name, out result))
            {
                return result;
            }

            if (name.Contains("`"))
            {
                name = name.Substring(0, name.IndexOf("`", StringComparison.Ordinal));
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
