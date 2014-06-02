using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Exclude or not the default function name from function find procedure.
    /// For functions like "MyClass.AddAnd(int a, int b)" the default name will be "AddAnd". Therefore, fitnesse can contain text like "| '''add''' | 5 | '''and''' | 3 |"
    /// If the default name if disabled then additional name could be used: <seealso cref="AdditionalFunctionNameAttribute"/>
    /// Attribute can be set on class or on function. Function attribute is more important that class attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class ExcludeDefaultFunctionNameAttribute : BaseOperationMarkerAttribute
    {
        public ExcludeDefaultFunctionNameAttribute(bool excludeDefaultFunctionName = true)
        {
            ExcludeDefaultFunctionName = excludeDefaultFunctionName;
        }

        public bool ExcludeDefaultFunctionName
        {
            get;
            private set;
        }
    }
}
