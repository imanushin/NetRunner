using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestsGenerator.ReadonlyObjectGenerators
{
    internal static class StringTestGenerator
    {
        private static int indexer;

        private const string checkEmptyStringConstructorTestTemplate =
@"
        [TestMethod]
        public void {2}_CheckEmptyStringArg_{0}{4}()
        {{
            CheckEmptyStringArg_{0}{4}(string.Empty);
            CheckEmptyStringArg_{0}{4}(""    "");
            CheckEmptyStringArg_{0}{4}(Environment.NewLine);
            CheckEmptyStringArg_{0}{4}(""\n\r"");
        }}

        private void CheckEmptyStringArg_{0}{4}(string stringArgument)
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

            Assert.Fail(""String in the argument '{0}' isn't checked for emply values"");
        }}
";


        internal static string CreateEmptyStringConstructor(
            bool skipOverloading, 
            ParameterInfo param,
            string argumentsList,
            string initialization)
        {
            const string replacement = "stringArgument";

            Type targetType = param.Member.DeclaringType;

            string methodSuffix;

            if (skipOverloading)
            {
                methodSuffix = string.Empty;
            }
            else
            {
                methodSuffix = "_" + indexer++;
            }

            string currentArgs = argumentsList.Replace(param.Name, replacement);

            return string.Format(checkEmptyStringConstructorTestTemplate, param.Name, initialization, targetType.Name, currentArgs, methodSuffix);
        }

    }
}
