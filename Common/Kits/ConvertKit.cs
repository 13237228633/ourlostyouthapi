namespace Common
{
    using System.ComponentModel;
    using System;
    
    /// <summary>
    ///  数据类型工具
    /// </summary>
    public static class ConvertKit
    {
        #region Convert 类型转换
        /// <summary>
        ///  类型转换
        /// </summary>
        /// <typeparam name="TValue">目标类型</typeparam>
        /// <param name="value">被转换的值</param>
        /// <param name="defaultValue">转换失败时返回默认值</param>
        /// <returns></returns>
        public static TValue Convert<TValue>(object value, TValue defaultValue)
        {
            TValue newValue;
            try
            {
                if (value != null && value is TValue)
                {
                    newValue = (TValue)value;
                }
                else
                {
                    value = System.Convert.ChangeType(value, typeof(TValue));
                    newValue = (TValue)value;
                    if (newValue.Equals(null))
                    {
                        return defaultValue;
                    }
                }
            }
            catch (Exception)
            {
                newValue = defaultValue;
            }
            return newValue;
        }

        #endregion

        #region Convert Convert 类型转换(Nullable)
        /// <summary>
        ///  类型转换(Nullable)
        /// </summary>
        /// <typeparam name="TValue">目标类型</typeparam>
        /// <param name="value">被转换的值</param>
        /// <param name="defaultValue">转换失败时返回默认值</param>
        /// <returns></returns>
        public static TValue? Convert<TValue>(object value, TValue? defaultValue) where TValue : struct
        {
            TValue? newValue;
            try
            {
                if (value != null && value is TValue)
                {
                    newValue = (TValue)value;
                }
                else
                {
                    value = System.Convert.ChangeType(value, typeof(TValue));
                    newValue = (TValue)value;
                }
            }
            catch (Exception)
            {
                newValue = defaultValue;
            }
            return newValue;
        }
        #endregion
        

        #region LongToInt32 将long型数值转换为Int32类型
        /// <summary>
        /// 将long型数值转换为Int32类型
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int SafeInt32(object target)
        {
            if (target == null)
            {
                return 0;
            }
            string numString = target.ToString();
            if (ValidateKit.IsNumeric(numString))
            {
                if (numString.Length > 9)
                {
                    if (numString.StartsWith("-"))
                    {
                        return int.MinValue;
                    }
                    return int.MaxValue;
                }
                return Int32.Parse(numString);
            }
            return 0;
        }
        #endregion

        #region ChangeType 装箱类型转换 兼容Nullable类型
        /// <summary>
        ///  装箱类型转换 兼容Nullable类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            }
            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                var nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return System.Convert.ChangeType(value, conversionType);
        }
        #endregion

        #region ToInt16
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int16 ToInt16(object value, Int16 defaultValue)
        {
            if (value != null)
            {
                Int16 temp;
                if (Int16.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 ToInt16(object value)
        {
            return ToInt16(value, 0);
        }
        #endregion

        #region ToInt32
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int32 ToInt32(object value, Int32 defaultValue)
        {
            if (value != null)
            {
                Int32 temp;
                if (Int32.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int32? ToInt32(object value, Int32? defaultValue)
        {
            if (value != null)
            {
                Int32 temp;
                if (Int32.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 ToInt32(object value)
        {
            try
            {
                return ToInt32(value, 0);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ToInt64
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int64 ToInt64(object value, Int64 defaultValue)
        {
            if (value != null)
            {
                Int64 temp;
                if (Int64.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 ToInt64(object value)
        {
            return ToInt64(value, 0);
        }
        #endregion

        #region ToUInt16
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(object value, UInt16 defaultValue)
        {
            if (value != null)
            {
                UInt16 temp;
                if (UInt16.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(object value)
        {
            return ToUInt16(value, default(UInt16));
        }
        #endregion

        #region ToUInt32
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(object value, UInt32 defaultValue)
        {
            if (value != null)
            {
                UInt32 temp;
                if (UInt32.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(object value)
        {
            return ToUInt32(value, default(UInt32));
        }
        #endregion

        #region ToUInt64
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(object value, UInt64 defaultValue)
        {
            if (value != null)
            {
                UInt64 temp;
                if (UInt64.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(object value)
        {
            return ToUInt64(value, default(UInt64));
        }
        #endregion

        #region ToFloat
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(object value, float defaultValue)
        {
            if (value != null)
            {
                float temp;
                if (float.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(object value)
        {
            return ToFloat(value, default(float));
        }
        #endregion

        #region ToDecimal
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(object value, Decimal defaultValue)
        {
            if (value != null)
            {
                Decimal temp;
                if (Decimal.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(object value)
        {
            return ToDecimal(value, default(Decimal));
        }
        #endregion

        #region ToDouble
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Double ToDouble(object value, Double defaultValue)
        {
            if (value != null)
            {
                Double temp;
                if (Double.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double ToDouble(object value)
        {
            return ToDouble(value, default(Double));
        }
        #endregion

        #region ToLong
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(object value, long defaultValue)
        {
            if (value != null)
            {
                long temp;
                if (long.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object value)
        {
            return ToLong(value, default(long));
        }
        #endregion

        #region ToULong
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ulong ToULong(object value, ulong defaultValue)
        {
            if (value != null)
            {
                ulong temp;
                if (ulong.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ulong ToULong(object value)
        {
            return ToULong(value, default(ulong));
        }
        #endregion

        #region ToShort
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short ToShort(object value, short defaultValue)
        {
            if (value != null)
            {
                short temp;
                if (short.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToShort(object value)
        {
            return ToShort(value, default(short));
        }
        #endregion

        #region ToUShort
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ushort ToUShort(object value, ushort defaultValue)
        {
            if (value != null)
            {
                ushort temp;
                if (ushort.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort ToUShort(object value)
        {
            return ToUShort(value, default(ushort));
        }
        #endregion

        #region ToByte
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Byte ToByte(object value, Byte defaultValue)
        {
            if (value != null)
            {
                Byte temp;
                if (Byte.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        public static Byte? ToByte(object value, Byte? defaultValue)
        {
            if (value != null)
            {
                Byte temp;
                if (Byte.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Byte ToByte(object value)
        {
            return ToByte(value, default(Byte));
        }

        #endregion

        #region ToSByte
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static SByte ToSByte(object value, SByte defaultValue)
        {
            if (value != null)
            {
                SByte temp;
                if (SByte.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SByte ToSByte(object value)
        {
            return ToSByte(value, default(SByte));
        }
        #endregion

        #region ToSingle
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Single ToSingle(object value, Single defaultValue)
        {
            if (value != null)
            {
                Single temp;
                if (Single.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Single ToSingle(object value)
        {
            return ToSingle(value, default(Single));
        }
        #endregion

        #region ToDateTime
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value, DateTime defaultValue)
        {
            if (value != null)
            {
                DateTime temp;
                if (DateTime.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        public static DateTime? ToDateTime(object value, DateTime? defaultValue)
        {
            if (value != null)
            {
                DateTime temp;
                if (DateTime.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value)
        {
            return ToDateTime(value, default(DateTime));
        }
        #endregion

        #region ToChar
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Char ToChar(object value, Char defaultValue)
        {
            if (value != null)
            {
                Char temp;
                if (Char.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Char ToChar(object value)
        {
            return ToChar(value, default(Char));
        }
        #endregion

        #region ToBoolean
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Boolean ToBoolean(object value, Boolean defaultValue)
        {
            if (value != null)
            {
                Boolean temp;
                if (Boolean.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        public static Boolean? ToBoolean(object value, Boolean? defaultValue)
        {
            if (value != null)
            {
                Boolean temp;
                if (Boolean.TryParse(value.ToString(), out temp))
                {
                    return temp;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean ToBoolean(object value)
        {
            return ToBoolean(value, default(Boolean));
        }
        #endregion

        /// <summary>
        ///  无小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToBaifen(object value)
        {
            return (ConvertKit.ToDecimal(value)*100).ToString("f0") + "%";
        }
        /// <summary>
        /// 保留两位小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToBaifen2x(object value)
        {
            return (ConvertKit.ToDecimal(value)*100).ToString("f2") + "%";
        }

        public static String ToBaifen(object value,object value2)
        {
            return ((ConvertKit.ToDecimal(value)/ConvertKit.ToDecimal(value2)) * 100).ToString("f2") + "%";
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToBaifen2(object value)
        {
            if (ConvertKit.ToDecimal(value) > 0)
            {
            return (ConvertKit.ToDecimal(value) * 100).ToString("f0") + "%";
            }
            else
            {
                return "-";
            }
        }
        public static string TodecimalInt(object value)
        {
            return ConvertKit.ToDecimal(value).ToString("f0");
        }

        /// <summary>
        /// 根据当日充值B-ROI计算评级
        /// </summary>
        /// <param name="payamountb">充值B</param>
        /// <param name="dtake">当日投入金额</param>
        /// <returns></returns>
        public static string GetSubLevel(decimal payamountb, decimal dtake)
        {
            if (dtake <= 0)
            {
                return "";
            }

            decimal levelValue =  (payamountb/dtake) * ConvertKit.ToDecimal(100);

            if (levelValue < 33)
            {
                return "C";
            }
            else if (levelValue <= 65)
            {
                return "B";
            }
            else if (levelValue <= 99)
            {
                return "A";
            }
            else
            {
                return "S";
            }

        }

        /// <summary>
        /// ROI=充值/投入
        /// </summary>
        /// <param name="payamount">充值</param>
        /// <param name="dtake">投入</param>
        /// <returns></returns>
        public static string GetRoI(decimal payamount, decimal dtake)
        {
            return dtake <= 0 ? "" : ConvertKit.ToBaifen2x(payamount / dtake).ToString();
        } 
    }
}
