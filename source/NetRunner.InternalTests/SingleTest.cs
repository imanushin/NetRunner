using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.InternalTests
{
    internal sealed class SingleTest
    {
        public SingleTest(string testName, IReadOnlyDictionary<string, int> counts, string contents)
        {
            TestName = testName;
            Counts = counts;
            Contents = contents;
        }

        public string TestName
        {
            get;
            private set;
        }

        public IReadOnlyDictionary<string, int> Counts
        {
            get;
            private set;
        }

        public string Contents
        {
            get;
            private set;
        }
    }
}
