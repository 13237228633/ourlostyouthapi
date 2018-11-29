using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

//using NPOI.HSSF.UserModel;
//using NPOI.HPSF;
//using NPOI.HSSF.Util;
//using NPOI.HSSF.UserModel.Contrib;
//using NPOI.HSSF.Model;

namespace Common
{
    #region Formatter

    /// <summary>
    /// JQGrid格式化器接口
    /// </summary>
    public interface IJqGridFormatter
    {
        /// <summary>
        /// 转为JavaScript代码
        /// </summary>
        /// <returns></returns>
        string ToString();
    }

    #endregion

    /// <summary>
    /// JQGrid列类
    /// </summary>
    public class JqGridColumn
    {
        /// <summary>
        /// 列名称。如果没有指定，自动用Name属性代替。
        /// </summary>
        public string ColumnHeader { get; set; }
        /// <summary>
        /// CSS class，多个可用空格分隔开
        /// </summary>
        public string CssClass { get; set; }
        /// <summary>
        /// Column的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 索引。用于排序。
        /// </summary>
        public string Index { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public string Width { get; set; }
        /// <summary>
        /// 是否为主键列
        /// </summary>
        public bool Key { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hidden { get; set; }
        /// <summary>
        /// 对齐方式
        /// </summary>
        public JqGridColumnAlign Align { get; set; }
        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool Editable { get; set; }
        /// <summary>
        /// 编辑的类型
        /// </summary>
        public JqGridEditType Edittype { get; set; }
        /// <summary>
        /// 编辑的详细配置
        /// </summary>
        public JqGridEditOptions Editoptions { get; set; }
        /// <summary>
        /// 数据格式化配置
        /// </summary>
        public IJqGridFormatter Formatter { get; set; }

        /// <summary>
        /// 验证规则
        /// </summary>
        public JQGridEditRule EditRule { get; set; }

        /// <summary>
        /// 是否可以排序
        /// </summary>
        public bool Sortable { get; set; }

        /// <summary>
        /// 返回JavaScript代码。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder txt = new StringBuilder();
            if (!string.IsNullOrEmpty(Name))
                txt.Append("name:'" + Name + "',");
            if (!string.IsNullOrEmpty(Index))
                txt.Append("index:'" + Index + "',");
            if (!string.IsNullOrEmpty(Width))
                txt.Append("width:" + Width + ",");

            txt.Append("sortable:" + Sortable.ToString().ToLower() + ",");

            if (!string.IsNullOrEmpty(CssClass))
                txt.Append("classes:'" + CssClass + "',");
            if (Key)
                txt.Append("key:" + Key.ToString().ToLower() + ",");
            if (Hidden)
                txt.Append("hidden:" + Hidden.ToString().ToLower() + ",");
            if (Align != JqGridColumnAlign.notSet)
                txt.Append("align:'" + Align + "',");
            if (Editable)
                txt.Append("editable:" + Editable.ToString().ToLower() + ",");
            if (Edittype != JqGridEditType.notSet)
                txt.Append("edittype:'" + Edittype + "',");
            if (Editoptions != null && !string.IsNullOrEmpty(Editoptions.ToString()))
                txt.Append("editoptions:" + Editoptions + ",");

            if (EditRule != null)
            {
                txt.Append("editrules:" + EditRule.ToString() + ",");
            }

            if (Formatter != null)
            {
                txt.Append(Formatter.ToString());
                txt.Append(",");
            }


            if (txt.Length == 0)
                return "";
            txt.Remove(txt.Length - 1, 1);
            return "{" + txt.ToString() + "}";
        }
    }

    /// <summary>
    /// JQGrid子表模型类
    /// </summary>
    public class JqSubGridModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string[] name;

        /// <summary>
        /// 宽度
        /// </summary>
        public int[] width;

        /// <summary>
        /// 对其方式
        /// </summary>
        public string[] align;

        /// <summary>
        /// 参数
        /// </summary>
        public string[] Params;
    }

    /// <summary>
    /// JQGrid编辑框类型
    /// </summary>
    public enum JqGridEditType
    {
        notSet,
        select,
        checkbox,
        textarea,
        password,
        custom
    }

    /// <summary>
    /// JQGrid列对齐方式
    /// </summary>
    public enum JqGridColumnAlign
    {
        notSet = 0,
        center,
        left,
        right
    }

    /// <summary>
    /// JQGrid编辑选项
    /// </summary>
    public class JqGridEditOptions
    {
        /// <summary>
        /// 数据源地址，仅用于Select的编辑类型
        /// </summary>
        public string DataUrl { get; set; }
        /// <summary>
        /// 文本框的大小
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public string dataInit { get; set; }
        /// <summary>
        /// 数据事件
        /// </summary>
        public string dataEvents { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string defaultValue { get; set; }

        /// <summary>
        /// 自定义元素
        /// </summary>
        public string custom_element { get; set; }

        /// <summary>
        /// 自定义元素的值
        /// </summary>
        public string custom_value { get; set; }

        /// <summary>
        /// 返回JavaScript代码
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder txt = new StringBuilder();
            if (!string.IsNullOrEmpty(DataUrl))
            {
                txt.Append("dataUrl:'" + DataUrl + "',");
            }
            if (Size != null)
            {
                txt.Append("size:" + Size + ",");
            }
            if (!string.IsNullOrEmpty(dataEvents))
            {
                txt.Append("dataEvents:" + dataEvents + ",");
            }
            if (!string.IsNullOrEmpty(value))
            {
                txt.Append("value:" + value + ",");
            }
            if (!string.IsNullOrEmpty(dataInit))
            {
                txt.Append("dataInit:" + dataInit + ",");
            }

            if (!string.IsNullOrEmpty(defaultValue))
            {
                txt.Append("defaultValue:" + defaultValue + ",");
            }
            if (!string.IsNullOrEmpty(custom_element))
            {
                txt.Append("custom_element:" + custom_element + ",");
            }
            if (!string.IsNullOrEmpty(custom_value))
            {
                txt.Append("custom_value:" + custom_value + ",");
            }
            if (txt.Length == 0)
                return "";
            txt.Remove(txt.Length - 1, 1);
            return "{" + txt.ToString() + "}";
        }
    }

    /// <summary>
    /// JQGrid编辑规则
    /// </summary>
    public class JQGridEditRule
    {
        /// <summary>
        /// 是否可编辑隐藏域
        /// </summary>
        public bool edithidden { get; set; }

        /// <summary>
        /// 是否为必填项
        /// </summary>
        public bool required { get; set; }

        /// <summary>
        /// 是否为数字
        /// </summary>
        public bool number { get; set; }

        /// <summary>
        /// 是否为整数
        /// </summary>
        public bool integer { get; set; }

        /// <summary>
        /// 是否为URL
        /// </summary>
        public bool url { get; set; }

        /// <summary>
        /// 是否为Email地址
        /// </summary>
        public bool email { get; set; }

