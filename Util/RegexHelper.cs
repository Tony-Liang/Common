using System.Text.RegularExpressions;

namespace LCW.Framework.Common.Util
{
    /// <summary>
    /// 操作正则表达式的公共类
    /// </summary>    
    public class RegexHelper
    {
        #region 验证输入字符串是否与模式字符串匹配
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
        #endregion

        /// <summary>
        /// 非负整数（正整数 + 0）   "^\d+$"
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Nonnegative_Integer(string target)
        {
            return IsMatch(target,@"^\d+$");
        }
        /// <summary>
        /// 正整数   "^[0-9]*[1-9][0-9]*$"
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Positive_Integer(string target)
        {
            return IsMatch(target,@"^[0-9]*[1-9][0-9]*$");
        }
        /// <summary>
        /// 非正整数（负整数 + 0）   "^((-/d+)|(0+))$"
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool NonPositive_Integer(string target)
        {
            return IsMatch(target, @"^((-/d+)|(0+))$");
        }
        /// <summary>
        /// 负整数   "^-[0-9]*[1-9][0-9]*$"　
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Negative_Integer(string target)
        {
            return IsMatch(target, @"^-[0-9]*[1-9][0-9]*$");
        }
        /// <summary>
        /// "^-?/d+$"　　　　//整数
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Integer(string target)
        {
            return IsMatch(target, @"^-?/d+$");
        }
        /// <summary>
        /// "^/d+(/./d+)?$"　　//非负浮点数（正浮点数 + 0）
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Nonnegativefloatingpoint(string target)
        {
            return IsMatch(target, @"^/d+(/./d+)?$");
        }
        /// <summary>
        ///"^(([0-9]+/.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*/.[0-9]+)|([0-9]*[1-9][0-9]*))$"　　//正浮点数 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Arefloatingpoint(string target)
        {
            return IsMatch(target, @"^(([0-9]+/.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*/.[0-9]+)|([0-9]*[1-9][0-9]*))$");
        }
        /// <summary>
        /// "^((-/d+(/./d+)?)|(0+(/.0+)?))$"　　//非正浮点数（负浮点数 + 0） 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Nonpositivefloatingpoint(string target)
        {
            return IsMatch(target, @"^((-/d+(/./d+)?)|(0+(/.0+)?))$");
        }
        /// <summary>
        /// @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"  //验证邮箱
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsEmail(string target)
        {
            return IsMatch(target, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsIP(string target)
        {
            string num = "(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)"; 
            return IsMatch(target,("^" + num + "\\." + num + "\\." + num + "\\." + num + "$")); 
        }
        /// <summary>
        /// @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsUrl(string target)
        {
            return IsMatch(target, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }
        /// <summary>
        /// @"^(\d{3,4}-)?\d{6,8}$"
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool IsTelephone(string target)
        {
            return IsMatch(target, @"^(\d{3,4}-)?\d{6,8}$");
        }
        /// <summary>
        ///  "[A-Za-z]+[0-9]" 输入密码条件(字符与数据同时出现)
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsPassword(string target)
        {
            return IsMatch(target, @"[A-Za-z]+[0-9]");
        }
        /// <summary>
        ///  "^\d{6}$" 邮政编号
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsPostalcode(string target)
        {
            return IsMatch(target, @"^\d{6}$");
        }
        /// <summary>
        /// "^[1]+[3,5]+\d{9}$"  手机号码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsMobilephone(string target)
        { 
            return IsMatch(target, @"^[1]+[3,5]+\d{9}$");
        }
        /// <summary>
        /// (^\d{18}$)|(^\d{15}$) 身份证号
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsIDcard(string target)
        { 
            return IsMatch(target, @"(^\d{18}$)|(^\d{15}$)");
        }
        /// <summary>
        /// ^[0-9]+(.[0-9]{2})?$    两位小数
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsDecimal(string target)
        { 
            return IsMatch(target, @"^[0-9]+(.[0-9]{2})?$");
        }
        /// <summary>
        /// "^[0-9]*$" 数字
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNumber(string target)
        { 
            return IsMatch(target, @"^[0-9]*$");
        }
        /// <summary>
        /// "^\d{6,18}$")  密码长度 (6-18位)
        /// </summary>
        /// <param name="str_Length"></param>
        /// <returns></returns>
        public bool IsPasswLength(string target)
        {
            return IsMatch(target, @"^\d{6,18}$");
        }
        /// <summary>
        /// "^\+?[1-9][0-9]*$"   非零的正整数
        /// </summary>
        /// <param name="str_intNumber"></param>
        /// <returns></returns>
        public bool IsIntNumber(string target)
        {
            return IsMatch(target, @"^\+?[1-9][0-9]*$");
        }
        /// <summary>
        /// 大写字母  "^[A-Z]+$"
        /// </summary>
        /// <param name="str_UpChar"></param>
        /// <returns></returns>
        public bool IsUpChar(string target)
        {
            return IsMatch(target, @"^[A-Z]+$");
        } 
        /// <summary>
        /// 小写字母  "^[a-z]+$"
        /// </summary>
        /// <param name="str_UpChar"></param>
        /// <returns></returns>
        public bool IsLowChar(string target)
        {
            return IsMatch(target, @"^[a-z]+$");
        }
        /// <summary>
        /// 验证输入字母  "^[A-Za-z]+$"
        /// </summary>
        /// <param name="str_Letter"></param>
        /// <returns></returns>
        public bool IsLetter(string target)
        {
            return IsMatch(target, @"^[A-Za-z]+$");
        } 
        /// <summary>
        /// 验证输入汉字  ^[\u4e00-\u9fa5],{0,}$
        /// </summary>
        /// <param name="str_chinese"></param>
        /// <returns></returns>
        public bool IsChinese(string target)
        {
            return IsMatch(target, @"^[\u4e00-\u9fa5]+$"); //@"^[\u4e00-\u9fa5],{0,}$");
        } 
        /// <summary>
        /// 验证输入字符串 (至少8个字符)  "^.{8,}$"
        /// </summary>
        /// <param name="str_Length"></param>
        /// <returns></returns>
        public bool IsLength(string target)
        {
            return IsMatch(target, @"^.{8,}$");
        } 

    }
}
