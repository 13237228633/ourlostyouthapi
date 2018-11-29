using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class DALMyVideo
    {
        public static DataTable selectMyVideo(int UserID,int SelectType,int ClassifyId,PageInfo pageinfo)
        {
            SqlParameter[] sqlPare = new SqlParameter[]
              {
                   new SqlParameter("@UserId",UserID),
                   new SqlParameter("@SelectType",SelectType),
                   new SqlParameter("@ClassifyId",ClassifyId),
                   new SqlParameter("@PageSize",pageinfo.PageSize),
                   new SqlParameter("@CurrentIndex",pageinfo.CurrentIndex),
                   new SqlParameter("@RecordCount",SqlDbType.Int)
              };
            sqlPare[5].Direction = ParameterDirection.Output;
            DataTable dtMyVideo = DBHelper.GetDataSet("[dbo].[pro_MyVideoList]", sqlPare);
            pageinfo.RecordCount = string.IsNullOrEmpty(sqlPare[5].Value.ToString()) ? 0 : Convert.ToInt32(sqlPare[5].Value.ToString());
            return dtMyVideo;
        }
    }
}
