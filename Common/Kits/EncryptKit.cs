namespace Common
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System;
    
    /// <summary>
    ///  加密工具
    /// </summary>
    public static class EncryptKit
    {
        #region string ToLowerMd5(string value) MD5加密(小写)
        /// <summary>
        ///  MD5加密(小写)
        /// </summary>
        /// <param name="value">被加密字符串</param>
        /// <returns></returns>
        public static string ToLowerMd5(string value)
        {
            byte[] bytes = Encoding.Default.GetBytes(value);
            bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            StringBuilder ciphertext = new StringBuilder(bytes.Length * 2);
            foreach (byte data in bytes)
            {
                ciphertext.Append(data.ToString("x").PadLeft(2, '0'));
            }
            return ciphertext.ToString();
        }
        #endregion

        #region string ToUpperMd5(string value) MD5加密(大写)
        /// <summary>
        ///  MD5加密(大写)
        /// </summary>
        /// <param name="value">被加密字符串</param>
        /// <returns></returns>
        public static string ToUpperMd5(string value)
        {
            byte[] bytes = Encoding.Default.GetBytes(value);
            bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            StringBuilder ciphertext = new StringBuilder(bytes.Length * 2);
            foreach (byte data in bytes)
            {
                ciphertext.Append(data.ToString("X").PadLeft(2, '0'));
            }
            return ciphertext.ToString();
        }
        #endregion

        #region ToSHA256 SHA256加密
        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="targetString">原始字符串</param>
        /// <returns>加密后字符串</returns>
        public static string ToSHA256(string targetString)
        {
            byte[] sha256Bytes = Encoding.UTF8.GetBytes(targetString);
            SHA256Managed sha256 = new SHA256Managed();
            byte[] result = sha256.ComputeHash(sha256Bytes);
            return Convert.ToBase64String(result);  //返回长度为44字节的字符串
        }
        #endregion

        #region Base64 加解密
        /// <summary>
        /// 进行 Base64 加密
        /// </summary>
        /// <param name="value">被加密字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string value)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(value));
        }

        /// <summary>
        /// 进行 Base64 解密
        /// </summary>
        /// <param name="value">被解密字符串</param>
        /// <returns></returns>
        public static string DecodeBase64(string value)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(value));
        }
        #endregion
    }

    /// <summary> 
    /// 加密
    /// </summary> 
    public class AES
    {
        //默认密钥向量
        private static readonly byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = StringKit.CutString(encryptKey, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged
                                                   {
                                                       Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32)),
                                                       IV = Keys
                                                   };
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = StringKit.CutString(decryptKey, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged
                                                       {
                                                           Key = Encoding.UTF8.GetBytes(decryptKey),
                                                           IV = Keys
                                                       };
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return "";
            }

        }

    }

    /// <summary> 
    /// 加密
    /// </summary> 
    public class DES
    {
        //默认密钥向量
        private static readonly byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = StringKit.CutString(encryptKey, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIv = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCsp = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCsp.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = StringKit.CutString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIv = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }
    } 
}
