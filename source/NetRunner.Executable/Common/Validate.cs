using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Common
{
    internal static class Validate
    {
        [StringFormatMethod("messageFormat")]
        public static void AreEqual(int first, int second, string messageFormat, params object[] args)
        {
            Condition(first == second, messageFormat, args);
        }

        [StringFormatMethod("messageFormat")]
        [ContractAnnotation("condition:false => halt")]
        public static void Condition(bool condition, string messageFormat, params object[] args)
        {
            if (!condition)
                throw new InvalidOperationException(string.Format(messageFormat, args));
        }

        [StringFormatMethod("messageFormat")]
        [ContractAnnotation("argumentCondition:false => halt")]
        public static void ArgumentCondition(bool argumentCondition, [InvokerParameterNameAttribute] string argumentName, string messageFormat, params object[] args)
        {
            if (!argumentCondition)
                throw new ArgumentException(string.Format(messageFormat, args), argumentName);
        }

        [StringFormatMethod("messageFormat")]
        public static void ArgumentIsNotNull(object argument, [InvokerParameterNameAttribute()] string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void CollectionArgumentHasElements<TValue>(IEnumerable<TValue> elements, [InvokerParameterNameAttribute] string argumentName)
        {
            ArgumentIsNotNull(elements, argumentName);
            ArgumentCondition(elements.Any(), argumentName, "Collection of type {0}<{1}> does not contain elements", elements.GetType().Name, typeof(TValue));
        }
    }
}
