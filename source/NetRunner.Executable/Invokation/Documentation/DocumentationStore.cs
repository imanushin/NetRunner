using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using NetRunner.Executable.Common;
using NetRunner.Executable.Properties;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal static class DocumentationStore
    {
        private static readonly Dictionary<string, string> internalStore = new Dictionary<string, string>();
        private static readonly ReadOnlyList<string> microsoftNamespaces = new ReadOnlyList<string>(new[] { "System.", "Microsoft." });

        private const string typeIdentityFormat = "T:{0}";
        private const string propertyFormat = "P:{0}.{1}";

        [CanBeNull]
        public static string GetFor(TypeReference type)
        {
            var identity = string.Format(typeIdentityFormat, type.FullName);

            return TryFindForKey(identity);
        }

        private static string TryFindForKey(string identity)
        {
            string result;
            if (internalStore.TryGetValue(identity, out result))
            {
                return result;
            }

            return null;
        }

        public static string GetFor(IHelpIdentity function)
        {
            return TryFindForKey(function.HelpIdentity);
        }

        public static string GetFor(PropertyReference property)
        {
            var key = string.Format(
                propertyFormat,
                property.Owner.FullName,
                property.Name);

            var result = TryFindForKey(key);

            return result ?? GetFor(property.Owner);
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

                    Trace.TraceInformation("Analysing html help file: '{0}'", xmlFile);

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

                AddNewMember(string.Format(ParameterInfoReference.ParameterFormat, memberName, newMemberName.InnerText), parameterNode);
            }
        }
    }
}
