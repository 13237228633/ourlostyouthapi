
using Aliyun.OSS;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
{
   public static  class PutObjectSample
    {       
        //const string accessKeyId = "LTAIhXas5wok5GBj";
        //const string accessKeySecret = "QU8PbG2FTRjeXngvERFX4q8zlKTgJQ";
        //const string endpoint = "http://oss-cn-shanghai.aliyuncs.com";
        //const string key = "tupian";

       const string accessKeyId = "r4dmSZEB6v3MqZeQ1EMSIVlVUDWrB5TfxOpArYxj";
        const string accessKeySecret = "-enk3KFq9lDUOYGMIeW-4D2omkdblOC_j8yHs2Sk";

        //public static void set(string File,string FileName)
        //{
        //    // 初始化OssClient
        //    var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
        //    //try
        //    //{
        //    //    string fileToUpload = "d:\\Desktop\\Myhome_prj\\Myhome_prj\\Myhome\\UploadImgs\\201707155302688_1.jpg";
        //    //    client.PutObject("myhome20170320", "tupian/女帝.jpg", fileToUpload);
        //    //    Console.WriteLine("图片上传成功！");
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine("Put object failed, {0}", ex.Message);
        //    //}

        //       System.IO.FileStream fs = new FileStream(File, FileMode.Open);

        //        var metadata = new ObjectMetadata();
        //        metadata.UserMetadata.Add("name", "my-data");
        //        metadata.ContentLength = fs.Length;
        //        client.PutObject("myhome20170320", "tupian/"+FileName, fs, metadata);
        //        Console.WriteLine("Put object succeeded");

        //        Entity.Album a = new Entity.Album();
        //        a.AlnumName = FileName;
        //        a.AlnumUrl = "http://myhome20170320.oss-cn-shanghai.aliyuncs.com/tupian/" +FileName;
        //        DALAlbum.upPhoto(a);
        //}


        public static HttpResult set(string File, string FileName)
       {
           ZoneID zoneId = ZoneID.CN_South;
           Qiniu.Common.Config.SetZone(zoneId, true);
            // 初始化OssClient
            //var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            ////try
            //{
            //    string fileToUpload = "d:\\Desktop\\Myhome_prj\\Myhome_prj\\Myhome\\UploadImgs\\201707155302688_1.jpg";
            //    client.PutObject("myhome20170320", "tupian/女帝.jpg", fileToUpload);
            //    Console.WriteLine("图片上传成功！");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Put object failed, {0}", ex.Message);
            //}

          /// <summary>
        /// 简单上传-上传小文件
        /// </summary>

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(accessKeyId,accessKeySecret);
            string bucket = "myhome1314";
            string saveKey = FileName;
            string localFile = File;
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            UploadManager um = new UploadManager();
            HttpResult result = um.UploadFile(localFile, saveKey, token);
            if (result.Text != "{\"error\":\"no such bucket\"}") 
            {
                Entity.Album a = new Entity.Album();
                a.AlnumName = FileName;
                a.AlnumUrl = "http://osmfge71u.bkt.clouddn.com/" + FileName;
                DALAlbum.upPhoto(a);
            }
            return result;
        }
    }
}
