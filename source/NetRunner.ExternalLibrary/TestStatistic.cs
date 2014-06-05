using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    public sealed class TestStatistic
    {
        [UsedImplicitly]
        internal static TestStatistic GlobalStatisticInternal = new TestStatistic(0, 0, 0, 0);

        public TestStatistic(int right, int wrong, int skipped, int errors)
        {
            Right = right;
            Wrong = wrong;
            Skipped = skipped;
            Errors = errors;
        }

        public int Right
        {
            get;
            private set;
        }

        public int Wrong
        {
            get;
            private set;
        }

        public int Skipped
        {
            get;
            private set;
        }

        public int Errors
        {
            get;
            private set;
        }

        public static TestStatistic GlobalStatistic
        {
            get
            {
                return GlobalStatisticInternal;
            }
        }
    }
}
