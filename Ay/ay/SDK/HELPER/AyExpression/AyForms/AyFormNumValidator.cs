
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ay.AyExpression
{
    public class AyFormNumValidator : AyFormValidator
    {
        public const string NumberRegex = @"^(\-|\+)?\d+(\.\d+)?$";
        public AyFormNumValidator()
        {
            Example = "示例: 整数12   小数12.34,   负数-3";
            ErrorInfo = AyFormErrorTemplate.Num;
        }

        public override bool Validate(string text, string expression = "")
        {
            bool returnResult = true;

            bool isNumber = IsMatch(text, NumberRegex);
            if (!isNumber)
            {
                ErrorInfo = AyFormErrorTemplate.Num;
                return false;
            }
            //处理num的规则
            if (expression == "num") //纯数字处理
            {
                ErrorInfo = AyFormErrorTemplate.Num;
                return isNumber;
            }

            if (expression.IndexOf("num(") >= 0)
            {
                //规则 num(3) 等同于(3,0)
                int nextEndHu = expression.IndexOfAny(new char[] { ')' }, 0);
                string subAd = expression.Substring(0, (nextEndHu + 1));  //这是截取后部分[]限制的依据
                string xy = null;
                MatchCollection matches = Regex.Matches(subAd, @"num\((?<ch>.+)\)");
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    xy = groups["ch"].Value;
                    break;
                }
                if (!xy.IsNullAndTrimAndEmpty())
                {
                    var sl = xy.Split(',');
         

                    int _sl2 = 0;

                    if (sl.Length == 2)
                    {
                        _sl2 = sl[1].ToInt();
                    }
                    else if (sl.Length == 0)
                    {
                        throw new Exception("num规则不正确,无法解析");
                    }
                    bool numberFloat = IsNumber(text, sl[0],  _sl2);
        
                  
                    if (!numberFloat)
                    {
                        var _1 = text.Split('.');
                        if (_1.Length == 1)
                        {
                            ErrorInfo = AyFormErrorTemplate.NumFloat1.StringFormat(sl[0]);
                            return false;
                        }

                        int _3 = _1[1].ToString().Length;
                        //if (_3 == 0)
                        //{
                        //    ErrorInfo = AyFormErrorTemplate.NumFloat1.StringFormat(_sl1);
                        //    return false;
                        //}
                        if (_3 > _sl2)
                        {
                            if (_sl2 == 0)
                            {
                                ErrorInfo = AyFormErrorTemplate.NumFloat3.StringFormat(_sl2);
                            }
                            else
                            {
                                ErrorInfo = AyFormErrorTemplate.NumFloat2.StringFormat(_sl2);
                            }

                        }
                        else
                        {
                            ErrorInfo = AyFormErrorTemplate.NumFloat1.StringFormat(sl[0]);
                        }
                        return numberFloat;
                    }
                    string prex = subAd + "[";
                    if (expression.IndexOf(prex) >= 0)
                    {
                        double _value = text.ToDouble();
                        int exLength = expression.Length;
                        string subAd3 = expression.Substring(prex.Length, (exLength - prex.Length - 1));
                        return ValidateStrict(_value, expression, subAd3);
                    }
                }

            }
            else if (expression.IndexOf("num[") >= 0)
            {
                double _value = text.ToDouble();
                int exLength = expression.Length;
                string subAd = expression.Substring(4, (exLength - 5));
                return ValidateStrict(_value, expression, subAd);
            }

            return returnResult;
        }

        private bool ValidateStrict(double _value, string expression, string subAd)
        {
            var sl = subAd.Split(',');

            foreach (var _sl in sl)
            {
                var _sls = _sl.Split(' ');
                if (_sls.Length != 2)
                {
                    throw new InvalidOperationException(AyExpression.exceptionStr + expression);
                }
                var fun = _sls[0];
                //第二个值获取
                double fun2 = _sls[1].ToDouble();

                if (AyExpression.formNumOperation.ContainsKey(fun))
                {
                    var _d = AyExpression.formNumOperation[fun](_value, fun2);
                    if (!_d)
                    {
                        if (fun != "@e" && fun != "@ne")
                        {
                            ErrorInfo = "你输入的值应该" + AyExpression.formNumOperationDes[fun] + fun2;
                        }
                        else if (fun == "@e")
                        {
                            ErrorInfo = "你输入的值不等于" + fun2;
                        }
                        else if (fun == "@ne")
                        {
                            ErrorInfo = "你输入的值不应该等于" + fun2;
                        }

                        return false;
                    }
                }
                else
                {
                    throw new InvalidOperationException("[AY表单表达式错误],无效的num运算符：\"" + fun + "\",在表达式:" + expression + "中");
                }
            }
            return true;
        }



        #region AY 2016-8-6 09:52:47拓展验证  浮点型
        /// <summary>  
        /// 判断一个字符串是否为合法整数(不限制长度)  
        /// </summary>  
        /// <param name="s">字符串</param>  
        /// <returns></returns>  
        public static bool IsInteger(string s)
        {
            string pattern = @"^\d*$";
            return Regex.IsMatch(s, pattern);
        }
        /**//// <summary>  
            /// 判断一个字符串是否为合法数字(0-32整数)  
            /// </summary>  
            /// <param name="s">字符串</param>  
            /// <returns></returns>  
        public static bool IsNumber(string s)
        {
            return IsNumber(s, "32", 0);
        }
        /**//// <summary>  
            /// 判断一个字符串是否为合法数字(指定整数位数和小数位数)  
            /// </summary>  
            /// <param name="s">字符串</param>  
            /// <param name="precision">整数位数</param>  
            /// <param name="scale">小数位数</param>  
            /// <returns></returns>  
        public static bool IsNumber(string s, string precision, int scale)
        {
            if ((precision == "0") && (scale == 0))
            {
                return false;
            }
            if (precision == "?")
            {
                precision = "";
            }
            string pattern = @"(^(\-|\+)?\d{1," + precision + "}";
            if (scale > 0)
            {
                pattern += @"\.\d{0," + scale + "}$)|" + pattern;
            }
            pattern += "$)";
            return Regex.IsMatch(s, pattern);
        }
        #endregion


    }
}
