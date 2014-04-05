namespace NetRunner.Executable
{
    public sealed class TestCounts
    {
        public int SuccessCount
        {
            get;
            private set;
        }

        public int FailCount
        {
            get;
            private set;
        }

        public int ExceptionCount
        {
            get;
            private set;
        }

        public int IgnoreCount
        {
            get;
            private set;
        }

        public void IncrementSuccessCount()
        {
            SuccessCount++;
        }

        public void IncrementFailCount()
        {
            FailCount++;
        }

        public void IncrementExceptionCount()
        {
            ExceptionCount++;
        }

        public void IncrementIgnoreCount()
        {
            IgnoreCount++;
        }
    }
}
