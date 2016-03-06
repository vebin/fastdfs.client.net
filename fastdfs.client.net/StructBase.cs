using System;
using System.Text;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public abstract class StructBase
    {
        protected class FieldInfo
        {
            protected string name;
            protected int offset;
            protected int size;

            public FieldInfo(string name, int offset, int size)
            {
                this.name = name;
                this.offset = offset;
                this.size = size;
            }

            public string Name { get { return this.name; } }
            public int Offset { get { return this.offset; } }
            public int Size { get { return this.size; } }
        }

        public abstract void setFields(byte[] bs, int offset);

        protected string stringValue(byte[] bs, int offset, FieldInfo filedInfo)
        {
            try
            {
                return Encoding.GetEncoding(ClientGlobal.g_charset).GetString(bs, offset + filedInfo.Offset, filedInfo.Size).Replace("\0", "").Trim();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected long longValue(byte[] bs, int offset, FieldInfo filedInfo)
        {
            //return BitConverter.ToInt64(bs, offset + filedInfo.Offset);
            return ProtoCommon.buff2long(bs, offset + filedInfo.Offset);
        }

        protected int intValue(byte[] bs, int offset, FieldInfo filedInfo)
        {
            //return BitConverter.ToInt16(bs, offset + filedInfo.Offset);
            return (int)ProtoCommon.buff2long(bs, offset + filedInfo.Offset);
        }

        protected int int32Value(byte[] bs, int offset, FieldInfo filedInfo)
        {
            //return BitConverter.ToInt32(bs, offset + filedInfo.Offset);
            return ProtoCommon.buff2int(bs, offset + filedInfo.Offset);
        }

        protected byte byteValue(byte[] bs, int offset, FieldInfo filedInfo)
        {
            return bs[offset + filedInfo.Offset];
        }

        protected bool booleanValue(byte[] bs, int offset, FieldInfo filedInfo)
        {
            return bs[offset + filedInfo.Offset] != 0;
        }

        protected DateTime dateValue(byte[] bs, int offset, FieldInfo filedInfo)
        {
            //return new DateTime(BitConverter.ToInt64(bs, offset + filedInfo.Offset) * 1000);
            return ProtoCommon.UnixTimestampToDateTime(ProtoCommon.buff2long(bs, offset + filedInfo.Offset));
        }
    }
}
