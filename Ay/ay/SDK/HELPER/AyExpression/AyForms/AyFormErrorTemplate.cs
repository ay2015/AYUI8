using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ay.AyExpression
{
    public partial class AyFormErrorTemplate
    {
        public static string Required = "该项不能为空";
        public static string ComboBoxInvalidItem = "您输入的值不在下拉选项中";

        public static string Email = "邮箱格式不正确";
        //public static string Month = "月份格式不支持";

        public static string Tel = "电话号码格式不正确";
        public static string Age = "年龄格式不正确(0到300之间)";
        public static string QQ = "QQ号格式不正确";
        public static string ChinaTel = "国内电话号码格式不正确";
        public static string Fax = "传真号码格式不正确";
        public static string Ip = "Ip格式不正确";
        public static string IpSec = "IpSec格式不正确";
        public static string Time = "时间格式不正确";
        public static string Date = "日期格式不正确";
        public static string IDCard = "身份证号码格式不正确";

        public static string Num = "请输入数字";
        public static string Integer = "请输入正整数";
        public static string IntegerZero = "请输入整数,该值要>=0";
        public static string NumFloat1 = "整数位最多只能有{0}位数字";
        public static string NumFloat2 = "小数位最多只能有{0}位数字";
        public static string NumFloat3 = "输入的值不能含有小数位";

        public static string Length1 = "输入的字数应该在{0}和{1}之间";
        public static string Length2 = "最少输入{0}个字";
        public static string Length3 = "最多输入{0}个字";
        public static string Length4 = "您输入的字数不在要求的范围内";
        public static string Length5 = "只能输入{0}个字";
        public static string Length6 = "只能输入{0}字";

        public static string UserName = "只能包含字母、数字和下划线";
        public static string Zip = "邮编格式不正确";
        public static string Password = "该项必须同时含有大写字母、小写字母和数字";
    }


}
