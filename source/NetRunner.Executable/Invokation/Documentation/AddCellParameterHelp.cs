using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal sealed class AddCellParameterHelp : BaseCellsHelp
    {
        public AddCellParameterHelp(HtmlCell parameterCell, ParameterInfoReference argument)
            : base(new ReadOnlyList<HtmlCell>(parameterCell), DocumentationHtmlHelpers.GetHintAttributeValue(argument))
        {
        }
    }
}
