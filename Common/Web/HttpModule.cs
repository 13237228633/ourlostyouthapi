using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

//using Ice.Common;
//using Ice.Config;
//using Ice.Config.Provider;

namespace Common.Web
{
    /// <summary>
    /// HttpModule类
    /// </summary>
    public class HttpModule : System.Web.IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.BeginRequest += new EventHandler(this.Application_BeginRequest);
            app.EndRequest += new EventHandler(this.Application_EndRequest);
        }

        public void Dispose() { }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;

            List<string> filters = new List<string>(){ ".jpg", ".png", ".gif", ".css", ".js", ".swf" };
            bool success = false;

            foreach (RewriterRule rule in GetRuleListCache())
            {
                //域名重写
                if (IsHttpUrl(rule.LookFor))
                {
                    string lookFor = "^" + ResolveUrl(app.Request.ApplicationPath, rule.LookFor) + "$";
                    Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);

                    if (re.IsMatch(app.Request.Url.AbsoluteUri))
                    {
                        string sendTo = ResolveUrl(app.Context.Request.ApplicationPath, re.Replace(app.Request.Url.AbsoluteUri, rule.SendTo));
                        RewritePath(app.Context, sendTo);
                        success = true;
                        break;
                    }
                }
                //站内路径重写
                else
                {
                    string lookFor = "^" + ResolveUrl(app.Request.ApplicationPath, "/" + rule.LookFor) + "$";
                    Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);
                    if (re.IsMatch(app.Request.Path))
                    {
                        string sendTo = ResolveUrl(app.Context.Request.ApplicationPath, re.Replace(app.Request.Path, "/" + rule.SendTo));
                        RewritePath(app.Context, sendTo);
                        success = true;
                        break;
                    }
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            if (app.Server.GetLastError() == null) return;
            Exception ex = app.Server.GetLastError().GetBaseException();
            if (ex != null)
            {
                string error = string.Format("捕获到异常：{0}\r\n错误信息：{1}\r\n错误堆栈：{2}\r\n应用程序：{3}\r\n", ex.TargetSite, ex.Message, ex.StackTrace, ex.Source);
                try
                {
                    //app.Context.Response.Redirect(BaseConfigs.GetPath + "admin/error/500");
                }
                catch (Exception) { }
            }
            app.Server.ClearError();
        }

        /// <summary>
        /// 是否为URL地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsHttpUrl(string url)
        {
            return url.IndexOf("http://") != -1;
        }

        /// <summary>
        /// 重写路径,主要处理参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sendToUrl"></param>
        protected void RewritePath(HttpContext context, string sendToUrl)
        {
            if (context.Request.QueryString.Count > 0)
            {
                if (sendToUrl.IndexOf('?') != -1)
                {
                    sendToUrl += "&" + context.Request.QueryString.ToString();
                }
                else
                {
                    sendToUrl += "?" + context.Request.QueryString.ToString();
                }
            }
            string queryString = String.Empty;
            string sendToUrlLessQString = sendToUrl;
            if (sendToUrl.IndexOf('?') > 0)
            {
                sendToUrlLessQString = sendToUrl.Substring(0, sendToUrl.IndexOf('?'));
                queryString = sendToUrl.Substring(sendToUrl.IndexOf('?') + 1);
            }
            //    context.RewritePath(sendToUrlLessQString +"?"+ queryString);
            context.RewritePath(sendToUrlLessQString, String.Empty, queryString);
        }

        /// <summary>
        /// 读取并缓存规则列表
        /// </summary>
        /// <returns></returns>
        public List<RewriterRule> GetRuleList()
        {
            List<RewriterRule> ruleList = new List<RewriterRule>();
            string urlFilePath = HttpContext.Current.Server.MapPath(string.Format("//config/rewrite.config", "/"));
            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();

            xml.Load(urlFilePath);

            XmlNode root = xml.SelectSingleNode("rewrite");
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "item")
                {
                    RewriterRule rule = new RewriterRule();
                    rule.LookFor = n.Attributes["lookfor"].Value;
                    rule.SendTo = n.Attributes["sendto"].Value;
                    ruleList.Add(rule);
                }
            }
            return ruleList;
        }

        public List<RewriterRule> GetRuleListCache()
        {
            string cacheKey = "rewritelist";
            string urlFilePath = HttpContext.Current.Server.MapPath(string.Format("//config/rewrite.config", "/"));

            List<RewriterRule> ruleList = (List<RewriterRule>)HttpContext.Current.Cache.Get(cacheKey);
            if (ruleList == null)
            {
                ruleList = GetRuleList();
                HttpContext.Current.Cache.Insert(cacheKey, ruleList, new System.Web.Caching.CacheDependency(urlFilePath));
            }
            return ruleList;
        }

        public void SaveRuleList(List<RewriterRule> ruleList)
        {
            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            System.Xml.XmlDeclaration xmldecl;
            xmldecl = xml.CreateXmlDeclaration("1.0", "utf-8", null);
            xml.AppendChild(xmldecl);
            System.Xml.XmlElement xmlelem = xml.CreateElement("", "rewrite", "");
            foreach (RewriterRule rule in ruleList)
            {
                System.Xml.XmlElement e = xml.CreateElement("", "item", "");
                e.SetAttribute("lookfor", rule.LookFor);
                e.SetAttribute("sendto", rule.SendTo);
                xmlelem.AppendChild(e);
            }
            xml.AppendChild(xmlelem);
            xml.Save(HttpContext.Current.Server.MapPath(string.Format("//config/rewrite.config", "/")));
        }

        /// <summary>
        /// 处理各种路径
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ResolveUrl(string appPath, string url)
        {
            //   return url;
            if (url.Length == 0 || url[0] != '~')
                return url;		// there is no ~ in the first character position, just return the url
            else
            {
                if (url.Length == 1)
                    return appPath;  // there is just the ~ in the URL, return the appPath
                if (url[1] == '/' || url[1] == '\\')
                {
                    // url looks like ~/ or ~\
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(2);
                    else
                        return "/" + url.Substring(2);
                }
                else
                {
                    // url looks like ~something
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(1);
                    else
                        return appPath + url.Substring(1);
                }
            }
        }
    }

    /// <summary>
    /// 规则实体
    /// </summary>
    public class RewriterRule
    {
        private string _lookfor;
        private string _sendto;
        /// <summary>
        /// 正则地址
        /// </summary>
        public string LookFor
        {
            get { return _lookfor; }
            set { _lookfor = value; }
        }
        /// <summary>
        /// 实际地址
        /// </summary>
        public string SendTo
        {
            get { return _sendto; }
            set { _sendto = value; }
        }
    }
}
