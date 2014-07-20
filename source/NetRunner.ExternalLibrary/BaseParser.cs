using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Use the tutorial here: https://github.com/imanushin/NetRunner/wiki/Parsing .
    /// The base type for the any parser. To add the custom parser implementation, simple inherit your parser type from this.
    /// See also overrides of this type in the same namespace. 
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public abstract class BaseParser
    {
        // ReSharper disable ConvertToConstant.Global
        /// <summary>
        /// Priorities which are used by this library. All parser with the bigger priority will be used before all others.
        /// </summary>
        public static class Priorities
        {
            public static readonly int EmbeddedParsersPriority = -10;

            public static readonly int ConverterParserPrioriry = -5;

            public static readonly int DefaultPriority = 0;
        }
        // ReSharper restore ConvertToConstant.Global

        /// <summary>
        /// Create parser with the zero priorite (e.g. this parser will be used <b>before</b> any embedded parser)
        /// </summary>
        protected BaseParser()
            : this(Priorities.DefaultPriority)
        {
        }

        protected BaseParser(int priority)
        {
            Priority = priority;
        }

        /// <summary>
        /// Tries to parse type. Function returns false if type is not supported.
        /// Function throws exception if type is supported and parsing error was occurred.
        /// Return value can be null.
        /// </summary>
        /// <typeparam name="TResult">Expected result type</typeparam>
        /// <param name="value">Value from Fitnesse input</param>
        /// <param name="parsedResult">Parse result if value could be parsed.</param>
        /// <returns>false is type is not supported. Exception is type is supported, however value could not be parsed. Otherwise true.</returns>
        public abstract bool TryParse<TResult>(string value, [CanBeNull] out TResult parsedResult);
        
        /// <summary>
        /// Parser priority. Can be set once during the parser construction
        /// </summary>
        public int Priority
        {
            get;
            private set;
        }
    }
}
