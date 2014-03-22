using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit.Fixtures;

namespace FitSample
{
    public sealed class SampleFixture : DoFixture
    {
        public SampleFixture()
        {
            Debugger.Break();
        }

        public bool PingSite(string url)
        {
            return true;
        }
    }
}
