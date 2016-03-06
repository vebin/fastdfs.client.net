using System.IO;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class UploadStream:UploadCallback
    {
        private Stream inputStream; //input stream for reading
        private long fileSize = 0;  //size of the uploaded file

        public UploadStream(Stream inputStream, long fileSize)
        {
            this.inputStream = inputStream;
            this.fileSize = fileSize;
        }

        public int send(Stream output)
        {
            long remainBytes = fileSize;
            byte[] buff = new byte[256 * 1024];
            int bytes;
            while (remainBytes > 0)
            {
                try
                {
                    if ((bytes = inputStream.Read(buff, 0, remainBytes > buff.Length ? buff.Length : (int)remainBytes)) < 0)
                    {
                        return -1;
                    }
                }
                catch (IOException ex)
                {
                    return -1;
                }

                output.Write(buff, 0, bytes);
                remainBytes -= bytes;
            }

            return 0;
        }
    }
}
