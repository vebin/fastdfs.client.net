using fastdfs.client.net;
using System.IO;

namespace fastdfstest
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class DownloadFileWriter : DownloadCallback
    {
        private string filename;
        private FileStream output = null;
        private long current_bytes = 0;

        public DownloadFileWriter(string filename)
        {
            this.filename = filename;
        }

        public int recv(long file_size, byte[] data, int bytes)
        {
            try
            {
                if (this.output == null)
                {
                    this.output = new FileStream(this.filename, FileMode.Create, FileAccess.Write, FileShare.None);
                }

                this.output.Write(data, 0, bytes);
                this.current_bytes += bytes;

                if (this.current_bytes == file_size)
                {
                    this.output.Close();
                    this.output = null;
                    this.current_bytes = 0;
                }
            }
            catch (IOException ex)
            {
                return -1;
            }

            return 0;
        }
    }
}
