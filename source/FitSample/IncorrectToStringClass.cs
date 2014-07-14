using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSample
{
    internal sealed class IncorrectToStringClass
    {
        public override string ToString()
        {
            throw new InvalidOperationException("Surprise !!!");
        }

        public static IncorrectToStringClass Parse(string inputLine)
        {
            return new IncorrectToStringClass();
        }
    }
}
