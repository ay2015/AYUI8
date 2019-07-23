using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ay.AyExpression
{
    public enum AyFormValidatorTypes
    {
        required,  //必填
        password,
        num,
        QQ,
        integer,
        integerZero,
        age,
        email, //邮箱地址
        tel,
        tel2,
        chinaTel, //国内电话号码
        length,
        IDCard, //身份证
        fax,
        Ip,
        IpSec,
        time,
        date,
        username, //用户名
        zip //邮编
    }
}
