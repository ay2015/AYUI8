
namespace ay.AyExpression
{
    public static class AyExpressionExtendMethod
    {
        /// <summary>
        /// 转换为Ay表达式
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ayexpression"></param>
        /// <returns></returns>
        public static string ToAyExpressionValue(this string text, string ayexpression)
        {
            return AyExpression.GetMaskedValue(ayexpression, text).ToString().TrimEnd(' ') ;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">被验证的文本</param>
        /// <param name="ayexpression">AY表单表达式</param>
        /// <returns></returns>
        public static AyFormResult ToAyExpressionFormResult(this string text, string ayexpression)
        {
            return AyExpression.GetFormValidateResult(ayexpression, text);
        }
    }


}