        /// <summary>
        /// 是否为日期
        /// </summary>
        public bool date { get; set; }

        /// <summary>
        /// 是否为时间
        /// </summary>
        public bool time { get; set; }

        /// <summary>
        /// 自定义规则
        /// </summary>
        public bool custom { get; set; }
        /// <summary>
        /// 自定义验证函数。（仅用于custom设置true时）
        /// </summary>
        public string custom_func { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int? minValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public int? maxValue { get; set; }

        /// <summary>
        /// 最小输入长度
        /// </summary>
        public int? minLength { get; set; }

        /// <summary>
        /// 最大输入长度
        /// </summary>
        public int? maxLength { get; set; }

        /// <summary>
        /// 转为JavaScript代码
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("{");
            if (edithidden)
            {
                sb.Append("edithidden:true,");
            }
            if (required)
            {
                sb.Append("required:true,");
            }
            if (number)
            {
                sb.Append("number:true,");
            }
            if (integer)
            {
                sb.Append("integer:true,");
            }
            if (url)
            {
                sb.Append("url:true,");
            }
            if (email)
            {
                sb.Append("email:true,");
            }
            if (date)
            {
                sb.Append("date:true,");
            }
            if (time)
            {
                sb.Append("time:true,");
            }
            if (custom)
            {
                sb.Append("custom:true,");
                if (!string.IsNullOrEmpty(custom_func))
                {
                    sb.AppendFormat("custom_func:{0},", custom_func);
                }
            }
            if (minValue.HasValue)
            {
                sb.AppendFormat("minValue:{0},", minValue.ToString());
            }

            if (maxValue.HasValue)
            {
                sb.AppendFormat("maxValue:{0},", maxValue.ToString());
            }

            if (minLength.HasValue)
            {
                sb.AppendFormat("minLength:{0},", minLength.ToString());
            }

            if (maxLength.HasValue)
            {
                sb.AppendFormat("maxLength:{0},", maxLength.ToString());
            }

            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// JQGrid选项类
    /// </summary>
    public class GridOptions
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GridOptions()
        {
            ColumnsModel = new List<JqGridColumn>();
            //Apply default values
            DataType = "json";
            RowNum = 999;
            Height = 200;
            //RowList = new int[] { 20, 40, 80 };
            AutoWidth = true;
            EmptyRecords = "无数据";
            PGButtons = true;
            ExportToExcel = true;
            CollapseButton = false;
            onDoubleClick = "function(id){var grid1=$(this);var gr = grid1.jqGrid('getGridParam', 'selrow');if (gr != null) grid1.jqGrid('editGridRow', gr, { reloadAfterSubmit: true, closeAfterEdit: true });else alert('请选择要编辑的数据行');}";
            ViewFormNavButtons = true;
        }

        /// <summary>
        /// 设置导航按钮是否可见
        /// </summary>
        public bool ViewFormNavButtons { get; set; }

        /// <summary>
        /// DoubleClick事件响应函数
        /// </summary>
        public string onDoubleClick { get; set; }

        /// <summary>
        /// 是否导出为Excel
        /// </summary>
        public bool ExportToExcel
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义列模型
        /// </summary>
        public IList<JqGridColumn> ColumnsModel
        {
            get;
            set;
        }

        /// <summary>
        /// 数据源URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 数据表的高度（不包括头部和分页部分）
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 数据类型，可以是json，xml
        /// </summary>
        [DefaultValue("json")]
        public string DataType { get; set; }
        /// <summary>
        /// 默认的Page Size
        /// </summary>
        [DefaultValue(20)]
        public int RowNum { get; set; }
        /// <summary>
        /// 可供选择的Page Size
        /// </summary>
        [DefaultValue(new int[] { 20, 40, 80 })]
        public int[] RowList { get; set; }

        private string Pager { get; set; }

        /// <summary>
        /// 初始排序列名
        /// </summary>
        public string SortName { get; set; }

        /// <summary>
        /// 是否显示记录总数
        /// </summary>
        [DefaultValue(true)]
        public bool ViewRecords { get; set; }
        /// <summary>
        /// 默认排序方式
        /// </summary>
        [DefaultValue("asc")]
        public string SortOrder { get; set; }

        /// <summary>
        /// Grid的标题文字
        /// </summary>
        public string Caption { get; set; }

        [DefaultValue(false)]
        public bool ForceFit { get; set; }

        /// <summary>
        /// 提交数据更改的地址
        /// </summary>
        public string EditUrl { get; set; }

        /// <summary>
        /// 整个Grid是否可以编辑
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// 是否显示编辑按钮
        /// </summary>
        public bool ShowEdit { get; set; }
        /// <summary>
        /// 是否显示添加按钮
        /// </summary>
        public bool ShowAdd { get; set; }
        /// <summary>
        /// 是否显示删除按钮
        /// </summary>
        public bool ShowDel { get; set; }
        /// <summary>
        /// 是否显示查找功能
        /// </summary>
        public bool ShowSearch { get; set; }
        /// <summary>
        /// 整个Grid是否可以进行排序
        /// </summary>
        public bool Sortable { get; set; }

        /// <summary>
        /// 自定义的Grid Complete 客户端事件，在Grid载入完成后调用。请注意避免方法名重复。
        /// </summary>
        public string AfterComplete { get; set; }

        /// <summary>
        /// 客户端事件，在Grid初始化后立即调用, 将得到GridID作为参数
        /// </summary>
        public string BeforeLoading { get; set; }

        /// <summary>
        /// 自定义编辑框
        /// </summary>
        public string customerEdit { get; set; }

        /// <summary>
        /// 自定义编辑框宽度
        /// </summary>
        public int customerEditWidth { get; set; }

        /// <summary>
        /// 设置JQGrid是否自动调整自己的宽度
        /// </summary>
        public bool AutoWidth { get; set; }

        /// <summary>
        /// 是否支持子表
        /// </summary>
        public bool SubGrid { get; set; }

        /// <summary>
        /// 字表URL
        /// </summary>
        public string SubGridUrl { get; set; }

        /// <summary>
        /// 字表模型
        /// </summary>
        public JqSubGridModel SubGridModel
        {
            get;
            set;
        }

        /// <summary>
        /// JQGrid工具栏
        /// </summary>
        public string toolbar { get; set; }

        /// <summary>
        /// 是否允许多选，如果=true将会有一个勾选框出现在左边
        /// </summary>
        public bool MultiSelect { get; set; }
        /// <summary>
        /// 是否仅用Checkbox作为选择框
        /// </summary>
        public bool MultiBoxOnly { get; set; }
        /// <summary>
        /// 是否在分页栏显示分页按钮
        /// </summary>
        public bool PGButtons { get; set; }

        /// <summary>
        /// 当无数据时显示的文本
        /// </summary>
        public string EmptyRecords { get; set; }

        /// <summary>
        /// 是否显示底部数据行（通常用于统计）
        /// </summary>
        public bool ShowFooterRow { get; set; }

        /// <summary>
        /// 是否每次Add和Edit都重新创建Form
        /// </summary>
        public bool RecreateForm { get; set; }

        /// <summary>
        /// 在显示新增、编辑Form之前调用的方法，得到FormID参数
        /// </summary>
        public string BeforeShowForm { get; set; }

        /// <summary>
        /// 在选定一行时的事件处理函数
        /// </summary>
        public string onSelectRow { get; set; }

        /// <summary>
        /// 当请求数据时，触发此事件
        /// </summary>
        public string BeforeRequest { get; set; }

        /// <summary>
        /// 当子表行展开时，触发此事件
        /// </summary>
        public string SubGridRowExpanded { get; set; }

        /// <summary>
        /// 设置是否显示收起/展开按钮
        /// </summary>
        public bool CollapseButton
        {
            get;
            set;
        }
    }


