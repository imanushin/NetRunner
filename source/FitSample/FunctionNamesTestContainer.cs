using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace FitSample
{
    [ExcludeDefaultFunctionName]
    internal sealed class FunctionNamesTestContainer : BaseTestContainer
    {
        [AdditionalFunctionName("SomeTestFunction1")]
        public void TestFunction1()
        {
        }

        [AdditionalFunctionName("SomeTestFunction2")]
        [ExcludeDefaultFunctionName(false)]
        public void TestFunction2()
        {
        }
    }
}
