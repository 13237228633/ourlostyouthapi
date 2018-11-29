using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common
{
    /// <summary>
    ///  序列化工具
    /// </summary>
    public static class SerializeKit
    {
        #region XmlDeserialize 反序列化Xml
        /// <summary>
        ///  反序列化Xml
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="xmlStr">Xml字符串</param>
        /// <returns>实体对象</returns>
        public static T XmlDeserialize<T>(string xmlStr)
        {
            try
            {
                byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(xmlStr);
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    return (T)xmlSerializer.Deserialize(memoryStream);
                }
            }
            catch
            {
                return default(T);
            }
        }
        #endregion

        #region XmlSerialize 序列化Xml
        /// <summary>
        ///  序列化Xml
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实体对象</param>
        /// <returns>Xml字符串</returns>
        public static string XmlSerialize<T>(T t)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
                    xmlSerializer.Serialize(memoryStream, t);
                    byte[] bytes = memoryStream.ToArray();
                    return Encoding.UTF8.GetString(bytes);
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region byte[] Serialize<T>(T t) 序列化
        /// <summary>
        ///  序列化
        /// </summary>
        /// <typeparam name="T">被序列类型</typeparam>
        /// <param name="t">被序列实体</param>
        /// <returns>序列化后的byte[]</returns>
        public static byte[] Serialize<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                
                new BinaryFormatter().Serialize(stream, t);
                return stream.ToArray();
            }
        }
        #endregion

        #region T Deserialize<T>(byte[] source) 反序列化
        /// <summary>
        ///  反序列化
        /// </summary>
        /// <typeparam name="T">被反序列化类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>反序列化后的实体</returns>
        public static T Deserialize<T>(byte[] source)
        {
            using (MemoryStream stream = new MemoryStream(source))
            {
                return (T)new BinaryFormatter().Deserialize(stream);
            }
        }
        #endregion

      

    }
}
