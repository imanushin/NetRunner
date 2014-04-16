using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
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
