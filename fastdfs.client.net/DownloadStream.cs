using System.IO;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class DownloadStream : DownloadCallback
    {
        private Stream output;
        private long currentBytes = 0;

        public DownloadStream(Stream output)
        {
            this.output = output;
        }

        public int recv(long file_size, byte[] data, int bytes)
        {
            try
            {
                output.Write(data, 0, bytes);
            }
            catch (IOException ex)
            {
                return -1;
            }

            currentBytes += bytes;
            if (this.currentBytes == file_size)
            {
                this.currentBytes = 0;
            }

            return 0;
        }
    }
}
