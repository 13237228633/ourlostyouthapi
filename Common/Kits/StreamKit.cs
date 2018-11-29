using System;
using System.IO;

namespace Common
{
    /// <summary>
    ///  流处理工具
    /// </summary>
    public static class StreamKit
    {
        #region ToBytes 流转byte数组
        /// <summary>
        ///  流转byte数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToBytes(Stream stream)
        {
            using (stream)
            {
                byte[] data = new byte[stream.Length];
                int offset = 0;
                int remaining = data.Length;
                while (remaining > 0)
                {
                    int read = stream.Read(data, offset, remaining);
                    if (read <= 0)
                    {
                        throw new EndOfStreamException(String.Format("End of stream reached with {0} bytes left to read", remaining));
                    }
                    remaining -= read;
                    offset += read;
                }
                return data;
            }
        }
        #endregion

        #region IsUTF8 判断文件流是否为UTF8字符集
        /// <summary>
        /// 判断文件流是否为UTF8字符集
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <returns>判断结果</returns>
        public static bool IsUTF8(FileStream fileStream)
        {
            bool isASC2 = true;
            long streamLength = fileStream.Length;
            byte num = 0;
            for (int count = 0; count < streamLength; count++)
            {
                byte @byte = (byte)fileStream.ReadByte();
                if ((@byte & 0x80) != 0)
                {
                    isASC2 = false;
                }
                if (num == 0)
                {
                    if (@byte >= 0x80)
                    {
                        do
                        {
                            @byte <<= 1;
                            num++;
                        }
                        while ((@byte & 0x80) != 0);
                        {
                            num--;
                            if (num == 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if ((@byte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    num--;
                }
            }
            if (num > 0)
            {
                return false;
            }
            if (isASC2)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
