using System.Data.SqlClient;

///// --Msdn  Comparison泛型委托 -- public delegate int Comparison<T>(T x, T y);

namespace XmlDateLibrary.Common
{

    public abstract class XmlChildListRepository<TEntity>
    where TEntity : XmlChildListBase, new()
    {
        /// <summary>
        /// XML对象中的子类List对象操作方法-排序用
        /// </summary>
        protected XmlChildListRepository()
        {
        }
        /// <summary>
        /// 排序字段的数据类型枚举
        /// </summary> 
        public enum SortDataTypeEnum
        {
            默认字段排序 = 0,
            字符串 = 1,
            整型 = 2,
            布尔值 = 3
        }
        /// <summary>
        /// 供外部修正的临时变量,比较的数据类型
        /// </summary>
        public SortDataTypeEnum SortDataType = SortDataTypeEnum.字符串;
        /// <summary>
        /// 受保护成员的外部修改方法，预留
        /// </summary>
        /// <param name="sortOrder"></param>
        public static void ChangeSortOrder(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Descending:
                    SortOrderModifier = -1;
                    break;
                case SortOrder.Ascending:
                    SortOrderModifier = 1;
                    break;
            }
        }
        #region List排序
        /// <summary>
        /// 用于在派生类中访问控制（方法重写时） 正序倒序
        /// </summary>
        protected static int SortOrderModifier = 1;

        /// <summary>
        /// 倒序 （Comparison 委托）
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <returns></returns>
        public virtual int ComparisonDescending(TEntity xx, TEntity yy)
        {
            ChangeSortOrder(SortOrder.Descending);
            switch (SortDataType)
            {
                case SortDataTypeEnum.字符串:
                    return ComparisonChildString(xx, yy);
                case SortDataTypeEnum.整型:
                    return ComparisonChildInt(xx, yy);
                default:
                    return ComparisonChildDefault(xx, yy);
            }
        }
        /// <summary>
        /// 升序 （Comparison 委托）
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <returns></returns>
        public virtual int ComparisonAscending(TEntity xx, TEntity yy)
        {
            ChangeSortOrder(SortOrder.Ascending);
            switch (SortDataType)
            {
                case SortDataTypeEnum.字符串:
                    return ComparisonChildString(xx, yy);
                case SortDataTypeEnum.整型:
                    return ComparisonChildInt(xx, yy);
                default:
                    return ComparisonChildDefault(xx, yy);
            }
        }
        //------------------------------分割线-------------------------------------------------
        /// <summary>
        /// 需要在子类中自行重写，字符串排序
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <returns></returns>
        protected virtual int ComparisonChildString(TEntity xx, TEntity yy)
        {
            return 0;
        }
        /// <summary>
        /// 需要在子类中自行重写，整型排序
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <returns></returns>
        protected virtual int ComparisonChildInt(TEntity xx, TEntity yy)
        {
            return 0;
        }
        /// <summary>
        /// 使用默认排序字段
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <returns></returns>
        internal static int ComparisonChildDefault(TEntity xx, TEntity yy)
        {
            int x = xx.Sort;
            int y = yy.Sort;
            int backdata = x.CompareTo(y);
            return backdata * SortOrderModifier;
        }
        #endregion
        #region 默认的比较器方法
        /// <summary>
        /// 默认的字符串排序方法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected int ComparisonChildString(string x, string y)
        {
            int backdata;
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    backdata = 0;
                    return backdata * SortOrderModifier;
                }
                // If x is null and y is not null, y
                // is greater.  
                backdata = -1;
                return backdata * SortOrderModifier;
            }
            // If x is not null... 
            if (y == null)
            // ...and y is null, x is greater.
            {
                backdata = 1;
                return backdata * SortOrderModifier;
            }
            // ...and y is not null, compare the 
            // lengths of the two strings.
            //
            int retval = x.Length.CompareTo(y.Length);
            if (retval != 0)
            {
                // If the strings are not of equal length,
                // the longer string is greater. 
                backdata = retval;
                return backdata * SortOrderModifier;
            }
            // If the strings are of equal length,
            // sort them with ordinary string comparison.
            backdata = x.CompareTo(y);
            return backdata * SortOrderModifier;
        }
        /// <summary>
        /// 默认的整型排序方法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected int ComparisonChildInt(int x, int y)
        {
            int backdata = x.CompareTo(y);
            return backdata * SortOrderModifier;
        }
        #endregion
    }
}
#region 自定义比较器
//private class RowComparison
//{
//    private static int _sortOrderModifier = 1;
//    public RowComparison(SortOrder sortOrder)
//    {
//        switch (sortOrder)
//        {
//            case SortOrder.Descending:
//                _sortOrderModifier = -1;
//                break;
//            case SortOrder.Ascending:
//                _sortOrderModifier = 1;
//                break;
//        }
//    }
//    //public delegate int Comparison<T>(T x, T y);
//    public static int Comparison(MyDictionary xx, MyDictionary yy)
//    {
//        _sortOrderModifier = -1;
//        return ComparisonChild(xx, yy);
//    }
//    public static int Comparison2(MyDictionary xx, MyDictionary yy)
//    {
//        _sortOrderModifier = 1;
//        return ComparisonChild(xx, yy);
//    }
//    private static int ComparisonChild(MyDictionary xx, MyDictionary yy)
//    {
//        string x = xx.DataTypeText;// xx.DataValue.ToString();
//        string y = yy.DataTypeText;//yy.DataValue.ToString();
//        int backdata;
//        if (x == null)
//        {
//            if (y == null)
//            {
//                // If x is null and y is null, they're
//                // equal. 
//                backdata = 0;
//                return backdata * _sortOrderModifier;
//            }
//            else
//            {
//                // If x is null and y is not null, y
//                // is greater.  
//                backdata = -1;
//                return backdata * _sortOrderModifier;
//            }
//        }
//        else
//        {
//            // If x is not null... 
//            if (y == null)
//            // ...and y is null, x is greater.
//            {
//                backdata = 1;
//                return backdata * _sortOrderModifier;
//            }
//            else
//            {
//                // ...and y is not null, compare the 
//                // lengths of the two strings.
//                //
//                int retval = x.Length.CompareTo(y.Length);
//                if (retval != 0)
//                {
//                    // If the strings are not of equal length,
//                    // the longer string is greater. 
//                    backdata = retval;
//                    return backdata * _sortOrderModifier;
//                }
//                else
//                {
//                    // If the strings are of equal length,
//                    // sort them with ordinary string comparison.
//                    backdata = x.CompareTo(y);
//                    return backdata * _sortOrderModifier;
//                }
//            }
//        }
//    }
//}
#endregion 