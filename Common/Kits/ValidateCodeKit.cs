namespace Common
{
    using System;
    using System.Web;
    using System.Drawing;

    public static class ValidateCodeKit
    {

        #region Const
        /// <summary>
        /// 
        /// </summary>
        public const string SessionName = "SecureCode";

        #endregion

        #region 验证安全码(若成功,则置空验证码Session)
        /// <summary>
        /// 验证安全码(若成功,则置空验证码Session)
        /// </summary>
        /// <param name="securecode">安全码</param>
        /// <returns>验证成功与否</returns>
        public static bool IsAuthenticated(string securecode)
        {
            return ValidateSecureCode(securecode, true);
        }

        /// <summary>
        /// 验证安全码
        /// </summary>
        /// <param name="securecode">安全码</param>
        /// <param name="removeCode">验证成功后,是否将安全码Session置空</param>
        /// <returns>验证成功与否</returns>
        public static bool ValidateSecureCode(string securecode, bool removeCode)
        {
            bool success = false;
            if (HttpContext.Current.Session[SessionName] != null)
            {
                string code = HttpContext.Current.Session[SessionName].ToString().ToLower();
                if (securecode.ToLower() == code)
                {
                    success = true;
                    if (removeCode)
                    {
                        HttpContext.Current.Session.Remove(SessionName);
                    }
                }
            }
            return success;
        }
        #endregion

        #region 生成安全码图片, 将得到的安全码写入Session["SecureCode"]
        /// <summary>
        /// 生成安全码图片, 将得到的安全码写入Session["SecureCode"]
        /// </summary>
        /// <param name="codelength">验证码位数</param>
        /// <param name="color">颜色</param>
        public static void DrawingSecureCode(int codelength, Color color)
        {
            string code = Guid.NewGuid().ToString().Substring(0, codelength).ToUpper();
            var font = new Font("System", 12, FontStyle.Regular);
            DrawingSecureCode(code, 100, 50, font, color);
        }

        /// <summary>
        /// 生成安全码图片, 将得到的安全码写入Session["SecureCode"]
        /// </summary>
        /// <param name="codelength">验证码位数</param>
        /// <param name="imgWidth">图片宽度</param>
        /// <param name="imgHeight">图片高度</param>
        /// <param name="font">字体</param>
        /// <param name="color">颜色</param>
        public static void DrawingSecureCode(int codelength, int imgWidth, int imgHeight, Font font, Color color)
        {
            string code = Guid.NewGuid().ToString().Substring(0, codelength).ToUpper();
            DrawingSecureCode(code, imgWidth, imgHeight, font, color);
        }

        /// <summary>
        /// 生成安全码图片, 将得到的安全码写入Session["SecureCode"]
        /// </summary>
        /// <param name="code">验证码</param>
        /// <param name="color">颜色</param>
        public static void DrawingSecureCode(string code, Color color)
        {
            var font = new Font("System", 12, FontStyle.Regular);
            DrawingSecureCode(code, 100, 50, font, color);
        }

        /// <summary>
        /// 生成安全码图片, 将得到的安全码写入Session["SecureCode"]
        /// </summary>
        /// <param name="code">验证码</param>
        /// <param name="imgwidth">图片宽度</param>
        /// <param name="imgheight">图片高度</param>
        /// <param name="font">字体</param>
        /// <param name="color">颜色</param>
        public static void DrawingSecureCode(string code, int imgwidth, int imgheight, Font font, Color color)
        {
            using (var bitmap = new Bitmap(imgwidth, imgheight))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawString(code, font, new SolidBrush(color), 0, 0);
                var stream = new System.IO.MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = @"image/png";
                HttpContext.Current.Response.BinaryWrite(stream.ToArray());
                stream.Dispose();
            }

            if (HttpContext.Current.Session[SessionName] == null)
            {
                HttpContext.Current.Session.Add(SessionName, code);
            }
            else
            {
                HttpContext.Current.Session[SessionName] = code;
            }
        }
        #endregion

        #region GetValidateCode 获取Session中的验证码
        /// <summary>
        ///  获取Session中的验证码
        /// </summary>
        /// <returns></returns>
        public static string GetValidateCode()
        {
            if (HttpContext.Current.Session[SessionName] == null)
            {
                return string.Empty;
            }
            return HttpContext.Current.Session[SessionName].ToString().ToLower();
        }
        #endregion
    }
}
