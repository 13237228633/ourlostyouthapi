using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
    public static class DALMyDiary
    {
        public static bool MyDiaryAdd(MyDiary MyDiary)
        {

            SqlParameter[] sqlPare = new SqlParameter[]
               { 
                   new SqlParameter("@DiaryContent",MyDiary.DiaryContent),
                   new SqlParameter("@DiaryClassifyId",MyDiary.DiaryClassifyId),
                   new SqlParameter("@DiaryOvertState",MyDiary.DiaryOvertState),
                   new SqlParameter("@CreateDate",MyDiary.CreateDate),
                   new SqlParameter("@UserId",MyDiary.UserId)
               };
            return DBHelper.ExecuteCommand("[dbo].[pro_MyDiaryAdd]", sqlPare) > 0;
        }

        public static bool MyDiaryAddA(MyDiary MyDiary)
        {

            SqlParameter[] sqlPare = new SqlParameter[]
               {
                   new SqlParameter("@DiaryContent",MyDiary.DiaryContent),
                   new SqlParameter("@DiaryClassifyId",MyDiary.DiaryClassifyId),
                   new SqlParameter("@DiaryOvertState",MyDiary.DiaryOvertState),
                   new SqlParameter("@CreateDate",MyDiary.CreateDate),
                   new SqlParameter("@UserId",MyDiary.UserId)
               };
            return DBHelper.ExecuteCommand("[dbo].[pro_MyDiaryAddA]", sqlPare) > 0;
        }

        //public static DataTable selectUserInfo(int UserID)
        //{
        //    SqlParameter[] sqlPare = new SqlParameter[]
        //       { 
        //           new SqlParameter("@UserId",UserID)
        //       };
        //    return DBHelper.GetDataSet("[dbo].[pro_UserInfoList]", sqlPare);
        //}

        public static DataTable getMyDiaryList(MyDiary mydiary, int SelectType)
        {
            SqlParameter[] sqlPare = new SqlParameter[]
                   { 
                       new SqlParameter("@UserId",mydiary.UserId),
                       new SqlParameter("@DiaryClassifyId",mydiary.DiaryClassifyId),
                       new SqlParameter("@DiaryOvertState",mydiary.DiaryOvertState),
                       new SqlParameter("@SelectType",SelectType),
                       new SqlParameter("@RecordCount",SqlDbType.Int)
                   };
            sqlPare[4].Direction = ParameterDirection.Output;
            return DBHelper.GetDataSet("[dbo].[pro_MyDiaryList]", sqlPare);
        }


        public static bool MyDiaryDelete(MyDiary mydiary,out int Result)
        {
            SqlParameter[] sqlPare = new SqlParameter[]
               { 
                   new SqlParameter("@DiaryId",mydiary.DiaryId),
                   new SqlParameter("@UserId",mydiary.UserId),
                   new SqlParameter("@Result",SqlDbType.Int)
               };
            sqlPare[2].Direction = ParameterDirection.Output;
            int count = DBHelper.ExecuteCommand("[dbo].[pro_MyDiaryDelete]", sqlPare);
            if(!(count>0))
            {
                Result=Convert.ToInt32( sqlPare[2].Value.ToString());
            }else
            {
                Result=-1;
            }

            return count > 0;
        }
    }
}
