using MvcMyhome.Controllers.Attribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMyhome.Controllers
{
    [Auth]
    public class BaseController : Controller
    {
        public DataTable dt { get; set; }

        // GET: Base
        public BaseController()
        {
            //int UserID = 0;
            //dt = DAL.DALAlbum.selectUserInfo(UserID);
            //ViewBag.UserHeadPortrait = dt.Rows[0]["UserHeadPortrait"].ToString();
            //ViewBag.UserName = dt.Rows[0]["UserName"].ToString();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}