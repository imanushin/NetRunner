using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.InternalTests
{
    internal sealed class RawResultAnalyzer : BaseTableArgument
    {
        private readonly string rawResult;

        public RawResultAnalyzer(string rawResult)
        {
            this.rawResult = rawResult.ToLowerInvariant();
        }

        public void Check(string text, out int count)
        {
            count = rawResult.Split(new[]
            {
                text.ToLowerInvariant()
            }, StringSplitOptions.None).Length - 1;
        }
    }
}
