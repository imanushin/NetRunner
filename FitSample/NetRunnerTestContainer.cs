using System;
using System.Collections;
using System.Collections.Generic;
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
        public bool PingSite(string url)
        {
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

        public IEnumerable ListFilesIn(string path)
        {
            return Directory.EnumerateFiles(path).Select(f => new
            {
                Name = Path.GetFileNameWithoutExtension(f),
                Extension = Path.GetExtension(f)
            });
        }
    }
}
