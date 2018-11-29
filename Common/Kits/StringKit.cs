namespace Common
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using Microsoft.VisualBasic;

    /// <summary>
    ///  字符串工具类
    /// </summary>
    public static class StringKit
    {
        #region GetByteLength 返回字符串字节数
        /// <summary>
        ///  返回字符串字节数（1个汉字占2个字节）
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>占用字节数</returns>
        public static int GetByteLength(string targetString)
        {
            return Encoding.Default.GetBytes(targetString).Length;
        }
        #endregion

        #region GetIndexInArray 返回字符串在指定字符串数组中的位置
        /// <summary>
        ///  返回字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="targetArray">目标数组</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>字符串所处位置</returns>
        public static int GetIndexInArray(string targetString, string[] targetArray, bool ignoreCase)
        {
            for (int i = 0; i < targetArray.Length; i++)
            {
                if (ignoreCase)
                {
                    if (targetString.ToLower() == targetArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else if (targetString == targetArray[i])
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        #region IsInArray 判断字符串是否在指定字符串数组内
        /// <summary>
        ///  判断字符串是否在指定字符串数组内
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="targetArray">目标数组</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static bool IsInArray(string targetString, string[] targetArray, bool ignoreCase)
        {
            return GetIndexInArray(targetString, targetArray, ignoreCase) >= 0;
        }
        #endregion

        #region Trim 清除字符串中的回车/换行/空格
        /// <summary>
        ///  清除字符串中的回车/换行/空格
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>处理后的新字符串</returns>
        public static string Trim(string targetString)
        {
            for (int i = targetString.Length; i >= 0; i--)
            {
                if (targetString[i].Equals(" ") || targetString[i].Equals("\r") || targetString[i].Equals("\n"))
                {
                    targetString.Remove(i, 1);
                }
            }
            return targetString;
        }
        #endregion

        #region ClearBr 清除字符串中的回车/换行
        /// <summary>
        ///  清除字符串中的回车/换行
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>处理后的新字符串</returns>
        public static string ClearBr(string targetString)
        {
            Regex regex = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);
            for (Match match = regex.Match(targetString); match.Success; match = match.NextMatch())
            {
                targetString = targetString.Replace(match.Groups[0].ToString(), "");
            }
            return targetString;
        }
        #endregion

        #region CutString 从字符串的指定位置截取指定长度
        /// <summary>
        ///  从字符串的指定位置截取指定长度
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="startIndex">开始截取位置</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取出的新字符串</returns>
        public static string CutString(string targetString, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }
                if (startIndex > targetString.Length)
                {
                    return string.Empty;
                }
            }
            else
            {
                if (length < 0)
                {
                    return string.Empty;
                }
                if (length + startIndex > 0)
                {
                    length = length + startIndex;
                    startIndex = 0;
                }
                else
                {
                    return string.Empty;
                }
            }
            if (targetString.Length - startIndex < length)
            {
                length = targetString.Length - startIndex;
            }
            return targetString.Substring(startIndex, length);
        }
        #endregion

        #region CutString 从字符串的指定位置开始截取到字符串尾部
        /// <summary>
        ///  从字符串的指定位置开始截取到字符串尾部
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="startIndex">开始截取位置</param>
        /// <returns>截取出的新字符串</returns>
        public static string CutString(string targetString, int startIndex)
        {
            return CutString(targetString, startIndex, targetString.Length);
        }
        #endregion

        #region CutString 从字符串的指定位置截取指定长度（字符串如果操过指定长度则将超出的部分用指定字符串代替）
        /// <summary>
        /// 截取指定长度的字符串（字符串如果操过指定长度则将超出的部分用指定字符串代替）
        /// </summary>
        /// <param name="targetString">要检查的字符串</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">指定长度</param>
        /// <param name="newString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(string targetString, int startIndex, int length, string newString)
        {
            if (targetString == null) return string.Empty;

            string resultString = targetString;
            if (length >= 0)
            {
                byte[] defaultBytes = Encoding.Default.GetBytes(targetString);
                //当字符串长度大于起始位置
                int endIndex = defaultBytes.Length;
                if (defaultBytes.Length > startIndex)
                {
                    //当要截取的长度在字符串的有效长度范围内
                    if (defaultBytes.Length > (startIndex + length))
                    {
                        endIndex = length + startIndex;
                    }
                    else
                    {
                        //当不在有效范围内时,只取到字符串的结尾
                        length = defaultBytes.Length - startIndex;
                        newString = string.Empty;
                    }
                    int realLength = length;
                    var resultFlag = new int[length];
                    int flag = 0;
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        if (defaultBytes[i] > 127)
                        {
                            flag++;
                            if (flag == 3)
                            {
                                flag = 1;
                            }
                        }
                        else
                        {
                            flag = 0;
                        }
                        resultFlag[i] = flag;
                    }
                    if ((defaultBytes[endIndex - 1] > 127) && (resultFlag[length - 1] == 1))
                    {
                        realLength = length + 1;
                    }
                    byte[] resultBytes = new byte[realLength];
                    Array.Copy(defaultBytes, startIndex, resultBytes, 0, realLength);
                    resultString = Encoding.Default.GetString(resultBytes);
                    resultString = resultString + newString;
                }
            }
            return resultString;
        }

        #endregion

        #region CutString 从字符串的指定位置开始截取到字符串尾部 （字符串如果操过指定长度则将超出的部分用指定字符串代替）
        /// <summary>
        /// 从字符串的指定位置开始截取到字符串尾部（字符串如果操过指定长度则将超出的部分用指定字符串代替）
        /// </summary>
        /// <param name="targetString">要检查的字符串</param>
        /// <param name="length">指定长度</param>
        /// <param name="newString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(string targetString, int length, string newString)
        {
            return CutString(targetString, 0, length, newString);
        }
        #endregion

        #region ToSimplifiedChinese 转换为简体中文
        /// <summary>
        ///  转换为简体中文
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns></returns>
        public static string ToSimplifiedChinese(string targetString)
        {
            return Strings.StrConv(targetString, VbStrConv.SimplifiedChinese, 0);
        }
        #endregion

        #region ToTraditionalChinese 转换为繁体中文
        /// <summary>
        ///  转换为繁体中文
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns></returns>
        public static string ToTraditionalChinese(string targetString)
        {
            return Strings.StrConv(targetString, VbStrConv.TraditionalChinese, 0);
        }
        #endregion

        #region Split 分割字符串
        /// <summary>
        ///  分割字符串
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="separator">分割符</param>
        /// <returns></returns>
        public static string[] Split(string targetString, string separator)
        {
            if (!string.IsNullOrEmpty(targetString))
            {
                if (targetString.IndexOf(separator) < 0)
                {
                    return new[] { targetString };
                }
                return Regex.Split(targetString, Regex.Escape(separator), RegexOptions.IgnoreCase);
            }
            return new string[0];
        }
        #endregion

        #region Split 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="separator">分割符</param>
        /// <param name="count">返回数组长度</param>
        /// <returns></returns>
        public static string[] Split(string targetString, string separator, int count)
        {
            string[] resultArray = new string[count];
            string[] splitedArray = Split(targetString, separator);
            for (int i = 0; i < count; i++)
            {
                if (i < splitedArray.Length)
                {
                    resultArray[i] = splitedArray[i];
                }
                else
                {
                    resultArray[i] = string.Empty;
                }
            }
            return resultArray;
        }
        #endregion

        #region Split 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="targetString">被分割的字符串</param>
        /// <param name="separator">分割符</param>
        /// <param name="ignoreRepeat">忽略重复项</param>
        /// <param name="maxItemLength">单个元素最大长度</param>
        /// <returns></returns>
        public static string[] Split(string targetString, string separator, bool ignoreRepeat, int maxItemLength)
        {
            string[] result = Split(targetString, separator);
            return ignoreRepeat ? DistinctArray(result, maxItemLength) : result;
        }
        #endregion

        #region Split 分割字符串
        /// <summary>
        ///  分割字符串
        /// </summary>
        /// <param name="targetString">被分割的字符串</param>
        /// <param name="separator">分割符</param>
        /// <param name="ignoreRepeat">忽略重复项</param>
        /// <param name="minItemLength">单个元素最小长度</param>
        /// <param name="maxItemLength">单个元素最大长度</param>
        /// <returns></returns>
        public static string[] Split(string targetString, string separator, bool ignoreRepeat, int minItemLength, int maxItemLength)
        {
            string[] resultArray = Split(targetString, separator);

            if (ignoreRepeat)
            {
                resultArray = DistinctArray(resultArray);
            }
            return PadArray(resultArray, minItemLength, maxItemLength);
        }
        #endregion

        #region Split 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="targetString">被分割的字符串</param>
        /// <param name="separator">分割符</param>
        /// <param name="ignoreRepeat">忽略重复项</param>
        /// <returns></returns>
        public static string[] Split(string targetString, string separator, bool ignoreRepeat)
        {
            return Split(targetString, separator, ignoreRepeat, 0);
        }
        #endregion

        #region Merge 合并字符串
        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <returns>合并到的目的字符串</returns>
        public static string Merge(string source, string target)
        {
            return Merge(source, target, string.Empty);
        }
        #endregion

        #region Merge 合并字符串
        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <param name="mergeChar">合并符</param>
        /// <returns>并到字符串</returns>
        public static string Merge(string source, string target, string mergeChar)
        {
            if (string.IsNullOrEmpty(target))
            {
                target = source;
            }
            else
            {
                target += mergeChar + source;
            }
            return target;
        }
        #endregion

        #region RemoveLastChar 删除最后一个字符
        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static string RemoveLastChar(string targetString)
        {
            return (targetString == string.Empty) ? string.Empty : targetString.Substring(0, targetString.Length - 1);
        }
        #endregion
 
        #region PadArray 过滤字符串数组中每个元素为合适的大小
        /// <summary>
        /// 过滤字符串数组中每个元素为合适的大小
        /// 当长度小于minLength时，删除掉，-1为不限制最小长度
        /// 当长度大于maxLength时，取其前maxLength位
        /// 如果数组中有null元素，会被忽略掉
        /// </summary>
        /// <param name="targetArray">目标数组</param>
        /// <param name="minLength">单个元素最小长度</param>
        /// <param name="maxLength">单个元素最大长度</param>
        /// <returns></returns>
        public static string[] PadArray(string[] targetArray, int minLength, int maxLength)
        {
            if (minLength > maxLength)
            {
                int length = maxLength;
                maxLength = minLength;
                minLength = length;
            }

            int miniStringCount = 0;

            for (int i = 0; i < targetArray.Length; i++)
            {
                if (minLength > -1 && targetArray[i].Length < minLength)
                {
                    targetArray[i] = null;
                    continue;
                }
                if (targetArray[i].Length > maxLength)
                {
                    targetArray[i] = targetArray[i].Substring(0, maxLength);
                }
                miniStringCount++;
            }

            string[] resultArray = new string[miniStringCount];

            for (int i = 0, j = 0; i < targetArray.Length && j < resultArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(targetArray[i]))
                {
                    resultArray[j] = targetArray[i];
                    j++;
                }
            }
            return resultArray;
        }
        #endregion

        #region DistinctArray 清除字符串数组中的重复项
        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="targetArray">字符串数组</param>
        /// <param name="maxItemLength">字符串数组中单个元素的最大长度</param>
        /// <returns></returns>
        public static string[] DistinctArray(string[] targetArray, int maxItemLength)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string @string in targetArray)
            {
                string key = @string;
                if (maxItemLength > 0 && key.Length > maxItemLength)
                {
                    key = key.Substring(0, maxItemLength);
                }
                dictionary[key.Trim()] = @string;
            }
            string[] resultArray = new string[dictionary.Count];
            dictionary.Keys.CopyTo(resultArray, 0);
            return resultArray;
        }
        #endregion

        #region DistinctArray 清除字符串数组中的重复项
        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="targetArray">字符串数组</param>
        /// <returns></returns>
        public static string[] DistinctArray(string[] targetArray)
        {
            return DistinctArray(targetArray, 0);
        }
        #endregion

        #region FilterIllegal 过滤非法字符（替换非法字符）
        /// <summary>
        /// 过滤非法字符（替换非法字符）
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="illegalString">非法字符集（oldValue=newValue\r\n格式）</param>
        /// <returns></returns>
        public static string FilterIllegal(string targetString, string illegalString)
        {
            string[] illegalArray = Split(illegalString, "\r\n");
            foreach (string illegal in illegalArray)
            {
                string oldValue = illegal.Substring(0, illegal.IndexOf("="));
                string newValue = illegal.Substring(illegal.IndexOf("=") + 1);
                targetString = targetString.Replace(oldValue, newValue);
            }
            return targetString;
        }
        #endregion

        #region SbcCaseToNumberic 将全角数字转换为数字
        /// <summary>
        /// 将全角数字转换为数字
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static string SbcCaseToNumberic(string targetString)
        {
            char[] @char = targetString.ToCharArray();
            for (int i = 0; i < @char.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(@char, i, 1);
                if (bytes.Length == 2)
                {
                    if (bytes[1] == 255)
                    {
                        bytes[0] = (byte)(bytes[0] + 32);
                        bytes[1] = 0;
                        @char[i] = Encoding.Unicode.GetChars(bytes)[0];
                    }
                }
            }
            return new string(@char);
        }
        #endregion
    }
}
