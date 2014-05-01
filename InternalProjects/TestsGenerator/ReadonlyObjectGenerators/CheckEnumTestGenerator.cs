using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestsGenerator.ReadonlyObjectGenerators
{
    internal static class CheckEnumTestGenerator
    {
        private const string CheckWrongEnumConstructorTestTemplate =
@"
        [TestMethod]
        public void {2}_CheckWrongEnumArg_{0}()
        {{
{1}

            try
            {{
                new {2}({3});
            }}
            catch(ArgumentException ex)
            {{
                CheckArgumentExceptionParameter( ""{0}"", ex.ParamName );

                return;
            }}

            Assert.Fail(""Enumeration in the argument '{0}' isn't checked for the not-defined values"");
        }}
";


        internal static string CreateWrongEnumConstructor(
            ParameterInfo param,
            string argumentsList,
            string initialization)
        {
            Type parameterType = param.ParameterType;

            int wrongValue = Enum.GetValues(parameterType).Cast<int>().Max() + 1;

            const string replacementTemplate = "({0}){1}";

            string replacement = string.Format(replacementTemplate, EnumTestsGenerator.GetEnumName(parameterType), wrongValue);

            Type targetType = param.Member.DeclaringType;

            string currentArgs = argumentsList.Replace(param.Name, replacement);

            return string.Format(CheckWrongEnumConstructorTestTemplate, param.Name, initialization, targetType.Name, currentArgs);
        }

    }
}
