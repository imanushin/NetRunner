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
            :base(Priorities.ConverterParserPriority)
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

        /// <summary>
        /// This parser can be disabled in the any time of the execution, however it is recommended to disable it before the first function execute.
        /// </summary>
        public static bool IsDisabled
        {
            get;
            set;
        }
    }
}
