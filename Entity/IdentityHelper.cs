using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entity
{
    public class IdentityHelper
    {
        #region Const

        private const string CookieName = "identity";
        private const string Domain = "ourlostyouth.top";

        private const string UserNameKey = "username";
        private const string UserIdKey = "userid";
        private const string TokenKey = "token";
        public static string UserPhoneKey = "UserPhone";
        public static string UserHeadPortraitKey = "UserHeadPortrait";
        private const bool Debug = true;

        #endregion

       
        /// <summary>
        /// 
        /// </summary>
        private static HttpCookie Cookie
        {
            get { return HttpContext.Current.Request.Cookies[CookieName]; }
        }
        /// <summary>
        /// 
        /// </summary>
        private static  bool IsNull
        {
            get { return Cookie == null; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static  Guid Token
        {
            get
            {
                if (IsNull) { return Guid.Empty; }
                var value = Cookie[TokenKey];
                return string.IsNullOrEmpty(value) ? Guid.Empty : new Guid(value);
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public static  string UserName
        {
            get { return IsNull ? null : Cookie[UserNameKey]; }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public static   int UserId
        {
            get { return IsNull ? default(int) : Convert.ToInt32(Cookie[UserIdKey]); }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public static int UserPhone
        {
            get { return IsNull ? default(int) : Convert.ToInt32(Cookie[UserPhoneKey]); }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        public static int UserHeadPortrait
        {
            get { return IsNull ? default(int) : Convert.ToInt32(Cookie[UserHeadPortraitKey]); }
        }
        ///// <summary>
        ///// 用户类型
        ///// </summary>
        //public static byte UserType
        //{
        //    get { return IsNull ? default(byte) : Convert.ToByte(Cookie[UserTypeKey]); }
        //}

        ///// <summary>
        ///// 用户昵称
        ///// </summary>
        //public static string NikeName
        //{
        //    get { return IsNull ? null : HttpContext.Current.Server.UrlDecode( Cookie[UserNameKey]); }
        //}




        #region Remove
        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            if (Cookie == null) return;
            Cookie.Expires = DateTime.Now.AddDays(-1);
            if (!Debug)
            {
                Cookie.Domain = Domain;
            }
            HttpContext.Current.Response.Cookies.Add(Cookie);
            if (HttpContext.Current.Session!=null)
            {
                HttpContext.Current.Session.Clear();
            }
            
        }
        #endregion

        #region WriteAdminUser

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="userId"></param>
        public static void WriteAdminUser(int UserId, string UserPhone, string UserName, string UserHeadPortrait, Guid token)
        {

            var cookie = new HttpCookie(CookieName);

            cookie.HttpOnly = true;
            if (!Debug) { cookie.Domain = Domain; }


            cookie.Values[UserNameKey] = HttpContext.Current.Server.UrlEncode(UserName);
            cookie.Values[UserIdKey] = UserId.ToString();
            cookie.Values[UserPhoneKey] = UserPhone.ToString();
            cookie.Values[UserHeadPortraitKey] = UserHeadPortrait.ToString();
            //cookie.Values[NikeNameKey] = HttpContext.Current.Server.UrlEncode(nikename);
            cookie.Values[TokenKey] = token.ToString();

            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Request.Cookies.Remove(CookieName);
            HttpContext.Current.Request.Cookies.Add(cookie);

            HttpContext.Current.Session["UserNameKey"] = cookie.Values[UserNameKey];
            HttpContext.Current.Session["UserIdKey"] = cookie.Values[UserIdKey];
            HttpContext.Current.Session["UserPhoneKey"] = cookie.Values[UserPhoneKey];
            HttpContext.Current.Session["UserHeadPortraitKey"] = cookie.Values[UserHeadPortraitKey];
        }
        #endregion

        #region
        public static  bool Vali()
        {

            if (Cookie != null)
            {
                HttpContext.Current.Session["UserNameKey"] = Cookie.Values[UserNameKey];
                HttpContext.Current.Session["UserIdKey"] = Cookie.Values[UserIdKey];
                HttpContext.Current.Session["UserPhoneKey"] = Cookie.Values[UserPhoneKey];
                HttpContext.Current.Session["UserHeadPortraitKey"] = Cookie.Values[UserHeadPortraitKey];
                return true;
            }
            return false;
        }
        public static bool LoginVali()
        {
            if(UserId>0 )
            {
                return true;
            }
            else
            {
               return false;
            }
        }

        //public static bool Noteshow()
        //{
        //    if (Cookie.Values[UserTypeKey] == "1")
        //    {
        //        return false;
               
        //    }
        //    return true;
        //}

        #endregion 
    }
}