    /// <summary>
    /// 对象转换为JQGrid数据
    /// </summary>
    public class Object2GridData
    {
        /// <summary>
        /// 行数据生成函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="sb"></param>
        public delegate void RowGenHandler<T>(T item, JQGridDataRow sb);

        /// <summary>
        /// Footer行数据生成函数
        /// </summary>
        /// <param name="footerData"></param>
        public delegate void FooterGenHandler(JQGridFooterData footerData);

        /// <summary>
        /// 构造函数
        /// </summary>
        public Object2GridData()
        {

        }

        /// <summary>
        /// 空数据
        /// </summary>
        public static string EmptyDataString = "{\"total\":0,\"page\":1,\"records\":0,\"rows\":[]}";

        /// <summary>
        /// 转换为JQGrid数据。（无分页）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static string ToJQGridData<T>(IList<T> t, RowGenHandler<T> handler)
        {
            if (t == null)
                t = new List<T>();
            return ToJQGridData<T>(t, t.Count, 1, t.Count, handler, null);
        }

        /// <summary>
        /// 转换为JQGrid数据。（带Footer行）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="handler"></param>
        /// <param name="footerHandler"></param>
        /// <returns></returns>
        public static string ToJQGridData<T>(IList<T> t, RowGenHandler<T> handler, FooterGenHandler footerHandler)
        {
            if (t == null)
                t = new List<T>();
            return ToJQGridData<T>(t, t.Count, 1, t.Count, handler, footerHandler);
        }

        /// <summary>
        /// 转换为JQGrid数据。（有分页）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="totalRecordCount"></param>
        /// <param name="currentPageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static string ToJQGridData<T>(IList<T> list, int totalRecordCount, int currentPageNo, int pageSize, RowGenHandler<T> handler)
        {
            return ToJQGridData<T>(list, totalRecordCount, currentPageNo, pageSize, handler, null);
        }

        /// <summary>
        /// 转换为JQGrid数据。（有分页，带Footer行）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="totalRecordCount"></param>
        /// <param name="currentPageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="handler"></param>
        /// <param name="footerHandler"></param>
        /// <returns></returns>
        public static string ToJQGridData<T>(IList<T> list, int totalRecordCount, int currentPageNo, int pageSize, RowGenHandler<T> handler, FooterGenHandler footerHandler)
        {
            IList<T> t = list;
            if (t == null)
            {
                t = new List<T>();
            }
            JQGridData data = new JQGridData();
            if (pageSize == 0)
                pageSize = 10;
            if (totalRecordCount == 0)
            {
                data.total = 0;
            }
            else
            {
                data.total = (int)Math.Ceiling((decimal)totalRecordCount / pageSize);
            }
            data.page = currentPageNo;

            data.records = totalRecordCount;
            data.rows = new List<JQGridDataRow>();
            foreach (T item in t)
            {
                JQGridDataRow row = new JQGridDataRow();
                handler(item, row);
                data.rows.Add(row);
            }
            if (footerHandler != null)
            {
                //JQGridFooterData footerData = new JQGridFooterData();
                //footerHandler(footerData);
                //StringBuilder rst = new StringBuilder(ObjectToJson(data));
                //rst.Remove(rst.Length - 1, 1);
                //rst.Append(",footer:");
                //rst.Append(footerData.ToString());
                //rst.Append("}");
                //return rst.ToString();


                JQGridFooterData footerData = new JQGridFooterData();
                footerHandler(footerData);
                StringBuilder rst = new StringBuilder(ObjectToJson(data));
                rst.Remove(rst.Length - 1, 1);
                rst.Append(",\"userdata\":");
                rst.Append(footerData.ToString());
                rst.Append("}");
                return rst.ToString();
            }
            else
            {
                return ObjectToJson(data);
            }
        }

        /// <summary>
        /// 字典数据处理函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="row"></param>
        public static void DictionaryHanlder(Dictionary<string, string> item, JQGridDataRow row)
        {
            List<string> list = new List<string>();

            foreach (KeyValuePair<string, string> kvp in item)
            {
                if (kvp.Key.ToLower().Equals("id"))
                    row.id = kvp.Value;
                list.Add(kvp.Value);
            }
            row.cell = list.ToArray();
        }

        /// <summary>
        /// 对象转换为Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(Object obj)
        {
          //  DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            Stream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Position = 0;
            StreamReader streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }
    }

    /// <summary>
    /// JQGrid数据类
    /// </summary>
    [DataContract]
    public class JQGridData
    {
        /// <summary>
        /// 总数据
        /// </summary>
        [DataMember]
        public int total { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        public int page { get; set; }

        /// <summary>
        /// 数据记录数
        /// </summary>
        [DataMember]
        public int records { get; set; }

        /// <summary>
        /// 数据行集合
        /// </summary>
        [DataMember]
        public IList<JQGridDataRow> rows { get; set; }

    }

    /// <summary>
    /// JQGrid数据行类
    /// </summary>
    [DataContract]
    public class JQGridDataRow
    {
        /// <summary>
        /// ID号
        /// </summary>
        [DataMember]
        public string id { get; set; }

        /// <summary>
        /// 单元格集合
        /// </summary>
        [DataMember]
        public string[] cell { get; set; }
    }

    /// <summary>
    /// JGGridFooter数据
    /// </summary>
    [DataContract]
    public class JQGridFooterData
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public JQGridFooterData()
        {
            data = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// 添加单元格
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        public void AddCell(string columnName, string value)
        {
            KeyValuePair<string, string> newItem = new KeyValuePair<string, string>(columnName, value);
            this.data.Add(newItem);
        }

        private IList<KeyValuePair<string, string>> data;

        /// <summary>
        /// 转换为Json字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder txt = new StringBuilder("{");
            foreach (var item in data)
            {
                txt.AppendFormat("\"{0}\"", item.Key);
                txt.Append(":");
                txt.Append('"');
                txt.Append(item.Value);
                txt.Append('"');
                txt.Append(',');
            }
            if (txt.Length > 0)
            {
                txt.Remove(txt.Length - 1, 1);
            }
            txt.Append("}");
            return txt.ToString();
        }
    }

