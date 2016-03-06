using System.IO;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public interface UploadCallback
    {
        /// <summary>
        /// send file content callback function, be called only once when the file uploaded
        /// </summary>
        /// <param name="output">output stream for writing file content</param>
        /// <returns>0 success, return none zero(errno) if fail</returns>
        int send(Stream output);
    }
}
