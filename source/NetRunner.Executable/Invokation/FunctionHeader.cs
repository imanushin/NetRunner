using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation
{
    internal sealed class FunctionHeader  : BaseReadOnlyObject
    {
        public FunctionHeader(string functionName, IReadOnlyCollection<string> arguments, HtmlRowReference rowReference)
        {
            RowReference = rowReference;
            Validate.ArgumentStringIsMeanful(functionName, "functionName");
            Validate.CollectionArgumentHasElements(arguments, "arguments");

            FunctionName = functionName.Replace(" ", string.Empty);
            Arguments = arguments.ToReadOnlyList();
        }

        public HtmlRowReference RowReference
        {
            get;
            private set;
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

        protected override string GetString()
        {
            return string.Format("FunctionName: {0}, Arguments: {1}", FunctionName, Arguments);
        }
    }
}
