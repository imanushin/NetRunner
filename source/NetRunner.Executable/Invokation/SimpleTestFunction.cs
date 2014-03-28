using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation
{
    internal sealed class SimpleTestFunction : AbstractTestFunction
    {
        public SimpleTestFunction(string functionName, IEnumerable<string> arguments)
        {
            FunctionName = functionName;
            Arguments = arguments.ToReadOnlyList();
        }

        public string FunctionName
        {
            get;
            private set;
        }

        public ReadOnlyList<string> Arguments
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return FunctionName;
            yield return Arguments;
        }
    }
}
