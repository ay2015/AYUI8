using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ay.AyExpression
{
    public class AyFormRequiredValidator : AyFormValidator
    {
        public AyFormRequiredValidator()
        {
            Example = "输入任意非空白字符,至少一个字符即可";
            ErrorInfo = AyFormErrorTemplate.Required;
            CustomFunc = (text) =>
            {
                return !text.IsNullAndTrimAndEmpty();
            };

        }
    }
    public class AyFormIntegerValidator : AyFormValidator
    {
        public AyFormIntegerValidator()
        {
            Example = "大于0的正整数";
            ErrorInfo = AyFormErrorTemplate.Integer;
            RegexExpression = @"^\+?[1-9][0-9]*$";
        }
    }
    public class AyFormIntegerZeroValidator : AyFormValidator
    {
        public AyFormIntegerZeroValidator()
        {
            Example = "输入的数是大于等于0的整数";
            ErrorInfo = AyFormErrorTemplate.IntegerZero;
            RegexExpression = @"^\d+$";
        }
    }

    public class AyFormAgeValidator : AyFormValidator
    {
        public AyFormAgeValidator()
        {
            Example = "人的年龄0-300";
            ErrorInfo = AyFormErrorTemplate.Age;
            RegexExpression = @"^((\d{0,2})|(((1|2)\d{2})|300))$";
        }
    }
    public class AyFormQQValidator : AyFormValidator
    {
        public AyFormQQValidator()
        {
            Example = "例如:875556003";
            ErrorInfo = AyFormErrorTemplate.QQ;
            RegexExpression = @"^[1-9][0-9]{4,}$";
        }
    }

    public class AyFormEmailValidator : AyFormValidator
    {
        public AyFormEmailValidator()
        {
            Example = "example@163.com";
            ErrorInfo = AyFormErrorTemplate.Email;
            RegexExpression = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        }
    }

    public class AyFormTelValidator : AyFormValidator
    {
        public AyFormTelValidator()
        {
            Example = "15255112050";
            ErrorInfo = AyFormErrorTemplate.Tel;
            RegexExpression = @"^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\d{8}$";
        }
       
    }
    //public static bool IsDateTime(string StrSource)
    //{
    //    return Regex.IsMatch(StrSource, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
    //}
    public class AyFormTel2Validator : AyFormValidator
    {
        public AyFormTel2Validator()
        {
            Example = @"""XXX - XXXXXXX""、""XXXX - XXXXXXXX""、""XXX - XXXXXXX""、""XXX - XXXXXXXX""、""XXXXXXX""和""XXXXXXXX""";
            ErrorInfo = AyFormErrorTemplate.ChinaTel;
            RegexExpression = @"^(\(\d{3,4}-)|\d{3.4}-)?\d{7,8}$";
        }
    }
    public class AyFormChinaTelValidator : AyFormValidator
    {
        public AyFormChinaTelValidator()
        {
            Example = @"0511-4405222、021-87888822";
            ErrorInfo = AyFormErrorTemplate.ChinaTel;
            RegexExpression = @"^\d{3}-\d{8}|\d{4}-\d{7}$";
        }
    }

    public class AyFormFaxValidator : AyFormValidator
    {
        public AyFormFaxValidator()
        {
            Example = "86-578-88888888";
            ErrorInfo = AyFormErrorTemplate.Fax;
            RegexExpression = @"^86-\d{2,3}-\d{7,8}$";
        }
    }

    public class AyFormIpValidator : AyFormValidator
    {
        public AyFormIpValidator()
        {
            Example = "192.168.1.110";
            ErrorInfo = AyFormErrorTemplate.Ip;
            RegexExpression = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
        }
    }
    public class AyFormIpSecValidator : AyFormValidator
    {
        public AyFormIpSecValidator()
        {
            Example = "6.24.1.2";
            ErrorInfo = AyFormErrorTemplate.IpSec;
            RegexExpression = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*$";
        }
    }
    public class AyFormTimeValidator : AyFormValidator
    {
        public AyFormTimeValidator()
        {
            Example = "18:00:00";
            ErrorInfo = AyFormErrorTemplate.Time;
            RegexExpression = @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$";
        }
    }
    public class AyFormDateValidator : AyFormValidator
    {
        public AyFormDateValidator()
        {
            Example = "2016-08-01";
            ErrorInfo = AyFormErrorTemplate.Date;
            RegexExpression = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
        }
    }

    public class AyFormUserNameValidator : AyFormValidator
    {
        public AyFormUserNameValidator()
        {
            Example = "hello_ay";
            ErrorInfo = AyFormErrorTemplate.UserName;
            RegexExpression = @"^\w{1,}$";
        }
    }
    public class AyFormZipValidator : AyFormValidator
    {
        public AyFormZipValidator()
        {
            Example = "280000";
            ErrorInfo = AyFormErrorTemplate.Zip;
            RegexExpression = @"^[1-9]\d{5}(?!\d)$";
        }
    }
    public class AyFormPasswordValidator : AyFormValidator
    {
        public AyFormPasswordValidator()
        {
            Example = "Yangyang2017";
            ErrorInfo = AyFormErrorTemplate.Password;
            RegexExpression = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9]{6,15}$";
        }

    }
    //public class AyFormMonthValidator : AyFormValidator
    //{
    //    public AyFormMonthValidator()
    //    {
    //        Example = "12";
    //        ErrorInfo = AyFormErrorTemplate.Month;
    //        RegexExpression = @"^(0?[1 - 9]|1[0-2])$";
    //    }

    //}



    //\u4e00-\u9fa5
}
