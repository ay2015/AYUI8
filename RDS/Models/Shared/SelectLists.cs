using Ay.Framework.WPF;
using System.Collections.Generic;
using System.ComponentModel;

namespace RDS.Models
{
    public static class SelectLists
    {

        //<ay:AyComboBox DisplayMemberPath= "Text" SelectedValue= "{Binding Model.Form.TypeFlag}"  ItemsSource= "{x:Static md:SelectLists.AuthTypeFlag}" SelectedValuePath= "Value" Width= "500"  AlignmentService.Alignment= "s c" >
        //      < ay:AyComboBox.LeftContent>
        //          <ContentControl Style = "{DynamicResource View.Text.Left}" ToolTip= "{DynamicResource ayauth_typeflag}" Content= "{DynamicResource ayauth_typeflag}"  ay:UIBase.IconWidth= "100" ></ ContentControl >
        //      </ ay:AyComboBox.LeftContent>
        //  </ay:AyComboBox>

        private static IList<SelectListItem> _AuthTypeFlag;
        /// <summary>
        /// 测试枚举的下拉框
        /// </summary>
        public static IList<SelectListItem> AuthTypeFlag
        {
            get
            {
                if (_AuthTypeFlag == null)
                {
                    _AuthTypeFlag = EnumHelper.ToSelectListByDesc(typeof(AuthTypeFlag));
                }
                return _AuthTypeFlag;
            }
        }


    }

    #region 枚举
    public enum AuthTypeFlag : int
    {
        [Description("系统")]
        System = 1,
        [Description("模块")]
        Module = 2,
        [Description("页面")]
        Page = 3,
        [Description("权限")]
        Auth = 4
    }
    #endregion

}
