using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSample
{
    public sealed class InOutObject
    {
        private string currentValue;

        public string InValue
        {
            set
            {
                currentValue = value;
            }
        }

        public string OutValue
        {
            get
            {
                return currentValue;
            }
        }
    }
}
