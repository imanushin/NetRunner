using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    public sealed class ConverterParser : BaseParser
    {
        public override bool TryParse<TResult>(string value, out TResult parsedResult)
        {
            if (IsDisabled)
            {
                parsedResult = default(TResult);

                return false;
            }

            var targetType = typeof(TResult);

            try
            {
                parsedResult = (TResult)Convert.ChangeType(value, targetType);

                return true;
            }
            catch (InvalidCastException)
            {
#warning Trace this exception to the log
                parsedResult = default(TResult);

                return false;
            }
        }

        public static bool IsDisabled
        {
            get;
            set;
        }
    }
}
