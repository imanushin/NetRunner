using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.InternalTests
{
    internal sealed class IndexRange
    {
        private readonly Tuple<int, int>[] subranges;

        public IndexRange(IEnumerable<Tuple<int, int>> ranges)
        {
            subranges = ranges.ToArray();
        }

        public bool Exists(int index)
        {
            return subranges.Any(s => s.Item1 <= index && index <= s.Item2);
        }

        public static IndexRange Parse(string inputData)
        {
            var ranges = inputData.Split(new[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

            var result = new List<Tuple<int, int>>();

            var parseException = new ArgumentException(
                 string.Format("Wrong input argument: value shouls be the simple integer (such as 1) or the index range (such as 0..10). Ranges can be comma delimeted, like '1, 3..5, 8, 10..100'. Current value: {0}", inputData),
                 "inputData");

            foreach (string range in ranges)
            {
                int singleInt;
                if (int.TryParse(range, out singleInt))
                {
                    return new IndexRange(new[] { new Tuple<int, int>(singleInt - 1, singleInt - 1) });
                }

                int delimeterIndex = range.IndexOf("..", StringComparison.Ordinal);

                if (delimeterIndex < 0)
                {
                    throw parseException;
                }

                int firstInt;

                if (!int.TryParse(range.Substring(0, delimeterIndex), out firstInt))
                {
                    throw parseException;
                }

                int lastInt;

                if (!int.TryParse(range.Substring(delimeterIndex + 2), out lastInt))
                {
                    throw parseException;
                }

                if (firstInt > lastInt)
                {
                    throw new InvalidOperationException(string.Format("First range item should be less or equal than last. First: {0}, last: {1}", firstInt, lastInt));
                }

                result.Add(new Tuple<int, int>(firstInt - 1, lastInt - 1));
            }

            return new IndexRange(result);
        }
    }
}
