using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal sealed class AddCellPropertyHelp : BaseCellsHelp
    {
        public AddCellPropertyHelp(HtmlCell cell, PropertyReference property)
            : base(new ReadOnlyList<HtmlCell>(new[] { cell }), DocumentationHtmlHelpers.GetHintAttributeValue(property))
        {
        }
    }
}
