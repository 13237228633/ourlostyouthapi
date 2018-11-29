using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.IO;
using MvcMyhome.Controllers.Attribute;
using System.Drawing.Imaging;
using System.Drawing;

namespace MvcMyhome.Controllers
{
    [Auth]
    public class MyDiaryController : BaseController
    {
        // GET: MyDiary
        public ActionResult MyDiary()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult MyDiaryAdd(string DiaryContent, string CreateDate, int DiaryClassifyId = 0, int DiaryOvertState = 0)
        {
            MyDiary mydiary = new MyDiary();
            mydiary.DiaryContent = DiaryContent;
            mydiary.DiaryClassifyId = DiaryClassifyId;
            mydiary.DiaryOvertState = DiaryOvertState;
            mydiary.CreateDate = CreateDate == "" ? DateTime.Now.ToString("yyyy-MM-dd") : CreateDate;
            mydiary.UserId = Common.PassPort.IdentityHelper.UserId;
            mydiary.IsEnable = 0;

            if (DAL.DALMyDiary.MyDiaryAdd(mydiary))
                return Json(string.Format("{{\"result\":{0},\"message\":\"{1}\"}}", "true", "恭喜你，记录成功..."));
            else
                return Json(string.Format("{{\"result\":{0},\"message\":\"{1}\"}}", "false", "Sorry，记录失败..."));

        }
        public ActionResult getMyDiaryList(int DiaryClassifyId = 0, int DiaryOvertState = 0)
        {
            MyDiary mydiary = new MyDiary();
            mydiary.DiaryClassifyId = 0;
            mydiary.DiaryOvertState = 0;
            mydiary.UserId = Common.PassPort.IdentityHelper.UserId;
            DataTable dt = DAL.DALMyDiary.getMyDiaryList(mydiary, 0);
            dt.Columns.Add("DataYear");
            dt.Columns.Add("DataTime");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["DataYear"] = Convert.ToDateTime(dt.Rows[i]["CreateDate"]).ToString("yyyy-MM-dd");
                dt.Rows[i]["DataTime"] = Convert.ToDateTime(dt.Rows[i]["CreateDate"]).ToString("HH:mm:ss");
            }
            //JsonSerializerSettings setting = new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};setting
            string ret = JsonConvert.SerializeObject(dt);
            return Content(ret);
        }

        public ActionResult MyDiaryDelete(int DiaryId)
        {
            MyDiary mydiary = new MyDiary();
            mydiary.DiaryId = DiaryId;
            mydiary.UserId = Common.PassPort.IdentityHelper.UserId;
            int Result = -1;
            if (DAL.DALMyDiary.MyDiaryDelete(mydiary, out Result))
                return Json(string.Format("{{\"result\":{0},\"message\":\"{1}\"}}", "true", "恭喜你，删除成功..."));
            else
                return Json(string.Format("{{\"result\":{0},\"message\":\"{1}\"}}", "false", "Sorry，删除失败..."));

        }


    }
}