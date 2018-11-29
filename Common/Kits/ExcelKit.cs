using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;   //   这个才能使用Missing字段 
using Excel= Microsoft.Office.Interop.Excel;
 


namespace Common.Kits
{
    public static  class ExcelKit
    {
        public static  void ExcelDataIn(string filepath,int xsheet)
        {
            //创建Application对象   
            Excel.Application xApp = new Excel.ApplicationClass();

            xApp.Visible = true;
            //得到WorkBook对象,   可以用两种方式之一:   下面的是打开已有的文件   
            Excel.Workbook xBook = xApp.Workbooks._Open(@"D:\Sample.xls ", Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            //xBook=xApp.Workbooks.Add(Missing.Value);//新建文件的代码   
            //指定要操作的Sheet，两种方式：   

            Excel.Worksheet xSheet = (Excel.Worksheet)xBook.Sheets[1];
            //Excel.Worksheet   xSheet=(Excel.Worksheet)xApp.ActiveSheet;   

            //读取数据，通过Range对象   
            Excel.Range rng1 = xSheet.get_Range("A1 ", Type.Missing);
            Console.WriteLine(rng1.Value2);

            //读取，通过Range对象，但使用不同的接口得到Range   
            Excel.Range rng2 = (Excel.Range)xSheet.Cells[3, 1];
            Console.WriteLine(rng2.Value2);

            //写入数据 
            Excel.Range rng3 = xSheet.get_Range("C6 ", Missing.Value);
            rng3.Value2 = "Hello ";
            rng3.Interior.ColorIndex = 6;   //设置Range的背景色   

            //保存方式一：保存WorkBook   
           // xBook.SaveAs(@"D:\CData.xls ", Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            //保存方式二：保存WorkSheet   
            xSheet.SaveAs(@"D:\CData2.xls ", Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


            //保存方式三   
            xBook.Save();

            xSheet = null;
            xBook = null;
            xApp.Quit();   //这一句是非常重要的，否则Excel对象不能从内存中退出   
            xApp = null;
        }

    }
}
