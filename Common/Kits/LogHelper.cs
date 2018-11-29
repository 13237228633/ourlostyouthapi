using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.Kits
{
    /// <summary>  
    /// 日志类型  
    /// </summary>  
    public enum LogFile
    {
        /// <summary>
        /// 崩溃日志
        /// </summary>
        Crash,
        /// <summary>
        /// 
        /// </summary>
        Trace,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// SQl语句
        /// </summary>
        SQL,
        /// <summary>
        /// 测试数据
        /// </summary>
        Test,
        /// <summary>
        ///猫播测试
        /// </summary>
        TMaoBo,
        /// <summary>
        ///猫播数据
        /// </summary>
        TMBData,
        /// <summary>
        /// 充值
        /// </summary>
        IOSPay,
        AliPay,
        WXPay,
        Data,
        /// <summary>
        /// 游戏
        /// </summary>
        Game,
        Temp,
        Debug,
        Active,
        GooglePay,
        GoogLeFristPay

    }
    public static class LogHelper
    {
        private static object lockHelper = new object();
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logFile">日志类型</param>
        /// <param name="requestUrl">请求的路径</param>
        /// <param name="msg">日志信息</param>
        private static void Log(string logFile, string url, string agentName, string msg)
        {
            //string path = ConfigurationManager.AppSettings["LogPath"];
            string path = HttpRuntime.AppDomainAppPath.ToString();
            if (path.Length == 0)
                return;

            string fileName = path + "LogPath\\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\\" + logFile.ToString() + ".txt";
            StringBuilder sb = new StringBuilder(8192);
            //写入一些必要的错误信息。
            sb.AppendFormat("时间:{0},设备:{1},IP:{3},路径:{2}\r\n", DateTime.Now, agentName, url, Tools.GetRealIP());
            sb.AppendFormat("内容:{0}\r\n", msg);
            sb.Append("-------------------------------------------------------------\r\n");

            FileInfo fi = new FileInfo(fileName);
            try
            {
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                lock (lockHelper)
                {
                    using (StreamWriter writer = fi.AppendText())
                    {
                        writer.Write(sb);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        /// <summary>
        /// 写日志1
        /// </summary>
        /// <param name="logFile">日志类型</param>
        /// <param name="requestUrl">请求的路径</param>
        /// <param name="msg">日志信息</param>
        public static void WriteLog(LogFile logFile, string msg)
        {
            string fullPath = GetPath();
            string agentName = Tools.GetAgentName();

            Log(logFile.ToString(), fullPath, agentName, msg);
        }
        /// <summary>
        /// 写日志2
        /// </summary>
        /// <param name="logFile">日志类型</param>
        /// <param name="requestUrl">请求的路径</param>
        /// <param name="msg">日志信息</param>
        //public static void WriteLog(LogFile logFile, string url, string msg)
        //{
        //    string agentName = Tools.GetAgentName();

        //    Log(logFile.ToString(), url, agentName, msg);
        //}

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <returns></returns>
        private static string GetPath()
        {
            var context = HttpContext.Current;
            if (null != context)
            {
                return context.Request.Url.Host + context.Request.Path;
            }
            return "";
        }
    }
}
