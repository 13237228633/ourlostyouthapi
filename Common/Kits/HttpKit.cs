using System.Collections.Generic;
using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Management;

using System.Configuration;
using System.Reflection;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;
using System.Net.Sockets;



namespace Common
{
    /// <summary>
    ///  Http工具类
    /// </summary>
    public static class HttpKit
    {
        #region IsPost 当前请求是否是Post请求
        /// <summary>
        ///  当前请求是否是Post请求
        /// </summary>
        public static bool IsPost
        {
            get { return HttpContext.Current.Request.HttpMethod.Equals("POST"); }
        }
        #endregion
        
        #region IsGet 当然请求是否是Get请求
        /// <summary>
        ///  当然请求是否是Get请求
        /// </summary>
        public static bool IsGet
        {
            get { return HttpContext.Current.Request.HttpMethod.Equals("GET"); }
        }
        #endregion

        #region IsAjax 当前请求是否是Ajax请求
        /// <summary>
        ///  当前请求是否是Ajax请求
        /// </summary>
        public static bool IsAjax
        {
            get
            {
                string xmlRequestString = HttpContext.Current.Request.Headers["X-Requested-With"];
                if (string.IsNullOrEmpty(xmlRequestString))
                {
                    return false;
                }
                return xmlRequestString.Equals("XMLHttpRequest");
            }
        }
        #endregion

        #region IsBrowserRequest 当前请求是否来自浏览器
        /// <summary>
        /// 当前请求是否来自浏览器
        /// </summary>
        public static bool IsBrowserRequest
        {
            get
            {
                string[] browserNames = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
                string currentBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
                foreach (string browserName in browserNames)
                {
                    if (currentBrowser.IndexOf(browserName) >= 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion

        #region IsSearchEngineRequest 当前请求是否来自搜索引擎
        /// <summary>
        /// 当前请求是否来自搜索引擎
        /// </summary>
        public static bool IsSearchEngineRequest
        {
            get
            {
                if (HttpContext.Current.Request.UrlReferrer == null)
                {
                    return false;
                }
                string[] searchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
                string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
                for (int i = 0; i < searchEngine.Length; i++)
                {
                    if (tmpReferrer.IndexOf(searchEngine[i]) >= 0)
                        return true;
                }
                return false;
            }
        }
        #endregion

        #region RawUrl 当前请求的原始Url
        /// <summary>
        /// 当前请求的原始Url(Url中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        public static string RawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
        }
        #endregion

        #region UrlReferrer 上一个请求地址
        /// <summary>
        /// 上一个请求地址
        /// </summary>
        public static string UrlReferrer
        {
            get
            {
                string urlReferrer;
                try
                {
                    urlReferrer = HttpContext.Current.Request.UrlReferrer.ToString();
                }
                catch
                {
                    return string.Empty;
                }
                return urlReferrer;
            }
        }
        #endregion

        #region CurrentCache

        public static Cache CurrentCache
        {
            get
            {
                return HttpContext.Current.Cache;
            }
        }
        #endregion

        #region CurrentFullUrl 当前请求完整的Url
        /// <summary>
        /// 当前请求完整的Url
        /// </summary>
        public static string CurrentFullUrl
        {
            get { return HttpContext.Current.Request.Url.ToString(); }
        }
        #endregion

        #region CurrentHost 当前请求主机头
        /// <summary>
        /// 当前请求主机头
        /// </summary>
        public static string CurrentHost
        {
            get { return HttpContext.Current.Request.Url.Host; }
        }
        #endregion

        #region CurrentAuthority
        /// <summary>
        /// 
        /// </summary>
        public static string CurrentAuthority
        {
            get { return HttpContext.Current.Request.Url.Authority; }
        }
        #endregion

        #region CurrentFullHost 当前请求完整主机头
        /// <summary>
        /// 当前请求完整主机头
        /// </summary>
        public static string CurrentFullHost
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                if (!request.Url.IsDefaultPort)
                {
                    return string.Format("{0}:{1}", request.Url.Host, request.Url.Port);
                }
                return request.Url.Host;
            }
        }
        #endregion

        #region CurrentPageName 当前请求页面的名称
        /// <summary>
        /// 当前请求页面的名称
        /// </summary>
        public static string CurrentPageName
        {
            get
            {
                string[] urlArray = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
                return urlArray[urlArray.Length - 1].ToLower();
            }
        }
        #endregion

        #region CurrentRequestIP 当前请求的IP地址
        /// <summary>
        /// 当前请求的IP地址
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string CurrentRequestIP
        {
            get
            {
                string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(result) || !ValidateKit.IsIP(result))
                {
                    return "127.0.0.1";
                }
                return result;
            }
        }
        #endregion

