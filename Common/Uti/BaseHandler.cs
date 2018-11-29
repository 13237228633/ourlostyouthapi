using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

using System.Text;

namespace Common
{
    public interface ITagable
    {
        /// <summary>
        /// 标签
        /// </summary>
        string Tag
        {
            get;
        }
    }

    /// <summary>
    /// Handler基类
    /// </summary>
    public abstract class BaseHandler : IHttpHandler, ITagable, IRequiresSessionState
    {
        /// <summary>
        /// 当前的操作员
        /// </summary>
      //  protected SIMUser CurrentUser;
        //protected string CurrentOperator
        //{
        //    get { return HttpContext.Current.User.Identity.Name; }
        //}

        /// <summary>
        /// 排序列
        /// </summary>
        protected string sidex;

        /// <summary>
        /// 排序顺序：升序/降序
        /// </summary>
        protected string sord;

        /// <summary>
        /// 页码
        /// </summary>
        protected int pageNo = 1;

        /// <summary>
        /// 每页记录行数
        /// </summary>
        protected int pageSize = 20;


        #region IHttpHandler Members

        /// <summary>
        /// 是否可重用
        /// </summary>
        public virtual bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Handler处理函数
        /// </summary>
        /// <param name="context"></param>
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            //this.CurrentUser = SIMBusiness.UserManager.GetUserByName(context.User.Identity.Name);
            sidex = context.Request["sidx"];
            sord = context.Request["sord"];


            int.TryParse(context.Request.QueryString["rows"], out pageSize);
            int.TryParse(context.Request.QueryString["page"], out pageNo);

            if (IsAuthenticatedUser)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Buffer = true;
                context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                context.Response.AddHeader("pragma", "no-cache");
                context.Response.AddHeader("cache-control", "");
                context.Response.CacheControl = "no-cache";

