using fastdfs.client.net;
using System.IO;

namespace fastdfstest
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class UploadLocalFileSender : UploadCallback
    {
        private string local_filename;

        public UploadLocalFileSender(string szLocalFilename)
        {
            this.local_filename = szLocalFilename;
        }

        public int send(Stream output)
        {
            FileStream fis;
            int readBytes;
            byte[] buff = new byte[256 * 1024];

            fis = new FileStream(this.local_filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                while ((readBytes = fis.Read(buff, 0, buff.Length)) >= 0)
                {
                    if (readBytes == 0)
                    {
                        break;
                    }

                    output.Write(buff, 0, readBytes);
                }
            }
            finally
            {
                fis.Close();
            }

            return 0;
        }
    }
}
