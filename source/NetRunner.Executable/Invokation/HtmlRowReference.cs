using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    [ImmutableObject(true)]
    internal sealed class HtmlRowReference : BaseReadOnlyObject
    {
        private static int rowGlobalIndex = 1;
        private const string globalAttributeIndexName = "GlobalRowIndex";

        private HtmlRowReference(int rowGlobalIndex)
        {
            RowGlobalIndex = rowGlobalIndex;
        }

        public int RowGlobalIndex
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return RowGlobalIndex;
        }

        public static HtmlRowReference MarkRowAndGenerateReference(HtmlNode tableRow)
        {
            Validate.ArgumentTagHasName(tableRow, HtmlParser.TableRowNodeName, "tableRow");

            var attribute = tableRow.Attributes.AttributesWithName(globalAttributeIndexName).FirstOrDefault();

            var index = rowGlobalIndex++;

            if (attribute == null)
            {
                var document = tableRow.OwnerDocument;

                attribute = document.CreateAttribute(globalAttributeIndexName);

                attribute.Value = index.ToString(CultureInfo.InvariantCulture);

                tableRow.Attributes.Append(attribute);
            }
            else
            {
                index = int.Parse(attribute.Value);
            }

            return new HtmlRowReference(index);
        }

        [NotNull, Pure]
        public HtmlNode GetRow(HtmlNode table)
        {
            Validate.ArgumentTagHasName(table, HtmlParser.TableNodeName, "table");

            var stringIndex = RowGlobalIndex.ToString(CultureInfo.InvariantCulture);

            var targetRow = table.ChildNodes.FirstOrDefault(
                row =>
                    row.Attributes
                    .AttributesWithName(globalAttributeIndexName)
                    .Any(a => string.Equals(a.Value, stringIndex, StringComparison.Ordinal)));

            Validate.IsNotNull(targetRow, "Unable to find row with index {0} in current table", stringIndex);

            return targetRow;
        }
    }
}
