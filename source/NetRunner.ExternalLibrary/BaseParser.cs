using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    public abstract class BaseParser
    {
        // ReSharper disable ConvertToConstant.Global
        public static class Priorities
        {
            public static readonly int EmbeddedParsersPriority = -10;

            public static readonly int DefaultPriority = 0;
        }
        // ReSharper restore ConvertToConstant.Global

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
        
        public int Priority
        {
            get;
            private set;
        }
    }
}
