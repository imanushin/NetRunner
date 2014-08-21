using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NetRunner.ExternalLibrary;

namespace FitSample
{
    /// <summary>
    /// NetRunnerTestContainer help
    /// <b>bold test</b><br/>
    /// <i>italic test</i><br/>
    /// <script>injection</script>
    /// </summary>
    internal sealed class NetRunnerTestContainer : BaseTestContainer
    {
        public string ConcatAnd(string left, string right)
        {
            return string.Concat(left, right);
        }

        /// <summary>
        /// Summ function
        /// </summary>
        /// <param name="first">Param1 hint</param>
        /// <returns></returns>
        public int SummAndWillBe(int first, int second)
        {
            return first + second;
        }

        /// <summary>
        /// Very positive help
        /// </summary>
        public bool IsPositive(int value)
        {
            return value > 0;
        }

        /// <summary>
        /// Function tries to parse input <see cref="string"/> to the int value
        /// Usage: <br/>
        /// Right: | '''try parse string ''' | 123 | <br/>
        /// Wrong: | '''try parse string ''' | 123 |
        /// <param name="value">String value for parsing</param>
        /// </summary>
        public bool TryParseString(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }

        public bool PingSite(string url)
        {
            if (url == "http://error.com")
            {
                throw new Exception("Error");
            }
            var request = WebRequest.CreateHttp(url);

            using (var responce = request.GetResponse())
            {
                using (var stream = responce.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return !string.IsNullOrWhiteSpace(reader.ReadToEnd());
                    }
                }
            }
        }

        public int StringLengthOfIs(string inputLine)
        {
            return inputLine.Length;
        }

        public int RawStringLengthOfIs([ArgumentPrepare(ArgumentPrepareAttribute.ArgumentPrepareMode.RawHtml)] string inputLine)
        {
            return inputLine.Length;
        }

        public IEnumerable ListNumbersFromTo(int start, int finish)
        {
            return Enumerable.Range(start, finish - start + 1).Select(i => new
            {
                String = i.ToString(),
                Int = i,
                Bool = i % 2 == 0,
                Byte = (byte)i
            });
        }

        public bool PingSiteWithoutExceptions(string url)
        {
            try
            {
                return PingSite(url);
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable ListFilesIn(string path)
        {
            return Directory.EnumerateFiles(path).Select(f => new
            {
                Name = Path.GetFileNameWithoutExtension(f),
                Extension = Path.GetExtension(f)
            });
        }

        public CreateFolderArgument CreateSubfoldersIn(string baseDirectoryPath)
        {
            return new CreateFolderArgument(baseDirectoryPath);
        }

        public RemoveFolderArgument RemoveSubfoldersFrom(string baseDirectoryPath)
        {
            return new RemoveFolderArgument(baseDirectoryPath);
        }

        public IEnumerable<InOutObject> ListInOutObjects(int count)
        {
            return Enumerable.Range(0, count).Select(i => new InOutObject());
        }

        /// <summary>
        /// Checks that a+b=c
        /// </summary>
        /// <param name="a">left argument - a</param>
        /// <param name="b">right argument - b</param>
        /// <param name="c">expected result</param>
        /// <returns></returns>
        public int PlusIs(int a, int b, out int c)
        {
            c = a + b;

            return c;
        }

        public IncorrectEqualityClass GetIncorrectEqualityClass()
        {
            return new IncorrectEqualityClass();
        }

        public IncorrectToStringClass GetIncorrectToStringClass()
        {
            return new IncorrectToStringClass();
        }

        public IEnumerable ListWrongEquality()
        {
            return new[]
            {
                new
                {
                    First=new IncorrectEqualityClass()
                }
            };
        }

        public IEnumerable ListWrongToString()
        {
            return new[]
            {
                new
                {
                    First=new IncorrectToStringClass()
                }
            };
        }

        /// <summary>
        /// Compare generic types
        /// </summary>
        /// <param name="input">Input generic value</param>
        /// <returns>Result generic value</returns>
        public GenericEnumInfo<EnvironmentVariableTarget> CompareEnvironmentVariableTarget(GenericEnumInfo<EnvironmentVariableTarget> input)
        {
            return input;
        }
    }
}
