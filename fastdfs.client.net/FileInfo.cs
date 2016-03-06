using System;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class FileInfo
    {
        protected string source_ip_addr;
        protected long file_size;
        protected DateTime create_timestamp;
        protected int crc32;

        public FileInfo(long file_size, int create_timestamp, int crc32, string source_ip_addr)
        {
            this.file_size = file_size;
            //this.create_timestamp = new DateTime(create_timestamp * 1000L);
            this.create_timestamp = ProtoCommon.UnixTimestampToDateTime(create_timestamp);
            this.crc32 = crc32;
            this.source_ip_addr = source_ip_addr;
        }

        public string Source_Ip_Addr
        {
            get { return this.source_ip_addr; }
            set { this.source_ip_addr = value; }
        }

        public long File_Size
        {
            get { return this.file_size; }
            set { this.file_size = value; }
        }

        public DateTime Create_Timestamp
        {
            get { return this.create_timestamp; }
            set { this.create_timestamp = value; }
        }

        public int Crc32
        {
            get { return this.crc32; }
            set { this.crc32 = value; }
        }

        public override string ToString()
        {
            return "source_ip_addr = " + this.source_ip_addr + ", " +
                    "file_size = " + this.file_size + ", " +
                    "create_timestamp = " + this.create_timestamp.ToString("yyyy-MM-dd HH:mm:ss") + ", " +
                    "crc32 = " + this.crc32;
        }
    }
}
