using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Remoting;
using NetRunner.Executable.Properties;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal static class DocumentationStore
    {
        private const char genericItemStart = '`';
        private static readonly string[] genericSuffixes = Enumerable.Range(1, 9).Select(i => string.Format("{0}{1}", genericItemStart, i)).ToArray();

        private static readonly Dictionary<string, string> internalStore = new Dictionary<string, string>();
        private static readonly ReadOnlyList<string> microsoftNamespaces = new ReadOnlyList<string>(new[] { "System.", "Microsoft." });


        private static string TryFindForKey(string identity)
        {
            string result;
            if (internalStore.TryGetValue(identity, out result))
            {
                return result;
            }

            return null;
        }

        [CanBeNull]
        public static string GetFor(IHelpIdentity function)
        {
            return TryFindForKey(function.HelpIdentity);
        }

        public static void LoadForAssemblies(ReadOnlyList<string> assemblyPathes)
        {
            Validate.ArgumentIsNotNull(assemblyPathes, "assemblyPathes");

            ProcessInternalHelp();

            foreach (var assemblyPath in assemblyPathes)
            {
                try
                {

                    var xmlFile = assemblyPath.Substring(0, assemblyPath.Length - 3) + "xml";

                    if (!File.Exists(xmlFile))
                    {
                        continue;
                    }

                    Trace.TraceInformation("Analyzing html help file: '{0}'", xmlFile);

                    var xmlDocument = new XmlDocument();

                    xmlDocument.Load(xmlFile);

                    ProcessXmlDocument(xmlDocument);
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning("Assembly '{0}' has incorrect xml help: {1}", assemblyPath, ex);
                }
            }
        }

        private static void ProcessInternalHelp()
        {
            var document = new XmlDocument();

            document.LoadXml(Resources.NetRunner_ExternalLibrary_xmlhelp);

            ProcessXmlDocument(document);
        }

        private static void ProcessXmlDocument(XmlDocument xmlDocument)
        {
            try
            {
                var nodes = xmlDocument.SelectNodes("//doc//members//member[contains(@prop, name) and descendant::summary]");

                if (ReferenceEquals(null, nodes))
                {
                    return;
                }

                foreach (XmlNode member in nodes)
                {
                    var attributes = member.Attributes;

                    Validate.IsNotNull(attributes, "Node {0} does not have attributes", member.Name);

                    var memberNameAttribute = attributes.GetNamedItem("name");

                    var memberName = memberNameAttribute.Value;

                    var divider = memberName.IndexOf(':');

                    if (divider < 0)
                    {
                        continue;
                    }

                    var summaryNode = member.SelectNodesWithName("summary").FirstOrDefault();

                    if (ReferenceEquals(summaryNode, null))
                    {
                        continue;
                    }

                    ReplaceWellKnownNodes(summaryNode);

                    ProcessParams(member, memberName);

                    AddNewMember(memberName, summaryNode);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to get help information: {0}", ex);
            }
        }

        private static void AddNewMember(string memberName, XmlNode summaryNode)
        {
            if (memberName.Contains(genericItemStart))
            {
                memberName = genericSuffixes.Aggregate(memberName, (current, suffix) => current.Replace(suffix, string.Empty));
            }

            if (memberName.StartsWith("M:", StringComparison.Ordinal) && memberName.Contains('{'))
            {
                memberName = Regex.Replace(memberName, @"\{(.*?)\}", string.Empty);
            }

            internalStore[memberName] = HtmlParser.ReplaceUnknownTags(summaryNode.InnerXml);
        }

        private static void ReplaceWellKnownNodes(XmlNode helpNode)
        {
            XmlDocument ownerDocument = helpNode.OwnerDocument;
            Validate.IsNotNull(ownerDocument, "Owner document is null");

            foreach (var seeNode in helpNode.SelectNodesWithName("see"))
            {
                var refAttribute = seeNode.Attributes["cref"];

                if (ReferenceEquals(null, refAttribute))
                {
                    continue;
                }

                var targetType = refAttribute.Value.Replace("T:", string.Empty);

                XmlElement newNode;

                if (microsoftNamespaces.Any(targetType.StartsWith))
                {
                    newNode = ownerDocument.CreateElement("a");

                    newNode.SetAttribute("href", string.Format("http://msdn.microsoft.com/en-us/library/{0}.aspx", targetType));
                }
                else
                {
                    newNode = ownerDocument.CreateElement("b");
                }

                newNode.InnerText = targetType;

                helpNode.ReplaceChild(newNode, seeNode);
            }
        }

        private static void ProcessParams(XmlNode helpNode, string memberName)
        {
            var parameterNodes = helpNode.SelectNodesWithName("param");

            foreach (var parameterNode in parameterNodes)
            {
                ReplaceWellKnownNodes(parameterNode);

                helpNode.RemoveChild(parameterNode);

                var attributes = parameterNode.Attributes;

                Validate.IsNotNull(attributes, "Attributes array of node 'param' for the help member '{0}' is null", memberName);

                var newMemberName = attributes["name"];

                if (ReferenceEquals(null, newMemberName))
                {
                    continue;
                }

                AddNewMember(string.Format(ParameterInfoData.ParameterFormat, memberName, newMemberName.InnerText), parameterNode);
            }
        }
    }
}
