using System.Text.RegularExpressions;
using System.Web;
//using System.Web.Mvc;


namespace Common.Web
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Linq.Expressions;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public static class HtmlHelperExtension
    {

        #region Checkbox
        /// <summary>
        ///  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        public static string CheckBox(string name, bool isChecked)
        {
            string html = "<input type=\"checkbox\" name=\"" + name + "\"" + (isChecked ? " checked=\"checked\"" : string.Empty) + " value=\"true\"/>";
            html += "<input type=\"hidden\" name=\"" + name + "\" value=\"false\"/>";
            return html;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isChecked"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string CheckBox(string name, bool isChecked, string attribute)
        {
            string html = "<input type=\"checkbox\" name=\"" + name + "\"" +
                          (isChecked ? " checked=\"checked\"" : string.Empty) + " value=\"true\" " + attribute + "/>";
            html += "<input type=\"hidden\" name=\"" + name + "\" value=\"false\"/>";
            return html;
        }
        #endregion

        #region Select

        public static string Select(string name, IDictionary<string, string> dataSource)
        {
            return Select(name, dataSource, string.Empty, string.Empty);
        }
        public static string Select(string name, IDictionary<string, string> dataSource, string defaultValue)
        {
            return Select(name, dataSource, defaultValue, string.Empty);
        }

        public static string Select(string name, IDictionary<string, string> dataSource, string defaultValue, string attrubutes, string disabled)
        {
            var sbHtml = new StringBuilder(100);
            sbHtml.AppendFormat("<select  disabled='disabled' id=\"{0}\" name=\"{1}\" {2}>", name.Replace('.', '_'), name, attrubutes);
            foreach (KeyValuePair<string, string> ke in dataSource)
            {
                sbHtml.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", ke.Key, ke.Key == defaultValue ? "selected=\"selected\"" : string.Empty, ke.Value);
            }
            sbHtml.Append("</select>");
            return sbHtml.ToString();
        }

        public static string Select(string name, IDictionary<string, string> dataSource, string defaultValue, string attrubutes)
        {
            var sbHtml = new StringBuilder(100);
            sbHtml.AppendFormat("<select id=\"{0}\" lay-verify=\"{1}\" lay-filter=\"{1}\" name=\"{1}\" {2}>", name.Replace('.', '_'), name, attrubutes);
            foreach (KeyValuePair<string, string> ke in dataSource)
            {
                sbHtml.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", ke.Key, ke.Key == defaultValue ? "selected=\"selected\"" : string.Empty, ke.Value);
            }
            sbHtml.Append("</select>");
            return sbHtml.ToString();
        }



        public static string Select(string name, IDictionary<string, string> dataSource, object defaultValue, bool defaultOption)
        {
            StringBuilder sbHtml = new StringBuilder(100);
            sbHtml.AppendFormat("<select id=\"{0}\" name=\"{1}\">", name.Replace('.', '_'), name);
            if (defaultOption)
            {
                sbHtml.Append("<option value=\"-1\">-请选择-</option>");
            }
            foreach (KeyValuePair<string, string> ke in dataSource)
            {
                sbHtml.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", ke.Key, ke.Key == defaultValue ? "selected=\"selected\"" : string.Empty, ke.Value);
                //sbHtml.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", ke.Key, ke.Key == (defaultValue != null ? defaultValue.ToString() : string.Empty) ? "selected=\"selected\"" : string.Empty, ke.Value);
            }
            sbHtml.Append("</select>");
            return sbHtml.ToString();
        }

        public static string Select<TKey, TValue>(string name, IDictionary<TKey, TValue> dataSource, TKey defaultValue, string attrubutes)
        {
            StringBuilder sbHtml = new StringBuilder(100);
            sbHtml.AppendFormat("<select id=\"{0}\" name=\"{1}\" {2}>", name.Replace('.', '_'), name, attrubutes);
            sbHtml.Append("<option value=\"\">-请选择-</option>");
            foreach (KeyValuePair<TKey, TValue> ke in dataSource)
            {
                sbHtml.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", ke.Key, ke.Key.Equals(defaultValue) ? "selected=\"selected\"" : string.Empty, ke.Value);
            }
            sbHtml.Append("</select>");
            return sbHtml.ToString();
        }

        public static string Select(string name, DataTable dataSource, string valueKey, string textKey, string defaultValue, string attrubutes)
        {
            string html = string.Empty;
            if (dataSource != null && dataSource.Rows.Count > 0)
            {
                StringBuilder sbHtml = new StringBuilder(100);
                sbHtml.AppendFormat("<select id=\"{0}\" name=\"{0}\" {1}>", name, attrubutes);
                foreach (DataRow row in dataSource.Rows)
                {
                    string valueString = row[valueKey].ToString();
                    sbHtml.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", valueString, valueString == defaultValue ? "selected=\"selected\"" : string.Empty, row[textKey]);
                }
                sbHtml.Append("</select>");
                html = sbHtml.ToString();
            }
            return html;
        }
        #endregion

        #region CreateAttributes
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string CreateAttributes(object htmlAttributes)
        {
            if (htmlAttributes == null) return string.Empty;
            return TypeDescriptor.GetProperties(htmlAttributes).Cast<PropertyDescriptor>().Aggregate(string.Empty, (current, descriptor) => current + (" " + descriptor.Name + "=" + "\"" + descriptor.GetValue(htmlAttributes) + "\""));
        }
        #endregion



        #region AreaSelect
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public static string AreaSelect(string controlName)
        {
            string codeName = controlName + "Code";
            string nameName = controlName + "Name";
            const string tag = "<div><select name=\"area_province\" onchange=\"loadCity(this);\"><script type=\"text/javascript\">loadProvince();</script></select><input type=\"hidden\" name=\"{0}\" /><input type=\"hidden\" name=\"{1}\" /></div>";
            return string.Format(tag, codeName, nameName);
        }

        public static string AreaSelect(string controlName, string areaCode)
        {
            string codeName = controlName + "Code";
            string nameName = controlName + "Name";
            const string tag = "<div><select name=\"area_province\" onchange=\"loadCity(this);\"><script type=\"text/javascript\">loadProvince();</script></select><input type=\"hidden\" name=\"{0}\" value=\"{2}\" /><input type=\"hidden\" name=\"{1}\" /></div>";
            return string.Format(tag, codeName, nameName, areaCode);
        }
        #endregion

        #region DaterText
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controlName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string DaterText(string controlName, object htmlAttributes)
        {
            const string daterTextTag = "<input type=\"text\" style=\"width:80px;\" name=\"{0}\" id=\"{0}\" readonly=\"readonly\" {1}><script>$(function(){{$(\"input[name='{0}']\").datepicker();}});</script>";
            return string.Format(daterTextTag, controlName, CreateAttributes(htmlAttributes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public static string DaterText(string controlName)
        {
            const string daterTextTag = "<input type=\"text\" style=\"width:80px;\"  name=\"{0}\" id=\"{0}\" readonly=\"readonly\"><script>$(function(){{$(\"input[name='{0}']\").datepicker();}});</script>";
            return string.Format(daterTextTag, controlName);
        }
        #endregion

        #region Flash
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="flashUrl"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string Flash(string flashUrl, int width, int height)
        {
            var sbHtml = new StringBuilder();
            sbHtml.AppendFormat("<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0\" width=\"{0}\" height=\"{1}\">", width, height);
            sbHtml.AppendFormat("<param name=\"movie\" value=\"{0}\">", flashUrl);
            sbHtml.Append("<param name=\"quality\" value=\"high\">");
            sbHtml.Append("<param name=\"wmode\" value=\"transparent\">");
            sbHtml.AppendFormat("<embed src=\"{0}\" width=\"{1}\" height=\"{2}\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" wmode=\"transparent\"></embed>", flashUrl, width, height);
            sbHtml.Append("</object>");
            return sbHtml.ToString();
        }
        #endregion

        #region QQLink
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string QQLink(string number)
        {
            const string tag = "<a target=\"_blank\" href=\"http://wpa.qq.com/msgrd?v=3&uin={0}&site=qq&menu=yes\"><img border=\"0\" src=\"/images/public/qq.gif\" alt=\"点击这里给我发消息\" title=\"点击这里给我发消息\"></a>";
            return string.Format(tag, number);
        }
        #endregion

        #region MapLoader

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="mapCode">地图ID</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="innnerHtml">需要在弹出层中显示的内容，可以不填</param>
        /// <returns></returns>
        public static string MapLoader(string mapCode, int width, int height, string innnerHtml)
        {
            mapCode = mapCode ?? string.Empty;
            var id = Guid.NewGuid();
            const string tag = "<div id=\"{0}\" style=\"width:{1}px;height:{2}px\"></div>";
            var mapCodeArray = mapCode.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var html = string.Format(tag, id, width, height);
            if (mapCodeArray.Length != 2)
            {
                return html;
            }
            if (!string.IsNullOrEmpty(innnerHtml)) innnerHtml = innnerHtml.Replace("'", "");
            var firstCode = mapCodeArray[0];
            var secondCode = mapCodeArray[1];
            html += string.Format("<script type=\"text/javascript\">$(function(){{ initialize('{0}','{1}','{2}','{3}'); }});</script>", id, firstCode, secondCode, innnerHtml);
            return html;
        }
        #endregion




        #region MonthSelect

        public static string MonthSelect(string name, int? value, object attributes)
        {

            var htmlBuilder = new StringBuilder();
            value = DateTime.Now.Month;
            htmlBuilder.AppendFormat("<div class='layui-input-inline_Time'><select lay-filter=\"{0}\"  name=\"{0}\" {1}>", name, attributes);
            htmlBuilder.AppendFormat("<option value=\"0\">全年</option>");
            for (int i = 1; i < 13; i++)
            {
                htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{0}月</option>", i,
                                         i == value ? " selected=\"selected\"" : string.Empty);
            }
            htmlBuilder.Append("</select></div>");
            return htmlBuilder.ToString();
        }

        public static string MonthSelect(string name, int? value)
        {
            return MonthSelect(name, value, null);
        }


        #endregion

        #region YearSelect

        public static string YearSelect(string name, int? value, List<int> dataSource, object htmlAttributes)
        {
            string attributes = CreateAttributes(htmlAttributes);
            var htmlBuilder = new StringBuilder();
            dataSource = dataSource ?? new List<int>();
            if (dataSource.Count > 0)
            {
                htmlBuilder.AppendFormat("<select name=\"{0}\" {1}>", name, attributes);
                foreach (var item in dataSource)
                {
                    htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{0}年</option>", item, value == item ? " selected=\"selected\"" : string.Empty);
                }
                htmlBuilder.AppendFormat("</select>");
            }
            return htmlBuilder.ToString();
        }

        public static string YearSelect(string name, int? value, List<int> dataSource)
        {
            return YearSelect(name, value, dataSource, null);
        }
        public static string YearSelect(string name, int? value)
        {
            List<int> yearData = new List<int>();
            for (int i = 2005; i < 2020; i++)
            {
                yearData.Add(i);
            }

            var htmlBuilder = new StringBuilder();
            if (yearData.Count > 0)
            {
                htmlBuilder.AppendFormat("<select name=\"{0}\" >", name);
                htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{2}年</option>", 0, value == 0 ? " selected=\"selected\"" : string.Empty, "请选择");
                foreach (var item in yearData)
                {
                    htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{0}年</option>", item, value == item ? " selected=\"selected\"" : string.Empty);
                }
                htmlBuilder.AppendFormat("</select>");
            }
            return htmlBuilder.ToString();
        }
        public static string YearSelect(string name, int? value, object attributes)
        {

            List<int> yearData = new List<int>();
            for (int i = 2011; i < 2022; i++)
            {
                yearData.Add(i);
            }

            value = DateTime.Now.Year;

            var htmlBuilder = new StringBuilder();
            if (yearData.Count > 0)
            {
                htmlBuilder.AppendFormat("<div class='layui-input-inline_Time'><select lay-filter=\"{0}\" name=\"{0}\" {1} >", name, attributes);
                htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{2}年</option>", 0, value == 0 ? " selected=\"selected\"" : string.Empty, "请选择");
                foreach (var item in yearData)
                {
                    htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{0}年</option>", item, value == item ? " selected=\"selected\"" : string.Empty);
                }
                htmlBuilder.AppendFormat("</select></div>");
            }
            return htmlBuilder.ToString();
        }


        #endregion

        public static string ItemSelectAll()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<div class='layui-input-inline_nonelabel'><select   lay-filter='SItemId' lay-verify='SItemId' name='SItemId' lay-search=''><option  value='0'>选择产品</option></select></div>");
            htmlBuilder.Append("<div class='layui-input-inline_nonelabel'><select  lay-filter='SUid'    lay-verify='SUid' name='SUid' lay-search=''><option value='0' >选择联盟</option></select></div>");
            htmlBuilder.Append("<div class='layui-input-inline_nonelabel'><select  lay-filter='SSubId'  lay-verify='SSubId' name='SSubId'  lay-search=''><option value='0'>选择渠道</option></select></div>");
            return htmlBuilder.ToString();
        }
        public static string ItemSelectAllS()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<div class='layui-input-inline_nonelabel'><select onclick='loadChannel(this);' style='width:200px' id='SItemId' name='SItemId' lay-search=''><option  value='0'>选择产品</option></select><div>");
            htmlBuilder.Append("<div class='layui-input-inline_nonelabel'><select onclick='loadSubChannelS(this);'style='width:100px'   name='SUid' lay-search=''><option value='0' lay-search>选择联盟</option></select><div>");
            htmlBuilder.Append("<div class='layui-input-inline_nonelabel'><select onclick='checkSubChannel(this);'style='width:120px'   name='SSubId' lay-search=''><option value='0' lay-search>选择渠道</option></select><div>");
            return htmlBuilder.ToString();
        }

        //推广金币管理搜索
        public static string AmountSelectAll()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<select onclick='loadChannel(this);' style='width:200px'  name='SItemId' lay-search=''><option  value='0' >选择产品</option></select>&nbsp;&nbsp;&nbsp;");
            htmlBuilder.Append("<select onclick='loadSubChannel(this);'style='width:100px'   name='SUid' lay-search=''><option value='0'>选择联盟</option></select>&nbsp;&nbsp;&nbsp;");
            htmlBuilder.Append("<select onclick='checkSubChannel(this);'style='width:120px'   name='SSubId' lay-search=''><option value='0'>选择渠道</option></select>&nbsp;&nbsp;&nbsp;");
            htmlBuilder.Append("<select onclick='checkGradeChannel(this);'style='width:120px'   name='SChannelId' lay-search=''><option value='0'>渠道评级</option></select>&nbsp;&nbsp;&nbsp;");
            return htmlBuilder.ToString();
        }
        public static string XiangliaoItemSelectAll()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<select onclick='loadChannel(this);' name='SItemId' disabled='disabled'><option  value='2' >想聊（xliao）</option></select>&nbsp;&nbsp;&nbsp;");
            htmlBuilder.Append("<select onclick='loadSubChannel(this);' name='SUid' disabled='disabled'><option value='270'>背景图片</option></select>&nbsp;&nbsp;&nbsp;");
            htmlBuilder.Append("<select onclick='checkSubChannel(this);' name='SSubId' disabled='disabled' ><option value='13'>背景图片（J021）</option></select>&nbsp;&nbsp;&nbsp;");
            return htmlBuilder.ToString();
        }
        public static string ItemSelect()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<select  lay-filter='SItemId' id='SItemId' name='SItemId'><option  value='0'>选择产品</option></select>");
            //htmlBuilder.Append("<select onclick='loadChannel(this);' id='SItemId' style='width:200px' name='SItemId'><option  value='0'  >选择产品</option></select>&nbsp;&nbsp;&nbsp;");
            return htmlBuilder.ToString();
        }
        public static string ItemSelectA()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<select onclick='loadChannel(this);'  lay-verify=\"SItemId\" id='SItemId' style='width:200px' name='SItemId'><option  value='0'  lay-search>选择产品</option></select>&nbsp;&nbsp;&nbsp;");
            return htmlBuilder.ToString();
        }

        //移动端产品
        public static string mobileItemSelect()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("产品名称<div class='layui-input-inline_nonelabel'><select onclick='loadChannel(this);' id='SItemId' style='width:200px' name='SItemId'><option  value='0'  lay-search>选择产品</option></select>&nbsp;&nbsp;&nbsp;</div>");
            return htmlBuilder.ToString();
        }

        public static string ChannelSelect()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<select lay-filter='SUid' id='SUid' name='SUid'><option value='0' lay-search>选择联盟</option></select>");
            return htmlBuilder.ToString();
        }

        public static string SubChannelSelect()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<select lay-filter='SSubId' id='SSubId'  name='SSubId' ><option value='0' lay-search>选择渠道</option></select>");
            return htmlBuilder.ToString();
        }


        //推广预算产品
        #region 推广预算产品
        public static object ItemSelect_PB()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<select onclick='loadSubChannel(this);' lay-filter='SItemId' lay-verify='SItemId' id='SItemId' style='width:200px' name='SItemId'><option  value='0'  >选择产品</option></select>");
            return htmlBuilder.ToString();
        }

        public static object SubChannelSelect_PB()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<select onclick='checkSubChannel(this);' lay-filter='SSubId' lay-verify='SSubId'  id='SSubId'  name='SSubId' ><option value='0'>选择渠道</option></select>");
            return htmlBuilder.ToString();
        }
        #endregion


        public static string OnlyItemSelect()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<select id='SItemId' style='width:200px' name='SItemId'><option  value='0'  >选择产品</option></select>");
            return htmlBuilder.ToString();
        }

        /// <summary>
        /// 常用渠道的选择
        /// </summary>
        /// <returns></returns>
        public static string CommItemSelect()
        {
            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<div class='layui-input-inline_nonelabel'><select  lay-filter='CommSItemId'  lay-verify='CommSItemId'  style='width:200px'  name='CommSItemId'><option  value='0'>选择产品</option></select></div>");
            htmlBuilder.Append("<div class='layui-input-inline_nonelabel'><select onclick='loadCommSubChannel(this);' style='width:200px'  name='CommSSubId' ><option  value='0'>选择渠道</option></select></div>");

            return htmlBuilder.ToString();
        }


        public static string NumtypeSelectAll()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append(@"<select  name='Numtype'>
                                    <option  value='1' >登陆</option>
                                    <option  value='2' >进房三次</option>
                                    <option  value='3' >作品用户</option>
                                 </select>&nbsp;&nbsp;&nbsp;");
            return htmlBuilder.ToString();
        }

        public static string DatatypeSelectAll()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append(@"<select  name='Datatype'>
                                    <option  value='1' >数值</option>
                                    <option  value='2' >比列</option>
                                 </select>&nbsp;&nbsp;&nbsp;");
            return htmlBuilder.ToString();
        }

        public static string TimeHourSelect(string name)
        {

            var htmlBuilder = new StringBuilder();
            int value = DateTime.Now.Hour;
            htmlBuilder.AppendFormat("<select name=\"{0}\">", name);
            for (int i = 1; i < 25; i++)
            {
                htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{0}点</option>", i,
                                         i == value ? " selected=\"selected\"" : string.Empty);
            }
            htmlBuilder.Append("</select>");
            return htmlBuilder.ToString();
        }

        public static string TimeHourSelect(string name, string value)
        {

            var htmlBuilder = new StringBuilder();

            htmlBuilder.AppendFormat("<select name=\"{0}\">", name);
            for (int i = 0; i < 25; i++)
            {
                htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{0}点</option>", i,
                                         i.ToString() == value ? " selected=\"selected\"" : string.Empty);
            }
            htmlBuilder.Append("</select>");
            return htmlBuilder.ToString();
        }


        public static string TimeminuteSelect(string name)
        {

            var htmlBuilder = new StringBuilder();
            htmlBuilder.AppendFormat("<select name=\"{0}\">", name);
            int value = DateTime.Now.Minute;
            for (int i = 0; i < 61; i++)
            {
                htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{0}分钟</option>", i,
                                         i == value ? " selected=\"selected\"" : string.Empty);
            }
            htmlBuilder.Append("</select>");
            return htmlBuilder.ToString();
        }


        public static string TimeminuteSelect(string name, string value)
        {

            var htmlBuilder = new StringBuilder();
            htmlBuilder.AppendFormat("<select name=\"{0}\">", name);
            for (int i = 0; i < 60; i++)
            {
                htmlBuilder.AppendFormat("<option value=\"{0}\"{1}>{0}分钟</option>", i,
                                         i.ToString() == value ? " selected=\"selected\"" : string.Empty);
            }
            htmlBuilder.Append("</select>");
            return htmlBuilder.ToString();
        }


        public static string APPSelectAll(string Devicetype)
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append(@"<select  name='Datatype' onchange>");
            htmlBuilder.AppendFormat("<option  value='1' {0}>安卓</option>",
                                        Devicetype == "1" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='2' {0}>IOS</option>",
                                        Devicetype == "2" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("</select>&nbsp;&nbsp;&nbsp;");
            return htmlBuilder.ToString();
        }


        public static string CapticalRateAll(string Devicetype)
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append(@"<select  name='CapticalRate' onchange>");
            htmlBuilder.AppendFormat("<option  value='黑龙江' {0}>黑龙江</option>",
                                        Devicetype == "黑龙江" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='吉林' {0}>吉林</option>",
                                        Devicetype == "吉林" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='辽宁' {0}>辽宁</option>",
                                       Devicetype == "辽宁" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='河北' {0}>河北</option>",
                                 Devicetype == "河北" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='山西' {0}>山西</option>",
                               Devicetype == "山西" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='陕西' {0}>陕西</option>",
                                   Devicetype == "陕西" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='山东' {0}>山东</option>",
                                   Devicetype == "山东" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='青海' {0}>青海</option>",
                          Devicetype == "青海" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='甘肃' {0}>甘肃</option>",
                                   Devicetype == "甘肃" ? " selected=\"selected\"" : string.Empty);

            htmlBuilder.AppendFormat("<option  value='宁夏' {0}>宁夏</option>",
                       Devicetype == "宁夏" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='河南' {0}>河南</option>",
                                   Devicetype == "河南" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='江苏' {0}>江苏</option>",
                 Devicetype == "江苏" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='湖北' {0}>湖北</option>",
                                   Devicetype == "湖北" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='浙江' {0}>浙江</option>",
                                 Devicetype == "浙江" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='安徽' {0}>安徽</option>",
                             Devicetype == "安徽" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='福建' {0}>福建</option>",
                         Devicetype == "福建" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='江西' {0}>江西</option>",
                 Devicetype == "江西" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='湖南' {0}>湖南</option>",
                Devicetype == "湖南" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='贵州' {0}>贵州</option>",
             Devicetype == "贵州" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='四川' {0}>四川</option>",
                Devicetype == "四川" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='广东' {0}>广东</option>",
                Devicetype == "广东" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='云南' {0}>云南</option>",
                Devicetype == "云南" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='海南' {0}>海南</option>",
                Devicetype == "海南" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='北京' {0}>北京</option>",
                Devicetype == "北京" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='上海' {0}>上海</option>",
                Devicetype == "上海" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='天津' {0}>天津</option>",
                Devicetype == "天津" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='重庆' {0}>重庆</option>",
                Devicetype == "重庆" ? " selected=\"selected\"" : string.Empty);

            htmlBuilder.AppendFormat("<option  value='内蒙古' {0}>内蒙古</option>",
                Devicetype == "内蒙古" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='新疆' {0}>新疆</option>",
                Devicetype == "新疆" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='西藏' {0}>西藏</option>",
                Devicetype == "西藏" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='宁夏' {0}>宁夏</option>",
                 Devicetype == "宁夏" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='广西' {0}>广西</option>",
                Devicetype == "广西" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='香港' {0}>香港</option>",
                Devicetype == "香港" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='澳门' {0}>澳门</option>",
                Devicetype == "澳门" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("<option  value='台湾' {0}>台湾</option>",
               Devicetype == "台湾" ? " selected=\"selected\"" : string.Empty);
            htmlBuilder.AppendFormat("</select>&nbsp;&nbsp;&nbsp;");
            return htmlBuilder.ToString();
        }


        /// <summary>
        /// 日志内容
        /// </summary>
        /// <param name="strMessage"></param>
        public static void WriteTextLog(string strMessage)
        {

            //取到当前的日期为日志名称
            string fileName = DateTime.Now.ToString("yyyyMMdd") + "Log.txt";
            string path = AppDomain.CurrentDomain.BaseDirectory + @"SystemLog\";
            path = path.Substring(0, path.IndexOf(@"\")) + "\\tiangeLog\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + fileName;
            StringBuilder str = new StringBuilder();
            str.Append("" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "" + strMessage + "\r\n");
            StreamWriter sw;
            if (!System.IO.File.Exists(fileFullPath))
            {
                sw = System.IO.File.CreateText(fileFullPath);
            }
            else
            {
                sw = System.IO.File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
    }


}



