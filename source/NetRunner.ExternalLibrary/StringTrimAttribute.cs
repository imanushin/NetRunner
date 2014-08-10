using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Trim input string by default or not. False by default
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.ReturnValue | AttributeTargets.Parameter,
    Inherited = false, AllowMultiple = false)]
    public sealed class StringTrimAttribute : BaseOperationMarkerAttribute
    {
        internal static readonly StringTrimAttribute Default = new StringTrimAttribute(false);

        public StringTrimAttribute(bool trimInputString = true)
        {
            this.TrimInputString = trimInputString;
        }

        public bool TrimInputString
        {
            get;
            private set;
        }
    }
}
