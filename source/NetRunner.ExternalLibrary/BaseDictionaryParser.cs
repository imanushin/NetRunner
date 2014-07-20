using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Key-value parser. To use it override <see cref="GetValues{TResult}"/> method by the all available keys and values. <br/>
    /// All spaces will be ignored. e.g. the lines "1_2" is equal with "1 2" and with "12". Override method <see cref="ReplaceWhiteSpace"/> to change this behaviour.
    /// </summary>
    public abstract class BaseDictionaryParser : BaseParser
    {
        private readonly object syncRoot = new object();

        private readonly Dictionary<Type,Dictionary<string,object>> parsedTypes = new Dictionary<Type, Dictionary<string, object>>(); 

        public sealed override bool TryParse<TResult>(string value, out TResult parsedResult)
        {
            var targetType = typeof (TResult);

            lock (syncRoot)
            {
                Dictionary<string, object> currentDictionary;

                if (!parsedTypes.TryGetValue(targetType, out currentDictionary))
                {
                    var tempDictinary = GetValues<TResult>();

                    if (tempDictinary == null)
                    {
                        parsedResult = default(TResult);

                        return false;
                    }

                    currentDictionary = tempDictinary.ToDictionary(kv => kv.Key, kv => (object) kv.Value);

                    parsedTypes[targetType] = currentDictionary;
                }

                object result;

                if (currentDictionary.TryGetValue(value, out result))
                {
                    parsedResult = (TResult) result;

                    return true;
                }

                KeyValuePair<string, object> otherValue = currentDictionary.FirstOrDefault(kv => string.Equals(kv.Key, value, StringComparison.OrdinalIgnoreCase));

                if (otherValue.Equals(default(KeyValuePair<string, object>)))
                {
                    otherValue = currentDictionary.FirstOrDefault(kv => string.Equals(ReplaceWhiteSpace(kv.Key), ReplaceWhiteSpace(value), StringComparison.OrdinalIgnoreCase));
                }

                if (otherValue.Equals(default(KeyValuePair<string, object>)))
                {
                    throw new ArgumentException(
                        string.Format(
                            "Unable to parse input line '{0}' - there are not matched values defines. Possible values: {1}", 
                            value, 
                            string.Join(", ", currentDictionary.Keys.Select(s=>'\'' + s + '\''))));
                }

                currentDictionary[value] = otherValue.Value;

                parsedResult = (TResult) otherValue.Value;

                return true;
            }
        }

        protected virtual string ReplaceWhiteSpace(string inputString)
        {
            return inputString
                .Replace(" ", string.Empty)
                .Replace(Environment.NewLine, string.Empty)
                .Replace("_", string.Empty);
        }

        /// <summary>
        /// Get possible type values
        /// null if type is not supported.
        /// Parser will cache this data and will try to find appropriate value with the following scheme:
        /// 1. If exact key was found then the value will be returned
        /// 2. If case insensitive key is found then it will be used to retrieve value (key aAa will equal with aaa)
        /// 3. If value with white spaces is found then it will be used to retrieve value (key "a_ a a" will equal with aaa)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>null if type is not supported.</returns>
        [CanBeNull]
        protected abstract IDictionary<string, TResult> GetValues<TResult>();
    }
}
