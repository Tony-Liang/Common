using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace LCW.Framework.Common.Web
{
    public class StringUtility
    {
        //去掉前面的全角空格
        public static string RemoveLeftSpace(string str)
        {
            string s = str.Trim();
            while (s.StartsWith(((char)12288).ToString()))
            {
                s = s.Substring(1);
            }
            return s;
        }
        //所有全角空格替换成半角空格
        public static string FullWidthToHalfWidth(string str)
        {
            byte[] t = Encoding.Default.GetBytes(str);
            for (int i = 0; i < t.Length; i++)
            {
                if ((t[i].ToString() == "161") && (t[i + 1].ToString() == "161"))
                {
                    t[i] = 32;
                    if (i + 1 == t.Length - 1)
                    {
                        t[i + 1] = 0;
                    }
                    else
                    {
                        for (int j = i + 1; j + 1 < t.Length; j++)
                        {
                            t[j] = t[j + 1];
                            if (j + 1 == t.Length - 1)
                            {
                                t[j + 1] = 0;
                            }
                        }
                    }
                }
            }
            return Encoding.Default.GetString(t);
        }

        /// <summary>
        /// 按字节数取出字符串的长度
        /// </summary>
        /// <param name="strTmp">要计算的字符串</param>
        /// <returns>字符串的字节数</returns>
        public static int GetByteCount(string strTmp)
        {
            int intCharCount = 0;
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (System.Text.UTF8Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intCharCount = intCharCount + 2;
                }
                else
                {
                    intCharCount = intCharCount + 1;
                }
            }
            return intCharCount;
        }
        /// <summary>
        /// 删除不可见字符
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string DeleteUnVisibleChar(string sourceString)
        {
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder(131);
            for (int i = 0; i < sourceString.Length; i++)
            {
                int Unicode = sourceString[i];
                if (Unicode >= 16)
                {
                    sBuilder.Append(sourceString[i].ToString());
                }
            }
            return sBuilder.ToString();
        }
        /// <summary>
        /// 过长的字符串，显示一部分，后以...结束
        /// 使用： <%# PLeft(Eval("Profile"), 56)%>
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string PLeft(string content, int length)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }
            if (content.Length > length)
            {
                content = content.Substring(0, length - 2) + "...";
            }
            return content;
        }

        /// <summary>
        ///  根据位数左补齐
        /// </summary>
        /// <param name="str">要补齐的字符</param>
        /// <param name="num">位数</param>
        /// <returns></returns>
        public static string LeftAppend(string str, int num)
        {
            string newStr = "";
            for (int i = 0; i < num; i++)
            {
                newStr += "0";
            }
            newStr += str;
            newStr = newStr.Substring(str.Length);
            return newStr;
        }
        //是否仅仅包含数字
        public static bool IncludeNumeric(string str)
        {
            foreach (char c in str)
            {
                if (Char.IsNumber(c))
                {
                    return true;
                }
            }
            return false;
        }
        //是否包含汉字
        public static bool IncludeHZ(string str)
        {
            Regex reg = new Regex(@"[\u4e00-\u9fa5]");//正则表达式

            if (reg.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region 日期操作
        public static bool IsDate(string vDateStr)
        {
            bool r = true;
            try
            {
                DateTime mDt = DateTime.Parse(vDateStr);
            }
            catch (FormatException)
            {
                r = false;
            }
            return r;
        }

        public static string GetDateFormat(string theTime)
        {
            if (string.IsNullOrWhiteSpace(theTime)) return "";
            if (!IsDate(theTime)) return "";
            return Convert.ToDateTime(theTime).ToString("yyyy-MM-dd");
        }
        public static int GetDiffDays(string bTime, string eTime)
        {
            if (string.IsNullOrWhiteSpace(bTime) && string.IsNullOrWhiteSpace(eTime)) return 0;
            if (string.IsNullOrWhiteSpace(bTime)) bTime = "2013-05-01";
            if (string.IsNullOrWhiteSpace(eTime))
            {
                eTime = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (!IsDate(bTime) && !IsDate(eTime)) return 0;
            DateTime d1 = DateTime.Parse(bTime);
            DateTime d2 = DateTime.Parse(eTime);
            System.TimeSpan ND = d2 - d1;
            return ND.Days;   //天数差
        }
        /// <summary>
        /// 返回两个日期之间相差的天数
        /// </summary>
        /// <param name="dtfrm">两个日期参数</param>
        /// <param name="dtto">两个日期参数</param>
        /// <returns>天数</returns>
        public static int GetDiffDays(DateTime dtfrm, DateTime dtto)
        {
            TimeSpan tsDiffer = dtto.Date - dtfrm.Date;
            return tsDiffer.Days;
        }
        /// <summary>返回本年有多少天</summary>
        /// <param name="iYear">年份</param>
        /// <returns>本年的天数</returns>
        public static int GetDaysOfYear(int iYear)
        {
            return IsRuYear(iYear) ? 366 : 365;
        }


        /// <summary>本年有多少天</summary>
        /// <param name="dt">日期</param>
        /// <returns>本天在当年的天数</returns>
        public static int GetDaysOfYear(DateTime dt)
        {
            return IsRuYear(dt.Year) ? 366 : 365;
        }


        /// <summary>本月有多少天</summary>
        /// <param name="iYear">年</param>
        /// <param name="Month">月</param>
        /// <returns>天数</returns>
        public static int GetDaysOfMonth(int iYear, int Month)
        {
            var days = 0;
            switch (Month)
            {
                case 1:
                    days = 31;
                    break;
                case 2:
                    days = IsRuYear(iYear) ? 29 : 28;
                    break;
                case 3:
                    days = 31;
                    break;
                case 4:
                    days = 30;
                    break;
                case 5:
                    days = 31;
                    break;
                case 6:
                    days = 30;
                    break;
                case 7:
                    days = 31;
                    break;
                case 8:
                    days = 31;
                    break;
                case 9:
                    days = 30;
                    break;
                case 10:
                    days = 31;
                    break;
                case 11:
                    days = 30;
                    break;
                case 12:
                    days = 31;
                    break;
            }

            return days;
        }


        /// <summary>本月有多少天</summary>
        /// <param name="dt">日期</param>
        /// <returns>天数</returns>
        public static int GetDaysOfMonth(DateTime dt)
        {
            //--------------------------------//
            //--从dt中取得当前的年，月信息  --//
            //--------------------------------//
            int days = 0;
            int year = dt.Year;
            int month = dt.Month;

            //--利用年月信息，得到当前月的天数信息。
            switch (month)
            {
                case 1:
                    days = 31;
                    break;
                case 2:
                    days = IsRuYear(year) ? 29 : 28;
                    break;
                case 3:
                    days = 31;
                    break;
                case 4:
                    days = 30;
                    break;
                case 5:
                    days = 31;
                    break;
                case 6:
                    days = 30;
                    break;
                case 7:
                    days = 31;
                    break;
                case 8:
                    days = 31;
                    break;
                case 9:
                    days = 30;
                    break;
                case 10:
                    days = 31;
                    break;
                case 11:
                    days = 30;
                    break;
                case 12:
                    days = 31;
                    break;
            }
            return days;
        }


        /// <summary>返回当前日期的星期名称</summary>
        /// <param name="dt">日期</param>
        /// <returns>星期名称</returns>
        public static string GetWeekNameOfDay(DateTime dt)
        {
            string week = string.Empty;
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    week = "星期四";
                    break;
                case DayOfWeek.Friday:
                    week = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    week = "星期六";
                    break;
                case DayOfWeek.Sunday:
                    week = "星期日";
                    break;
            }
            return week;
        }


        /// <summary>返回当前日期的星期编号</summary>
        /// <param name="dt">日期</param>
        /// <returns>星期数字编号</returns>
        public static int GetWeekNumberOfDay(DateTime dt)
        {
            int week = 0;
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = 1;
                    break;
                case DayOfWeek.Tuesday:
                    week = 2;
                    break;
                case DayOfWeek.Wednesday:
                    week = 3;
                    break;
                case DayOfWeek.Thursday:
                    week = 4;
                    break;
                case DayOfWeek.Friday:
                    week = 5;
                    break;
                case DayOfWeek.Saturday:
                    week = 6;
                    break;
                case DayOfWeek.Sunday:
                    week = 7;
                    break;
            }
            return week;
        }

        /// <summary>
        /// 获取某一年有多少周
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>该年周数</returns>
        public static int GetWeekAmount(int year)
        {
            var end = new DateTime(year, 12, 31); //该年最后一天
            var gc = new GregorianCalendar();
            return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday); //该年星期数
        }

        /// <summary>
        /// 获取某一日期是该年中的第几周
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>该日期在该年中的周数</returns>
        public static int GetWeekOfYear(DateTime dt)
        {
            var gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// 根据某年的第几周获取这周的起止日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOrder"></param>
        /// <param name="firstDate"></param>
        /// <param name="lastDate"></param>
        /// <returns></returns>
        public static void WeekRange(int year, int weekOrder, ref DateTime firstDate, ref DateTime lastDate)
        {
            //当年的第一天
            var firstDay = new DateTime(year, 1, 1);

            //当年的第一天是星期几
            int firstOfWeek = Int32.Parse(firstDay.DayOfWeek.ToString());

            //计算当年第一周的起止日期，可能跨年
            int dayDiff = (-1) * firstOfWeek + 1;
            int dayAdd = 7 - firstOfWeek;

            firstDate = firstDay.AddDays(dayDiff).Date;
            lastDate = firstDay.AddDays(dayAdd).Date;

            //如果不是要求计算第一周
            if (weekOrder != 1)
            {
                int addDays = (weekOrder - 1) * 7;
                firstDate = firstDate.AddDays(addDays);
                lastDate = lastDate.AddDays(addDays);
            }
        }

   

        /// <summary>判断当前年份是否是闰年，私有函数</summary>
        /// <param name="iYear">年份</param>
        /// <returns>是闰年：True ，不是闰年：False</returns>
        private static bool IsRuYear(int iYear)
        {
            //形式参数为年份
            //例如：2003
            int n = iYear;
            return (n % 400 == 0) || (n % 4 == 0 && n % 100 != 0);
        }

        #endregion

        public static string GetSubString(string str, int length, bool flag)
        {
            string temp = str;
            int j = 0;
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= length)
                {
                    k += 1;
                }
                if (j >= length)
                {
                    temp = temp.Substring(0, k);
                    break;
                }
            }
            if (flag)
            {
                temp += "...";
            }
            return temp;
        }

        public static string GetSubStringD(string str, int length, bool flag)
        {
            string temp = str;
            int j = 0;
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] < 27 || temp[i] > 126)
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= length)
                {
                    k += 1;
                }
                if (j >= length)
                {
                    temp = temp.Substring(0, k);
                    break;
                }
            }
            if (flag)
            {
                temp += "...";
            }
            return temp;
        }

    }
}
