using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Specialized;

namespace LCW.Framework.Common.DataTypes.Extension
{
    [Serializable]
    public class DateDescription
    {
        public string Acronym{get;private set;}
        public string Description{get;private set;}
        public string DescriptionEn{get;private set;}

        internal DateDescription(string acronym,string description,string descriptionEn)
        {
            Acronym=acronym;
            Description=description;
            DescriptionEn=descriptionEn;
        }
    }

    public static class DateTimeExtensions
    {
        //private static readonly IList<string> MonthArrEn=new List<string>(){"Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"};     
       // private static readonly IList<string> MonthArr=new List<string>(){"一月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月"};
        //private static readonly IList<string> WeekArrEn=new List<string>(){"Sun","Mon","Tue","Web","Thu","Fri","Sat"};
        //private static readonly IList<string> WeekArr=new List<string>(){"星期日","星期一","星期二","星期三","星期四","星期五","星期六"};
       // private static readonly IList<string> WeekArr=new List<string>(){"星期日","星期一","星期二","星期三","星期四","星期五","星期六"};
       private static readonly IDictionary<string, DateDescription> week = new Dictionary<string, DateDescription>(){
            {"Sunday", new DateDescription("Sun","星期日","Sunday")},
            {"Monday",new DateDescription("Mon","星期一","Monday")},
            {"Tuesday",new DateDescription("Tue","星期二","Tuesday")},
            {"Wednesday",new DateDescription("Web","星期三","Wednesday")},
            {"Thursday",new DateDescription("Thu","星期四","Thursday")},
            {"Friday",new DateDescription("Fri","星期五","Friday")},
            {"Saturday",new DateDescription("Sat","星期六","Saturday")}
        };
       private static readonly IDictionary<string, DateDescription> month = new Dictionary<string, DateDescription>(){
            {"January",new DateDescription("Jan","一月","January")},
            {"February",new DateDescription("Feb","二月","February")},
            {"March",new DateDescription("Mar","三月","March")},
            {"April",new DateDescription("Apr","四月","April")},
            {"May",new DateDescription("May","五月","May")},
            {"June",new DateDescription("Jun","六月","June")},
            {"July",new DateDescription("Jul","七月","July")},
            {"August",new DateDescription("Aug","八月","August")},
            {"September",new DateDescription("Sep","九月","September")},
            {"October",new DateDescription("Oct","十月","JuOctoberne")},
            {"November",new DateDescription("Nov","十一月","November")},
            {"December",new DateDescription("Dec","十二月","December")}
        };
        /// <summary>
        /// Format yyyy年MM月dd日 HH:mm:ss
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static string FormatToSecond(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Date.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        /// <summary>
        /// Format yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static string FormatToSecondEN(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Format yyyy年 MMM dd EEE
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static string FormatToWeek(this DateTime Date)
        {
            Date.ThrowIfNull("Date");

            return Date.ToString("yyyy年MM月dd日", DateTimeFormatInfo.InvariantInfo) + " " +week[Date.ToString("dddd", DateTimeFormatInfo.InvariantInfo)].Description;
        }

        /// <summary>
        /// Format yyyy年 MMM dd EEE
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static string FormatToWeekEN(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Date.ToString("D", DateTimeFormatInfo.InvariantInfo);
        }
    }   
}
//格式模式 说明
//d 月中的某一天。一位数的日期没有前导零。
//dd 月中的某一天。一位数的日期有一个前导零。
//ddd 周中某天的缩写名称，在 AbbreviatedDayNames 中定义。
//dddd 周中某天的完整名称，在 DayNames 中定义。
//M 月份数字。一位数的月份没有前导零。
//MM 月份数字。一位数的月份有一个前导零。
//MMM 月份的缩写名称，在 AbbreviatedMonthNames 中定义。
//MMMM 月份的完整名称，在 MonthNames 中定义。
//y 不包含纪元的年份。如果不包含纪元的年份小于 10，则显示不具有前导零的年份。
//yy 不包含纪元的年份。如果不包含纪元的年份小于 10，则显示具有前导零的年份。
//yyyy 包括纪元的四位数的年份。
//gg 时期或纪元。如果要设置格式的日期不具有关联的时期或纪元字符串，则忽略该模式。
//h 12 小时制的小时。一位数的小时数没有前导零。
//hh 12 小时制的小时。一位数的小时数有前导零。
//H 24 小时制的小时。一位数的小时数没有前导零。
//HH 24 小时制的小时。一位数的小时数有前导零。
//m 分钟。一位数的分钟数没有前导零。
//mm 分钟。一位数的分钟数有一个前导零。
//s 秒。一位数的秒数没有前导零。
//ss 秒。一位数的秒数有一个前导零。
//f 秒的小数精度为一位。其余数字被截断。
//ff 秒的小数精度为两位。其余数字被截断。
//fff 秒的小数精度为三位。其余数字被截断。
//ffff 秒的小数精度为四位。其余数字被截断。
//fffff 秒的小数精度为五位。其余数字被截断。
//ffffff 秒的小数精度为六位。其余数字被截断。
//fffffff 秒的小数精度为七位。其余数字被截断。
//t 在 AMDesignator 或 PMDesignator 中定义的 AM/PM 指示项的第一个字符（如果存在）。
//tt 在 AMDesignator 或 PMDesignator 中定义的 AM/PM 指示项（如果存在）。
//z 时区偏移量（“+”或“-”后面仅跟小时）。一位数的小时数没有前导零。例如，太平洋标准时间是“-8”。
//zz 时区偏移量（“+”或“-”后面仅跟小时）。一位数的小时数有前导零。例如，太平洋标准时间是“-08”。
//zzz 完整时区偏移量（“+”或“-”后面跟有小时和分钟）。一位数的小时数和分钟数有前导零。例如，太平洋标准时间是“-08:00”。
//: 在 TimeSeparator 中定义的默认时间分隔符。
/// 在 DateSeparator 中定义的默认日期分隔符。
//% c 其中 c 是格式模式（如果单独使用）。如果格式模式与原义字符或其他格式模式合并，则可以省略“%”字符。
/// c 其中 c 是任意字符。照原义显示字符。若要显示反斜杠字符，请使用“//”。