    /// <summary>
    /// Ajax异步操作结果类
    /// </summary>
    [DataContract]
    public class AjaxOperationResult
    {
        /// <summary>
        /// 指示结果是否成功
        /// </summary>
        [DataMember]
        public bool success { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        [DataMember]
        public string message { get; set; }

        /// <summary>
        /// 额外的数据
        /// </summary>
        [DataMember]
        public string extradata { get; set; }

        /// <summary>
        /// 新ID
        /// </summary>
        [DataMember]
        public string new_id { get; set; }

        /// <summary>
        /// 转换为Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return Object2GridData.ObjectToJson(this);
        }
    }

    /// <summary>
    /// 日志记录事件委托
    /// </summary>
    /// <param name="moduleName"></param>
    /// <param name="message"></param>
    public delegate void LoggingEventHanlder(string moduleName, string message);


    #region Excel元素定义（用于XML序列化和反序列化）

    /// <summary>
    /// Excel DataRange
    /// </summary>
    public class XtDataRange
    {
        #region Private Fields

        private string m_name = string.Empty;
        private string m_startCell = string.Empty;
        private string m_endCell = string.Empty;
        private string m_value = string.Empty;
        private string m_width = string.Empty;
        private string m_fontSize = string.Empty;
        private string m_align = string.Empty;
        private string m_wordWrap = string.Empty;
        private string m_bold = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttributeAttribute("Name")]
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// 起始单元格
        /// </summary>
        [XmlAttributeAttribute("StartCell")]
        public string StartCell
        {
            get { return this.m_startCell; }
            set { this.m_startCell = value; }
        }

