using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.InternalTests
{
    internal sealed class UriParser : GenericParser<Uri>
    {
        protected override Uri Parse(string inputData)
        {
            return new Uri(inputData, UriKind.RelativeOrAbsolute);
        }
    }
}
