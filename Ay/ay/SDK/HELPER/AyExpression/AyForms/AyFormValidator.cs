using System;
using System.Text.RegularExpressions;

namespace ay.AyExpression
{
    /*<AyForm x:Name="userForm" >
     * <AyInput  Mode="Edit" Rule=""  Regex="" Text="" Mask="" Width="" Height="" Canvas.Left="" Canvas.Top="" 其他属性>
     * <AyInput Mode="Edit"  Rule=""  Regex="" Text="" Mask="" Width="" Height="" Canvas.Left="" Canvas.Top="" 其他属性>
     * 
     * </AyForm>
     * 后台通过userForm.Validate()返回的bool型判断表单是否填写完成,系统会自动验证加提示
     * 完成后，直接userForm.DataContext属性获得Model
       可能性:Regex可填可不填，除非当Rule中包含Regex
       Rule中的值 required,email,tel,cell,datetime,date,time,num,length,fax,regex,zoombig,website,身份证,护照,军官证,港澳台,出生证,户口本
         @e[$elementname:propertypath],
         @ge[$elementname:propertypath],
         @g[$elementname:propertypath],
         @le[$elementname:propertypath],
         @l[$elementname:propertypath]
         @ne[$elementname:propertypath]

         $model.minNumber,$model.maxNumber
         示例   Rule="required,num[@ge $tb1:Text,@le $tb2:Text]" 整型类型    $tb1:Text <= num <= $tb2:Text     
                   Rule="required,num[@ge 10,@le 200],length[(2,3)|18]"   整型类型    10 <= num <=200   
                                                                                                                   输入的字符长度2到3个字符长度，或者18个字符长度
                   Rule="email" 验证邮箱，不填写不验证，  tel 验证电话号码，单个都是类似
                   Rule="num(14,2)[@ge 10,@l 200.12]"  浮点类型 整数14位，小数是2位    10.00<= num < 200.1
                   Rule="num(14,2?)[@ge 10,@l 200.12]"  浮点类型 整数14位，小数是2位,比如文本框输入12,失去焦点时候自动补上2位
                                                                                    10.00<= num < 200.1
                   Rule="num(3)[@ge 0,@le 100]" 整型 最多3位数  大于等于0 小于等于100
                 
            
                   Mode="Edit"   显示文本框模式
                   Mode="View"  显示TextBlock模式
                   MaskTooltip也是基于ViewMaskValue显示，如果ViewMask不填，那么就是Text的值
                   例如 Text="6228480402564890018"
                   如果ViewMask="#### #### #### ####" 
                   ViewMaskValue可以拿到ViewMask后的值
                   那么View的时候显示6228 4804 0256 4890 018
                   
                 
                   
         */
    public abstract class AyFormValidator : AyFormBase
    {
        public AyFormValidator() { }

        public Func<string, bool> CustomFunc { get; set; }

        public string Example { get; set; }

        private string errorInfo = "格式不正确";
        /// <summary>
        /// 错误详细信息
        /// </summary>
        public string ErrorInfo
        {
            get
            {
                return errorInfo;
            }

            set
            {
                errorInfo = value;
            }
        }
        /// <summary>
        /// 正则表达式
        /// </summary>
        public string RegexExpression { get; set; }


        #region 验证输入字符串是否与模式字符串匹配
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        public bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
        #endregion

        public virtual bool Validate(string text,string expression="")
        {
            if (CustomFunc != null)
            {
                return CustomFunc(text);
            }
            else
            {
                return IsMatch(text, RegexExpression);
            }
        }

    }


}
