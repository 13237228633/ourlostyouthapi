namespace Common
{
    using System.Xml;

    /// <summary>
    /// XmlNode访问器接口
    /// </summary>
    interface IXmlNodeVisitor
    {
        /// <summary>
        /// 设置当前访问器结点
        /// </summary>
        /// <param name="xmlNode">当前要访问的节点信息</param>
        void SetNode(XmlNode xmlNode);
        /// <summary>
        /// 返回当前访问器所用结点信息
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        XmlNode GetNode(string nodeName);
        /// <summary>
        /// 索引器属性
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        string this[string nodeName] { get; set; }
    }


    /// <summary>
    /// 属性节点Value访问器类
    /// </summary>
    public class XmlNodeAttributeValueVisitor : IXmlNodeVisitor
    {
        private XmlNode _xmlNode;
        
        /// <summary>
        /// 定义索引器
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string this[string nodeName]
        {
            get
            {
                return this._xmlNode.Attributes[nodeName].Value;
            }
            set
            {
                this._xmlNode.Attributes[nodeName].Value = value;
            }
        }

        /// <summary>
        /// 设置当前访问器结点
        /// </summary>
        /// <param name="xmlNode">当前要访问的节点信息</param>
        public void SetNode(XmlNode xmlNode)
        {
            this._xmlNode = xmlNode;
        }

        /// <summary>
        /// 返回当前访问器所用结点信息
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public XmlNode GetNode(string nodeName)
        {
            return this._xmlNode.SelectSingleNode(nodeName);
        }
    }
    
    /// <summary>
    /// Select节点InnerText访问器类
    /// </summary>
    public class XmlNodeInnerTextVisitor : IXmlNodeVisitor
    {
        private XmlNode _xmlNode;

        /// <summary>
        /// 定义索引器
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string this[string nodeName]
        {
            get
            {
                return this._xmlNode.SelectSingleNode(nodeName).InnerText;
            }
            set
            {
                this._xmlNode.SelectSingleNode(nodeName).InnerText = value;
            }
        }

        /// <summary>
        /// 设置当前访问器结点
        /// </summary>
        /// <param name="xmlNode">当前要访问的节点信息</param>
        public void SetNode(XmlNode xmlNode)
        {
            this._xmlNode = xmlNode;
        }

        /// <summary>
        /// 返回当前访问器所用结点信息
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public XmlNode GetNode(string nodeName)
        {
            return this._xmlNode.SelectSingleNode(nodeName);
        }
    }
}
