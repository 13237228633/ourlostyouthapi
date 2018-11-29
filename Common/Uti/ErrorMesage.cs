using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatherPlat.Web.Uti
{
    public class ErrorMesage
    {
        public const string SearchParam_Number = "Number";
        public const string SearchParam_Date = "Date";
        public static string InputTrueSearch = "myerror$请输入正确的数据";
        public static string InputTrueSearch_Number = "myerror$请输入正确的整数";
        public static string InputTrueSearch_Date = "myerror$请输入正确的日期格式(如：2000-01-01)";

        public static string ReturnInputErrorMessage(string param)
        {
            switch (param)
            {
                case ErrorMesage.SearchParam_Number:
                    return ErrorMesage.InputTrueSearch_Number;
                case ErrorMesage.SearchParam_Date:
                    return ErrorMesage.InputTrueSearch_Date;
                default:
                    return ErrorMesage.InputTrueSearch;
            }
        }
    }
}