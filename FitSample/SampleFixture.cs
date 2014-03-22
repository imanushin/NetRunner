using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fitlibrary;

namespace FitSample
{
    public sealed class SampleFixture : DoFixture
    {
        public SampleFixture()
        {
        }

        public bool PingSite(string url)
        {
            return true;
        }
    }
}
