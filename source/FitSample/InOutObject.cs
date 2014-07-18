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

        /// <summary>
        /// Value for items inserting
        /// </summary>
        public int InValue
        {
            set
            {
                currentValue = value;
            }
        }

        /// <summary>
        /// Return value from InValue property
        /// </summary>
        public int OutValue
        {
            get
            {
                return currentValue;
            }
        }
    }
}
