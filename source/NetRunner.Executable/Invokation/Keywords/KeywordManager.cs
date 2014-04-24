using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation.Keywords
{
    internal static class KeywordManager
    {
        [NotNull]
        public static AbstractKeyword Parse(IReadOnlyCollection<HtmlCell> cells)
        {
            Validate.CollectionArgumentHasElements(cells, "cells");

            var firstCell = cells.First();

            if(firstCell.IsBold)
                return new EmptyKeyword(cells);

            Trace.TraceWarning("!!!!!");

            return new UnknownKeyword(cells);
        }
    }
}
