using System;
using fastdfs.client.net;

namespace fastdfstest
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class Monitor
    {
        public static void Run(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Error: Must have 1 parameter: config filename");
                return;
            }

            try
            {
                ClientGlobal.init(args[0]);
                TrackerClient tc = new TrackerClient();
                TrackerServer tracker = tc.getConnection();
                if (tracker == null) return;

                int count = 0;
                StructGroupStat[] groupStats = tc.listGroups(tracker);
                if (groupStats == null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("ERROR! list groups error, error no: " + tc.ErrorCode);
                    Console.WriteLine("");
                    return;
                }
                Console.WriteLine("group count: " + groupStats.Length);

                foreach (StructGroupStat groupStat in groupStats)
                {
                    count++;
                    Console.WriteLine("Group " + count + ":");
                    Console.WriteLine("group name = " + groupStat.GroupName);
                    Console.WriteLine("disk total space = " + groupStat.TotalMB + "MB");
                    Console.WriteLine("disk free space = " + groupStat.FreeMB + " MB");
                    Console.WriteLine("trunk free space = " + groupStat.TrunkFreeMB + " MB");
                    Console.WriteLine("storage server count = " + groupStat.StorageCount);
                    Console.WriteLine("active server count = " + groupStat.ActiveCount);
                    Console.WriteLine("storage server port = " + groupStat.StoragePort);
                    Console.WriteLine("storage HTTP port = " + groupStat.StorageHttpPort);
                    Console.WriteLine("store path count = " + groupStat.StorePathCount);
                    Console.WriteLine("subdir count per path = " + groupStat.SubdirCountPerPath);
                    Console.WriteLine("current write server index = " + groupStat.CurrentWriteServer);
                    Console.WriteLine("current trunk file id = " + groupStat.CurrentTrunkFileId);

                    StructStorageStat[] storageStats = tc.listStorages(tracker, groupStat.GroupName);
                    if (storageStats == null)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("ERROR! list storage error, error no: " + tc.ErrorCode);
                        Console.WriteLine("");
                        break;
                    }

                    int stroageCount = 0;
                    foreach (StructStorageStat storageStat in storageStats)
                    {
                        stroageCount++;
                        Console.WriteLine("\tStorage " + stroageCount + ":");
                        Console.WriteLine("\t\tstorage id = " + storageStat.Id);
                        Console.WriteLine("\t\tip_addr = " + storageStat.IpAddr + "  " + ProtoCommon.getStorageStatusCaption(storageStat.Status));
                        Console.WriteLine("\t\thttp domain = " + storageStat.DomainName);
                        Console.WriteLine("\t\tversion = " + storageStat.Version);
                        Console.WriteLine("\t\tjoin time = " + storageStat.JoinTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        Console.WriteLine("\t\tup time = " + storageStat.UpTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        Console.WriteLine("\t\ttotal storage = " + storageStat.TotalMB + "MB");
                        Console.WriteLine("\t\tfree storage = " + storageStat.FreeMB + "MB");
                        Console.WriteLine("\t\tupload priority = " + storageStat.UploadPriority);
                        Console.WriteLine("\t\tstore_path_count = " + storageStat.StorePathCount);
                        Console.WriteLine("\t\tsubdir_count_per_path = " + storageStat.SubdirCountPerPath);
                        Console.WriteLine("\t\tstorage_port = " + storageStat.StoragePort);
                        Console.WriteLine("\t\tstorage_http_port = " + storageStat.StorageHttpPort);
                        Console.WriteLine("\t\tcurrent_write_path = " + storageStat.CurrentWritePath);
                        Console.WriteLine("\t\tsource ip_addr = " + storageStat.SrcIpAddr);
                        Console.WriteLine("\t\tif_trunk_server = " + storageStat.IfTrunkServer);
                        Console.WriteLine("\t\tconntion.alloc_count  = " + storageStat.ConnectionAllocCount);
                        Console.WriteLine("\t\tconntion.current_count  = " + storageStat.ConnectionCurrentCount);
                        Console.WriteLine("\t\tconntion.max_count  = " + storageStat.ConnectionMaxCount);
                        Console.WriteLine("\t\ttotal_upload_count = " + storageStat.TotalUploadCount);
                        Console.WriteLine("\t\tsuccess_upload_count = " + storageStat.SuccessUploadCount);
                        Console.WriteLine("\t\ttotal_append_count = " + storageStat.TotalAppendCount);
                        Console.WriteLine("\t\tsuccess_append_count = " + storageStat.SuccessAppendCount);
                        Console.WriteLine("\t\ttotal_modify_count = " + storageStat.TotalModifyCount);
                        Console.WriteLine("\t\tsuccess_modify_count = " + storageStat.SuccessModifyCount);
                        Console.WriteLine("\t\ttotal_truncate_count = " + storageStat.TotalTruncateCount);
                        Console.WriteLine("\t\tsuccess_truncate_count = " + storageStat.SuccessTruncateCount);
                        Console.WriteLine("\t\ttotal_set_meta_count = " + storageStat.TotalSetMetaCount);
                        Console.WriteLine("\t\tsuccess_set_meta_count = " + storageStat.SuccessSetMetaCount);
                        Console.WriteLine("\t\ttotal_delete_count = " + storageStat.TotalDeleteCount);
                        Console.WriteLine("\t\tsuccess_delete_count = " + storageStat.SuccessDeleteCount);
                        Console.WriteLine("\t\ttotal_download_count = " + storageStat.TotalDownloadCount);
                        Console.WriteLine("\t\tsuccess_download_count = " + storageStat.SuccessDownloadCount);
                        Console.WriteLine("\t\ttotal_get_meta_count = " + storageStat.TotalGetMetaCount);
                        Console.WriteLine("\t\tsuccess_get_meta_count = " + storageStat.SuccessGetMetaCount);
                        Console.WriteLine("\t\ttotal_create_link_count = " + storageStat.TotalCreateLinkCount);
                        Console.WriteLine("\t\tsuccess_create_link_count = " + storageStat.SuccessCreateLinkCount);
                        Console.WriteLine("\t\ttotal_delete_link_count = " + storageStat.TotalDeleteLinkCount);
                        Console.WriteLine("\t\tsuccess_delete_link_count = " + storageStat.SuccessDeleteLinkCount);
                        Console.WriteLine("\t\ttotal_upload_bytes = " + storageStat.TotalUploadBytes);
                        Console.WriteLine("\t\tsuccess_upload_bytes = " + storageStat.SuccessUploadBytes);
                        Console.WriteLine("\t\ttotal_append_bytes = " + storageStat.TotalAppendBytes);
                        Console.WriteLine("\t\tsuccess_append_bytes = " + storageStat.SuccessAppendBytes);
                        Console.WriteLine("\t\ttotal_modify_bytes = " + storageStat.TotalModifyBytes);
                        Console.WriteLine("\t\tsuccess_modify_bytes = " + storageStat.SuccessModifyBytes);
                        Console.WriteLine("\t\ttotal_download_bytes = " + storageStat.TotalDownloadloadBytes);
                        Console.WriteLine("\t\tsuccess_download_bytes = " + storageStat.SuccessDownloadloadBytes);
                        Console.WriteLine("\t\ttotal_sync_in_bytes = " + storageStat.TotalSyncInBytes);
                        Console.WriteLine("\t\tsuccess_sync_in_bytes = " + storageStat.SuccessSyncInBytes);
                        Console.WriteLine("\t\ttotal_sync_out_bytes = " + storageStat.TotalSyncOutBytes);
                        Console.WriteLine("\t\tsuccess_sync_out_bytes = " + storageStat.SuccessSyncOutBytes);
                        Console.WriteLine("\t\ttotal_file_open_count = " + storageStat.TotalFileOpenCount);
                        Console.WriteLine("\t\tsuccess_file_open_count = " + storageStat.SuccessFileOpenCount);
                        Console.WriteLine("\t\ttotal_file_read_count = " + storageStat.TotalFileReadCount);
                        Console.WriteLine("\t\tsuccess_file_read_count = " + storageStat.SuccessFileReadCount);
                        Console.WriteLine("\t\ttotal_file_write_count = " + storageStat.TotalFileWriteCount);
                        Console.WriteLine("\t\tsuccess_file_write_count = " + storageStat.SuccessFileWriteCount);
                        Console.WriteLine("\t\tlast_heart_beat_time = " + storageStat.LastHeartBeatTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        Console.WriteLine("\t\tlast_source_update = " + storageStat.LastSourceUpdate.ToString("yyyy-MM-dd HH:mm:ss"));
                        Console.WriteLine("\t\tlast_sync_update = " + storageStat.LastSyncUpdate.ToString("yyyy-MM-dd HH:mm:ss"));
                        //Console.WriteLine("\t\tlast_synced_timestamp = " + storageStat.LastSyncedTimestamp.ToString("yyyy-MM-dd HH:mm:ss") + getSyncedDelayString(storageStats, storageStat));
                    }
                }
                tracker.close();

                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
