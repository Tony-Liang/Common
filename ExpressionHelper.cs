using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace LCW.Framework.Common
{
    public class ExpressionHelper<T> where T : class
    {
        public static string PropertyString<TProperty>(Expression<Func<T,TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("express");
            }
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("请为类型 \"" + typeof(T).FullName + "\" 的指定一个字段（Field）或属性（Property）作为 Lambda 的主体（Body）。");
            }
            return memberExpression.Member.Name;
        }
    }
}
