using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Help class to extend the collections
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Gets value from dictionary if it exists.
        /// Adds new key to the dictionary otherwise
        /// </summary>
        /// <returns>Exiting value or created new value</returns>
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            TValue value;

            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            value = new TValue();

            dictionary[key] = value;

            return value;
        }

        /// <summary>
        /// Gets value from dictionary if it exists.
        /// Create new value and put into dictionary otherwise
        /// </summary>
        /// <returns>Exiting value or created new value</returns>
        public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> ctor)
        {
            TValue value;

            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            value = ctor(key);

            dictionary[key] = value;

            return value;
        }
    }
}
