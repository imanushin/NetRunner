using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public abstract class GeneralReferenceObject : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            //http://social.msdn.microsoft.com/Forums/en-US/3ab17b40-546f-4373-8c08-f0f072d818c9/remotingexception-when-raising-events-across-appdomains?forum=netfxremoting
            return null;
        }
    }
}
