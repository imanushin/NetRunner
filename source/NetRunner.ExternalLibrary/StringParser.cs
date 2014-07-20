using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Interal string parser. Used to avoid any string conversion during the execution. It can be replaced to the custom, however input string conversion attributes (such as <see cref="StringTrimAttribute"/> are applyed before any parsing)
    /// </summary>
    internal sealed class StringParser : GenericParser<string>
    {
        public StringParser()
            :base(Priorities.EmbeddedParsersPriority)
        {
        }

        protected override string Parse(string inputData)
        {
            return inputData;
        }
    }
}
