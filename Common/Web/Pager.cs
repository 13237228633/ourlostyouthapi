namespace Common.Web
{
    using Common;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    
    public sealed class Pager
    {
        private readonly PageInfo _pageInfo;

        private int _rowCounter = 1;

        public Pager(PageInfo pageInfo)
        {
            this._pageInfo = pageInfo;
        }

        #region Render
        /// <summary>
        ///  
        /// </summary>
        /// <param name="pageIndexParam"></param>
        /// <returns></returns>
        public string Render(string pageIndexParam)
        {
            //this._pageInfo.CurrentIndex = this._pageInfo.CurrentIndex > 0 ? this._pageInfo.CurrentIndex : 1;
            //this._pageInfo.RecordCount = this._pageInfo.RecordCount > 0 ? this._pageInfo.RecordCount : 0;
            //this._pageInfo.PageSize = this._pageInfo.PageSize > 1 ? this._pageInfo.PageSize : 1;

            if (this._pageInfo.RecordCount > this._pageInfo.PageSize)
            {
                const int linkCount = 9;
                int endIndexFront = 0;
                int startIndexMiddle = 0;      //中间显示的起始链接页(例如5:1 2 ... 5 6 7 8 9 ... 11 12)
                int endIndexMiddle = 0;        //中间显示的结束链接页(例如9:1 2 ... 5 6 7 8 9 ... 11 12)
                int startIndexBack = 0;        //右边显示的起始链接页(例如11:1 2 ... 5 6 7 8 9 ... 11 12)
                const int maxLinkCountSide = 7;
                const int linkCountMiddle = 5;
                const int minLinkCountSite = 2;
                //
                int pageRecordCount = this._pageInfo.CurrentIndex * this._pageInfo.PageSize - this._pageInfo.RecordCount;
                pageRecordCount = pageRecordCount > 0 ? this._pageInfo.PageSize - pageRecordCount : this._pageInfo.PageSize;
                int pageItemBegin = (this._pageInfo.CurrentIndex - 1) * this._pageInfo.PageSize + 1;
                int pageItemEnd = pageItemBegin + pageRecordCount - 1;
                int pageCount = (this._pageInfo.RecordCount / this._pageInfo.PageSize) + (this._pageInfo.RecordCount % this._pageInfo.PageSize == 0 ? 0 : 1);
                this._pageInfo.CurrentIndex = this._pageInfo.CurrentIndex <= pageCount ? this._pageInfo.CurrentIndex : pageCount;
                //  如果当前页的总数<=_LinkCount的值,则全部显示(1 2 3)
                if (pageCount <= linkCount)
                {
                    endIndexFront = pageCount;
                    startIndexMiddle = 0;
                    endIndexMiddle = 0;
                    startIndexBack = pageCount;
                }
                else
                {
                    //  1 2 3 4 5 ...11 12(如果当前页<左边的链接总数)
                    if (this._pageInfo.CurrentIndex < maxLinkCountSide)
                    {
                        endIndexFront = maxLinkCountSide;
                        startIndexMiddle = 0;
                        endIndexMiddle = 0;
                        startIndexBack = pageCount + 1 - minLinkCountSite;
                    }
                    //  1 2 ... 5 6 7 8 9 ... 11 12
                    else if (this._pageInfo.CurrentIndex >= maxLinkCountSide && this._pageInfo.CurrentIndex <= (pageCount - maxLinkCountSide + 1))
                    {
                        const int halfMiddleLinkCount = (linkCountMiddle + 1) / 2;
                        endIndexFront = minLinkCountSite;
                        startIndexMiddle = this._pageInfo.CurrentIndex - halfMiddleLinkCount + 1;
                        endIndexMiddle = startIndexMiddle - 1 + linkCountMiddle;
                        startIndexBack = pageCount + 1 - minLinkCountSite;
                    }
                    //  1 2 ... 8 9 10 11 12
                    else if (this._pageInfo.CurrentIndex > (pageCount - maxLinkCountSide + 1))
                    {
                        endIndexFront = minLinkCountSite;
                        startIndexMiddle = 0;
                        endIndexMiddle = 0;
                        startIndexBack = pageCount + 1 - maxLinkCountSide;
                    }
                }
                //
                var html = new StringBuilder("<div class=\"allpage\">");
                //

                if (this._pageInfo.CurrentIndex > 1)
                {
                    html.AppendFormat("<a href=\"{0}\" class=\"pgope\">上一页</a>", GetPagingUrl(this._pageInfo.CurrentIndex - 1, pageIndexParam, false));
                }
             
                //
                html.Append(GetTextLink(this._pageInfo.CurrentIndex, 1, endIndexFront, false, pageIndexParam));
                html.Append(GetTextLink(this._pageInfo.CurrentIndex, startIndexMiddle, endIndexMiddle, true, pageIndexParam));
                html.Append(GetTextLink(this._pageInfo.CurrentIndex, startIndexBack, pageCount, true, pageIndexParam));
                //
                if (this._pageInfo.CurrentIndex < pageCount)
                {
                    html.AppendFormat("<a href=\"{0}\" class=\"pgope\">下一页</a> ", GetPagingUrl(this._pageInfo.CurrentIndex + 1, pageIndexParam, false));
                }
                
                //共 <span class="red">80</span> 页 跳转到 <input type="text" class="num" /> 页 <input type="button" class="sum" value="确定" />
                html.AppendFormat("共<span class=\"red\"> {0} </span>页&nbsp;&nbsp;", pageCount);
                html.AppendFormat("跳转到 <input type=\"text\" class=\"num\" /> 页 <input type=\"button\" class=\"sum\" value=\"确定\" onclick=\"goPage('{0}');\" /></div>", pageIndexParam);
                return html.ToString();
            }
            return string.Empty;
        }

        private static string GetTextLink(int currentPageIndex, int start, int end, bool hasEllipsis, string pageIndexParam)
        {
            string html = string.Empty;
            if (start < end)
            {
                if (hasEllipsis)
                {
                    html += "<div class=\"amore\">...</div>";
                }
                for (int i = start; i <= end; i++)
                {
                    html += GetTextLink(currentPageIndex, i, i.ToString(), pageIndexParam);
                }
            }
            return html;
        }

        private static string GetTextLink(int currentPageIndex, int pageIndex, string text, string pageIndexParam)
        {
            string pagingUrl = GetPagingUrl(pageIndex, pageIndexParam, false);
            return pageIndex == currentPageIndex ? string.Format("<div class=\"sbox\">{0}</div>", text) : string.Format("<a href=\"{0}\" class=\"pgbox\">{1}</a>", pagingUrl, text);
        }

        private static string GetPagingUrl(int newIndex, string pageIndexParam, bool mvcMode)
        {
            string url = HttpContext.Current.Request.RawUrl.TrimEnd(new[] { '/' });
            string newParam = pageIndexParam + "=" + newIndex;
            string pattern = (mvcMode ? "(?<=(/|&)?)" : "(?<=(\\?|&)?)") + pageIndexParam + "=\\d+";
            Match match = Regex.Match(url, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                url = Regex.Replace(url, pattern, newParam, RegexOptions.IgnoreCase);
            }
            else if (url.Contains("="))
            {
                url += "&" + newParam;
            }
            else
            {
                url += (mvcMode ? "/" : "?") + newParam;
            }
            return url;
        }
        #endregion

        #region OutputInterval
        /// <summary>
        ///  
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public string OutputInterval(string className)
        {
            string style = string.Empty;
            if (this._rowCounter % 2 == 0)
            {
                style = " class=\"" + className + "\"";
            }
            this._rowCounter++;
            return style;
        }
        #endregion
    }
}
