using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.DataTypes.Extension
{
    public static class StringExtensions
    {
        public static string Left(this string Input, int Length)
        {
            return string.IsNullOrEmpty(Input) ? "" : Input.Substring(0, Input.Length > Length ? Length : Input.Length);
        }

        public static string Right(this string Input, int Length)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            Length = Input.Length > Length ? Length : Input.Length;
            return Input.Substring(Input.Length - Length, Length);
        }

        public static byte[] ToByteArray(this string Input, Encoding EncodingUsing)
        {
            return string.IsNullOrEmpty(Input) ? null : EncodingUsing==null?(new UTF8Encoding()).GetBytes(Input):EncodingUsing.GetBytes(Input);
        }
    }
}
