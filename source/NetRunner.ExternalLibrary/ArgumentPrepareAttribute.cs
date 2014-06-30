using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.ReturnValue | AttributeTargets.Parameter,
        Inherited = false, AllowMultiple = false)]
    public sealed class ArgumentPrepareAttribute : Attribute
    {
        public static readonly ArgumentPrepareAttribute Default = new ArgumentPrepareAttribute(ArgumentPrepareMode.CleanupHtmlContent);

        public enum ArgumentPrepareMode
        {
            CleanupHtmlContent,
            RawHtml
        }

        // This is a positional argument
        public ArgumentPrepareAttribute(ArgumentPrepareMode mode)
        {
            Mode = mode;
        }

        public ArgumentPrepareMode Mode
        {
            get;
            private set;
        }
    }
}
