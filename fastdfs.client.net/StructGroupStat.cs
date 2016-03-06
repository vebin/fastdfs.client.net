
namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class StructGroupStat : StructBase
    {
        protected static int FIELD_INDEX_GROUP_NAME = 0;
        protected static int FIELD_INDEX_TOTAL_MB = 1;
        protected static int FIELD_INDEX_FREE_MB = 2;
        protected static int FIELD_INDEX_TRUNK_FREE_MB = 3;
        protected static int FIELD_INDEX_STORAGE_COUNT = 4;
        protected static int FIELD_INDEX_STORAGE_PORT = 5;
        protected static int FIELD_INDEX_STORAGE_HTTP_PORT = 6;
        protected static int FIELD_INDEX_ACTIVE_COUNT = 7;
        protected static int FIELD_INDEX_CURRENT_WRITE_SERVER = 8;
        protected static int FIELD_INDEX_STORE_PATH_COUNT = 9;
        protected static int FIELD_INDEX_SUBDIR_COUNT_PER_PATH = 10;
        protected static int FIELD_INDEX_CURRENT_TRUNK_FILE_ID = 11;

        protected static int fieldsTotalSize;
        protected static StructBase.FieldInfo[] fieldsArray = new StructBase.FieldInfo[12];

        static StructGroupStat()
        {
            int offset = 0;
            fieldsArray[FIELD_INDEX_GROUP_NAME] = new StructBase.FieldInfo("groupName", offset, ProtoCommon.FDFS_GROUP_NAME_MAX_LEN + 1);
            offset += ProtoCommon.FDFS_GROUP_NAME_MAX_LEN + 1;

            fieldsArray[FIELD_INDEX_TOTAL_MB] = new StructBase.FieldInfo("totalMB", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_FREE_MB] = new StructBase.FieldInfo("freeMB", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_TRUNK_FREE_MB] = new StructBase.FieldInfo("trunkFreeMB", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_STORAGE_COUNT] = new StructBase.FieldInfo("storageCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_STORAGE_PORT] = new StructBase.FieldInfo("storagePort", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_STORAGE_HTTP_PORT] = new StructBase.FieldInfo("storageHttpPort", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_ACTIVE_COUNT] = new StructBase.FieldInfo("activeCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_CURRENT_WRITE_SERVER] = new StructBase.FieldInfo("currentWriteServer", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_STORE_PATH_COUNT] = new StructBase.FieldInfo("storePathCount", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_SUBDIR_COUNT_PER_PATH] = new StructBase.FieldInfo("subdirCountPerPath", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsArray[FIELD_INDEX_CURRENT_TRUNK_FILE_ID] = new StructBase.FieldInfo("currentTrunkFileId", offset, ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE);
            offset += ProtoCommon.FDFS_PROTO_PKG_LEN_SIZE;

            fieldsTotalSize = offset;
        }

        protected string groupName;  //name of this group
        protected long totalMB;      //total disk storage in MB
        protected long freeMB;       //free disk space in MB
        protected long trunkFreeMB;  //trunk free space in MB
        protected int storageCount;  //storage server count
        protected int storagePort;   //storage server port
        protected int storageHttpPort; //storage server HTTP port
        protected int activeCount;     //active storage server count
        protected int currentWriteServer; //current storage server index to upload file
        protected int storePathCount;     //store base path count of each storage server
        protected int subdirCountPerPath; //sub dir count per store path
        protected int currentTrunkFileId; //current trunk file id

        public string GroupName { get { return this.groupName; } }
        public long TotalMB { get { return this.totalMB; } }
        public long FreeMB { get { return this.freeMB; } }
        public long TrunkFreeMB { get { return this.trunkFreeMB; } }
        public int StorageCount { get { return this.storageCount; } }
        public int StoragePort { get { return this.storagePort; } }
        public int StorageHttpPort { get { return this.storageHttpPort; } }
        public int ActiveCount { get { return this.activeCount; } }
        public int CurrentWriteServer { get { return this.currentWriteServer; } }
        public int StorePathCount { get { return this.storePathCount; } }
        public int SubdirCountPerPath { get { return this.subdirCountPerPath; } }
        public int CurrentTrunkFileId { get { return this.currentTrunkFileId; } }

        public override void setFields(byte[] bs, int offset)
        {
            this.groupName = stringValue(bs, offset, fieldsArray[FIELD_INDEX_GROUP_NAME]);
            this.totalMB = longValue(bs, offset, fieldsArray[FIELD_INDEX_TOTAL_MB]);
            this.freeMB = longValue(bs, offset, fieldsArray[FIELD_INDEX_FREE_MB]);
            this.trunkFreeMB = longValue(bs, offset, fieldsArray[FIELD_INDEX_TRUNK_FREE_MB]);
            this.storageCount = intValue(bs, offset, fieldsArray[FIELD_INDEX_STORAGE_COUNT]);
            this.storagePort = intValue(bs, offset, fieldsArray[FIELD_INDEX_STORAGE_PORT]);
            this.storageHttpPort = intValue(bs, offset, fieldsArray[FIELD_INDEX_STORAGE_HTTP_PORT]);
            this.activeCount = intValue(bs, offset, fieldsArray[FIELD_INDEX_ACTIVE_COUNT]);
            this.currentWriteServer = intValue(bs, offset, fieldsArray[FIELD_INDEX_CURRENT_WRITE_SERVER]);
            this.storePathCount = intValue(bs, offset, fieldsArray[FIELD_INDEX_STORE_PATH_COUNT]);
            this.subdirCountPerPath = intValue(bs, offset, fieldsArray[FIELD_INDEX_SUBDIR_COUNT_PER_PATH]);
            this.currentTrunkFileId = intValue(bs, offset, fieldsArray[FIELD_INDEX_CURRENT_TRUNK_FILE_ID]);
        }

        public static int getFieldsTotalSize()
        {
            return fieldsTotalSize;
        }
    }
}
