using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using ay.AyExpression;
using ay.Controls.Args;
using ay.Controls.Info;
using ay.Controls.Services;
using ay.Date.Info;
using ay.Enums;

namespace ay.Controls
{
    /// <summary>
    /// AyCalendar.xaml 的交互逻辑
    /// 2016-12-24 
    /// </summary>
    public partial class AyCalendar : UserControl
    {
        #region 构造函数


        public AyCalendar()
        {
            InitializeComponent();

            DateRuleObjects = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(DateRule);
            Loaded += AyCalendar_Loaded;
        }

        /// <summary>
        /// 返回选中日期星期几
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public string GetWeekChinese()
        //{
        //    return SelectedDateTime.Value.ToString("dddd", culture);
        //}
        public AyCalendar(string dateRule)
        {
            InitializeComponent();
            this.DateRule = dateRule;
            //实例化rule
            SetSelectModeFromDateRule();
            Loaded += AyCalendar_Loaded;
        }
        public AyCalendar(string dateRule, AyDateRuleJsonToObjects ruleObject)
        {
            InitializeComponent();
            this.DateRule = dateRule;
            this.DateRuleObjects = ruleObject;
            Loaded += AyCalendar_Loaded;
        }

        #endregion


        #region 用于DisabledDates  2017-3-8 15:52:28 AY设计
        public List<string> DisabledDatesStrings;
        //用于SpecialDatesStrings    2017-3-10 13:33:40
        public List<string> SpecialDatesStrings;
        #endregion


