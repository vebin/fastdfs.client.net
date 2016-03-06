using System.Net.Sockets;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class ServerInfo
    {
        protected string ip_addr;
        protected int port;

        public ServerInfo(string ip_addr, int port)
        {
            this.ip_addr = ip_addr;
            this.port = port;
        }

        public string Ip_Addr
        {
            get { return this.ip_addr; }
        }

        public int Port
        {
            get { return this.port; }
        }

        /// <summary>
        /// connect to server
        /// </summary>
        /// <returns>connected TcpClient object</returns>
        public TcpClient connect()
        {
            TcpClient sock = new TcpClient();
            sock.ExclusiveAddressUse = false;
            sock.Connect(this.ip_addr, this.port);
            return sock;
        }
    }
}
