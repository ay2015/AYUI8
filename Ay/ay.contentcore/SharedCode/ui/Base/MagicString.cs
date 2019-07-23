
using System;
using System.Linq.Expressions;
using System.Windows;

namespace ay.Controls
{
    public static class MagicString
    {
        public static string Get<T>(Expression<Func<T, object>> ex)
        {
            string name;
            switch (ex.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    name = ex.Body.ToString();
                    break;
                case ExpressionType.Convert:
                    name = ((UnaryExpression)ex.Body).Operand.ToString();
                    break;
                default:
                    throw new Exception(String.Format("Expression type {0} unknown", ex.Body.NodeType));
            }

            name = name.Substring(name.IndexOf('.') + 1);    // remove the lambda name from expression (d=>d.Test to Test)
            return name;
        }
        public static void Bind<T>(this FrameworkElement el, DependencyProperty dp, Expression<Func<T, object>> ex)
        {
            el.SetBinding(dp, Get<T>(ex));
        }
    }
}
