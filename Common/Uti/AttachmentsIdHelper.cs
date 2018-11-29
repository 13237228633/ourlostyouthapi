using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace Common
{
    public class AttachmentsIdHelper
    {
        public static string InsertAttachValue(string ids, int id)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                string[] array_ids = ids.Split(',');
                bool isexist = false;
                foreach (string temp_id in array_ids)
                {
                    int i_temp_id;
                    if (int.TryParse(temp_id, out i_temp_id))
                    {
                        if (i_temp_id == id)
                        {
                            isexist = true;
                        }
                    }
                }

                if (!isexist)
                {
                    return ids += ("," + id.ToString());
                }
            }
            else
            {
                return ids += (id.ToString());
            }
            return ids;
        }

        public static string DeleteAttachValue(string ids, int id)
        {
            StringBuilder res_ids = new StringBuilder();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] array_ids = ids.Split(',');

                foreach (string temp_id in array_ids)
                {
                    int i_temp_id;
                    if (int.TryParse(temp_id.Trim(), out i_temp_id))
                    {
                        if (i_temp_id != id)
                        {
                            res_ids.Append(i_temp_id + ",");
                        }
                    }
                }
            }

            string temp_res = res_ids.ToString();

            if (temp_res.Length > 0)
            {
                return temp_res.Substring(0, temp_res.Length - 1);
            }
            return string.Empty;
        }

        public static void DelteAttachById(int id, HttpContext context)
        {

           // Dictionary<string, object> param = new Dictionary<string, object>();
           // param.Add("Id", id);
           //// SIMAttachment attach = SIMBusiness.AttachmentManager.GetAttachmentById(param);

           // if (null != attach)
           // {
           //     string filepath = attach.FilePath;
           //     string filefullname = context.Server.MapPath(filepath);

           //     if (File.Exists(filefullname))
           //     {
           //         File.Delete(filefullname);
           //     }
           // }
        }
    }
}