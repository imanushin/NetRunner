using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.Invokation
{
    internal static class ParametersConverter
    {
        public static object ConvertParameter(string inputData, Type expectedType, ReflectionLoader loader)
        {
            try
            {
                return Convert.ChangeType(inputData, expectedType);
            }
            catch (Exception ex)
            {
                throw new ConversionException(expectedType, inputData, ex);
            }
        }
    }
}
