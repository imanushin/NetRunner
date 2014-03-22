using System;
using System.Collections.Generic;
using System.Text;

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

        public static string FormatInteger(int encodeInteger)
        {
            string numberPartOfString = "" + encodeInteger;
            return new String('0', 10 - numberPartOfString.Length) + numberPartOfString;
        }

    }
}
