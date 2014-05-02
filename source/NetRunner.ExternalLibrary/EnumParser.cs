using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    public sealed class EnumParser : BaseDictionaryParser
    {
        protected override IDictionary<string, TResult> GetValues<TResult>()
        {
            var targetType = typeof (TResult);

            if (IsDisabled || !targetType.IsEnum)
                return null;

            return Enum.GetNames(targetType).ToDictionary(n => n, n => (TResult)Enum.Parse(targetType, n));
        }

        public static bool IsDisabled
        {
            get;
            set;
        }
    }
}
