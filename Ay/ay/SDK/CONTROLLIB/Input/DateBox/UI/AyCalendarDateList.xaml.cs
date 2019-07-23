using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ay.AyExpression;
using ay.Controls.Args;
using ay.Controls.Info;
using ay.Controls.Services;
using ay.Date.Info;

namespace ay.Controls
{
    /// <summary>
    /// AyCalendarDateList.xaml 的交互逻辑
    /// 生日：2017-2-21 15:51:08
    /// 用于剥离 日期中肚子部分
    /// </summary>
    public partial class AyCalendarDateList : Grid
    {
        public List<DateTime?> MinDateCopy;
        public List<DateTime?> MaxDateCopy;
        public AyDateBoxCalendar MinDateReferToElement;
        public AyDateBoxCalendar MaxDateReferToElement;

        public List<string> DisabledDatesStrings;

        public bool firstInitMinMax = true;
        internal void FilterDatePickerItem(AyDatePickerItem item)
        {
            if (firstInitMinMax)
            {
                var _1 = AyCalendarService.FilterDatePickerItem(DateRuleObjects, MinDateReferToElement, MaxDateReferToElement);
                MinDateCopy = _1.Item1;
                MaxDateCopy = _1.Item2;
                firstInitMinMax = false;
            }
            if (item.IsNotNull())
            {
                if (MinDateCopy.Count > 0)
                {
                    foreach (var subitem in MinDateCopy)
                    {
                        if (subitem.HasValue && item.Date < subitem.Value.Date)
                        {
                            item.IsEnabled = false;
                            break;
                        }
                    }
                }
                if (MaxDateCopy.Count > 0)
                {
                    foreach (var subitem in MaxDateCopy)
                    {
                        if (subitem.HasValue && item.Date > subitem.Value.Date)
                        {
                            item.IsEnabled = false;
                            break;
                        }
                    }
                }

                //周几限制
                if (DateRuleObjects.IsNotNull())
                {
                    if (DateRuleObjects.disabledDays.IsNotNull() && DateRuleObjects.disabledDays.Count > 0)
                    {
                        var _1 = item.Date.DayOfWeek.GetHashCode();
                        foreach (var disabledDay in DateRuleObjects.disabledDays)
                        {
                            if (_1 == disabledDay)
                            {
                                item.IsEnabled = false;
                                break;
                            }
                        }
                    }

                    //限制disabledDate
                    var _validateItemResult = AyCalendarService.ValidateRegexDate(item.Date, this.DisabledDatesStrings, DateRuleObjects.opposite);
                    if (!_validateItemResult)
                    {
                        item.IsEnabled = false;
                    }


                    //高亮日和高亮日期
                    if (DateRuleObjects.specialDays.IsNotNull() && DateRuleObjects.specialDays.Count > 0)
                    {
                        var _1 = item.Date.DayOfWeek.GetHashCode();
                        foreach (var specialDay in DateRuleObjects.specialDays)
                        {
                            if (_1 == specialDay)
                            {
                                item.IsHighlight = true;
                                break;
                            }
                        }
                    }

                    if (SpecialDatesStrings.IsNotNull() && SpecialDatesStrings.Count > 0)
                    {
                        foreach (var SpecialDates in SpecialDatesStrings)
                        {
                            bool vaResult = false;
                            if (SpecialDates.IndexOf(":") > -1)
                            {
                                string[] _01 = item.Date.ToString("yyyy-MM-dd HH:mm:ss").Split(' ');
                                string[] _02 = SpecialDates.Split(' ');
                                vaResult = System.Text.RegularExpressions.Regex.IsMatch(_01[0] + " " + _02[1], SpecialDates);
                            }
                            else
                            {
                                vaResult = System.Text.RegularExpressions.Regex.IsMatch(item.Date.ToString("yyyy-MM-dd"), SpecialDates);
                            }
                            if (vaResult)
                            {
                                item.IsHighlight = true;
                                break;
                            }
                        }
                    }

                }
            }
        }
        public List<string> SpecialDatesStrings;

        public AyCalendarDateList()
        {
            InitializeComponent();
            Loaded += AyCalendarDateList_Loaded;
        }

        private void AyCalendarDateList_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyCalendarDateList_Loaded;

            DateList.SetBinding(ListBox.ItemsSourceProperty, new Binding { Source = items });
        }



        public ObservableCollection<AyDatePickerItem> items = new ObservableCollection<AyDatePickerItem>();

