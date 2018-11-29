using System;
using System.IO;
using System.Text;
using System.Web;


namespace Common
{
    /// <summary>
    ///  文件操作工具类
    /// </summary>
    public static class FileKit
    {
        #region string GetExtension(string fileName) 获取文件后缀名
        /// <summary>
        ///  获取文件后缀名(如.exe)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }
        #endregion

        #region string CreateFileName(string extension) 创建随即不重复文件名
        /// <summary>
        ///  创建随即不重复文件名(yyyyMMddhhmmss+8位Guid)
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string CreateFileName(string extension)
        {
            return string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddhhmmss"),
                                 Guid.NewGuid().ToString().Substring(0, 8), extension);
        }
        #endregion

        #region bool SaveFile(string filePath, Stream stream) 文件保存
        /// <summary>
        ///  文件保存
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool SaveFile(string filePath, Stream stream)
        {
            bool result = true;
            try
            {
                //if (File.Exists(filePath))
                //{
                //    return false;
                //}
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    byte[] bytes = StreamKit.ToBytes(stream);
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region bool SaveFile(string filePath, byte[] bytes) 文件保存
        /// <summary>
        ///  文件保存
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool SaveFile(string filePath, byte[] bytes)
        {
            bool result = true;
            try
            {
                //if (File.Exists(filePath))
                //{
                //    return false;
                //}
                
                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate,FileAccess.Write))
                {
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Close();
                }
                System.GC.Collect();
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region DeleteFile

        public static bool DeleteFile(string filePath)
        {
            bool result = true;
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        #endregion

        #region FileExist 判断文件是否存在
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExist(string fileName)
        {
            return File.Exists(fileName);
        }
        #endregion

        #region ResponseFile 以指定的ContentType输出指定文件文件
        /// <summary>
        /// 以指定的ContentType输出指定文件文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">输出的文件名</param>
        /// <param name="fileType">将文件输出时设置的ContentType</param>
        public static void ResponseFile(string filePath, string fileName, string fileType)
        {
            Stream stream = null;
            // 缓冲区为10k
            byte[] buffer = new Byte[10000];
            try
            {
                // 打开文件
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                // 需要读的数据长度
                long dataToRead = stream.Length;
                HttpContext.Current.Response.ContentType = fileType;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HtmlKit.UrlEncode(fileName.Trim()).Replace("+", " "));
                while (dataToRead > 0)
                {
                    // 检查客户端是否还处于连接状态
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        // 文件长度
                        int length = stream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        // 如果不再连接则跳出死循环
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            HttpContext.Current.Response.End();
        }
        #endregion

        #region IsBrowserImage 判断文件是否为浏览器可以直接显示的图片文件
        /// <summary>
        ///  判断文件是否为浏览器可以直接显示的图片文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsBrowserImage(string fileName)
        {
            fileName = fileName.Trim();
            if (fileName.EndsWith(".") || fileName.IndexOf(".") == -1)
            {
                return false;
            }
            string extension = GetExtension(fileName);
            return (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "bmp" || extension == "gif");
        }
        #endregion

        #region GetUrlExtension 获取Url结尾的文件名
        /// <summary>
        ///  获取Url结尾的文件名
        /// </summary>		
        public static string GetUrlExtension(string url)
        {
            if (url == null)
            {
                return string.Empty;
            }
            string[] array = url.Split(new[] { '/' });
            return array[array.Length - 1].Split(new[] { '?' })[0];
        }
        #endregion

        #region FindNoUTF8File 返回指定目录下的非UTF8字符集文件
        /// <summary>
        /// 返回指定目录下的非UTF8字符集文件
        /// </summary>
        /// <param name="foldPath">路径</param>
        /// <returns>文件名的字符串数组</returns>
        public static string[] FindNoUTF8File(string foldPath)
        {
            StringBuilder sbFile = new StringBuilder();
            DirectoryInfo folder = new DirectoryInfo(foldPath);
            FileInfo[] fileInfos = folder.GetFiles();
            foreach (FileInfo t in fileInfos)
            {
                if (t.Extension.ToLower().Equals(".htm"))
                {
                    FileStream fileStream = new FileStream(t.FullName, FileMode.Open, FileAccess.Read);
                    bool isUTF8 = StreamKit.IsUTF8(fileStream);
                    fileStream.Close();
                    if (!isUTF8)
                    {
                        sbFile.Append(t.FullName);
                        sbFile.Append("\r\n");
                    }
                }
            }
            return StringKit.Split(sbFile.ToString(), "\r\n");
        }
        #endregion

        #region BackupFile 备份文件
        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            if (!overwrite && File.Exists(destFileName))
            {
                return false;
            }
            File.Copy(sourceFileName, destFileName, true);
            return true;
        }
        #endregion

        #region BackupFile 备份文件，当目标文件存在时覆盖
        /// <summary>
        /// 备份文件，当目标文件存在时覆盖
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }
        #endregion

        #region RestoreFile 恢复文件
        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称，如果为null，则不再备份恢复文件</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            if (!File.Exists(backupFileName))
            {
                throw new FileNotFoundException(backupFileName + "文件不存在！");
            }

            if (backupTargetFileName != null)
            {
                if (!File.Exists(targetFileName))
                {
                    throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                }
                File.Copy(targetFileName, backupTargetFileName, true);
            }
            File.Delete(targetFileName);
            File.Copy(backupFileName, targetFileName);
            return true;
        }
        #endregion

        #region RestoreFile 恢复文件
        /// <summary>
        ///  恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }
        #endregion

    }
}
