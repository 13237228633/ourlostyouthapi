using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class MyDiary
    {
        /// <summary>
        /// 日记内容
        /// </summary>
        public string DiaryContent { get; set; }

        /// <summary>
        /// 日记类别ID
        /// </summary>
        public int DiaryClassifyId { get; set; }
        /// <summary>
        /// 日记公开状态 0:私有，1公开
        /// </summary>
        public int DiaryOvertState { get; set; }
        /// <summary>
        ///  日记创建时间
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 日记作者用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 日记是否被删除，0未删除，1已删除
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// 日记主键ID
        /// </summary>
        public int DiaryId { get; set; }
    }
}
