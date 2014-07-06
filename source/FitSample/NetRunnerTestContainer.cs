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
    internal sealed class NetRunnerTestContainer : BaseTestContainer
    {
        public int SummAndWillBe(int first, int second)
        {
            return first + second;
        }

        public bool IsPositive(int value)
        {
            return value > 0;
        }

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
    }
}
