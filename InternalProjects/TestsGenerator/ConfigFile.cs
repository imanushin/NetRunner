using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsGenerator
{
    internal sealed class ConfigFile
    {
        private readonly Dictionary<string, List<string>> sections = new Dictionary<string, List<string>>();

        public ConfigFile(string filePath)
        {
            var contents = new string[0];

            if (File.Exists(filePath))
                contents = File.ReadAllLines(filePath);

            for (int lineIndex = 0; lineIndex < contents.Length; lineIndex++)
            {
                string currentLine = contents[lineIndex].Trim();

                if (IsContentHeader(currentLine))
                {
                    string contentHeader = currentLine.Substring(1, currentLine.Length - 2);

                    var innerItems = new List<string>();

                    for (lineIndex = lineIndex + 1; lineIndex < contents.Length; lineIndex++)
                    {
                        currentLine = contents[lineIndex].Trim();

                        if (IsContentHeader(currentLine))
                        {
                            lineIndex--;

                            break;
                        }

                        if (string.IsNullOrWhiteSpace(currentLine))
                            continue;

                        innerItems.Add(currentLine);
                    }

                    sections[contentHeader] = innerItems;

                }
            }
        }

        private static bool IsContentHeader(string currentLine)
        {
            return currentLine.StartsWith("[") && currentLine.EndsWith("]");
        }

        public string[] GetSectionItems(string sectionName)
        {
            foreach (var section in sections)
            {
                if (string.Equals(section.Key, sectionName, StringComparison.OrdinalIgnoreCase))
                {
                    return section.Value.ToArray();
                }
            }

            return new string[0];
        }
    }
}
