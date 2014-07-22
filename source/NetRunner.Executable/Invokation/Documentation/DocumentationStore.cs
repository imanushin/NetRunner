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

        private const string typeIdentityFormat = "T:{0}";
        private const string methodIdentityFormat = "M:{0}.{1}({2})";
        private const string parameterFormat = "Par:{0}.{1}";
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

        public static string GetFor(TestFunctionReference function)
        {
            return GetFor(function.Method);
        }

        private static string GetFor(FunctionMetaData function)
        {
            var key = GetKey(function);

            return TryFindForKey(key);
        }

        private static string GetKey(FunctionMetaData function)
        {
            return string.Format(
                methodIdentityFormat,
                function.Owner.FullName,
                function.SystemName,
                string.Join(",", function.GetParameters().Select(a => ReplaseRefSymbol(a.ParameterType.FullName))));
        }

        private static string ReplaseRefSymbol(string typeName)
        {
            if (typeName.EndsWith("&"))
            {
                return typeName.Substring(0, typeName.Length - 1) + "@";
            }

            return typeName;
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

        public static string GetFor(ParameterInfoReference parameter)
        {
            var functionKey = GetKey(parameter.Owner);

            var key = string.Format(parameterFormat, functionKey, parameter.Name);

            return TryFindForKey(key);
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

                    var summaryNode = member.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => string.Equals(n.Name, "summary", StringComparison.OrdinalIgnoreCase));

                    if (ReferenceEquals(summaryNode, null))
                    {
                        continue;
                    }

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

        private static void ProcessParams(XmlNode helpNode, string memberName)
        {
            var parameterNodes = helpNode.ChildNodes.Cast<XmlNode>().Where(n => string.Equals(n.Name, "param")).ToReadOnlyList();

            foreach (XmlNode parameterNode in parameterNodes)
            {
                Validate.IsNotNull(parameterNode, "Node with the 'param' name is null for the help member '{0}'", memberName);

                helpNode.RemoveChild(parameterNode);

                var attributes = parameterNode.Attributes;

                Validate.IsNotNull(attributes, "Attributes array of node 'param' for the help member '{0}' is null", memberName);

                var newMemberName = attributes["name"];

                if (ReferenceEquals(null, newMemberName))
                {
                    continue;
                }

                AddNewMember(string.Format(parameterFormat, memberName, newMemberName.InnerText), parameterNode);
            }
        }
    }
}
