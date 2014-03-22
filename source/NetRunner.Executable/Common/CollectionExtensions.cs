using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
