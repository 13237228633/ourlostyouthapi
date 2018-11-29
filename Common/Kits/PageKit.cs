using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Compilation;
//using System.Web.Mvc;
using System.Web.UI;

namespace Common
{
    public static class PageKit
    {
        #region CreateHtml 生成静态html
        /// <summary>
        ///  生成静态html
        /// </summary>
        /// <param name="path">动态页地址</param>
        /// <param name="outPath">静态页存放地址</param>
        public static void CreateHtml(string path, string outPath)
        {
            Page page = new Page();
            StringWriter writer = new StringWriter();
            page.Server.Execute(path, writer);
            FileStream fileStream;
            if (File.Exists(page.Server.MapPath("") + "\\" + outPath))
            {
                File.Delete(page.Server.MapPath("") + "\\" + outPath);
                fileStream = File.Create(page.Server.MapPath("") + "\\" + outPath);
            }
            else
            {
                fileStream = File.Create(page.Server.MapPath("") + "\\" + outPath);
            }
            byte[] bytes = Encoding.Default.GetBytes(writer.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }
        #endregion


        #region GetHtmlByUrl 根据Url获取Html
        /// <summary>
        /// 根据Url获取Html
        /// </summary>
        /// <param name="url">合法的Url地址</param>
        /// <returns></returns>
        public static string GetHtmlByUrl(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Timeout = 20000;//20秒超时
            WebResponse webResponse = webRequest.GetResponse();
            using(Stream stream = webResponse.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
        #endregion

        //public static string RenderPartialView(string partial, object model)
        //{
        //    try
        //    {
        //        var viewInstance = BuildManager.CreateInstanceFromVirtualPath(partial, typeof(object));
        //        var control = viewInstance as ViewUserControl;

        //        if (control != null)
        //        {
        //            control.ViewContext = new ViewContext();
        //            control.ViewData = new ViewDataDictionary(model);
        //        }

        //        Page page = new ViewPage();
        //        page.Controls.Add(control);

        //        TextWriter writer = new StringWriter();
        //        HttpContext.Current.Server.Execute(page, writer, true);

        //        return writer.ToString();
        //    }
        //    catch (Exception)
        //    {
        //        return string.Empty;
        //    }
        //}

        //public static string RenderView(string partial, object model)
        //{
        //    try
        //    {
        //        var viewInstance = BuildManager.CreateInstanceFromVirtualPath(partial, typeof(object));
        //        var page = viewInstance as ViewPage;

        //        if (page != null)
        //        {
        //            page.ViewContext = new ViewContext();
        //            page.ViewData = new ViewDataDictionary(model);
        //        }
                
        //        using(TextWriter writer = new StringWriter())
        //        {
        //            HttpContext.Current.Server.Execute(page, writer, true);
        //            return writer.ToString();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return string.Empty;
        //    }
        //}
       


    }
}
