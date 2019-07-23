using System;

namespace ay.Controls.Info
{
    public class AyDatePickerItem
    {
        public String MonthAndDay { get; set; }
        public String NameOfDay { get; set; }

        public String Title { get; set; }

        public int TitleType { get; set; }


        public String NumberOfDay { get; set; }

        public string Week_S { get; set; }

        public string TipShow { get; set; }

        public string ShengXiao { get; set; }

        public string GanZhi { get; set; }

        public String JieQi { get; set; }

        public String GongHoliday { get; set; }
        public String NongHoliday { get; set; }

        public string TianGan { get; set; }

        public string DiGan { get; set; }


        public DateTime Date { get; set; }

        private bool isEnabled = true;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        private bool isHighlight = false;
        /// <summary>
        /// 2017-3-10 13:16:26
        /// 是否高亮日
        /// </summary>
        public bool IsHighlight
        {
            get { return isHighlight; }
            set { isHighlight = value; }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        private bool isWeekDay;

        public bool IsWeekDay
        {
            get { return isWeekDay; }
            set { isWeekDay = value; }
        }

        private bool isLastOrNextDateItem;

        public bool IsLastOrNextDateItem
        {
            get { return isLastOrNextDateItem; }
            set { isLastOrNextDateItem = value; }
        }

        private bool isBlackDateItem;

        public bool IsBlackDateItem
        {
            get { return isBlackDateItem; }
            set { isBlackDateItem = value; }
        }

        private bool isToday;

        public bool IsToday
        {
            get { return isToday; }
            set { isToday = value; }
        }




    }
}
