using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ay.Controls.Info
{
    public class AyCheckBoxListItemModel : AyPropertyChanged,IAyCheckedItem
    {
        private bool _IsChecked;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return _IsChecked; }
            set { Set(ref _IsChecked, value); }
        }


        private string _ItemText;

        /// <summary>
        /// 显示的内容
        /// </summary>
        public string ItemText
        {
            get { return _ItemText; }
            set { Set(ref _ItemText, value); }
        }

        private string _ItemValue;

        /// <summary>
        /// 隐藏选中的值
        /// </summary>
        public string ItemValue
        {
            get { return _ItemValue; }
            set { Set(ref _ItemValue, value); }
        }



    }
}
