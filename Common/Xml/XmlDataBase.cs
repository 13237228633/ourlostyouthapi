using System;
using System.Xml.Serialization;

namespace XmlDateLibrary.Common
{
    [Serializable] 
    public class XmlDataBase
    {
        public XmlDataBase()
        {
            Record = 0;
        }
        /// <summary>
        /// 记录XML文档的记录数
        /// </summary>
        [XmlAttribute(AttributeName = "record")] 
        public int Record { get; set; } 
    } 
        
       

}