        #region ExtralIP 当前请求的IP地址
        /// <summary>
        /// 获得外网IP
        /// </summary>
        /// <returns>获得外网IP</returns>
        public static string CurrentExtralIP
        {
            get
            {
                string strUrl = "http://iframe.ip138.com/ic.asp";     //获得IP的网址
                Uri uri = new Uri(strUrl);
                WebRequest webreq = WebRequest.Create(uri);
                Stream s = webreq.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd();         //读取网站返回的数据  格式：您的IP地址是：[x.x.x.x] 
                int i = all.IndexOf("[") + 1;
                string tempip = all.Substring(i, 15);
                string ip = tempip.Replace("]", "").Replace(" ", "").Replace("<", "");     //去除杂项找出ip
                return ip;
 
            
            }
        }
        #endregion

        #region WriteToEnd 向Http响应流中写入文本并立即终止响应
        /// <summary>
        ///  WriteToEnd 向Http响应流中写入文本并立即终止响应
        /// </summary>
        /// <param name="content">写入对象</param>
        public static void WriteToEnd(object content)
        {
            HttpContext.Current.Response.Write(content);
            HttpContext.Current.Response.End();
        }
        #endregion

        #region Redirect 请求重定向
        /// <summary>
        ///  请求重定向
        /// </summary>
        /// <param name="url">跳转Url地址</param>
        public static void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(url, true);
        }
        #endregion
        
        #region GetServerVariable 返回指定的服务器变量信息
        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="variableName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerVariable(string variableName)
        {
            if (HttpContext.Current.Request.ServerVariables[variableName] == null)
            {
                return string.Empty;
            }
            return HttpContext.Current.Request.ServerVariables[variableName];
        }
        #endregion

        #region GetUrlParam 获取指定Url参数的值
        /// <summary>
        ///  获取指定Url参数的值
        /// </summary>
        /// <param name="key">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetUrlParam(string key)
        {
            return GetUrlParam(key, false);
        }
        #endregion

