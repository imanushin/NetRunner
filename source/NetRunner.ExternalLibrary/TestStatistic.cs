using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Test statistic of the functions executed.
    /// This is mutable class
    /// </summary>
    public sealed class TestStatistic
    {
        [UsedImplicitly]
        internal static readonly TestStatistic SuiteStatisticInternal = new TestStatistic();

        [UsedImplicitly]
        internal static readonly TestStatistic CurrentTestStatisticInternal = new TestStatistic();

        private TestStatistic()
        {
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
        /// Common suite statistic. This field is updated after each table execution.
        /// </summary>
        public static TestStatistic SuiteStatistic
        {
            get
            {
                return SuiteStatisticInternal;
            }
        }

        /// <summary>
        /// Statistic of the currently executed test.  This field is updated after each table execution.
        /// </summary>
        public static TestStatistic CurrentTestStatistic
        {
            get
            {
                return CurrentTestStatisticInternal;
            }
        }

        /// <summary>
        /// Obsolete property. Please use <see cref="CurrentTestStatistic"/> or <see cref="SuiteStatistic"/> instead of your requirements.
        /// </summary>
        [Obsolete("Please use CurrentTestStatistic or SuiteStatistic", true)]
        public static TestStatistic GlobalStatistic
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        internal void Update(int right, int wrong, int skipped, int errors)
        {
            Right = right;
            Wrong = wrong;
            Skipped = skipped;
            Errors = errors;
        }
    }
}
