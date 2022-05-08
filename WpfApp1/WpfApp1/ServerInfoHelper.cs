using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class ServerInfoHelper
    {
        private static string server = "imap.gmx.net";
        public static string Server {

            get => server;
        }

        private static int port = 993;
        public static int Port
        {
            get => port;
        }

        private static bool ssl = true;
        public static bool SSL
        {
            get => ssl;
        }
    }
}
