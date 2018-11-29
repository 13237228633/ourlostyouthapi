using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// 输入检查器。用来在服务器端检查做通用的数据检查
    /// </summary>
    public class InputChecker
    {
        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="InputCheckTypes"></param>
        /// <returns></returns>
        public static CheckResult CheckData(string data, params InputCheckType[] InputCheckTypes)
        {
            CheckResult checkResult = new CheckResult();

            if (InputCheckTypes == null)
            {
                return checkResult;
            }

            foreach (InputCheckType InputCheckType in InputCheckTypes)
            {
                switch (InputCheckType)
                {
                    case InputCheckType.NotNUll:
                        if (string.IsNullOrEmpty(data))
                        {
                            checkResult.status = CheckResult.Status.Failed;
                            checkResult.Message = "关键项不能为空";
                        }
                        break;
                    case InputCheckType.IsEmailAddress:
                        if (string.IsNullOrEmpty(data))
                            break;
                        else if (!Regex.IsMatch(data, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                        {
                            checkResult.status = CheckResult.Status.Failed;
                            checkResult.Message = "邮件格式不正确";
                        }
                        break;
                    case InputCheckType.IsNumber:
                        if (string.IsNullOrEmpty(data))
                            break;
                        else if (!Regex.IsMatch(data, @"^\d+(\.)?\d*$"))
                        {
                            checkResult.status = CheckResult.Status.Failed;
                            checkResult.Message = "数字格式填写不正确";
                        }
                        break;
                }

            }

            return checkResult;
        }

        /// <summary>
        /// 检查结果类
        /// </summary>
        public class CheckResult
        {
            public string Message
            {
                get;
                set;
            }

            public Status status = Status.Successed;
            public InputCheckType InputCheckType;
            public enum Status
            {
                Successed,
                Failed
            }
        }
    }

    /// <summary>
    /// 输入检查类型
    /// </summary>
    public enum InputCheckType
    {
        /// <summary>
        /// 检查是否为空对象/字符串
        /// </summary>
        NotNUll,

        /// <summary>
        /// 检查Email地址是否正确
        /// </summary>
        IsEmailAddress,

        /// <summary>
        /// 检查指定字符串是否可转为数字
        /// </summary>
        IsNumber
    }


}