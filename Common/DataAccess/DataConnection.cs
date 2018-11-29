namespace Common
{
    using System;
    using System.Web;
    using System.Data;
    using System.Collections;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    /// <summary>
    ///  数据库访问类
    /// </summary>
    public sealed class DataConnection : IDisposable
    {
        #region Private Members 私有成员

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private   SqlConnection _sqlConnection;

        
        /// <summary>
        ///  数据库事务对象
        /// </summary>
        private SqlTransaction _sqlTransaction;
        /// <summary>
        ///  是否已经标识释放
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        /// 
        /// </summary>
        public const string GatherConfig = "Gather";
         
        

        #endregion

        #region Constructor 构造方法
        /// <summary>
        ///  构造方法
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        public DataConnection(string databaseName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;
            _sqlConnection=new SqlConnection(connectionString);
        }
        #endregion

        #region GetConnection 获取数据库连接
        /// <summary>
        ///  获取数据库连接
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public static DataConnection GetConnection(string databaseName)
        {
            string connectionKey = string.Format("Database_{0}", databaseName);
            DataConnection dataConnection;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(connectionKey))
                {
                    dataConnection = HttpContext.Current.Items[connectionKey] as DataConnection;
                }
                else
                {
                    dataConnection = new DataConnection(databaseName);
                    HttpContext.Current.Items.Add(connectionKey, dataConnection);
                }
            }
            else
            {
                dataConnection = new DataConnection(databaseName);
            }
            return dataConnection;
        }
        #endregion
        

        #region OpenConnection 打开数据库连接
        /// <summary>
        ///  打开数据库连接
        /// </summary>
        private void OpenConnection()
        {
            if (this._sqlConnection != null && this._sqlConnection.State != ConnectionState.Open)
            {
                this._sqlConnection.Open();
            }
        }
        #endregion

        #region CloseConnection 关闭数据库连接
        /// <summary>
        ///  关闭数据库连接
        /// </summary>
        public  void CloseConnection()
        {
            if (this._sqlConnection != null && this._sqlConnection.State != ConnectionState.Closed)
            {
                this._sqlConnection.Close();
            }
        }
        #endregion

        #region PrepareTransaction 准备事务
        /// <summary>
        ///  准备事务
        /// </summary>
        /// <param name="cmd"></param>
        private void PrepareTransaction(SqlCommand cmd)
        {
            if (this._sqlTransaction != null)
            {
                cmd.Transaction = this._sqlTransaction;
            }
        }
        #endregion

        #region ExecuteDataSet 执行Sql语句并返回Dataset
        /// <summary>
        ///  执行Sql语句并返回Dataset
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">sql参数集合</param>
        /// <returns>Dataset对象</returns>
        public DataSet ExecuteDataSet(string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var cmd = this._sqlConnection.CreateCommand())
            {
                cmd.CommandType = commandType;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);

                this.PrepareTransaction(cmd);
                this.OpenConnection();

                using (var dataAdapter = new SqlDataAdapter(cmd))
                {
                    var dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    cmd.Parameters.Clear();
                   
                    return dataSet;
                }
            }
        }
        #endregion

        #region ExecuteDataTable 执行Sql语句并返回DataTable
        /// <summary>
        ///  执行Sql语句并返回DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">sql参数集合</param>
        /// <returns>DataTable对象</returns>
        public DataTable ExecuteDataTable(string sql, CommandType commandType, params  SqlParameter[] parameters)
        {
            using (var cmd = this._sqlConnection.CreateCommand())
            {
                cmd.CommandType = commandType;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                cmd.CommandTimeout = 120;
                this.PrepareTransaction(cmd);
                this.OpenConnection();

                using (var dataAdapter = new SqlDataAdapter(cmd))
                {
                    var dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    cmd.Parameters.Clear();
                    return dataTable;
                }
            }
        }
        #endregion

        #region ExecuteReader 执行Sql语句并返回SqlDataReader
        /// <summary>
        ///  执行Sql语句并返回SqlDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">sql参数集合</param>
        /// <returns>SqlDataReader对象</returns>
        public SqlDataReader ExecuteReader(string sql, CommandType commandType, params SqlParameter[] parameters)
        {

            using (var cmd = this._sqlConnection.CreateCommand())
            {
                cmd.CommandType = commandType;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);

                this.PrepareTransaction(cmd);
                this.OpenConnection();

                var dataReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
     
                return dataReader;
            }

            

        }

        #endregion
      




        #region ExecuteNonQuery 执行Sql语句并返回影响行数
        /// <summary>
        /// 执行Sql语句并返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">sql参数集合</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var cmd = this._sqlConnection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandTimeout = 10000;   //要加这一句
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                this.PrepareTransaction(cmd);
                this.OpenConnection();

                int result = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                
                return result;
            }
        }

        #endregion


        #region ExecuteScalar 执行Sql语句并返回单个对象
        /// <summary>
        ///  执行Sql语句并返回单个对象
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">sql参数集合</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var cmd = this._sqlConnection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                this.PrepareTransaction(cmd);
                this.OpenConnection();

                object result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();

                return result;
            }

        }

        #endregion

        #region BeginTransaction 开始执行事务
        /// <summary>
        ///  开始执行事务
        /// </summary>
        public void BeginTransaction()
        {
            this.OpenConnection();
            this._sqlTransaction = this._sqlConnection.BeginTransaction();
        }
        #endregion

        #region BeginTransaction 开始执行事务
        /// <summary>
        ///  开始执行事务
        /// </summary>
        /// <param name="level">事务隔离级别</param>
        public void BeginTransaction(IsolationLevel level)
        {
            this.OpenConnection();
            this._sqlTransaction = this._sqlConnection.BeginTransaction(level);
        }
        #endregion

        #region Commit 提交事务
        /// <summary>
        ///  提交事务
        /// </summary>
        public void Commit()
        {
            if (this._sqlTransaction != null && this._sqlTransaction.Connection != null)
            {
                this._sqlTransaction.Commit();
            }
            this.CloseConnection();
        }
        #endregion

        #region Rollback 回滚事务
        /// <summary>
        ///  回滚事务
        /// </summary>
        public void Rollback()
        {
            if (this._sqlTransaction != null)
            {
                this._sqlTransaction.Rollback();
            }
            this.CloseConnection();
        }
        #endregion

        #region IDisposable Implement IDisposable接口实现

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposedValue && disposing)
            {
                if (this._sqlTransaction != null)
                {
                    this._sqlTransaction.Dispose();
                }
                if (this._sqlConnection.State != ConnectionState.Closed)
                {
                    this._sqlConnection.Close();
                }
            }
            this._disposedValue = true;
        }
        #endregion

        #region ClearConnection 清除数所有据库连接对象
        /// <summary>
        ///  清除数所有据库连接对象
        /// </summary>
        public static void ClearConnection()
        {
            var dictionary = HttpContext.Current.Items;
            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Value is DataConnection)
                {
                    using (var connection = (DataConnection)entry.Value)
                    {
                        connection.Dispose();
                    }
                }
            }
        }
        #endregion



        public static string connectionString = ConfigurationManager.ConnectionStrings["Gather"].ConnectionString;
        /// <summary>  
        /// 执行多条SQL语句，实现数据库事务。  
        /// </summary>  
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>  
        public static void ExecuteSqlTran(Hashtable SQLStringList,string count)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();//打开数据库连接  
                using (SqlTransaction trans = conn.BeginTransaction())//开始数据库事务  
                {
                    SqlCommand cmd = new SqlCommand();//创建SqlCommand命令  
                    try
                    {
                        int val=0;
                        //循环  
                        foreach (DictionaryEntry myDE in SQLStringList)//循环哈希表（本例中 即，循环执行添加在哈希表中的sql语句  
                        {
                            string cmdText = myDE.Key.ToString().Substring(0, myDE.Key.ToString().Length-1);//获取键值（本例中 即，sql语句）  
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;//获取键值（本例中 即，sql语句对应的参数）  
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms); //调用PrepareCommand()函数，添加参数  
                             val += cmd.ExecuteNonQuery();//调用增删改函数ExcuteNoQuery()，执行哈希表中添加的sql语句  
                            cmd.Parameters.Clear(); //清除参数  
                        }
                        if (val < SQLStringList.Count)
                        {
                            trans.Rollback(); //事务回滚  
                        }
                        trans.Commit();//提交事务
                    }
                    catch //捕获异常  
                    {
                        trans.Rollback(); //事务回滚  
                        throw; //抛出异常  
                    }
                }
            }
        }  

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)  
        {  
            if (conn.State != ConnectionState.Open)//如果数据库连接为关闭状态  
                conn.Open();//打开数据库连接  
            cmd.Connection = conn;//设置命令连接  
            cmd.CommandText = cmdText;//设置执行命令的sql语句  
            if (trans != null)//如果事务不为空  
                cmd.Transaction = trans;//设置执行命令的事务  
            cmd.CommandType = CommandType.StoredProcedure;//设置解释sql语句的类型为“文本”类型（也是就说该函数不适用于存储过程）  
            if (cmdParms != null)//如果参数数组不为空  
            {
                cmd.Parameters.AddRange(cmdParms);
            }  
        }  


    }
}
