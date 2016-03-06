using System;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class StorageClientEx : StorageClient
    {
        public static string SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR = "/";

        public StorageClientEx()
            : base()
        {

        }

        public StorageClientEx(TrackerServer trackerServer, StorageServer storageServer)
            : base(trackerServer, storageServer)
        {

        }

        public static byte split_file_id(string file_id, string[] results)
        {
            int pos = file_id.IndexOf(SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR);
            if ((pos <= 0) || (pos == file_id.Length - 1))
            {
                return ProtoCommon.ERR_NO_EINVAL;
            }

            results[0] = file_id.Substring(0, pos); //group name
            results[1] = file_id.Substring(pos + 1); //file name
            return 0;
        }
        /// <summary>
        /// upload file to storage server (by file name)
        /// </summary>
        /// <param name="local_filename">local filename to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.), null to extract ext name from the local filename</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>file id(including group name and filename) if success, return null if fail</returns>
        public string upload_file1(string local_filename, string file_ext_name, NameValuePair[] meta_list)
        {
            string[] parts = this.upload_file(local_filename, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// upload file to storage server (by file name)
        /// </summary>
        /// <param name="group_name"></param>
        /// <param name="local_filename"></param>
        /// <param name="file_ext_name"></param>
        /// <param name="meta_list"></param>
        /// <returns></returns>
        public string upload_file1(string group_name, string local_filename, string file_ext_name, NameValuePair[] meta_list)
        {
            string[] parts = this.upload_file(group_name, local_filename, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// upload file to storage server (by file buff)
        /// </summary>
        /// <param name="file_buff"></param>
        /// <param name="file_ext_name"></param>
        /// <param name="meta_list"></param>
        /// <returns></returns>
        public string upload_file1(byte[] file_buff, string file_ext_name, NameValuePair[] meta_list)
        {
            string[] parts = this.upload_file(file_buff, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// upload file to storage server (by file buff)
        /// </summary>
        /// <param name="group_name"></param>
        /// <param name="file_buff"></param>
        /// <param name="file_ext_name"></param>
        /// <param name="meta_list"></param>
        /// <returns></returns>
        public string upload_file1(string group_name, byte[] file_buff, string file_ext_name, NameValuePair[] meta_list)
        {
            string[] parts = this.upload_file(group_name, file_buff, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public string upload_file1(string group_name, long file_size,
       UploadCallback callback, string file_ext_name,
       NameValuePair[] meta_list)
        {
            string[] parts = this.upload_file(group_name, file_size, callback, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public string upload_appender_file1(string local_filename, string file_ext_name,
       NameValuePair[] meta_list)
        {
            string[] parts = this.upload_appender_file(local_filename, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public String upload_appender_file1(string group_name, string local_filename, string file_ext_name,
       NameValuePair[] meta_list)
        {
            string[] parts = this.upload_appender_file(group_name, local_filename, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public string upload_appender_file1(byte[] file_buff, string file_ext_name,
       NameValuePair[] meta_list)
        {
            string[] parts = this.upload_appender_file(file_buff, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public string upload_appender_file1(string group_name, byte[] file_buff, string file_ext_name,
       NameValuePair[] meta_list)
        {
            string[] parts = this.upload_appender_file(group_name, file_buff, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public string upload_appender_file1(string group_name, long file_size,
       UploadCallback callback, string file_ext_name,
       NameValuePair[] meta_list)
        {
            string[] parts = this.upload_appender_file(group_name, file_size, callback, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public string upload_file1(string master_file_id, string prefix_name,
       string local_filename, string file_ext_name, NameValuePair[] meta_list)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(master_file_id, parts);
            if (this.errno != 0)
            {
                return null;
            }

            parts = this.upload_file(parts[0], parts[1], prefix_name,
                                              local_filename, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public string upload_file1(string master_file_id, string prefix_name,
       byte[] file_buff, string file_ext_name, NameValuePair[] meta_list)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(master_file_id, parts);
            if (this.errno != 0)
            {
                return null;
            }

            parts = this.upload_file(parts[0], parts[1], prefix_name, file_buff, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public string upload_file1(string master_file_id, string prefix_name,
       byte[] file_buff, int offset, int length, string file_ext_name,
       NameValuePair[] meta_list)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(master_file_id, parts);
            if (this.errno != 0)
            {
                return null;
            }

            parts = this.upload_file(parts[0], parts[1], prefix_name, file_buff,
                         offset, length, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public string upload_file1(string master_file_id, string prefix_name, long file_size,
       UploadCallback callback, string file_ext_name,
       NameValuePair[] meta_list)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(master_file_id, parts);
            if (this.errno != 0)
            {
                return null;
            }

            parts = this.upload_file(parts[0], parts[1], prefix_name, file_size, callback, file_ext_name, meta_list);
            if (parts != null)
            {
                return parts[0] + SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + parts[1];
            }
            else
            {
                return null;
            }
        }

        public int append_file1(string appender_file_id, string local_filename)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.append_file(parts[0], parts[1], local_filename);
        }

        public int append_file1(string appender_file_id, byte[] file_buff)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.append_file(parts[0], parts[1], file_buff);
        }

        public int append_file1(string appender_file_id, byte[] file_buff, int offset, int length)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.append_file(parts[0], parts[1], file_buff, offset, length);
        }

        public int append_file1(string appender_file_id, long file_size, UploadCallback callback)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.append_file(parts[0], parts[1], file_size, callback);
        }

        public int modify_file1(string appender_file_id,
        long file_offset, string local_filename)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.modify_file(parts[0], parts[1], file_offset, local_filename);
        }

        public int modify_file1(string appender_file_id,
        long file_offset, byte[] file_buff)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.modify_file(parts[0], parts[1], file_offset, file_buff);
        }

        public int modify_file1(string appender_file_id,
       long file_offset, byte[] file_buff, int buffer_offset, int buffer_length)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.modify_file(parts[0], parts[1], file_offset,
                    file_buff, buffer_offset, buffer_length);
        }

        public int modify_file1(string appender_file_id,
       long file_offset, long modify_size, UploadCallback callback)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.modify_file(parts[0], parts[1], file_offset, modify_size, callback);
        }

        public int delete_file1(string file_id)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.delete_file(parts[0], parts[1]);
        }

        public int truncate_file1(string appender_file_id)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.truncate_file(parts[0], parts[1]);
        }

        public int truncate_file1(string appender_file_id, long truncated_file_size)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(appender_file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.truncate_file(parts[0], parts[1], truncated_file_size);
        }

        public byte[] download_file1(string file_id)
        {
            long file_offset = 0;
            long download_bytes = 0;

            return this.download_file1(file_id, file_offset, download_bytes);
        }

        public byte[] download_file1(string file_id, long file_offset, long download_bytes)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(file_id, parts);
            if (this.errno != 0)
            {
                return null;
            }

            return this.download_file(parts[0], parts[1], file_offset, download_bytes);
        }

        public int download_file1(string file_id, string local_filename)
        {
            long file_offset = 0;
            long download_bytes = 0;

            return this.download_file1(file_id, file_offset, download_bytes, local_filename);
        }

        public int download_file1(string file_id, long file_offset, long download_bytes, string local_filename)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.download_file(parts[0], parts[1], file_offset, download_bytes, local_filename);
        }

        public int download_file1(string file_id, DownloadCallback callback)
        {
            long file_offset = 0;
            long download_bytes = 0;

            return this.download_file1(file_id, file_offset, download_bytes, callback);
        }

        public int download_file1(string file_id, long file_offset, long download_bytes, DownloadCallback callback)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.download_file(parts[0], parts[1], file_offset, download_bytes, callback);
        }

        public NameValuePair[] get_metadata1(string file_id)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(file_id, parts);
            if (this.errno != 0)
            {
                return null;
            }

            return this.get_metadata(parts[0], parts[1]);
        }

        public int set_metadata1(string file_id, NameValuePair[] meta_list, byte op_flag)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(file_id, parts);
            if (this.errno != 0)
            {
                return this.errno;
            }

            return this.set_metadata(parts[0], parts[1], meta_list, op_flag);
        }

        public FileInfo query_file_info1(string file_id)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(file_id, parts);
            if (this.errno != 0)
            {
                return null;
            }

            return this.query_file_info(parts[0], parts[1]);
        }

        public FileInfo get_file_info1(string file_id)
        {
            string[] parts = new string[2];
            this.errno = split_file_id(file_id, parts);
            if (this.errno != 0)
            {
                return null;
            }

            return this.get_file_info(parts[0], parts[1]);
        }
    }
}
