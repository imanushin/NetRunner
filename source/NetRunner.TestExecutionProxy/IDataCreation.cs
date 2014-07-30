using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    public interface IDataCreation<TData, in TTargetType> : IReference<TData> 
        where TData : class
    {
        TData Create(TTargetType targetItem);
    }
}
