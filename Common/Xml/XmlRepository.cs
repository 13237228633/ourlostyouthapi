using System;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;

//学习资料： http://blog.csdn.net/shizhiyingnj/article/details/1507943
//.NET对象的XML序列化和反序列化 
//required to use EncryptedXml object, requires the System.Security assembly to be referenced

namespace XmlDateLibrary.Common
{
     public abstract class XmlRepository<TEntity>
     where TEntity : XmlDataBase, new()
    {
        private static TEntity _tEntity; 
         /// <summary>
        /// .NET对象的XML正反序列化 
         /// </summary>
         /// <param name="tEntity"></param>
        protected XmlRepository(TEntity tEntity)
        {
            _tEntity = tEntity;
        }
        protected XmlRepository()
        { 
        }
        #region 反序列化读取
        /// <summary>
        /// 获得XML文档数据
        /// </summary>
        /// <param name="path">读取的文档的绝对路径</param>
        /// <returns></returns>
        public virtual TEntity GetXml(string path)
        {
            if (_tEntity != null)
            {
                return _tEntity;
            }
            return GetFromXml(path);
        }
        /// <summary>
        /// 获得XML文档数据,强制重新获取。
        /// </summary>
        /// <param name="path">读取的文档的绝对路径</param>
        /// <returns></returns>
        public virtual TEntity GetStrongXml(string path)
        {
            _tEntity = null;
            return GetFromXml(path);
        }
        private static TEntity GetFromXml(string path)
        {
            FileStream fs = null;
            try
            {
                var xs = new XmlSerializer(typeof(TEntity));
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                _tEntity = (TEntity)xs.Deserialize(fs);
            }
            catch
            {
                throw new Exception("Xml 反序列化失败!" + Environment.NewLine + path);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return _tEntity;
        }

        #endregion
        /// <summary>
        /// 重设XML文档数据
        /// </summary>
        /// <param name="path">写入的文档的绝对路径</param>
        /// <param name="entityResource">XML文档结构对象</param>
        /// <returns></returns>
        public virtual bool SetXml(string path, TEntity entityResource)
        {  
            bool backbool = true;
            FileStream fs = null;
            try
            {
                var xs = new XmlSerializer(typeof(TEntity));
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                xs.Serialize(fs, entityResource);
                _tEntity = null; 
            }
            catch
            {
                backbool = false;
                throw new Exception("Xml 序列化失败!");
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return backbool;
        }

        /// <summary>
        /// 自定义资源释放
        /// </summary>
        public virtual void Dispose()
        {
            _tEntity = null;
        } 
    }
}
