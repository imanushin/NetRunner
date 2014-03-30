using System;
using System.Collections.Generic;
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
            try
            {
                var request = WebRequest.CreateHttp(url);

                var responce = request.GetResponse();

                return responce.ContentLength > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
