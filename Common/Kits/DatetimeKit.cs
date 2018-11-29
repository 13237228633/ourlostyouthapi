namespace Common
{
    using System;

    public static class DatetimeKit
    {
        private static readonly string[] ChineseMonths = new[] { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
        
        private static readonly string[] EnglishMonths = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        private static readonly string[] ChineseWeekDays = new[] { "星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

        private static readonly string[] EnglishWeekDays = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        #region GetChineseMonth 根据阿拉伯数字返回月份的中文名称
        /// <summary>
        ///  根据阿拉伯数字返回月份的中文名称
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetChineseMonth(int month)
        {
            if (month < 0 || month >11)
            {
                return string.Empty;
            }
            return ChineseMonths[month];  
        }
        #endregion

        #region GetEnglishMonth 根据阿拉伯数字返回月份的英文名称
        /// <summary>
        ///  根据阿拉伯数字返回月份的英文名称
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetEnglishMonth(int month)
        {
            if (month < 0 || month > 11)
            {
                return string.Empty;
            }
            return EnglishMonths[month];
        }
        #endregion

        #region GetChineseWeekDay 根据阿拉伯数字返回中文WeekDay
        /// <summary>
        ///  根据阿拉伯数字返回中文WeekDay
        /// </summary>
        /// <param name="weekDay"></param>
        /// <returns></returns>
        public static string GetChineseWeekDay(int weekDay)
        {
            if (weekDay < 0 || weekDay > 6)
            {
                return string.Empty;
            }
            return ChineseWeekDays[weekDay];
        }
        #endregion

        #region GetEnglishWeekDay 根据阿拉伯数字返回英文WeekDay
        /// <summary>
        ///  根据阿拉伯数字返回英文WeekDay
        /// </summary>
        /// <param name="weekDay"></param>
        /// <returns></returns>
        public static string GetEnglishWeekDay(int weekDay)
        {
            if (weekDay < 0 || weekDay > 6)
            {
                return string.Empty;
            }
            return EnglishWeekDays[weekDay];
        }
        #endregion

        #region ToStandardDate 返回标准日期格式字符串
        /// <summary>
        ///  返回标准日期格式字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToStandardDate(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString("yyyy-MM-dd");
            }
            return string.Empty;
        }
        #endregion

        #region ToStandardDate 返回标准日期格式字符串
        /// <summary>
        ///  返回指定日期格式
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="defaultDate">目标时间</param>
        /// <returns></returns>
        public static string ToStandardDate(object target, string defaultDate)
        {
            string resultString;
            if (target.Equals(null) || target.ToString().Equals(string.Empty))
            {
                return defaultDate;
            }
            try
            {
                resultString = Convert.ToDateTime(target).ToString("yyyy-MM-dd").Replace("1900-01-01", defaultDate);
            }
            catch
            {
                return defaultDate;
            }
            return resultString;
        }
        #endregion

        #region ToStandardTime 返回标准时间格式字符串
        /// <summary>
        ///  返回标准时间格式字符串
        /// </summary>
        /// <param name="dateTime">目标时间</param>
        /// <returns></returns>
        public static string ToStandardTime(DateTime dateTime)
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
        #endregion

        #region ToStandardTime 返回标准时间格式字符串
        /// <summary>
        ///  返回标准时间格式字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public static string ToStandardTime(object target, string defaultTime)
        {
            string resultString;
            if (target.Equals(null) || target.ToString().Equals(string.Empty))
            {
                return defaultTime;
            }
            try
            {
                resultString = Convert.ToDateTime(target).ToString("HH:mm:ss").Replace("00:00:00", defaultTime);
            }
            catch
            {
                return defaultTime;
            }
            return resultString;
        }
        #endregion

        #region ToStandardDateTime 返回标准日期时间格式字符串
        /// <summary>
        ///  返回标准日期时间格式字符串
        /// </summary>
        /// <param name="dateTime">目标时间</param>
        /// <returns></returns>
        public static string ToStandardDateTime(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return string.Empty;
        }
        #endregion

        #region ToStandardDatetime 返回标准日期时间格式字符串
        /// <summary>
        ///  返回标准日期时间格式字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static string ToStandardDatetime(object target, string defaultString)
        {
            string resultString;
            if (target.Equals(null) || target.ToString().Equals(string.Empty))
            {
                return defaultString;
            }
            try
            {
                resultString = Convert.ToDateTime(target).ToString("yyyy-MM-dd HH:mm").Replace("1900-01-01 00:00:00", defaultString);
            }
            catch
            {
                return defaultString;
            }
            return resultString;
        }

        #endregion

        #region ToFullDatetime 返回完整的日期时间格式字符串
        /// <summary>
        ///  返回完整的日期时间格式字符串
        /// </summary>
        /// <param name="dateTime">目标时间</param>
        /// <returns></returns>
        public static string ToFullDatetime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }
        #endregion

        #region ConvertToUnixTimeStamp 转换时间为Unix时间戳
        /// <summary>
        /// 转换时间为Unix时间戳
        /// </summary>
        /// <param name="dateTime">需要传递UTC时间,避免时区误差,例:DataTime.UTCNow</param>
        /// <returns></returns>
        public static double ConvertToUnixTimeStamp(DateTime dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan timeSpan = dateTime - origin;
            return Math.Floor(timeSpan.TotalSeconds);
        }
        #endregion

        public static string GetDateString()
        {
            string str_result = "今天是" + DateTime.Now.ToShortDateString() + "," + CnWeekDayName(DateTime.Now);
            return str_result;
        }

        public static string CnWeekDayName(DateTime dt)
        {
            string[] cnWeekDayNames = { "日", "一", "二", "三", "四", "五", "六" };

            return "星期" + cnWeekDayNames[(int)dt.DayOfWeek];
        }
        /// <summary>
        /// 本周起始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetWeekBegin(DateTime dt)
        {
            int spanDays = 0;//相隔天数

            switch (dt.DayOfWeek)
            {

                case DayOfWeek.Monday: spanDays = 0;

                    break;

                case DayOfWeek.Tuesday: spanDays = 1;

                    break;

                case DayOfWeek.Wednesday: spanDays = 2;

                    break;

                case DayOfWeek.Thursday: spanDays = 3;

                    break;

                case DayOfWeek.Friday: spanDays = 4;

                    break;

                case DayOfWeek.Saturday: spanDays = 5;

                    break;

                case DayOfWeek.Sunday: spanDays = 6;

                    break;

            }

            return dt.AddDays(-spanDays);//本周起始时间

        }
        /// <summary>
        /// 前7天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetSevenBegin(DateTime dt)
        {
            return dt.AddDays(-7);
        }

        /// <summary>
        /// 本月起始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static  DateTime GetMonthBegin(DateTime dt)
        {
            return dt.AddDays(1 - dt.Day);
        }
        /// <summary>
        /// 本年起始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetYearBegin(DateTime dt)
        {
            return dt.AddDays(1 - dt.DayOfYear);
        }

        /// <summary>
        /// 获取是星期几
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DayOfWeek GetDateWeek(DateTime dt)
        {
            return dt.DayOfWeek;
        }

        /// <summary>
        /// 获取两个时间间隔多少天
        /// </summary>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public static  int GetDateDiffDay(DateTime  btime, DateTime etime)
        {
            int i = 0;
            for (int j = 0; btime.Date < etime.Date; j++)
            { 
                
               btime= btime.AddDays(1);
                i++;
            }
            return i;
        }

    }
}
