using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ay.Enums;

namespace ay.Controls
{
    /// <summary>
    /// AyCalendarTime.xaml 的交互逻辑
    /// </summary>
    public partial class AyCalendarTime : UserControl
    {
        public AyCalendarTime()
        {
            InitializeComponent();
            Loaded += AyCalendarTime_Loaded;
        }

        #region 时间限制拓展 2017-3-1 14:52:31

        //public void SetDisabledDatesTime(Dictionary<string, DateTime?> DisplayDates, List<string> DisplayDatesStrings)
        //{
        //    txt_Hour.SelectedDateTime = SelectedDateTime;
        //    txt_Minute.SelectedDateTime = SelectedDateTime;
        //    txt_Second.SelectedDateTime = SelectedDateTime;
        //    //判断时间是否合法

        //}
        public void SetDisabledDatesStrings(List<string> DisabledDatesStrings,bool opposite)
        {
            txt_Hour.DisabledDatesStrings = DisabledDatesStrings;
            txt_Minute.DisabledDatesStrings = DisabledDatesStrings;
            txt_Second.DisabledDatesStrings = DisabledDatesStrings;
            txt_Hour.setOpposite = opposite;
            txt_Minute.setOpposite = opposite;
            txt_Minute.setOpposite = opposite;
        }

        public void SetSelectedDateTime(DateTime? SelectedDateTime)
        {
            txt_Hour.SelectedDateTime = SelectedDateTime;
            txt_Minute.SelectedDateTime = SelectedDateTime;
            txt_Second.SelectedDateTime = SelectedDateTime;
            //判断时间是否合法

        }
        public void SetMinMaxDateTime(List<DateTime?> MinDateTime, List<DateTime?> MaxDateTime)
        {
            txt_Hour.MinDateTime = MinDateTime;
            txt_Minute.MinDateTime = MinDateTime;
            txt_Second.MinDateTime = MinDateTime;

            txt_Hour.MaxDateTime = MaxDateTime;
            txt_Minute.MaxDateTime = MaxDateTime;
            txt_Second.MaxDateTime = MaxDateTime;
        }


        #endregion

