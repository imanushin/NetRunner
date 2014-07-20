using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Parsed for any defined enumerable type. The system type <see cref="System.Enum"/> it used to retrieve all possible enumerations names.
    /// </summary>
    public sealed class EnumParser : BaseDictionaryParser
    {
        protected override IDictionary<string, TResult> GetValues<TResult>()
        {
            var targetType = typeof (TResult);

            if (IsDisabled || !targetType.IsEnum)
                return null;

            return Enum.GetNames(targetType).ToDictionary(n => n, n => (TResult)Enum.Parse(targetType, n));
        }

        /// <summary>
        /// Disable this type by using this property. The type disabling should be used before the first type parsing. Use the <see cref="BaseTestContainer"/> constructors for this purpose.
        /// </summary>
        public static bool IsDisabled
        {
            get;
            set;
        }
    }
}
