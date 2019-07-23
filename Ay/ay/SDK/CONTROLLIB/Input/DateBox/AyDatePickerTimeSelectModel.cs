namespace ay.Date.Info
{
    /// <summary>
    /// 年月日时分秒的Model
    /// </summary>
    public class AyDatePickerTimeSelectModel
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }


        private bool isEnabled = true;
        /// <summary>
        /// 2015-10-12 14:58:26  动态限制，是否可用
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

    }
}
