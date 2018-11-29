using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;

namespace DAL //可以修改成实际项目的命名空间名称
{
	/**//// <summary>
	/// Copyright (C) 2004-2008 LiTianPing 
	/// 数据访问基础类(基于SQLServer)
	/// 用户可以修改满足自己项目的需要。
	/// </summary>
    public static class DBHelper 
	{
        private static SqlConnection connection;
        public static SqlConnection Connection
        {
            get
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                if (connection == null)
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }
        /// <summary>
        /// 通过SQL语句和条件增删改一条数据
        /// </summary>
        /// <param name="safeSql">存储过程名</param>
        /// <returns>数据库信息数量</returns>
        public static int ExecuteCommand(string safeSql)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        /// <summary>
        /// 通过SQL语句增删改至少一条数据库信息
        /// </summary>
        /// <param name="safeSql">存储过程名</param>
        /// <param name="values">存储过程参数值</param>
        /// <returns>数据库信息数量</returns>
        public static int ExecuteCommand(string safeSql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(values);
            int count= cmd.ExecuteNonQuery();
            Connection.Close();
            return count;
        }

        /// <summary>
        /// 通过SQL语句查询一条数据库信息
        /// </summary>
        /// <param name="safeSql">存储过程名</param>
        /// <returns>数据库信息数量</returns>
        public static int GetScalar(string safeSql)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }
        public static int GetScalarBySql(string safeSql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            cmd.Parameters.AddRange(values);
            cmd.CommandType = CommandType.StoredProcedure;
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }
        /// <summary>
        /// 通过SQL语句和条件查询一条数据库信息
        /// </summary>
        /// <param name="safeSql">存储过程名</param>
        /// <param name="values">存储过程参数值</param>
        /// <returns>数据库信息数量</returns>
        public static int GetScalar(string safeSql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(values);
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }

