using ay.contents;
using ay.Controls.Services;
using ay.SDK.CONTROLLIB.Primitive;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;


namespace ay.Controls
{
    public class AyDateBoxMonth : AyFormInput
    {
        public AyDateBoxMonth()
        {
            IsShowAddMinusButton = false;
            IsIntegerBox = true;
            MaxValue = 12;
            MaxLength = 2;
            MinValue = 1;
            Loaded += AyDateBoxMonth_Loaded;
            AutoSelected = Enums.AutoSelectBehavior.OnFocus;
            Unloaded += AyDateBoxMonth_Unloaded;
        }

        private void AyDateBoxMonth_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= AyDateBoxMonth_Unloaded;
            this.KeyDown -= AyDateBox_KeyDown;
        }

        public bool setOpposite = false;
        public void ClosePopup()
        {
            if (_PopupContent.IsNotNull())
            {
                _PopupContent.IsOpen = false;
            }
        }
        private void AyDateBoxMonth_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyDateBoxMonth_Loaded;
            this.KeyDown += AyDateBox_KeyDown;
            this.GotKeyboardFocus += AyDateBoxMonth_GotKeyboardFocus;
        }
        private void AyDateBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (_PopupContent.IsNotNull())
                {
                    WhenBoxLostFocus();
                }
            }
        }

        private void AyDateBoxMonth_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            PopupContent.IsOpen = true;
        }


        private void WhenBoxLostFocus()
        {
            if (_PopupContent != null)
            {
                _PopupContent.IsOpen = false;
                OnAyBoxLostFocus?.Invoke(this, null);
            }
        }

        #region 用于决定可用性 而拓展的3个属性 2017-2-27 14:20:24

        /// <summary>
        /// ay 2017-2-27 14:08:17
        /// 拓展：用于知晓年份后，可以限制当前月按钮的可用性
        /// </summary>
        public int? Year
        {
            get { return (int?)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }
        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(int?), typeof(AyDateBoxMonth), new PropertyMetadata(null));

        // 需要知道最小的日期，最大的日期，然后当前年份-》获得日期



        public List<DateTime?> MinDateTime
        {
            get { return (List<DateTime?>)GetValue(MinDateTimeProperty); }
            set { SetValue(MinDateTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinDateTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinDateTimeProperty =
            DependencyProperty.Register("MinDateTime", typeof(List<DateTime?>), typeof(AyDateBoxMonth), new PropertyMetadata(null));

        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(AyDateBoxMonth), new PropertyMetadata(null));



        public List<DateTime?> MaxDateTime
        {
            get { return (List<DateTime?>)GetValue(MaxDateTimeProperty); }
            set { SetValue(MaxDateTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxDateTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxDateTimeProperty =
            DependencyProperty.Register("MaxDateTime", typeof(List<DateTime?>), typeof(AyDateBoxMonth), new PropertyMetadata(null));


        #region 用于DisabledDates  2017-3-8 15:52:28 AY设计
        //public Dictionary<string, DateTime?> DisabledDates;
        public List<string> DisabledDatesStrings;
        #endregion

        #endregion




        public event EventHandler<EventArgs> OnAyBoxLostFocus;

        public Button CreateButtons(string content, int ctag)
        {
            Button btn = new Button();

            btn.ClickMode = ClickMode.Press;
            btn.Content = content.ToString();
            btn.SetResourceReference(Button.StyleProperty, "AyDateBoxCalendar.Button");

            AyCalendarService.SetClickMonthButtonsEnabled(btn, ctag, Year, MinDateTime, MaxDateTime, DisabledDatesStrings, setOpposite, SelectedDateTime);

            btn.Tag = ctag;
            btn.Click += Btn_Click;
            return btn;
        }


        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var _1 = btn.Tag.ToString();
            Text = _1;
            _PopupContent.IsOpen = false;
            OnAyBoxLostFocus?.Invoke(this, null);
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
                if (_PopupContent == null)
                {
                    _PopupContent = new AyPopup(this);
                    _PopupContent.Placement = PlacementMode.Bottom;
                    _PopupContent.AllowsTransparency = true;
                    _PopupContent.HorizontalOffset = -24;
                    _PopupContent.VerticalOffset = 1;
                    _PopupContent.PlacementTarget = this;
                    Binding b = new Binding();
                    b.Source = this;
                    b.Path = new PropertyPath("IsKeyboardFocused");
                    _PopupContent.StaysOpen = true;
                    _PopupContent.SetBinding(Popup.StaysOpenProperty, b);
                }
                AyPopupContentAdorner ap = new AyPopupContentAdorner();
                Binding b1 = new Binding();
                b1.Source = _PopupContent;
                b1.Path = new PropertyPath("IsOpen");
                ap.SetBinding(AyPopupContentAdorner.IsOpenProperty, b1);
                CreatePopupList();
                ap.Content = RootGrid;
                _PopupContent.Child = ap;

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

        //int currentMonth = 1;
        public void CreatePopupList()
        {
            RootGrid.Children.Clear();

            GridService.SetColumns(RootGrid, "? ?");
            GridService.SetRows(RootGrid, "? ? ? ? ? ?");
            //currentMonth =this.Text.ToInt();
            var _11 = CreateButtons(Langs.ay_MonthName1.Lang(), 1);
            GridService.SetRowColumn(_11, "0 0");
            var _12 = CreateButtons(Langs.ay_MonthName2.Lang(), 2);
            GridService.SetRowColumn(_12, "1 0");
            var _13 = CreateButtons(Langs.ay_MonthName3.Lang(), 3);
            GridService.SetRowColumn(_13, "2 0");
            var _14 = CreateButtons(Langs.ay_MonthName4.Lang(), 4);
            GridService.SetRowColumn(_14, "3 0");
            var _15 = CreateButtons(Langs.ay_MonthName5.Lang(), 5);
            GridService.SetRowColumn(_15, "4 0");
            var _16 = CreateButtons(Langs.ay_MonthName6.Lang(), 6);
            GridService.SetRowColumn(_16, "5 0");

            var _17 = CreateButtons(Langs.ay_MonthName7.Lang(), 7);
            GridService.SetRowColumn(_17, "0 1");
            var _18 = CreateButtons(Langs.ay_MonthName8.Lang(), 8);
            GridService.SetRowColumn(_18, "1 1");
            var _19 = CreateButtons(Langs.ay_MonthName9.Lang(), 9);
            GridService.SetRowColumn(_19, "2 1");
            var _20 = CreateButtons(Langs.ay_MonthName10.Lang(), 10);
            GridService.SetRowColumn(_20, "3 1");
            var _21 = CreateButtons(Langs.ay_MonthName11.Lang(), 11);
            GridService.SetRowColumn(_21, "4 1");
            var _22 = CreateButtons(Langs.ay_MonthName12.Lang(), 12);
            GridService.SetRowColumn(_22, "5 1");

            RootGrid.Children.Add(_11);
            RootGrid.Children.Add(_12);
            RootGrid.Children.Add(_13);
            RootGrid.Children.Add(_14);
            RootGrid.Children.Add(_15);
            RootGrid.Children.Add(_16);
            RootGrid.Children.Add(_17);
            RootGrid.Children.Add(_18);
            RootGrid.Children.Add(_19);
            RootGrid.Children.Add(_20);
            RootGrid.Children.Add(_21);
            RootGrid.Children.Add(_22);

        }


    }
}
