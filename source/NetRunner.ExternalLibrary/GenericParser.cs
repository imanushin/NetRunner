using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Abstract to parse only the types inherited from the <typeparam name="TResultValue"/>.
    /// </summary>
    public abstract class GenericParser<TResultValue> : BaseParser
    {
        protected GenericParser(int priority)
            : base(priority)
        {
        }

        protected GenericParser()
        {
        }

        /// <summary>
        /// Locked default parsing implementation
        /// </summary>
        public sealed override bool TryParse<TResult>(string value, out TResult parsedResult)
        {
            if (typeof(TResult).IsAssignableFrom(typeof(TResultValue)))
            {
                parsedResult = (TResult)((object)Parse(value));

                return true;
            }

            parsedResult = default(TResult);

            return false;
        }

        /// <summary>
        /// Override this method to parse input type <typeparam name="TResultValue"/>.
        /// </summary>
        protected abstract TResultValue Parse(string inputData);
    }
}
