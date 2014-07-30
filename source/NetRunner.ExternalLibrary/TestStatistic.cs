using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Test statistic of the functions executed.
    /// This is readonly class, to use property <see cref="GlobalStatistic"/> to get the latest execution status
    /// </summary>
    public sealed class TestStatistic
    {
        [UsedImplicitly]
        internal static TestStatistic GlobalStatisticInternal = new TestStatistic(0, 0, 0, 0);

        internal TestStatistic(int right, int wrong, int skipped, int errors)
        {
            Right = right;
            Wrong = wrong;
            Skipped = skipped;
            Errors = errors;
        }

        /// <summary>
        /// Count of right tests
        /// </summary>
        public int Right
        {
            get;
            private set;
        }

        /// <summary>
        /// Count of wrong tests (e.g. tests which executed without exception and returned incorrect result)
        /// </summary>
        public int Wrong
        {
            get;
            private set;
        }

        /// <summary>
        /// Skipped tests count
        /// </summary>
        public int Skipped
        {
            get;
            private set;
        }

        /// <summary>
        /// Count of tests executed with exception
        /// </summary>
        public int Errors
        {
            get;
            private set;
        }

        /// <summary>
        /// Current statistic. After each test invokation it is changed to the other, so read this property to get the latest information
        /// </summary>
        public static TestStatistic GlobalStatistic
        {
            get
            {
                return GlobalStatisticInternal;
            }
        }
    }
}
