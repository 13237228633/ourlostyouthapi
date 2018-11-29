namespace Common
{
    using System;
    using System.Web;
    using System.Reflection;
    using System.Collections.Generic;
    
    public static class EnumerateKit
    {
        #region ToDescriptionDictionary
        /// <summary>
        ///  
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDescriptionDictionary(Type enumType)
        {
            string cacheKey = "CACHE_ENUM_KEYVALUE_" + enumType.FullName.ToUpper();

            var dictionary = HttpContext.Current.Cache[cacheKey] as Dictionary<string, string>;

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, string>();
                object enumInstance = enumType.Assembly.CreateInstance(enumType.FullName);
                FieldInfo[] fieldInfos = enumType.GetFields();
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (descriptionAttributes.Length > 0)
                    {
                        var key = ((int) fieldInfo.GetValue(enumInstance)).ToString();
                        var value = descriptionAttributes[0].Description;

                        if (!dictionary.ContainsKey(key))
                        {
                            dictionary.Add(key, value);
                        }
                    }
                }
                HttpContext.Current.Cache.Insert(cacheKey, dictionary);
            }
            var result = new OptimizedDictionary<string, string>();
            foreach (var kvp in dictionary)
            {
                result.Add(kvp.Key, kvp.Value);
            }
            return result;
        }
        #endregion

        #region ToDescriptionDictionary

        public static IDictionary<TKey, string> ToDescriptionDictionary<TKey>(Type enumType)
        {
            string cacheKey = "CACHE_ENUM_KEYVALUE_" + enumType.FullName.ToUpper() + "_" + typeof(TKey).ToString().ToUpper();

            var dictionary = HttpContext.Current.Cache[cacheKey] as Dictionary<TKey, string>;
            if (dictionary == null)
            {
                dictionary = new Dictionary<TKey, string>();
                object enumInstance = enumType.Assembly.CreateInstance(enumType.FullName);
                FieldInfo[] fieldInfos = enumType.GetFields();

                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    
                    if (descriptionAttributes.Length > 0)
                    {
                        TKey key = ConvertKit.Convert(fieldInfo.GetValue(enumInstance), default(TKey));
                        string value = descriptionAttributes[0].Description;

                        if (!dictionary.ContainsKey(key))
                        {
                            dictionary.Add(key, value);
                        }
                    }
                }
                HttpContext.Current.Cache.Insert(cacheKey, dictionary);
            }
            var result = new OptimizedDictionary<TKey, string>();
            foreach (var kvp in dictionary)
            {
                result.Add(kvp.Key, kvp.Value);
            }
            return result;
        }
        #endregion

        public static IDictionary<string, string> ToNDescriptionDictionary(Type enumType)
        {
       
            var dictionary = new Dictionary<string, string>();
            object enumInstance = enumType.Assembly.CreateInstance(enumType.FullName);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                var descriptionAttributes =
                    (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), true);
                if (descriptionAttributes.Length > 0)
                {
                    var key = ((int) fieldInfo.GetValue(enumInstance)).ToString();
                    var value = descriptionAttributes[0].Description;

                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, value);
                    }
                }
            }
            var result = new OptimizedDictionary<string, string>();
            foreach (var kvp in dictionary)
            {
                result.Add(kvp.Key, kvp.Value);
            }
            return result;
        }


    }
}
