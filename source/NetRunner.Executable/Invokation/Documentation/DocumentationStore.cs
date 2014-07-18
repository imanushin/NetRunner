using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using NetRunner.Executable.Common;
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
            var key = string.Format(
                methodIdentityFormat,
                function.Owner.FullName,
                function.Method.SystemName,
                string.Join(",", function.Arguments.Select(a => a.ParameterType.FullName)));

            return TryFindForKey(key);
        }

        public static Dictionary<string, string> GetAllTypesHelp()
        {
            return internalStore
                .Where(kv => kv.Key.StartsWith("T:", StringComparison.Ordinal))
                .ToDictionary(kv => kv.Key.Substring(2), kv => kv.Value);
        }

        public static void LoadForAssemblies(ReadOnlyList<string> assemblyPathes)
        {
            Validate.ArgumentIsNotNull(assemblyPathes, "assemblyPathes");

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

                    var nodes = xmlDocument.SelectNodes("//doc//members//member[contains(@prop, name) and descendant::summary]");

                    if (ReferenceEquals(null, nodes))
                    {
                        continue;
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

                        var summaryNode = member.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => string.Equals(n.Name, "summary"));

                        if (ReferenceEquals(summaryNode, null))
                        {
                            continue;
                        }

                        internalStore[memberName] = HtmlParser.ReplaceUnknownTags(summaryNode.InnerXml);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning("Assembly '{0}' has incorrect xml help: {1}", assemblyPath, ex);
                }
            }
        }
    }
}
