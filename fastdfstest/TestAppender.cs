using fastdfs.client.net;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace fastdfstest
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class TestAppender
    {
        public static void Run(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error: Must have 2 parameters, one is config filename, "
                                 + "the other is the local filename to upload");
                return;
            }

            string conf_filename = args[0];
            string local_filename = args[1];

            try
            {
                ClientGlobal.init(conf_filename);

                string group_name;
                string remote_filename;
                ServerInfo[] servers;
                TrackerClient tracker = new TrackerClient();
                TrackerServer trackerServer = tracker.getConnection();

                StorageServer storageServer = null;

                StorageClient client = new StorageClient(trackerServer, storageServer);
                byte[] file_buff;
                NameValuePair[] meta_list;
                string[] results;
                string appender_filename;
                string file_ext_name;
                int errno;

                meta_list = new NameValuePair[4];
                meta_list[0] = new NameValuePair("width", "800");
                meta_list[1] = new NameValuePair("heigth", "600");
                meta_list[2] = new NameValuePair("bgcolor", "#FFFFFF");
                meta_list[3] = new NameValuePair("author", "Mike");

                file_buff = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes("this is a test");
                Console.WriteLine("file length: " + file_buff.Length);

                group_name = null;
                StorageServer[] storageServers = tracker.getStoreStorages(trackerServer, group_name);
                if (storageServers == null)
                {
                    Console.WriteLine("get store storage servers fail, error code: " + tracker.ErrorCode);
                }
                else
                {
                    Console.WriteLine("store storage servers count: " + storageServers.Length);
                    for (int k = 0; k < storageServers.Length; k++)
                    {
                        Console.WriteLine((k + 1) + ". " + storageServers[k].InetSockAddr.Address + ":" + storageServers[k].InetSockAddr.Port);
                    }
                    Console.WriteLine("");
                }

                Stopwatch sw = new Stopwatch();
                sw.Start();
                results = client.upload_appender_file(file_buff, "txt", meta_list);
                sw.Stop();
                Console.WriteLine("upload_appender_file time used: " + sw.ElapsedMilliseconds + " ms");

                if (results == null)
                {
                    Console.WriteLine("upload file fail, error code: " + client.ErrorCode);
                    return;
                }
                else
                {
                    group_name = results[0];
                    remote_filename = results[1];
                    Console.WriteLine("group_name: " + group_name + ", remote_filename: " + remote_filename);
                    Console.WriteLine(client.get_file_info(group_name, remote_filename));

                    servers = tracker.getFetchStorages(trackerServer, group_name, remote_filename);
                    if (servers == null)
                    {
                        Console.WriteLine("get storage servers fail, error code: " + tracker.ErrorCode);
                    }
                    else
                    {
                        Console.WriteLine("storage servers count: " + servers.Length);
                        for (int k = 0; k < servers.Length; k++)
                        {
                            Console.WriteLine((k + 1) + ". " + servers[k].Ip_Addr + ":" + servers[k].Port);
                        }
                        Console.WriteLine("");
                    }

                    meta_list = new NameValuePair[4];
                    meta_list[0] = new NameValuePair("width", "1024");
                    meta_list[1] = new NameValuePair("heigth", "768");
                    meta_list[2] = new NameValuePair("bgcolor", "#000000");
                    meta_list[3] = new NameValuePair("title", "Untitle");

                    sw.Restart();
                    errno = client.set_metadata(group_name, remote_filename, meta_list, ProtoCommon.STORAGE_SET_METADATA_FLAG_MERGE);
                    sw.Stop();
                    Console.WriteLine("set_metadata time used: " + sw.ElapsedMilliseconds + " ms");
                    if (errno == 0)
                    {
                        Console.WriteLine("set_metadata success");
                    }
                    else
                    {
                        Console.WriteLine("set_metadata fail, error no: " + errno);
                    }

                    meta_list = client.get_metadata(group_name, remote_filename);
                    if (meta_list != null)
                    {
                        for (int i = 0; i < meta_list.Length; i++)
                        {
                            Console.WriteLine(meta_list[i]._Name + " " + meta_list[i]._Value);
                        }
                    }

                    sw.Restart();
                    file_buff = client.download_file(group_name, remote_filename);
                    sw.Stop();
                    Console.WriteLine("download_file time used: " + sw.ElapsedMilliseconds + " ms");

                    if (file_buff != null)
                    {
                        Console.WriteLine("file length:" + file_buff.Length);
                        Console.WriteLine(Encoding.GetEncoding(ClientGlobal.g_charset).GetString(file_buff));
                    }

                    file_buff = Encoding.GetEncoding(ClientGlobal.g_charset).GetBytes("this is a slave buff");
                    appender_filename = remote_filename;
                    file_ext_name = "txt";
                    sw.Restart();
                    errno = client.append_file(group_name, appender_filename, file_buff);
                    sw.Stop();
                    Console.WriteLine("append_file time used: " + sw.ElapsedMilliseconds + " ms");
                    if (errno == 0)
                    {
                        Console.WriteLine(client.get_file_info(group_name, appender_filename));
                    }
                    else
                    {
                        Console.WriteLine("append file fail, error no: " + errno);
                    }

                    sw.Restart();
                    errno = client.delete_file(group_name, remote_filename);
                    sw.Stop();
                    Console.WriteLine("delete_file time used: " + sw.ElapsedMilliseconds + " ms");
                    if (errno == 0)
                    {
                        Console.WriteLine("Delete file success");
                    }
                    else
                    {
                        Console.WriteLine("Delete file fail, error no: " + errno);
                    }
                }

                results = client.upload_appender_file(local_filename, null, meta_list);
                if (results != null)
                {
                    string file_id;
                    int ts;
                    string token;
                    string file_url;
                    IPEndPoint inetSockAddr;

                    group_name = results[0];
                    remote_filename = results[1];
                    file_id = group_name + StorageClientEx.SPLIT_GROUP_NAME_AND_FILENAME_SEPERATOR + remote_filename;

                    inetSockAddr = trackerServer.InetSockAddr;
                    file_url = "http://" + inetSockAddr.Address;
                    if (ClientGlobal.g_tracker_http_port != 80)
                    {
                        file_url += ":" + ClientGlobal.g_tracker_http_port;
                    }
                    file_url += "/" + file_id;
                    if (ClientGlobal.g_anti_steal_token)
                    {
                        ts = (int)ProtoCommon.DateTimeToUnixTimestamp(DateTime.Now);
                        token = ProtoCommon.getToken(file_id, ts, ClientGlobal.g_secret_key);
                        file_url += "?token=" + token + "&ts=" + ts;
                    }

                    Console.WriteLine("group_name: " + group_name + ", remote_filename: " + remote_filename);
                    Console.WriteLine(client.get_file_info(group_name, remote_filename));
                    Console.WriteLine("file url: " + file_url);

                    errno = client.download_file(group_name, remote_filename, 0, 0, "d:\\" + remote_filename.Replace("/", "_"));
                    if (errno == 0)
                    {
                        Console.WriteLine("Download file success");
                    }
                    else
                    {
                        Console.WriteLine("Download file fail, error no: " + errno);
                    }

                    errno = client.download_file(group_name, remote_filename, 0, 0, new DownloadFileWriter("d:\\" + remote_filename.Replace("/", "-")));
                    if (errno == 0)
                    {
                        Console.WriteLine("Download file success");
                    }
                    else
                    {
                        Console.WriteLine("Download file fail, error no: " + errno);
                    }

                    appender_filename = remote_filename;
                    file_ext_name = null;
                    sw.Restart();
                    errno = client.append_file(group_name, appender_filename, local_filename);
                    sw.Stop();
                    Console.WriteLine("append_file time used: " + sw.ElapsedMilliseconds + " ms");
                    if (errno == 0)
                    {
                        Console.WriteLine(client.get_file_info(group_name, appender_filename));
                    }
                    else
                    {
                        Console.WriteLine("append file fail, error no: " + errno);
                    }
                }

                System.IO.FileInfo f;
                f = new System.IO.FileInfo(local_filename);
                file_ext_name = Path.GetExtension(local_filename).Trim('.');

                results = client.upload_appender_file(null, f.Length,
                   new UploadLocalFileSender(local_filename), file_ext_name, meta_list);
                if (results != null)
                {
                    group_name = results[0];
                    remote_filename = results[1];

                    Console.WriteLine("group name: " + group_name + ", remote filename: " + remote_filename);
                    Console.WriteLine(client.get_file_info(group_name, remote_filename));

                    appender_filename = remote_filename;
                    sw.Restart();
                    errno = client.append_file(group_name, appender_filename, f.Length, new UploadLocalFileSender(local_filename));
                    sw.Stop();
                    Console.WriteLine("append_file time used: " + sw.ElapsedMilliseconds + " ms");
                    if (errno == 0)
                    {
                        Console.WriteLine(client.get_file_info(group_name, appender_filename));
                    }
                    else
                    {
                        Console.WriteLine("append file fail, error no: " + errno);
                    }

                    sw.Restart();
                    errno = client.modify_file(group_name, appender_filename, 0, f.Length, new UploadLocalFileSender(local_filename));
                    sw.Stop();
                    Console.WriteLine("modify_file time used: " + sw.ElapsedMilliseconds + " ms");
                    if (errno == 0)
                    {
                        Console.WriteLine(client.get_file_info(group_name, appender_filename));
                    }
                    else
                    {
                        Console.WriteLine("modify file fail, error no: " + errno);
                    }

                    sw.Restart();
                    errno = client.truncate_file(group_name, appender_filename);
                    sw.Stop();
                    Console.WriteLine("truncate_file time used: " + sw.ElapsedMilliseconds + " ms");
                    if (errno == 0)
                    {
                        Console.WriteLine(client.get_file_info(group_name, appender_filename));
                    }
                    else
                    {
                        Console.WriteLine("truncate file fail, error no: " + errno);
                    }
                }
                else
                {
                    Console.WriteLine("Upload file fail, error no: " + errno);
                }

                storageServer = tracker.getFetchStorage(trackerServer, group_name, remote_filename);
                if (storageServer == null)
                {
                    Console.WriteLine("getFetchStorage fail, errno code: " + tracker.ErrorCode);
                    return;
                }

                Console.WriteLine("active test to storage server: " + ProtoCommon.activeTest(storageServer.getSocket()));

                storageServer.close();

                Console.WriteLine("active test to tracker server: " + ProtoCommon.activeTest(trackerServer.getSocket()));

                trackerServer.close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }
    }
}
