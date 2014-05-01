using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    internal abstract class InternalException : Exception
    {
        [StringFormatMethod("messageFormat")]
        protected InternalException(Exception innerException, string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args), innerException)
        {

        }
    }
}
