using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSample
{
    internal sealed class IncorrectEqualityClass
    {
        public override bool Equals(object obj)
        {
            throw new InvalidOperationException("Surprise !!!");
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static IncorrectEqualityClass Parse(string inputLine)
        {
            return new IncorrectEqualityClass();
        }
    }
}
