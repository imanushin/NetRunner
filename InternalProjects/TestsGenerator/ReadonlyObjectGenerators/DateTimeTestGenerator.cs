using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestsGenerator.ReadonlyObjectGenerators
{
    internal static class DateTimeTestGenerator
    {
        private const string CheckWrongDateTimeTemplate =
@"
        [TestMethod]
        public void {1}_RestrictDateTime_{0}()
        {{
            Assert.Fail(""Please avoid using DateTime structure"");
        }}
";


        internal static string CreateWrongDateTimeConstructor(
            ParameterInfo param)
        {
            Type targetType = param.Member.DeclaringType;

            return string.Format(CheckWrongDateTimeTemplate, param.Name, targetType.Name);
        }

    }
}
