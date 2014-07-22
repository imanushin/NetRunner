using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Embedded tests. They are fully supported on the all platforms where NetRunner can work.
    /// </summary>
    public sealed class EmbeddedTestContainer : BaseTestContainer
    {
        /// <summary>
        /// Invokes the Debugger.Launch() method.<br/>
        /// Usage:<br/>
        /// | ''' NetRunner:Launch Debugger ''' |
        /// </summary>
        [ExcludeDefaultFunctionName]
        [AdditionalFunctionName("NetRunner:Launch Debugger")]
        public void DebbugerBreak()
        {
            Debugger.Launch();
        }
    }
}
