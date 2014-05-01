using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class StringCanBeEmptyAttribute : Attribute
    {
    }
}
