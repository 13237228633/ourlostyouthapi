using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MvcMyhome.Controllers.Attribute   
{
    public class AuthAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //if (HttpContext.Current.Session["userid"] == null && context.ActionDescriptor.ActionName != "Login" && context.ActionDescriptor.ActionName != "ExecuteLogin")
            //{
            //    context.Result = new RedirectResult("/User/Login");
            //}
            if (!Common.PassPort.IdentityHelper.LoginVali() && context.ActionDescriptor.ActionName != "Login" && context.ActionDescriptor.ActionName != "WriteLog" && context.ActionDescriptor.ActionName != "ExecuteLogin" && context.ActionDescriptor.ActionName != "QqLogin" && context.ActionDescriptor.ActionName != "ExecuteQqLogin" && context.ActionDescriptor.ActionName != "ShowQqLogin" && context.ActionDescriptor.ActionName != "AlbumListJson")
            {
                context.Result = new RedirectResult("/User/Login");
            }
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}