        private void AyCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyCalendar_Loaded;
            InitMinMaxDateTime();
            //判断今天是否在有效期内
            if (MINMAX.IsNotNull())
            {

                DateTime nowdate = DateTime.Now;
                if (MINMAX.Item1.Count > 0)
                {
                    foreach (var subtime in MINMAX.Item1)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectMode == AyDatePickerSelectMode.HM || SelectMode == AyDatePickerSelectMode.HMS)
                        {
                            if (nowdate < subtime.Value)
                            {
                                btnToday.IsEnabled = false;
                                break;
                            }
                        }
                        else
                        {

                            if (nowdate.Date < subtime.Value.Date)
                            {
                                btnToday.IsEnabled = false;
                                break;
                            }

                        }
                    }
                }
                if (MINMAX.Item2.Count > 0)
                {
                    foreach (var subtime in MINMAX.Item2)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectMode == AyDatePickerSelectMode.HM || SelectMode == AyDatePickerSelectMode.HMS)
                        {
                            if (nowdate > subtime.Value)
                            {
                                btnToday.IsEnabled = false;
                                break;
                            }
                        }
                        else
                        {
                            if (nowdate.Date > subtime.Value.Date)
                            {
                                btnToday.IsEnabled = false;
                                break;
                            }
                        }
                    }
                }
                //禁用周几的规则
                if (DateRuleObjects.IsNotNull())
                {
                    if (DateRuleObjects.disabledDays.IsNotNull() && DateRuleObjects.disabledDays.Count > 0)
                    {

                        DateTime dtnow = DateTime.Now;
                        var _1 = dtnow.DayOfWeek.GetHashCode();
                        foreach (var disabledDay in DateRuleObjects.disabledDays)
                        {
                            if (_1 == disabledDay)
                            {
                                btnToday.IsEnabled = false;
                                break;
                            }
                        }
                    }
                    //禁用特殊指定日期,这里决定是否禁用今天按钮
                    if (DateRuleObjects.disabledDates.IsNotNull() && DateRuleObjects.disabledDates.Count > 0)
                    {
                        if (DisabledDatesStrings == null) DisabledDatesStrings = new List<string>();
                        DateTime dtnow = DateTime.Now;
                        foreach (var disabledDate in DisabledDatesStrings)
                        {

                            //正则处理
                            if (disabledDate.IndexOf(":") < 0)
                            {
                                bool d = Regex.IsMatch(dtnow.ToString("yyyy-MM-dd"), disabledDate);
                                if (d)
                                {
                                    btnToday.IsEnabled = false;
                                    //break;
                                }
                            }
                            else
                            {
                                bool d = Regex.IsMatch(dtnow.ToString("yyyy-MM-dd HH:mm:ss"), disabledDate);
                                if (d)
                                {
                                    btnToday.IsEnabled = false;
                                    //break;
                                }
                            }
                            if (DateRuleObjects.opposite)
                            {
                                btnToday.IsEnabled = !btnToday.IsEnabled;
                            }
                        }
                        act_time.SetDisabledDatesStrings(DisabledDatesStrings, DateRuleObjects.opposite);
                        this.txt_Year.DisplayDatesStrings = this.DisabledDatesStrings;
                        this.txt_Month.DisabledDatesStrings = this.DisabledDatesStrings;
                        this.txt_Year.setOpposite = this.DateRuleObjects.opposite;
                        this.txt_Month.setOpposite = this.DateRuleObjects.opposite;
                    }
                }
            }

            DateTime? calcDateTime = null;
            //计算时间
            if (DateRuleObjects.IsNotNull())
            {
                if (DateBoxInput.IsNotNull())
                {
                    //如果文本框为空,startDate不为空，则展开时间为  startDate
                    //如果文本框不为空,startDate不为空，alwaysUseStartDate为true，则展开时间为  startDate
                    //如果文本框不为空,startDate不考虑，alwaysUseStartDate为false，则展开时间为  selectedDateTime
                    if ((DateBoxInput.Text.IsNullAndTrimAndEmpty() && !DateRuleObjects.startDate.IsNullAndTrimAndEmpty())
                        || (SelectedDateTime.HasValue && !DateRuleObjects.startDate.IsNullAndTrimAndEmpty() && DateRuleObjects.alwaysUseStartDate)
                        )
                    {
                        calcDateTime = AyDateStrictExpression.Convert(DateRuleObjects.startDate);
                        //calcDateTime = ReturnStartDate(DateRuleObjects.startDate);
                    }
                    else if (SelectedDateTime.HasValue && !DateRuleObjects.alwaysUseStartDate)
                    {
                        calcDateTime = SelectedDateTime;
                    }
                }
                else
                {
                    if (SelectedDateTime.HasValue)
                    {
                        calcDateTime = SelectedDateTime;
                    }
                    else
                    {
                        calcDateTime = DateTime.Now;
                    }
                }

            }
            if (!calcDateTime.HasValue)
            {
                calcDateTime = DateTime.Now;
            }

            DateTime now = DateTime.Now;
            switch (SelectMode)
            {
                case AyDatePickerSelectMode.DateTime:
                    SelectedDateTime = calcDateTime;
                    ControlMinMaxShow();
                    act_time.OnTimeChanged += Act_time_OnTimeChanged;
                    break;
                case AyDatePickerSelectMode.DateTimeNoSecond:
                    SelectedDateTime = new DateTime(calcDateTime.Value.Year, calcDateTime.Value.Month, calcDateTime.Value.Day, calcDateTime.Value.Hour, calcDateTime.Value.Minute, 0);
                    ControlMinMaxShow();
                    act_time.OnTimeChanged += Act_time_OnTimeChanged;
                    break;
                case AyDatePickerSelectMode.DateTimeH:
                    SelectedDateTime = new DateTime(calcDateTime.Value.Year, calcDateTime.Value.Month, calcDateTime.Value.Day, calcDateTime.Value.Hour, 0, 0);
                    ControlMinMaxShow();
                    act_time.OnTimeChanged += Act_time_OnTimeChanged;
                    break;
                case AyDatePickerSelectMode.OnlySelectDate:
                    SelectedDateTime = new DateTime(calcDateTime.Value.Year, calcDateTime.Value.Month, calcDateTime.Value.Day, 0, 0, 0);
                    clickdayisClose = true;
                    break;
                case AyDatePickerSelectMode.YearMonth:
                    SelectedDateTime = new DateTime(calcDateTime.Value.Year, calcDateTime.Value.Month, 1, 0, 0, 0);
                    clickdayisClose = true;
                    break;
                case AyDatePickerSelectMode.HM:
                    SelectedDateTime = new DateTime(now.Year, now.Month, now.Day, calcDateTime.Value.Hour, calcDateTime.Value.Minute, 0);
                    ControlMinMaxShow();
                    clickdayisClose = true;
                    act_time.OnTimeChanged += Act_time_OnTimeChanged;
                    break;
                case AyDatePickerSelectMode.HMS:
                    SelectedDateTime = new DateTime(now.Year, now.Month, now.Day, calcDateTime.Value.Hour, calcDateTime.Value.Minute, calcDateTime.Value.Second);
                    clickdayisClose = true;
                    ControlMinMaxShow();
                    act_time.OnTimeChanged += Act_time_OnTimeChanged;
                    break;
            }

            txt_Month.OnAyBoxLostFocus += Txt_Month_OnAyBoxLostFocus;
            txt_Month.OnAyBoxMouseWheeled += Txt_Month_OnAyBoxMouseWheeled;
            txt_Year.OnAyBoxLostFocus += Txt_Year_OnAyBoxLostFocus;
            txt_Year.OnAyBoxMouseWheeled += Txt_Year_OnAyBoxMouseWheeled;

            if (!SelectedDateTime.HasValue)
            {
                SelectedDateTime = DateTime.Now;
            }

            if (DateRuleObjects.doubleCalendar)
            {
          
                abm = new AyDateBoxMonth();
                abm.Margin = new Thickness(0,0,10,0);
                abm.Rule = "required";
        
               
                Grid.SetColumn(abm, 11);
                gridCdrToolsBar.Children.Add(abm);

                aby = new AyDateBoxYear();
                aby.Rule = "required";
                Grid.SetColumn(aby, 12);
                gridCdrToolsBar.Children.Add(aby);


                Grid.SetColumn(PART_NextMonthButton, 14);
                Grid.SetColumn(PART_NextYearButton, 15);

                //加一个月
                abm.Text = SelectedNextMonthDateTime.Month.ToString();
                aby.Text = SelectedNextMonthDateTime.Year.ToString();
                Binding binding = new Binding { Path = new PropertyPath("Text"), Source = aby, Mode = BindingMode.TwoWay };
                abm.SetBinding(AyDateBoxMonth.YearProperty, binding);

                abm.OnAyBoxLostFocus += abm_OnAyBoxLostFocus; ;
                abm.OnAyBoxMouseWheeled += abm_OnAyBoxMouseWheeled;
                aby.OnAyBoxLostFocus += aby_OnAyBoxLostFocus;
                aby.OnAyBoxMouseWheeled += aby_OnAyBoxMouseWheeled;
                GridService.SetColumns(DateListGrid, "* *");
                DateList2 = new AyCalendarDateList();


                Grid.SetColumn(DateList2, 1);
                DateListGrid.Children.Add(DateList2);

                DateList2.OnClickItem += DateList_OnClickItem;
            }


            SyncTextBoxDateTime();
            SyncDaysInfo();
            DateList.OnClickItem += DateList_OnClickItem;
        }

        #region 双月年月事件绑定
        AyCalendarDateList DateList2 = null;
        public DateTime SelectedNextMonthDateTime
        {
            get
            {
                return SelectedDateTime.Value.AddMonths(1);
            }
        }

        private void abm_OnAyBoxLostFocus(object sender, EventArgs e)
        {
            MonthBoxTriggerDateListUpdate2();
        }
        private void abm_OnAyBoxMouseWheeled(object sender, EventArgs e)
        {
            if (abm.IsEnabled)
            {
                MonthBoxTriggerDateListUpdate2();
            }

        }


        private void aby_OnAyBoxLostFocus(object sender, EventArgs e)
        {
            YearBoxTriggerDateListUpdate2();
        }

        private void aby_OnAyBoxMouseWheeled(object sender, EventArgs e)
        {
            if (aby.IsEnabled)
            {
                YearBoxTriggerDateListUpdate2();
            }
        }



        #endregion

        AyDateBoxMonth abm = null;
        AyDateBoxYear aby = null;

        private void DateList_OnClickItem(object sender, AyDateListItemClickEventArgs e)
        {
            ChangedWhenTimeTrigger(e.PickerItem);
            FocusDateBoxInput();
        }


        #region 拓展事件
        public event EventHandler<EventArgs> OnCleared;
        /// <summary>
        /// 当day被单击的时候触发
        /// </summary>
        public event EventHandler<AyDatePickEventArgs> OnPicked;
        #endregion

        ///// <summary>
        ///// 上一次选择的日期
        ///// 2017-2-13 13:45:05
        ///// 为了方便 用户点击确定键时候，时间是否发生变化，从而决定是否触发OnPicked事件
        ///// </summary>
        //public DateTime? LastPickedDateTime = null;

        /// <summary>
        /// 根据daterule，设置selectmode
        /// ay 2017-2-9 16:10:15
        /// </summary>
        private void SetSelectModeFromDateRule()
        {
            DateRuleObjects = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(DateRule);
            //if (DateRuleObjects.dateFmt.IsNullAndTrimAndEmpty()) DateRuleObjects.dateFmt = "yyyy-MM-dd";
            AyCalendarFMT _fmt = AyCalendarService.GetAyCalendarFMT(DateRuleObjects.dateFmt);
            switch (_fmt)
            {
                case AyCalendarFMT.None:
                    break;
                case AyCalendarFMT.YearMonth:
                    SelectMode = AyDatePickerSelectMode.YearMonth;
                    break;
                case AyCalendarFMT.YearMonthDay:
                    SelectMode = AyDatePickerSelectMode.OnlySelectDate;
                    break;
                case AyCalendarFMT.YearMonthDayH:
                    SelectMode = AyDatePickerSelectMode.DateTimeH;
                    break;
                case AyCalendarFMT.YearMonthDayHM:
                    SelectMode = AyDatePickerSelectMode.DateTimeNoSecond;
                    break;
                case AyCalendarFMT.YearMonthDayHMS:
                    SelectMode = AyDatePickerSelectMode.DateTime;
                    break;
                case AyCalendarFMT.HM:
                    SelectMode = AyDatePickerSelectMode.HM;
                    break;
                case AyCalendarFMT.HMS:
                    SelectMode = AyDatePickerSelectMode.HMS;
                    break;
                default:
                    break;
            }
        }

        private void Act_time_OnTimeChanged(object sender, EventArgs e)
        {
            UpdateWhenTimeBoxChanged();
        }

        private void UpdateWhenTimeBoxChanged()
        {
            DateTime now = DateTime.Now;
            switch (SelectMode)
            {
                case AyDatePickerSelectMode.DateTime:
                    SelectedDateTime = new DateTime(SelectedDateTime.Value.Date.Year, SelectedDateTime.Value.Date.Month, SelectedDateTime.Value.Date.Day, act_time.Hour.ToInt(), act_time.Minute.ToInt(), act_time.Second.ToInt());
                    SetSelectedDateTimeToTimer();
                    ControlMinMaxShow();
                    //控制时分秒
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case AyDatePickerSelectMode.DateTimeH:
                    SelectedDateTime = new DateTime(SelectedDateTime.Value.Date.Year, SelectedDateTime.Value.Date.Month, SelectedDateTime.Value.Date.Day, act_time.Hour.ToInt(), 0, 0);
                    SetSelectedDateTimeToTimer();
                    ControlMinMaxShow();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH时");
                    break;
                case AyDatePickerSelectMode.DateTimeNoSecond:
                    SelectedDateTime = new DateTime(SelectedDateTime.Value.Date.Year, SelectedDateTime.Value.Date.Month, SelectedDateTime.Value.Date.Day, act_time.Hour.ToInt(), act_time.Minute.ToInt(), 0);
                    SetSelectedDateTimeToTimer();
                    ControlMinMaxShow();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm");
                    break;

                case AyDatePickerSelectMode.OnlySelectDate:
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd");

                    break;
                case AyDatePickerSelectMode.YearMonth:
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM");

                    break;
                case AyDatePickerSelectMode.HM:
                    SelectedDateTime = new DateTime(now.Year, now.Month, now.Day, act_time.Hour.ToInt(), act_time.Minute.ToInt(), 0);
                    SetSelectedDateTimeToTimer();
                    ControlMinMaxShow();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("HH:mm");

                    break;
                case AyDatePickerSelectMode.HMS:
                    SelectedDateTime = new DateTime(now.Year, now.Month, now.Day, act_time.Hour.ToInt(), act_time.Minute.ToInt(), act_time.Second.ToInt());
                    SetSelectedDateTimeToTimer();
                    ControlMinMaxShow();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("HH:mm:ss");

                    break;
            }
        }

        #region 依赖属性
        /// <summary>
        /// 选中的日期
        /// AY 2016-11-28 13:27:40
        /// </summary>
        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }


        // Using a DependencyProperty as the backing store for SelectedDateTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(AyCalendar), new PropertyMetadata(null));
        /// <summary>
        /// 日期的选择模式
        /// </summary>
        public AyDatePickerSelectMode SelectMode
        {
            get { return (AyDatePickerSelectMode)GetValue(SelectModeProperty); }
            set { SetValue(SelectModeProperty, value); }
        }

        public static readonly DependencyProperty SelectModeProperty =
            DependencyProperty.Register("SelectMode", typeof(AyDatePickerSelectMode), typeof(AyCalendar), new PropertyMetadata(AyDatePickerSelectMode.OnlySelectDate, OnSelectDateChanged));

        private static void OnSelectDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = (d as AyCalendar);
            if (_1.SelectMode == AyDatePickerSelectMode.OnlySelectDate)
            {
                _1.cdrTime.Visibility = Visibility.Collapsed;
            }
            else
            {
                _1.cdrTime.Visibility = Visibility.Visible;
            }
        }


        #endregion

        #region 默认年月事件绑定
        private void Txt_Month_OnAyBoxMouseWheeled(object sender, EventArgs e)
        {
            if (txt_Month.IsEnabled)
            {
                MonthBoxTriggerDateListUpdate();
            }

        }

        private void MonthBoxTriggerDateListUpdate()
        {
            if (SelectedDateTime.HasValue)
            {
                if (txt_Month.Text != SelectedDateTime.Value.Month.ToString())
                {
                    SyncDaysInfo();
                }
                if (DateRuleObjects.doubleCalendar)
                {

                    int month = txt_Month.Text.ToInt();
                    if (month < txt_Month.MaxValue)
                    {
                        abm.Text = (month + 1).ToString();
                    }
                    else
                    {
                        var _0 = txt_Year.Text.ToInt() + 1;
                        abm.Text = "1";
                        aby.Text = _0.ToString();
                    }
                    if (month == 12)
                    {
                        aby.Text = SelectedNextMonthDateTime.Year.ToString();
                    }
                    else
                    {
                        aby.Text = txt_Year.Text;
                    }


                }
            }
        }

        private void MonthBoxTriggerDateListUpdate2()
        {
            if (aby.Text.ToInt() > YearStrick.MAXYEAR)
            {
                abm.Text = "1";
                return;
            }
            int month = abm.Text.ToInt();
            if (month <= abm.MaxValue)
            {
                int _m1 = month - 1;
                if (_m1 == 0)
                {
                    _m1 = 12;
                }
                txt_Month.Text = _m1.ToString();
            }
            if (month == 1)
            {
                var _0 = aby.Text.ToInt() - 1;
                txt_Year.Text = _0.ToString();
            }
            else
            {
                txt_Year.Text = aby.Text;
            }
            SyncDaysInfo();
        }
        private void YearBoxTriggerDateListUpdate2()
        {
            var _month = abm.Text.ToInt();

            if (_month == 1)
            {
                txt_Year.Text = (aby.Text.ToInt() - 1).ToString();
            }
            else
            {
                txt_Year.Text = aby.Text;
            }
            SyncDaysInfo();
        }

        private void YearBoxTriggerDateListUpdate()
        {
            if (SelectedDateTime.HasValue)
            {
                if (txt_Year.Text != SelectedDateTime.Value.Year.ToString())
                {

                    //当前的年和月是否超过了MINMAX
                    var year = txt_Year.Text.ToInt();
                    var month = txt_Month.Text.ToInt();

                    if (MINMAX.Item1.Count > 0)
                    {
                        foreach (var subtime in MINMAX.Item1)
                        {
                            if (subtime == null)
                            {
                                continue;
                            }
                            if (subtime.Value.Year == year && month < subtime.Value.Month)
                            {
                                txt_Month.Text = subtime.Value.Month.ToString();
                                //调整日,预期将被调整的日期，调整为最小日期   2017-2-28 11:09:23
                                SelectedDateTime = subtime.Value;
                            }
                        }
                    }
                    if (MINMAX.Item2.Count > 0)
                    {
                        foreach (var subtime in MINMAX.Item2)
                        {
                            if (subtime == null)
                            {
                                continue;
                            }
                            if (subtime.Value.Year == year && month > subtime.Value.Month)
                            {
                                txt_Month.Text = subtime.Value.Month.ToString();
                                //调整日 预期将被调整的日期，调整为最大日期
                                SelectedDateTime = subtime.Value;

                            }
                        }
                    }


                    //if (MINMAX.Item2.HasValue)
                    //{
                    //    if (MINMAX.Item2.Value.Year == year && month > MINMAX.Item2.Value.Month)
                    //    {
                    //        txt_Month.Text = MINMAX.Item2.Value.Month.ToString();
                    //        //调整日 预期将被调整的日期，调整为最大日期
                    //        SelectedDateTime = MINMAX.Item2.Value;

                    //    }
                    //}


                    SyncDaysInfo();
                    if (DateRuleObjects.doubleCalendar)
                    {
                        //var year = txt_Year.Text.ToInt();
                        //if (year < txt_Year.MaxValue)
                        //{

                        var _month = txt_Month.Text.ToInt();
                        if (_month == 12)
                        {
                            aby.Text = (year + 1).ToString();
                        }
                        else
                        {
                            aby.Text = txt_Year.Text;
                        }
                    }
                }
            }
        }

        private void Txt_Year_OnAyBoxMouseWheeled(object sender, EventArgs e)
        {
            if (txt_Year.IsEnabled)
            {
                YearBoxTriggerDateListUpdate();
            }
        }

        private void Txt_Year_OnAyBoxLostFocus(object sender, EventArgs e)
        {
            YearBoxTriggerDateListUpdate();
        }

        private void Txt_Month_OnAyBoxLostFocus(object sender, EventArgs e)
        {
            MonthBoxTriggerDateListUpdate();
        }

        #endregion



        public Visibility IsShowClear
        {
            get { return (Visibility)GetValue(IsShowClearProperty); }
            set { SetValue(IsShowClearProperty, value); }
        }

        public static readonly DependencyProperty IsShowClearProperty =
            DependencyProperty.Register("IsShowClear", typeof(Visibility), typeof(AyCalendar), new UIPropertyMetadata(Visibility.Visible));



        public Visibility IsShowToday
        {
            get { return (Visibility)GetValue(IsShowTodayProperty); }
            set { SetValue(IsShowTodayProperty, value); }
        }

        public static readonly DependencyProperty IsShowTodayProperty =
            DependencyProperty.Register("IsShowToday", typeof(Visibility), typeof(AyCalendar), new UIPropertyMetadata(Visibility.Visible));




        public void SyncTextBoxDateTime()
        {
            txt_Month.Text = SelectedDateTime.Value.Month.ToString();
            txt_Year.Text = SelectedDateTime.Value.Year.ToString();

            switch (SelectMode)
            {
                case AyDatePickerSelectMode.DateTime:
                    act_time.Hour = SelectedDateTime.Value.Hour.ToString();
                    act_time.Minute = SelectedDateTime.Value.Minute.ToString();
                    act_time.Second = SelectedDateTime.Value.Second.ToString();
                    break;
                case AyDatePickerSelectMode.DateTimeH:
                    act_time.Hour = SelectedDateTime.Value.Hour.ToString();
                    break;

                case AyDatePickerSelectMode.DateTimeNoSecond:
                    act_time.Hour = SelectedDateTime.Value.Hour.ToString();
                    act_time.Minute = SelectedDateTime.Value.Minute.ToString();

                    break;
                case AyDatePickerSelectMode.HM:
                    act_time.Hour = SelectedDateTime.Value.Hour.ToString();
                    act_time.Minute = SelectedDateTime.Value.Minute.ToString();
                    break;
                case AyDatePickerSelectMode.HMS:
                    act_time.Hour = SelectedDateTime.Value.Hour.ToString();
                    act_time.Minute = SelectedDateTime.Value.Minute.ToString();
                    act_time.Second = SelectedDateTime.Value.Second.ToString();
                    break;

            }



        }

        public void ControlMinMaxShow()
        {

            if (MINMAX.IsNotNull())
            {
                if (MINMAX.Item1.Count > 0)
                {
                    foreach (var subtime in MINMAX.Item1)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectedDateTime < subtime.Value)
                        {
                            SelectedDateTime = subtime.Value;
                            txt_Year.Text = SelectedDateTime.Value.Year.ToString();
                            txt_Month.Text = SelectedDateTime.Value.Month.ToString();
                            act_time.Hour = SelectedDateTime.Value.Hour.ToString();
                            act_time.Minute = SelectedDateTime.Value.Minute.ToString();
                            act_time.Second = SelectedDateTime.Value.Second.ToString();
                            if (DateRuleObjects.doubleCalendar)
                            {
                                if (aby.IsNotNull())
                                {
                                    var _ = SelectedNextMonthDateTime;
                                    aby.Text = _.Year.ToString();
                                    abm.Text = _.Month.ToString();
                                }
                            }
                            break;
                        }
                    }
                }

                if (MINMAX.Item2.Count > 0)
                {
                    foreach (var subtime in MINMAX.Item2)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectedDateTime > subtime.Value)
                        {
                            SelectedDateTime = subtime.Value;
                            txt_Year.Text = SelectedDateTime.Value.Year.ToString();
                            txt_Month.Text = SelectedDateTime.Value.Month.ToString();

                            act_time.Hour = SelectedDateTime.Value.Hour.ToString();
                            act_time.Minute = SelectedDateTime.Value.Minute.ToString();
                            act_time.Second = SelectedDateTime.Value.Second.ToString();
                            if (DateRuleObjects.doubleCalendar)
                            {
                                if (aby.IsNotNull())
                                {
                                    var _ = SelectedNextMonthDateTime;
                                    aby.Text = _.Year.ToString();
                                    abm.Text = _.Month.ToString();
                                }
                            }
                            break;
                        }
                    }
                }
                //周几限制
                if (DateRuleObjects.IsNotNull())
                {
                    bool istriggerfalse = false;
                    if (DateRuleObjects.disabledDays.IsNotNull() && DateRuleObjects.disabledDays.Count > 0)
                    {
                        btnOk.IsEnabled = true;

                        var _1 = SelectedDateTime.Value.DayOfWeek.GetHashCode();
                        foreach (var disabledDay in DateRuleObjects.disabledDays)
                        {
                            if (_1 == disabledDay)
                            {
                                btnOk.IsEnabled = false;
                                istriggerfalse = true;
                                break;
                            }
                        }
                    }

                    if (DateRuleObjects.disabledDates.IsNotNull() && DateRuleObjects.disabledDates.Count > 0)
                    {
                        if (!istriggerfalse)
                        {
                            btnOk.IsEnabled = true;
                        }
                        //限制disabledDate,返回false，说明有问题，应该不能确定了。
                        var _validateItemResult = AyCalendarService.ValidateRegexDateByAyCalendar(SelectedDateTime.Value, this.DisabledDatesStrings, DateRuleObjects.opposite);
                        if (!_validateItemResult)
                        {
                            btnOk.IsEnabled = false;
                        }

                    }



                    //禁用特殊指定日期,这里决定是否禁用今天按钮
                    //if (DateRuleObjects.disabledDates.IsNotNull() && DateRuleObjects.disabledDates.Count > 0)
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    if (DisplayDates.IsNull())
                    //    {
                    //        DisplayDates = new Dictionary<string, DateTime?>();
                    //    }
                    //    foreach (var disabledDate in DateRuleObjects.disabledDates)
                    //    {
                    //        var _ti = AyCalendarService.hasTeShu(disabledDate);
                    //        if (_ti)
                    //        {
                    //            DateTime? dtd = null;
                    //            if (DisplayDates.Keys.Contains(disabledDate))
                    //            {
                    //                dtd = DisplayDates[disabledDate];
                    //            }
                    //            else
                    //            {
                    //                dtd = AyDateStrictExpression.Convert(disabledDate);
                    //                DisplayDates.Add(disabledDate, dtd);
                    //            }
                    //            if (disabledDate.IndexOf(":") < 0)
                    //            {
                    //                if (dtd.Date == dtnow.Date)
                    //                {
                    //                    btnToday.IsEnabled = false;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            //正则处理
                    //            if (disabledDate.IndexOf(":") < 0)
                    //            {
                    //                bool d = Regex.IsMatch(dtnow.ToString("yyyy-MM-dd"), disabledDate);
                    //                if (d)
                    //                {
                    //                    btnToday.IsEnabled = false;
                    //                    break;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                bool d = Regex.IsMatch(dtnow.ToString("yyyy-MM-dd HH:mm:ss"), disabledDate);
                    //                if (d)
                    //                {
                    //                    btnToday.IsEnabled = false;
                    //                    break;
                    //                }

                    //            }

                    //        }
                    //    }
                    //}
                    //dddddddddddddddddd

                }


            }
        }
        /// <summary>
        /// 同步列表day信息
        /// </summary>
        public void SyncDaysInfo()
        {
            try
            {
                int _1 = txt_Month.Text.ToInt();

                int _2 = txt_Year.Text.ToInt();
                if ((_1 >= txt_Month.MinValue && _1 <= txt_Month.MaxValue) && (_2 >= txt_Year.MinValue && _2 <= txt_Year.MaxValue))
                {
                    DateTime now = DateTime.Now;
                    switch (SelectMode)
                    {
                        case AyDatePickerSelectMode.DateTime:
                            try
                            {
                                SelectedDateTime = new DateTime(_2, _1, SelectedDateTime.Value.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, SelectedDateTime.Value.Second);
                            }
                            catch
                            {
                                SelectedDateTime = new DateTime(_2, _1, AyDatePickerHelper.NumOfDays(_2, _1), SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, SelectedDateTime.Value.Second);
                            }
                            ControlMinMaxShow();
                            DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            break;
                        case AyDatePickerSelectMode.DateTimeH:
                            try
                            {
                                SelectedDateTime = new DateTime(_2, _1, SelectedDateTime.Value.Day, SelectedDateTime.Value.Hour, 0, 0);
                            }
                            catch
                            {
                                SelectedDateTime = new DateTime(_2, _1, AyDatePickerHelper.NumOfDays(_2, _1), SelectedDateTime.Value.Hour, 0, 0);

                            }
                            ControlMinMaxShow();
                            DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH时");
                            break;
                        case AyDatePickerSelectMode.DateTimeNoSecond:
                            try
                            {
                                SelectedDateTime = new DateTime(_2, _1, SelectedDateTime.Value.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, 0);
                            }
                            catch
                            {
                                SelectedDateTime = new DateTime(_2, _1, AyDatePickerHelper.NumOfDays(_2, _1), SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, 0);
                            }
                            ControlMinMaxShow();
                            DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm");
                            break;
                        case AyDatePickerSelectMode.OnlySelectDate:
                            try
                            {
                                SelectedDateTime = new DateTime(_2, _1, SelectedDateTime.Value.Day);
                            }
                            catch
                            {
                                SelectedDateTime = new DateTime(_2, _1, AyDatePickerHelper.NumOfDays(_2, _1));
                            }
                            ControlMinMaxShow();
                            DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd");
                            break;
                        case AyDatePickerSelectMode.YearMonth:
                            SelectedDateTime = new DateTime(_2, _1, 1);
                            ControlMinMaxShow();
                            DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM");
                            break;
                        case AyDatePickerSelectMode.HM:
                            SelectedDateTime = new DateTime(now.Year, now.Month, now.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, 0);

                            DateShowInfo.Text = SelectedDateTime.Value.ToString("HH:mm");
                            break;
                        case AyDatePickerSelectMode.HMS:
                            SelectedDateTime = new DateTime(now.Year, now.Month, now.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, SelectedDateTime.Value.Second);

                            DateShowInfo.Text = SelectedDateTime.Value.ToString("HH:mm:ss");
                            break;

                    }
                }
                else
                {
                    return;
                }

                SetList(SelectedDateTime.Value.Year, SelectedDateTime.Value.Month, SelectedDateTime.Value.Day);
            }
            catch
            {


            }
            FocusDateBoxInput();
        }


        public string DateRule
        {
            get { return (string)GetValue(DateRuleProperty); }
            set { SetValue(DateRuleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateRule.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateRuleProperty =
            DependencyProperty.Register("DateRule", typeof(string), typeof(AyCalendar), new PropertyMetadata(""));

        public Button CreateYearMonthButtons(string content, DateTime ctag, int isHMS = 0)
        {
            Button btn = new Button();
            btn.ClickMode = ClickMode.Press;
            btn.Content = content.ToString();
            btn.Height = 40;
            btn.HorizontalAlignment = HorizontalAlignment.Stretch;
            btn.Padding = new Thickness(0);
            btn.SetResourceReference(Button.StyleProperty, "AyDateBoxCalendar.Button");
            if (MINMAX != null)
            {
                if (isHMS == 0)
                {
                    AyCalendarService.SetClickYearMonthButtonsEnabled(btn, ctag, MINMAX.Item1, MINMAX.Item2);
                }
                else if (isHMS == 1)
                {
                    AyCalendarService.SetClickHMSButtonsEnabled(btn, ctag, MINMAX.Item1, MINMAX.Item2);
                }

            }

            btn.Cursor = Cursors.Hand;
            btn.Tag = ctag;
            btn.Click += Btn_Click; ;
            return btn;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = (DateTime)((sender as Button).Tag);
            SelectedDateTime = dt;
            WhenDateTimePicked();
            FocusDateBoxInput();
        }
        public void SetYearMonthList(int year, int month)
        {
            sp_YearMonthList.Children.Clear();
            yearMonthTitle.Visibility = sp_YearMonthList.Visibility = Visibility.Visible;
            cdrTime.Visibility = Visibility.Collapsed;
            //sp_weekhead.Visibility = Visibility.Collapsed;
            SetDateListVisibility();
            cdrTime.Visibility = Visibility.Collapsed;
            var _dts = ReturnYearMonth(year, month);
            if (DateRuleObjects._dateFmt == null)
            {
                DateRuleObjects._dateFmt = "yyyy-MM";
            }
            foreach (var item in _dts)
            {
                var _ = CreateYearMonthButtons(item.ToString(DateRuleObjects.dateFmt), item);

                sp_YearMonthList.Children.Add(_);
            }
        }
        public void SetHMList(int hour)
        {
            sp_YearMonthList.Children.Clear();
            yearMonthTitle.Visibility = sp_YearMonthList.Visibility = Visibility.Visible;
            cdrTools.Visibility = Visibility.Collapsed;
            //sp_weekhead.Visibility = Visibility.Collapsed;
            SetDateListVisibility();
            List<DateTime> dts = new List<DateTime>();
            DateTime now = DateTime.Now;
            DateTime dt3 = new DateTime(now.Year, now.Month, now.Day, hour, 0, 0);
            dts.Add(dt3.AddMinutes(-60));
            dts.Add(dt3.AddMinutes(-30));
            dts.Add(dt3);
            dts.Add(dt3.AddMinutes(30));
            dts.Add(dt3.AddMinutes(60));

            if (DateRuleObjects._dateFmt == null)
            {
                DateRuleObjects._dateFmt = "HH:mm";
            }
            foreach (var item in dts)
            {
                sp_YearMonthList.Children.Add(CreateYearMonthButtons(item.ToString(DateRuleObjects.dateFmt), item, 1));
            }
        }
        public void SetDateListVisibility(bool show = false)
        {
            if (show)
            {
                DateListGrid.Visibility = Visibility.Visible;
            }
            else
            {
                DateListGrid.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 设置时分模式列表
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        public void SetHMSList(int hour, int minute)
        {
            sp_YearMonthList.Children.Clear();
            yearMonthTitle.Visibility = sp_YearMonthList.Visibility = Visibility.Visible;
            cdrTools.Visibility = Visibility.Collapsed;
            //sp_weekhead.Visibility = Visibility.Collapsed;
            SetDateListVisibility();
            List<DateTime> dts = new List<DateTime>();
            DateTime now = DateTime.Now;
            DateTime dt3 = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
            dts.Add(dt3.AddSeconds(-60));
            dts.Add(dt3.AddSeconds(-30));
            dts.Add(dt3);
            dts.Add(dt3.AddSeconds(30));
            dts.Add(dt3.AddSeconds(60));
            if (DateRuleObjects._dateFmt == null)
            {
                DateRuleObjects._dateFmt = "HH:mm:ss";
            }
            foreach (var item in dts)
            {
                sp_YearMonthList.Children.Add(CreateYearMonthButtons(item.ToString(DateRuleObjects.dateFmt), item, 1));
            }
        }
        public List<DateTime> ReturnYearMonth(int year, int month)
        {
            List<DateTime> dts = new List<DateTime>();
            if (year == 1900 && month < 4)
            {
                DateTime dt1 = new DateTime(1900, 1, 1, 0, 0, 0);
                DateTime dt2 = new DateTime(1900, 2, 1, 0, 0, 0);
                DateTime dt3 = new DateTime(1900, 3, 1, 0, 0, 0);
                DateTime dt4 = new DateTime(1900, 4, 1, 0, 0, 0);
                DateTime dt5 = new DateTime(1900, 5, 1, 0, 0, 0);
                dts.Add(dt1);
                dts.Add(dt2);
                dts.Add(dt3);
                dts.Add(dt4);
                dts.Add(dt5);

            }
            else if (year == 2099 && month >= 10)
            {
                DateTime dt1 = new DateTime(2099, 8, 1, 0, 0, 0);
                DateTime dt2 = new DateTime(2099, 9, 1, 0, 0, 0);
                DateTime dt3 = new DateTime(2099, 10, 1, 0, 0, 0);
                DateTime dt4 = new DateTime(2099, 11, 1, 0, 0, 0);
                DateTime dt5 = new DateTime(2099, 12, 1, 0, 0, 0);
                dts.Add(dt1);
                dts.Add(dt2);
                dts.Add(dt3);
                dts.Add(dt4);
                dts.Add(dt5);
            }
            else
            {
                //正常
                DateTime dt3 = new DateTime(year, month, 1, 0, 0, 0);
                dts.Add(dt3.AddMonths(-2));
                dts.Add(dt3.AddMonths(-1));
                dts.Add(dt3);
                dts.Add(dt3.AddMonths(1));
                dts.Add(dt3.AddMonths(2));
            }
            return dts;
        }

        Tuple<List<DateTime?>, List<DateTime?>> MINMAX;
        public AyDateBoxCalendar MinDateReferToElement;
        public AyDateBoxCalendar MaxDateReferToElement;

        bool firstInitMINMAX = true;
        public void InitMinMaxDateTime()
        {
            if (firstInitMINMAX)
            {
                MINMAX = AyCalendarService.FilterDatePickerItem(DateRuleObjects, MinDateReferToElement, MaxDateReferToElement);
                firstInitMINMAX = false;
            }

        }

        private void SetList(int year, int month, int SelectedDay)
        {
            DateList.DateRuleObjects = DateRuleObjects;

            if (SelectMode == AyDatePickerSelectMode.HM)
            {
                SetSelectedDateTimeToTimer();
                act_time.SetMinMaxDateTime(MINMAX.Item1, MINMAX.Item2);
                SetHMList(SelectedDateTime.Value.Hour);
            }
            else
            if (SelectMode == AyDatePickerSelectMode.HMS)
            {
                SetSelectedDateTimeToTimer();
                act_time.SetMinMaxDateTime(MINMAX.Item1, MINMAX.Item2);
                SetHMSList(SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute);
            }
            else
            if (SelectMode == AyDatePickerSelectMode.YearMonth)
            {

                SetYearMonthList(year, month);
            }
            else
            {
                SetSelectedDateTimeToTimer();
                sp_YearMonthList.Visibility = Visibility.Collapsed;
                SetDateListVisibility(true);
                if (DateList.currentYear != 0)
                {
                    //启动判断
                    if (DateList.currentYear == SelectedDateTime.Value.Year
                        && DateList.currentMonth == SelectedDateTime.Value.Month
                           && DateList.currentDay == SelectedDateTime.Value.Day
                        )
                    {
                        return;
                    }
                }
                //TODON 需要设置时间吗 关于disabledDate的 2017-3-8 16:37:22


                act_time.SetMinMaxDateTime(MINMAX.Item1, MINMAX.Item2);
                DateList.DisabledDatesStrings = this.DisabledDatesStrings;

                //DateList.DisabledDates = this.DisabledDates;
                //设置最大最小日期
                DateList.firstInitMinMax = false;
                //设置月的最大最小
                txt_Month.MinDateTime = MINMAX.Item1;
                txt_Month.MaxDateTime = MINMAX.Item2;
                txt_Year.MinDateTime = MINMAX.Item1;
                txt_Year.MaxDateTime = MINMAX.Item2;

                DateList.MinDateCopy = MINMAX.Item1;
                DateList.MaxDateCopy = MINMAX.Item2;
                DateList.SpecialDatesStrings = this.SpecialDatesStrings;
                DateList.SetList(year, month, SelectedDay);

                if (DateRuleObjects.doubleCalendar)
                {
                    DateList2.DateRuleObjects = DateRuleObjects;
                    var _nextMonth = SelectedNextMonthDateTime;
                    abm.MinDateTime = MINMAX.Item1;
                    abm.MaxDateTime = MINMAX.Item2;
                    aby.MinDateTime = MINMAX.Item1;
                    aby.MaxDateTime = MINMAX.Item2;
                    //右侧日历每次都不会有选中的。
                    DateList2.firstInitMinMax = false;
                    DateList2.DisabledDatesStrings = this.DisabledDatesStrings;
                    //DateList2.DisabledDates = this.DisabledDates;
                    DateList2.MinDateCopy = MINMAX.Item1;
                    DateList2.MaxDateCopy = MINMAX.Item2;
                    DateList2.SpecialDatesStrings = this.SpecialDatesStrings;
                    DateList2.SetList(_nextMonth.Year, _nextMonth.Month, SelectedDay, false);
                }
                //设置周
                if (DateRuleObjects.isShowWeek && DateList.items.Count > 0)
                {
                    Width = CalcRootGridWidth();
                    if (DateRuleObjects.doubleCalendar)
                    {
                            GridService.SetColumns(gridCdrToolsBar, "40 40 10 1* 1* 10 40 40 40 40 10 1* 1* 10 40 40");
                    }
                    else {
                        GridService.SetColumns(gridCdrToolsBar, "40 40 10 1* 1* 10 40 40");
                    }
                  
                }
                else
                {
                    Width = CalcRootGridWidth();
                    if (DateRuleObjects.doubleCalendar)
                    {
                    GridService.SetColumns(gridCdrToolsBar, "40 40 10 auto auto 10 40 40 40 40 10 auto auto 10 40 40");
                }
                    else
                    {
                        GridService.SetColumns(gridCdrToolsBar, "40 40 10 auto auto 10 40 40");
                    }

         
                }


            }
        }
        //计算宽高
        public double CalcRootGridWidth()
        {
            double rg = 0;
            if (DateRuleObjects.isShowWeek)
            {
                rg = 344+ay.Utils.UIGeneric.DayWidth.Value;
            }
            else
            {
                rg = 344;
            }
            if (DateRuleObjects.doubleCalendar)
            {
                rg = rg * 2 - 7;
            }
            return rg;
        }


        public AyDateBoxCalendar DateBoxInput
        {
            get { return (AyDateBoxCalendar)GetValue(DateBoxInputProperty); }
            set { SetValue(DateBoxInputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateBoxInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateBoxInputProperty =
            DependencyProperty.Register("DateBoxInput", typeof(AyDateBoxCalendar), typeof(AyCalendar), new PropertyMetadata(null));




        /// <summary>
        /// 用于限制
        /// </summary>
        public AyDateRuleJsonToObjects DateRuleObjects { get; set; }


        private void PART_PreviousYearButton_Click(object sender, RoutedEventArgs e)
        {
            int year = txt_Year.Text.ToInt();
            if (year > txt_Year.MinValue)
            {
                txt_Year.Text = (year - 1).ToString();
                SyncDaysInfo();
                if (DateRuleObjects.doubleCalendar)
                {
                    if (aby.IsNotNull())
                    {
                        var _ = SelectedNextMonthDateTime;
                        aby.Text = _.Year.ToString();
                        abm.Text = _.Month.ToString();
                    }
                }
            }
            FocusDateBoxInput();
        }

        private void PART_PreviousMonthButton_Click(object sender, RoutedEventArgs e)
        {
            {
                int month = txt_Month.Text.ToInt();
                if (month > txt_Month.MinValue)
                {
                    txt_Month.Text = (month - 1).ToString();
                }
                else
                {

                    var _0 = txt_Year.Text.ToInt() - 1;
                    if (_0 >= txt_Year.MinValue && _0 <= txt_Year.MaxValue)
                    {
                        txt_Month.Text = "12";
                        txt_Year.Text = _0.ToString();
                    }
                    else
                    {
                        return;
                    }
                }

            }

            SyncDaysInfo();
            if (DateRuleObjects.doubleCalendar)
            {
                if (aby.IsNotNull())
                {
                    var _ = SelectedNextMonthDateTime;
                    aby.Text = _.Year.ToString();
                    abm.Text = _.Month.ToString();
                }

            }

        }

        private void PART_NextMonthButton_Click(object sender, RoutedEventArgs e)
        {

            {
                int month = txt_Month.Text.ToInt();
                if (month < txt_Month.MaxValue)
                {
                    txt_Month.Text = (month + 1).ToString();
                }
                else
                {
                    var _0 = txt_Year.Text.ToInt() + 1;
                    if (_0 >= txt_Year.MinValue && _0 <= txt_Year.MaxValue)
                    {
                        txt_Month.Text = "1";
                        txt_Year.Text = _0.ToString();
                    }

                }
            }

            SyncDaysInfo();

            if (DateRuleObjects.doubleCalendar)
            {
                if (aby.IsNotNull())
                {
                    var _ = SelectedNextMonthDateTime;
                    aby.Text = _.Year.ToString();
                    abm.Text = _.Month.ToString();
                }
                //{
                //    int month = abm.Text.ToInt();
                //    int year = aby.Text.ToInt();
                //    if (month < abm.MaxValue && year <= YearStrick.MAXYEAR)
                //    {
                //        abm.Text = (month + 1).ToString();
                //    }
                //    else
                //    {
                //        if (year <= YearStrick.MAXYEAR)
                //        {
                //            var _0 = aby.Text.ToInt() + 1;
                //            //if (_0 >= aby.MinValue && _0 <= aby.MaxValue)
                //            //{
                //            //    abm.Text = "1";
                //            //    aby.Text = _0.ToString();
                //            //}
                //            //else
                //            //{
                //            abm.Text = "1";
                //            aby.Text = _0.ToString();
                //            //}
                //        }

                //    }
                //}

            }
        }

        private void PART_NextYearButton_Click(object sender, RoutedEventArgs e)
        {
            //if (MINMAX.Item2.HasValue)
            //{
            //    //下个月1号是否超过了 最大日期
            //    var _selectDate = SelectedDateTime.Value.AddYears(1);

            //    if (_selectDate > MINMAX.Item2.Value)
            //    {
            //        if (MINMAX.Item2.Value != SelectedDateTime.Value)
            //        {
            //            SelectedDateTime = MINMAX.Item2.Value;
            //            SyncDaysInfo();
            //        }

            //        return;
            //    }
            //}

            int year = txt_Year.Text.ToInt();
            if (year < txt_Year.MaxValue)
            {
                txt_Year.Text = (year + 1).ToString();

                SyncDaysInfo();

                if (DateRuleObjects.doubleCalendar)
                {
                    //int year2 = aby.Text.ToInt();
                    //aby.Text = (year2 + 1).ToString();
                    if (aby.IsNotNull())
                    {
                        var _ = SelectedNextMonthDateTime;
                        aby.Text = _.Year.ToString();
                        abm.Text = _.Month.ToString();
                    }

                }
            }
        }

        internal bool DateItemIsInValidRange(DateTime item)
        {
            bool returnResult = true;
            if (MINMAX.Item1.Count > 0)
            {
                foreach (var subtime in MINMAX.Item1)
                {
                    if (subtime == null)
                    {
                        continue;
                    }

                    if (item < subtime.Value)
                    {
                        returnResult = false;
                        break;
                    }
                }
            }
            if (MINMAX.Item2.Count > 0)
            {
                foreach (var subtime in MINMAX.Item2)
                {
                    if (subtime == null)
                    {
                        continue;
                    }

                    if (item > subtime.Value)
                    {
                        returnResult = false;
                        break;
                    }
                }
            }
            return returnResult;
        }

        /// <summary>
        /// 用于控制下方按钮组的整体可见性可见性
        /// </summary>
        public Visibility ExtButtonsVisibility
        {
            get { return (Visibility)GetValue(ExtButtonsVisibilityProperty); }
            set { SetValue(ExtButtonsVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExtButtonsVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExtButtonsVisibilityProperty =
            DependencyProperty.Register("ExtButtonsVisibility", typeof(Visibility), typeof(AyCalendar), new PropertyMetadata(Visibility.Visible));

        internal void SetSelectedDateTimeToTimer()
        {
            act_time.SetSelectedDateTime(SelectedDateTime);
            txt_Month.SelectedDateTime = SelectedDateTime;
            txt_Year.SelectedDateTime = SelectedDateTime;
        }
        /// <summary>
        /// 单击日 是否关闭popup
        /// </summary>
        bool clickdayisClose = false;
        private void ChangedWhenTimeTrigger(AyDatePickerItem _1)
        {

            bool isclickTwo = false;
            if (SelectedDateTime.HasValue && (_1.Date.Year == SelectedDateTime.Value.Year)
                && (_1.Date.Month == SelectedDateTime.Value.Month)
                && (_1.Date.Day == SelectedDateTime.Value.Day))
            {
                isclickTwo = true;
            }
            DateTime now = DateTime.Now;
            switch (SelectMode)
            {
                case AyDatePickerSelectMode.DateTime:
                    SelectedDateTime = new DateTime(_1.Date.Year, _1.Date.Month, _1.Date.Day, act_time.txt_Hour.Text.ToInt(), act_time.txt_Minute.Text.ToInt(), act_time.txt_Second.Text.ToInt());
                    ControlMinMaxShow();
                    SetSelectedDateTimeToTimer();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case AyDatePickerSelectMode.DateTimeH:
                    SelectedDateTime = new DateTime(_1.Date.Year, _1.Date.Month, _1.Date.Day, act_time.txt_Hour.Text.ToInt(), 0, 0);
                    ControlMinMaxShow();
                    SetSelectedDateTimeToTimer();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH时");
                    break;
                case AyDatePickerSelectMode.DateTimeNoSecond:
                    SelectedDateTime = new DateTime(_1.Date.Year, _1.Date.Month, _1.Date.Day, act_time.txt_Hour.Text.ToInt(), act_time.txt_Minute.Text.ToInt(), 0);
                    ControlMinMaxShow();
                    SetSelectedDateTimeToTimer();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm");
                    break;
                case AyDatePickerSelectMode.OnlySelectDate:
                    SelectedDateTime = new DateTime(_1.Date.Year, _1.Date.Month, _1.Date.Day, 0, 0, 0);
                    ControlMinMaxShow();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd");
                    clickdayisClose = true;
                    break;
                case AyDatePickerSelectMode.YearMonth:
                    SelectedDateTime = new DateTime(_1.Date.Year, _1.Date.Month, 1, 0, 0, 0);
                    ControlMinMaxShow();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM");
                    clickdayisClose = true;
                    break;
                case AyDatePickerSelectMode.HM:

                    SelectedDateTime = new DateTime(now.Year, now.Month, now.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, 0);
                    ControlMinMaxShow();
                    SetSelectedDateTimeToTimer();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("HH:mm");
                    clickdayisClose = true;
                    break;
                case AyDatePickerSelectMode.HMS:
                    SelectedDateTime = new DateTime(now.Year, now.Month, now.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, SelectedDateTime.Value.Second);
                    ControlMinMaxShow();
                    SetSelectedDateTimeToTimer();
                    DateShowInfo.Text = SelectedDateTime.Value.ToString("HH:mm:ss");
                    clickdayisClose = true;
                    break;

            }

            //单击或者触摸  日  触发 2017-2-10 16:48:24
            AyDatePickEventArgs pickargs = new AyDatePickEventArgs(SelectedDateTime);
            if (OnPicked.IsNotNull()) OnPicked(this, pickargs);
            if (clickdayisClose || isclickTwo || DateRuleObjects.doubleCalendar)
            {
                if (DateBoxInput.IsNotNull())
                {
                    DateBoxInput.Text = SelectedDateTime.Value.
                        ToString(DateRuleObjects.dateFmt);
                    if (SetDateBoxInputPickedDate())
                    {
                        DateBoxInput.InvokeOnPicked(pickargs);
                    }

                }
                CloseParentPopup();
            }
        }
        public bool SetDateBoxInputPickedDate()
        {
            if (SelectedDateTime.HasValue)
            {
                bool _isInRange = DateItemIsInValidRange(SelectedDateTime.Value);
                if (_isInRange)
                {
                    DateBoxInput.PickedDate = SelectedDateTime;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 让绑定的文本框获得焦点
        /// 2017-2-10 16:48:20
        /// </summary>
        public void FocusDateBoxInput()
        {
            //if (DateBoxInput.IsNotNull())
            //{
            //    //FocusManager.SetFocusedElement(DateBoxInput,null);
            //    AyTime.setTimeout(50, () =>
            //    {
            //        Keyboard.Focus(DateBoxInput);
            //    });

            //    //DateBoxInput.Focus();
            //}
        }
        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            SelectedDateTime = DateTime.Now;
            SyncTextBoxDateTime();
            int _1 = txt_Month.Text.ToInt();
            int _2 = txt_Year.Text.ToInt();
            if ((_1 >= txt_Month.MinValue && _1 <= txt_Month.MaxValue) && (_2 >= txt_Year.MinValue && _2 <= txt_Year.MaxValue))
            {
                //DateTime now = DateTime.Now;
                switch (SelectMode)
                {
                    case AyDatePickerSelectMode.DateTime:
                        SelectedDateTime = new DateTime(_2, _1, SelectedDateTime.Value.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, SelectedDateTime.Value.Second);
                        DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case AyDatePickerSelectMode.DateTimeH:
                        SelectedDateTime = new DateTime(_2, _1, SelectedDateTime.Value.Day, SelectedDateTime.Value.Hour, 0, 0);
                        DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH时");
                        break;
                    case AyDatePickerSelectMode.DateTimeNoSecond:
                        SelectedDateTime = new DateTime(_2, _1, SelectedDateTime.Value.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, 0);
                        DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm");
                        break;
                    case AyDatePickerSelectMode.OnlySelectDate:
                        SelectedDateTime = new DateTime(_2, _1, SelectedDateTime.Value.Day);
                        DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd");
                        break;
                    case AyDatePickerSelectMode.YearMonth:
                        SelectedDateTime = new DateTime(_2, _1, 1);
                        DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM");
                        break;
                    case AyDatePickerSelectMode.HM:
                        SelectedDateTime = new DateTime(SelectedDateTime.Value.Year, SelectedDateTime.Value.Month, SelectedDateTime.Value.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, 0);
                        DateShowInfo.Text = SelectedDateTime.Value.ToString("HH:mm");
                        break;
                    case AyDatePickerSelectMode.HMS:
                        //SelectedDateTime = new DateTime(SelectedDateTime.Value.Year, SelectedDateTime.Value.Month, SelectedDateTime.Value.Day, SelectedDateTime.Value.Hour, SelectedDateTime.Value.Minute, SelectedDateTime.Value.Second);
                        DateShowInfo.Text = SelectedDateTime.Value.ToString("HH:mm:ss");
                        break;
                }
            }
            else
            {
                return;
            }

            SetList(SelectedDateTime.Value.Year, SelectedDateTime.Value.Month, SelectedDateTime.Value.Day);

            WhenDateTimePicked();

        }

        /// <summary>
        /// 当日期被选择时候触发
        /// 2017-2-13 13:34:03
        /// </summary>
        private void WhenDateTimePicked()
        {
            if (OnPicked.IsNotNull()) OnPicked(this, null);
            if (DateBoxInput.IsNotNull())
            {
                AyDatePickEventArgs pickargs = new AyDatePickEventArgs(SelectedDateTime);
                DateBoxInput.Text = SelectedDateTime.Value.
                        ToString(DateRuleObjects.dateFmt);
                //DateBoxInput.PickedDate = SelectedDateTime;
                //DateBoxInput.InvokeOnPicked(pickargs);
                if (SetDateBoxInputPickedDate())
                {
                    DateBoxInput.InvokeOnPicked(pickargs);
                }
            }
            CloseParentPopup();
        }
        //关闭父层的popup
        private void CloseParentPopup()
        {
            var _p = this.GetLogicalAncestor<Popup>();
            if (_p.IsNotNull())
            {
                _p.IsOpen = false;
                //if (DateBoxInput.IsNotNull())
                //{
                //    DateBoxInput.ValidateDateTimeIsUseful();
                //}

            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (OnCleared.IsNotNull()) OnCleared(this, null);
            if (DateBoxInput.IsNotNull())
            {
                DateBoxInput.Text = "";
                DateBoxInput.PickedDate = null;
                DateBoxInput.InvokeOnClear();
            }
            CloseParentPopup();
        }



        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            WhenDateTimePicked();
        }


    }


}
