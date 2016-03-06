using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class TrackerGroup:ICloneable
    {
        protected object obj;
        public int tracker_server_index;
        public IPEndPoint[] tracker_servers;

        public TrackerGroup(IPEndPoint[] tracker_servers)
        {
            this.tracker_servers = tracker_servers;
            this.obj = new object();
            this.tracker_server_index = 0;
        }

        /// <summary>
        ///return connected tracker server
        /// </summary>
        /// <param name="serverIndex"></param>
        /// <returns>connected tracker server, null for fail</returns>
        public TrackerServer getConnection(int serverIndex)
        {
            TcpClient sock = new TcpClient();
            sock.ExclusiveAddressUse = false;
            sock.Connect(this.tracker_servers[serverIndex]);
            return new TrackerServer(sock, this.tracker_servers[serverIndex]);
        }
        /// <summary>
        /// return connected tracker server
        /// </summary>
        /// <returns>connected tracker server, null for fail</returns>
        public TrackerServer getConnection()
        {
            int current_index;

            lock (this.obj)
            {
                this.tracker_server_index++;
                if (this.tracker_server_index >= this.tracker_servers.Length)
                {
                    this.tracker_server_index = 0;
                }

                current_index = this.tracker_server_index;
            }

            try
            {
                return this.getConnection(current_index);
            }
            catch (IOException ex)
            {
                Console.WriteLine("connect to server " + this.tracker_servers[current_index].Address + ":" + this.tracker_servers[current_index].Port + " fail");
            }

            for (int i = 0; i < this.tracker_servers.Length; i++)
            {
                if (i == current_index)
                {
                    continue;
                }

                try
                {
                    TrackerServer trackerServer = this.getConnection(i);

                    lock (this.obj)
                    {
                        if (this.tracker_server_index == current_index)
                        {
                            this.tracker_server_index = i;
                        }
                    }

                    return trackerServer;
                }
                catch (IOException ex)
                {
                    Console.WriteLine("connect to server " + this.tracker_servers[i].Address + ":" + this.tracker_servers[i].Port + " fail");
                }
            }

            return null;
        }

        public object Clone()
        {
            IPEndPoint[] trackerServers = new IPEndPoint[this.tracker_servers.Length];
            for (int i = 0; i < trackerServers.Length; i++)
            {
                trackerServers[i] = new IPEndPoint(this.tracker_servers[i].Address, this.tracker_servers[i].Port);
            }

            return new TrackerGroup(trackerServers);
        }
    }
}