        ///每次赋值，由日历AyCalendar那方赋值
        public AyDateRuleJsonToObjects DateRuleObjects { get; set; }
        public int currentYear = 0;
        public int currentMonth = 0;
        public int currentDay = 0;
        public void SetList(int year, int month, int SelectedDay, bool Select = true)
        {
            items.Clear();
            sp_weekhead.Children.Clear();

            currentYear = year;
            currentMonth = month;
            currentDay = SelectedDay;
            DateTime now = DateTime.Now;
            int IndexOfFistDay = 1;
            if (DateRuleObjects == null) return;
            if (DateRuleObjects.firstDayOfWeek > -1 && DateRuleObjects.firstDayOfWeek < 7)
            {
                IndexOfFistDay = AyDatePickerHelper.GetDayOfWeek(year, month, AyFirstOfWeekDictionary.FirstDayDistanceDays[DateRuleObjects.firstDayOfWeek]);

                for (int i = DateRuleObjects.firstDayOfWeek; i < 7; i++)
                {
                    sp_weekhead.Children.Add(AyCalendarService.CreateWeekHeadLabel(AyFirstOfWeekDictionary.FirstDayDistanceDaysText[i]));
                }
                for (int i = 0; i < DateRuleObjects.firstDayOfWeek; i++)
                {
                    sp_weekhead.Children.Add(AyCalendarService.CreateWeekHeadLabel(AyFirstOfWeekDictionary.FirstDayDistanceDaysText[i]));
                }


            }
            else
            {
                IndexOfFistDay = AyDatePickerHelper.GetDayOfWeek(year, month, 1);

                for (int i = 0; i < 7; i++)
                {
                    sp_weekhead.Children.Add(AyCalendarService.CreateWeekHeadLabel(AyFirstOfWeekDictionary.FirstDayDistanceDaysText[i]));
                }

            }


            DateTime weekStart = new DateTime(year, month, 1);
            DateTime nextMonthStart = weekStart.AddMonths(1);
            //6 代表 这个月1号是星期六 0-6
            int day = 1;
            int nextMonth = 0;
            int monDay = AyDatePickerHelper.NumOfDays(year, month);
            for (int i = 0; i < 42; i++)
            {
                AyDatePickerItem item = new AyDatePickerItem();
                if (i >= IndexOfFistDay && day <= monDay)
                {
                    //var nongli = AyDatePickerHelper.GetNongLi(year, month, day);
                    //item.NameOfDay = nongli.Day;
                    //item.MonthAndDay = nongli.MonthAndDay;
                    item.NumberOfDay = day.ToString();
                    item.Date = new DateTime(year, month, day);
                    if (item.Date.Year > YearStrick.MAXYEAR || item.Date.Year < YearStrick.MINYEAR)
                    {
                        item.IsEnabled = false;
                    }
                    //item.Week_S = item.Date.ToString("dddd", AyDatePickerHelper.culture);
                    //item.ShengXiao = nongli.ShengXiao;
                    //item.GanZhi = nongli.TianGan + nongli.DiGan;
                    //item.TianGan = nongli.TianGan;
                    //item.TianGan = nongli.DiGan;
                    //item.JieQi = nongli.JieQi;
                    //item.GongHoliday = nongli.GongHoliday;
                    //item.NongHoliday = nongli.NongHoliday;
                    //item.Title = nongli.Title;
                    //item.TitleType = nongli.TitleType;

                    //item.TipShow = item.Date.ToString("yyyy年MM月dd日 ") + item.Week_S
                    //   + Environment.NewLine + "[" + item.GanZhi + item.ShengXiao + "年]" + "农历" + item.MonthAndDay;
                    //item.TipShow = item.Date.ToString("yyyy年MM月dd日 ") + item.Week_S;
                    item.IsLastOrNextDateItem = false;

                    //TODO 最大最小值判断
                    FilterDatePickerItem(item);

                    if (item.Date.Date == now.Date)
                    {
                        item.IsToday = true;
                    }

                    if (SelectedDay == day && Select)
                    {
                        item.IsSelected = true;
                    }
                    //是不是周末
                    if (DateRuleObjects.highLineWeekDay)
                    {
                        var t = item.Date.DayOfWeek;
                        if (t == DayOfWeek.Saturday || t == DayOfWeek.Sunday)
                        {
                            item.IsWeekDay = true;
                        }
                    }
                    day++;
                    items.Add(item);
                }
                else if (i < IndexOfFistDay)
                {//前置补空
                    int ad = i - IndexOfFistDay;
                    item.Date = weekStart.AddDays(ad);
                    if (item.Date.Year > YearStrick.MAXYEAR || item.Date.Year < YearStrick.MINYEAR)
                    {
                        item.IsEnabled = false;
                    }
                    //var nongli = AyDatePickerHelper.GetNongLi(item.Date.Year, item.Date.Month, item.Date.Day);
                    //item.NameOfDay = nongli.Day;
                    //item.MonthAndDay = nongli.MonthAndDay;
                    item.NumberOfDay = item.Date.Day.ToString();

                    //item.Week_S = item.Date.ToString("dddd", AyDatePickerHelper.culture);
                    //item.ShengXiao = nongli.ShengXiao;
                    //item.GanZhi = nongli.TianGan + nongli.DiGan;
                    //item.TianGan = nongli.TianGan;
                    //item.TianGan = nongli.DiGan;
                    //item.JieQi = nongli.JieQi;
                    //item.GongHoliday = nongli.GongHoliday;
                    //item.NongHoliday = nongli.NongHoliday;
                    //item.Title = nongli.Title;
                    //item.TitleType = nongli.TitleType;
                    //item.TipShow = item.Date.ToString("yyyy年MM月dd日 ") + item.Week_S;


                    item.IsLastOrNextDateItem = true;

                    //TODO 最大最小值判断
                    FilterDatePickerItem(item);


                    if (item.Date.Date == now.Date)
                    {
                        item.IsToday = true;
                    }

                    //是不是周末
                    if (DateRuleObjects.highLineWeekDay)
                    {
                        var t = item.Date.DayOfWeek;
                        if (t == DayOfWeek.Saturday || t == DayOfWeek.Sunday)
                        {
                            item.IsWeekDay = true;
                        }
                    }
                    items.Add(item);
                }
                else
                { //后置补空
                    item.Date = nextMonthStart.AddDays(nextMonth);
                    if (item.Date.Year > YearStrick.MAXYEAR || item.Date.Year < YearStrick.MINYEAR)
                    {
                        item.IsEnabled = false;
                    }
                    //var nongli = AyDatePickerHelper.GetNongLi(item.Date.Year, item.Date.Month, item.Date.Day);
                    //item.NameOfDay = nongli.Day;
                    //item.MonthAndDay = nongli.MonthAndDay;
                    item.NumberOfDay = item.Date.Day.ToString();

                    //item.Week_S = item.Date.ToString("dddd", AyDatePickerHelper.culture);
                    //item.ShengXiao = nongli.ShengXiao;
                    //item.GanZhi = nongli.TianGan + nongli.DiGan;
                    //item.TianGan = nongli.TianGan;
                    //item.TianGan = nongli.DiGan;
                    //item.JieQi = nongli.JieQi;
                    //item.GongHoliday = nongli.GongHoliday;
                    //item.NongHoliday = nongli.NongHoliday;
                    //item.Title = nongli.Title;
                    //item.TitleType = nongli.TitleType;
                    //item.TipShow = item.Date.ToString("yyyy年MM月dd日 ") + item.Week_S;
                    item.IsLastOrNextDateItem = true;
                    //TODO 最大最小值判断
                    FilterDatePickerItem(item);

                    if (item.Date.Date == now.Date)
                    {
                        item.IsToday = true;
                    }

                    //是不是周末
                    if (DateRuleObjects.highLineWeekDay)
                    {
                        var t = item.Date.DayOfWeek;
                        if (t == DayOfWeek.Saturday || t == DayOfWeek.Sunday)
                        {
                            item.IsWeekDay = true;
                        }
                    }

                    items.Add(item);
                    nextMonth++;
                }

            }

            //设置周
            if (DateRuleObjects.isShowWeek && items.Count > 0)
            {
                bdWeekHead.Visibility = Visibility.Visible;
                //计算周
                sp_WeekNo.Children.Clear();
                var _1 = AyCalendarService.GetWeekOfYear(items[0].Date, DateRuleObjects.firstDayOfWeek);

                sp_WeekNo.Children.Add(AyCalendarService.CreateWeekHeadLabel2((_1++).ToString()));
                sp_WeekNo.Children.Add(AyCalendarService.CreateWeekHeadLabel2((_1++).ToString()));
                sp_WeekNo.Children.Add(AyCalendarService.CreateWeekHeadLabel2((_1++).ToString()));
                sp_WeekNo.Children.Add(AyCalendarService.CreateWeekHeadLabel2((_1++).ToString()));
                sp_WeekNo.Children.Add(AyCalendarService.CreateWeekHeadLabel2((_1++).ToString()));
                sp_WeekNo.Children.Add(AyCalendarService.CreateWeekHeadLabel2((_1++).ToString()));
            }
            else
            {
                bdWeekHead.Visibility = Visibility.Collapsed;
                sp_WeekNo.Children.Clear();
            }

        }

        public event EventHandler<AyDateListItemClickEventArgs> OnClickItem;

        private void bg_TouchDown(object sender, TouchEventArgs e)
        {
            WhenClickDay(sender);
        }

        private void bg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WhenClickDay(sender);
        }
        public void WhenClickDay(object sender)
        {
            AyDatePickerItem _1 = ((Border)sender).Tag as AyDatePickerItem;
            AyDateListItemClickEventArgs ad = new AyDateListItemClickEventArgs(_1);
            if (OnClickItem.IsNotNull())
            {
                OnClickItem(this, ad);
            }

        }

    }
}
