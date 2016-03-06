using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class StorageClient
    {
        public class UploadBuff : UploadCallback
        {
            private byte[] fileBuff;
            private int offset;
            private int length;

            public UploadBuff(byte[] fileBuff, int offset, int length)
            {
                this.fileBuff = fileBuff;
                this.offset = offset;
                this.length = length;
            }

            public int send(Stream output)
            {
                output.Write(this.fileBuff, this.offset, this.length);

                return 0;
            }
        }

        protected TrackerServer trackerServer;
        protected StorageServer storageServer;
        protected byte errno;
        public static Base64 base64 = new Base64('-', '_', '.', 0);

        public StorageClient()
        {
            this.trackerServer = null;
            this.storageServer = null;
        }

        public StorageClient(TrackerServer trackerServer, StorageServer storageServer)
        {
            this.trackerServer = trackerServer;
            this.storageServer = storageServer;
        }

        public byte ErrorCode
        {
            get
            {
                return this.errno;
            }
        }

        /// <summary>
        /// upload file to storage server (by file name)
        /// </summary>
        /// <param name="local_filename">local filename to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.), null to extract ext name from the local filename</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(string local_filename, string file_ext_name, NameValuePair[] meta_list)
        {
            string group_name = null;
            return this.upload_file(group_name, local_filename, file_ext_name, meta_list);
        }
        /// <summary>
        /// upload file to storage server (by file name)
        /// </summary>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="local_filename">local filename to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.), null to extract ext name from the local filename</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        protected string[] upload_file(string group_name, string local_filename, string file_ext_name, NameValuePair[] meta_list)
        {
            byte cmd = ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_FILE;
            return this.upload_file(cmd, group_name, local_filename, file_ext_name, meta_list);
        }
        /// <summary>
        /// upload file to storage server (by file name)
        /// </summary>
        /// <param name="cmd">the command</param>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="local_filename">local filename to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.), null to extract ext name from the local filename</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        protected string[] upload_file(byte cmd, string group_name, string local_filename, string file_ext_name, NameValuePair[] meta_list)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(local_filename);
            FileStream fis = new FileStream(local_filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            if (file_ext_name == null)
            {
                file_ext_name = Path.GetExtension(local_filename).TrimStart('.');
            }

            try
            {
                return this.do_upload_file(cmd, group_name, null, null, file_ext_name,
                         f.Length, new UploadStream(fis, f.Length), meta_list);
            }
            finally
            {
                fis.Close();
            }
        }
        /// <summary>
        /// upload file to storage server (by file buff)
        /// </summary>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="offset">start offset of the buff</param>
        /// <param name="length">the length of buff to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file</li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(byte[] file_buff, int offset, int length, string file_ext_name, NameValuePair[] meta_list)
        {
            string group_name = null;
            return this.upload_file(group_name, file_buff, offset, length, file_ext_name, meta_list);
        }
        /// <summary>
        /// upload file to storage server (by file buff)
        /// </summary>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="offset">start offset of the buff</param>
        /// <param name="length">the length of buff to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file</li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(string group_name, byte[] file_buff, int offset, int length, string file_ext_name, NameValuePair[] meta_list)
        {
            return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_FILE, group_name, null, null, file_ext_name,
                       length, new UploadBuff(file_buff, offset, length), meta_list);
        }
        /// <summary>
        /// upload file to storage server (by file buff)
        /// </summary>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file</li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(byte[] file_buff, string file_ext_name, NameValuePair[] meta_list)
        {
            string group_name = null;
            return this.upload_file(group_name, file_buff, 0, file_buff.Length, file_ext_name, meta_list);
        }
        /// <summary>
        /// upload file to storage server (by file buff)
        /// </summary>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file</li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(string group_name, byte[] file_buff, string file_ext_name, NameValuePair[] meta_list)
        {
            return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_FILE, group_name, null, null, file_ext_name,
                       file_buff.Length, new UploadBuff(file_buff, 0, file_buff.Length), meta_list);
        }
        /// <summary>
        /// upload file to storage server (by callback)
        /// </summary>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="file_size">the file size</param>
        /// <param name="callback">the write data callback object</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file</li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(string group_name, long file_size, UploadCallback callback, string file_ext_name, NameValuePair[] meta_list)
        {
            string master_filename = null;
            string prefix_name = null;

            return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_FILE, group_name, master_filename, prefix_name,
               file_ext_name, file_size, callback, meta_list);
        }
        /// <summary>
        /// upload file to storage server (by file name, slave file mode)
        /// </summary>
        /// <param name="group_name">the group name of master file</param>
        /// <param name="master_filename">the master file name to generate the slave file</param>
        /// <param name="prefix_name">the prefix name to generate the slave file</param>
        /// <param name="local_filename">local filename to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.), null to extract ext name from the local filename</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file</li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(string group_name, string master_filename, string prefix_name, string local_filename, string file_ext_name, NameValuePair[] meta_list)
        {
            if ((group_name == null || group_name.Length == 0) ||
                (master_filename == null || master_filename.Length == 0) ||
                (prefix_name == null))
            {
                throw new MyException("invalid arguement");
            }

            System.IO.FileInfo f = new System.IO.FileInfo(local_filename);
            FileStream fis = new FileStream(local_filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            if (file_ext_name == null)
            {
                file_ext_name = Path.GetExtension(local_filename).TrimStart('.');
            }

            try
            {
                return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_SLAVE_FILE, group_name, master_filename, prefix_name,
                                         file_ext_name, f.Length, new UploadStream(fis, f.Length), meta_list);
            }
            finally
            {
                fis.Close();
            }
        }
        /// <summary>
        /// upload file to storage server (by file buff, slave file mode)
        /// </summary>
        /// <param name="group_name">the group name of master file</param>
        /// <param name="master_filename">the master file name to generate the slave file</param>
        /// <param name="prefix_name">the prefix name to generate the slave file</param>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file</li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(string group_name, string master_filename, string prefix_name, byte[] file_buff, string file_ext_name, NameValuePair[] meta_list)
        {
            if ((group_name == null || group_name.Length == 0) ||
                (master_filename == null || master_filename.Length == 0) ||
                (prefix_name == null))
            {
                throw new MyException("invalid arguement");
            }

            return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_SLAVE_FILE, group_name, master_filename, prefix_name,
                                       file_ext_name, file_buff.Length, new UploadBuff(file_buff, 0, file_buff.Length), meta_list);
        }
        /// <summary>
        /// upload file to storage server (by file buff, slave file mode)
        /// </summary>
        /// <param name="group_name">the group name of master file</param>
        /// <param name="master_filename">the master file name to generate the slave file</param>
        /// <param name="prefix_name">the prefix name to generate the slave file</param>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="offset">start offset of the buff</param>
        /// <param name="length">the length of buff to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file</li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(string group_name, string master_filename, string prefix_name, byte[] file_buff, int offset, int length, string file_ext_name, NameValuePair[] meta_list)
        {
            if ((group_name == null || group_name.Length == 0) ||
                (master_filename == null || master_filename.Length == 0) ||
                (prefix_name == null))
            {
                throw new MyException("invalid arguement");
            }

            return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_SLAVE_FILE, group_name, master_filename, prefix_name,
                        file_ext_name, length, new UploadBuff(file_buff, offset, length), meta_list);
        }
        /// <summary>
        /// upload file to storage server (by file buff, slave file mode)
        /// </summary>
        /// <param name="group_name">the group name of master file</param>
        /// <param name="master_filename">the master file name to generate the slave file</param>
        /// <param name="prefix_name">the prefix name to generate the slave file</param>
        /// <param name="file_size">the file size</param>
        /// <param name="callback">the write data callback object</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list"> meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file</li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_file(string group_name, string master_filename, string prefix_name, long file_size, UploadCallback callback, string file_ext_name, NameValuePair[] meta_list)
        {
            return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_SLAVE_FILE, group_name, master_filename, prefix_name,
               file_ext_name, file_size, callback, meta_list);
        }
        /// <summary>
        /// upload appender file to storage server (by file name)
        /// </summary>
        /// <param name="local_filename">local filename to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.), null to extract ext name from the local filename</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_appender_file(string local_filename, string file_ext_name, NameValuePair[] meta_list)
        {
            string group_name = null;
            return this.upload_appender_file(group_name, local_filename, file_ext_name, meta_list);
        }
        /// <summary>
        /// upload appender file to storage server (by file name)
        /// </summary>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="local_filename">local filename to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.), null to extract ext name from the local filename</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        protected string[] upload_appender_file(string group_name, string local_filename, string file_ext_name, NameValuePair[] meta_list)
        {
            byte cmd = ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_APPENDER_FILE;
            return this.upload_file(cmd, group_name, local_filename, file_ext_name, meta_list);
        }
        /// <summary>
        /// upload appender file to storage server (by file buff)
        /// </summary>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="offset">start offset of the buff</param>
        /// <param name="length">the length of buff to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_appender_file(byte[] file_buff, int offset, int length, string file_ext_name, NameValuePair[] meta_list)
        {
            string group_name = null;
            return this.upload_appender_file(group_name, file_buff, offset, length, file_ext_name, meta_list);
        }
        /// <summary>
        /// upload appender file to storage server (by file buff)
        /// </summary>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="offset">start offset of the buff</param>
        /// <param name="length">the length of buff to upload</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_appender_file(string group_name, byte[] file_buff, int offset, int length, string file_ext_name, NameValuePair[] meta_list)
        {
            return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_APPENDER_FILE, group_name, null, null, file_ext_name,
                       length, new UploadBuff(file_buff, offset, length), meta_list);
        }
        /// <summary>
        /// upload appender file to storage server (by file buff)
        /// </summary>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_appender_file(byte[] file_buff, string file_ext_name, NameValuePair[] meta_list)
        {
            string group_name = null;
            return this.upload_appender_file(group_name, file_buff, 0, file_buff.Length, file_ext_name, meta_list);
        }
        /// <summary>
        /// upload appender file to storage server (by file buff)
        /// </summary>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_appender_file(string group_name, byte[] file_buff, string file_ext_name, NameValuePair[] meta_list)
        {
            return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_APPENDER_FILE, group_name, null, null, file_ext_name,
                       file_buff.Length, new UploadBuff(file_buff, 0, file_buff.Length), meta_list);
        }
        /// <summary>
        /// upload appender file to storage server (by callback)
        /// </summary>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="file_size">the file size</param>
        /// <param name="callback">the write data callback object</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li>results[0]: the group name to store the file </li></ul>
        /// <ul><li>results[1]: the new created filename</li></ul>
        /// return null if fail
        /// </returns>
        public string[] upload_appender_file(string group_name, long file_size, UploadCallback callback, string file_ext_name, NameValuePair[] meta_list)
        {
            string master_filename = null;
            string prefix_name = null;

            return this.do_upload_file(ProtoCommon.STORAGE_PROTO_CMD_UPLOAD_APPENDER_FILE, group_name, master_filename, prefix_name,
               file_ext_name, file_size, callback, meta_list);
        }
        /// <summary>
        /// append file to storage server (by file name)
        /// </summary>
        /// <param name="group_name">the group name of appender file</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="local_filename">local filename to append</param>
        /// <returns>0 for success, != 0 for error (error no)</returns>
        public int append_file(string group_name, string appender_filename, string local_filename)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(local_filename);
            FileStream fis = new FileStream(local_filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            try
            {
                return this.do_append_file(group_name, appender_filename, f.Length, new UploadStream(fis, f.Length));
            }
            finally
            {
                fis.Close();
            }
        }
        /// <summary>
        /// append file to storage server (by file buff)
        /// </summary>
        /// <param name="group_name">the group name of appender file</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="file_buff">file content/buff</param>
        /// <returns>0 for success, != 0 for error (error no)</returns>
        public int append_file(string group_name, string appender_filename, byte[] file_buff)
        {
            return this.do_append_file(group_name, appender_filename, file_buff.Length, new UploadBuff(file_buff, 0, file_buff.Length));
        }
        /// <summary>
        /// append file to storage server (by file buff)
        /// </summary>
        /// <param name="group_name">the group name of appender file</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="offset">start offset of the buff</param>
        /// <param name="length">the length of buff to append</param>
        /// <returns>0 for success, != 0 for error (error no)</returns>
        public int append_file(string group_name, string appender_filename, byte[] file_buff, int offset, int length)
        {
            return this.do_append_file(group_name, appender_filename, length, new UploadBuff(file_buff, offset, length));
        }
        /// <summary>
        /// append file to storage server (by callback)
        /// </summary>
        /// <param name="group_name">the group name to append file to</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="file_size">the file size</param>
        /// <param name="callback">the write data callback object</param>
        /// <returns>0 for success, != 0 for error (error no)</returns>
        public int append_file(string group_name, string appender_filename, long file_size, UploadCallback callback)
        {
            return this.do_append_file(group_name, appender_filename, file_size, callback);
        }
        /// <summary>
        /// modify appender file to storage server (by file name)
        /// </summary>
        /// <param name="group_name">the group name of appender file</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="file_offset">the offset of appender file</param>
        /// <param name="local_filename">local filename to append</param>
        /// <returns>0 for success, != 0 for error (error no)</returns>
        public int modify_file(string group_name, string appender_filename, long file_offset, string local_filename)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(local_filename);
            FileStream fis = new FileStream(local_filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            try
            {
                return this.do_modify_file(group_name, appender_filename, file_offset,
                      f.Length, new UploadStream(fis, f.Length));
            }
            finally
            {
                fis.Close();
            }
        }
        /// <summary>
        /// modify appender file to storage server (by file buff)
        /// </summary>
        /// <param name="group_name">the group name of appender file</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="file_offset">the offset of appender file</param>
        /// <param name="file_buff">file content/buff</param>
        /// <returns>0 for success, != 0 for error (error no)</returns>
        public int modify_file(string group_name, string appender_filename, long file_offset, byte[] file_buff)
        {
            return this.do_modify_file(group_name, appender_filename, file_offset,
                    file_buff.Length, new UploadBuff(file_buff, 0, file_buff.Length));
        }
        /// <summary>
        /// modify appender file to storage server (by file buff)
        /// </summary>
        /// <param name="group_name">the group name of appender file</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="file_offset">the offset of appender file</param>
        /// <param name="file_buff">file content/buff</param>
        /// <param name="buffer_offset">start offset of the buff</param>
        /// <param name="buffer_length">the length of buff to modify</param>
        /// <returns>0 for success, != 0 for error (error no)</returns>
        public int modify_file(string group_name, string appender_filename, long file_offset, byte[] file_buff, int buffer_offset, int buffer_length)
        {
            return this.do_modify_file(group_name, appender_filename, file_offset,
                    buffer_length, new UploadBuff(file_buff, buffer_offset, buffer_length));
        }
        /// <summary>
        /// modify appender file to storage server (by callback)
        /// </summary>
        /// <param name="group_name">the group name to modify file to</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="file_offset">the offset of appender file</param>
        /// <param name="modify_size">the modify size</param>
        /// <param name="callback">the write data callback object</param>
        /// <returns>0 for success, != 0 for error (error no)</returns>
        public int modify_file(string group_name, string appender_filename, long file_offset, long modify_size, UploadCallback callback)
        {
            return this.do_modify_file(group_name, appender_filename, file_offset,
                    modify_size, callback);
        }
        /// <summary>
        /// modify appender file to storage server
        /// </summary>
        /// <param name="group_name">the group name of appender file</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="file_offset">the offset of appender file</param>
        /// <param name="modify_size">the modify size</param>
        /// <param name="callback">the write data callback object</param>
        /// <returns>true for success, false for fail</returns>
        protected int do_modify_file(string group_name, string appender_filename, long file_offset, long modify_size, UploadCallback callback)
        {
            byte[] header;
            bool bNewConnection;
            TcpClient storageSocket;
            byte[] hexLenBytes;
            byte[] appenderFilenameBytes;
            int offset;
            long body_len;

            if ((group_name == null || group_name.Length == 0) ||
                (appender_filename == null || appender_filename.Length == 0))
            {
                this.errno = ProtoCommon.ERR_NO_EINVAL;
                return this.errno;
            }

            bNewConnection = this.newUpdatableStorageConnection(group_name, appender_filename);

            try
            {
                storageSocket = this.storageServer.getSocket();

                appenderFilenameBytes = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(appender_filename);
                body_len = 3 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE + appenderFilenameBytes.Length + modify_size;

                header = ProtoCommon.packHeader(ProtoCommon.STORAGE_PROTO_CMD_MODIFY_FILE, body_len, (byte)0);
                byte[] wholePkg = new byte[(int)(header.Length + body_len - modify_size)];
                Array.Copy(header, 0, wholePkg, 0, header.Length);
                offset = header.Length;

                //hexLenBytes = BitConverter.GetBytes((long)appender_filename.Length);
                hexLenBytes = ProtoCommon.long2buff(appender_filename.Length);
                Array.Copy(hexLenBytes, 0, wholePkg, offset, hexLenBytes.Length);
                offset += hexLenBytes.Length;

                //hexLenBytes = BitConverter.GetBytes(file_offset);
                hexLenBytes = ProtoCommon.long2buff(file_offset);
                Array.Copy(hexLenBytes, 0, wholePkg, offset, hexLenBytes.Length);
                offset += hexLenBytes.Length;

                //hexLenBytes = BitConverter.GetBytes(modify_size);
                hexLenBytes = ProtoCommon.long2buff(modify_size);
                Array.Copy(hexLenBytes, 0, wholePkg, offset, hexLenBytes.Length);
                offset += hexLenBytes.Length;

                Stream output = storageSocket.GetStream();

                Array.Copy(appenderFilenameBytes, 0, wholePkg, offset, appenderFilenameBytes.Length);
                offset += appenderFilenameBytes.Length;

                output.Write(wholePkg, 0, wholePkg.Length);
                if ((this.errno = (byte)callback.send(output)) != 0)
                {
                    return this.errno;
                }

                ProtoCommon.RecvPackageInfo pkgInfo = ProtoCommon.recvPackage(storageSocket.GetStream(),
                                             ProtoCommon.STORAGE_PROTO_CMD_RESP, 0);
                this.errno = pkgInfo.errno;
                if (pkgInfo.errno != 0)
                {
                    return this.errno;
                }

                return 0;
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// append file to storage server
        /// </summary>
        /// <param name="group_name">the group name of appender file</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="file_size">the file size</param>
        /// <param name="callback">the write data callback object</param>
        /// <returns>true for success, false for fail</returns>
        protected int do_append_file(string group_name, string appender_filename, long file_size, UploadCallback callback)
        {
            byte[] header;
            bool bNewConnection;
            TcpClient storageSocket;
            byte[] hexLenBytes;
            byte[] appenderFilenameBytes;
            int offset;
            long body_len;

            if ((group_name == null || group_name.Length == 0) ||
                (appender_filename == null || appender_filename.Length == 0))
            {
                this.errno = ProtoCommon.ERR_NO_EINVAL;
                return this.errno;
            }

            bNewConnection = this.newUpdatableStorageConnection(group_name, appender_filename);

            try
            {
                storageSocket = this.storageServer.getSocket();

                appenderFilenameBytes = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(appender_filename);
                body_len = 2 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE + appenderFilenameBytes.Length + file_size;

                header = ProtoCommon.packHeader(ProtoCommon.STORAGE_PROTO_CMD_APPEND_FILE, body_len, (byte)0);
                byte[] wholePkg = new byte[(int)(header.Length + body_len - file_size)];
                Array.Copy(header, 0, wholePkg, 0, header.Length);
                offset = header.Length;

                //hexLenBytes = BitConverter.GetBytes((long)appender_filename.Length);
                hexLenBytes = ProtoCommon.long2buff(appender_filename.Length);
                Array.Copy(hexLenBytes, 0, wholePkg, offset, hexLenBytes.Length);
                offset += hexLenBytes.Length;

                //hexLenBytes = BitConverter.GetBytes(file_size);
                hexLenBytes = ProtoCommon.long2buff(file_size);
                Array.Copy(hexLenBytes, 0, wholePkg, offset, hexLenBytes.Length);
                offset += hexLenBytes.Length;

                Stream output = storageSocket.GetStream();

                Array.Copy(appenderFilenameBytes, 0, wholePkg, offset, appenderFilenameBytes.Length);
                offset += appenderFilenameBytes.Length;

                output.Write(wholePkg, 0, wholePkg.Length);
                if ((this.errno = (byte)callback.send(output)) != 0)
                {
                    return this.errno;
                }

                ProtoCommon.RecvPackageInfo pkgInfo = ProtoCommon.recvPackage(storageSocket.GetStream(),
                                             ProtoCommon.STORAGE_PROTO_CMD_RESP, 0);
                this.errno = pkgInfo.errno;
                if (pkgInfo.errno != 0)
                {
                    return this.errno;
                }

                return 0;
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// upload file to storage server
        /// </summary>
        /// <param name="cmd">the command code</param>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <param name="master_filename">the master file name to generate the slave file</param>
        /// <param name="prefix_name">the prefix name to generate the slave file</param>
        /// <param name="file_ext_name">file ext name, do not include dot(.)</param>
        /// <param name="file_size">the file size</param>
        /// <param name="callback">the write data callback object</param>
        /// <param name="meta_list">meta info array</param>
        /// <returns>
        /// 2 elements string array if success:<br>
        /// <ul><li> results[0]: the group name to store the file</li></ul>
        /// <ul><li> results[1]: the new created filename</li></ul> 
        /// return null if fail
        /// </returns>
        protected string[] do_upload_file(byte cmd, string group_name, string master_filename, string prefix_name, string file_ext_name, long file_size, UploadCallback callback, NameValuePair[] meta_list)
        {
            byte[] header;
            byte[] ext_name_bs;
            string new_group_name;
            string remote_filename;
            bool bNewConnection = false;
            TcpClient storageSocket;
            byte[] sizeBytes;
            byte[] hexLenBytes;
            byte[] masterFilenameBytes;
            bool bUploadSlave;
            int offset;
            long body_len;

            bUploadSlave = ((group_name != null && group_name.Length > 0) &&
                            (master_filename != null && master_filename.Length > 0) &&
                            (prefix_name != null));
            if (bUploadSlave)
            {
                bNewConnection = this.newUpdatableStorageConnection(group_name, master_filename);
            }
            else
            {
                bNewConnection = this.newWritableStorageConnection(group_name);
            }

            try
            {
                storageSocket = this.storageServer.getSocket();

                ext_name_bs = new byte[ProtoCommon.FDFS_FILE_EXT_NAME_MAX_LEN];
                for (int i = 0; i < ext_name_bs.Length; i++)
                    ext_name_bs[i] = (byte)0;
                if (file_ext_name != null && file_ext_name.Length > 0)
                {
                    byte[] bs = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(file_ext_name);
                    int ext_name_len = bs.Length;
                    if (ext_name_len > ProtoCommon.FDFS_FILE_EXT_NAME_MAX_LEN)
                    {
                        ext_name_len = ProtoCommon.FDFS_FILE_EXT_NAME_MAX_LEN;
                    }
                    Array.Copy(bs, 0, ext_name_bs, 0, ext_name_len);
                }

                if (bUploadSlave)
                {
                    masterFilenameBytes = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(master_filename);

                    sizeBytes = new byte[2 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE];
                    body_len = sizeBytes.Length + ProtoCommon.FDFS_FILE_PREFIX_MAX_LEN + ProtoCommon.FDFS_FILE_EXT_NAME_MAX_LEN
                             + masterFilenameBytes.Length + file_size;

                    //hexLenBytes = BitConverter.GetBytes((long)master_filename.Length);
                    hexLenBytes = ProtoCommon.long2buff(master_filename.Length);
                    Array.Copy(hexLenBytes, 0, sizeBytes, 0, hexLenBytes.Length);
                    offset = hexLenBytes.Length;
                }
                else
                {
                    masterFilenameBytes = null;
                    sizeBytes = new byte[1 + 1 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE];
                    body_len = sizeBytes.Length + ProtoCommon.FDFS_FILE_EXT_NAME_MAX_LEN + file_size;

                    sizeBytes[0] = (byte)this.storageServer.Store_Path_Index;
                    offset = 1;
                }

                //hexLenBytes = BitConverter.GetBytes(file_size);
                hexLenBytes = ProtoCommon.long2buff(file_size);
                Array.Copy(hexLenBytes, 0, sizeBytes, offset, hexLenBytes.Length);

                Stream output = storageSocket.GetStream();
                header = ProtoCommon.packHeader(cmd, body_len, (byte)0);
                byte[] wholePkg = new byte[(int)(header.Length + body_len - file_size)];
                Array.Copy(header, 0, wholePkg, 0, header.Length);
                Array.Copy(sizeBytes, 0, wholePkg, header.Length, sizeBytes.Length);
                offset = header.Length + sizeBytes.Length;
                if (bUploadSlave)
                {
                    byte[] prefix_name_bs = new byte[ProtoCommon.FDFS_FILE_PREFIX_MAX_LEN];
                    byte[] bs = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(prefix_name);
                    int prefix_name_len = bs.Length;
                    for (int i = 0; i < prefix_name_bs.Length; i++)
                        prefix_name_bs[i] = (byte)0;
                    if (prefix_name_len > ProtoCommon.FDFS_FILE_PREFIX_MAX_LEN)
                    {
                        prefix_name_len = ProtoCommon.FDFS_FILE_PREFIX_MAX_LEN;
                    }
                    if (prefix_name_len > 0)
                    {
                        Array.Copy(bs, 0, prefix_name_bs, 0, prefix_name_len);
                    }

                    Array.Copy(prefix_name_bs, 0, wholePkg, offset, prefix_name_bs.Length);
                    offset += prefix_name_bs.Length;
                }

                Array.Copy(ext_name_bs, 0, wholePkg, offset, ext_name_bs.Length);
                offset += ext_name_bs.Length;

                if (bUploadSlave)
                {
                    Array.Copy(masterFilenameBytes, 0, wholePkg, offset, masterFilenameBytes.Length);
                    offset += masterFilenameBytes.Length;
                }

                output.Write(wholePkg, 0, wholePkg.Length);

                if ((this.errno = (byte)callback.send(output)) != 0)
                {
                    return null;
                }

                ProtoCommon.RecvPackageInfo pkgInfo = ProtoCommon.recvPackage(storageSocket.GetStream(),
                                             ProtoCommon.STORAGE_PROTO_CMD_RESP, -1);
                this.errno = pkgInfo.errno;
                if (pkgInfo.errno != 0)
                {
                    return null;
                }

                if (pkgInfo.body.Length <= ProtoCommon.FDFS_GROUP_NAME_MAX_LEN)
                {
                    throw new MyException("body length: " + pkgInfo.body.Length + " <= " + ProtoCommon.FDFS_GROUP_NAME_MAX_LEN);
                }

                new_group_name = Encoding.GetEncoding(ClientGlobal.g_charset).GetString(pkgInfo.body, 0, ProtoCommon.FDFS_GROUP_NAME_MAX_LEN).Replace("\0", "").Trim();
                remote_filename = Encoding.GetEncoding(ClientGlobal.g_charset).GetString(pkgInfo.body, ProtoCommon.FDFS_GROUP_NAME_MAX_LEN, pkgInfo.body.Length - ProtoCommon.FDFS_GROUP_NAME_MAX_LEN);
                string[] results = new string[2];
                results[0] = new_group_name;
                results[1] = remote_filename;

                if (meta_list == null || meta_list.Length == 0)
                {
                    return results;
                }

                int result = 0;
                try
                {
                    result = this.set_metadata(new_group_name, remote_filename,
                                    meta_list, ProtoCommon.STORAGE_SET_METADATA_FLAG_OVERWRITE);
                }
                catch (IOException ex)
                {
                    result = 5;
                    throw ex;
                }
                finally
                {
                    if (result != 0)
                    {
                        this.errno = (byte)result;
                        this.delete_file(new_group_name, remote_filename);
                    }
                }

                return results;
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// check storage socket, if null create a new connection
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <returns>true if create a new connection</returns>
        protected bool newUpdatableStorageConnection(string group_name, string remote_filename)
        {
            if (this.storageServer != null)
            {
                return false;
            }
            else
            {
                TrackerClient tracker = new TrackerClient();
                this.storageServer = tracker.getUpdateStorage(this.trackerServer, group_name, remote_filename);
                if (this.storageServer == null)
                {
                    throw new MyException("getStoreStorage fail, errno code: " + tracker.ErrorCode);
                }
                return true;
            }
        }
        /// <summary>
        /// check storage socket, if null create a new connection
        /// </summary>
        /// <param name="group_name">the group name to upload file to, can be empty</param>
        /// <returns>true if create a new connection</returns>
        protected bool newWritableStorageConnection(string group_name)
        {
            if (this.storageServer != null)
            {
                return false;
            }
            else
            {
                TrackerClient tracker = new TrackerClient();
                this.storageServer = tracker.getStoreStorage(this.trackerServer, group_name);
                if (this.storageServer == null)
                {
                    throw new MyException("getStoreStorage fail, errno code: " + tracker.ErrorCode);
                }
                return true;
            }
        }
        /// <summary>
        /// set metadata items to storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <param name="meta_list">meta item array</param>
        /// <param name="op_flag">
        /// flag, can be one of following values: <br>
        /// <ul><li> ProtoCommon.STORAGE_SET_METADATA_FLAG_OVERWRITE: overwrite all old metadata items</li></ul>
        /// <ul><li> ProtoCommon.STORAGE_SET_METADATA_FLAG_MERGE: merge, insert when the metadata item not exist, otherwise update it</li></ul>
        /// </param>
        /// <returns>0 for success, !=0 fail (error code)</returns>
        public int set_metadata(string group_name, string remote_filename, NameValuePair[] meta_list, byte op_flag)
        {
            bool bNewConnection = this.newUpdatableStorageConnection(group_name, remote_filename);
            TcpClient storageSocket = this.storageServer.getSocket();

            try
            {
                byte[] header;
                byte[] groupBytes;
                byte[] filenameBytes;
                byte[] meta_buff;
                byte[] bs;
                int groupLen;
                byte[] sizeBytes;
                ProtoCommon.RecvPackageInfo pkgInfo;

                if (meta_list == null)
                {
                    meta_buff = new byte[0];
                }
                else
                {
                    meta_buff = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(ProtoCommon.pack_metadata(meta_list));
                }

                filenameBytes = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(remote_filename);
                sizeBytes = new byte[2 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE];
                for (int i = 0; i < sizeBytes.Length; i++)
                    sizeBytes[i] = (byte)0;

                //bs = BitConverter.GetBytes((long)filenameBytes.Length);
                bs = ProtoCommon.long2buff(filenameBytes.Length);
                Array.Copy(bs, 0, sizeBytes, 0, bs.Length);
                //bs = BitConverter.GetBytes((long)meta_buff.Length);
                bs = ProtoCommon.long2buff(meta_buff.Length);
                Array.Copy(bs, 0, sizeBytes, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE, bs.Length);

                groupBytes = new byte[ProtoCommon.FDFS_GROUP_NAME_MAX_LEN];
                bs = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(group_name);
                for (int i = 0; i < groupBytes.Length; i++)
                    groupBytes[i] = (byte)0;
                if (bs.Length <= groupBytes.Length)
                {
                    groupLen = bs.Length;
                }
                else
                {
                    groupLen = groupBytes.Length;
                }
                Array.Copy(bs, 0, groupBytes, 0, groupLen);

                header = ProtoCommon.packHeader(ProtoCommon.STORAGE_PROTO_CMD_SET_METADATA,
                           2 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE + 1 + groupBytes.Length
                           + filenameBytes.Length + meta_buff.Length, (byte)0);
                Stream output = storageSocket.GetStream();
                byte[] wholePkg = new byte[header.Length + sizeBytes.Length + 1 + groupBytes.Length + filenameBytes.Length];
                Array.Copy(header, 0, wholePkg, 0, header.Length);
                Array.Copy(sizeBytes, 0, wholePkg, header.Length, sizeBytes.Length);
                wholePkg[header.Length + sizeBytes.Length] = op_flag;
                Array.Copy(groupBytes, 0, wholePkg, header.Length + sizeBytes.Length + 1, groupBytes.Length);
                Array.Copy(filenameBytes, 0, wholePkg, header.Length + sizeBytes.Length + 1 + groupBytes.Length, filenameBytes.Length);
                output.Write(wholePkg, 0, wholePkg.Length);
                if (meta_buff.Length > 0)
                {
                    output.Write(meta_buff, 0, meta_buff.Length);
                }

                pkgInfo = ProtoCommon.recvPackage(storageSocket.GetStream(),
                                             ProtoCommon.STORAGE_PROTO_CMD_RESP, 0);

                this.errno = pkgInfo.errno;
                return pkgInfo.errno;
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// delete file from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <returns>0 for success, none zero for fail (error code)</returns>
        public int delete_file(string group_name, string remote_filename)
        {
            bool bNewConnection = this.newUpdatableStorageConnection(group_name, remote_filename);
            TcpClient storageSocket = this.storageServer.getSocket();

            try
            {
                this.send_package(ProtoCommon.STORAGE_PROTO_CMD_DELETE_FILE, group_name, remote_filename);
                ProtoCommon.RecvPackageInfo pkgInfo = ProtoCommon.recvPackage(storageSocket.GetStream(),
                                             ProtoCommon.STORAGE_PROTO_CMD_RESP, 0);

                this.errno = pkgInfo.errno;
                return pkgInfo.errno;
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// send package to storage server
        /// </summary>
        /// <param name="cmd">which command to send</param>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        protected void send_package(byte cmd, string group_name, string remote_filename)
        {
            byte[] header;
            byte[] groupBytes;
            byte[] filenameBytes;
            byte[] bs;
            int groupLen;

            groupBytes = new byte[ProtoCommon.FDFS_GROUP_NAME_MAX_LEN];
            bs = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(group_name);
            filenameBytes = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(remote_filename);
            for (int i = 0; i < groupBytes.Length; i++)
                groupBytes[i] = (byte)0;
            if (bs.Length <= groupBytes.Length)
            {
                groupLen = bs.Length;
            }
            else
            {
                groupLen = groupBytes.Length;
            }
            Array.Copy(bs, 0, groupBytes, 0, groupLen);

            header = ProtoCommon.packHeader(cmd, groupBytes.Length + filenameBytes.Length, (byte)0);
            byte[] wholePkg = new byte[header.Length + groupBytes.Length + filenameBytes.Length];
            Array.Copy(header, 0, wholePkg, 0, header.Length);
            Array.Copy(groupBytes, 0, wholePkg, header.Length, groupBytes.Length);
            Array.Copy(filenameBytes, 0, wholePkg, header.Length + groupBytes.Length, filenameBytes.Length);
            this.storageServer.getSocket().GetStream().Write(wholePkg, 0, wholePkg.Length);
        }
        /// <summary>
        /// truncate appender file to size 0 from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <returns>0 for success, none zero for fail (error code)</returns>
        public int truncate_file(string group_name, string appender_filename)
        {
            long truncated_file_size = 0;
            return this.truncate_file(group_name, appender_filename, truncated_file_size);
        }
        /// <summary>
        /// truncate appender file from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="appender_filename">the appender filename</param>
        /// <param name="truncated_file_size">truncated file size</param>
        /// <returns>0 for success, none zero for fail (error code)</returns>
        public int truncate_file(string group_name, string appender_filename, long truncated_file_size)
        {
            byte[] header;
            bool bNewConnection;
            TcpClient storageSocket;
            byte[] hexLenBytes;
            byte[] appenderFilenameBytes;
            int offset;
            int body_len;

            if ((group_name == null || group_name.Length == 0) ||
                (appender_filename == null || appender_filename.Length == 0))
            {
                this.errno = ProtoCommon.ERR_NO_EINVAL;
                return this.errno;
            }

            bNewConnection = this.newUpdatableStorageConnection(group_name, appender_filename);

            try
            {
                storageSocket = this.storageServer.getSocket();

                appenderFilenameBytes = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(appender_filename);
                body_len = 2 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE + appenderFilenameBytes.Length;

                header = ProtoCommon.packHeader(ProtoCommon.STORAGE_PROTO_CMD_TRUNCATE_FILE, body_len, (byte)0);
                byte[] wholePkg = new byte[header.Length + body_len];
                Array.Copy(header, 0, wholePkg, 0, header.Length);
                offset = header.Length;

                //hexLenBytes = BitConverter.GetBytes((long)appender_filename.Length);
                hexLenBytes = ProtoCommon.long2buff(appender_filename.Length);
                Array.Copy(hexLenBytes, 0, wholePkg, offset, hexLenBytes.Length);
                offset += hexLenBytes.Length;

                //hexLenBytes = BitConverter.GetBytes(truncated_file_size);
                hexLenBytes = ProtoCommon.long2buff(truncated_file_size);
                Array.Copy(hexLenBytes, 0, wholePkg, offset, hexLenBytes.Length);
                offset += hexLenBytes.Length;

                Stream output = storageSocket.GetStream();

                Array.Copy(appenderFilenameBytes, 0, wholePkg, offset, appenderFilenameBytes.Length);
                offset += appenderFilenameBytes.Length;

                output.Write(wholePkg, 0, wholePkg.Length);
                ProtoCommon.RecvPackageInfo pkgInfo = ProtoCommon.recvPackage(storageSocket.GetStream(),
                                             ProtoCommon.STORAGE_PROTO_CMD_RESP, 0);
                this.errno = pkgInfo.errno;
                return pkgInfo.errno;
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// download file from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <returns>file content/buff, return null if fail</returns>
        public byte[] download_file(string group_name, string remote_filename)
        {
            long file_offset = 0;
            long download_bytes = 0;

            return this.download_file(group_name, remote_filename, file_offset, download_bytes);
        }
        /// <summary>
        /// download file from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <param name="file_offset">the start offset of the file</param>
        /// <param name="download_bytes">download bytes, 0 for remain bytes from offset</param>
        /// <returns>file content/buff, return null if fail</returns>
        public byte[] download_file(string group_name, string remote_filename, long file_offset, long download_bytes)
        {
            bool bNewConnection = this.newReadableStorageConnection(group_name, remote_filename);
            TcpClient storageSocket = this.storageServer.getSocket();

            try
            {
                ProtoCommon.RecvPackageInfo pkgInfo;

                this.send_download_package(group_name, remote_filename, file_offset, download_bytes);
                pkgInfo = ProtoCommon.recvPackage(storageSocket.GetStream(),
                                             ProtoCommon.STORAGE_PROTO_CMD_RESP, -1);

                this.errno = pkgInfo.errno;
                if (pkgInfo.errno != 0)
                {
                    return null;
                }

                return pkgInfo.body;
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// download file from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <param name="local_filename">filename on local</param>
        /// <returns>0 success, return none zero errno if fail</returns>
        public int download_file(string group_name, string remote_filename, string local_filename)
        {
            long file_offset = 0;
            long download_bytes = 0;
            return this.download_file(group_name, remote_filename,
                          file_offset, download_bytes, local_filename);
        }
        /// <summary>
        /// download file from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <param name="file_offset">the start offset of the file</param>
        /// <param name="download_bytes">download bytes, 0 for remain bytes from offset</param>
        /// <param name="local_filename">filename on local</param>
        /// <returns>0 success, return none zero errno if fail</returns>
        public int download_file(string group_name, string remote_filename, long file_offset, long download_bytes, string local_filename)
        {
            bool bNewConnection = this.newReadableStorageConnection(group_name, remote_filename);
            TcpClient storageSocket = this.storageServer.getSocket();
            try
            {
                ProtoCommon.RecvHeaderInfo header;
                FileStream output = new FileStream(local_filename, FileMode.Create, FileAccess.Write, FileShare.Read);
                try
                {
                    this.errno = 0;
                    this.send_download_package(group_name, remote_filename, file_offset, download_bytes);

                    Stream input = storageSocket.GetStream();
                    header = ProtoCommon.recvHeader(input, ProtoCommon.STORAGE_PROTO_CMD_RESP, -1);
                    this.errno = header.errno;
                    if (header.errno != 0)
                    {
                        return header.errno;
                    }

                    byte[] buff = new byte[256 * 1024];
                    long remainBytes = header.body_len;
                    int bytes;

                    //System.out.println("expect_body_len=" + header.body_len);

                    while (remainBytes > 0)
                    {
                        if ((bytes = input.Read(buff, 0, remainBytes > buff.Length ? buff.Length : (int)remainBytes)) < 0)
                        {
                            throw new IOException("recv package size " + (header.body_len - remainBytes) + " != " + header.body_len);
                        }

                        output.Write(buff, 0, bytes);
                        remainBytes -= bytes;

                        //System.out.println("totalBytes=" + (header.body_len - remainBytes));
                    }

                    return 0;
                }
                catch (IOException ex)
                {
                    if (this.errno == 0)
                    {
                        this.errno = ProtoCommon.ERR_NO_EIO;
                    }

                    throw ex;
                }
                finally
                {
                    output.Close();
                    if (this.errno != 0)
                    {
                        File.Delete(local_filename);
                    }
                }
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// download file from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <param name="callback">call callback.recv() when data arrive</param>
        /// <returns>0 success, return none zero errno if fail</returns>
        public int download_file(string group_name, string remote_filename, DownloadCallback callback)
        {
            long file_offset = 0;
            long download_bytes = 0;
            return this.download_file(group_name, remote_filename,
                          file_offset, download_bytes, callback);
        }
        /// <summary>
        /// download file from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <param name="file_offset">the start offset of the file</param>
        /// <param name="download_bytes">download bytes, 0 for remain bytes from offset</param>
        /// <param name="callback">call callback.recv() when data arrive</param>
        /// <returns>0 success, return none zero errno if fail</returns>
        public int download_file(string group_name, string remote_filename, long file_offset, long download_bytes, DownloadCallback callback)
        {
            int result;
            bool bNewConnection = this.newReadableStorageConnection(group_name, remote_filename);
            TcpClient storageSocket = this.storageServer.getSocket();

            try
            {
                ProtoCommon.RecvHeaderInfo header;
                this.send_download_package(group_name, remote_filename, file_offset, download_bytes);

                Stream input = storageSocket.GetStream();
                header = ProtoCommon.recvHeader(input, ProtoCommon.STORAGE_PROTO_CMD_RESP, -1);
                this.errno = header.errno;
                if (header.errno != 0)
                {
                    return header.errno;
                }

                byte[] buff = new byte[2 * 1024];
                long remainBytes = header.body_len;
                int bytes;

                //System.out.println("expect_body_len=" + header.body_len);

                while (remainBytes > 0)
                {
                    if ((bytes = input.Read(buff, 0, remainBytes > buff.Length ? buff.Length : (int)remainBytes)) < 0)
                    {
                        throw new IOException("recv package size " + (header.body_len - remainBytes) + " != " + header.body_len);
                    }

                    if ((result = callback.recv(header.body_len, buff, bytes)) != 0)
                    {
                        this.errno = (byte)result;
                        return result;
                    }

                    remainBytes -= bytes;
                    //System.out.println("totalBytes=" + (header.body_len - remainBytes));
                }

                return 0;
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// check storage socket, if null create a new connection
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <returns>true if create a new connection</returns>
        protected bool newReadableStorageConnection(string group_name, string remote_filename)
        {
            if (this.storageServer != null)
            {
                return false;
            }
            else
            {
                TrackerClient tracker = new TrackerClient();
                this.storageServer = tracker.getFetchStorage(this.trackerServer, group_name, remote_filename);
                if (this.storageServer == null)
                {
                    throw new MyException("getStoreStorage fail, errno code: " + tracker.ErrorCode);
                }
                return true;
            }
        }
        /// <summary>
        /// send package to storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <param name="file_offset">the start offset of the file</param>
        /// <param name="download_bytes">download bytes</param>
        protected void send_download_package(string group_name, string remote_filename, long file_offset, long download_bytes)
        {
            byte[] header;
            byte[] bsOffset;
            byte[] bsDownBytes;
            byte[] groupBytes;
            byte[] filenameBytes;
            byte[] bs;
            int groupLen;

            //bsOffset = BitConverter.GetBytes(file_offset);
            bsOffset = ProtoCommon.long2buff(file_offset);
            //bsDownBytes = BitConverter.GetBytes(download_bytes);
            bsDownBytes = ProtoCommon.long2buff(download_bytes);
            groupBytes = new byte[ProtoCommon.FDFS_GROUP_NAME_MAX_LEN];
            bs = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(group_name);
            filenameBytes = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(remote_filename);
            for (int i = 0; i < groupBytes.Length; i++)
                groupBytes[i] = (byte)0;
            if (bs.Length <= groupBytes.Length)
            {
                groupLen = bs.Length;
            }
            else
            {
                groupLen = groupBytes.Length;
            }
            Array.Copy(bs, 0, groupBytes, 0, groupLen);

            header = ProtoCommon.packHeader(ProtoCommon.STORAGE_PROTO_CMD_DOWNLOAD_FILE,
                 bsOffset.Length + bsDownBytes.Length + groupBytes.Length + filenameBytes.Length, (byte)0);
            byte[] wholePkg = new byte[header.Length + bsOffset.Length + bsDownBytes.Length + groupBytes.Length + filenameBytes.Length];
            Array.Copy(header, 0, wholePkg, 0, header.Length);
            Array.Copy(bsOffset, 0, wholePkg, header.Length, bsOffset.Length);
            Array.Copy(bsDownBytes, 0, wholePkg, header.Length + bsOffset.Length, bsDownBytes.Length);
            Array.Copy(groupBytes, 0, wholePkg, header.Length + bsOffset.Length + bsDownBytes.Length, groupBytes.Length);
            Array.Copy(filenameBytes, 0, wholePkg, header.Length + bsOffset.Length + bsDownBytes.Length + groupBytes.Length, filenameBytes.Length);
            this.storageServer.getSocket().GetStream().Write(wholePkg, 0, wholePkg.Length);
        }
        /// <summary>
        /// get all metadata items from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <returns>meta info array, return null if fail</returns>
        public NameValuePair[] get_metadata(string group_name, string remote_filename)
        {
            bool bNewConnection = this.newUpdatableStorageConnection(group_name, remote_filename);
            TcpClient storageSocket = this.storageServer.getSocket();

            try
            {
                ProtoCommon.RecvPackageInfo pkgInfo;

                this.send_package(ProtoCommon.STORAGE_PROTO_CMD_GET_METADATA, group_name, remote_filename);
                pkgInfo = ProtoCommon.recvPackage(storageSocket.GetStream(),
                                             ProtoCommon.STORAGE_PROTO_CMD_RESP, -1);

                this.errno = pkgInfo.errno;
                if (pkgInfo.errno != 0)
                {
                    return null;
                }

                return ProtoCommon.split_metadata(Encoding.GetEncoding(ClientGlobal.g_charset).GetString(pkgInfo.body));
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
        /// <summary>
        /// get file info decoded from the filename, fetch from the storage if necessary
        /// </summary>
        /// <param name="group_name">the group name</param>
        /// <param name="remote_filename">the filename</param>
        /// <returns>FileInfo object for success, return null for fail</returns>
        public FileInfo get_file_info(string group_name, string remote_filename)
        {
            if (remote_filename.Length < ProtoCommon.FDFS_FILE_PATH_LEN + ProtoCommon.FDFS_FILENAME_BASE64_LENGTH
                             + ProtoCommon.FDFS_FILE_EXT_NAME_MAX_LEN + 1)
            {
                this.errno = ProtoCommon.ERR_NO_EINVAL;
                return null;
            }
            byte[] buff = base64.decodeAuto(remote_filename.Substring(ProtoCommon.FDFS_FILE_PATH_LEN,
                /*ProtoCommon.FDFS_FILE_PATH_LEN + */ProtoCommon.FDFS_FILENAME_BASE64_LENGTH));

            long file_size = ProtoCommon.buff2long(buff, 4 * 2);
            if (((remote_filename.Length > ProtoCommon.TRUNK_LOGIC_FILENAME_LENGTH) ||
                    ((remote_filename.Length > ProtoCommon.NORMAL_LOGIC_FILENAME_LENGTH) && ((file_size & ProtoCommon.TRUNK_FILE_MARK_SIZE) == 0))) ||
                    ((file_size & ProtoCommon.APPENDER_FILE_SIZE) != 0))
            { //slave file or appender file
                FileInfo fi = this.query_file_info(group_name, remote_filename);
                if (fi == null)
                {
                    return null;
                }
                return fi;
            }

            FileInfo fileInfo = new FileInfo(file_size, 0, 0, ProtoCommon.getIpAddress(buff, 0));
            //fileInfo.Create_Timestamp = new DateTime(BitConverter.ToInt32(buff, 4) * 1000L);
            //fileInfo.Create_Timestamp = new DateTime(ProtoCommon.buff2int(buff, 4) * 1000L);
            fileInfo.Create_Timestamp = ProtoCommon.UnixTimestampToDateTime(ProtoCommon.buff2int(buff, 4));
            if ((file_size >> 63) != 0)
            {
                file_size &= 0xFFFFFFFFL;  //low 32 bits is file size
                fileInfo.File_Size = file_size;
            }
            //fileInfo.Crc32 = BitConverter.ToInt32(buff, 4 * 4);
            fileInfo.Crc32 = ProtoCommon.buff2int(buff, 4 * 4);

            return fileInfo;
        }
        /// <summary>
        /// get file info from storage server
        /// </summary>
        /// <param name="group_name">the group name of storage server</param>
        /// <param name="remote_filename">filename on storage server</param>
        /// <returns>FileInfo object for success, return null for fail</returns>
        public FileInfo query_file_info(string group_name, string remote_filename)
        {
            bool bNewConnection = this.newUpdatableStorageConnection(group_name, remote_filename);
            TcpClient storageSocket = this.storageServer.getSocket();

            try
            {
                byte[] header;
                byte[] groupBytes;
                byte[] filenameBytes;
                byte[] bs;
                int groupLen;
                ProtoCommon.RecvPackageInfo pkgInfo;

                filenameBytes = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(remote_filename);
                groupBytes = new byte[ProtoCommon.FDFS_GROUP_NAME_MAX_LEN];
                bs = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes(group_name);
                for (int i = 0; i < groupBytes.Length; i++)
                    groupBytes[i] = (byte)0;
                if (bs.Length <= groupBytes.Length)
                {
                    groupLen = bs.Length;
                }
                else
                {
                    groupLen = groupBytes.Length;
                }
                Array.Copy(bs, 0, groupBytes, 0, groupLen);

                header = ProtoCommon.packHeader(ProtoCommon.STORAGE_PROTO_CMD_QUERY_FILE_INFO,
                           +groupBytes.Length + filenameBytes.Length, (byte)0);
                Stream output = storageSocket.GetStream();
                byte[] wholePkg = new byte[header.Length + groupBytes.Length + filenameBytes.Length];
                Array.Copy(header, 0, wholePkg, 0, header.Length);
                Array.Copy(groupBytes, 0, wholePkg, header.Length, groupBytes.Length);
                Array.Copy(filenameBytes, 0, wholePkg, header.Length + groupBytes.Length, filenameBytes.Length);
                output.Write(wholePkg, 0, wholePkg.Length);

                pkgInfo = ProtoCommon.recvPackage(storageSocket.GetStream(),
                                             ProtoCommon.STORAGE_PROTO_CMD_RESP,
                                             3 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE +
                                             ProtoCommon.FDFS_IPADDR_SIZE);

                this.errno = pkgInfo.errno;
                if (pkgInfo.errno != 0)
                {
                    return null;
                }

                long file_size = ProtoCommon.buff2long(pkgInfo.body, 0);
                int create_timestamp = (int)ProtoCommon.buff2long(pkgInfo.body, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
                int crc32 = (int)ProtoCommon.buff2long(pkgInfo.body, 2 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
                string source_ip_addr = Encoding.GetEncoding(ClientGlobal.g_charset).GetString(pkgInfo.body, 3 * ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE, ProtoCommon.FDFS_IPADDR_SIZE).Replace("\0", "").Trim();
                return new FileInfo(file_size, create_timestamp, crc32, source_ip_addr);
            }
            catch (IOException ex)
            {
                if (!bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }

                throw ex;
            }
            finally
            {
                if (bNewConnection)
                {
                    try
                    {
                        this.storageServer.close();
                    }
                    catch (IOException ex1)
                    {

                    }
                    finally
                    {
                        this.storageServer = null;
                    }
                }
            }
        }
    }
}
