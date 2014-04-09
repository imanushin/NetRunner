using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Common
{
    internal static class CollectionExtensions
    {
        public static ReadOnlyList<TValue> ToReadOnlyList<TValue>(this IEnumerable<TValue> items)
        {
            var list = items as ReadOnlyList<TValue>;

            if (list != null)
                return list;

            return new ReadOnlyList<TValue>(items);
        }

        public static IEnumerable<TValue> SkipNulls<TValue>(this IEnumerable<TValue> items)
        {
            Validate.ArgumentIsNotNull(items, "items");

            return items.Where(item => !ReferenceEquals(null, item));
        }

        [NotNull]
        public static HtmlNode FirstCell(this ICollection<HtmlNode> items)
        {
            Validate.CollectionArgumentHasElements(items, "items");

            var result = items
                .FirstOrDefault(item => string.Equals(item.Name, HtmlParser.TableCellNodeName, StringComparison.OrdinalIgnoreCase));

            Validate.IsNotNull(result, "Current row does not contain any cell");

            return result;
        }

        [CanBeNull]
        public static HtmlNode FirstBoldCellOrNull(this IEnumerable<HtmlNode> items)
        {
            Validate.CollectionArgumentHasElements(items, "items");

            return items
                .Where(item => string.Equals(item.Name, HtmlParser.TableCellNodeName, StringComparison.OrdinalIgnoreCase))
                .Where(item => item.HasChildNodes)
                .FirstOrDefault(item => string.Equals(item.ChildNodes.First().Name, HtmlParser.BoldNodeName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
