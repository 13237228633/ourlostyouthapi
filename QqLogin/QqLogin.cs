using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QqLogin
{
    public class ExecQqLogin
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isMobile">判断是否是用手机设备，true为是，false为否</param>
        /// <param name="APPID">QQ互联应用管理中的 APP ID</param>
        /// <param name="redirecturi">QQ互联应用管理中的 网站回调域 </param>
        /// <returns>打开QQ登录地址</returns>
        public string openLogin(bool isMobile, string APPID, string redirecturi) 
        {
            if (isMobile)
                return "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id=" + APPID + "&redirect_uri=" + redirecturi + "&state=1995&scope=get_user_info,list_album,upload_pic,do_like&display=mobile";
            else
                return "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id=" + APPID + "&redirect_uri=" + redirecturi + "&state=1995&scope=get_user_info,list_album,upload_pic,do_like";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="APPID">QQ互联应用管理中的 APP ID</param>
        /// <param name="APPKey">QQ互联应用管理中的 APP Key</param>
        /// <param name="code">QQ登录回调时获取到的参数</param>
        /// <param name="redirecturi">QQ互联应用管理中的 网站回调域 </param>
        /// <returns>登录账号的QQ用户信息，与提示信息</returns>
        public Messge QqLogin(string APPID, string APPKey, string code,string redirecturi)
        {
            Messge messge = new Messge();
            string ulr = "https://graph.qq.com/oauth2.0/token";

            Dictionary<string, string> Parameter = new Dictionary<string, string>();
            Parameter.Add("grant_type", "authorization_code");
            Parameter.Add("client_id", APPID);
            Parameter.Add("client_secret", APPKey);
            Parameter.Add("code", code);
            Parameter.Add("redirect_uri", redirecturi);
            Parameter.Add("state", "1995");

            Message message = HttpRequest.ExecHttpRequest(ulr, Parameter, false, true);
            if (!message.data.Contains("error") && !message.data.Contains("callback") && message.code == 100)
            {
                access_token = message.data.Split('&')[0].Split('=')[1];
                expires_in = message.data.Split('&')[1].Split('=')[1];
                refresh_token = message.data.Split('&')[2].Split('=')[1];
            }
            else
            {
                messge.Code = 101;
                messge.messge = "access_token获取失败";
                messge.openid = "";
                messge.qqLoginUser = null;
                return messge;
            }

            string ulr1 = "https://graph.qq.com/oauth2.0/me";
            Dictionary<string, string> Parameter1 = new Dictionary<string, string>();
            Parameter1.Add("access_token", access_token);
            Message message1 =  HttpRequest.ExecHttpRequest(ulr1, Parameter1, false, true);

            if (!message1.data.Contains("error")&& message1.code==100)
            {
                string[] sArray = Regex.Split(message1.data, "\"openid\":\"", RegexOptions.IgnoreCase);
                openid = sArray[1].Split('\"')[0];
            }
            else
            {
                messge.Code = 102;
                messge.messge = "openid获取失败";
                messge.openid = "";
                messge.qqLoginUser = null;
                return messge;
            }

            string ulr2 = "https://graph.qq.com/user/get_user_info";
            Dictionary<string, string> Parameter2 = new Dictionary<string, string>();
            Parameter2.Add("access_token", access_token);
            Parameter2.Add("oauth_consumer_key", APPID);
            Parameter2.Add("openid", openid);
            Message message2 = HttpRequest.ExecHttpRequest(ulr2, Parameter2,false, true);
            try
            {
                messge.Code = 100;
                messge.messge = "QQ用户信息获取成功";
                messge.openid = openid;
                messge.qqLoginUser = JsonConvert.DeserializeObject<QqLoginUser>(message2.data);
                return messge;
            }
            catch (Exception)
            {
                messge.Code = 103;
                messge.messge = "QQ用户信息获取失败 错误信息："+ message2.data;
                messge.openid = "";
                messge.qqLoginUser = null;
                return messge;
            }
        }

        public string getWebRequest(string ulr)
        {
            try
            {
                //测试编码问题
                WebRequest request = WebRequest.Create(ulr);
                //Post请求方式
                request.Method = "GET";
                // 内容类型
                request.ContentType = "application/x-www-form-urlencoded";
                WebResponse response = request.GetResponse();
                System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string str = myreader.ReadToEnd();

                myreader.Close();
                return str;
            }
            catch (Exception ex)
            {
                return "错误：" + ex.Message;
            }
        }
    }
    /// <summary>
    /// 返回信息类
    /// </summary>
    public class Messge
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string messge { get; set; }
        /// <summary>
        /// QQ用户的唯一标识openid
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// QQ用户的信息
        /// </summary>
        public QqLoginUser qqLoginUser { get; set; }
    }

    //qq登录信息json解析类
    public class QqLoginUser
    {
        public string ret { get; set; }
        public string msg { get; set; }
        public string is_lost { get; set; }
        public string nickname { get; set; }
        public string gender { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string year { get; set; }
        public string figureurl { get; set; }
        public string figureurl_1 { get; set; }
        public string figureurl_2 { get; set; }
        public string figureurl_qq_1 { get; set; }
        public string figureurl_qq_2 { get; set; }
        public string is_yellow_vip { get; set; }
        public string vip { get; set; }
        public string yellow_vip_level { get; set; }
        public string level { get; set; }
        public string is_yellow_year_vip { get; set; }
    }
}

