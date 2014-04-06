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
    }
}
