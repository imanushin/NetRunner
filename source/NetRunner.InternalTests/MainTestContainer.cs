using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.ExternalLibrary;

namespace NetRunner.InternalTests
{
    internal sealed class MainTestContainer : BaseTestContainer
    {
        private Uri pathToFitnesseRoot;

        private List<HtmlNode> tables;

        public bool SetFintessePath(Uri pathToRoot)
        {
            try
            {
                var request = WebRequest.CreateHttp(pathToRoot);

                using (var responce = request.GetResponse())
                {
                    using (var stream = responce.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to access to {0}: {1}", pathToRoot, ex);

                return false;
            }

            pathToFitnesseRoot = pathToRoot;

            return true;
        }

        public bool ExecutePage(string pageLocalUrl)
        {
            /**/
            return false;
        }

        public int ResultTableCount()
        {
            if (tables == null)
            {
                Trace.TraceError("No tests were executed");

                return -1;
            }

            return tables.Count;
        }
    }
}