        public AyDatePickerSelectMode SelectMode
        {
            get { return (AyDatePickerSelectMode)GetValue(SelectModeProperty); }
            set { SetValue(SelectModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectModeProperty =
            DependencyProperty.Register("SelectMode", typeof(AyDatePickerSelectMode), typeof(AyCalendarTime), new PropertyMetadata(AyDatePickerSelectMode.DateTime, OnSelectDateChanged));



        private static void OnSelectDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyCalendarTime).UpdateSelectMode();
        }

        public string Hour
        {
            get { return (string)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hour.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register("Hour", typeof(string), typeof(AyCalendarTime), new PropertyMetadata(""));



        public string Minute
        {
            get { return (string)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minute.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register("Minute", typeof(string), typeof(AyCalendarTime), new PropertyMetadata(""));



        public event EventHandler<EventArgs> OnTimeChanged;

        public string Second
        {
            get { return (string)GetValue(SecondProperty); }
            set { SetValue(SecondProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Second.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.Register("Second", typeof(string), typeof(AyCalendarTime), new PropertyMetadata(""));



        private void AyCalendarTime_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyCalendarTime_Loaded;
            txt_Hour.OnAyBoxLostFocus += Txt_Time_OnAyBoxLostFocus;
            txt_Hour.OnAyBoxMouseWheeled += Txt_Time_OnAyBoxMouseWheeled;
            txt_Minute.OnAyBoxLostFocus += Txt_Time_OnAyBoxLostFocus;
            txt_Minute.OnAyBoxMouseWheeled += Txt_Time1_OnAyBoxMouseWheeled;
            txt_Second.OnAyBoxLostFocus += Txt_Time_OnAyBoxLostFocus;
            txt_Second.OnAyBoxMouseWheeled += Txt_Time2_OnAyBoxMouseWheeled;

            UpdateSelectMode();
        }

        public void SetHourEnabled()
        {
            txt_Hour.IsEnabled = false;
         
        }
        public void SetMinuteEnabled()
        {
            txt_Minute.IsEnabled = false;
    
        }
        public void SetSecondEnabled()
        {
            txt_Second.IsEnabled = false;
           
        }

        private void UpdateSelectMode()
        {
            switch (SelectMode)
            {
                case AyDatePickerSelectMode.DateTime:
                    cdrTime.Visibility = Visibility.Visible;
                    break;
                case AyDatePickerSelectMode.HM:
                    cdrTime.Visibility = Visibility.Visible;
                    //minMaohao.Visibility = txt_Second.Visibility = Visibility.Collapsed;
                    txt_Second.Text = "0";
                    SetSecondEnabled();

                    break;
                case AyDatePickerSelectMode.HMS:
                    cdrTime.Visibility = Visibility.Visible;

                    break;
                case AyDatePickerSelectMode.DateTimeH:
                    cdrTime.Visibility = Visibility.Visible;

                    btn0s.Visibility =
                    btn30s.Visibility =
                    btn59s.Visibility = Visibility.Collapsed;
                    txt_Second.Text = "0";
                    txt_Minute.Text = "0";
                    SetMinuteEnabled();
                    SetSecondEnabled();
                    break;
                case AyDatePickerSelectMode.DateTimeNoSecond:
                    cdrTime.Visibility = Visibility.Visible;
                    //minMaohao.Visibility = txt_Second.Visibility = Visibility.Collapsed;
                    txt_Second.Text = "0";
                    SetSecondEnabled();
                    break;
                case AyDatePickerSelectMode.OnlySelectDate:
                    cdrTime.Visibility = Visibility.Collapsed;
                    txt_Hour.Text = "0";
                    txt_Minute.Text = "0";
                    txt_Second.Text = "0";

                    break;
            }
        }

        private void Txt_Time_OnAyBoxMouseWheeled(object sender, EventArgs e)
        {
            if (txt_Hour.IsEnabled)
            {

                if (txt_Hour.SelectedDateTime.HasValue)
                {
                    var _hour1 = txt_Hour.Text.ToInt();
                    if (txt_Hour.MinDateTime.Count > 0)
                    {
                        foreach (var subitem in txt_Hour.MinDateTime)
                        {
                            if (subitem.IsNull()) continue;
                            if (txt_Hour.SelectedDateTime.Value.Date == subitem.Value.Date
                                    && _hour1 < subitem.Value.Hour
                                    )
                            {
                                txt_Hour.Text = subitem.Value.Hour.ToString();
                                break;
                            }
                        }
                    }
                    if (txt_Hour.MaxDateTime.Count > 0)
                    {
                        foreach (var subitem in txt_Hour.MaxDateTime)
                        {
                            if (subitem.IsNull()) continue;
                            if (txt_Hour.SelectedDateTime.Value.Date == subitem.Value.Date && _hour1 > subitem.Value.Hour)
                            {
                                txt_Hour.Text = subitem.Value.Hour.ToString();
                                break;
                            }
                        }
                    }
                }

                UpdateWhenTimeBoxChanged();
            }
        }
        private void Txt_Time1_OnAyBoxMouseWheeled(object sender, EventArgs e)
        {
            if (txt_Minute.IsEnabled)
            {
                if (txt_Minute.SelectedDateTime.HasValue)
                {
                    var _min1 = txt_Minute.Text.ToInt();

                    if (txt_Minute.MinDateTime.Count > 0)
                    {
                        foreach (var subitem in txt_Minute.MinDateTime)
                        {
                            if (subitem.IsNull()) continue;

                            if (txt_Minute.SelectedDateTime.Value.Date == subitem.Value.Date
                                && txt_Minute.SelectedDateTime.Value.Hour == subitem.Value.Hour
                                && _min1 < subitem.Value.Minute
                                )
                            {
                                txt_Minute.Text = subitem.Value.Minute.ToString();
                                break;
                            }
                        }
                    }
                    if (txt_Minute.MaxDateTime.Count > 0)
                    {
                        foreach (var subitem in txt_Minute.MaxDateTime)
                        {
                            if (subitem.IsNull()) continue;
                            if (txt_Minute.SelectedDateTime.Value.Date == subitem.Value.Date && txt_Minute.SelectedDateTime.Value.Hour == subitem.Value.Hour
                          && _min1 > subitem.Value.Minute)
                            {
                                txt_Minute.Text = subitem.Value.Minute.ToString();
                                break;
                            }
                        }
                    }
                }

                UpdateWhenTimeBoxChanged();
            }
        }
        private void Txt_Time2_OnAyBoxMouseWheeled(object sender, EventArgs e)
        {
            if (txt_Second.IsEnabled)
            {
                if (txt_Second.SelectedDateTime.HasValue)
                {
                    var _sec1 = txt_Minute.Text.ToInt();

                    if (txt_Second.MinDateTime.Count > 0)
                    {
                        foreach (var subitem in txt_Second.MinDateTime)
                        {
                            if (subitem.IsNull()) continue;

                            if (txt_Second.SelectedDateTime.Value.Date == subitem.Value.Date &&
                     txt_Second.SelectedDateTime.Value.Hour == subitem.Value.Hour &&
                     txt_Second.SelectedDateTime.Value.Minute == subitem.Value.Minute)
                            {
                                if (_sec1 < subitem.Value.Second)
                                {
                                    txt_Second.Text = subitem.Value.Second.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    if (txt_Second.MaxDateTime.Count > 0)
                    {
                        foreach (var subitem in txt_Second.MaxDateTime)
                        {
                            if (subitem.IsNull()) continue;
                            if (txt_Second.SelectedDateTime.Value.Date == subitem.Value.Date &&
                      txt_Second.SelectedDateTime.Value.Hour == subitem.Value.Hour
                      &&
                      txt_Second.SelectedDateTime.Value.Minute == subitem.Value.Minute
                      )
                            {
                                if (_sec1 > subitem.Value.Second)
                                {
                                    txt_Second.Text = subitem.Value.Second.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }

                UpdateWhenTimeBoxChanged();
            }
        }

        private void Txt_Time_OnAyBoxLostFocus(object sender, EventArgs e)
        {
            UpdateWhenTimeBoxChanged();
        }

        private void UpdateWhenTimeBoxChanged()
        {
            if (OnTimeChanged != null)
                OnTimeChanged(this, null);
        }
        private void btn0s_Click(object sender, RoutedEventArgs e)
        {
            txt_Minute.Text = "0";
            txt_Second.Text = "0";
            UpdateWhenTimeBoxChanged();
            //SelectedDateTime = new DateTime(SelectedDateTime.Value.Year, SelectedDateTime.Value.Month, SelectedDateTime.Value.Day, txt_Hour.Text.ToInt(), 0, 0);
            //DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void btn30s_Click(object sender, RoutedEventArgs e)
        {
            txt_Minute.Text = "30";
            txt_Second.Text = "0";
            UpdateWhenTimeBoxChanged();
            //SelectedDateTime = new DateTime(SelectedDateTime.Value.Year, SelectedDateTime.Value.Month, SelectedDateTime.Value.Day, txt_Hour.Text.ToInt(), 30, 0);
            //DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void btn59s_Click(object sender, RoutedEventArgs e)
        {
            txt_Minute.Text = "59";
            txt_Second.Text = "59";
            UpdateWhenTimeBoxChanged();
            //SelectedDateTime = new DateTime(SelectedDateTime.Value.Year, SelectedDateTime.Value.Month, SelectedDateTime.Value.Day, txt_Hour.Text.ToInt(), 59, 59);
            //DateShowInfo.Text = SelectedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
