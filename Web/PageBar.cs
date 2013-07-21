using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LCW.Framework.Common.Web
{
    public static class PageBar
    {
        /// <summary>
        /// 简单模式：数字+上下页
        /// </summary>
        /// <param name="stype"></param>
        /// <param name="stepNum"></param>
        /// <param name="pageRoot"></param>
        /// <param name="pageFoot"></param>
        /// <param name="totalCount"></param>
        /// <param name="currentPage"></param>
        /// <param name="Http1"></param>
        /// <param name="HttpM"></param>
        /// <param name="HttpN"></param>
        /// <param name="limitPage"></param>
        /// <returns></returns>
        private static string getbar1(string stype, int stepNum, int pageRoot, int pageFoot, int pageCount, int totalCount, int pageSize, int currentPage, string Http1, string HttpM, string HttpN, int limitPage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='p_btns'>");
            if (totalCount > pageSize)
            {
                if (currentPage != 1)
                {//只要不是第一页
                    sb.Append("<a target='_self' href='" + GetPageUrl(1, Http1, HttpM, HttpN, limitPage) + "'>&laquo;</a>");
                    sb.Append("<a target='_self' href='" + GetPageUrl(currentPage - 1, Http1, HttpM, HttpN, limitPage) + "'>&#8249;</a>");
                }
                else
                {
                    sb.Append("<a class='disabled'>&laquo;</a>");
                    sb.Append("<a class='disabled'>&#8249;</a>");
                }
                if (stepNum > 0)
                {
                    for (int i = pageRoot; i <= pageFoot; i++)
                    {
                        if (i == currentPage)
                            sb.Append("<span class='currentpage'>" + i.ToString() + "</span>");
                        else
                            sb.Append("<a target='_self' href='" + GetPageUrl(i, Http1, HttpM, HttpN, limitPage) + "'>" + i.ToString() + "</a>");
                        if (i == pageCount)
                            break;
                    }
                }
                if (currentPage != pageCount)
                {//只要不是最后一页
                    sb.Append("<a target='_self' href='" + GetPageUrl(currentPage + 1, Http1, HttpM, HttpN, limitPage) + "'>&#8250;</a>");
                    sb.Append("<a target='_self' href='" + GetPageUrl(pageCount, Http1, HttpM, HttpN, limitPage) + "'>&raquo;</a>");
                }
                else
                {
                    sb.Append("<a class='disabled'>&#8250;</a>");
                    sb.Append("<a class='disabled'>&raquo;</a>");
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        /// <summary>
        /// 标准模式：数字+上下页+总记录信息
        /// </summary>
        /// <param name="stype"></param>
        /// <param name="stepNum"></param>
        /// <param name="pageRoot"></param>
        /// <param name="pageFoot"></param>
        /// <param name="totalCount"></param>
        /// <param name="currentPage"></param>
        /// <param name="Http1"></param>
        /// <param name="HttpM"></param>
        /// <param name="HttpN"></param>
        /// <param name="limitPage"></param>
        /// <returns></returns>
        private static string getbar2(string stype, int stepNum, int pageRoot, int pageFoot, int pageCount, int totalCount, int pageSize, int currentPage, string Http1, string HttpM, string HttpN, int limitPage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='p_btns'>");
            //sb.Append("<span class='total_count'>共" + totalCount.ToString() + "条记录，当前第" + currentPage.ToString() + "/" + pageCount.ToString() + "页&nbsp;&nbsp;</span>");
            sb.Append("<span class='total_count'>共" + totalCount.ToString() + "条记录/" + pageCount.ToString() + "页&nbsp;</span>");
            if (totalCount > pageSize)
            {
                if (currentPage != 1)
                {//只要不是第一页
                    sb.Append("<a target='_self' href='" + GetPageUrl(1, Http1, HttpM, HttpN, limitPage) + "'>&laquo;</a>");
                    sb.Append("<a target='_self' href='" + GetPageUrl(currentPage - 1, Http1, HttpM, HttpN, limitPage) + "'>&#8249;</a>");
                }
                else
                {
                    sb.Append("<a class='disabled'>&laquo;</a>");
                    sb.Append("<a class='disabled'>&#8249;</a>");
                }
                if (stepNum > 0)
                {
                    for (int i = pageRoot; i <= pageFoot; i++)
                    {
                        if (i == currentPage)
                            sb.Append("<span class='currentpage'>" + i.ToString() + "</span>");
                        else
                            sb.Append("<a target='_self' href='" + GetPageUrl(i, Http1, HttpM, HttpN, limitPage) + "'>" + i.ToString() + "</a>");
                        if (i == pageCount)
                            break;
                    }
                }
                if (currentPage != pageCount)
                {//只要不是最后一页
                    sb.Append("<a target='_self' href='" + GetPageUrl(currentPage + 1, Http1, HttpM, HttpN, limitPage) + "'>&#8250;</a>");
                    sb.Append("<a target='_self' href='" + GetPageUrl(pageCount, Http1, HttpM, HttpN, limitPage) + "'>&raquo;</a>");
                }
                else
                {
                    sb.Append("<a class='disabled'>&#8250;</a>");
                    sb.Append("<a class='disabled'>&raquo;</a>");
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        /// <summary>
        /// 完整模式：数字+上下页+首末+总记录信息+指定页码翻转
        /// </summary>
        /// <param name="stype"></param>
        /// <param name="stepNum"></param>
        /// <param name="pageRoot"></param>
        /// <param name="pageFoot"></param>
        /// <param name="totalCount"></param>
        /// <param name="currentPage"></param>
        /// <param name="Http1"></param>
        /// <param name="HttpM"></param>
        /// <param name="HttpN"></param>
        /// <param name="limitPage"></param>
        /// <returns></returns>
        private static string getbar3(string stype, int stepNum, int pageRoot, int pageFoot, int pageCount, int totalCount, int pageSize, int currentPage, string Http1, string HttpM, string HttpN, int limitPage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='p_btns'>");
            //sb.Append("<span class='total_count'>共" + totalCount.ToString() + "条，当前第" + currentPage.ToString() + "/" + pageCount.ToString() + "页&nbsp;&nbsp;&nbsp;</span>");
            sb.Append("<span class='total_count'>共" + totalCount.ToString() + "条记录/" + pageCount.ToString() + "页&nbsp;&nbsp;</span>");
            if (totalCount > pageSize)
            {
                if (currentPage != 1)
                {//只要不是第一页
                    sb.Append("<a target='_self' href='" + GetPageUrl(1, Http1, HttpM, HttpN, limitPage) + "'>&laquo;</a>");
                    sb.Append("<a target='_self' href='" + GetPageUrl(currentPage - 1, Http1, HttpM, HttpN, limitPage) + "'>&#8249;</a>");
                }
                else
                {
                    sb.Append("<a class='disabled'>&laquo;</a>");
                    sb.Append("<a class='disabled'>&#8249;</a>");
                }
                if (pageRoot > 1)
                {
                    sb.Append("<a target='_self' href='" + GetPageUrl(1, Http1, HttpM, HttpN, limitPage) + "'>1..</a>");
                }
                if (stepNum > 0)
                {
                    for (int i = pageRoot; i <= pageFoot; i++)
                    {
                        if (i == currentPage)
                            sb.Append("<span class='currentpage'>" + i.ToString() + "</span>");
                        else
                            sb.Append("<a target='_self' href='" + GetPageUrl(i, Http1, HttpM, HttpN, limitPage) + "'>" + i.ToString() + "</a>");
                        if (i == pageCount)
                            break;
                    }
                }
                if (pageFoot < pageCount)
                {
                    sb.Append("<a target='_self' href='" + GetPageUrl(pageCount, Http1, HttpM, HttpN, limitPage) + "'>.." + pageCount + "</a>");

                }
                if (currentPage != pageCount)
                {//只要不是最后一页
                    sb.Append("<a target='_self' href='" + GetPageUrl(currentPage + 1, Http1, HttpM, HttpN, limitPage) + "'>&#8250;</a>");
                    sb.Append("<a target='_self' href='" + GetPageUrl(pageCount, Http1, HttpM, HttpN, limitPage) + "'>&raquo;</a>");
                }
                else
                {
                    sb.Append("<a class='disabled'>&#8250;</a>");
                    sb.Append("<a class='disabled'>&raquo;</a>");
                }
                if (stype == "html")
                    sb.Append("<span class='jumppage'>转到第 <input type='text' name='custompage' size='2' onkeyup=\"this.value=this.value.replace(/\\D/g,'')\" onafterpaste=\"this.value=this.value.replace(/\\D/g,'')\" onkeydown=\"if(event.keyCode==13) {window.location='" + HttpN + "'.replace('<#page#>',this.value); return false;}\" /> 页</span>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        /// <summary>
        /// 专用于网站前台
        /// </summary>
        /// <param name="stype"></param>
        /// <param name="stepNum"></param>
        /// <param name="pageRoot"></param>
        /// <param name="pageFoot"></param>
        /// <param name="pageCount"></param>
        /// <param name="countNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="Http1"></param>
        /// <param name="HttpN"></param>
        /// <returns></returns>
        private static string getbar4(string stype, int stepNum, int pageRoot, int pageFoot, int pageCount, int countNum, int pageSize, int currentPage, string Http1, string HttpM, string HttpN, int limitPage, string language)
        {
            Dictionary<string, object> Lang = GetEntity(language);
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='p_btns'>");
            if (countNum > pageSize)
            {
                sb.Append("<span class='total_count'>" + ((string)Lang["page_totalinfo"]).Replace("{totalcount}", countNum.ToString()).Replace("{currentpage}", currentPage.ToString()).Replace("{totalpage}", pageCount.ToString()) + "</span>");

                if (currentPage != 1)
                {//只要不是第一页
                    sb.Append("<a target='_self' href='" + GetPageUrl(1, Http1, HttpM, HttpN, limitPage) + "'>" + ((string)Lang["page_first"]) + "</a>");
                    sb.Append("<a target='_self' href='" + GetPageUrl(currentPage - 1, Http1, HttpM, HttpN, limitPage) + "'>" + ((string)Lang["page_prev"]) + "</a>");
                }
                else
                {
                    sb.Append("<a class='disabled'>" + ((string)Lang["page_first"]) + "</a>");
                    sb.Append("<a class='disabled'>" + ((string)Lang["page_prev"]) + "</a>");
                }
                if (pageRoot > 1)
                {
                    sb.Append("<a target='_self' href='" + GetPageUrl(1, Http1, HttpM, HttpN, limitPage) + "'>1..</a>");
                }
                if (stepNum > 0)
                {
                    for (int i = pageRoot; i <= pageFoot; i++)
                    {
                        if (i == currentPage)
                            sb.Append("<span class='currentpage'>" + i.ToString() + "</span>");
                        else
                            sb.Append("<a target='_self' href='" + GetPageUrl(i, Http1, HttpM, HttpN, limitPage) + "'>" + i.ToString() + "</a>");
                        if (i == pageCount)
                            break;
                    }
                }
                if (pageFoot < pageCount)
                {
                    sb.Append("<a target='_self' href='" + GetPageUrl(pageCount, Http1, HttpM, HttpN, limitPage) + "'>.." + pageCount + "</a>");

                }
                if (currentPage != pageCount)
                {//只要不是最后一页
                    sb.Append("<a target='_self' href='" + GetPageUrl(currentPage + 1, Http1, HttpM, HttpN, limitPage) + "'>" + ((string)Lang["page_next"]) + "</a>");
                    sb.Append("<a target='_self' href='" + GetPageUrl(pageCount, Http1, HttpM, HttpN, limitPage) + "'>" + ((string)Lang["page_last"]) + "</a>");
                }
                else
                {
                    sb.Append("<a class='disabled'>" + ((string)Lang["page_next"]) + "</a>");
                    sb.Append("<a class='disabled'>" + ((string)Lang["page_last"]) + "</a>");
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }


        /// <summary>
        /// 分页导航
        /// </summary>
        /// <param name="mode">支持1=simple,2=normal,3=full</param>
        /// <param name="stype">html/js,只有当stype为html且mode为3的时候显示任意页的转向</param>
        /// <param name="stepNum">步数,如果步数为i，则每页的数字导航就有2i+1</param>
        /// <param name="totalCount">记录总数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="Http1">第1页的链接地址模板，支持js</param>
        /// <param name="HttpM">第M页的链接地址模板，支持js,M不大于limitPage</param>
        /// <param name="HttpN">第N页的链接地址模板，支持js,N大于limitPage</param>
        /// <param name="limitPage"></param>
        /// <returns></returns>
        public static string GetPageBar(int mode, string stype, int stepNum, int totalCount, int pageSize, int currentPage, string Http1, string HttpM, string HttpN, int limitPage)
        {
            string _pagebar = "";
            //if (totalCount > pageSize)
            //{
            int pageCount = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
            currentPage = currentPage > pageCount ? pageCount : currentPage;
            currentPage = currentPage < 1 ? 1 : currentPage;
            int stepageSize = stepNum * 2;
            int pageRoot = 1;
            int pageFoot = pageCount;
            pageCount = pageCount == 0 ? 1 : pageCount;
            if (pageCount - stepageSize < 1)//页数比较少
            {
                pageRoot = 1;
                pageFoot = pageCount;
            }
            else
            {
                pageRoot = currentPage - stepNum > 1 ? currentPage - stepNum : 1;
                pageFoot = pageRoot + stepageSize > pageCount ? pageCount : pageRoot + stepageSize;
                pageRoot = pageFoot - stepageSize < pageRoot ? pageFoot - stepageSize : pageRoot;
            }
            switch (mode)
            {
                case 1://1=simple：数字+上下页
                    _pagebar = getbar1(stype, stepNum, pageRoot, pageFoot, pageCount, totalCount, pageSize, currentPage, Http1, HttpM, HttpN, limitPage);
                    break;
                case 2://2=normal：数字+上下页+总记录信息
                    _pagebar = getbar2(stype, stepNum, pageRoot, pageFoot, pageCount, totalCount, pageSize, currentPage, Http1, HttpM, HttpN, limitPage);
                    break;
                case 3://3=full：数字+上下页+首末+总记录信息+指定页码翻转
                    _pagebar = getbar3(stype, stepNum, pageRoot, pageFoot, pageCount, totalCount, pageSize, currentPage, Http1, HttpM, HttpN, limitPage);
                    break;
                case 4://专用于前台中文版
                    _pagebar = getbar4(stype, stepNum, pageRoot, pageFoot, pageCount, totalCount, pageSize, currentPage, Http1, HttpM, HttpN, limitPage, "cn");
                    break;
                case 5://专用于前台英文版
                    _pagebar = getbar4(stype, stepNum, pageRoot, pageFoot, pageCount, totalCount, pageSize, currentPage, Http1, HttpM, HttpN, limitPage, "en");
                    break;
                default:
                    break;
            }
            // }
            return _pagebar;
        }
        public static string GetPageBar(int mode, string stype, int stepNum, int totalCount, int pageSize, int currentPage, string HttpN)
        {
            return GetPageBar(mode, stype, stepNum, totalCount, pageSize, currentPage, HttpN, HttpN, HttpN, 0);
        }
        public static string GetPageUrl(int chkPage, string Http1, string HttpM, string HttpN, int limitPage)
        {
            string Http = string.Empty;
            if (chkPage == 1)
                Http = Http1;
            else
                Http = (chkPage > limitPage || limitPage == 0) ? HttpN : HttpM;
            return Http.Replace("<#page#>", chkPage.ToString());
        }
        /// <summary>
        /// 绑定语言包(V6之后深入开发)
        /// </summary>
        /// <param name="_lng">如cn表示中文，en表示英文</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetEntity(string _lng)
        {
            return null;
            //string json = JumboTCMS.Utils.DirFile.ReadFile("~/_data/languages/" + _lng + ".js");
            //json = JumboTCMS.Utils.Strings.GetHtml(json, "//<!--语言包begin", "//-->语言包end");
            //return (Dictionary<string, object>)JumboTCMS.Utils.fastJSON.JSON.Instance.ToObject(json);
        }
    }   
}
