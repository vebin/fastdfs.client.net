using System.Net;
using System.Net.Sockets;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class ClientGlobal
    {
        public static int g_connect_timeout; //millisecond
        public static int g_network_timeout; //millisecond
        public static string g_charset;
        public static int g_tracker_http_port;
        public static bool g_anti_steal_token;  //if anti-steal token
        public static string g_secret_key;   //generage token secret key
        public static TrackerGroup g_tracker_group;

        public static int DEFAULT_CONNECT_TIMEOUT = 5;  //second
        public static int DEFAULT_NETWORK_TIMEOUT = 30; //second

        private ClientGlobal()
        {
        }
        /// <summary>
        /// construct TcpClient object
        /// </summary>
        /// <param name="ip_addr">ip address or hostname</param>
        /// <param name="port">port number</param>
        /// <returns>connected TcpClient object</returns>
        public static TcpClient getSocket(string ip_addr, int port)
        {
            TcpClient sock = new TcpClient();
            sock.Connect(ip_addr, port);
            return sock;
        }
        /// <summary>
        /// construct TcpClient object
        /// </summary>
        /// <param name="addr">IPEndPoint object, including ip address and port</param>
        /// <returns>connected TcpClient object</returns>
        public static TcpClient getSocket(IPEndPoint addr)
        {
            TcpClient sock = new TcpClient();
            sock.Connect(addr);
            return sock;
        }

        public static int getG_connect_timeout()
        {
            return g_connect_timeout;
        }

        public static void setG_connect_timeout(int connect_timeout)
        {
            ClientGlobal.g_connect_timeout = connect_timeout;
        }

        public static int getG_network_timeout()
        {
            return g_network_timeout;
        }

        public static void setG_network_timeout(int network_timeout)
        {
            ClientGlobal.g_network_timeout = network_timeout;
        }

        public static string getG_charset()
        {
            return g_charset;
        }

        public static void setG_charset(string charset)
        {
            ClientGlobal.g_charset = charset;
        }

        public static int getG_tracker_http_port()
        {
            return g_tracker_http_port;
        }

        public static void setG_tracker_http_port(int tracker_http_port)
        {
            ClientGlobal.g_tracker_http_port = tracker_http_port;
        }

        public static bool getG_anti_steal_token()
        {
            return g_anti_steal_token;
        }

        public static bool isG_anti_steal_token()
        {
            return g_anti_steal_token;
        }

        public static void setG_anti_steal_token(bool anti_steal_token)
        {
            ClientGlobal.g_anti_steal_token = anti_steal_token;
        }

        public static string getG_secret_key()
        {
            return g_secret_key;
        }

        public static void setG_secret_key(string secret_key)
        {
            ClientGlobal.g_secret_key = secret_key;
        }

        public static TrackerGroup getG_tracker_group()
        {
            return g_tracker_group;
        }

        public static void setG_tracker_group(TrackerGroup tracker_group)
        {
            ClientGlobal.g_tracker_group = tracker_group;
        }

        public static void init(string conf_filename)
        {
            IniFileReader iniReader;
            string[] szTrackerServers;
            string[] parts;

            iniReader = new IniFileReader(conf_filename);

            g_connect_timeout = iniReader.getIntValue("connect_timeout", DEFAULT_CONNECT_TIMEOUT);
            if (g_connect_timeout < 0)
            {
                g_connect_timeout = DEFAULT_CONNECT_TIMEOUT;
            }
            g_connect_timeout *= 1000; //millisecond

            g_network_timeout = iniReader.getIntValue("network_timeout", DEFAULT_NETWORK_TIMEOUT);
            if (g_network_timeout < 0)
            {
                g_network_timeout = DEFAULT_NETWORK_TIMEOUT;
            }
            g_network_timeout *= 1000; //millisecond

            g_charset = iniReader.getStrValue("charset");
            if (g_charset == null || g_charset.Length == 0)
            {
                g_charset = "ISO8859-1";
            }

            szTrackerServers = iniReader.getValues("tracker_server");
            if (szTrackerServers == null)
            {
                throw new MyException("item \"tracker_server\" in " + conf_filename + " not found");
            }

            IPEndPoint[] tracker_servers = new IPEndPoint[szTrackerServers.Length];
            for (int i = 0; i < szTrackerServers.Length; i++)
            {
                parts = szTrackerServers[i].Split("\\:".ToCharArray(), 2);
                if (parts.Length != 2)
                {
                    throw new MyException("the value of item \"tracker_server\" is invalid, the correct format is host:port");
                }

                tracker_servers[i] = new IPEndPoint(IPAddress.Parse(parts[0].Replace("\0", "").Trim()), int.Parse(parts[1].Replace("\0", "").Trim()));
            }
            g_tracker_group = new TrackerGroup(tracker_servers);

            g_tracker_http_port = iniReader.getIntValue("http.tracker_http_port", 80);
            g_anti_steal_token = iniReader.getBoolValue("http.anti_steal_token", false);
            if (g_anti_steal_token)
            {
                g_secret_key = iniReader.getStrValue("http.secret_key");
            }
        }
    }
}
