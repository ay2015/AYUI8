/*
             转换器 ========== WPF时候会做
            ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang=======请不要转载谢谢了。=========
            作者：AY  aaronyang    杨洋 安徽 合肥
            联系QQ: 875556003
            时间：2016-7-27 14:46:17
            官网：www.ayjs.net
            最后修改：
            2016-9-27 23:26:27  增加密码验证器
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ay.AyExpression
{
    public class AyExpression
    {
        #region AY掩码表达式
        static string specChar = "@#%*$?\\()";
        #endregion

        #region AY表单表达式初始化值
        //const string formspecMask = "☆★∩∈≌∠◇◆";
        public static Dictionary<string, Func<double, double, bool>> formNumOperation = new Dictionary<string, Func<double, double, bool>>
        {
            {"@e",(x,y)=> {return x==y;} },
            {"@ge",(x,y)=> {return x>=y;} },
            {"@g",(x,y)=> {return x>y;} },
            {"@le",(x,y)=> {return x<=y;} },
            {"@l",(x,y)=> {return x<y;} },
            {"@ne",(x,y)=> {return x!=y;} }
        };
        public static Dictionary<string, string> formNumOperationDes = new Dictionary<string, string>
        {
            {"@e","等于"},
            {"@ge",">=" },
            {"@g",">" },
            {"@le","<=" },
            {"@l","<"},
            {"@ne","不等于" }
        };


        internal static Dictionary<AyFormValidatorTypes, AyFormValidator> validators = new Dictionary<AyFormValidatorTypes, AyFormValidator>
        {
            { AyFormValidatorTypes.required, new AyFormRequiredValidator()},
            { AyFormValidatorTypes.email, new AyFormEmailValidator()},
            { AyFormValidatorTypes.IDCard, new AyFormIDCardValidator()},
            { AyFormValidatorTypes.num, new AyFormNumValidator()},
            { AyFormValidatorTypes.length, new AyFormLengthValidator()},
            { AyFormValidatorTypes.tel, new AyFormTelValidator()},
            { AyFormValidatorTypes.tel2, new AyFormTel2Validator()},
            { AyFormValidatorTypes.chinaTel, new AyFormChinaTelValidator()},
            { AyFormValidatorTypes.fax, new AyFormFaxValidator()},
            { AyFormValidatorTypes.Ip, new AyFormIpValidator()},
            { AyFormValidatorTypes.IpSec, new AyFormIpSecValidator()},
            { AyFormValidatorTypes.time, new AyFormTimeValidator()},
            { AyFormValidatorTypes.date, new AyFormDateValidator()},
            { AyFormValidatorTypes.username, new AyFormUserNameValidator()},
             { AyFormValidatorTypes.zip, new AyFormZipValidator()},
             { AyFormValidatorTypes.integer, new AyFormIntegerValidator()},
             { AyFormValidatorTypes.integerZero, new AyFormIntegerZeroValidator()},
             { AyFormValidatorTypes.QQ, new AyFormQQValidator()},
             { AyFormValidatorTypes.age, new AyFormAgeValidator()},
             { AyFormValidatorTypes.password, new AyFormPasswordValidator()}
            
        };
        internal const string exceptionStr = "[AY表单表达式错误],遇到了无法识别的规则:";
        static Dictionary<string, AyFormValidatorTypes> typesCache = new Dictionary<string, AyFormValidatorTypes>();

        /// <summary>
        /// 拓展用户自定义验证
        /// </summary>
        public static Dictionary<string, AyFormValidator> CustomValidators = new Dictionary<string, AyFormValidator>();

        #endregion
        //动态补充规则，暂时不支持


        /// <summary>
        /// 表单验证表达式，所属AY表达式
        /// 作者:AY
        /// 时间：2016-7-28 14:37:16
        /// </summary>
        /// <param name="ayexpression">AY表单表达式</param>
        /// <param name="text">被验证的文本</param>
        /// <returns>AyFormResult验证结果</returns>
        public static AyFormResult GetFormValidateResult(string ayexpression, string text)
        {
            //版权引入TODO

            AyFormResult result = new AyFormResult();
            if (ayexpression.IsNullAndTrimAndEmpty())
            {
                return result;
            }
            int isReq = ayexpression.IndexOf("required");
            if ((ayexpression.Length > 0 && isReq >= 0) || (isReq < 0 && ayexpression.Length > 0 && text.ToObjectString().Trim().Length > 0))
            {
                ayexpression = ayexpression.TrimEnd(';');
                var ads = ayexpression.Split(';');

                foreach (var subRule in ads)
                {
                    //自定义的是否有
                    if (CustomValidators.ContainsKey(subRule))
                    {
                        #region 让用户可以自定义验证规则
                        var _t = AyExpression.CustomValidators[subRule];
                        if (_t.IsNotNull())
                        {
                            var _1 = _t.Validate(text, subRule);
                            if (!_1)
                            {
                                result.Result = false;
                                result.Example = _t.Example;
                                result.Error = _t.ErrorInfo;
                                result.ValidatorType = _t.GetType();
                                break;
                            }
                        } 
                        #endregion
                    }
                    else
                    {
                        #region AY表达式内置验证
                        if (subRule.IndexOf("num") >= 0)
                        {
                            var _t = AyExpression.validators[AyFormValidatorTypes.num];
                            if (_t.IsNotNull())
                            {
                                var _1 = _t.Validate(text, subRule);
                                if (!_1)
                                {
                                    result.Result = false;
                                    result.Example = _t.Example;
                                    result.Error = _t.ErrorInfo;
                                    result.ValidatorType = _t.GetType();
                                    break;
                                }
                            }
                        }
                        else if (subRule.IndexOf("length") >= 0)
                        {
                            var _t = AyExpression.validators[AyFormValidatorTypes.length];
                            if (_t.IsNotNull())
                            {
                                var _1 = _t.Validate(text, subRule);
                                if (!_1)
                                {
                                    result.Result = false;
                                    result.Example = _t.Example;
                                    result.Error = _t.ErrorInfo;
                                    result.ValidatorType = _t.GetType();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            AyFormValidatorTypes curType = AyFormValidatorTypes.required;
                            if (typesCache.ContainsKey(subRule))
                            {
                                curType = typesCache[subRule];
                            }
                            else
                            {
                                try
                                {
                                    curType = (AyFormValidatorTypes)Enum.Parse(typeof(AyFormValidatorTypes), subRule);
                                }
                                catch
                                {
                                    throw new InvalidCastException(exceptionStr + subRule);
                                }
                            }
                            if (subRule == curType.ToString())
                            {
                                var _t = AyExpression.validators[curType];
                                if (_t.IsNotNull())
                                {
                                    var _1 = _t.Validate(text);
                                    if (!_1)
                                    {
                                        result.Result = false;
                                        result.Example = _t.Example;
                                        result.Error = _t.ErrorInfo;
                                        result.ValidatorType = _t.GetType();
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            return result;
        }


        #region 掩码方法 和 辅助方法   2016-7-22 到 2016-7-27 完成
        /// <summary>
        /// 掩码验证表达式，所属AY表达式
        /// </summary>
        /// <param name="ayexpression">AY表达式</param>
        /// <param name="text">被验证的文本</param>
        /// <returns>返回表达式执行后的结果</returns>
        public static StringBuilder GetMaskedValue(string ayexpression, string text)
        {
            if (text.IsNull())
            {
                return new StringBuilder();
            }
            //版权引入TODO
            if (ayexpression.IsNull())
            {
                return new StringBuilder(text);
            }
            int ulength = text.Length;
            StringBuilder result = new StringBuilder();
            int uIndex = 0;
            int axlength = ayexpression.Length;
            for (int i = 0; i < axlength; i++)
            {
                if (uIndex >= ulength)
                {
                    int sd = ayexpression.Length - i;
                    if (sd > 0)
                    {
                        string _d = ayexpression.Substring(i, sd);
                        StringBuilder sbNext = new StringBuilder();
                        //检测剩余表达式，直接过滤
                        MatchCollection m0 = Regex.Matches(_d, @"\*\((?<ch>.+)\)");

                        List<string> filterString = new List<string>();
                        foreach (Match match in m0)
                        {
                            GroupCollection groups = match.Groups;
                            filterString.Add(groups[0].Value);
                        }
                        MatchCollection m1 = Regex.Matches(_d, @"\#\((?<ch>.+)\)");
                        foreach (Match match in m1)
                        {
                            GroupCollection groups = match.Groups;
                            filterString.Add(groups[0].Value);
                        }
                        MatchCollection m2 = Regex.Matches(_d, @"\%\((?<ch>.+)\)");
                        foreach (Match match in m2)
                        {
                            GroupCollection groups = match.Groups;
                            filterString.Add(groups[0].Value);
                        }
                        MatchCollection m3 = Regex.Matches(_d, @"\@\((?<ch>.+)\)");
                        foreach (Match match in m3)
                        {
                            GroupCollection groups = match.Groups;
                            filterString.Add(groups[0].Value);
                        }
                        foreach (var item in filterString)
                        {
                            _d = _d.Replace(item, "");
                        }
                        _d = _d.TrimStart(' ').TrimEnd(' ');
                        if (_d.Length > 0)
                        {
                            foreach (var subChar in specChar)
                            {
                                if (subChar == '\\')
                                {
                                    _d = _d.Replace("\\\\", "≌");
                                }
                                else
                                {
                                    _d = _d.Replace("\\" + subChar, "◇");
                                    _d = _d.Replace(subChar.ToString(), "");
                                    _d = _d.Replace("◇", subChar.ToString());
                                }
                            }
                            _d = _d.Replace("≌", "\\");
                        }

                        result.Append(_d);
                    }
                    break;
                }
                char _char = ayexpression[i];
                if (_char == '\\')
                {
                    char next = ayexpression[i + 1];
                    bool isOk = false;
                    foreach (char subChar in specChar)
                    {
                        if (next == subChar)
                        {
                            result.Append(subChar);
                            i++;
                            isOk = true;
                            break;
                        }
                    }
                    if (!isOk)
                    {
                        //判断是不是UL
                        if (next == 'U')
                        {
                            result.Append(char.ToUpper(text[uIndex]));
                            uIndex++;
                            i++;

                        }
                        else if (next == 'L')
                        {
                            result.Append(char.ToLower(text[uIndex]));
                            uIndex++;
                            i++;

                        }
                    }
                }
                else
                {
                    if (_char == '#') //匹配数字
                    {
                        int ds_1 = i + 1;
                        char next = ' ';
                        bool isEnd = false;
                        if (ds_1 == axlength)
                        {
                            isEnd = true;
                        }
                        else
                        {
                            next = ayexpression[i + 1];
                        }

                        if (!isEnd && next == '(')
                        {
                            int nextEndHu = ayexpression.IndexOfAny(new char[] { ')' }, i);
                            if (nextEndHu < 0)
                            {
                                throw new Exception("掩码规则不正确，错误位置:" + i + ",是否少了个右括号?");
                            }
                            string subAd = ayexpression.Substring(i, (nextEndHu - i + 1));
                            MatchCollection matches = Regex.Matches(subAd, @"\#\((?<ch>.+)\)");
                            int xingVal = 0;
                            string xingValue = null;
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                xingValue = groups["ch"].Value;
                                if (string.IsNullOrEmpty(xingValue))
                                {
                                    throw new Exception("#(number)掩码规则出错，错误位置:" + i + ",缺少限定值数字");
                                }
                                xingVal = xingValue.ToInt();
                                if (xingVal == 0)
                                {
                                    throw new Exception("掩码规则不正确，错误位置:" + i + ",括号内数字不能为0或者其他非数字字符");
                                }
                            }
                            for (int mm = 0; mm < xingVal; mm++)
                            {
                                bool _dig = false;
                                for (int j = uIndex; j < ulength; j++)
                                {
                                    if (char.IsDigit(text[j]))
                                    {
                                        _dig = true;
                                        result.Append(text[j]);
                                    }
                                    if (_dig)
                                    {
                                        uIndex++;
                                        break;
                                    }
                                    else
                                    {
                                        uIndex++;
                                    }
                                }
                            }
                            i = i + xingValue.Length + 2;
                        }
                        else
                        {

                            bool _dig = false;
                            for (int j = uIndex; j < ulength; j++)
                            {
                                if (char.IsDigit(text[j]))
                                {
                                    _dig = true;
                                    result.Append(text[j]);
                                }
                                if (_dig)
                                {
                                    uIndex++;
                                    break;
                                }
                                else
                                {
                                    uIndex++;
                                }

                            }
                        }

                    }
                    else if (_char == '@') //匹配字母
                    {

                        int ds_1 = i + 1;
                        char next = ' ';
                        bool isEnd = false;
                        if (ds_1 == axlength)
                        {
                            isEnd = true;
                        }
                        else
                        {
                            next = ayexpression[i + 1];
                        }

                        if (!isEnd && next == '(')
                        {
                            int nextEndHu = ayexpression.IndexOfAny(new char[] { ')' }, i);
                            if (nextEndHu < 0)
                            {
                                throw new Exception("掩码规则不正确，错误位置:" + i + ",是否少了个右括号?");
                            }
                            string subAd = ayexpression.Substring(i, (nextEndHu - i + 1));
                            MatchCollection matches = Regex.Matches(subAd, @"\@\((?<ch>.+)\)");
                            int xingVal = 0;
                            string xingValue = null;
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                xingValue = groups["ch"].Value;
                                if (string.IsNullOrEmpty(xingValue))
                                {
                                    throw new Exception("@(number)掩码规则出错，错误位置:" + i + ",缺少限定值数字");
                                }
                                xingVal = xingValue.ToInt();
                                if (xingVal == 0)
                                {
                                    throw new Exception("掩码规则不正确，错误位置:" + i + ",括号内数字不能为0或者其他非数字字符");
                                }
                            }
                            for (int mm = 0; mm < xingVal; mm++)
                            {
                                bool _dig = false;
                                for (int j = uIndex; j < ulength; j++)
                                {
                                    if (char.IsLetter(text[j]))
                                    {
                                        _dig = true;
                                        result.Append(text[j]);
                                    }
                                    if (_dig)
                                    {
                                        uIndex++;
                                        break;
                                    }
                                    else
                                    {
                                        uIndex++;
                                    }
                                }
                            }
                            i = i + xingValue.Length + 2;

                        }
                        else
                        {
                            bool _dig = false;
                            for (int j = uIndex; j < ulength; j++)
                            {
                                if (char.IsLetter(text[j]))
                                {
                                    _dig = true;
                                    result.Append(text[j]);
                                }
                                if (_dig)
                                {
                                    uIndex++;
                                    break;
                                }
                                else
                                {
                                    uIndex++;
                                }
                            }
                        }
                    }
                    else if (_char == '%') //匹配字母和数字
                    {
                        int ds_1 = i + 1;
                        char next = ' ';
                        bool isEnd = false;
                        if (ds_1 == axlength)
                        {
                            isEnd = true;
                        }
                        else
                        {
                            next = ayexpression[i + 1];
                        }

                        if (!isEnd && next == '(')
                        {
                            int nextEndHu = ayexpression.IndexOfAny(new char[] { ')' }, i);
                            if (nextEndHu < 0)
                            {
                                throw new Exception("掩码规则不正确，错误位置:" + i + ",是否少了个右括号?");
                            }
                            string subAd = ayexpression.Substring(i, (nextEndHu - i + 1));
                            MatchCollection matches = Regex.Matches(subAd, @"\%\((?<ch>.+)\)");
                            int xingVal = 0;
                            string xingValue = null;
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                xingValue = groups["ch"].Value;
                                if (string.IsNullOrEmpty(xingValue))
                                {
                                    throw new Exception("%(number)掩码规则出错，错误位置:" + i + ",缺少限定值数字");
                                }
                                xingVal = xingValue.ToInt();
                                if (xingVal == 0)
                                {
                                    throw new Exception("掩码规则不正确，错误位置:" + i + ",括号内数字不能为0或者其他非数字字符");
                                }
                            }
                            for (int mm = 0; mm < xingVal; mm++)
                            {
                                bool _dig = false;
                                for (int j = uIndex; j < ulength; j++)
                                {
                                    if (char.IsLetterOrDigit(text[j]))
                                    {
                                        _dig = true;
                                        result.Append(text[j]);
                                    }
                                    if (_dig)
                                    {
                                        uIndex++;
                                        break;
                                    }
                                    else
                                    {
                                        uIndex++;
                                    }
                                }
                            }
                            i = i + xingValue.Length + 2;

                        }
                        else
                        {
                            bool _dig = false;
                            for (int j = uIndex; j < ulength; j++)
                            {
                                if (char.IsLetterOrDigit(text[j]))
                                {
                                    _dig = true;
                                    result.Append(text[j]);
                                }
                                if (_dig)
                                {
                                    uIndex++;
                                    break;
                                }
                                else
                                {
                                    uIndex++;
                                }
                            }
                        }
                    }
                    else if (_char == '*') //匹配任意
                    {
                        //判断下一个是不是括号
                        int ds_1 = i + 1;
                        char next = ' ';
                        bool isEnd = false;
                        if (ds_1 == axlength)
                        {
                            isEnd = true;
                        }
                        else
                        {
                            next = ayexpression[i + 1];
                        }

                        if (!isEnd && next == '(')
                        {
                            int nextEndHu = 0;
                            bool isFindRightHu = false;
                            int _i = i;
                            int max_i = 30;
                            int max_i_i = 0;
                            do
                            {
                                nextEndHu = ayexpression.IndexOfAny(new char[] { ')' }, _i);
                                var ch = ayexpression[nextEndHu - 1];
                                if (ch != '\\')
                                {
                                    isFindRightHu = true;
                                }
                                _i = nextEndHu + 1;
                                max_i_i++;
                                if (max_i_i == max_i)
                                {
                                    throw new Exception("掩码规则不正确，错误位置:" + i + ",请检查你的表达式");
                                }
                            } while (!isFindRightHu);

                            if (nextEndHu < 0)
                            {
                                throw new Exception("掩码规则不正确，错误位置:" + i + ",是否少了个右括号?");
                            }
                            string subAd = ayexpression.Substring(i, (nextEndHu - i + 1));
                            MatchCollection matches = Regex.Matches(subAd, @"\*\((?<cha>.+)\)");
                            int xingVal = 0;
                            string xingValue = null;
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                xingValue = groups["cha"].Value;
                                if (string.IsNullOrEmpty(xingValue))
                                {
                                    throw new Exception("*(number)掩码规则出错，错误位置:" + i + ",缺少限定值数字");
                                }

                            }
                            xingVal = xingValue.ToInt();
                            if (xingVal == 0)
                            {
                                if (subAd.Contains(" ^> "))
                                {
                                    string[] _resultString = Regex.Split(xingValue, @" \^> ", RegexOptions.IgnoreCase);
                                    if (_resultString.Length != 2)
                                    {
                                        throw new Exception("*前置替换规则出错，错误位置:" + i);
                                    }
                                    var _r1 = _resultString[0];
                                    string dstr = null;
                                    int _otherLen = ulength - uIndex;
                                    if (_r1 == "?")
                                    {
                                        dstr = text.Substring(uIndex, _otherLen - 1);
                                    }
                                    else
                                    {
                                        dstr = text.Substring(uIndex, Math.Min(_r1.ToInt(), Math.Min(_otherLen, text.Length)));
                                    }
                                    string _d = _resultString[1];
                                    var _d1 = _d.Split('^');
                                    var _d11 = _d1[0];

                                    var _d12 = _d1[1];
                                    var _e1 = Regex.Split(_d12, " -> ", RegexOptions.IgnoreCase);
                                    var _e11 = _e1[0];
                                    var _e12 = _e1[1];
                                    //处理 _e11 和 _e12
                                    foreach (var subChar in specChar)
                                    {
                                        if (subChar == '\\')
                                        {
                                            _e11 = _e11.Replace("\\\\", "≌");
                                            _e12 = _e12.Replace("\\\\", "≌");
                                        }
                                        else
                                        {
                                            _e11 = _e11.Replace("\\" + subChar, "◇");
                                            _e11 = _e11.Replace(subChar.ToString(), "");
                                            _e11 = _e11.Replace("◇", subChar.ToString());

                                            _e12 = _e12.Replace("\\" + subChar, "◇");
                                            _e12 = _e12.Replace(subChar.ToString(), "");
                                            _e12 = _e12.Replace("◇", subChar.ToString());
                                        }
                                    }
                                    _e11 = _e11.Replace("≌", "\\");

                                    if (_d11 == "?")
                                    {
                                        dstr = dstr.Replace(_e11, _e12);
                                        result.Append(dstr);
                                        uIndex = ulength;
                                        i = i + xingValue.Length + 2;
                                    }
                                    else
                                    if (_d11.Contains("+"))  //连续替换
                                    {
                                        string end = dstr;
                                        int _f = (_d11.Substring(1, _d11.Length - 1)).ToInt();
                                        int _f_index = 0;
                                        int _f_count = GetSubStrCountInStr(dstr, _e11, 0).Length;
                                        for (int _g = 0; _g < _f_count; _g++)
                                        {
                                            end = AyReplaceByPlace(end, _e11, _e12, 1);
                                            _f_index++;
                                            if (_f_index == _f)
                                            {
                                                break;
                                            }
                                        }

                                        result.Append(end);

                                        uIndex = ulength;
                                        i = i + xingValue.Length + 2;
                                    }
                                    else //指定第几个替换
                                    {
                                        int _f = _d11.ToInt();
                                        var end = AyReplaceByPlace(dstr, _e11, _e12, _f);
                                        result.Append(end);

                                        uIndex = ulength;
                                        i = i + xingValue.Length + 2;
                                    }
                                }
                                else if (subAd.Contains(" ^< "))  //开始
                                {

                                    string[] _resultString = Regex.Split(xingValue, @" \^< ", RegexOptions.IgnoreCase);
                                    if (_resultString.Length != 2)
                                    {
                                        throw new Exception("*后置替换规则出错，错误位置:" + i);
                                    }
                                    var _r1 = _resultString[0];
                                    string dstr = null;
                                    int _otherLen = ulength - uIndex;
                                    if (_r1 == "?")
                                    {
                                        //dstr = text.Substring(i, _otherLen);
                                        dstr = text.Substring(uIndex, _otherLen);
                                    }
                                    else
                                    {
                                        dstr = text.Substring(uIndex, Math.Min(_r1.ToInt(), Math.Min(_otherLen, text.Length)));
                                    }
                                    string _d = _resultString[1];
                                    var _d1 = _d.Split('^');
                                    var _d11 = _d1[0];

                                    var _d12 = _d1[1];
                                    var _e1 = Regex.Split(_d12, " -> ", RegexOptions.IgnoreCase);
                                    var _e11 = _e1[0];
                                    var _e12 = _e1[1];
                                    //处理 _e11 和 _e12
                                    foreach (var subChar in specChar)
                                    {
                                        if (subChar == '\\')
                                        {
                                            _e11 = _e11.Replace("\\\\", "≌");
                                            _e12 = _e12.Replace("\\\\", "≌");
                                        }
                                        else
                                        {
                                            _e11 = _e11.Replace("\\" + subChar, "◇");
                                            _e11 = _e11.Replace(subChar.ToString(), "");
                                            _e11 = _e11.Replace("◇", subChar.ToString());

                                            _e12 = _e12.Replace("\\" + subChar, "◇");
                                            _e12 = _e12.Replace(subChar.ToString(), "");
                                            _e12 = _e12.Replace("◇", subChar.ToString());
                                        }
                                    }
                                    _e11 = _e11.Replace("≌", "\\");



                                    if (_d11 == "?")
                                    {
                                        dstr = dstr.Replace(_e11, _e12);
                                        result.Append(dstr);
                                        uIndex = ulength;
                                        i = i + xingValue.Length + 2;
                                    }
                                    else
                                    if (_d11.Contains("+"))  //连续替换
                                    {
                                        string end = dstr;
                                        int _f = (_d11.Substring(1, _d11.Length - 1)).ToInt();  // 连续次数
                                        int _f_count = GetSubStrCountInStr(dstr, _e11, 0).Length;
                                        if (_f_count == 0) { }
                                        else if (_f_count <= _f)
                                        {
                                            dstr = dstr.Replace(_e11, _e12);
                                            result.Append(dstr);
                                            uIndex = ulength;
                                            i = i + xingValue.Length + 2;
                                        }
                                        else
                                        {
                                            for (int _g = 0; _g < _f; _g++)
                                            {
                                                end = AyReplaceByPlace(end, _e11, _e12, _f_count--);
                                            }
                                        }

                                        result.Append(end);
                                        uIndex = ulength;
                                        i = i + xingValue.Length + 2;
                                    }
                                    else //指定第几个替换
                                    {
                                        int _f = _d11.ToInt();
                                        int _f_count = GetSubStrCountInStr(dstr, _e11, 0).Length;
                                        if (_f_count == 0 || _f_count <= _f)
                                        {
                                            result.Append(dstr);
                                            uIndex = ulength;
                                            i = i + xingValue.Length + 2;
                                        }
                                        else
                                        {
                                            var end = AyReplaceByPlace(dstr, _e11, _e12, (_f_count - _f + 1));
                                            result.Append(end);
                                            uIndex = ulength;
                                            i = i + xingValue.Length + 2;
                                        }
                                    }
                                }
                                else   //正常替换
                                {

                                    if (xingValue.IndexOf(" -> ") < 0)
                                    {
                                        if (xingValue == "?")
                                        {
                                            int utlen = ulength - uIndex;
                                            result.Append(text.Substring(uIndex, utlen));
                                            uIndex = ulength;
                                            //掩码跳过位数()+值的length；
                                            i = i + xingValue.Length + 2;
                                        }
                                        else
                                        {
                                            throw new Exception("*(规则) 掩码规则出错，错误位置:" + i + ",限定值不合法");
                                        }

                                    }

                                    else
                                    {
                                        string[] _resultString = Regex.Split(xingValue, " -> ", RegexOptions.IgnoreCase);
                                        string _d = _resultString[1];
                                        foreach (var subChar in specChar)
                                        {
                                            if (subChar == '\\')
                                            {
                                                _d = _d.Replace("\\\\", "≌");
                                            }
                                            else
                                            {
                                                _d = _d.Replace("\\" + subChar, "◇");
                                                _d = _d.Replace(subChar.ToString(), "");
                                                _d = _d.Replace("◇", subChar.ToString());
                                            }
                                        }
                                        _d = _d.Replace("≌", "\\");

                                        if (_resultString[0] == "?")
                                        {
                                            int utlen = ulength - uIndex;
                                            StringBuilder sb1 = new StringBuilder();
                                            for (int k = 0; k < utlen; k++)
                                            {
                                                sb1.Append(_d);
                                            }
                                            result.Append(sb1.ToString());

                                            uIndex = ulength;
                                            //掩码跳过位数()+值的length；
                                            i = i + xingValue.Length + 2;
                                        }
                                        else
                                        {
                                            int _ind = _resultString[0].ToInt();
                                            int utlen = ulength - uIndex;
                                            int minXin = Math.Min(_ind, utlen);
                                            StringBuilder sb1 = new StringBuilder();
                                            for (int k = 0; k < _ind; k++)
                                            {
                                                sb1.Append(_d);
                                            }
                                            result.Append(sb1.ToString());
                                            if (minXin == utlen) break; //如果要取的字符已经不够了，下次直接终止掩码过程了
                                            uIndex = uIndex + minXin;
                                            //掩码跳过位数()+值的length；
                                            i = i + xingValue.Length + 2;
                                        }
                                    }

                                }

                            }
                            else
                            {
                                int utlen = ulength - uIndex;
                                int minXin = Math.Min(xingVal, utlen);
                                result.Append(text.Substring(uIndex, minXin));
                                if (minXin == utlen) break; //如果要取的字符已经不够了，下次直接终止掩码过程了
                                uIndex = uIndex + minXin;
                                //掩码跳过位数()+值的length；
                                i = i + xingValue.Length + 2;
                            }
                        }
                        else
                        {
                            result.Append(text[uIndex]);
                            uIndex++;
                        }

                    }
                    else if (_char == '$') //终止
                    {
                        break;
                    }
                    else
                    {
                        result.Append(_char);
                    }
                }
            }
            return result;
        }
        public void Version()
        {
            Console.WriteLine("AY表达式版本: 1.2.0.0");
        }
        public void Helper()
        {
            Console.WriteLine(@"==============AYUI. AY表达式用法=======================

    @  代表 字母占位符 
    @(number)  重复指定number个@号,比如   @(3) 等同于 @@@ 
    #   代表数字占位符
    #(number)  重复指定number个#号,比如   #(3) 等同于 ###
    % 代表数字或者字母 占位符
    %(number)  重复指定number个&号,比如   &(3) 等同于&&&
    \U  代表下一个字符将被转换为大写,数字会忽略，U是Upper的单词首字符
    \L  代表下一个字符将被转换为小写,L是Lower的单词首字符
    $   代表终止匹配，结束，掩码的处理任务,可以提前结束任务
    \   转义字符
    *  任意字符占位符
    *(number)  重复指定number个*号,比如   *(3) 等同于 *** 
    *(13 -> \*)  替换规则     表示接下来的 13个字符都替换成*号 ，特殊内置符号需要\来转义
    *()
    *(?)  剩余所有字符
    使用*(? -> \*)  表示剩余所有字符 都替换成 * 号   这里的*是特殊的符号，所以需要\* 转义

==============AYUI. AY表达式用法=======================
            ");
        }
        #endregion
        /// <summary>
        /// 根据字符串，然后子字符串，返回所有匹配的子字符串的位置
        /// </summary>
        /// <param name="str">被搜索的字符串</param>
        /// <param name="substr">子字符串</param>
        /// <param name="StartPos">开始位置</param>
        /// <returns>返回所有匹配的子字符串的位置</returns>
        public static int[] GetSubStrCountInStr(String str, String substr, int StartPos)
        {
            int foundPos = -1;
            int count = 0;
            List<int> foundItems = new List<int>();

            do
            {
                foundPos = str.IndexOf(substr, StartPos);
                if (foundPos > -1)
                {
                    StartPos = foundPos + 1;
                    count++;
                    foundItems.Add(foundPos);
                }
            } while (foundPos > -1 && StartPos < str.Length);

            return ((int[])foundItems.ToArray());
        }

        #region 替换第一个匹配到的字符串
        //int i = _dstr.IndexOf(split);//获取第一索引个要合并字符串的
        //s1 = s1.Replace(split, "");//替换为空
        //s1 = s1.Insert(i, split);//在一个处插入合并字符串
        #endregion

        public static string AyReplaceByPlace(string str, string pattern, string newstr, int index)
        {
            int leadListIndex = 0;
            return Regex.Replace(str, Regex.Escape(pattern), m => ++leadListIndex == index ? newstr : m.Value);
        }
    }
}
