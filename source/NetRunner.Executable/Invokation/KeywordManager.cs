using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    internal static class KeywordManager
    {
        [NotNull]
        public static AbstractKeyword Parse(HtmlCell firstCell)
        {
            Validate.ArgumentCondition(!firstCell.IsBold, "firstCell", "Unable to create keyword from '{0}': keyword candidate should not be bold", firstCell);

            Trace.TraceWarning("!!!!!");

            return new UnknownKeyword();
        }
    }
}
