using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;
using System.Data;
using System.Collections.Generic;

namespace Common
{
    public static class Tools
    {
        /// <summary>
        /// 手机号验证
        /// </summary>
        public static string TelRegex = @"^0?(13\d|14[5,7,9]|15[0-3,5-9]|17[0,1,3,5-8]|18\d)\d{8}$";

        public static Regex numRegex = new Regex(@"^\d+$");
        /// <summary>
        /// 验证输入字符串为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            return Regex.IsMatch(str, "^([0]|([1-9]+\\d{0,}?))(.[\\d]+)?$");
        }
        /// <summary>
        /// 是否为字母加数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLetterAndNumber(string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z0-9]*$");
        }

        /// <summary>
        /// 是否为汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]+$");
        }

        /// <summary>
        /// 是否为英文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEnglish(string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z]");
        }
        /// <summary>
        /// 验证输入字符串为电话号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPhone(string str)
        {
            return Regex.IsMatch(str, @"^0?(13\d|14[5,7,9]|15[0-3,5-9]|17[0,1,3,5-8]|18\d)\d{8}$");
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
        /// <summary>
        /// 分页返回总页数
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int GetPageCount(int totalCount, int pageSize)
        {
            int pageCount = 0;
            if (totalCount % pageSize == 0)
            {
                pageCount = totalCount / pageSize;
            }
            else
            {
                pageCount = totalCount / pageSize + 1;
            }
            return pageCount;
        }
        /// <summary>
        /// IP地址转换
        /// </summary>
        /// <param name="Ip"></param>
        /// <returns></returns>
        public static long ConvertIpToMetric(string Ip)
        {
            //取出IP地址去掉‘.’后的string数组
            string[] Ip_List = Ip.Split(".".ToCharArray());
            string X_Ip = "";
            //循环数组，把数据转换成十六进制数，并合并数组(3dafe81e)
            foreach (string ip in Ip_List)
            {
                X_Ip += Convert.ToInt16(ip).ToString("x");
            }

            //将十六进制数转换成十进制数(1034938398)
            long N_Ip = long.Parse(X_Ip, System.Globalization.NumberStyles.HexNumber);

            return N_Ip;
        }
        #region 产生随机数

        /// <summary>
        /// 生成n位字母
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateLetter(int Length)
        {
            char[] constant = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            StringBuilder newRandom = new StringBuilder(constant.Length);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(constant.Length - 1)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 生成n为验证码
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateNum(int Length)
        {
            char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder newRandom = new StringBuilder(constant.Length);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(constant.Length - 1)]);
            }
            return newRandom.ToString();
        }
        public static string Generate(int length)
        {
            char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            StringBuilder newRandom = new StringBuilder(constant.Length);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(constant.Length - 1)]);
            }
            return newRandom.ToString().ToLower();
        }

        #endregion


        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetRealIP()
        {
            string result = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result) || !Tools.IsIP(result))
                result = "127.0.0.1";

            return result;
        }
        /// <summary>
        /// 是否是手机终端
        /// </summary>
        /// <returns></returns>
        public static bool IsMobile()
        {
            string agent = (HttpContext.Current.Request.UserAgent + "").ToLower().Trim();
            if (agent == "" ||
                agent.IndexOf("mobile", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("mobi", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("nokia", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("samsung", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("sonyericsson", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("mot", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("blackberry", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("lg", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("htc", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("j2me", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("ucweb", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("opera mini", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("mobi", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("android", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("iphone", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("ipad", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("ipod", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("okhttp", StringComparison.Ordinal) != -1 ||
                agent.IndexOf("miao bo", StringComparison.Ordinal) != -1
                )
            {
                //终端可能是手机
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取代理名称
        /// </summary>
        /// <returns></returns>
        public static string GetAgentName()
        {
            string agent = HttpContext.Current.Request.UserAgent;
            if (null == agent)
            {
                return "未知";
            }
            string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Ruby", "okhttp" };

            ////排除Window 桌面系统 和 苹果桌面系统
            if (!agent.Contains("Windows NT") && !agent.Contains("Macintosh"))
            {
                foreach (string item in keywords)
                {
                    if (agent.Contains(item))
                    {
                        return item;
                    }
                }
            }
            return "PC";
        }

        #region 图片压缩

        //生成缩略图函数
        //顺序参数：源图文件流、缩略图存放地址、模版宽、模版高
        //注：缩略图大小控制在模版区域内
        public static void MakeSmallImg(Stream fromFileStream, string fileSaveUrl, System.Double templateWidth, System.Double templateHeight)
        {
            //从文件取得图片对象，并使用流中嵌入的颜色管理信息
            System.Drawing.Image myImage = System.Drawing.Image.FromStream(fromFileStream, true);
            //缩略图宽、高
            System.Double newWidth = myImage.Width, newHeight = myImage.Height;
            //宽大于模版的横图
            if (myImage.Width > myImage.Height || myImage.Width == myImage.Height)
            {
                if (myImage.Width > templateWidth)
                {
                    //宽按模版，高按比例缩放
                    newWidth = templateWidth;
                    newHeight = myImage.Height * (newWidth / myImage.Width);
                }
            }
            //高大于模版的竖图
            else
            {
                if (myImage.Height > templateHeight)
                {
                    //高按模版，宽按比例缩放
                    newHeight = templateHeight;
                    newWidth = myImage.Width * (newHeight / myImage.Height);
                }
            }
            //取得图片大小
            System.Drawing.Size mySize = new Size((int)newWidth, (int)newHeight);
            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(mySize.Width, mySize.Height);
            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空一下画布
            g.Clear(Color.White);
            //在指定位置画图
            g.DrawImage(myImage, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
            new System.Drawing.Rectangle(0, 0, myImage.Width, myImage.Height),
            System.Drawing.GraphicsUnit.Pixel);
            ///文字水印
            //System.Drawing.Graphics G = System.Drawing.Graphics.FromImage(bitmap);
            //System.Drawing.Font f = new Font("宋体", 13);
            //System.Drawing.Brush b = new SolidBrush(Color.Black);
            //G.DrawString("9158直播", f, b, 10, 10);
            //G.Dispose();
            ///图片水印
            //System.Drawing.Image copyImage = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("pic/1.gif"));
            //Graphics a = Graphics.FromImage(bitmap);
            //a.DrawImage(copyImage, new Rectangle(bitmap.Width-copyImage.Width,bitmap.Height-copyImage.Height,copyImage.Width, copyImage.Height),0,0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
            //copyImage.Dispose();
            //a.Dispose();
            //copyImage.Dispose();
            //保存缩略图
            bitmap.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            g.Dispose();
            myImage.Dispose();
            bitmap.Dispose();
        }
        #endregion

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns>分好页的DataTable数据</returns>  
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0) { return dt; }
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
            { return newdt; }

            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }

            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }


        /// <summary>
        /// 获取代理名称
        /// </summary>
        /// <returns></returns>
        public static string GetdeviceName()
        {
            string agent = HttpContext.Current.Request.UserAgent.ToLower();
            if (null == agent) return "unknown";

            List<string> list = new List<string>() { "Android", "iPhone", "iPod", "iPad", "Ruby", "okhttp", "Windows" };

            string deviceName = list.Find(f => agent.ToLower().Contains(f.ToLower()));

            if (deviceName.Equals("Ruby") || deviceName.Equals("okhttp"))
                deviceName = "Android";
            if (agent.Contains("91"))
                deviceName = "miaopai";
            else if (agent.ToLower().Contains("micromessenger"))
                deviceName = "WeiXin";
            else if (agent.ToLower().Contains("qq"))
                deviceName = "QQ";
            if (string.IsNullOrEmpty(deviceName))
                deviceName = "PC";
            return deviceName;
        }

    }
}
