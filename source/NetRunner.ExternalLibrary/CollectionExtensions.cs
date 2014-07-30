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
        /// Gets key from dictionary if it exists.
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
    }
}
