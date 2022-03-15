using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ay.contentcore;
using ay.Enums;
using ay.Controls.Services;
using ay.AyExpression;

using ay.Controls.Args;
using ay.Controls.Info;
using ay.SDK.CONTROLLIB.Primitive;

namespace ay.Controls
{

    public class AyDateBoxCalendar : AyFormInput
    {
        public AyDateBoxCalendar()
        {
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            Loaded += AyDateBoxCalendar_Loaded;

        }
        public override bool Validate()
        {
            ValdiateTextBox(this);
            if (!IsError)
            {
                ValidateDateTimeIsUseful();
            }

            return !this.IsError;
        }
        public override void UpdateTextWhenDateChange()
        {
            //数据第一次赋值的时候一定要验证一下
            //ValidateDateTimeIsUsefulLoadedUse();
            if (Text.IsNullAndTrimAndEmpty() && PickedDate.HasValue)
            {
                if (DateRuleObjects.IsNull())
                {
                    DateRuleObjects = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(DateRule);
                }
                string t = PickedDate.Value.ToString(DateRuleObjects.dateFmt);
                if (t.Contains("0001") || t.Contains("9999"))
                {
                    t = "";
                }
                else
                {
                    Text = t;
                }
            }
        }
        /// <summary>
        /// 关闭弹窗
        /// </summary>
        public void ClosePopup()
        {
            if (PopupContent != null)
            {
                PopupContent.IsOpen = false;
            }
        }


        #region 拓展事件
        public event EventHandler<EventArgs> OnCleared;

        public void InvokeOnClear()
        {
            if (OnCleared.IsNotNull())
            {
                OnCleared(this, null);
            }
            Week = string.Empty;
        }
        /// <summary>
        /// 当day被单击的时候触发
        /// </summary>
        public event EventHandler<AyDatePickEventArgs> OnPicked;
        public void InvokeOnPicked(AyDatePickEventArgs args)
        {
            if (OnPicked.IsNotNull())
            {
                OnPicked(this, args);
            }
            Week = AyCalendarService.GetWeekOfYear(PickedDate.Value, DateRuleObjects.firstDayOfWeek).ToObjectString();
        }
        #endregion

        private void AyDateBoxCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyDateBoxCalendar_Loaded;

            this.KeyDown += AyDateBox_KeyDown;
            //this.Cursor = Cursors.Hand;
            this.GotKeyboardFocus += AyDateBoxCalendar_GotKeyboardFocus;

            this.IsVisibleChanged += (ss, ee) =>
            {
                if ((bool)ee.NewValue)
                {

                }
                else
                {
                    ClosePopup();
                }
            };
            var _2 = this.GetVisualAncestor<Page>();
            if (_2.IsNotNull())
            {
                _2.MouseLeftButtonDown += _2_MouseLeftButtonDown;

            }
            else
            {
                var _3 = this.GetVisualAncestor<Window>();
                if (_2.IsNotNull())
                {
                    _3.MouseLeftButtonDown += _2_MouseLeftButtonDown;
                }
            }
            UpdateTextWhenDateChange();
            if (!Icon.IsNullAndTrimAndEmpty())
            {
                Border b = new Border();
                b.IsHitTestVisible = false;
                b.Background = new SolidColorBrush(Colors.Transparent);
                b.Width = 30;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                AyIconAll btn = new AyIconAll();
                Binding binding = new Binding { Path = new PropertyPath("Icon"), Source = this, Mode = BindingMode.TwoWay };
                //btn.Icon = "more_ay_pickmonthicon";
                btn.SetBinding(AyIconAll.IconProperty, binding);
                btn.HorizontalAlignment = HorizontalAlignment.Right;
                btn.VerticalAlignment = VerticalAlignment.Center;
                btn.SetResourceReference(AyIconAll.ForegroundProperty, "colortextplaceholder");
                btn.Width = 20;
                btn.Margin = new Thickness(0, 0, 10, 0);
                btn.Stretch = Stretch.Uniform;
                btn.FontSize = 20;

                b.Child = btn;
                this.RightContent = b;

            }





        }