        #region GetUrlParam 获取指定Url参数的值
        /// <summary>
        /// 获取指定Url参数的值
        /// </summary> 
        /// <param name="key">Url参数</param>
        /// <param name="sqlSafeCheck">是否进行Sql安全检查</param>
        /// <returns>Url参数的值</returns>
        public static string GetUrlParam(string key, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.QueryString[key] == null)
            {
                return string.Empty;
            }
            if (sqlSafeCheck && !ValidateKit.IsSafeSqlString(HttpContext.Current.Request.QueryString[key]))
            {
                return "unsafe string";
            }
            return HttpContext.Current.Request.QueryString[key];
        }
        #endregion

        #region UrlParamCount 当前请求Url参数数量
        /// <summary>
        ///  当前请求Url参数数量
        /// </summary>
        public static int UrlParamCount
        {
            get { return HttpContext.Current.Request.QueryString.Count; }
        }
        #endregion

        #region GetFormParam 获取指定表单参数的值
        /// <summary>
        /// 获取指定表单参数的值
        /// </summary>
        /// <param name="key">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormParam(string key)
        {
            return GetFormParam(key, false);
        }
        #endregion

        #region GetFormParam 获取指定表单参数的值
        /// <summary>
        /// 获取指定表单参数的值
        /// </summary>
        /// <param name="key">表单参数</param>
        /// <param name="sqlSafeCheck">是否进行Sql安全检查</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormParam(string key, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[key] == null)
            {
                return string.Empty;
            }
            if (sqlSafeCheck && !ValidateKit.IsSafeSqlString(HttpContext.Current.Request.Form[key]))
            {
                return string.Empty;
            }
            return HttpContext.Current.Request.Form[key];
        }
        #endregion

        #region FormParamCount 当前请求表单参数数量
        /// <summary>
        ///  当前请求表单参数数量
        /// </summary>
        public static int FormParamCount
        {
            get { return HttpContext.Current.Request.Form.Count; }
        }
        #endregion

        #region WriteCookie 完整写入Cookie值
        /// <summary>
        /// 完整写入Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        public static void WriteCookie(string cookieName, string cookieValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region WriteCookie 完整写入Cookie值
        /// <summary>
        /// 完整写入Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="expires">Cookie过期时间(分钟)</param>
        public static void WriteCookie(string cookieName, string cookieValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region WriteCookieValue 写入Cookie单个值
        /// <summary>
        /// 写入Cookie单个值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">Cookie键</param>
        /// <param name="value">值</param>
        public static void WriteCookieValue(string cookieName, string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie[key] = value;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region WriteCookieValue 写入Cookie单个值
        /// <summary>
        /// 写入Cookie单个值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">Cookie键</param>
        /// <param name="value">值</param>
        /// <param name="expires">Cookie过期时间(分钟)</param>
        public static void WriteCookieValue(string cookieName, string key, string value,int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie[key] = value;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region GetCookie 读取Cookie完整值
        /// <summary>
        /// 读取Cookie完整值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie != null)
                {
                    return cookie.Value;
                }
            }
            return string.Empty;
        }
        #endregion

        #region GetCookieValue 读取Cookie某一值
        /// <summary>
        /// 读取Cookie某一值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">键名</param>
        /// <returns>值</returns>
        public static string GetCookieValue(string cookieName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie != null)
                {
                    return cookie[key];
                }
            }
            return string.Empty;
        }
        #endregion

        #region GetMapPath 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="path">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            path = path.Replace("/", "\\");
            if (path.StartsWith("\\"))
            {
                path = path.Substring(path.IndexOf('\\', 1)).TrimStart('\\');
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }
        #endregion

        #region GetFormCheckboxValue 获取From表单中Checkbox的值
        /// <summary>
        ///  获取From表单中Checkbox的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool GetFormCheckboxValue(string name)
        {
            string[] values = HttpContext.Current.Request.Form.GetValues(name) ?? new[] { "false" };
            return values.Length == 2
                       ? (Convert.ToBoolean(values[0]) || Convert.ToBoolean(values[1]))
                       : Convert.ToBoolean(values[0]);
        }
        #endregion

        #region GetUrlCheckboxValue
        /// <summary>
        ///  获取Url参数中Checkbox的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool GetUrlCheckboxValue(string name)
        {
            string[] values = HttpContext.Current.Request.QueryString.GetValues(name) ?? new[] { "false" };
            return values.Length == 2
                       ? (Convert.ToBoolean(values[0]) || Convert.ToBoolean(values[1]))
                       : Convert.ToBoolean(values[0]);
        }
        #endregion



        #region GetFormValues 读取From表单中的某一组值
        /// <summary>
        ///  读取From表单中的某一组值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<TValue> GetFormValues<TValue>(string name)
        {
            List<TValue> list = new List<TValue>();
            string[] values = HttpContext.Current.Request.Form.GetValues(name);
            if (values != null)
            {
                foreach (string value in values)
                {
                    list.Add(ConvertKit.Convert(value, default(TValue)));
                }
            }
            return list;
        }
        #endregion

        #region ClearResponseCache 清除客户端缓存
        /// <summary>
        ///  清除客户端缓存
        /// </summary>
        public static void ClearResponseCache()
        {
            HttpContext.Current.Response.Cache.SetNoStore();
        }
        #endregion


        #region GetMac 获得当前访问MAC地址
        /// <summary>
        /// 获得当前访问MAC地址
        /// </summary>
        ///
        /// <returns>访问MAC地址</returns>
        public static string GetMac()
        {

            string stringMAC = "";

            ManagementClass MC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection MOC = MC.GetInstances();

            foreach (ManagementObject MO in MOC)
            {
                if ((bool)MO["IPEnabled"] == true)
                {
                    stringMAC += MO["MACAddress"].ToString();
                }

            }
            return stringMAC;
        }
        #endregion

        #region
        /// <summary>
        /// 获取本地计算机的IP(ipv4)地址。
        /// </summary>
        public static string  GetIPAddresses(string name)
        {
            StringBuilder sb = new StringBuilder();

            // 获取所有网络接口的列表(通常是一个网卡、拨号和VPN连接)
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface network in networkInterfaces)
            {
                if (network.Name == name)
                {
                    // 读取每个网络的IP配置
                    IPInterfaceProperties properties = network.GetIPProperties();

                    // 每个网络接口可能有多个IP地址
                    foreach (IPAddressInformation address in properties.UnicastAddresses)
                    {
                        if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;

                        // 忽略回送地址 (e.g., 127.0.0.1)
                        if (IPAddress.IsLoopback(address.Address))
                            continue;

                        //sb.AppendLine(address.Address.ToString() + " (" + network.Name + ")");
                        sb.AppendLine(address.Address.ToString());
                        sb.AppendLine();
                    }
                    break;
                }
            }
            return sb.ToString().Trim();
        }
        #endregion
    }
}
