namespace Common
{
    using System;
    using System.Text;
    using System.Web;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class HtmlKit
    {
        #region GroupHtmlElements
        /// <summary>
        ///  
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<string> GroupHtmlElements(string start, string end, string html)
        {
            List<string> list = new List<string>();
            try
            {
                string pattern = string.Format("{0}(?<g>(.|[\r\n])+?){1}", start, end);
                MatchCollection matchCollection = Regex.Matches(html, pattern);
                if (matchCollection.Count != 0)
                {
                    foreach (Match match in matchCollection)
                    {
                        GroupCollection gc = match.Groups;
                        list.Add(gc["g"].Value);
                    }
                }
            }
            catch
            {
                throw new Exception("Regex Error!");
            }
            return list;
        }
        #endregion

        #region SingleHtmlElement
        /// <summary>
        ///  
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string SingleHtmlElement(string start, string end, string html)
        {
            string content;
            try
            {
                string pattern = string.Format("{0}(?<g>(.|[\r\n])+?)?{1}", start, end);
                content = Regex.Match(html, pattern).Groups["g"].Value;
            }
            catch
            {
                throw new Exception("Regex Error!");
            }
            return content;
        }
        #endregion

        #region RemoveHtmlTag 移除Html标记
        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string targetString)
        {
            return Regex.Replace(targetString, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }
        #endregion

        #region RemoveUnsafeTag 过滤HTML中的不安全标签
        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns></returns>
        public static string RemoveUnsafeTag(string targetString)
        {
            targetString = Regex.Replace(targetString, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            targetString = Regex.Replace(targetString, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return targetString;
        }
        #endregion

        #region RemoveEnter 去除换行符号
        /// <summary>
        ///  去除换行符号
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static string RemoveEnter(string targetString)
        {
            string resultString;
            if (targetString == null)
            {
                resultString = string.Empty;
            }
            else
            {
                targetString = targetString.Replace("\r\n", "");
                targetString = targetString.Replace("\n", "");
                targetString = targetString.Replace("\r", "");
                resultString = targetString;
            }
            return resultString;
        }

        #endregion

        #region RemoveHtmlTagAndSrcript
        /// <summary>
        /// 去除HTML标记和脚本标签
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string RemoveHtmlTarget(string htmlstring)
        {
            //删除脚本
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            htmlstring.Replace("<", "");
            htmlstring.Replace(">", "");
            htmlstring.Replace("\r\n", "");
            htmlstring = HttpContext.Current.Server.HtmlEncode(htmlstring).Trim();

            return htmlstring;
        }
        #endregion

        #region RemoveUbbTag 清除UBB标签
        /// <summary>
        /// 清除UBB标签
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>清除后字符串</returns>
        public static string RemoveUbbTag(string targetString)
        {
            return Regex.Replace(targetString, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }
        #endregion

        #region GetTextFromHtml 从HTML中获取文本,保留br,p,img
        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static string GetTextFromHtml(string targetString)
        {
            Regex regex = new Regex(@"</?(?!br|/?p|img)[^>]*>", RegexOptions.IgnoreCase);
            return regex.Replace(targetString, "");
        }
        #endregion
       
        #region int CountCharacter(string str)
        /// <summary>
        ///  计算汉字字数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CountChineseCharacter(string str)
        {
            CharEnumerator charEnumerator = str.GetEnumerator();
            Regex regex = new Regex("^[\u4E00-\u9FA5]{0,}$");
            int chineseCount = 0;
            while (charEnumerator.MoveNext())
            {
                if (regex.IsMatch(charEnumerator.Current.ToString(), 0))
                {
                    chineseCount++;
                }
            }
            return chineseCount;
        }

        #endregion

        #region int CountNonChineseCharacter(string str)
        /// <summary>
        ///  计算非汉字字数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CountNoChineseCharacter(string str)
        {
            CharEnumerator charEnumerator = str.GetEnumerator();
            Regex regex = new Regex("^[\u4E00-\u9FA5]{0,}$");
            int normalCount = 0;
            while (charEnumerator.MoveNext())
            {
                if (!regex.IsMatch(charEnumerator.Current.ToString(), 0))
                {
                    normalCount++;
                }
            }
            return normalCount;
        }
        #endregion

        #region CreateSpacesString 生成指定数量的html空格符号
        /// <summary>
        /// 生成指定数量的html空格符号
        /// </summary>
        public static string CreateSpacesString(int spacesCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < spacesCount; i++)
            {
                sb.Append("&nbsp;&nbsp;");
            }
            return sb.ToString();
        }
        #endregion

        #region EnterToBr 替换回车换行符为html换行符
        /// <summary>
        ///  替换回车换行符为html换行符
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static string EnterToBr(string targetString)
        {
            string resultString;
            if (targetString == null)
            {
                resultString = string.Empty;
            }
            else
            {
                targetString = targetString.Replace("\r\n", "<br />");
                targetString = targetString.Replace("\n", "<br />");
                resultString = targetString;
            }
            return resultString;
        }
        #endregion

        #region EncodeSymbol 将标点符号替换为Html符号标记
        /// <summary>
        ///  将标点符号替换为Html符号标记
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns></returns>
        public static string EncodeSymbol(string targetString)
        {
            if (!string.IsNullOrEmpty(targetString))
            {
                targetString = targetString.Replace(",", "&def");
                targetString = targetString.Replace("'", "&dot");
                targetString = targetString.Replace(";", "&dec");
                return targetString;
            }
            return string.Empty;
        }
        #endregion

        #region HtmlEncode 返回HTML字符串的编码结果
        /// <summary>
        /// 返回HTML字符串的编码结果
        /// </summary>
        /// <param name="targetString">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(string targetString)
        {
            return HttpUtility.HtmlEncode(targetString);
        }
        #endregion

        #region HtmlDecode 返回HTML字符串的解码结果
        /// <summary>
        /// 返回HTML字符串的解码结果
        /// </summary>
        /// <param name="targetString">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(string targetString)
        {
            return HttpUtility.HtmlDecode(targetString);
        }
        #endregion

        #region UrlEncode 返回URL字符串的编码结果
        /// <summary>
        /// 返回URL字符串的编码结果
        /// </summary>
        /// <param name="targetString">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string targetString)
        {
            return HttpUtility.UrlEncode(targetString);
        }
        #endregion

        #region UrlDecode 返回URL字符串的编码结果
        /// <summary>
        /// 返回URL字符串的编码结果
        /// </summary>
        /// <param name="targetString">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string targetString)
        {
            return HttpUtility.UrlDecode(targetString);
        }
        #endregion


        
    }
}
