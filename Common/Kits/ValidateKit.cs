namespace Common
{
    using System.Text.RegularExpressions;

    public static class ValidateKit
    {
        #region IsNumeric 判断对象是否为Int32类型的数字
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNumeric(object target)
        {
            if (target != null)
            {
                return IsNumeric(target.ToString());
            }
            return false;
        }
        #endregion

        #region IsNumeric 判断字符串是否为Int32类型的数字
        /// <summary>
        /// 判断字符串是否为Int32类型的数字
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static bool IsNumeric(string targetString)
        {
            if (targetString != null)
            {
                string @string = targetString;
                if (@string.Length > 0 && @string.Length <= 11 && Regex.IsMatch(@string, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((@string.Length < 10) || (@string.Length == 10 && @string[0] == '1') || (@string.Length == 11 && @string[0] == '-' && @string[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region IsPositiveInteger 验证是否为正整数
        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static bool IsPositiveInteger(string targetString)
        {
            return Regex.IsMatch(targetString, @"^[0-9]*$");
        }
        #endregion
        
        #region IsDouble 判断对象是否为Double类型
        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsDouble(object target)
        {
            if (target != null)
            {
                return Regex.IsMatch(target.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }
        #endregion

        #region IsNumericArray 判断给定字符串数据中是否全为Int32类型数字
        /// <summary>
        /// 判断给定字符串数据中是否全为Int32类型数字
        /// </summary>
        /// <param name="targetArray">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] targetArray)
        {
            if (targetArray == null)
            {
                return false;
            }
            if (targetArray.Length < 1)
            {
                return false;
            }
            foreach (string id in targetArray)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region IsEmail 检测是否符合email格式
        /// <summary>
        ///  检测是否符合email格式
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsEmail(string targetString)
        {
            return Regex.IsMatch(targetString, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }
        #endregion

        #region IsUrl 检测是否是有效的Url地址
        /// <summary>
        /// 检测是否是有效的Url地址
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsUrl(string targetString)
        {
            return Regex.IsMatch(targetString, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
        #endregion

        #region GetEmailHostName 获取邮箱域名
        /// <summary>
        ///  获取邮箱域名 
        /// </summary>
        /// <param name="emailString">邮箱地址</param>
        /// <returns>邮箱域名</returns>
        public static string GetEmailHostName(string emailString)
        {
            if (emailString.IndexOf("@") < 0)
            {
                return string.Empty;
            }
            return emailString.Substring(emailString.LastIndexOf("@")).ToLower();
        }
        #endregion

        #region IsBase64String 判断是否为base64字符串
        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsBase64String(string targetString)
        {
            return Regex.IsMatch(targetString, @"[A-Za-z0-9\+\/\=]");
        }
        #endregion

        #region IsSafeSqlString 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string targetString)
        {
            return !Regex.IsMatch(targetString, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        #endregion

        #region IsTime 判断是否是有效的时间格式字符串
        /// <summary>
        ///  判断是否是有效的时间格式字符串
        /// </summary>
        /// <param name="timeval"></param>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }
        #endregion

        #region IsIP 判断是否为IP
        /// <summary>
        /// 是否为IP
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns></returns>
        public static bool IsIP(string targetString)
        {
            return Regex.IsMatch(targetString, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region IsIPSect 判断是否为IsIPSect
        /// <summary>
        ///  targetString
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns></returns>
        public static bool IsIPSect(string targetString)
        {
            return Regex.IsMatch(targetString, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }
        #endregion

        #region InIPArray 判断指定IP是否在指定的IP数组所限定的范围内
        /// <summary>
        /// 判断指定IP是否在指定的IP数组所限定的范围内
        /// IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="targetIP">目标IP</param>
        /// <param name="ipArray">提供检测的IP数组</param>
        /// <returns></returns>
        public static bool InIPArray(string targetIP, string[] ipArray)
        {
            string[] userIP = StringKit.Split(targetIP, @".");

            for (int ipIndex = 0; ipIndex < ipArray.Length; ipIndex++)
            {
                string[] tempIP = StringKit.Split(ipArray[ipIndex], @".");
                int num = 0;
                for (int i = 0; i < tempIP.Length; i++)
                {
                    if (tempIP[i] == "*")
                    {
                        return true;
                    }
                    if (userIP.Length > i)
                    {
                        if (tempIP[i] == userIP[i])
                        {
                            num++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (num == 4)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region IsDate 判断字符串是否是yyyy-MM-dd格式
        /// <summary>
        /// 判断字符串是否是yyyy-MM-dd格式
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsDate(string targetString)
        {
            return Regex.IsMatch(targetString, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }
        #endregion

        #region RemoveUnsafeString 过滤脚本注入和SQL注入字符
        /// <summary>
        /// 过滤脚本注入和SQL注入字符
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string RemoveUnsafeString(string targetString)
        {
            if (string.IsNullOrEmpty(targetString))
            {
                return targetString;
            }
            targetString = Regex.Replace(targetString, @"<script[^>]*?>.*?</script>", string.Empty, RegexOptions.IgnoreCase);
            return Regex.Replace(targetString, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']", string.Empty, RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
