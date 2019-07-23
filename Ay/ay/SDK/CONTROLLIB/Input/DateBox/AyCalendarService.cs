using ay.AyExpression;
using ay.contentcore;
using ay.contents;
using ay.Enums;
using ay.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ay.Controls.Services
{
    public class AyCalendarService
    {
        public AyCalendarService()
        {
        }
        public static AyCalendarFMT GetAyCalendarFMT(string dateFmt)
        {
            bool yy = dateFmt.IndexOf("y") > -1;
            bool MM = dateFmt.IndexOf("M") > -1;
            bool dd = dateFmt.IndexOf("d") > -1;
            bool hh = dateFmt.IndexOf("H") > -1;
            bool mm = dateFmt.IndexOf("m") > -1;
            bool ss = dateFmt.IndexOf("s") > -1;
            AyCalendarFMT df = AyCalendarFMT.None;
            if (yy && MM && !dd && !hh && !mm && !ss)
            {
                df = AyCalendarFMT.YearMonth;
            }
            else if (yy && MM && dd && !hh && !mm && !ss)
            {
                df = AyCalendarFMT.YearMonthDay;
            }
            else if (yy && MM && dd && hh && !mm && !ss)
            {
                df = AyCalendarFMT.YearMonthDayH;
            }
            else if (yy && MM && dd && hh && mm && !ss)
            {
                df = AyCalendarFMT.YearMonthDayHM;
            }
            else if (yy && MM && dd && hh && mm && ss)
            {
                df = AyCalendarFMT.YearMonthDayHMS;
            }
            else if (!yy && !MM && !dd && hh && mm && ss)
            {
                df = AyCalendarFMT.HMS;
            }
            else if (!yy && !MM && !dd && hh && mm && !ss)
            {
                df = AyCalendarFMT.HM;
            }
            return df;
        }
        static GregorianCalendar gc = new GregorianCalendar();
        public static int GetWeekOfYear(DateTime picktime, int firstDayOfWeek)
        {
            int weekOfYear = 0;
            if (firstDayOfWeek == 0)
            {
                weekOfYear = gc.GetWeekOfYear(picktime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            }
            else if (firstDayOfWeek == 1)
            {
                weekOfYear = gc.GetWeekOfYear(picktime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            }
            else if (firstDayOfWeek == 2)
            {
                weekOfYear = gc.GetWeekOfYear(picktime, CalendarWeekRule.FirstDay, DayOfWeek.Tuesday);
            }
            else if (firstDayOfWeek == 3)
            {
                weekOfYear = gc.GetWeekOfYear(picktime, CalendarWeekRule.FirstDay, DayOfWeek.Wednesday);
            }
            else if (firstDayOfWeek == 4)
            {
                weekOfYear = gc.GetWeekOfYear(picktime, CalendarWeekRule.FirstDay, DayOfWeek.Thursday);
            }
            else if (firstDayOfWeek == 5)
            {
                weekOfYear = gc.GetWeekOfYear(picktime, CalendarWeekRule.FirstDay, DayOfWeek.Friday);
            }
            else if (firstDayOfWeek == 6)
            {
                weekOfYear = gc.GetWeekOfYear(picktime, CalendarWeekRule.FirstDay, DayOfWeek.Saturday);
            }
            return weekOfYear;
        }
        /// <summary>
        /// 设置月可用性
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="month"></param>
        /// <param name="Year"></param>
        /// <param name="MinDateTime"></param>
        /// <param name="MaxDateTime"></param>
        public static void SetClickYearMonthButtonsEnabled(Button btn, DateTime dt, List<DateTime?> MinDateTime, List<DateTime?> MaxDateTime)
        {
            if (MinDateTime.IsNotNull() && MinDateTime.Count > 0)
            {
                foreach (var subtime in MinDateTime)
                {
                    if (subtime == null)
                    {
                        continue;
                    }
                    if ((dt.Year < subtime.Value.Year) || (dt.Year == subtime.Value.Year && dt.Month < subtime.Value.Month))
                    {
                        btn.IsEnabled = false;
                        break;
                    }
                }

            }


            if (MaxDateTime.IsNotNull() && MaxDateTime.Count > 0)
            {
                foreach (var subtime in MaxDateTime)
                {
                    if (subtime == null)
                    {
                        continue;
                    }
                    if ((dt.Year > subtime.Value.Year) || (dt.Year == subtime.Value.Year && dt.Month > subtime.Value.Month))
                    {
                        btn.IsEnabled = false;
                        break;
                    }
                }
            }
        }

        public static void SetClickHMSButtonsEnabled(Button btn, DateTime dt, List<DateTime?> MinDateTime, List<DateTime?> MaxDateTime)
        {
            if (MinDateTime.IsNotNull() && MinDateTime.Count > 0)
            {
                foreach (var subtime in MinDateTime)
                {
                    if (subtime == null)
                    {
                        continue;
                    }
                    if (dt < subtime)
                    {
                        btn.IsEnabled = false;
                        break;
                    }
                }
            }

            if (MaxDateTime.IsNotNull() && MaxDateTime.Count > 0)
            {
                foreach (var subtime in MaxDateTime)
                {
                    if (subtime == null)
                    {
                        continue;
                    }
                    if (dt > subtime)
                    {
                        btn.IsEnabled = false;
                        break;
                    }
                }
            }

        }


        public static void SetClickHourButtonsEnabled(Button btn, int hour, DateTime? SelectedDateTime, List<DateTime?> MinDateTime, List<DateTime?> MaxDateTime, List<string> DisabledDatesStrings, bool opposite)
        {
            if (SelectedDateTime.HasValue)
            {

                if (MinDateTime.IsNotNull() && MinDateTime.Count > 0)
                {
                    foreach (var subtime in MinDateTime)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectedDateTime.Value.Date == subtime.Value.Date)
                        {
                            if (hour < subtime.Value.Hour)
                            {
                                btn.IsEnabled = false;
                                break;
                            }
                        }
                    }
                }

                if (MaxDateTime.IsNotNull() && MaxDateTime.Count > 0)
                {
                    foreach (var subtime in MaxDateTime)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectedDateTime.Value.Date == subtime.Value.Date)
                        {
                            if (hour > subtime.Value.Hour)
                            {
                                btn.IsEnabled = false;
                                break;
                            }
                        }
                    }
                }

                if (DisabledDatesStrings.IsNotNull() && DisabledDatesStrings.Count > 0)
                {
                    foreach (var disabledDate in DisabledDatesStrings)
                    {
                        string _1 = SelectedDateTime.Value.ToString("yyyy-MM-dd A:mm:ss");
                        _1 = _1.Replace("A", hour.ToString().PadLeft(2, '0'));
                        bool vaResult = System.Text.RegularExpressions.Regex.IsMatch(_1, disabledDate);

                        if (vaResult)
                        {
                            btn.IsEnabled = false;
                            break;
                        }
                    }
                    if (opposite)
                    {
                        btn.IsEnabled = !btn.IsEnabled;
                    }
                }
            }

        }

        public static void SetClickMinuteButtonsEnabled(Button btn, int minute, DateTime? SelectedDateTime, List<DateTime?> MinDateTime, List<DateTime?> MaxDateTime, List<string> DisabledDatesStrings, bool opposite)
        {
            if (SelectedDateTime.HasValue)
            {
                if (MinDateTime.IsNotNull() && MinDateTime.Count > 0)
                {
                    foreach (var subtime in MinDateTime)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectedDateTime.Value.Date == subtime.Value.Date && SelectedDateTime.Value.Hour == subtime.Value.Hour)
                        {
                            if (minute < subtime.Value.Minute)
                            {
                                btn.IsEnabled = false;
                                break;
                            }
                        }
                    }
                }

                if (MaxDateTime.IsNotNull() && MaxDateTime.Count > 0)
                {
                    foreach (var subtime in MaxDateTime)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectedDateTime.Value.Date == subtime.Value.Date &&
                   SelectedDateTime.Value.Hour == subtime.Value.Hour)
                        {
                            if (minute > subtime.Value.Minute)
                            {
                                btn.IsEnabled = false;
                                break;
                            }
                        }

                    }
                }
                if (DisabledDatesStrings.IsNotNull() && DisabledDatesStrings.Count > 0)
                {
                    foreach (var disabledDate in DisabledDatesStrings)
                    {
                        string _1 = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:A:ss");
                        _1 = _1.Replace("A", minute.ToString().PadLeft(2, '0'));
                        bool vaResult = System.Text.RegularExpressions.Regex.IsMatch(_1, disabledDate);
                        if (vaResult)
                        {
                            btn.IsEnabled = false;
                            break;
                        }


                    }
                    if (opposite)
                    {
                        btn.IsEnabled = !btn.IsEnabled;
                    }
                }
            }

        }

        public static void SetClickSecondButtonsEnabled(Button btn, int second, DateTime? SelectedDateTime, List<DateTime?> MinDateTime, List<DateTime?> MaxDateTime, List<string> DisabledDatesStrings, bool opposite)
        {
            if (SelectedDateTime.HasValue)
            {
                if (MinDateTime.IsNotNull() && MinDateTime.Count > 0)
                {
                    foreach (var subtime in MinDateTime)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectedDateTime.Value.Date == subtime.Value.Date &&
                SelectedDateTime.Value.Hour == subtime.Value.Hour &&
                SelectedDateTime.Value.Minute == subtime.Value.Minute)
                        {
                            if (second < subtime.Value.Second)
                            {
                                btn.IsEnabled = false;
                                break;
                            }
                        }


                    }
                }

                if (MaxDateTime.IsNotNull() && MaxDateTime.Count > 0)
                {
                    foreach (var subtime in MaxDateTime)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if (SelectedDateTime.Value.Date == subtime.Value.Date &&
                            SelectedDateTime.Value.Hour == subtime.Value.Hour &&
                            SelectedDateTime.Value.Minute == subtime.Value.Minute
                            )
                        {
                            if (second > subtime.Value.Second)
                            {
                                btn.IsEnabled = false;
                                break;
                            }
                        }
                    }
                }
                if (DisabledDatesStrings.IsNotNull() && DisabledDatesStrings.Count > 0)
                {
                    foreach (var disabledDate in DisabledDatesStrings)
                    {
                        string _1 = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm:A");
                        _1 = _1.Replace("A", second.ToString().PadLeft(2, '0'));
                        bool vaResult = System.Text.RegularExpressions.Regex.IsMatch(_1, disabledDate);
                        if (vaResult)
                        {
                            btn.IsEnabled = false;
                            break;
                        }


                    }
                    if (opposite)
                    {
                        btn.IsEnabled = !btn.IsEnabled;
                    }
                }
            }
        }

        /// <summary>
        /// 设置年可用性
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="Year"></param>
        /// <param name="MinDateTime"></param>
        /// <param name="MaxDateTime"></param>
        public static void SetClickYearButtonsEnabled(Button btn, int Year, List<DateTime?> MinDateTime, List<DateTime?> MaxDateTime, List<string> DisabledDatesStrings, bool opposite, DateTime? SelectedDateTime)
        {
            if (MinDateTime.IsNotNull() && MinDateTime.Count > 0)
            {
                foreach (var subtime in MinDateTime)
                {
                    if (subtime == null)
                    {
                        continue;
                    }

                    if (Year < subtime.Value.Year)
                    {
                        btn.IsEnabled = false;
                        break;
                    }
                }
            }
            if (MaxDateTime.IsNotNull() && MaxDateTime.Count > 0)
            {
                foreach (var subtime in MaxDateTime)
                {
                    if (subtime == null)
                    {
                        continue;
                    }
                    if (Year > subtime.Value.Year)
                    {
                        btn.IsEnabled = false;
                        break;
                    }
                }
            }
            if (DisabledDatesStrings.IsNotNull() && DisabledDatesStrings.Count > 0)
            {

                foreach (var disabledDate in DisabledDatesStrings)
                {
                    string _1 = SelectedDateTime.Value.ToString("A-MM-dd");
                    _1 = _1.Replace("A", Year.ToString().PadLeft(2, '0'));
                    bool vaResult = System.Text.RegularExpressions.Regex.IsMatch(_1, disabledDate.Split(' ')[0]);
                    if (vaResult)
                    {
                        btn.IsEnabled = false;
                        break;
                    }

                }
                if (opposite)
                {
                    btn.IsEnabled = !btn.IsEnabled;
                }
            }

        }

        public static void SetClickMonthButtonsEnabled(Button btn, int month, int? Year, List<DateTime?> MinDateTime, List<DateTime?> MaxDateTime, List<string> DisabledDatesStrings, bool opposite, DateTime? SelectedDateTime)
        {
            if (Year.HasValue) //年份有值，肯定是 要控制可用性了。
            {
                if (MinDateTime.IsNotNull() && MinDateTime.Count > 0)
                {
                    foreach (var subtime in MinDateTime)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if ((Year <= subtime.Value.Year) && (month < subtime.Value.Month))
                        {
                            btn.IsEnabled = false;
                            break;
                        }
                    }
                }

                if (MaxDateTime.IsNotNull() && MaxDateTime.Count > 0)
                {
                    foreach (var subtime in MaxDateTime)
                    {
                        if (subtime == null)
                        {
                            continue;
                        }
                        if ((Year >= subtime.Value.Year) && (month > subtime.Value.Month))
                        {
                            btn.IsEnabled = false;
                            break;
                        }
                    }
                }

                if (DisabledDatesStrings.IsNotNull() && DisabledDatesStrings.Count > 0)
                {
                    foreach (var disabledDate in DisabledDatesStrings)
                    {
                        string _1 = SelectedDateTime.Value.ToString("yyyy-A-dd");
                        _1 = _1.Replace("A", month.ToString().PadLeft(2, '0'));
                        bool vaResult = System.Text.RegularExpressions.Regex.IsMatch(_1, disabledDate.Split(' ')[0]);
                        if (vaResult)
                        {
                            btn.IsEnabled = false;
                            break;
                        }

                    }
                    if (opposite)
                    {
                        btn.IsEnabled = !btn.IsEnabled;
                    }
                }

                //TODON   AY暂时不会处理，太难了。你能你上,比如2月都不能选
                //var _validateItemResult = AyCalendarService.ValidateRegexDate(item.Date, DisabledDates, DisabledDatesStrings);
                //if (!_validateItemResult)
                //{
                //    btn.IsEnabled = false;
                //}

                //if (MaxDateTime.HasValue)
                //{
                //    if ((Year >= MaxDateTime.Value.Year) && (month > MaxDateTime.Value.Month))
                //    {
                //        btn.IsEnabled = false;
                //    }
                //}
            }
         
        }

        private static string _WRONGTIP;
        /// <summary>
        /// 日期宽度
        /// </summary>
        public static string WRONGTIP
        {
            get
            {
                if (_WRONGTIP == null)
                {

                    _WRONGTIP = Langs.ay_ErrorDateTime.Lang();
                }
                return _WRONGTIP;
            }
        }


        [Pure]
        public static Grid CreateWeekHeadLabel2(string cnt)
        {
            Grid g = new Grid();
            g.Height = 40;
            g.Width = UIGeneric.DayWidth.Value;

            AyText label = new AyText();
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Text = cnt;
            label.SetResourceReference(AyText.ForegroundProperty, "colorsuccess");
            g.Children.Add(label);
            return g;
        }
        [Pure]
        public static AyText CreateWeekHeadLabel(string cnt)
        {
            AyText label = new AyText();
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.TextAlignment = TextAlignment.Center;
            label.Width = UIGeneric.DayWidth.Value;
            label.Text = cnt;
            return label;
        }
        [Pure]
        internal static Tuple<List<DateTime?>, List<DateTime?>> FilterDatePickerItem(AyDateRuleJsonToObjects DateRuleObjects, AyDateBoxCalendar MinDateReferToElement, AyDateBoxCalendar MaxDateReferToElement)
        {
            List<DateTime?> MinDateCopy = new List<DateTime?>();
            List<DateTime?> MaxDateCopy = new List<DateTime?>();

            if (DateRuleObjects.IsNotNull())
            {
                //AyCalendarFMT _fmt = GetAyCalendarFMT(DateRuleObjects.dateFmt);
                if (!DateRuleObjects.minDate.IsNullAndTrimAndEmpty())
                {
                    //是否含有#F{ }
                    if (DateRuleObjects.minDate.IndexOf(@"#F{") == 0)
                    {
                        if (DateRuleObjects.minDate.IndexOf("ay") > -1) //使用了绑定
                        {
                            if (MinDateReferToElement.IsNotNull())
                            {
                                string _text = MinDateReferToElement.Text;
                                string _dateRule = MinDateReferToElement.DateRule;
                                var ddo = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(_dateRule);

                                if (ddo.IsNotNull())
                                {
                                    MinDateCopy = AyDateStrictExpression.ConvertDDVF(_text, DateRuleObjects.minDate, ddo.dateFmt);
                                }
                                else
                                {
                                    MinDateCopy = AyDateStrictExpression.ConvertDDVF(_text, DateRuleObjects.minDate, "yyyy-MM-dd");
                                }
                            }
                        }
                        else
                        {
                            MinDateCopy = AyDateStrictExpression.ConvertDDVF(null, DateRuleObjects.minDate, null);
                        }

                    }
                    else
                    {
                        MinDateCopy.Add(AyDateStrictExpression.Convert(DateRuleObjects.minDate));
                    }
                }
                if (!DateRuleObjects.maxDate.IsNullAndTrimAndEmpty())
                {
                    //是否含有#F{ }
                    if (DateRuleObjects.maxDate.IndexOf(@"#F{") == 0)
                    {
                        if (DateRuleObjects.maxDate.IndexOf("ay") > -1) //使用了绑定
                        {
                            if (MaxDateReferToElement.IsNotNull())
                            {
                                string _text = MaxDateReferToElement.Text;
                                string _dateRule = MaxDateReferToElement.DateRule;
                                var ddo = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(_dateRule);

                                if (ddo.IsNotNull())
                                {
                                    MaxDateCopy = AyDateStrictExpression.ConvertDDVF(_text, DateRuleObjects.maxDate, ddo.dateFmt);
                                }
                                else
                                {
                                    MaxDateCopy = AyDateStrictExpression.ConvertDDVF(_text, DateRuleObjects.maxDate, "yyyy-MM-dd");
                                }
                            }
                        }
                        else
                        {
                            MaxDateCopy = AyDateStrictExpression.ConvertDDVF(null, DateRuleObjects.maxDate, null);
                        }

                    }
                    else
                    {
                        MaxDateCopy.Add(AyDateStrictExpression.Convert(DateRuleObjects.maxDate));
                    }
                }

            }
            return Tuple.Create<List<DateTime?>, List<DateTime?>>(MinDateCopy, MaxDateCopy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="expressionStringLists"></param>
        /// <param name="opposite">opposite如果是true，就取反</param>
        /// <returns></returns>
        public static bool ValidateRegexDate(DateTime dt, List<string> expressionStringLists, bool opposite)
        {
            bool result = true;
            //if (expressionLists.IsNotNull() && expressionLists.Count > 0)
            //{
            //    foreach (var disabledDate in expressionLists)
            //    {
            //        if (!disabledDate.Value.HasValue) continue;
            //        if (dt.Date == disabledDate.Value.Value.Date)
            //        {
            //            result = false;
            //            break;
            //        }
            //    }
            //}
            //if (result)
            //{
            if (expressionStringLists.IsNotNull() && expressionStringLists.Count > 0)
            {
                foreach (var disabledDate in expressionStringLists)
                {
                    bool vaResult = false;
                    if (disabledDate.IndexOf(":") > -1)
                    {
                        string[] _01 = dt.ToString("yyyy-MM-dd HH:mm:ss").Split(' ');
                        string[] _02 = disabledDate.Split(' ');
                        vaResult = System.Text.RegularExpressions.Regex.IsMatch(_01[0] + " " + _02[1], disabledDate);
                    }
                    else
                    {
                        vaResult = System.Text.RegularExpressions.Regex.IsMatch(dt.ToString("yyyy-MM-dd"), disabledDate);
                    }
                    if (opposite)
                    {
                        vaResult = !vaResult;
                    }

                    if (vaResult)
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        public static bool ValidateRegexDateByAyCalendar(DateTime dt, List<string> expressionStringLists, bool opposite)
        {
            bool result = true;
            if (expressionStringLists.IsNotNull() && expressionStringLists.Count > 0)
            {
                foreach (var disabledDate in expressionStringLists)
                {
                    bool vaResult = false;
                    if (disabledDate.IndexOf(":") > -1)
                    {
                        vaResult = System.Text.RegularExpressions.Regex.IsMatch(dt.ToString("yyyy-MM-dd HH:mm:ss"), disabledDate);
                    }
                    else
                    {
                        vaResult = System.Text.RegularExpressions.Regex.IsMatch(dt.ToString("yyyy-MM-dd"), disabledDate);
                    }

                    if (vaResult)
                    {
                        result = false;
                        break;
                    }
                }
            }
            if (opposite)
            {
                result = !result;
            }
            return result;
        }
        public static bool hasTeShu(string express)
        {
            if (express.IndexOf("%y") > -1 || express.IndexOf("%M") > -1 || express.IndexOf("%d") > -1
                || express.IndexOf("%H") > -1 || express.IndexOf("%m") > -1 || express.IndexOf("%s") > -1
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
