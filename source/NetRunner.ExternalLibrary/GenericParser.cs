using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    internal abstract class GenericParser<TResultValue> : BaseParser
    {
        protected GenericParser(int priority)
            : base(priority)
        {
        }

        protected GenericParser()
        {
        }

        public override bool TryParse<TResult>(string value, out TResult parsedResult)
        {
            if (typeof(TResult).IsAssignableFrom(typeof(TResultValue)))
            {
                parsedResult = (TResult)((object)Parse(value));

                return true;
            }

            parsedResult = default(TResult);

            return false;
        }

        protected abstract TResultValue Parse(string inputData);
    }
}
