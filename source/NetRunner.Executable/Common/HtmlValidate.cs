using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Common
{
    public static class HtmlValidate
    {
        [StringFormatMethod("messageFormat")]
        [ContractAnnotation("argument:null => halt")]
        public static void ArgumentTagHasName(HtmlNode argument, string targetName, [InvokerParameterNameAttribute()] string argumentName)
        {
            Validate.ArgumentIsNotNull(argument, argumentName);
            Validate.ArgumentStringIsMeanful(targetName, "targetName");
            Validate.ArgumentCondition(string.Equals(argument.Name, targetName, StringComparison.OrdinalIgnoreCase), argumentName, "Tag '{0}' should have name '{1}'", argument.Name, targetName);
        }

        [StringFormatMethod("messageFormat")]
        [ContractAnnotation("argument:null => halt")]
        public static void ArgumentIntGreaterOrEqualZero(int argument, [InvokerParameterNameAttribute()] string argumentName)
        {
            Validate.ArgumentCondition(argument >= 0, argumentName, "Argument {0} has value {1} which is less than zero", argumentName, argument);
        }

        [StringFormatMethod("messageFormat")]
        [ContractAnnotation("argument:null => halt")]
        public static void ArgumentIntLessThan(int argument, int limitValue, [InvokerParameterNameAttribute()] string argumentName)
        {
            Validate.ArgumentCondition(argument < limitValue, argumentName, "Argument {0} should be less than {2}. Current value: {1}", argumentName, argument, limitValue);
        }

        [StringFormatMethod("messageFormat")]
        public static void CollectionArgumentHasElements<TValue>(IEnumerable<TValue> elements, [InvokerParameterNameAttribute] string argumentName)
        {
            Validate.ArgumentIsNotNull(elements, argumentName);
            Validate.ArgumentCondition(elements.Any(), argumentName, "Collection of type {0}<{1}> does not contain elements", elements.GetType().Name, typeof(TValue));
        }
    }
}
