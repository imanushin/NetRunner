using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.InternalTests
{
    internal sealed class IndexRange
    {
        public IndexRange(int firstIndex, int lastIndex)
        {
            FirstIndex = firstIndex - 1;
            LastIndex = lastIndex - 1;
        }

        public int FirstIndex
        {
            get;
            private set;
        }

        public int LastIndex
        {
            get;
            private set;
        }

        public static IndexRange Parse(string inputData)
        {
            int singleInt;
            if (int.TryParse(inputData, out singleInt))
            {
                return new IndexRange(singleInt, singleInt);
            }

            var parseException = new ArgumentException(
                 string.Format("Wrong input argument: value shouls be the simple integer (such as 1) or the index range (such as 0..10). Current value: {0}", inputData),
                 "inputData");

            int delimeterIndex = inputData.IndexOf("..", StringComparison.Ordinal);

            if (delimeterIndex < 0)
            {
                throw parseException;
            }

            int firstInt;

            if (!int.TryParse(inputData.Substring(0, delimeterIndex), out firstInt))
            {
                throw parseException;
            }

            int lastInt;

            if (!int.TryParse(inputData.Substring(delimeterIndex + 2), out lastInt))
            {
                throw parseException;
            }

            if (firstInt > lastInt)
            {
                throw new InvalidOperationException(string.Format("First range item should be less or equal than last. First: {0}, last: {1}", firstInt, lastInt));
            }

            return new IndexRange(firstInt, lastInt);
        }
    }
}
