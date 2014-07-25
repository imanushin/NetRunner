using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigTestsGenerator
{
    internal static class RandomUtils
    {
        internal static readonly Random random = new Random(951369416);

        internal static Type GetRandomType()
        {
            var typeId = random.Next(0, 5);

            switch (typeId)
            {
                case 0:
                    return typeof(bool);
                case 1:
                    return typeof(int);
                case 2:
                    return typeof(string);
                case 3:
                    return typeof(Uri);
                case 4:
                    return typeof(GCCollectionMode);
                case 5:
                    return typeof(MidpointRounding);
                default:
                    throw new ArgumentOutOfRangeException(String.Format("Type id {0} is not supported", typeId));
            }
        }

        internal static string RandomString(int size)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        internal static string RandomSentence(int minWords, int maxWords)
        {
            var wordsCount = random.Next(minWords, maxWords);

            var words = Enumerable.Range(0, wordsCount).Select(i => RandomString(random.Next(3, 8))).ToArray();

            return String.Join(" ", words);
        }
    }
}
