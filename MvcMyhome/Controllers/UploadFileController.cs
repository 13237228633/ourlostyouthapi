using Common.PassPort;
using DAL;
using Newtonsoft.Json;
using QCloud.CosApi.Api;
using QCloud.CosApi.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcMyhome.Controllers
{
    public class UploadFileController : Controller
    {
        // GET: UploadFile
        //上传头像
        public ActionResult UploadFile(string UploadType="")
        {
            string path = "";
            
            returnJson rg = new returnJson();
            data data = new data();

            try
            {
                switch (UploadType)
	            {
                    //上传头像
                    case "UploadAvatar":
                        path = UploadAvatar();
                        break;
                    //上传文件
		            default:
                        path = UploadMyDiary();
                        break;
	            }
            }
            catch (Exception ex)
            {
                rg.code = 1;
                rg.msg = ex.Message;
                data.src = "";
                data.title = "";
                rg.data = data;
                return Json(rg);
            }



            if (path == "false")
            {
                rg.code = 1;
                rg.msg = "上传失败...";
                data.src = "";
                data.title = "";
                rg.data = data;
                return Json(rg);
            }
            else
            {
                //上传成功后显示IMG文件  
                StringBuilder sb = new StringBuilder();
                sb.Append(path.Split(',')[0].Replace("\\", "/"));

                rg.code = 0;
                rg.msg = "上传成功...";
                data.src = sb.ToString();
                data.title = path.Split(',')[1];
                rg.data = data;
                return Json(rg);
            }
        }

        public string UploadAvatar() 
        {
            string path = "\\Content\\UploadFile\\";
            string base64 = Request["image"].Split(',')[1];
            byte[] arr = Convert.FromBase64String(base64);

            string imgname = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
            path = path + imgname;

            FileStream fs = new FileStream(Server.MapPath(path), FileMode.Create, FileAccess.Write);
            fs.Write(arr, 0, arr.Length);
            fs.Close();
            if (!UploadTenXunOss(Server.MapPath(path), imgname, "photo", "UploadAvatarOss"))
            {
                if (System.IO.File.Exists(Server.MapPath(path)))//判断文件是否存在
                {
                    System.IO.File.Delete(Server.MapPath(path));//执行IO文件删除,需引入命名空间System.IO;    
                }
                return "false";
            }
    
            if (System.IO.File.Exists(Server.MapPath(path)))//判断文件是否存在
            {
                System.IO.File.Delete(Server.MapPath(path));//执行IO文件删除,需引入命名空间System.IO;    
            }
            IdentityHelper.WriteAdminUser(IdentityHelper.UserId, HttpUtility.UrlDecode(IdentityHelper.UserPhone), HttpUtility.UrlDecode(IdentityHelper.UserName) , "http://myhome1314-1253564673.cossh.myqcloud.com/photo/" + imgname, new Guid());
            return "http://myhome1314-1253564673.cossh.myqcloud.com/photo/" +imgname +"," + imgname;
        }


        public string UploadMyDiary()
        {
            string path = "\\Content\\UploadFile\\";
            HttpRequest request = System.Web.HttpContext.Current.Request;
            HttpFileCollection FileCollect = request.Files;
            HttpPostedFile file = FileCollect[0];

            //获取文件的扩展名
            //string fileName = Path.GetExtension(file.FileName);  
            string fileName = DateTime.Now.ToString("yyyyMMHHmmssfff") + new Random().Next(1000000) + Path.GetExtension(file.FileName);
            path +=fileName;
            file.SaveAs(Server.MapPath(path));

            if (!UploadTenXunOss(Server.MapPath(path), fileName, "MyDiaryPhoto", "UploadMyDiaryOss"))
            {
                if (System.IO.File.Exists(Server.MapPath(path)))//判断文件是否存在
                {
                    System.IO.File.Delete(Server.MapPath(path));//执行IO文件删除,需引入命名空间System.IO;    
                }
                return "false";
            }

            if (System.IO.File.Exists(Server.MapPath(path)))//判断文件是否存在
            {
                System.IO.File.Delete(Server.MapPath(path));//执行IO文件删除,需引入命名空间System.IO;    
            }
            return "http://myhome1314-1253564673.cossh.myqcloud.com/MyDiaryPhoto/" + fileName + "," + fileName;
        }



        const int APP_ID = 1253564673;
        const string SECRET_ID = "AKID3vYw61S6IFwOccPW7UeYDdplRs53jFSQ";
        const string SECRET_KEY = "oMjsqZ4JK6dvzoOjohNozve7LJ2asvjH";
        //上传到腾讯云，并添加到数据库
        public bool UploadTenXunOss(string file, string fileName, string TenXunfile,string HodeDataType)
        {

            Entity.Album a = new Entity.Album();
    
            //创建cos对象
            var cos = new CosCloud(APP_ID, SECRET_ID, SECRET_KEY);
            string bucketName = "myhome1314";
            string localPath = file;
            string remotePath = "/" + TenXunfile + "/" + fileName;

            //上传文件（不论文件是否分片，均使用本接口）
            var uploadParasDic = new Dictionary<string, string>();
            uploadParasDic.Add(CosParameters.PARA_BIZ_ATTR, "");
            uploadParasDic.Add(CosParameters.PARA_INSERT_ONLY, "0");
            var result = cos.UploadFile(bucketName, remotePath, localPath, uploadParasDic);

            a.AlnumName = fileName;
            a.AlnumUrl = "http://myhome1314-1253564673.cossh.myqcloud.com/" + TenXunfile + "/" + fileName;
            a.UserId=Common.PassPort.IdentityHelper.UserId;
       
            switch (HodeDataType)
            {
                case "UploadAvatarOss":
                    if (!DALAlbum.upUserHeadPortrait(a))
                    return false;
                    break;
                case "UploadMyDiaryOss":
                    break;
                default:
                    break;
            }
            return true;
        }

           


         public class returnJson
        {
            public int code { get; set; }
            public string msg { get; set; }
            public data data { get; set; } 
        }
        public class data
        {
            public string src { get; set; }
            public string title { get; set; } 
        }
    }
}