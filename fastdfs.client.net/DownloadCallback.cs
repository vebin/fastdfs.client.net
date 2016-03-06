
namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public interface DownloadCallback
    {
        /// <summary>
        /// recv file content callback function, may be called more than once when the file downloaded
        /// </summary>
        /// <param name="file_size">file size</param>
        /// <param name="data">data buff</param>
        /// <param name="bytes">data bytes</param>
        /// <returns>0 success, return none zero(errno) if fail</returns>
        int recv(long file_size, byte[] data, int bytes);
    }
}
