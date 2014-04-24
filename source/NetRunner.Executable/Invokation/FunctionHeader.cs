using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Keywords;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    internal sealed class FunctionHeader : BaseReadOnlyObject
    {
        public FunctionHeader(string functionName, IReadOnlyCollection<string> arguments, HtmlRowReference rowReference, AbstractKeyword keyword)
        {
            Validate.ArgumentStringIsMeanful(functionName, "functionName");
            Validate.ArgumentIsNotNull(arguments, "arguments");
            Validate.ArgumentIsNotNull(keyword, "keyword");

            FunctionName = functionName.Replace(" ", string.Empty);
            Arguments = arguments.ToReadOnlyList();
            RowReference = rowReference;
            Keyword = keyword;
        }

        public HtmlRowReference RowReference
        {
            get;
            private set;
        }

        public AbstractKeyword Keyword
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
            yield return Keyword;
        }

        protected override string GetString()
        {
            return string.Format("FunctionName: {0}, Arguments: {1}, Keyword: {2}", FunctionName, Arguments, Keyword);
        }
    }
}
