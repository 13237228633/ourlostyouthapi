using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALUser
    {
        public DataTable UserLogin(string userphone, string password)
        {
            SqlParameter[] sqlpare = {
                new SqlParameter("@userphone",userphone),
                new SqlParameter("@password",password)
            };
           return DBHelper.GetDataSet("[dbo].[pro_UserLogin]", sqlpare);
        }

        public bool innerUserloginlog(Userloginlog tg_loginlog)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteQqLogin(User user)
        {
            SqlParameter[] sqlpare = {
                new SqlParameter("@UserName",user.UserName),
                new SqlParameter("@UserHeadPortrait",user.UserHeadPortrait),
                new SqlParameter("@UserProvince",user.UserProvince),
                new SqlParameter("@UserCity",user.UserCity),
                new SqlParameter("@UserYear",user.UserYear),
                new SqlParameter("@userOpenid",user.userOpenid)
            };
            return DBHelper.GetDataSet("[dbo].[pro_QqUserLogin]", sqlpare);
        }
    }
}