        /// <summary>
        /// 通过SQL语句查询一条数据库信息
        /// </summary>
        /// <param name="safeSql">存储过程名</param>
        /// <returns>Reader对象</returns>
        public static SqlDataReader GetReader(string safeSql)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        /// <summary>
        /// 通过SQL语句和条件查询一条数据库信息
        /// </summary>
        /// <param name="sql">存储过程名</param>
        /// <param name="values">存储过程参数值</param>
        /// <returns>Reader对象</returns>
        public static SqlDataReader GetReader(string sql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(values);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        /// <summary>
        /// 通过SQL语句查询多条数据库信息
        /// </summary>
        /// <param name="safeSql">存储过程名</param>
        /// <returns>DataTable对象</returns>
        public static DataTable GetDataSet(string safeSql)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds.Tables[0];
        }
        /// <summary>
        /// 通过SQL语句和条件查询多条数据库信息
        /// </summary>
        /// <param name="sql">存储过程名</param>
        /// <param name="values">存储过程参数值</param>
        /// <returns>DataTable对象</returns>
        public static DataTable GetDataSet(string sql, params SqlParameter[] values)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(values);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds.Tables[0];
        }
        ///<summary>
        /// AspNetPager分页
        ///</summary>
        ///<param name="conn">已存在连接对象</param>
        ///<param name="sqlStr">执行查询语句</param>
        ///<param name="pageIndex">当前页码</param>
        ///<param name="pageSize">每页记录条数</param>
        ///<param name="outtable">输出表名</param>
        ///<returns></returns>
        public static DataSet GetCurrentPage(string sqlStr, int pageIndex, int pageSize, string outtable, params SqlParameter[] values)
        {
            DataSet dataset = new DataSet();
            SqlCommand cmd = new SqlCommand(sqlStr, Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(values);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dataset, pageIndex, pageSize, outtable);
            dataAdapter.Dispose();
            return dataset;
        }


        //#region 绑定DropDownList控件的数据源
        ///// <summary>
        ///// 绑定DropDownList控件的数据源
        ///// </summary>
        ///// <param name="ddl">ddl为DropDownList控件的ID</param>
        ///// <param name="sqlStr">为存储过程</param>
        //public static void DataBindDropDownList(DropDownList ddl, string sqlStr)
        //{
        //    DataSet dataset = new DataSet();
        //    SqlCommand cmd = new SqlCommand(sqlStr, Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //    dataAdapter.Fill(dataset);
        //    ddl.DataSource = dataset;
        //    ddl.DataBind();
        //    dataAdapter.Dispose();
        //}
        ///// <summary>
        ///// 绑定DropDownList控件的数据源
        ///// </summary>
        ///// <param name="ddl">ddl为DropDownList控件的ID</param>
        ///// <param name="sqlStr">为存储过程</param>
        ///// <param name="dvf">为绑定到该控件的数据表的字段</param>
        //public static void DataBindDropDownList(DropDownList ddl, string sqlStr, string dvf)
        //{
        //    DataSet dataset = new DataSet();
        //    SqlCommand cmd = new SqlCommand(sqlStr, Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //    dataAdapter.Fill(dataset);
        //    ddl.DataSource = dataset;
        //    ddl.DataValueField = dvf;
        //    ddl.DataBind();
        //    dataAdapter.Dispose();
        //}
        ///// <summary>
        ///// 绑定DropDownList控件的数据源
        ///// </summary>
        ///// <param name="ddl">ddl为DropDownList控件的ID</param>
        ///// <param name="sqlStr">为存储过程</param>
        ///// <param name="dtf">为绑定到该控件的数据表的字段</param>
        ///// /// <param name="dvf">为绑定到该控件的数据表的字段</param>
        //public static void DataBindDropDownList(DropDownList ddl, string sqlStr, string dtf, string dvf)
        //{
        //    DataSet dataset = new DataSet();
        //    SqlCommand cmd = new SqlCommand(sqlStr, Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //    dataAdapter.Fill(dataset);
        //    ddl.DataSource = dataset;
        //    ddl.DataTextField = dtf;
        //    ddl.DataValueField = dvf;
        //    ddl.DataBind();
        //    dataAdapter.Dispose();
        //}
        ///// <summary>
        ///// ListBox控件绑定所有的用户的登录帐号和用户名
        ///// </summary>
        ///// <param name="lbo">ddl为ListBox控件的ID</param>
        ///// <param name="sqlStr">为存储过程</param>
        ///// <param name="dtf">为绑定到该控件的数据表的字段</param>
        ///// <param name="dvf">为绑定到该控件的数据表的字段</param>
        //public static void DataBindListBox(ListBox lbo, string sqlStr, string dtf, string dvf)
        //{
        //    DataSet dataset = new DataSet();
        //    SqlCommand cmd = new SqlCommand(sqlStr, Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //    dataAdapter.Fill(dataset);
        //    lbo.DataSource = dataset;
        //    lbo.DataTextField = dtf;
        //    lbo.DataValueField = dvf;
        //    lbo.DataBind();
        //    dataAdapter.Dispose();
        //}
        ///// <summary>
        ///// ListBox控件绑定所有的用户的登录帐号和用户名
        ///// </summary>
        ///// <param name="lbo">ddl为ListBox控件的ID</param>
        ///// <param name="sqlStr">为存储过程</param>
        ///// <param name="dtf">为绑定到该控件的数据表的字段</param>
        ///// <param name="dvf">为绑定到该控件的数据表的字段</param>
        //public static void DataBindListBox(ListBox lbo, string sqlStr, string dtf, string dvf, params SqlParameter[] values)
        //{
        //    DataSet dataset = new DataSet();
        //    SqlCommand cmd = new SqlCommand(sqlStr, Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddRange(values);
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //    dataAdapter.Fill(dataset);
        //    lbo.DataSource = dataset;
        //    lbo.DataTextField = dtf;
        //    lbo.DataValueField = dvf;
        //    lbo.DataBind();
        //    dataAdapter.Dispose();
        //}
        //#endregion


        //#region 绑定GridView控件的数据源
        ///// <summary>
        ///// 绑定GridView控件的数据源
        ///// </summary>
        ///// <param name="gv">gv为GridView控件的ID名</param>
        ///// <param name="sqlStr">为存储过程</param>
        //public static void DataBindGridView(GridView gv, string sqlStr)
        //{
        //    DataSet dataset = new DataSet();
        //    SqlCommand cmd = new SqlCommand(sqlStr, Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //    dataAdapter.Fill(dataset);
        //    gv.DataSource = dataset;
        //    gv.DataBind();
        //    dataAdapter.Dispose();
        //}
        ///// <summary>
        ///// 绑定GridView控件的数据源
        ///// </summary>
        ///// <param name="gv">gv为GridView控件的ID名</param>
        ///// <param name="sqlStr">为存储过程</param>
        ///// <param name="DataKeyNames">主键字段属性</param>
        ///// <param name="DNK">主键字段值</param>
        //public static void DataBindGridView(GridView gv, string sqlStr, string DNK)
        //{
        //    DataSet dataset = new DataSet();
        //    SqlCommand cmd = new SqlCommand(sqlStr, Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //    dataAdapter.Fill(dataset);
        //    gv.DataSource = dataset;
        //    gv.DataKeyNames = new string[] { DNK };
        //    gv.DataBind();
        //    dataAdapter.Dispose();
        //}
        //#endregion


        /// <summary>
        /// 通过ADO.NET执行FOR XML查询
        /// 将一个表的数据转换为一个XMl文件
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static XmlReader GetXmlReader(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            XmlReader reader1 = cmd.ExecuteXmlReader();
            return reader1;
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="strs">传入将要格式化的字符串</param>
        /// <param name="sLength">要截取字符串的实际长度</param>
        /// <returns>sNewStr</returns>
        public static string SubString(string strs, int sLength)
        {
            if (strs.Length <= sLength)
            {
                return strs;
            }
            int nStrLength = sLength - 1;
            string sNewStr = strs.Substring(0, sLength);
            sNewStr = sNewStr + "Mores...";
            return sNewStr;
        } 
		
	}
}