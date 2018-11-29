using DAL;
using MvcMyhome.Controllers.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using System.Data;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using QqLogin;
using Common.Kits;

namespace MvcMyhome.Controllers
{
    [Auth]
    public class UserController : Controller
    {
        /// QQ互联应用管理中的 APP ID
        private string APPID = "101424961";
        /// QQ互联应用管理中的 APP Key
        private string APPKey = "6cbae0702b635381013fbceb65fcc19d";
        /// QQ互联应用管理中的 网站回调域
        private string redirecturi = "http://116.196.118.203/User/QqLogin";

        ExecQqLogin execQqLogin = new ExecQqLogin();
        /// <summary>
        /// QQ登录调用方法
        /// </summary>
        /// <param name="code">QQ登录回调时获取到的参数</param>
        /// <param name="isShowLogin">判断是否是打开qq登录的窗口，true为是，false为否</param>
        /// <param name="isMobile">判断是否是用手机设备，true为是，false为否</param>
        /// <returns></returns>
        public ActionResult QqLogin(string code = "", bool isShowLogin = false, bool isMobile = false)
        {
            string urlLogin = "";
            if (isShowLogin)
            {
                try
                {
                    //获取QQ登录窗口连接，跳转QQ登录窗口
                    urlLogin = execQqLogin.openLogin(isMobile, APPID, redirecturi);
                }
                catch (Exception ex)
                {

                    LogHelper.WriteLog(LogFile.Error, ex.Message);
                }
             
            }
            else
            {
                //获取QQ信息
                QqLogin.Messge qqmessge = execQqLogin.QqLogin(APPID, APPKey, code, redirecturi);
                //执行数据库操作，执行程序登录
                if (ExecuteQqLogin(qqmessge))
                {
                    urlLogin = "/Home/Index";//登录成功，跳转到首页
                }
                else
                {
                    urlLogin = "/User/Login";//登录失败，跳转到登录界面让其用账号登录
                    LogHelper.WriteLog(LogFile.Error, qqmessge.Code.ToString()+"|"+ qqmessge.messge.ToString());
                }
            }
            return Redirect(urlLogin);
        }


        DALUser dalUser = new DALUser();
        public bool ExecuteQqLogin(QqLogin.Messge qqmessge)
        {
            User user = new User();
            if (qqmessge.Code == 100)
            {
                user.UserCity = qqmessge.qqLoginUser.city;
                user.UserHeadPortrait = qqmessge.qqLoginUser.figureurl_qq_2.Trim('\\');
                user.UserName = qqmessge.qqLoginUser.nickname;
                user.UserProvince = qqmessge.qqLoginUser.province;
                user.UserYear = qqmessge.qqLoginUser.year;
                user.userOpenid = qqmessge.openid;

                DataTable dtuser = dalUser.ExecuteQqLogin(user);
                if (dtuser.Rows.Count > 0)
                {
                    Common.PassPort.IdentityHelper.WriteAdminUser(Convert.ToInt32(dtuser.Rows[0]["UserId"]), dtuser.Rows[0]["UserPhone"].ToString(), dtuser.Rows[0]["UserName"].ToString(), dtuser.Rows[0]["UserHeadPortrait"].ToString(), new Guid());
                    try
                    {
                        Userloginlog userloginlog = new Userloginlog();
                        Userloginlog.MAC = Common.HttpKit.GetMac();
                        Userloginlog.ExternalIp = Common.HttpKit.CurrentRequestIP;
                        Userloginlog.InsideIp = Request.UserHostAddress;
                        Userloginlog.UserId = Convert.ToInt32(dtuser.Rows[0]["UserId"]);
                        dalUser.innerUserloginlog(userloginlog);
                    }
                    catch (Exception)
                    {
                    }
                    return true;
                }
                else
                    return false;
            }
            return false;
        }
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ExecuteLogin(string userphone, string password)
        {
            DataTable user = dalUser.UserLogin(userphone, Common.EncryptKit.ToLowerMd5(password.ToLower()));
            if (user.Rows.Count > 0)
            {
                Common.PassPort.IdentityHelper.WriteAdminUser(Convert.ToInt32(user.Rows[0]["UserId"]), user.Rows[0]["UserPhone"].ToString(), user.Rows[0]["UserName"].ToString(), user.Rows[0]["UserHeadPortrait"].ToString(), new Guid());

                try
                {
                    Userloginlog userloginlog = new Userloginlog();
                    Userloginlog.MAC = Common.HttpKit.GetMac();
                    Userloginlog.ExternalIp = Common.HttpKit.CurrentRequestIP;
                    Userloginlog.InsideIp = Request.UserHostAddress;
                    Userloginlog.UserId = Convert.ToInt32(user.Rows[0]["UserId"]);
                    dalUser.innerUserloginlog(userloginlog);
                }
                catch (Exception)
                {
                }
                return Json(string.Format("{{\"result\":{0},\"message\":\"{1}\"}}", "true", "登陆成功"));

                // return RedirectToAction("Iframe", "Home");
            }
            else
            {
                return Json(string.Format("{{\"result\":{0},\"message\":\"{1}\"}}", "false", "账户或密码错误，登陆失败"));
            }
        }

        public ActionResult ExecuteLoginOut()
        {
            Common.PassPort.IdentityHelper identityHelper = new Common.PassPort.IdentityHelper();
            identityHelper.Remove();
            identityHelper.Remove();
            if (!Common.PassPort.IdentityHelper.LoginVali())
            {
                //try
                //{
                //    Userloginlog userloginlog = new Userloginlog();
                //    Userloginlog.MAC = Common.HttpKit.GetMac();
                //    Userloginlog.ExternalIp = Common.HttpKit.CurrentRequestIP;
                //    Userloginlog.InsideIp = Request.UserHostAddress;
                //    Userloginlog.UserId = Convert.ToInt32(user.Rows[0]["UserId"]);
                //    bll.innerUserloginlog(userloginlog);
                //}
                //catch (Exception)
                //{
                //}
                return Json(string.Format("{{\"result\":{0},\"message\":\"{1}\"}}", "true", "退出成功..."));
            }
            else
            {
                return Json(string.Format("{{\"result\":{0},\"message\":\"{1}\"}}", "false", "退出失败..."));
            }
        }

        public ActionResult UserInfo()
        {
            return View();
        }


        public ActionResult UserInfoDataManage()
        {
            return View();
        }

        public void WriteLog(string messge)
        {
            LogHelper.WriteLog(LogFile.Error, messge);
        }
    }
}