        /// <summary>
        /// 结束单元格
        /// </summary>
        [XmlAttributeAttribute("EndCell")]
        public string EndCell
        {
            get { return this.m_endCell; }
            set { this.m_endCell = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        [XmlAttributeAttribute("Value")]
        public string Value
        {
            get { return this.m_value; }
            set { this.m_value = value; }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        [XmlAttributeAttribute("Width")]
        public string Width
        {
            get { return this.m_width; }
            set { this.m_width = value; }
        }

        /// <summary>
        /// 字号
        /// </summary>
        [XmlAttributeAttribute("FontSize")]
        public string FontSize
        {
            get { return this.m_fontSize; }
            set { this.m_fontSize = value; }
        }

        /// <summary>
        /// 对齐方式
        /// </summary>
        [XmlAttributeAttribute("Align")]
        public string Align
        {
            get { return this.m_align; }
            set { this.m_align = value; }
        }

        /// <summary>
        /// 是否换行。“0”或“1”
        /// </summary>
        [XmlAttributeAttribute("WordWrap")]
        public string WordWrap
        {
            get { return this.m_wordWrap; }
            set { this.m_wordWrap = value; }
        }

        /// <summary>
        /// 是否粗体。“0”或“1”
        /// </summary>
        [XmlAttributeAttribute("Bold")]
        public string Bold
        {
            get { return this.m_bold; }
            set { this.m_bold = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        public XtDataRange()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        /// <param name="value"></param>
        public XtDataRange(string startCell, string endCell, string value)
        {
            this.m_startCell = startCell;
            this.m_endCell = endCell;
            this.m_value = value;
        }

        #endregion
    }

    /// <summary>
    /// Excel DataRange列表
    /// </summary>
    public class XtDataRangeList
    {
        #region Private Fields

        private XtDataRange[] m_dataRanges = new XtDataRange[0];

        #endregion

        #region Properties

        /// <summary>
        /// 索引器。返回指定索引的DataRange
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [XmlIgnore()]
        public XtDataRange this[int index]
        {
            get
            {

                return this.m_dataRanges[index];
            }
        }

        /// <summary>
        /// 索引器。返回指定名称的DataRange
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [XmlIgnore()]
        public XtDataRange this[string name]
        {
            get
            {
                foreach (XtDataRange range in this.m_dataRanges)
                {
                    if (range.Name.Equals(name))
                    {
                        return range;
                    }
                }

                return new XtDataRange();
            }
        }

        /// <summary>
        /// 数据项集合
        /// </summary>
        [XmlElementAttribute("Range")]
        public XtDataRange[] Items
        {
            get
            {
                return this.m_dataRanges;
            }
            set
            {
                this.m_dataRanges = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 添加DataRange
        /// </summary>
        /// <param name="dataRange"></param>
        public void Add(XtDataRange dataRange)
        {
            Array.Resize<XtDataRange>(ref this.m_dataRanges, this.m_dataRanges.Length + 1);
            this.m_dataRanges.SetValue(dataRange, this.m_dataRanges.Length - 1);
        }

        /// <summary>
        /// 添加DataRange
        /// </summary>
        /// <param name="dataRangeList"></param>
        public void AddRange(List<XtDataRange> dataRangeList)
        {
            int sourceLength = this.m_dataRanges.Length;
            Array.Resize<XtDataRange>(ref this.m_dataRanges, this.m_dataRanges.Length + dataRangeList.Count);
            Array.Copy(dataRangeList.ToArray(), 0, this.m_dataRanges, sourceLength, dataRangeList.Count);
        }

        #endregion
    }

    /// <summary>
    /// Excel BorderRange
    /// </summary>
    public class XtBorderRange
    {
        #region Private Fields

        private string m_startCell = string.Empty;
        private string m_endCell = string.Empty;
        private string m_insideHorizontal = string.Empty;
        private string m_insideVertical = string.Empty;
        private string m_around = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// 起始单元格
        /// </summary>
        [XmlAttributeAttribute("StartCell")]
        public string StartCell
        {
            get { return this.m_startCell; }
            set { this.m_startCell = value; }
        }

        /// <summary>
        /// 结束单元格
        /// </summary>
        [XmlAttributeAttribute("EndCell")]
        public string EndCell
        {
            get { return this.m_endCell; }
            set { this.m_endCell = value; }
        }

        /// <summary>
        /// Range内水平边框
        /// </summary>
        [XmlAttributeAttribute("InsideHorizontal")]
        public string InsideHorizontal
        {
            get { return this.m_insideHorizontal; }
            set { this.m_insideHorizontal = value; }
        }

        /// <summary>
        /// Range内垂直边框
        /// </summary>
        [XmlAttributeAttribute("InsideVertial")]
        public string InsideVertical
        {
            get { return this.m_insideVertical; }
            set { this.m_insideVertical = value; }
        }

        /// <summary>
        /// Range外边框
        /// </summary>
        [XmlAttributeAttribute("Around")]
        public string Around
        {
            get { return this.m_around; }
            set { this.m_around = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        public XtBorderRange()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        /// <param name="insideHorizontalLine"></param>
        /// <param name="insideVerticalLine"></param>
        /// <param name="aroundLine"></param>
        public XtBorderRange(string startCell, string endCell, bool insideHorizontalLine, bool insideVerticalLine, bool aroundLine)
        {
            this.m_startCell = startCell;
            this.m_endCell = endCell;
            this.m_insideHorizontal = insideHorizontalLine ? "1" : "0";
            this.m_insideVertical = insideVerticalLine ? "1" : "0";
            this.m_around = aroundLine ? "1" : "0";
        }

        #endregion
    }

    /// <summary>
    /// Excel BorderRange列表
    /// </summary>
    public class XtBorderRangeList
    {
        #region Private Fields

        private XtBorderRange[] m_borderRanges = new XtBorderRange[0];

        #endregion

        #region Properties

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [XmlIgnore()]
        public XtBorderRange this[int index]
        {
            get
            {
                return this.m_borderRanges[index];
            }
        }

        /// <summary>
        /// 数据项
        /// </summary>
        [XmlElementAttribute("Range")]
        public XtBorderRange[] Items
        {
            get
            {
                return this.m_borderRanges;
            }

            set
            {
                this.m_borderRanges = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 添加BorderRange
        /// </summary>
        /// <param name="borderRange"></param>
        public void Add(XtBorderRange borderRange)
        {
            Array.Resize<XtBorderRange>(ref this.m_borderRanges, this.m_borderRanges.Length + 1);
            this.m_borderRanges.SetValue(borderRange, this.m_borderRanges.Length - 1);
        }

        #endregion
    }

    /// <summary>
    /// Excel Margin
    /// </summary>
    public class XtMargin
    {
        #region Private Fields

        private decimal m_value = 0m;

        #endregion

        #region Properties

        /// <summary>
        /// 值
        /// </summary>
        [XmlAttributeAttribute("Value")]
        public decimal Value
        {
            get { return this.m_value; }
            set { this.m_value = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel列宽类
    /// </summary>
    public class XtColumnWidth
    {
        #region Private Fields

        private string m_defaultValues = string.Empty;
        private string m_specificValues = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// 缺省值
        /// </summary>
        [XmlAttributeAttribute("DefaultValues")]
        public string DefaultValues
        {
            get { return this.m_defaultValues; }
            set { this.m_defaultValues = value; }
        }

        /// <summary>
        /// 指定值
        /// </summary>
        [XmlAttributeAttribute("SpecificValues")]
        public string SpecificValues
        {
            get { return this.m_specificValues; }
            set { this.m_specificValues = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel行高类
    /// </summary>
    public class XtRowHeight
    {
        #region Private Fields

        private string m_defaultValues = string.Empty;
        private string m_specificValues = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// 缺省值
        /// </summary>
        [XmlAttributeAttribute("DefaultValues")]
        public string DefaultValues
        {
            get { return this.m_defaultValues; }
            set { this.m_defaultValues = value; }
        }

        /// <summary>
        /// 指定值
        /// </summary>
        [XmlAttributeAttribute("SpecificValues")]
        public string SpecificValues
        {
            get { return this.m_specificValues; }
            set { this.m_specificValues = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel表格尺寸类
    /// </summary>
    public class XtGridSizes
    {
        #region Private Fields

        private XtColumnWidth m_columnWidth = new XtColumnWidth();
        private XtRowHeight m_rowHeight = new XtRowHeight();

        #endregion

        #region Properties

        /// <summary>
        /// 列宽度
        /// </summary>
        [XmlElementAttribute("ColumnWidth")]
        public XtColumnWidth ColumnWidth
        {
            get { return this.m_columnWidth; }
            set { this.m_columnWidth = value; }
        }

        /// <summary>
        /// 行高度
        /// </summary>
        [XmlElementAttribute("RowHeight")]
        public XtRowHeight RowHeight
        {
            get { return this.m_rowHeight; }
            set { this.m_rowHeight = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel Margin列表
    /// </summary>
    public class XtMargins
    {
        #region Private Fields

        private XtMargin m_top = new XtMargin();
        private XtMargin m_left = new XtMargin();
        private XtMargin m_bottom = new XtMargin();
        private XtMargin m_right = new XtMargin();
        private XtMargin m_header = new XtMargin();
        private XtMargin m_footer = new XtMargin();

        #endregion

        #region Properties

        /// <summary>
        /// 顶部Margin
        /// </summary>
        [XmlElementAttribute("TopMargin")]
        public XtMargin TopMargin
        {
            get { return this.m_top; }
            set { this.m_top = value; }
        }

        /// <summary>
        /// 左边Margin
        /// </summary>
        [XmlElementAttribute("LeftMargin")]
        public XtMargin LeftMargin
        {
            get { return this.m_left; }
            set { this.m_left = value; }
        }

        /// <summary>
        /// 底部Margin
        /// </summary>
        [XmlElementAttribute("BottomMargin")]
        public XtMargin BottomMargin
        {
            get { return this.m_bottom; }
            set { this.m_bottom = value; }
        }

        /// <summary>
        /// 右边Margin
        /// </summary>
        [XmlElementAttribute("RightMargin")]
        public XtMargin RightMargin
        {
            get { return this.m_right; }
            set { this.m_right = value; }
        }

        /// <summary>
        /// Header Margin
        /// </summary>
        [XmlElementAttribute("HeaderMargin")]
        public XtMargin HeaderMargin
        {
            get { return this.m_header; }
            set { this.m_header = value; }
        }

        /// <summary>
        /// Footer Margin
        /// </summary>
        [XmlElementAttribute("FooterMargin")]
        public XtMargin FooterMargin
        {
            get { return this.m_footer; }
            set { this.m_footer = value; }
        }
        #endregion
    }

    /// <summary>
    /// Excel sheet
    /// </summary>
    public class XtSheet
    {
        #region Private Fields

        private int m_index = 0;
        private string m_name = string.Empty;
        private XtDataRangeList m_dataRanges = new XtDataRangeList();
        private XtBorderRangeList m_borderRanges = new XtBorderRangeList();
        private XtGridSizes m_gridSizes = new XtGridSizes();
        private XtMargins m_margins = null;

        #endregion

        #region Properties

        /// <summary>
        /// 索引
        /// </summary>
        [XmlAttributeAttribute("Index")]
        public int Index
        {
            get { return this.m_index; }
            set { this.m_index = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttributeAttribute("Name")]
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// DataRange集合
        /// </summary>
        [XmlElementAttribute("Data")]
        public XtDataRangeList DataRanges
        {
            get { return this.m_dataRanges; }
            set { this.m_dataRanges = value; }
        }

        /// <summary>
        /// BorderRange集合
        /// </summary>
        [XmlElementAttribute("Borders")]
        public XtBorderRangeList BorderRanges
        {
            get { return this.m_borderRanges; }
            set { this.m_borderRanges = value; }
        }

        /// <summary>
        /// GridSize集合
        /// </summary>
        [XmlElementAttribute("GridSizes")]
        public XtGridSizes GridSizes
        {
            get { return this.m_gridSizes; }
            set { this.m_gridSizes = value; }
        }

        /// <summary>
        /// Margin集合
        /// </summary>
        [XmlElementAttribute("Margins")]
        public XtMargins Margins
        {
            get { return this.m_margins; }
            set { this.m_margins = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel sheet列表
    /// </summary>
    public class XtSheetList
    {
        #region Private Fields

        private XtSheet[] m_sheets = new XtSheet[0];

        #endregion

        #region Properties

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [XmlIgnore()]
        public XtSheet this[int index]
        {
            get
            {
                return this.m_sheets[index];
            }
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [XmlIgnore()]
        public XtSheet this[string name]
        {
            get
            {
                foreach (XtSheet sheet in this.m_sheets)
                {
                    if (sheet.Name.Equals(name))
                    {
                        return sheet;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// 数据项
        /// </summary>
        [XmlElementAttribute("Sheet")]
        public XtSheet[] Items
        {
            get
            {
                return this.m_sheets;
            }

            set
            {
                this.m_sheets = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Excel纸张
    /// </summary>
    public class XtLayerPaper
    {
        #region Private Fields

        private string m_size = string.Empty;
        private string m_orientation = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// 尺寸
        /// </summary>
        [XmlAttributeAttribute("Size")]
        public string Size
        {
            get { return this.m_size; }
            set { this.m_size = value; }
        }

        /// <summary>
        /// 方向
        /// </summary>
        [XmlAttributeAttribute("Orientation")]
        public string Orientation
        {
            get { return this.m_orientation; }
            set { this.m_orientation = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel布局
    /// </summary>
    public class XtLayout
    {
        #region Private Fields

        private XtLayerPaper m_paper = new XtLayerPaper();

        #endregion

        #region Properties

        /// <summary>
        /// 纸张
        /// </summary>
        [XmlElementAttribute("Paper")]
        public XtLayerPaper Paper
        {
            get { return this.m_paper; }
            set { this.m_paper = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel 格式字体
    /// </summary>
    public class XtFormatFont
    {
        #region Private Fields

        private string m_name = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// 字体名称
        /// </summary>
        [XmlAttributeAttribute("FontName")]
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel 格式边框
    /// </summary>
    public class XtFormatBorder
    {
        #region Private Fields

        private string m_lineStyle = string.Empty;
        private string m_lineWeight = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// 边线类型
        /// </summary>
        [XmlAttributeAttribute("LineStyle")]
        public string LineStyle
        {
            get { return this.m_lineStyle; }
            set { this.m_lineStyle = value; }
        }

        /// <summary>
        /// 边线宽度
        /// </summary>
        [XmlAttributeAttribute("LineWeight")]
        public string LineWeight
        {
            get { return this.m_lineWeight; }
            set { this.m_lineWeight = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel 格式
    /// </summary>
    public class XtFormat
    {
        #region Private Fields

        private XtFormatFont m_font = new XtFormatFont();
        private XtFormatBorder m_border = new XtFormatBorder();

        #endregion

        #region Properties

        /// <summary>
        /// 字体
        /// </summary>
        [XmlElementAttribute("Font")]
        public XtFormatFont Font
        {
            get { return this.m_font; }
            set { this.m_font = value; }
        }

        /// <summary>
        /// 边框
        /// </summary>
        [XmlElementAttribute("Border")]
        public XtFormatBorder Border
        {
            get { return this.m_border; }
            set { this.m_border = value; }
        }

        #endregion
    }

    /// <summary>
    /// Excel 模板
    /// </summary>
    public class XtTemplate
    {
        #region Private Fields

        private XtLayout m_layout = new XtLayout();
        private XtFormat m_format = new XtFormat();
        private XtSheetList m_sheets = new XtSheetList();

        #endregion

        #region Properties

        /// <summary>
        /// 布局
        /// </summary>
        [XmlElementAttribute("Layout")]
        public XtLayout Layout
        {
            get { return this.m_layout; }
            set { this.m_layout = value; }
        }

        /// <summary>
        /// 格式
        /// </summary>
        [XmlElementAttribute("Format")]
        public XtFormat Format
        {
            get { return this.m_format; }
            set { this.m_format = value; }
        }

        /// <summary>
        /// Sheet集合
        /// </summary>
        [XmlElementAttribute("Sheets")]
        public XtSheetList Sheets
        {
            get { return this.m_sheets; }
            set { this.m_sheets = value; }
        }

        #endregion
    }

    #endregion


    ///// <summary>
    ///// JQGrid Excel 帮助工具类
    ///// </summary>
    //public class JQGridExcelHelper : IDisposable
    //{
    //    #region Private Fields

    //    private static JQGridExcelHelper s_instance = null;
    //    private XmlDocument m_xmlDoc = new XmlDocument();
    //    private static object locker = new object();

    //    private event LoggingEventHanlder m_onLogging = null;

    //    #endregion

    //    #region Properties

    //    /// <summary>
    //    /// 实例
    //    /// </summary>
    //    public static JQGridExcelHelper Instance
    //    {
    //        get
    //        {
    //            if (s_instance == null)
    //            {
    //                Monitor.Enter(locker);

    //                try
    //                {
    //                    if (s_instance == null)
    //                    {
    //                        s_instance = new JQGridExcelHelper();
    //                    }
    //                }
    //                finally
    //                {
    //                    Monitor.Exit(locker);
    //                }
    //            }

    //            return s_instance;
    //        }
    //    }

    //    Dictionary<string, int> ColumnMap
    //    {
    //        get
    //        {
    //            Dictionary<string, int> _columnMap = new Dictionary<string, int>();
    //            _columnMap.Add("A", 0);
    //            _columnMap.Add("B", 1);
    //            _columnMap.Add("C", 2);
    //            _columnMap.Add("D", 3);
    //            _columnMap.Add("E", 4);
    //            _columnMap.Add("F", 5);
    //            _columnMap.Add("G", 6);
    //            _columnMap.Add("H", 7);
    //            _columnMap.Add("I", 8);
    //            _columnMap.Add("J", 9);
    //            _columnMap.Add("K", 10);
    //            _columnMap.Add("L", 11);
    //            _columnMap.Add("M", 12);
    //            _columnMap.Add("N", 13);
    //            _columnMap.Add("O", 14);
    //            _columnMap.Add("P", 15);
    //            _columnMap.Add("Q", 16);
    //            _columnMap.Add("R", 17);
    //            _columnMap.Add("S", 18);
    //            _columnMap.Add("T", 19);
    //            _columnMap.Add("U", 20);
    //            _columnMap.Add("V", 21);
    //            _columnMap.Add("W", 22);
    //            _columnMap.Add("X", 23);
    //            _columnMap.Add("Y", 24);
    //            _columnMap.Add("Z", 25);
    //            return _columnMap;
    //        }
    //    }

    //    #endregion

    //    #region Events

    //    /// <summary>
    //    /// 日志记录事件
    //    /// </summary>
    //    public event LoggingEventHanlder OnLogging
    //    {
    //        add
    //        {
    //            this.m_onLogging += value;
    //        }
    //        remove
    //        {
    //            this.m_onLogging -= value;
    //        }
    //    }

    //    #endregion

    //    #region Constructor

    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public JQGridExcelHelper()
    //    {
    //    }

    //    #endregion

    //    #region Disposing Methods

    //    /// <summary>
    //    /// 回收函数
    //    /// </summary>
    //    /// <param name="disposedStatus"></param>
    //    protected virtual void Dispose(bool disposedStatus)
    //    {
    //        if (disposedStatus)
    //        {
    //        }
    //    }

    //    #endregion

    //    #region Public Methods

    //    /// <summary>
    //    /// 从JQGrid导出到Excel
    //    /// </summary>
    //    /// <param name="backupPath"></param>
    //    /// <param name="caption"></param>
    //    /// <param name="gridData"></param>
    //    /// <returns></returns>
    //    public string ExportFromGrid(string backupPath, string caption, List<Dictionary<string, string>> gridData)
    //    {
    //        // Performance is not very well when the data size is greater than 20
    //        return DoExportFromGrid_Excel(backupPath, caption, gridData);
    //    }

    //    /// <summary>
    //    /// 提示用户下载文件
    //    /// </summary>
    //    /// <param name="response">Current http response</param>
    //    /// <param name="filePath">The expected file for user</param>
    //    public void WriteExcelToBrowser(HttpContext context, string filePath)
    //    {
    //        WriteExcelToBrowser(context, filePath, true);
    //    }

    //    /// <summary>
    //    /// 提示用户下载文件
    //    /// </summary>
    //    /// <param name="response">Current http response</param>
    //    /// <param name="filePath">The expected file for user</param>
    //    public void WriteExcelToBrowser(HttpContext context, string filePath, bool deleteAfterFinish)
    //    {
    //        WriteExcelToBrowser(context, "", filePath, deleteAfterFinish);
    //    }

    //    /// <summary>
    //    /// 提示用户下载文件
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="fileName"></param>
    //    /// <param name="filePath"></param>
    //    /// <param name="deleteAfterFinish"></param>
    //    public void WriteExcelToBrowser(HttpContext context, string fileName, string filePath, bool deleteAfterFinish)
    //    {
    //        if (String.IsNullOrEmpty(filePath) || !File.Exists(filePath))
    //            return;

    //        FileInfo fileInfo = new FileInfo(filePath);
    //        string clientFileName = String.IsNullOrEmpty(fileName) ? Path.GetFileNameWithoutExtension(filePath).Split('_')[0] : fileName;
    //        clientFileName += Path.GetExtension(filePath);
    //        HttpResponse response = context.Response;

    //        response.Clear();
    //        response.ClearContent();
    //        response.ClearHeaders();
    //        response.AddHeader("Content-Disposition", "attachment;filename=" + context.Server.UrlPathEncode(clientFileName));
    //        response.AddHeader("Content-Length", fileInfo.Length.ToString());
    //        response.AddHeader("Content-Transfer-Encoding", "binary");
    //        response.ContentType = "application/vnd.ms-excel";
    //        response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
    //        response.WriteFile(fileInfo.FullName, true);

    //        try
    //        {
    //            if (deleteAfterFinish)
    //                File.Delete(filePath);
    //        }
    //        catch (Exception ex)
    //        {
    //            this.LogInfo("WriteExcelToBrowser", ex.Message);
    //        }

    //        response.Flush();
    //        response.End();
    //    }

    //    #endregion

    //    #region Private Methods

    //    private void SetBorder(HSSFWorkbook workbook, HSSFSheet sheet, XtSheet xtSheet)
    //    {
    //        foreach (XtBorderRange xtBorderRange in xtSheet.BorderRanges.Items)
    //        {

    //            int tmpStartRow = Int32.Parse(xtBorderRange.StartCell.Substring(1)) - 1;
    //            int tmpStartColumn = ColumnMap[xtBorderRange.StartCell.Substring(0, 1)];

    //            int tmpEndRow = Int32.Parse(xtBorderRange.StartCell.Substring(1)) - 1;
    //            int tmpEndColumn = ColumnMap[xtBorderRange.StartCell.Substring(0, 1)];

    //            if (!string.IsNullOrEmpty(xtBorderRange.EndCell))
    //            {
    //                tmpEndRow = Int32.Parse(xtBorderRange.EndCell.Substring(1)) - 1;
    //                tmpEndColumn = ColumnMap[xtBorderRange.EndCell.Substring(0, 1)];
    //            }

    //            SetBorder(tmpStartRow, tmpEndRow, tmpStartColumn, tmpEndColumn, HSSFBorderFormatting.BORDER_NONE, sheet, workbook);

    //            HSSFRow tmpRow;
    //            for (int i = tmpStartRow; i <= tmpEndRow; i++)
    //            {
    //                tmpRow = sheet.GetRow(i);
    //                for (int j = tmpStartColumn; j <= tmpEndColumn; j++)
    //                {
    //                    if (xtBorderRange.InsideHorizontal == "1" && xtBorderRange.InsideVertical == "1")
    //                    {
    //                        SetBorder(i, i, j, j, sheet, workbook);
    //                    }
    //                    else if (xtBorderRange.InsideHorizontal == "1")
    //                    {
    //                        short borderType = 1;
    //                        CellRangeAddress table = new CellRangeAddress(i, i, j, j);
    //                        HSSFRegionUtil.SetBorderTop(borderType, table, sheet, workbook);
    //                        HSSFRegionUtil.SetBorderBottom(borderType, table, sheet, workbook);
    //                    }
    //                    else if (xtBorderRange.InsideVertical == "1")
    //                    {
    //                        short borderType = 1;
    //                        CellRangeAddress table = new CellRangeAddress(i, i, j, j);
    //                        HSSFRegionUtil.SetBorderRight(borderType, table, sheet, workbook);
    //                        HSSFRegionUtil.SetBorderLeft(borderType, table, sheet, workbook);
    //                    }

    //                }
    //            }

    //            if (xtBorderRange.Around == "1")
    //            {
    //                SetBorder(tmpStartRow, tmpEndRow, tmpStartColumn, tmpEndColumn, sheet, workbook);
    //            }
    //        }
    //    }

    //    private HSSFFont BuildFont(HSSFWorkbook workbook, string fontName, short boldSize, string fontSize)
    //    {
    //        HSSFFont boldfont = workbook.CreateFont();
    //        boldfont.Boldweight = boldSize;
    //        if (string.IsNullOrEmpty(fontSize))
    //        {
    //            boldfont.FontHeightInPoints = 11;
    //        }
    //        else
    //        {
    //            boldfont.FontHeightInPoints = short.Parse(fontSize);
    //        }
    //        boldfont.FontName = fontName;
    //        return boldfont;
    //    }

    //    private void SetBorder(int minRowNum, int maxRowNum, int minColNum, int maxColNum, short borderType, HSSFSheet sheet, HSSFWorkbook workbook)
    //    {

    //        CellRangeAddress table = new CellRangeAddress(minRowNum, maxRowNum, minColNum, maxColNum);

    //        HSSFRegionUtil.SetBorderRight(borderType, table, sheet, workbook);

    //        HSSFRegionUtil.SetBorderLeft(borderType, table, sheet, workbook);

    //        HSSFRegionUtil.SetBorderTop(borderType, table, sheet, workbook);

    //        HSSFRegionUtil.SetBorderBottom(borderType, table, sheet, workbook);

    //    }
    //    private void SetBorder(int minRowNum, int maxRowNum, int minColNum, int maxColNum, HSSFSheet sheet, HSSFWorkbook workbook)
    //    {
    //        short borderType = 1; // CellBorderType.THIN; //HSSFCellStyle.BORDER_THIN;
    //        SetBorder(minRowNum, maxRowNum, minColNum, maxColNum, borderType, sheet, workbook);
    //    }

    //    private string RemoveSpecialCsvCharacter(string s)
    //    {
    //        if (String.IsNullOrEmpty(s))
    //            s = String.Empty;
    //        if (s.Contains(","))
    //            s = s.Replace(',', ';');
    //        if (s.Contains("\""))
    //            s = s.Replace('"', '\'');

    //        return s;
    //    }

    //    private string DoExportFromGrid_Csv(string backupPath, string caption, List<Dictionary<string, string>> gridData)
    //    {
    //        if (gridData == null || gridData.Count == 0)
    //            return String.Empty;

    //        string filePath = Path.Combine(backupPath, String.Format("{0}-{1}.csv", "grid", Guid.NewGuid()));
    //        StringBuilder csvBuilder = new StringBuilder();
    //        try
    //        {
    //            csvBuilder.Append("\"");
    //            foreach (string colName in gridData[0].Keys)
    //            {
    //                csvBuilder.AppendFormat("{0},", RemoveSpecialCsvCharacter(colName));
    //            }
    //            csvBuilder.Append("\"");
    //            csvBuilder.AppendLine();

    //            foreach (Dictionary<string, string> rows in gridData)
    //            {
    //                csvBuilder.Append("\"");
    //                foreach (KeyValuePair<string, string> cell in rows)
    //                {
    //                    csvBuilder.AppendFormat("{0},", RemoveSpecialCsvCharacter(cell.Value));
    //                }
    //                csvBuilder.Append("\"");
    //                csvBuilder.AppendLine();
    //            }

    //            TextWriter tw = new StreamWriter(filePath, false, Encoding.Unicode);
    //            tw.Write(csvBuilder.ToString());
    //            tw.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            filePath = null;
    //            this.LogInfo("CSV Export", ex.Message);
    //        }

    //        return filePath;
    //    }

    //    private string DoExportFromGrid_Excel(string backupPath, string caption, List<Dictionary<string, string>> gridData)
    //    {
    //        if (gridData == null)
    //            return String.Empty;

    //        caption = String.IsNullOrEmpty(caption) ? "sheet1" : caption;

    //        HSSFWorkbook workbook = null;
    //        HSSFSheet sheet = null;
    //        string sheetName = string.Empty;
    //        string filePath = Path.Combine(backupPath, String.Format("{0}-{1}.xls", "grid", Guid.NewGuid()));

    //        try
    //        {
    //            workbook = new HSSFWorkbook();

    //            // Create an entry of DocumentSummaryInformation
    //            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
    //            dsi.Company = "";
    //            workbook.DocumentSummaryInformation = dsi;

    //            // Create an entry of SummaryInformation
    //            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
    //            si.Subject = caption;

    //            int i = 0;
    //            int j = 0;

    //            sheet = workbook.CreateSheet(caption);

    //            HSSFRow header = sheet.CreateRow(0);
    //            foreach (string colName in gridData[0].Keys)
    //            {
    //                header.CreateCell(j++).SetCellValue(colName);
    //            }

    //            for (i = 0; i < gridData.Count; i++)
    //            {
    //                HSSFRow row = sheet.CreateRow(i + 1);
    //                j = 0;

    //                foreach (string colName in gridData[i].Keys)
    //                {
    //                    HSSFCell cell = row.CreateCell(j++);
    //                    cell.SetCellValue(gridData[i][colName]);
    //                }
    //            }

    //            /*
    //            int i = 0;
    //            int j = 0;

    //            HSSFRow header = sheet.CreateRow(0);
    //            foreach (string colName in data[0].Keys)
    //            {
    //                header.CreateCell(j++).SetCellValue(colName);
    //            }

    //            for (i = 0; i < data.Count; i++)
    //            {
    //                HSSFRow row = sheet.CreateRow(i + 1);
    //                j = 0;

    //                foreach (string colName in data[i].Keys)
    //                {
    //                    row.CreateCell(j++).SetCellValue(data[i][colName]);
    //                }
    //            }*/

    //            //for (int row = 0; row <= gridData.Count; row++)
    //            //{
    //            //    for (int col = 0; col < gridData[0].Keys.Count; col++)
    //            //    {
    //            //        SetBorder(row, row, col, col, sheet, workbook);
    //            //    }
    //            //}

    //            FileStream fs = new FileStream(filePath, FileMode.Create);
    //            workbook.Write(fs);
    //            fs.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            filePath = null;
    //            this.LogInfo("Grid Export", ex.Message);
    //        }
    //        finally
    //        {
    //            this.LogInfo("Data Backup", "Closing the Excel file.");
    //            workbook = null;
    //        }

    //        return filePath;
    //    }

    //    private void LogInfo(string moduleName, string message)
    //    {
    //        if (this.m_onLogging != null)
    //        {
    //            this.m_onLogging(moduleName, message);
    //        }
    //    }

    //    private string RMBString(decimal rmb)
    //    {
    //        string[] rmbNumbers = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
    //        string[] unitNumbers = { "元", "拾", "佰", "千", "万" };
    //        string rmbSource = ((int)rmb).ToString();
    //        string rmbDest = "";

    //        if ((int)rmb == 0)
    //        {
    //            rmbDest = "零元";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < rmbSource.Length; i++)
    //            {
    //                if (i <= 4)
    //                {
    //                    rmbDest = rmbDest.Insert(0, unitNumbers[i]);
    //                }
    //                else
    //                {
    //                    rmbDest = rmbDest.Insert(0, unitNumbers[(i % 5) + 1]);
    //                }

    //                rmbDest = rmbDest.Insert(0, rmbNumbers[int.Parse(rmbSource.Substring(rmbSource.Length - i - 1, 1))]);
    //            }
    //        }

    //        return rmbDest;
    //    }

    //    #endregion

    //    #region IDisposable Members

    //    /// <summary>
    //    /// Dispose函数
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    #endregion
    //}

}