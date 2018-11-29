using Entity;
using MvcMyhome.Controllers.Attribute;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMyhome.Controllers
{
    [Auth]
    public class HomeController : BaseController
    {
        public ActionResult Iframe()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Album()
        {
            int UserID = Common.PassPort.IdentityHelper.UserId;
            ViewData["dtAlbum"] = DAL.DALAlbum.selectAlbum(UserID);
            return View();
        }

        public ActionResult UploadAlbum()
        {
            return View();
        }

        //获取相册列表
        public ActionResult AlbumListJson(int UserId)
        {
            int UserID = Common.PassPort.IdentityHelper.UserId;
            DataTable dt = DAL.DALAlbum.selectAlbum(UserId);

            var Json = new { code = 0, msg = "", count = dt.Rows.Count, data = dt };

            string ret = JsonConvert.SerializeObject(Json);
            return Content(ret);
        }


        public ActionResult Albumceshi()
        {
            int UserID = Common.PassPort.IdentityHelper.UserId;
            ViewData["dtAlbum"] = DAL.DALAlbum.selectAlbum(UserID);
            return View();
        }

        public ActionResult MyVideo(int PageSize = 6, int CurrentIndex = 1, int ClassifyId = 0)
        {
            ViewBag.ClassifyId = ClassifyId;
            int UserID = Common.PassPort.IdentityHelper.UserId;
            PageInfo pageinfo = new PageInfo
            {
                PageSize = PageSize,
                CurrentIndex = CurrentIndex
            };

            ViewData["dtMyVideo"] = DAL.DALMyVideo.selectMyVideo(UserID, 3, ClassifyId, pageinfo);
            ViewBag.RecordCount = pageinfo.RecordCount;
            ViewBag.CurrentIndex = CurrentIndex;
            return View();
        }
        public ActionResult ceshi()
        {
            return View();
        }

        public ActionResult patu()
        {
            return View();
        }
        public ActionResult Execpatu()
        {
            return View();
        }
    }
}