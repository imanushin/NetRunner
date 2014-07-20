using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Parser for the standard <see cref="System.Convert"/> class implementation.
    /// Register conversion from string to the target type to parse them automatically by using this parser.
    /// </summary>
    public sealed class ConverterParser : BaseParser
    {
        internal ConverterParser()
            :base(Priorities.ConverterParserPrioriry)
        {
        }

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
