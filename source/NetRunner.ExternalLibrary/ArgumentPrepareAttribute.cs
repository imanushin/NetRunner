using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Allow to modify input text. By default all input text is cleaned up from any html markup. For example, wiki text ''123'' will be transferred to <code><![CDATA[<i>123</i>]]></code> on the html page.
    /// Use <see cref="ArgumentPrepareMode.CleanupHtmlContent"/> to have 123 as input line. And use <see cref="ArgumentPrepareMode.RawHtml"/> to have <code><![CDATA[<i>123</i>]]></code> as input line
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.ReturnValue | AttributeTargets.Parameter,
        Inherited = false, AllowMultiple = false)]
    public sealed class ArgumentPrepareAttribute : Attribute
    {
        internal static readonly ArgumentPrepareAttribute Default = new ArgumentPrepareAttribute(ArgumentPrepareMode.CleanupHtmlContent);

        public enum ArgumentPrepareMode
        {
            /// <summary>
            /// Exclude any html markup from the input cells
            /// </summary>
            CleanupHtmlContent,
            /// <summary>
            /// Provide html code as-is, without any changes
            /// </summary>
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
