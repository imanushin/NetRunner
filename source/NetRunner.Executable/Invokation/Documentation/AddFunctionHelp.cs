using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal sealed class AddFunctionHelp : BaseCellsHelp
    {
        public AddFunctionHelp(ReadOnlyList<HtmlCell> functionCells, TestFunctionReference function)
            : base(functionCells, DocumentationHtmlHelpers.GetHintAttributeValue(function))
        {
        }
    }
}