        private void _2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }

        private void AyDateBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (PopupContent.IsNotNull())
                {
                    WhenBoxLostFocus();
                }
            }
        }

        private void AyDateBoxCalendar_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            WhenBoxGotFocus();

        }

        private void WhenBoxGotFocus()
        {
            PopupContent.IsOpen = true;
        }
        private void WhenBoxLostFocus()
        {
            if (PopupContent != null)
            {
                PopupContent.IsOpen = false;
            }
        }


        /// <summary>
        /// 日期图标，如果为空或者null，则不显示图标
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }


        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(AyDateBoxCalendar), new PropertyMetadata("more_ay_pickmonthicon"));



        /// <summary>
        /// 当前选中的时间的周
        /// 2017-2-20 15:21:08
        /// </summary>
        public string Week
        {
            get { return (string)GetValue(WeekProperty); }
            set { SetValue(WeekProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Week.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WeekProperty =
            DependencyProperty.Register("Week", typeof(string), typeof(AyDateBoxCalendar), new PropertyMetadata(""));

        public void ValidateDateTimeIsUseful()
        {
            if (!Text.IsNullOrWhiteSpace() && PopupContent != null && PopupContent.IsOpen == false)
            {
                UseValidate();
            }

        }



        //internal void ValidateDateTimeIsUsefulLoadedUse()
        //{
        //    if (Text.IsNullAndTrimAndEmpty() && PickedDate.HasValue)
        //    {
        //        if (DateRuleObjects.IsNull())
        //        {
        //            DateRuleObjects = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(DateRule);
        //        }
        //        string t = PickedDate.Value.ToString(DateRuleObjects.dateFmt);
        //        if (t.Contains("0001") || t.Contains("9999"))
        //        {
        //            t = "";
        //        }
        //        else
        //        {
        //            Text = t;
        //        }
        //    }
        //    if (!Text.IsNullAndTrimAndEmpty())
        //    {
        //        UseValidate();
        //    }
        //}
        List<string> DisabledDatesStrings = null;
        #region 绑定的max和min的控件

        /// <summary>
        /// 参考的最小日期的 日期控件
        /// </summary>
        public AyDateBoxCalendar MinDateReferToElement
        {
            get { return (AyDateBoxCalendar)GetValue(MinDateReferToElementProperty); }
            set { SetValue(MinDateReferToElementProperty, value); }
        }

        public static readonly DependencyProperty MinDateReferToElementProperty =
            DependencyProperty.Register("MinDateReferToElement", typeof(AyDateBoxCalendar), typeof(AyDateBoxCalendar), new PropertyMetadata(null));


        /// <summary>
        /// 参考的最大日期的 日期控件
        /// </summary>
        public AyDateBoxCalendar MaxDateReferToElement
        {
            get { return (AyDateBoxCalendar)GetValue(MaxDateReferToElementProperty); }
            set { SetValue(MaxDateReferToElementProperty, value); }
        }

        public static readonly DependencyProperty MaxDateReferToElementProperty =
            DependencyProperty.Register("MaxDateReferToElement", typeof(AyDateBoxCalendar), typeof(AyDateBoxCalendar), new PropertyMetadata(null));




        #endregion
        /// <summary>
        /// 验证时间是否在有效值的范围内
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool DateItemIsInValidRange(DateTime item)
        {
            bool returnResult = true;
            if (DateRuleObjects.IsNotNull())
            {
                if (!DateRuleObjects.minDate.IsNullAndTrimAndEmpty())
                {
                    //是否含有#F{ }
                    if (DateRuleObjects.minDate.IndexOf(@"#F{") == 0)
                    {
                        List<DateTime?> _4 = null;
                        if (DateRuleObjects.minDate.IndexOf("ay") > -1) //使用了绑定
                        {
                            if (MinDateReferToElement.IsNotNull())
                            {
                                string _text = MinDateReferToElement.Text;
                                string _dateRule = MinDateReferToElement.DateRule;
                                var ddo = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(_dateRule);

                                if (ddo.IsNotNull())
                                {
                                    _4 = AyDateStrictExpression.ConvertDDVF(_text, DateRuleObjects.minDate, ddo.dateFmt);
                                }
                                else
                                {
                                    _4 = AyDateStrictExpression.ConvertDDVF(_text, DateRuleObjects.minDate, "yyyy-MM-dd");
                                }
                            }
                        }
                        else
                        {
                            _4 = AyDateStrictExpression.ConvertDDVF(null, DateRuleObjects.minDate, null);
                        }
                        bool ret = true;

                        foreach (var subtime in _4)
                        {
                            if (subtime.IsNotNull() && item < subtime.Value)
                            {
                                ret = false;
                                break;
                            }
                        }

                        return ret;
                    }
                    else
                    {
                        //单控制
                        DateTime MinDateCopy = AyDateStrictExpression.Convert(DateRuleObjects.minDate);
                        if (item < MinDateCopy)
                        {
                            returnResult = false;
                        }
                    }
                }
                if (!DateRuleObjects.maxDate.IsNullAndTrimAndEmpty())
                {
                    //是否含有#F{ }
                    if (DateRuleObjects.maxDate.IndexOf(@"#F{") == 0)
                    {
                        List<DateTime?> _4 = null;
                        if (DateRuleObjects.maxDate.IndexOf("ay") > -1) //使用了绑定
                        {
                            if (MaxDateReferToElement.IsNotNull())
                            {
                                string _text = MaxDateReferToElement.Text;
                                string _dateRule = MaxDateReferToElement.DateRule;
                                var ddo = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(_dateRule);

                                if (ddo.IsNotNull())
                                {
                                    _4 = AyDateStrictExpression.ConvertDDVF(_text, DateRuleObjects.maxDate, ddo.dateFmt);
                                }
                                else
                                {
                                    _4 = AyDateStrictExpression.ConvertDDVF(_text, DateRuleObjects.maxDate, "yyyy-MM-dd");
                                }
                            }
                        }
                        else
                        {
                            _4 = AyDateStrictExpression.ConvertDDVF(null, DateRuleObjects.maxDate, null);
                        }
                        bool ret = true;

                        foreach (var subtime in _4)
                        {
                            if (subtime.IsNotNull() && item > subtime.Value)
                            {
                                ret = false;
                                break;
                            }
                        }

                        return ret;
                    }
                    else
                    {
                        //单控制
                        DateTime MaxDateCopy = AyDateStrictExpression.Convert(DateRuleObjects.maxDate);
                        if (item > MaxDateCopy)
                        {
                            returnResult = false;
                        }
                    }
                }


                if (DateRuleObjects.disabledDays.IsNotNull() && DateRuleObjects.disabledDays.Count > 0)
                {
                    var _1 = item.DayOfWeek.GetHashCode();
                    foreach (var disabledDay in DateRuleObjects.disabledDays)
                    {
                        if (_1 == disabledDay)
                        {
                            returnResult = false;
                            break;
                        }
                    }
                }
                if (DateRuleObjects.disabledDates.IsNotNull() && DateRuleObjects.disabledDates.Count > 0)
                {
                    if (DisabledDatesStrings == null)
                        DisabledDatesStrings = new List<string>();
                    else
                        DisabledDatesStrings.Clear();

                    foreach (var disabledDate in DateRuleObjects.disabledDates)
                    {
                        var _ti = AyCalendarService.hasTeShu(disabledDate);
                        string _2 = disabledDate;
                        if (_ti)
                        {
                            _2 = AyDateStrictExpression.ConvertDynamicAyDateExpression(disabledDate);
                            if (!DisabledDatesStrings.Contains(_2))
                            {
                                DisabledDatesStrings.Add(_2);
                            }
                        }
                        else
                        {
                            if (!DisabledDatesStrings.Contains(disabledDate))
                            {
                                DisabledDatesStrings.Add(disabledDate);
                            }
                        }
                        //正则处理
                        if (_2.IndexOf(":") < 0)
                        {
                            bool d = Regex.IsMatch(item.ToString("yyyy-MM-dd"), _2);
                            if (d)
                            {
                                returnResult = false;
                            }
                        }
                        else
                        {
                            bool d = Regex.IsMatch(item.ToString("yyyy-MM-dd HH:mm:ss"), _2);
                            if (d)
                            {
                                returnResult = false;
                            }

                        }
                        if (DateRuleObjects.opposite)
                        {
                            returnResult = !returnResult;
                        }
                    }
                }
                else
                {
                    DisabledDatesStrings = null;
                }
            }
            return returnResult;
        }

        internal void UseValidate()
        {
            bool hasTan = false;
            try
            {

                DateRuleObjects = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(DateRule);

                if (DateRuleObjects.IsNotNull())
                {
                    string[] expectedFormats = { DateRuleObjects.dateFmt };
                    DateTime dt = DateTime.ParseExact(Text, expectedFormats, AyDatePickerHelper.culture, DateTimeStyles.AllowInnerWhite);
                    if (dt.Year < YearStrick.MINYEAR)
                    {
                        if (!hasTan)
                        {
                            HasWrongShowTip();
                            hasTan = true;
                        }

                        return;
                    }
                    else if (dt.Year > YearStrick.MAXYEAR)
                    {
                        if (!hasTan)
                        {
                            HasWrongShowTip();
                            hasTan = true;
                        }
                    }
                    if (!DateItemIsInValidRange(dt))
                    {
                        if (!hasTan)
                        {
                            HasWrongShowTip();
                            hasTan = true;
                        }
                    }
                    if (!hasTan)
                    {
                        PickedDate = dt;
                        if (DateRuleObjects.IsNotNull())
                        {
                            Week = AyCalendarService.GetWeekOfYear(PickedDate.Value, DateRuleObjects.firstDayOfWeek).ToObjectString();
                        }
                        else
                        {
                            Week = "0";
                        }
                    }

                    //}
                }
            }
            catch
            {
                if (!hasTan)
                {
                    hasTan = true;

                    ErrorInfo = AyCalendarService.WRONGTIP;
                    apErrorToolTip.IsOpen = true;
                    IsError = true;
                    return;
                }
            }
        }

        private void HasWrongShowTip()
        {
            ErrorInfo = AyCalendarService.WRONGTIP;
            apErrorToolTip.IsOpen = true;
            IsError = true;
            return;
            //if (MessageBoxResult.OK == MessageBox.Show(AyCalendarService.WRONGTIP, "提示"))
            //{
            //    if (PickedDate.HasValue)
            //    {
            //        this.Text = PickedDate.Value.ToString(DateRuleObjects.dateFmt);
            //    }
            //    else
            //    {
            //        this.Text = "";
            //    }
            //}
            //else
            //{
            //    this.Focus();
            //}
        }


        Guid popGUID = Guid.Empty;


        AyCalendarFMT _fmt;

        public string DateRule
        {
            get { return (string)GetValue(DateRuleProperty); }
            set { SetValue(DateRuleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateRule.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateRuleProperty =
            DependencyProperty.Register("DateRule", typeof(string), typeof(AyDateBoxCalendar), new PropertyMetadata("", OnDateRuleChanged));

        private static void OnDateRuleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = d as AyDateBoxCalendar;
            if (_1.IsNotNull())
            {
                var oldStr = e.OldValue.ToObjectString();
                var newStr = e.NewValue.ToObjectString();
                try
                {
                    _1.RootGrid.Children.Clear();
                    if (_1._PopupContent == null) return;
                    _1._PopupContent.Child = null;
                    _1._PopupContent = null;
                    var _oldRule = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(oldStr);
                    if (_oldRule.IsNotNull())
                    {
                        //if (_oldRule.dateFmt.IsNullAndTrimAndEmpty())
                        //{
                        //    _oldRule.dateFmt = "yyyy-MM-dd";
                        //}
                        string[] expectedFormats = { _oldRule.dateFmt };
                        DateTime dt =
                         DateTime.ParseExact(_1.Text,
                         expectedFormats,
                         AyDatePickerHelper.culture,
                         DateTimeStyles.AllowInnerWhite);

                        var _newRule = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(newStr);
                        if (_newRule.IsNotNull())
                        {
                            //if (_newRule.dateFmt.IsNullAndTrimAndEmpty())
                            //{
                            //    _newRule.dateFmt = "yyyy-MM-dd";
                            //}
                            _1.Text = dt.ToString(_newRule.dateFmt);


                        }
                    }
                }
                catch
                {

                }
            }
        }
        public DateTime? ReverseDateTimeFromText(AyCalendar ca = null)
        {
            var _oldRule = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(DateRule);
            if (ca.IsNotNull())
            {
                ca.IsShowClear = _oldRule.isShowClear ? Visibility.Visible : Visibility.Collapsed;
                ca.IsShowToday = _oldRule.isShowToday ? Visibility.Visible : Visibility.Collapsed;
                this.IsReadOnly = _oldRule.readOnly;
            }
            if (Text.IsNullAndTrimAndEmpty())
            {
                return null;
            }



            if (_oldRule.IsNotNull())
            {
                //if (_oldRule.dateFmt.IsNullAndTrimAndEmpty())
                //{
                //    _oldRule.dateFmt = "yyyy-MM-dd";
                //}
                string[] expectedFormats = { _oldRule.dateFmt };

                try
                {
                    DateTime dt =
                             DateTime.ParseExact(Text,
                             expectedFormats,
                             AyDatePickerHelper.culture,
                             DateTimeStyles.AllowInnerWhite);
                    return dt;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public AyDateRuleJsonToObjects DateRuleObjects;


        AyPopupContentAdorner _ap;
        AyPopupContentAdorner Ap
        {
            get
            {
                if (_ap == null)
                    _ap = new AyPopupContentAdorner();
                _ap.SetResourceReference(AyPopupContentAdorner.StyleProperty, "AyPopupContentAdornerStyle");
                Binding b1 = new Binding();
                b1.Source = _PopupContent;
                b1.Path = new PropertyPath("IsOpen");
                _ap.SetBinding(AyPopupContentAdorner.IsOpenProperty, b1);
                return _ap;
            }
            set
            {
                _ap = value;

            }
        }




        private AyTimeSetTimeout _SetPopupHor;
        /// <summary>
        /// 无注释
        /// </summary>
        public AyTimeSetTimeout SetPopupHor
        {
            get
            {
                if (_SetPopupHor == null)
                {
                    _SetPopupHor = new AyTimeSetTimeout(() =>
                    {
                        _PopupContent.HorizontalOffset = -24;
                        _PopupContent.VerticalOffset = 1;
                    });
                    _SetPopupHor.MillSecond = 5;
                }
                return _SetPopupHor;
            }
        }



        private AyPopup _PopupContent;

        public AyPopup PopupContent
        {
            get
            {
                if (_PopupContent.IsNotNull())
                {
                    return _PopupContent;
                }
                //yyyy年MM月dd日 HH时mm分ss秒
                DateRuleObjects = AyJsonUtility.DecodeObject2<AyDateRuleJsonToObjects>(DateRule);

                if (_PopupContent == null)
                {
                    _PopupContent = new AyPopup(this);
                    _PopupContent.Placement = PlacementMode.Bottom;
                    _PopupContent.AllowsTransparency = true;

                    _PopupContent.PlacementTarget = this;
                    SetPopupHor.Start();
                    Binding b = new Binding();
                    b.Source = this;
                    b.Path = new PropertyPath("IsKeyboardFocused");
         
                    _PopupContent.SetBinding(Popup.StaysOpenProperty, b);
                    popGUID = Guid.NewGuid();
                    PopupContent.Tag = popGUID;
                }
                if (DateRuleObjects.IsNotNull())
                {
                    if (DateRuleObjects.position.IsNotNull())
                    {
                        _PopupContent.HorizontalOffset = DateRuleObjects.position.left;
                        _PopupContent.VerticalOffset = DateRuleObjects.position.top;
                    }
                    else
                    {
                        _PopupContent.VerticalOffset = 1;
                    }
                }




                CreatePopupList();
                Ap.Content = RootGrid;
          
                _PopupContent.Child = Ap;


                return _PopupContent;

            }
            set { _PopupContent = value; }
        }


        Grid _grid;
        Grid RootGrid
        {
            get
            {
                if (_grid == null)
                    _grid = new Grid();
                return _grid;
            }
            set
            {
                _grid = value;

            }
        }


        public void CreatePopupList()
        {

            if (DateRule == "")            //如果是空白，默认选中今天，啥也不限制
            {

                AyCalendar ac = new AyCalendar();
                ac.BorderBrush = Brushes.Transparent;
                ac.MinDateReferToElement = this.MinDateReferToElement;
                ac.MaxDateReferToElement = this.MaxDateReferToElement;
                ac.DateBoxInput = this;
                //ac.DisabledDatesStrings = this.DisabledDatesStrings;
                ac.HorizontalAlignment = HorizontalAlignment.Center;
                ac.VerticalAlignment = VerticalAlignment.Center;
                ac.SelectMode = AyDatePickerSelectMode.OnlySelectDate;
                if (!this.Text.IsNullAndTrimAndEmpty())
                {
                    DateTime? date = ReverseDateTimeFromText(ac);
                    if (date.HasValue)
                    {
                        ac.SelectedDateTime = date;
                    }
                }
                RootGrid.Children.Add(ac);
            }
            else
            {
                if (DateRuleObjects.IsNotNull())
                {
                    _fmt = AyCalendarService.GetAyCalendarFMT(DateRuleObjects.dateFmt);
                    switch (_fmt)
                    {
                        case AyCalendarFMT.None:
                            break;
                        default:
                            AyCalendar ac = CreateAyCalendar();
                            RootGrid.Children.Add(ac);
                            break;
                    }
                }
            }

        }

        private AyCalendar CreateAyCalendar()
        {
            if (DateRuleObjects.disabledDates.IsNotNull() && DateRuleObjects.disabledDates.Count > 0)
            {
                if (DisabledDatesStrings == null)
                    DisabledDatesStrings = new List<string>();
                else
                    DisabledDatesStrings.Clear();

                foreach (var disabledDate in DateRuleObjects.disabledDates)
                {
                    var _ti = AyCalendarService.hasTeShu(disabledDate);
                    string _2 = disabledDate;
                    if (_ti)
                    {
                        _2 = AyDateStrictExpression.ConvertDynamicAyDateExpression(disabledDate);
                        if (!DisabledDatesStrings.Contains(_2))
                        {
                            DisabledDatesStrings.Add(_2);
                        }
                    }
                    else
                    {
                        if (!DisabledDatesStrings.Contains(_2))
                        {
                            DisabledDatesStrings.Add(_2);
                        }
                    }
                }
            }
            else
            {
                DisabledDatesStrings = null;
            }
            List<string> SpecialDatesStrings = null;
            if (DateRuleObjects.specialDates.IsNotNull() && DateRuleObjects.specialDates.Count > 0)
            {
                if (SpecialDatesStrings == null)
                    SpecialDatesStrings = new List<string>();
                foreach (var specialDates in DateRuleObjects.specialDates)
                {
                    var _ti = AyCalendarService.hasTeShu(specialDates);
                    string _2 = specialDates;
                    if (_ti)
                    {
                        _2 = AyDateStrictExpression.ConvertDynamicAyDateExpression(specialDates);
                        if (!SpecialDatesStrings.Contains(_2))
                        {
                            SpecialDatesStrings.Add(_2);
                        }
                    }
                    else
                    {
                        if (!SpecialDatesStrings.Contains(_2))
                        {
                            SpecialDatesStrings.Add(_2);
                        }
                    }
                }
            }


            AyCalendar ac = new AyCalendar(DateRule);
            ac.BorderBrush = Brushes.Transparent;
            ac.MinDateReferToElement = this.MinDateReferToElement;
            ac.MaxDateReferToElement = this.MaxDateReferToElement;
            ac.DisabledDatesStrings = this.DisabledDatesStrings;
            ac.SpecialDatesStrings = SpecialDatesStrings;
            ac.DateBoxInput = this;
            ac.HorizontalAlignment = HorizontalAlignment.Center;
            ac.VerticalAlignment = VerticalAlignment.Center;
            DateTime? date = ReverseDateTimeFromText(ac);
            if (date.HasValue)
            {
                ac.SelectedDateTime = date;
            }
            return ac;
        }

    }
}