                DoProcessRequest(context);
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("false");
            }
        }

        /// <summary>
        /// 是否为已验证用户
        /// </summary>
        protected virtual bool IsAuthenticatedUser
        {
            get { return true; }
        }

        #endregion

        /// <summary>
        /// Handler处理函数
        /// </summary>
        /// <param name="context"></param>
        abstract public void DoProcessRequest(HttpContext context);

        #region ITag Members

        /// <summary>
        /// 标签
        /// </summary>
        public virtual string Tag
        {
            get { return String.Empty; }
        }

        #endregion

        #region 数据检查方法

        /// <summary>
        /// 根据指定键试图获取允许为空的字符串
        /// </summary>
        /// <param name="requestKey"></param>
        /// <returns></returns>
        protected string TryGetString(string requestKey)
        {
            return TryGetString(requestKey, false);
        }

        /// <summary>
        /// 根据指定键获取字符串
        /// </summary>
        /// <param name="requestKey"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        protected string TryGetString(string requestKey, bool required)
        {
            InputCheckType[] ict = null;
            if (required)
            {
                ict = new InputCheckType[] { InputCheckType.NotNUll };
            }
            return TryGetString(requestKey, requestKey, ict);
        }

        /// <summary>
        /// 根据指定键获取不允许为空的字符串
        /// </summary>
        /// <param name="requestKey"></param>
        /// <param name="fieldTitle"></param>
        /// <returns></returns>
        protected string TryGetString(string requestKey, string fieldTitle)
        {
            string strdata = HttpContext.Current.Request[requestKey];
            InputChecker.CheckResult checkResult = InputChecker.CheckData(strdata, InputCheckType.NotNUll);
            return TryGetString(requestKey, fieldTitle, InputCheckType.NotNUll);
        }


        /// <summary>
        /// 获取符合InputCheckTypes规则的字符串
        /// </summary>
        /// <param name="requestKey">key</param>
        /// <param name="fieldTitle">the field's name</param>
        /// <param name="InputCheckTypes">check type</param>
        /// <returns></returns>
        protected string TryGetString(string requestKey, string fieldTitle, params InputCheckType[] InputCheckTypes)
        {
            string strdata = HttpContext.Current.Request[requestKey];

            InputChecker.CheckResult checkResult = InputChecker.CheckData(strdata, InputCheckTypes);

            if (checkResult.status == InputChecker.CheckResult.Status.Failed)
            {
                throw new Exception("内容[" + fieldTitle + "]" + checkResult.Message);
            }
            else
            {
                return strdata;
            }
        }


        #endregion

        /// <summary>
        /// 空Response
        /// </summary>
        protected string EmptyResponse
        {
            get
            {
                return Object2GridData.EmptyDataString;
            }
        }

        /// <summary>
        /// 将列集合按指定顺序排序
        /// </summary>
        /// <param name="rawList"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        protected IList<Dictionary<string, string>> ResortColumns(IList<Dictionary<string, string>> rawList, string[] columns)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (Dictionary<string, string> item in rawList)
            {
                Dictionary<string, string> newItem = new Dictionary<string, string>();
                foreach (string column in columns)
                    newItem.Add(column, item[column]);
                result.Add(newItem);
            }

            return result;
        }



        /// <summary>
        /// 根据指定参数获取对应的Dictionary对象
        /// </summary>
        /// <param name="paramsName">参数名列表，须符合如下格式： "client_name:server_name"</param>
        /// <returns></returns>
        protected Dictionary<string, string> GetQueryParameters(params string[] paramsName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in paramsName)
            {
                string clientParamName = null;
                string serverParamName = null;
                if (item.IndexOf(':') < 0)
                {
                    clientParamName = serverParamName = item;
                }
                else
                {
                    string[] temp = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length != 2)
                        throw new ArgumentException("参数使用错误:" + item);
                    clientParamName = temp[0];
                    serverParamName = temp[1];
                }
                string paramValue = HttpContext.Current.Request[clientParamName];
                if (!string.IsNullOrEmpty(paramValue))
                    result.Add(serverParamName, HttpContext.Current.Request[clientParamName]);
            }
            return result;
        }

        /// <summary>
        /// 读取http参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string ReadHttpParameter(HttpContext context, string key)
        {
            string result = context.Request.Params[key];
            if (result == null)
                result = String.Empty;

            return result;
        }

        /// <summary>
        /// 读取http整型参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected int ReadHttpParameterInt(HttpContext context, string key)
        {
            int tmp = 0;
            string result = ReadHttpParameter(context, key);
            Int32.TryParse(result, out tmp);

            return tmp;
        }

        /// <summary>
        /// 缺省的删除/恢复操作
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objName"></param>
        /// <param name="delHandler"></param>
        /// <param name="resHandler"></param>
        /// <returns></returns>
        protected string DefaultDelOrResOperation(HttpContext context, string objName, Func<string, List<string>, bool> delHandler, Func<string, List<string>, bool> resHandler)
        {
            if (ReadHttpParameter(context, "id").Equals("delete"))
                return DefaultDeleteOperation(context, objName, delHandler);
            else if (ReadHttpParameter(context, "id").Equals("restore"))
                return DefaultRestoreOperation(context, objName, resHandler);

            return JSON.ToJsonByJSNET(new AjaxOperationResult());
        }

        /// <summary>
        /// 缺省的删除操作
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objName"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        protected string DefaultDeleteOperation(HttpContext context, string objName, Func<string, List<string>, bool> handler)
        {
            AjaxOperationResult aor = new AjaxOperationResult();

            try
            {
                string delData = ReadHttpParameter(context, "del_ids");
                List<string> ids = JSON.ToObject<List<string>>(delData);

                // append operator information
              //  objName = objName + "$$" + CurrentUser.UserName;
                aor.success = handler(objName, ids);
            }
            catch
            {
                aor.success = false;
            }

            if (!aor.success)
            {
                aor.message = "删除失败";
            }

            return JSON.ToJson(aor);
        }

        /// <summary>
        /// 缺省的恢复操作
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objName"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        protected string DefaultRestoreOperation(HttpContext context, string objName, Func<string, List<string>, bool> handler)
        {
            AjaxOperationResult aor = new AjaxOperationResult();

            try
            {
                string restoreData = ReadHttpParameter(context, "restore_ids");
                List<string> ids = JSON.ToObject<List<string>>(restoreData);

                // append operator information
             //  objName = objName + "$$" + CurrentUser.UserName;
                aor.success = handler(objName, ids);
            }
            catch
            {
                aor.success = false;
            }

            if (!aor.success)
            {
                aor.message = "还原失败";
            }

            return JSON.ToJson(aor);
        }

        protected string GetDisplayID(string Prefix, int ID)
        {
            string Delimiter = "-";
            int Amount = 6;
            string Seeds = "000000000000000000000000";
            StringBuilder sb=new StringBuilder();
            string sID=ID.ToString();
            try
            {
                sb.Append(Prefix);
                sb.Append(Delimiter);
                string temp = Seeds.Substring(0, (Amount - sID.Length));
                sb.Append(temp);
                sb.Append(sID);
                return sb.ToString();
            }
            catch(Exception ex)
            {
                //SIMBusiness.UIProcessException(ex);
                return string.Empty;
            }

            
        }

        //protected string GetUserDisplayName(string username)
        //{
        //    SIMUser user = SIMBusiness.UserManager.GetUserByName(username);
        //    if(user!=null)
        //    {
        //        if (!string.IsNullOrEmpty(user.DisplayName))
        //        {
        //            return user.DisplayName;
        //        }
        //    }
        //    return username;
        //}
    //    //根据前台传入的过滤选项，返回筛选规则对象
    //    protected SIMFilterRuleCollection GetRulesByFilter(SIMFilterRuleCollection rules, Model.Filter filter)
    //    {
    //        rules.GroupOption = filter.GroupOn.ToLower() == "and" ? SIMFilterGroupOption.And : SIMFilterGroupOption.Or;
    //        foreach (var item in filter.Rules)
    //        {
    //            rules.Items.Add(new SIMFilterRule() { Field = item.Field, Option = new SIMSearchRuleOption(item.Op), Data = item.FileterData });
    //        }
    //        if (filter.Groups == null)
    //        {
    //            return rules;
    //        }
    //        foreach (var ftr in filter.Groups)
    //        {
    //            SIMFilterRuleCollection rule = new SIMFilterRuleCollection();
    //            rules.Groups.Add(rule);
    //            GetRulesByFilter(rule, ftr);
    //        }
    //        return rules;
    //    }
    }

}