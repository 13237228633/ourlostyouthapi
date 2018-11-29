using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QqLogin
{
    public static class HttpRequest
    {
        /// <summary>
        /// HttpRequest C# http请求封装类
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="Parameter">请求参数</param>
        /// <param name="IsGet">true:get; false:post 默认:get</param>
        /// <param name="IsUrlEncode">是否编码，默认:true</param>
        /// <returns></returns>
        public static Message ExecHttpRequest(string url ,Dictionary<string, string> Parameter=null,bool IsGet=true, bool IsUrlEncode = true)
        {
            string strParameter = "";
            if (Parameter != null)
            {
                foreach (KeyValuePair<string, string> kvp in Parameter)
                {
                    if (IsUrlEncode)
                    {
                        strParameter += kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value.ToString()) + "&";
                    }
                    else
                    {
                        strParameter += kvp.Key + "=" + kvp.Value.ToString() + "&";
                    }
                    strParameter.TrimEnd('&');
                }
            }

            if (IsGet)
            {
                try
                {
                    //测试编码问题
                    WebRequest request = WebRequest.Create(url + "?" + strParameter);
                    //Post请求方式
                    request.Method = "GET";
                    // 内容类型
                    request.ContentType = "application/x-www-form-urlencoded";
                    WebResponse response = request.GetResponse();
                    System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string str = myreader.ReadToEnd();
                    myreader.Close();
                    Message message = new Message();
                    message.code = 100;
                    message.messages = "请求成功";
                    message.data = str;
                    return message;
                }
                catch (Exception ex)
                {
                    Message message = new Message();
                    message.code = 101;
                    message.messages = "请求失败";
                    message.data = ex.Message;
                    return message;
                }
            }
            else
            {
                try
                {
                    //测试编码问题
                    System.Net.HttpWebRequest request;
                    request = (System.Net.HttpWebRequest)HttpWebRequest.Create(url);
                    //Post请求方式
                    request.Method = "POST";
                    // 内容类型
                    request.ContentType = "application/x-www-form-urlencoded";

                    //这是原始代码：
                    //将URL编码后的字符串转化为字节
                    byte[] payload = System.Text.Encoding.UTF8.GetBytes(strParameter);
                    //设置请求的 ContentLength 
                    request.ContentLength = payload.Length;
                    //获得请 求流
                    Stream writer = request.GetRequestStream();
                    //将请求参数写入流
                    writer.Write(payload, 0, payload.Length);
                    // 关闭请求流
                    writer.Close();
                    System.Net.HttpWebResponse response;
                    // 获得响应流
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                    System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string str = myreader.ReadToEnd();
                    myreader.Close();
                    Message message = new Message();
                    message.code = 100;
                    message.messages = "请求成功";
                    message.data = str;
                    return message;
                }
                catch (Exception ex)
                {
                    Message message = new Message();
                    message.code = 101;
                    message.messages = "请求失败";
                    message.data = ex.Message;
                    return message;
                }
              
            }
        }

       
    }
    public class Message
    {
        public int code { get; set; }
        public string messages { get; set; }
        public string data { get; set; }
    }
}
