using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable
{
    internal sealed class ApplicationSettings
    {
        public ApplicationSettings(string assemblylist, string host, int port, string socketToken)
        {
            Assemblylist = assemblylist;
            Host = host;
            Port = port;
            SocketToken = socketToken;
        }

        public string Assemblylist
        {
            get;
            private set;
        }

        public string Host
        {
            get;
            private set;
        }
        public int Port
        {
            get;
            private set;
        }
        public string SocketToken
        {
            get;
            private set;
        }
    }
}
