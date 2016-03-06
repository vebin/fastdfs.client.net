using System;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class StructStorageStat : StructBase
    {
        protected static int FIELD_INDEX_STATUS = 0;
        protected static int FIELD_INDEX_ID = 1;
        protected static int FIELD_INDEX_IP_ADDR = 2;
        protected static int FIELD_INDEX_DOMAIN_NAME = 3;
        protected static int FIELD_INDEX_SRC_IP_ADDR = 4;
        protected static int FIELD_INDEX_VERSION = 5;
        protected static int FIELD_INDEX_JOIN_TIME = 6;
        protected static int FIELD_INDEX_UP_TIME = 7;
        protected static int FIELD_INDEX_TOTAL_MB = 8;
        protected static int FIELD_INDEX_FREE_MB = 9;
        protected static int FIELD_INDEX_UPLOAD_PRIORITY = 10;
        protected static int FIELD_INDEX_STORE_PATH_COUNT = 11;
        protected static int FIELD_INDEX_SUBDIR_COUNT_PER_PATH = 12;
        protected static int FIELD_INDEX_CURRENT_WRITE_PATH = 13;
        protected static int FIELD_INDEX_STORAGE_PORT = 14;
        protected static int FIELD_INDEX_STORAGE_HTTP_PORT = 15;

        protected static int FIELD_INDEX_CONNECTION_ALLOC_COUNT = 16;
        protected static int FIELD_INDEX_CONNECTION_CURRENT_COUNT = 17;
        protected static int FIELD_INDEX_CONNECTION_MAX_COUNT = 18;

        protected static int FIELD_INDEX_TOTAL_UPLOAD_COUNT = 19;
        protected static int FIELD_INDEX_SUCCESS_UPLOAD_COUNT = 20;
        protected static int FIELD_INDEX_TOTAL_APPEND_COUNT = 21;
        protected static int FIELD_INDEX_SUCCESS_APPEND_COUNT = 22;
        protected static int FIELD_INDEX_TOTAL_MODIFY_COUNT = 23;
        protected static int FIELD_INDEX_SUCCESS_MODIFY_COUNT = 24;
        protected static int FIELD_INDEX_TOTAL_TRUNCATE_COUNT = 25;
        protected static int FIELD_INDEX_SUCCESS_TRUNCATE_COUNT = 26;
        protected static int FIELD_INDEX_TOTAL_SET_META_COUNT = 27;
        protected static int FIELD_INDEX_SUCCESS_SET_META_COUNT = 28;
        protected static int FIELD_INDEX_TOTAL_DELETE_COUNT = 29;
        protected static int FIELD_INDEX_SUCCESS_DELETE_COUNT = 30;
        protected static int FIELD_INDEX_TOTAL_DOWNLOAD_COUNT = 31;
        protected static int FIELD_INDEX_SUCCESS_DOWNLOAD_COUNT = 32;
        protected static int FIELD_INDEX_TOTAL_GET_META_COUNT = 33;
        protected static int FIELD_INDEX_SUCCESS_GET_META_COUNT = 34;
        protected static int FIELD_INDEX_TOTAL_CREATE_LINK_COUNT = 35;
        protected static int FIELD_INDEX_SUCCESS_CREATE_LINK_COUNT = 36;
        protected static int FIELD_INDEX_TOTAL_DELETE_LINK_COUNT = 37;
        protected static int FIELD_INDEX_SUCCESS_DELETE_LINK_COUNT = 38;
        protected static int FIELD_INDEX_TOTAL_UPLOAD_BYTES = 39;
        protected static int FIELD_INDEX_SUCCESS_UPLOAD_BYTES = 40;
        protected static int FIELD_INDEX_TOTAL_APPEND_BYTES = 41;
        protected static int FIELD_INDEX_SUCCESS_APPEND_BYTES = 42;
        protected static int FIELD_INDEX_TOTAL_MODIFY_BYTES = 43;
        protected static int FIELD_INDEX_SUCCESS_MODIFY_BYTES = 44;
        protected static int FIELD_INDEX_TOTAL_DOWNLOAD_BYTES = 45;
        protected static int FIELD_INDEX_SUCCESS_DOWNLOAD_BYTES = 46;
        protected static int FIELD_INDEX_TOTAL_SYNC_IN_BYTES = 47;
        protected static int FIELD_INDEX_SUCCESS_SYNC_IN_BYTES = 48;
        protected static int FIELD_INDEX_TOTAL_SYNC_OUT_BYTES = 49;
        protected static int FIELD_INDEX_SUCCESS_SYNC_OUT_BYTES = 50;
        protected static int FIELD_INDEX_TOTAL_FILE_OPEN_COUNT = 51;
        protected static int FIELD_INDEX_SUCCESS_FILE_OPEN_COUNT = 52;
        protected static int FIELD_INDEX_TOTAL_FILE_READ_COUNT = 53;
        protected static int FIELD_INDEX_SUCCESS_FILE_READ_COUNT = 54;
        protected static int FIELD_INDEX_TOTAL_FILE_WRITE_COUNT = 55;
        protected static int FIELD_INDEX_SUCCESS_FILE_WRITE_COUNT = 56;
        protected static int FIELD_INDEX_LAST_SOURCE_UPDATE = 57;
        protected static int FIELD_INDEX_LAST_SYNC_UPDATE = 58;
        protected static int FIELD_INDEX_LAST_SYNCED_TIMESTAMP = 59;
        protected static int FIELD_INDEX_LAST_HEART_BEAT_TIME = 60;
        protected static int FIELD_INDEX_IF_TRUNK_FILE = 61;

        protected static int fieldsTotalSize;
        protected static StructBase.FieldInfo[] fieldsArray = new StructBase.FieldInfo[62];

        static StructStorageStat()
        {
            int offset = 0;

            fieldsArray[FIELD_INDEX_STATUS] = new StructBase.FieldInfo("status", offset, 1);
            offset += 1;

            fieldsArray[FIELD_INDEX_ID] = new StructBase.FieldInfo("id", offset, ProtoCommon.FDFS_STORAGE_ID_MAX_SIZE);
            offset += ProtoCommon.FDFS_STORAGE_ID_MAX_SIZE;

            fieldsArray[FIELD_INDEX_IP_ADDR] = new StructBase.FieldInfo("ipAddr", offset, ProtoCommon.FDFS_IPADDR_SIZE);
            offset += ProtoCommon.FDFS_IPADDR_SIZE;

            fieldsArray[FIELD_INDEX_DOMAIN_NAME] = new StructBase.FieldInfo("domainName", offset, ProtoCommon.FDFS_DOMAIN_NAME_MAX_SIZE);
            offset += ProtoCommon.FDFS_DOMAIN_NAME_MAX_SIZE;

            fieldsArray[FIELD_INDEX_SRC_IP_ADDR] = new StructBase.FieldInfo("srcIpAddr", offset, ProtoCommon.FDFS_IPADDR_SIZE);
            offset += ProtoCommon.FDFS_IPADDR_SIZE;

            fieldsArray[FIELD_INDEX_VERSION] = new StructBase.FieldInfo("version", offset, ProtoCommon.FDFS_VERSION_SIZE);
            offset += ProtoCommon.FDFS_VERSION_SIZE;

            fieldsArray[FIELD_INDEX_JOIN_TIME] = new StructBase.FieldInfo("joinTime", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_UP_TIME] = new StructBase.FieldInfo("upTime", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_MB] = new StructBase.FieldInfo("totalMB", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_FREE_MB] = new StructBase.FieldInfo("freeMB", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_UPLOAD_PRIORITY] = new StructBase.FieldInfo("uploadPriority", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_STORE_PATH_COUNT] = new StructBase.FieldInfo("storePathCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUBDIR_COUNT_PER_PATH] = new StructBase.FieldInfo("subdirCountPerPath", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_CURRENT_WRITE_PATH] = new StructBase.FieldInfo("currentWritePath", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_STORAGE_PORT] = new StructBase.FieldInfo("storagePort", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_STORAGE_HTTP_PORT] = new StructBase.FieldInfo("storageHttpPort", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_CONNECTION_ALLOC_COUNT] = new StructBase.FieldInfo("connectionAllocCount", offset, 4);
            offset += 4;

            fieldsArray[FIELD_INDEX_CONNECTION_CURRENT_COUNT] = new StructBase.FieldInfo("connectionCurrentCount", offset, 4);
            offset += 4;

            fieldsArray[FIELD_INDEX_CONNECTION_MAX_COUNT] = new StructBase.FieldInfo("connectionMaxCount", offset, 4);
            offset += 4;

            fieldsArray[FIELD_INDEX_TOTAL_UPLOAD_COUNT] = new StructBase.FieldInfo("totalUploadCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_UPLOAD_COUNT] = new StructBase.FieldInfo("successUploadCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_APPEND_COUNT] = new StructBase.FieldInfo("totalAppendCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_APPEND_COUNT] = new StructBase.FieldInfo("successAppendCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_MODIFY_COUNT] = new StructBase.FieldInfo("totalModifyCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_MODIFY_COUNT] = new StructBase.FieldInfo("successModifyCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_TRUNCATE_COUNT] = new StructBase.FieldInfo("totalTruncateCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_TRUNCATE_COUNT] = new StructBase.FieldInfo("successTruncateCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_SET_META_COUNT] = new StructBase.FieldInfo("totalSetMetaCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_SET_META_COUNT] = new StructBase.FieldInfo("successSetMetaCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_DELETE_COUNT] = new StructBase.FieldInfo("totalDeleteCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_DELETE_COUNT] = new StructBase.FieldInfo("successDeleteCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_DOWNLOAD_COUNT] = new StructBase.FieldInfo("totalDownloadCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_DOWNLOAD_COUNT] = new StructBase.FieldInfo("successDownloadCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_GET_META_COUNT] = new StructBase.FieldInfo("totalGetMetaCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_GET_META_COUNT] = new StructBase.FieldInfo("successGetMetaCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_CREATE_LINK_COUNT] = new StructBase.FieldInfo("totalCreateLinkCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_CREATE_LINK_COUNT] = new StructBase.FieldInfo("successCreateLinkCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_DELETE_LINK_COUNT] = new StructBase.FieldInfo("totalDeleteLinkCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_DELETE_LINK_COUNT] = new StructBase.FieldInfo("successDeleteLinkCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_UPLOAD_BYTES] = new StructBase.FieldInfo("totalUploadBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_UPLOAD_BYTES] = new StructBase.FieldInfo("successUploadBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_APPEND_BYTES] = new StructBase.FieldInfo("totalAppendBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_APPEND_BYTES] = new StructBase.FieldInfo("successAppendBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_MODIFY_BYTES] = new StructBase.FieldInfo("totalModifyBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_MODIFY_BYTES] = new StructBase.FieldInfo("successModifyBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_DOWNLOAD_BYTES] = new StructBase.FieldInfo("totalDownloadloadBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_DOWNLOAD_BYTES] = new StructBase.FieldInfo("successDownloadloadBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_SYNC_IN_BYTES] = new StructBase.FieldInfo("totalSyncInBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_SYNC_IN_BYTES] = new StructBase.FieldInfo("successSyncInBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_SYNC_OUT_BYTES] = new StructBase.FieldInfo("totalSyncOutBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_SYNC_OUT_BYTES] = new StructBase.FieldInfo("successSyncOutBytes", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_FILE_OPEN_COUNT] = new StructBase.FieldInfo("totalFileOpenCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_FILE_OPEN_COUNT] = new StructBase.FieldInfo("successFileOpenCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_FILE_READ_COUNT] = new StructBase.FieldInfo("totalFileReadCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_FILE_READ_COUNT] = new StructBase.FieldInfo("successFileReadCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TOTAL_FILE_WRITE_COUNT] = new StructBase.FieldInfo("totalFileWriteCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUCCESS_FILE_WRITE_COUNT] = new StructBase.FieldInfo("successFileWriteCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_LAST_SOURCE_UPDATE] = new StructBase.FieldInfo("lastSourceUpdate", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_LAST_SYNC_UPDATE] = new StructBase.FieldInfo("lastSyncUpdate", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_LAST_SYNCED_TIMESTAMP] = new StructBase.FieldInfo("lastSyncedTimestamp", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_LAST_HEART_BEAT_TIME] = new StructBase.FieldInfo("lastHeartBeatTime", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_IF_TRUNK_FILE] = new StructBase.FieldInfo("ifTrunkServer", offset, 1);
            offset += 1;

            fieldsTotalSize = offset;
        }

        protected byte status;
        protected string id;
        protected string ipAddr;
        protected string srcIpAddr;
        protected string domainName; //http domain name
        protected string version;
        protected long totalMB; //total disk storage in MB
        protected long freeMB;  //free disk storage in MB
        protected int uploadPriority;  //upload priority
        protected DateTime joinTime; //storage join timestamp (create timestamp)
        protected DateTime upTime;   //storage service started timestamp
        protected int storePathCount;  //store base path count of each storage server
        protected int subdirCountPerPath;
        protected int storagePort;
        protected int storageHttpPort; //storage http server port
        protected int currentWritePath; //current write path index
        protected int connectionAllocCount;
        protected int connectionCurrentCount;
        protected int connectionMaxCount;
        protected long totalUploadCount;
        protected long successUploadCount;
        protected long totalAppendCount;
        protected long successAppendCount;
        protected long totalModifyCount;
        protected long successModifyCount;
        protected long totalTruncateCount;
        protected long successTruncateCount;
        protected long totalSetMetaCount;
        protected long successSetMetaCount;
        protected long totalDeleteCount;
        protected long successDeleteCount;
        protected long totalDownloadCount;
        protected long successDownloadCount;
        protected long totalGetMetaCount;
        protected long successGetMetaCount;
        protected long totalCreateLinkCount;
        protected long successCreateLinkCount;
        protected long totalDeleteLinkCount;
        protected long successDeleteLinkCount;
        protected long totalUploadBytes;
        protected long successUploadBytes;
        protected long totalAppendBytes;
        protected long successAppendBytes;
        protected long totalModifyBytes;
        protected long successModifyBytes;
        protected long totalDownloadloadBytes;
        protected long successDownloadloadBytes;
        protected long totalSyncInBytes;
        protected long successSyncInBytes;
        protected long totalSyncOutBytes;
        protected long successSyncOutBytes;
        protected long totalFileOpenCount;
        protected long successFileOpenCount;
        protected long totalFileReadCount;
        protected long successFileReadCount;
        protected long totalFileWriteCount;
        protected long successFileWriteCount;
        protected DateTime lastSourceUpdate;
        protected DateTime lastSyncUpdate;
        protected DateTime lastSyncedTimestamp;
        protected DateTime lastHeartBeatTime;
        protected bool ifTrunkServer;

        public byte Status { get { return this.status; } }
        public string Id { get { return this.id; } }
        public string IpAddr { get { return this.ipAddr; } }
        public string SrcIpAddr { get { return this.srcIpAddr; } }
        public string DomainName { get { return this.domainName; } }
        public string Version { get { return this.version; } }
        public long TotalMB { get { return this.totalMB; } }
        public long FreeMB { get { return this.freeMB; } }
        public int UploadPriority { get { return this.uploadPriority; } }
        public DateTime JoinTime { get { return this.joinTime; } }
        public DateTime UpTime { get { return this.upTime; } }
        public int StorePathCount { get { return this.storePathCount; } }
        public int SubdirCountPerPath { get { return this.subdirCountPerPath; } }
        public int StoragePort { get { return this.storagePort; } }
        public int StorageHttpPort { get { return this.storageHttpPort; } }
        public int CurrentWritePath { get { return this.currentWritePath; } }
        public int ConnectionAllocCount { get { return this.connectionAllocCount; } }
        public int ConnectionCurrentCount { get { return this.connectionCurrentCount; } }
        public int ConnectionMaxCount { get { return this.connectionMaxCount; } }
        public long TotalUploadCount { get { return this.totalUploadCount; } }
        public long SuccessUploadCount { get { return this.successUploadCount; } }
        public long TotalAppendCount { get { return this.totalAppendCount; } }
        public long SuccessAppendCount { get { return this.successAppendCount; } }
        public long TotalModifyCount { get { return this.totalModifyCount; } }
        public long SuccessModifyCount { get { return this.successModifyCount; } }
        public long TotalTruncateCount { get { return this.totalTruncateCount; } }
        public long SuccessTruncateCount { get { return this.successTruncateCount; } }
        public long TotalSetMetaCount { get { return this.totalSetMetaCount; } }
        public long SuccessSetMetaCount { get { return this.successSetMetaCount; } }
        public long TotalDeleteCount { get { return this.totalDeleteCount; } }
        public long SuccessDeleteCount { get { return this.successDeleteCount; } }
        public long TotalDownloadCount { get { return this.totalDownloadCount; } }
        public long SuccessDownloadCount { get { return this.successDownloadCount; } }
        public long TotalGetMetaCount { get { return this.totalGetMetaCount; } }
        public long SuccessGetMetaCount { get { return this.successGetMetaCount; } }
        public long TotalCreateLinkCount { get { return this.totalCreateLinkCount; } }
        public long SuccessCreateLinkCount { get { return this.successCreateLinkCount; } }
        public long TotalDeleteLinkCount { get { return this.totalDeleteLinkCount; } }
        public long SuccessDeleteLinkCount { get { return this.successDeleteLinkCount; } }
        public long TotalUploadBytes { get { return this.totalUploadBytes; } }
        public long SuccessUploadBytes { get { return this.successUploadBytes; } }
        public long TotalAppendBytes { get { return this.totalAppendBytes; } }
        public long SuccessAppendBytes { get { return this.successAppendBytes; } }
        public long TotalModifyBytes { get { return this.totalModifyBytes; } }
        public long SuccessModifyBytes { get { return this.successModifyBytes; } }
        public long TotalDownloadloadBytes { get { return this.totalDownloadloadBytes; } }
        public long SuccessDownloadloadBytes { get { return this.successDownloadloadBytes; } }
        public long TotalSyncInBytes { get { return this.totalSyncInBytes; } }
        public long SuccessSyncInBytes { get { return this.successSyncInBytes; } }
        public long TotalSyncOutBytes { get { return this.totalSyncOutBytes; } }
        public long SuccessSyncOutBytes { get { return this.successSyncOutBytes; } }
        public long TotalFileOpenCount { get { return this.totalFileOpenCount; } }
        public long SuccessFileOpenCount { get { return this.successFileOpenCount; } }
        public long TotalFileReadCount { get { return this.totalFileReadCount; } }
        public long SuccessFileReadCount { get { return this.successFileReadCount; } }
        public long TotalFileWriteCount { get { return this.totalFileWriteCount; } }
        public long SuccessFileWriteCount { get { return this.successFileWriteCount; } }
        public DateTime LastSourceUpdate { get { return this.lastSourceUpdate; } }
        public DateTime LastSyncUpdate { get { return this.lastSyncUpdate; } }
        public DateTime LastSyncedTimestamp { get { return this.lastSyncedTimestamp; } }
        public DateTime LastHeartBeatTime { get { return this.lastHeartBeatTime; } }
        public bool IfTrunkServer { get { return this.ifTrunkServer; } }

        public override void setFields(byte[] bs, int offset)
        {
            this.status = byteValue(bs, offset, fieldsArray[FIELD_INDEX_STATUS]);
            this.id = stringValue(bs, offset, fieldsArray[FIELD_INDEX_ID]);
            this.ipAddr = stringValue(bs, offset, fieldsArray[FIELD_INDEX_IP_ADDR]);
            this.srcIpAddr = stringValue(bs, offset, fieldsArray[FIELD_INDEX_SRC_IP_ADDR]);
            this.domainName = stringValue(bs, offset, fieldsArray[FIELD_INDEX_DOMAIN_NAME]);
            this.version = stringValue(bs, offset, fieldsArray[FIELD_INDEX_VERSION]);
            this.totalMB = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_MB]);
            this.freeMB = longValue(bs, offset, fieldsArray[FIELD_INDEX_FREE_MB]);
            this.uploadPriority = intValue(bs, offset, fieldsArray[FIELD_INDEX_UPLOAD_PRIORITY]);
            this.joinTime = dateValue(bs, offset, fieldsArray[FIELD_INDEX_JOIN_TIME]);
            this.upTime = dateValue(bs, offset, fieldsArray[FIELD_INDEX_UP_TIME]);
            this.storePathCount = intValue(bs, offset, fieldsArray[FIELD_INDEX_STORE_PATH_COUNT]);
            this.subdirCountPerPath = intValue(bs, offset, fieldsArray[FIELD_INDEX_SUBDIR_COUNT_PER_PATH]);
            this.storagePort = intValue(bs, offset, fieldsArray[FIELD_INDEX_STORAGE_PORT]);
            this.storageHttpPort = intValue(bs, offset, fieldsArray[FIELD_INDEX_STORAGE_HTTP_PORT]);
            this.currentWritePath = intValue(bs, offset, fieldsArray[FIELD_INDEX_CURRENT_WRITE_PATH]);

            this.connectionAllocCount = int32Value(bs, offset, fieldsArray[FIELD_INDEX_CONNECTION_ALLOC_COUNT]);
            this.connectionCurrentCount = int32Value(bs, offset, fieldsArray[FIELD_INDEX_CONNECTION_CURRENT_COUNT]);
            this.connectionMaxCount = int32Value(bs, offset, fieldsArray[FIELD_INDEX_CONNECTION_MAX_COUNT]);

            this.totalUploadCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_UPLOAD_COUNT]);
            this.successUploadCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_UPLOAD_COUNT]);
            this.totalAppendCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_APPEND_COUNT]);
            this.successAppendCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_APPEND_COUNT]);
            this.totalModifyCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_MODIFY_COUNT]);
            this.successModifyCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_MODIFY_COUNT]);
            this.totalTruncateCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_TRUNCATE_COUNT]);
            this.successTruncateCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_TRUNCATE_COUNT]);
            this.totalSetMetaCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_SET_META_COUNT]);
            this.successSetMetaCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_SET_META_COUNT]);
            this.totalDeleteCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_DELETE_COUNT]);
            this.successDeleteCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_DELETE_COUNT]);
            this.totalDownloadCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_DOWNLOAD_COUNT]);
            this.successDownloadCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_DOWNLOAD_COUNT]);
            this.totalGetMetaCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_GET_META_COUNT]);
            this.successGetMetaCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_GET_META_COUNT]);
            this.totalCreateLinkCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_CREATE_LINK_COUNT]);
            this.successCreateLinkCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_CREATE_LINK_COUNT]);
            this.totalDeleteLinkCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_DELETE_LINK_COUNT]);
            this.successDeleteLinkCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_DELETE_LINK_COUNT]);
            this.totalUploadBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_UPLOAD_BYTES]);
            this.successUploadBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_UPLOAD_BYTES]);
            this.totalAppendBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_APPEND_BYTES]);
            this.successAppendBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_APPEND_BYTES]);
            this.totalModifyBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_MODIFY_BYTES]);
            this.successModifyBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_MODIFY_BYTES]);
            this.totalDownloadloadBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_DOWNLOAD_BYTES]);
            this.successDownloadloadBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_DOWNLOAD_BYTES]);
            this.totalSyncInBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_SYNC_IN_BYTES]);
            this.successSyncInBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_SYNC_IN_BYTES]);
            this.totalSyncOutBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_SYNC_OUT_BYTES]);
            this.successSyncOutBytes = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_SYNC_OUT_BYTES]);
            this.totalFileOpenCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_FILE_OPEN_COUNT]);
            this.successFileOpenCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_FILE_OPEN_COUNT]);
            this.totalFileReadCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_FILE_READ_COUNT]);
            this.successFileReadCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_FILE_READ_COUNT]);
            this.totalFileWriteCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_FILE_WRITE_COUNT]);
            this.successFileWriteCount = longValue(bs, offset, fieldsArray[FIELD_INDEX_SUCCESS_FILE_WRITE_COUNT]);
            this.lastSourceUpdate = dateValue(bs, offset, fieldsArray[FIELD_INDEX_LAST_SOURCE_UPDATE]);
            this.lastSyncUpdate = dateValue(bs, offset, fieldsArray[FIELD_INDEX_LAST_SYNC_UPDATE]);
            this.lastSyncedTimestamp = dateValue(bs, offset, fieldsArray[FIELD_INDEX_LAST_SYNCED_TIMESTAMP]);
            this.lastHeartBeatTime = dateValue(bs, offset, fieldsArray[FIELD_INDEX_LAST_HEART_BEAT_TIME]);
            this.ifTrunkServer = booleanValue(bs, offset, fieldsArray[FIELD_INDEX_IF_TRUNK_FILE]);
        }

        public static int getFieldsTotalSize()
        {
            return fieldsTotalSize;
        }
    }
}
