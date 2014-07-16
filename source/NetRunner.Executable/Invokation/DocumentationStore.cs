﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation
{
    internal static class DocumentationStore
    {
        private static readonly Dictionary<string, string> internalStore = new Dictionary<string, string>();

        private const string typeIdentityFormat = "T:{0}";

        [CanBeNull]
        public static string GetForType(TypeReference type)
        {
            string result;

            var identity = string.Format(typeIdentityFormat, type.FullName);

            if (internalStore.TryGetValue(identity, out result))
            {
                return result;
            }

            return null;
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

                        internalStore[memberName] = summaryNode.InnerXml;
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