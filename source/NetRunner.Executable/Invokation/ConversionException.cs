using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.Invokation
{
    internal sealed class ConversionException : Exception
    {
        private readonly Type fromType;
        private readonly string inputData;
        private const string messageFormat = "Unable to convert '{0}' to {1}. See inner exception for details.";

        public ConversionException(Type fromType, string inputData, Exception innerException)
            : base(string.Format(messageFormat, inputData, fromType), innerException)
        {
            this.fromType = fromType;
            this.inputData = inputData;
        }

        public string GetCleanedString()
        {
            return string.Format("Unable to convert '{0}' to {1}: {2}", fromType, inputData, messageFormat);
        }
    }
}
