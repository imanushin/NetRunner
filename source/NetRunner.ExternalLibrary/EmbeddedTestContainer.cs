using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    public sealed class EmbeddedTestContainer : BaseTestContainer
    {
        [ExcludeDefaultFunctionName]
        [AdditionalFunctionName("NetRunner:Launch Debugger")]
        public void DebbugerBreak()
        {
            Debugger.Launch();
        }
    }
}
