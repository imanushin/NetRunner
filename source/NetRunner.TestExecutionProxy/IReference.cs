using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public interface IReference<TData>
        where TData : class 
    {
        string StrongIdentity
        {
            get;
        }
    }
}
