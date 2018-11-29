namespace Common.Kits
{
    using System.Drawing;
    using System.IO;

    public static class ImageKit
    {
        #region BytesToImage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] datas)
        {
            var stream = new MemoryStream(datas);
            return Image.FromStream(stream);
        }

        #endregion

    }
}
