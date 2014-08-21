using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSample
{
    /// <summary>
    /// Generic Parser
    /// </summary>
    public sealed class GenericEnumInfo<TEnum>
    {
        private GenericEnumInfo(TEnum value)
        {
            Value = value;
        }

        /// <summary>
        /// Parsed value
        /// </summary>
        public TEnum Value
        {
            get;
            private set;
        }

        public static GenericEnumInfo<TEnum> Parse(string inputLine)
        {
            var value = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .First(n => string.Equals(inputLine, n.ToString(), StringComparison.OrdinalIgnoreCase));

            return new GenericEnumInfo<TEnum>(value);
        }

        public override bool Equals(object obj)
        {
            var other = obj as GenericEnumInfo<TEnum>;

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return Equals(other.Value, Value);
        }
    }
}
