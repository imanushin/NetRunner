using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;

namespace NetRunner.Executable.Invokation
{
    internal sealed class ParsedTable : BaseReadOnlyObject
    {
        public ParsedTable(HtmlTable table, AbstractTestFunction testFunction, IEnumerable<AbstractTableChange> tableParseInfo)
        {
            Validate.ArgumentIsNotNull(table,"table");
            Validate.ArgumentIsNotNull(testFunction, "testFunction");
            Validate.ArgumentIsNotNull(tableParseInfo, "tableParseInfo");

            Table = table;
            TestFunction = testFunction;
            TableParseInfo = tableParseInfo.ToReadOnlyList();
        }

        public HtmlTable Table
        {
            get;
            private set;
        }

        public AbstractTestFunction TestFunction
        {
            get;
            private set;
        }

        public ReadOnlyList<AbstractTableChange> TableParseInfo
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return TestFunction;
            yield return Table;
            yield return TableParseInfo;
        }
    }
}
