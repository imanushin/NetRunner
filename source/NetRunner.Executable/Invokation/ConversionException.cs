using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.Invokation
{
    internal sealed class ConversionException : InternalException
    {
        private const string messageFormat = "Unable to convert '{0}' to {1}. See inner exception for details.";

        public ConversionException(Type fromType, string inputData, Exception innerException)
            : base(innerException, messageFormat, inputData, fromType)
        {
        }
    }
}
