using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ay.AyExpression
{
    public class AyFormLengthValidator : AyFormValidator
    {
        public AyFormLengthValidator()
        {
            Example = "输入的字符长度范围限制";
            ErrorInfo = AyFormErrorTemplate.Length1;
        }
        public override bool Validate(string text, string expression = "")
        {
            int textLength = text.Length;
            int exLength = expression.Length;
            string subAd = expression.Substring(7, (exLength - 8));
            var sl = subAd.Split('|');
            List<bool> validateResults = new List<bool>();
            List<string> validateErrorResults = new List<string>();
            bool isOnlyOneCondition = sl.Length == 1;
            bool isOnlySingleLengthStrict = true;
            StringBuilder onlySingleLengthStrictError = null;

            foreach (var _sl in sl)
            {
                if (_sl.IndexOf(",") >= 0)
                {
                    isOnlySingleLengthStrict = false;
                    var _sla = _sl.Split(',');
                    int le = _sla.Length;


                    if (_sla[0] == "(")
                    {
                        double _y = _sla[1].TrimEnd(')').ToDouble();
                        //字符可以不输入，但是最多只能输入y个
                        if (textLength == 0)
                        {
                            validateResults.Add(true);
                        }
                        else if (textLength <= _y)
                        {
                            validateResults.Add(true);
                        }
                        else
                        {
                            validateResults.Add(false);
                            if (isOnlyOneCondition)
                                validateErrorResults.Add(string.Format(AyFormErrorTemplate.Length3, _y));
                            else
                            {
                                if (validateErrorResults.Count == 0)
                                {
                                    validateErrorResults.Add(AyFormErrorTemplate.Length4);
                                }

                            }

                        }
                    }
                    else if (_sla[1] == ")")
                    {
                        double _y = _sla[0].TrimStart('(').ToDouble();
                        if (textLength >= _y)
                        {
                            validateResults.Add(true);
                        }
                        else
                        {
                            validateResults.Add(false);
                            if (isOnlyOneCondition)
                                validateErrorResults.Add(string.Format(AyFormErrorTemplate.Length2, _y));
                            else
                            {
                                if (validateErrorResults.Count == 0)
                                {
                                    validateErrorResults.Add(AyFormErrorTemplate.Length4);
                                }
                            }
                        }
                    }

                    else
                    {

                        double _x = _sla[0].TrimStart('(').ToDouble();
                        double _y1 = _sla[1].TrimEnd(')').ToDouble();
                        if (_x <= textLength && _y1 >= textLength)
                        {
                            validateResults.Add(true);
                        }
                        else
                        {
                            validateResults.Add(false);
                            if (isOnlyOneCondition)
                                validateErrorResults.Add(string.Format(AyFormErrorTemplate.Length1, _x, _y1));
                            else
                            {
                                if (validateErrorResults.Count == 0)
                                {
                                    validateErrorResults.Add(AyFormErrorTemplate.Length4);
                                }
                            }
                        }
                    }
                }
                else    //直接是数字
                {
                    var _x = _sl.ToDouble();
                    if (_x == textLength)
                    {
                        validateResults.Add(true);
                    }
                    else
                    {
                        validateResults.Add(false);
                        if (isOnlyOneCondition)
                        {
                            validateErrorResults.Add(string.Format(AyFormErrorTemplate.Length5, _x));
                        }
                        else
                        {
                            if (onlySingleLengthStrictError == null) onlySingleLengthStrictError = new StringBuilder();
                            onlySingleLengthStrictError.Append(_x + "个,");
                        }
                    }
                }
            }

            if (isOnlySingleLengthStrict && onlySingleLengthStrictError != null)
            {
                bool _b = false;
                foreach (var subResult in validateResults)
                {
                    if (subResult)
                    {
                        _b = true;
                        break;
                    }
                }
                if (!_b)
                {
                    ErrorInfo  = string.Format(AyFormErrorTemplate.Length6, onlySingleLengthStrictError.ToString().TrimEnd(',').ToAyExpressionValue(@"*(? ^< 1^, -> 或者)"));
                    Console.WriteLine("33333");
                    return false;
                }
               
            }
            else
            {
                bool _b = false;
                foreach (var subResult in validateResults)
                {
                    if (subResult)
                    {
                        _b = true;
                        break;
                    }
                }
                if (!_b)
                {
                    ErrorInfo = validateErrorResults[0];
                    return false;
                }
            }
            return true;

        }

    }
}
