using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.Common
{
    internal static class Utils
    {
        public static string ToFinnessIntegerString(this int value)
        {
            return value.ToString(CultureInfo.InvariantCulture).PadLeft(FitnesseCommunicator.IntengerFitnessSize,'0');
        }

        public static void AppendJoin<TValue>(this StringBuilder builder, string joinItem, Action<StringBuilder, TValue> action, IEnumerable<TValue> items)
        {
            bool isFirst = true;

            foreach (var item in items)
            {
                if (!isFirst)
                {
                    builder.Append(joinItem);
                }

                isFirst = false;

                action(builder, item);
            }
        }
    }
}
