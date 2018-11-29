namespace Common
{
    public class PageInfo
    {
        #region Property

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 构造方法
        /// </summary>
        public PageInfo()
        {
            this.PageSize = 10;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="size">每页记录数</param>
        public PageInfo(int size)
        {
            this.PageSize = size;
        }

        #endregion
        
    }
}
