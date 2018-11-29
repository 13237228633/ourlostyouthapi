using Entity;
using QqLogin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 线程并发
{
    class Program
    {
        //定义一个委托实现回调函数
        public delegate bool CallBackDelegate(bool message);
        static void Main(string[] args)
        {
            Dictionary<string, string> paremar = new Dictionary<string, string>(); 
            HttpRequest.ExecHttpRequest("", paremar,false,true);

          //// 程序开始的时候
          ////把回调的方法给委托变量
          //  CallBackDelegate cbd = CallBack;
          //  Thread t = new Thread(insertdate)
          //  {
          //      IsBackground = false
          //  };

            // t.Start(cbd);
        }

        static void insertdate(object o)
        {
            Console.WriteLine("开始填充数据时间："+ DateTime.Now);
            DataTable dt = new DataTable();
            dt.Columns.Add("DiaryContent");
            dt.Columns.Add("DiaryClassifyId");
            dt.Columns.Add("DiaryOvertState");
            dt.Columns.Add("CreateDate");
            dt.Columns.Add("UserId");
            dt.Columns.Add("IsEnable");
            
            for (int i = 0; i < 100000; i++)
            {
                DataRow dr = dt.NewRow();
                dr["DiaryContent"] = "<img src='http://myhome1314-1253564673.cossh.myqcloud.com/MyDiaryPhoto/201710134214586283562.jpg' alt='201710134214586283562.jpg' class='img-responsive'>我喜欢你在吗？<img src='http://myhome1314-1253564673.cossh.myqcloud.com/MyDiaryPhoto/20171013431116762988.jpg' alt='20171013431116762988.jpg' class='img-responsive'>酸不。";
                dr["DiaryClassifyId"] = 1;
                dr["DiaryOvertState"] = 1;
                dr["CreateDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                dr["UserId"] = 1;
                dr["IsEnable"] = 0;
                dt.Rows.Add(dr);
            }
            Console.WriteLine("数据填充完成时间：" + DateTime.Now);
            SqlBulkCopyInsert("MyDiarya", dt);
            Console.WriteLine("数据插入完成时间：" + DateTime.Now);

            //把传来的参数转换为委托
            CallBackDelegate cbd = o as CallBackDelegate;
            //执行回调.
            cbd(false);
        }
        //string ThNumber = Thread.CurrentThread.ManagedThreadId.ToString();
        //Console.WriteLine(1 + "个线程插入100万条数据所花的时间：" + ThNumber);
        //回调方法
        private static bool CallBack(bool message)
        {
            //主线程报告信息,可以根据这个信息做判断操作,执行不同逻辑.
            return message;
        }

        #region 使用SqlBulkCopy将DataTable中的数据批量插入数据库中
        /// <summary>  
        /// 使用SqlBulkCopy将DataTable中的数据批量插入数据库中  
        /// </summary>  
        /// <param name="strTableName">数据库中对应的表名</param>  
        /// <param name="dtData">数据集</param>  
        public static void SqlBulkCopyInsert(string strTableName, DataTable dtData)
        {
            string ConStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;// 数据库连接字符串  

            try
            {
                using (SqlBulkCopy sqlRevdBulkCopy = new SqlBulkCopy(ConStr))//引用SqlBulkCopy  
                {
                    sqlRevdBulkCopy.DestinationTableName = strTableName;//数据库中对应的表名  

                    sqlRevdBulkCopy.NotifyAfter = dtData.Rows.Count;//有几行数据  

                    sqlRevdBulkCopy.WriteToServer(dtData);//数据导入数据库  

                    sqlRevdBulkCopy.Close();//关闭连接  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "数据库处理出错SqlBulkCopyInsert");
                throw (ex);
            }
        }
        #endregion


        //public void multiThreadImport(int ThreadNum)
        //{
        //    //for (int i = 0; i < ThreadNum; i++) 
        //    //{

        //    //}
        //    string ThNumber = Thread.CurrentThread.ManagedThreadId.ToString();
        //    Console.WriteLine(ThreadNum+"个线程插入100万条数据所花的时间："+ThNumber);
        //}
    }
}

