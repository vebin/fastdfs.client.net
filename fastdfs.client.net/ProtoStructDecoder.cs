using System;
using System.IO;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class ProtoStructDecoder
    {
        public ProtoStructDecoder()
        {
        }

        public T[] decode<T>(byte[] bs, int fieldsTotalSize) where T : StructBase
        {
            if (bs.Length % fieldsTotalSize != 0)
            {
                throw new IOException("byte array length: " + bs.Length + " is invalid!");
            }

            int count = bs.Length / fieldsTotalSize;
            int offset;
            T[] results = (T[])Array.CreateInstance(typeof(T), count);

            offset = 0;
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = Activator.CreateInstance<T>();
                results[i].setFields(bs, offset);
                offset += fieldsTotalSize;
            }

            return results;
        }
    }
}
