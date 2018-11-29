using System;
using System.Xml.Serialization;

namespace XmlDateLibrary.Common
{ 
    /// <summary>
    /// 需要提供排序方法时继承
    /// </summary>
    [Serializable] 
    public class XmlChildListBase
    {
        public XmlChildListBase()
        {
            Sort = 0;
        }
        /// <summary>
        /// list排序字段
        /// </summary>
        [XmlAttribute(AttributeName = "sort")] 
        public int Sort { get; set; } 
    }
}
