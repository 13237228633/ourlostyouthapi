using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace Common
{
    /// <summary>
    /// XMLComponent 类
    /// </summary>
    public abstract class XmlComponent
    {
        /// <summary>
        /// 源数据表
        /// </summary>
        public DataTable SourceDataTable { get; set; }

        private string _fileOutputPath = @"";
        /// <summary>
        /// 文件输出路径
        /// </summary>
        public string FileOutPath
        {
            set
            {   //保证路径字符串变量的合法性
                if (value.LastIndexOf("\\") != (value.Length - 1))
                { this._fileOutputPath = value + "\\"; }
            }
            get { return _fileOutputPath; }
        }


        private string _fileEncode = "utf-8";
        /// <summary>
        /// 文件编码
        /// </summary>
        public string FileEncode
        {
            set { _fileEncode = value; }
            get { return _fileEncode; }
        }


        private int _indentation = 6;
        /// <summary>
        /// 文件缩进
        /// </summary>
        public int Indentation
        {
            set { _indentation = value; }
            get { return _indentation; }
        }


        private string _version = "2.0";
        /// <summary>
        /// 版本信息
        /// </summary>
        public string Version
        {
            set { this._version = value; }
            get { return _version; }
        }

        private string _startElement = "channel";
        /// <summary>
        /// 开始元素
        /// </summary>
        public string StartElement
        {
            set { this._startElement = value; }
            get { return _startElement; }
        }

        /// <summary>
        /// XSL链接
        /// </summary>
        public string XslLink { get; set; }

        private string _fileName = "MyFile.xml";
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            set { this._fileName = value; }
            get { return _fileName; }
        }

        private string _parentField = "Item";
        /// <summary>
        /// 表中指向父记录的字段名称
        /// </summary>
        public string ParentField
        {
            set { this._parentField = value; }
            get { return this._parentField; }
        }

        private string _key = "ItemID";
        /// <summary>
        /// 表中一个主键的值
        /// </summary>
        public string Key
        {
            set { this._key = value; }
            get { return this._key; }
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <returns></returns>
        public abstract string WriteFile();

        /// <summary>
        /// 写入StringBuilder对象
        /// </summary>
        /// <returns></returns>
        public abstract StringBuilder WriteStringBuilder();

        /// <summary>
        /// 文档对象
        /// </summary>
        public XmlDocument XmlDocMetone = new XmlDocument();

        #region 构XML树
        /// <summary>
        /// 构XML树
        /// </summary>
        /// <param name="tempXmlElement"></param>
        /// <param name="location"></param>
        protected void BulidXmlTree(XmlElement tempXmlElement, string location)
        {

            DataRow tempRow = this.SourceDataTable.Select(this.Key + "=" + location)[0];
            //生成Tree节点
            XmlElement treeElement = XmlDocMetone.CreateElement(this.ParentField);
            tempXmlElement.AppendChild(treeElement);


            foreach (DataColumn c in this.SourceDataTable.Columns)  //依次找出当前记录的所有列属性
            {
                if ((c.Caption.ToLower() != this.ParentField.ToLower()))
                    this.AppendChildElement(c.Caption.Trim().ToLower(), tempRow[c.Caption.Trim()].ToString().Trim(), treeElement);
            }

            foreach (DataRow dr in this.SourceDataTable.Select(this.ParentField + "=" + location))
            {
                if (this.SourceDataTable.Select("item=" + dr[this.Key]).Length >= 0)
                {
                    this.BulidXmlTree(treeElement, dr[this.Key].ToString().Trim());
                }
                else continue;
            }
        }
        #endregion

        #region 追加子节点
        /// <summary>
        /// 追加子节点
        /// </summary>
        /// <param name="strName">节点名字</param>
        /// <param name="strInnerText">节点内容</param>
        /// <param name="parentElement">父节点</param>
        /// <param name="xmlDocument">XmlDocument对象</param>
        protected void AppendChildElement(string strName, string strInnerText, XmlElement parentElement, XmlDocument xmlDocument)
        {
            XmlElement xmlElement = xmlDocument.CreateElement(strName);
            xmlElement.InnerText = strInnerText;
            parentElement.AppendChild(xmlElement);
        }

        /// <summary>
        /// 使用默认的Xml文档
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strInnerText"></param>
        /// <param name="parentElement"></param>
        protected void AppendChildElement(string strName, string strInnerText, XmlElement parentElement)
        {
            AppendChildElement(strName, strInnerText, parentElement, XmlDocMetone);
        }
        #endregion


        #region 创建存储生成XML的文件夹
        /// <summary>
        /// 创建存储生成XML的文件夹
        /// </summary>
        public void CreatePath()
        {
            if (this.FileOutPath != null)
            {
                string path = this.FileOutPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            else
            {
                const string path = @"C:\";
                string nowString = DateTime.Now.ToString("yyyy-M").Trim();
                if (!Directory.Exists(path + nowString))
                {
                    Directory.CreateDirectory(path + "\\" + nowString);
                }
            }
        }

        #endregion
    }


    /// <summary>
    /// 无递归直接生成XML
    /// </summary>
    class ConcreteComponent : XmlComponent
    {
        /// <summary>
        /// 写入StringBuilder对象
        /// </summary>
        /// <returns></returns>
        public override StringBuilder WriteStringBuilder()
        {
            string xmlData = string.Format("<?xml version='1.0' encoding='{0}'?><{1} ></{1}>", this.FileEncode, this.SourceDataTable.TableName);

            this.XmlDocMetone.Load(new StringReader(xmlData));
            //写入channel
            foreach (DataRow r in this.SourceDataTable.Rows)   //依次取出所有行
            {
                //普通方式生成XML
                XmlElement treeContentElement = this.XmlDocMetone.CreateElement(this.StartElement);
                if (XmlDocMetone.DocumentElement != null)
                {
                    XmlDocMetone.DocumentElement.AppendChild(treeContentElement);
                }

                foreach (DataColumn c in this.SourceDataTable.Columns)  //依次找出当前记录的所有列属性
                {
                    this.AppendChildElement(c.Caption.ToLower(), r[c].ToString().Trim(), treeContentElement);
                }
            }
            return new StringBuilder().Append(XmlDocMetone.InnerXml);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <returns></returns>
        public override string WriteFile()
        {
            if (this.SourceDataTable != null)
            {
                string filename = this.FileOutPath + this.FileName;
                Encoding encode = Encoding.GetEncoding(this.FileEncode);
                CreatePath();
                XmlTextWriter picXmlWriter = new XmlTextWriter(filename, encode);

                try
                {

                    picXmlWriter.Formatting = Formatting.Indented;
                    picXmlWriter.Indentation = this.Indentation;
                    picXmlWriter.Namespaces = false;
                    picXmlWriter.WriteStartDocument();
                    picXmlWriter.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + this.XslLink + "'");

                    picXmlWriter.WriteStartElement(this.SourceDataTable.TableName);
                    picXmlWriter.WriteAttributeString("", "version", null, this.Version);

                    //写入channel
                    foreach (DataRow r in this.SourceDataTable.Rows)   //依次取出所有行
                    {
                        picXmlWriter.WriteStartElement("", this.StartElement, "");
                        foreach (DataColumn c in this.SourceDataTable.Columns)  //依次找出当前记录的所有列属性
                        {
                            picXmlWriter.WriteStartElement("", c.Caption.Trim().ToLower(), "");
                            picXmlWriter.WriteString(r[c].ToString().Trim());
                            picXmlWriter.WriteEndElement();
                        }
                        picXmlWriter.WriteEndElement();
                    }

                    picXmlWriter.WriteEndElement();
                    picXmlWriter.Flush();
                    this.SourceDataTable.Dispose();
                }
                catch (Exception e) { Console.WriteLine("异常：{0}", e); }
                finally
                {
                    Console.WriteLine("对文件 {0} 的处理已完成。");
                    picXmlWriter.Close();
                }
                return filename;
            }
            Console.WriteLine("对文件 {0} 的处理未完成。");
            return "";
        }
    }


    /// <summary>
    /// 无递归直接生成XML
    /// </summary>
    public class TreeNodeComponent : XmlComponent
    {
        /// <summary>
        /// 写入StringBuilder对象
        /// </summary>
        /// <returns></returns>
        public override StringBuilder WriteStringBuilder()
        {
            string xmlData = string.Format("<?xml version='1.0' encoding='{0}'?><{1} ></{1}>", this.FileEncode, this.SourceDataTable.TableName);

            this.XmlDocMetone.Load(new StringReader(xmlData));
            //写入channel
            foreach (DataRow r in this.SourceDataTable.Rows)   //依次取出所有行
            {
                //普通方式生成XML
                XmlElement treeContentElement = this.XmlDocMetone.CreateElement(this.StartElement);
                if (XmlDocMetone.DocumentElement != null)
                {
                    XmlDocMetone.DocumentElement.AppendChild(treeContentElement);
                }
                foreach (DataColumn c in this.SourceDataTable.Columns)  //依次找出当前记录的所有列属性
                {
                    this.AppendChildElement(c.Caption.ToLower(), r[c].ToString().Trim(), treeContentElement);
                }
            }
            return new StringBuilder().Append(XmlDocMetone.InnerXml);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <returns></returns>
        public override string WriteFile()
        {
            if (this.SourceDataTable != null)
            {
                string filename = this.FileOutPath + this.FileName;
                Encoding encode = Encoding.GetEncoding(this.FileEncode);
                CreatePath();
                XmlTextWriter picXmlWriter = new XmlTextWriter(filename, encode);
                try
                {
                    picXmlWriter.Formatting = Formatting.Indented;
                    picXmlWriter.Indentation = this.Indentation;
                    picXmlWriter.Namespaces = false;
                    picXmlWriter.WriteStartDocument();
                    picXmlWriter.WriteStartElement(this.SourceDataTable.TableName);
                    //写入channel
                    foreach (DataRow r in this.SourceDataTable.Rows)   //依次取出所有行
                    {
                        string content = "  Text=\"" + r[0].ToString().Trim() + "\"   ImageUrl=\"../../editor/images/smilies/" + r[1].ToString().Trim() + "\"";
                        picXmlWriter.WriteStartElement("", this.StartElement + content, "");
                        picXmlWriter.WriteEndElement();
                    }

                    picXmlWriter.WriteEndElement();
                    picXmlWriter.Flush();
                    this.SourceDataTable.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine("异常：{0}", e);
                }
                finally
                {
                    Console.WriteLine("对文件 {0} 的处理已完成。");
                    picXmlWriter.Close();
                }
                return filename;
            }
            Console.WriteLine("对文件 {0} 的处理未完成。");
            return "";
        }
    }


    /// <summary>
    /// RSS生成
    /// </summary>
    public class RssXmlComponent : XmlComponent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RssXmlComponent()
        {
            FileEncode = "gb2312";
            Version = "2.0";
            StartElement = "channel";
        }

        /// <summary>
        /// 写入StringBuilder对象
        /// </summary>
        /// <returns></returns>
        public override StringBuilder WriteStringBuilder()
        {
            string xmlData = string.Format("<?xml version='1.0' encoding='{0}'?><?xml-stylesheet type=\"text/xsl\" href=\"{1}\"?><rss version='{2}'></rss>", this.FileEncode, this.XslLink, this.Version);
            this.XmlDocMetone.Load(new StringReader(xmlData));
            string key = "-1";
            //写入channel
            foreach (DataRow r in this.SourceDataTable.Rows)   //依次取出所有行
            {
                if ((this.Key != null) && (this.ParentField != null)) //递归进行XML生成
                {
                    if ((r[this.ParentField].ToString().Trim() == "") || (r[this.ParentField].ToString().Trim() == "0"))
                    {
                        XmlElement treeContentElement = this.XmlDocMetone.CreateElement(this.StartElement);
                        if (XmlDocMetone.DocumentElement != null)
                        {
                            XmlDocMetone.DocumentElement.AppendChild(treeContentElement);
                        }
                        foreach (DataColumn c in this.SourceDataTable.Columns)  //依次找出当前记录的所有列属性
                        {
                            if ((c.Caption.ToLower() == this.ParentField.ToLower()))
                            {
                                key = r[this.Key].ToString().Trim();
                            }
                            else
                            {
                                if ((r[this.ParentField].ToString().Trim() == "") || (r[this.ParentField].ToString().Trim() == "0"))
                                {
                                    this.AppendChildElement(c.Caption.ToLower(), r[c].ToString().Trim(), treeContentElement);                       
                                }
                            }
                        }
                        foreach (DataRow dr in this.SourceDataTable.Select(this.ParentField + "=" + key))
                        {
                            if (this.SourceDataTable.Select(this.ParentField + "=" + dr[this.Key]).Length >= 0)
                            {
                                this.BulidXmlTree(treeContentElement, dr["ItemID"].ToString().Trim());
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                else  //普通方式生成XML
                {
                    XmlElement treeContentElement = this.XmlDocMetone.CreateElement(this.StartElement);
                    if (XmlDocMetone.DocumentElement != null)
                    {
                        XmlDocMetone.DocumentElement.AppendChild(treeContentElement);
                    }
                    foreach (DataColumn c in this.SourceDataTable.Columns)  //依次找出当前记录的所有列属性
                    {
                        this.AppendChildElement(c.Caption.ToLower(), r[c].ToString().Trim(), treeContentElement);
                    }
                }
            }
            return new StringBuilder().Append(XmlDocMetone.InnerXml);
        }


        /// <summary>
        /// 写入文件
        /// </summary>
        /// <returns></returns>
        public override string WriteFile()
        {
            CreatePath();
            string xmlData = string.Format("<?xml version='1.0' encoding='{0}'?><?xml-stylesheet type=\"text/xsl\" href=\"{1}\"?><rss version='{2}'></rss>", this.FileEncode, this.XslLink, this.Version);
            this.XmlDocMetone.Load(new StringReader(xmlData));
            string key = "-1";
            //写入channel
            foreach (DataRow r in this.SourceDataTable.Rows)   //依次取出所有行
            {
                if ((this.Key != null) && (this.ParentField != null)) //递归进行XML生成
                {
                    if ((r[this.ParentField].ToString().Trim() == "") || (r[this.ParentField].ToString().Trim() == "0"))
                    {
                        XmlElement treeContentElement = this.XmlDocMetone.CreateElement(this.StartElement);
                        if (XmlDocMetone.DocumentElement != null)
                        {
                            XmlDocMetone.DocumentElement.AppendChild(treeContentElement);
                        }

                        foreach (DataColumn c in this.SourceDataTable.Columns)  //依次找出当前记录的所有列属性
                        {
                            if ((c.Caption.ToLower() == this.ParentField.ToLower()))
                                key = r[this.Key].ToString().Trim();
                            else if ((r[this.ParentField].ToString().Trim() == "") || (r[this.ParentField].ToString().Trim() == "0"))
                                this.AppendChildElement(c.Caption.ToLower(), r[c].ToString().Trim(), treeContentElement);
                        }

                        foreach (DataRow dr in this.SourceDataTable.Select(this.ParentField + "=" + key))
                        {
                            if (this.SourceDataTable.Select(this.ParentField + "=" + dr[this.Key]).Length >= 0)
                                this.BulidXmlTree(treeContentElement, dr["ItemID"].ToString().Trim());
                            else
                                continue;
                        }
                    }
                }
                else  //普通方式生成XML
                {
                    XmlElement treeContentElement = this.XmlDocMetone.CreateElement(this.StartElement);
                    if (XmlDocMetone.DocumentElement != null)
                    {
                        XmlDocMetone.DocumentElement.AppendChild(treeContentElement);
                    }
                    foreach (DataColumn c in this.SourceDataTable.Columns)  //依次找出当前记录的所有列属性
                    {
                        this.AppendChildElement(c.Caption.ToLower(), r[c].ToString().Trim(), treeContentElement);
                    }
                }

            }
            XmlDocMetone.Save(this.FileOutPath + this.FileName);

            return this.FileOutPath + this.FileName;
        }
    }


    //装饰器类
    public class XmlDecorator : XmlComponent
    {
        protected XmlComponent ActualXmlComponent;

        public void SetXmlComponent(XmlComponent xc)
        {
            ActualXmlComponent = xc;
            GetSettingFromComponent(xc);
        }

        //将被装入的对象的默认设置为当前装饰者的初始值
        public void GetSettingFromComponent(XmlComponent xc)
        {
            this.FileEncode = xc.FileEncode;
            this.FileOutPath = xc.FileOutPath;
            this.Indentation = xc.Indentation;
            this.SourceDataTable = xc.SourceDataTable;
            this.StartElement = xc.StartElement;
            this.Version = xc.Version;
            this.XslLink = xc.XslLink;
            this.Key = xc.Key;
            this.ParentField = xc.ParentField;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <returns></returns>
        public override string WriteFile()
        {
            if (ActualXmlComponent != null)
                ActualXmlComponent.WriteFile();

            return null;
        }

        /// <summary>
        /// 写入StringBuilder对象
        /// </summary>
        /// <returns></returns>
        public override StringBuilder WriteStringBuilder()
        {
            if (ActualXmlComponent != null)
                return ActualXmlComponent.WriteStringBuilder();

            return null;
        }
    }
}
