using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ay.AyExpression
{
    /// <summary>
    /// 用于拓展AyDateBoxCalendar的api接口
    /// </summary>
    public class AyDateRuleJsonToObjects
    {
        /// <summary>
        /// 用于控制PopupList的位置
        /// </summary>
        public class PopupListPosition
        {
            private double _left = 0;

            public double left
            {
                get { return _left; }
                set { _left = value; }
            }
            private double _top = 1;

            public double top
            {
                get { return _top; }
                set { _top = value; }
            }

        }
        public string startDate { get; set; }

        public bool alwaysUseStartDate { get; set; }

        public string _dateFmt;


        private string _minDate;

        public string minDate
        {
            get { return _minDate; }
            set { _minDate = value; }
        }

        private string _maxDate;

        public string maxDate
        {
            get { return _maxDate; }
            set { _maxDate = value; }
        }


        public string dateFmt
        {
            get
            {
                if (_dateFmt.IsNullOrWhiteSpace())
                {
                    return "yyyy-MM-dd";
                }
                return _dateFmt;
            }
            set { _dateFmt = value; }
        }


        //public bool isShowClear { get; set; }

        private bool _isShowToday = true;

        public bool isShowToday
        {
            get { return _isShowToday; }
            set { _isShowToday = value; }
        }

        private bool _isShowClear = true;

        public bool isShowClear
        {
            get { return _isShowClear; }
            set { _isShowClear = value; }
        }
        private bool _readOnly = false;

        public bool readOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        private PopupListPosition _position = new PopupListPosition();

        public PopupListPosition position
        {
            get { return _position; }
            set { _position = value; }
        }
        private bool _highLineWeekDay = true;

        public bool highLineWeekDay
        {
            get { return _highLineWeekDay; }
            set { _highLineWeekDay = value; }
        }
        private bool _isShowWeek = false;
        /// <summary>
        /// 是否显示周，2017-2-20 15:58:22
        /// AY增加
        /// </summary>
        public bool isShowWeek
        {
            get { return _isShowWeek; }
            set { _isShowWeek = value; }
        }

        private int _firstDayOfWeek = 0;
        /// <summary>
        ///  各个国家的习惯不同,有些喜欢以星期日作为第一天,有些以星期一作为第一天.
        ///  相关属性:firstDayOfWeek: 可设置 0 - 6 的任意一个数字,0:星期日     1:星期一 以此类推
        /// </summary>
        public int firstDayOfWeek
        {
            get { return _firstDayOfWeek; }
            set { _firstDayOfWeek = value; }
        }
        /// <summary>
        /// 双月功能开启或者关闭,双月时候，单击时候，自动关闭列表
        /// </summary>
        private bool _doubleCalendar = false;

        public bool doubleCalendar
        {
            get { return _doubleCalendar; }
            set { _doubleCalendar = value; }
        }

        private List<int> _disabledDays;

        public List<int> disabledDays
        {
            get { return _disabledDays; }
            set { _disabledDays = value; }
        }

        private List<string> _disabledDates;
        /// <summary>
        /// 禁用日期
        /// </summary>
        public List<string> disabledDates
        {
            get { return _disabledDates; }
            set { _disabledDates = value; }
        }

        private bool _opposite = false;

        public bool opposite
        {
            get { return _opposite; }
            set { _opposite = value; }
        }


        private List<int> _specialDays;
        /// <summary>
        /// 高亮周几
        /// </summary>
        public List<int> specialDays
        {
            get { return _specialDays; }
            set { _specialDays = value; }
        }

        private List<string> _specialDates;
        /// <summary>
        /// 高亮日期
        /// </summary>
        public List<string> specialDates
        {
            get { return _specialDates; }
            set { _specialDates = value; }
        }

    }

}
