using System;

namespace ay.Controls.Info
{
    public class DayInfo
    {
        public String Title { get; set; }

        /// <summary>
        /// 0  代表正常  1代表初一已经换成月了  2代表节气 3代表公历节气 4代表农历节日
        /// </summary>
        public int TitleType { get; set; }
        public String Month { get; set; }
        public String Day { get; set; }
        public int monthIndex { get; set; }
        public int dayIndex { get; set; }
        public String MonthAndDay { get; set; }


        public String JieQi { get; set; }

        public String GongHoliday { get; set; }
        public String NongHoliday { get; set; }

        public string TianGan { get; set; }

        public string DiGan { get; set; }
        public string ShengXiao { get; set; }
        public int Year { get; set; }


    }
}
