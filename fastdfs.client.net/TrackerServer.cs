using System.IO;
using System.Net;
using System.Net.Sockets;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class TrackerServer
    {
        protected TcpClient sock;
        protected IPEndPoint inetSockAddr;

        public TrackerServer(TcpClient sock, IPEndPoint inetSockAddr)
        {
            this.sock = sock;
            this.inetSockAddr = inetSockAddr;
        }

        /// <summary>
        /// get the connected TcpClient
        /// </summary>
        /// <returns>the TcpClient</returns>
        public TcpClient getSocket()
        {
            if (this.sock == null)
            {
                this.sock = ClientGlobal.getSocket(this.inetSockAddr);
            }

            return this.sock;
        }

        public IPEndPoint InetSockAddr
        {
            get
            {
                return this.inetSockAddr;
            }
        }

        public Stream getOutputStream()
        {
            return this.sock.GetStream();
        }

        public Stream getInputStream()
        {
            return this.sock.GetStream();
        }

        public void close()
        {
            if (this.sock != null)
            {
                try
                {
                    ProtoCommon.closeSocket(this.sock);
                }
                finally
                {
                    this.sock = null;
                }
            }
        }
    }
}
