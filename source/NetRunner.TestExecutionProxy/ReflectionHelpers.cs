using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.TestExecutionProxy
{
    internal static class ReflectionHelpers
    {
        public static TAttribute FindAttribute<TAttribute>(ParameterInfo parameter, TAttribute defaultValue) 
            where TAttribute : Attribute
        {
            var result = parameter.GetCustomAttribute<TAttribute>();

            if (result != null)
            {
                return result;
            }

            var member = parameter.Member;

            result = member.GetCustomAttribute<TAttribute>();

            if (result != null)
            {
                return result;
            }

            var type = member.DeclaringType;

            result = type.GetCustomAttribute<TAttribute>();

            if (result != null)
            {
                return result;
            }

            var assembly = type.Assembly;

            result = assembly.GetCustomAttribute<TAttribute>();

            if (result != null)
            {
                return result;
            }

            return defaultValue;
        }
    }
}
