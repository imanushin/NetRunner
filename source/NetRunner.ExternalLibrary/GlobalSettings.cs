using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    [UsedImplicitly]
    public static class GlobalSettings
    {
        /// <summary>
        /// All parsers will Trim input lines before conversion. All input lines will be trimmed
        /// </summary>
        public static bool TrimAllInputLines
        {
            get;
            set;
        }
    }
}
