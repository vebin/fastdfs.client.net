using System.Net;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class StorageServer : TrackerServer
    {
        protected int store_path_index = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ip_addr">the ip address of storage server</param>
        /// <param name="port">the port of storage server</param>
        /// <param name="store_path">he store path index on the storage server</param>
        public StorageServer(string ip_addr, int port, int store_path)
            : base(ClientGlobal.getSocket(ip_addr, port), new IPEndPoint(IPAddress.Parse(ip_addr), port))
        {
            this.store_path_index = store_path;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ip_addr">the ip address of storage server</param>
        /// <param name="port">the port of storage server</param>
        /// <param name="store_path">the store path index on the storage server</param>
        public StorageServer(string ip_addr, int port, byte store_path)
            : base(ClientGlobal.getSocket(ip_addr, port), new IPEndPoint(IPAddress.Parse(ip_addr), port))
        {
            if (store_path < 0)
            {
                this.store_path_index = 256 + store_path;
            }
            else
            {
                this.store_path_index = store_path;
            }
        }

        public int Store_Path_Index
        {
            get
            {
                return this.store_path_index;
            }
        }
    }
}
