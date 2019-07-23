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
    public class AyDateBoxSecond : AyFormInput
    {
        public AyDateBoxSecond()
        {
            IsShowAddMinusButton = false;
            IsIntegerBox = true;
            MaxValue = 59;
            MaxLength = 2;
            MinValue = 0;
            Loaded += AyDateBoxSecond_Loaded;
            AutoSelected = Enums.AutoSelectBehavior.OnFocus;
            Unloaded += AyDateBox_Unloaded;
        }
        public void ClosePopup()
        {
            if (_PopupContent.IsNotNull())
            {
                _PopupContent.IsOpen = false;
            }
        }
        private void AyDateBox_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= AyDateBox_Unloaded;
            this.KeyDown -= AyDateBox_KeyDown;
        }


        /// <summary>
        /// 增加位置，默认弹出位置
        /// </summary>
        public PlacementMode PlacementMode
        {
            get { return (PlacementMode)GetValue(PlacementModeProperty); }
            set { SetValue(PlacementModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlacementMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlacementModeProperty =
            DependencyProperty.Register("PlacementMode", typeof(PlacementMode), typeof(AyDateBoxSecond), new PropertyMetadata(PlacementMode.Top));


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
        private void WhenBoxLostFocus()
        {
            if (_PopupContent != null)
            {
                _PopupContent.IsOpen = false;
                OnAyBoxLostFocus?.Invoke(this, null);
            }
        }

        private void AyDateBoxSecond_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyDateBoxSecond_Loaded;
            this.KeyDown += AyDateBox_KeyDown;
            this.Cursor = Cursors.Hand;
            this.GotKeyboardFocus += AyDateBoxSecond_GotKeyboardFocus;
        }


        private void AyDateBoxSecond_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            WhenBoxGotFocus();
        }

        private void WhenBoxGotFocus()
        {
            PopupContent.IsOpen = true;

        }
     


        /// <summary>
        /// 这是验证后，修复文本框值后，触发的文本框的值
        /// </summary>
        public event EventHandler<EventArgs> OnAyBoxLostFocus;


        #region 时间限制拓展 2017-3-1 14:41:02
        /// <summary>
        /// 当前日历控件选择的时间，此属性不对外用户设置，设置无效
        /// </summary>
        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(AyDateBoxSecond), new PropertyMetadata(null));



        public List<DateTime?> MinDateTime
        {
            get { return (List<DateTime?>)GetValue(MinDateTimeProperty); }
            set { SetValue(MinDateTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinDateTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinDateTimeProperty =
            DependencyProperty.Register("MinDateTime", typeof(List<DateTime?>), typeof(AyDateBoxSecond), new PropertyMetadata(null));




        public List<DateTime?> MaxDateTime
        {
            get { return (List<DateTime?>)GetValue(MaxDateTimeProperty); }
            set { SetValue(MaxDateTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxDateTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxDateTimeProperty =
            DependencyProperty.Register("MaxDateTime", typeof(List<DateTime?>), typeof(AyDateBoxSecond), new PropertyMetadata(null));
        public bool setOpposite = false;
        #endregion

        public Button CreateButtons(int content)
        {
            Button btn = new Button();
            btn.ClickMode = ClickMode.Press;
            btn.Content = content.ToString();
            btn.SetResourceReference(Button.StyleProperty, "AyDateBoxCalendar.Button");
            AyCalendarService.SetClickSecondButtonsEnabled(btn, content, SelectedDateTime, MinDateTime, MaxDateTime, DisabledDatesStrings, setOpposite);
            btn.Tag = content;
            btn.Click += Btn_Click;
            return btn;
        }
        #region 用于DisabledDates  2017-3-8 15:52:28 AY设计
        //public Dictionary<string, DateTime?> DisabledDates;
        public List<string> DisabledDatesStrings;
        #endregion
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
                if (_PopupContent.IsNotNull() )
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






        public void CreatePopupList()
        {
            RootGrid.Children.Clear();
            GridService.SetColumns(RootGrid, "? ? ? ? ? ?");

            int _1 = 0;
         
                for (int i = 0; i < 4; i++)
                {
                    //创建按钮
                    var _11 = CreateButtons(_1);
                    GridService.SetRowColumn(_11, "0 " + i);
                    _1 = _1 + 15;
                RootGrid.Children.Add(_11);
                }

        }


    }
}
