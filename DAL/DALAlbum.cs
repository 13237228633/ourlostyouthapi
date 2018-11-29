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
    public static class DALAlbum
    {
        public static bool upPhoto(Album listAlbum)
        {

            SqlParameter[] sqlPare = new SqlParameter[]
           {
                   new SqlParameter("@AlnumUrl",listAlbum.AlnumUrl),
                   new SqlParameter("@AlnumName",listAlbum.AlnumName),
                   new SqlParameter("@AlnumRemarks",listAlbum.AlnumRemarks),
                   new SqlParameter("@UserId",listAlbum.UserId)
           };
            return DBHelper.ExecuteCommand("[dbo].[pro_MyAlbumAdd]", sqlPare) > 0;
        }

        public static bool upUserHeadPortrait(Album listAlbum)
        {

            SqlParameter[] sqlPare = new SqlParameter[]
               {
                   new SqlParameter("@UserHeadPortrait",listAlbum.AlnumUrl),
                   new SqlParameter("@UserId",listAlbum.UserId)
               };
            return DBHelper.ExecuteCommand("[dbo].[pro_UserInfoupdate]", sqlPare) > 0;
        }

        public static DataTable selectAlbum(int UserID)
        {
            SqlParameter[] sqlPare = new SqlParameter[]
               {
                   new SqlParameter("@UserId",UserID)
               };
            return DBHelper.GetDataSet("[dbo].[pro_MyAlbumList]", sqlPare);
        }

        public static DataTable selectUserInfo(int UserID)
        {
            SqlParameter[] sqlPare = new SqlParameter[]
               {
                   new SqlParameter("@UserId",UserID)
               };
            return DBHelper.GetDataSet("[dbo].[pro_UserInfoList]", sqlPare);
        }
    }
}
