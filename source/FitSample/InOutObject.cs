using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSample
{
    public sealed class InOutObject
    {
        private int currentValue;

        public int InValue
        {
            set
            {
                currentValue = value;
            }
        }

        public int OutValue
        {
            get
            {
                return currentValue;
            }
        }
    }
}
