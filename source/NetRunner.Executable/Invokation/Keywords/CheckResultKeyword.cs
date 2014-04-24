using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation.Keywords
{
    internal sealed class CheckResultKeyword: AbstractKeyword
    {
        public override ReadOnlyList<HtmlCell> PatchedCells
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
