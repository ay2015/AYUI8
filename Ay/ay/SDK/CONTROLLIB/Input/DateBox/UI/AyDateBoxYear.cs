using ay.contentcore;
using ay.contents;
using ay.Controls.Info;
using ay.Controls.Services;
using ay.SDK.CONTROLLIB.Primitive;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ay.Controls
{
    public class AyDateBoxYear : AyFormInput
    {
        public AyDateBoxYear()
        {
            IsShowAddMinusButton = false;
            IsIntegerBox = true;
            MaxValue = YearStrick.MAXYEAR;
            MinValue = YearStrick.MINYEAR;
            MaxLength = 4;
            Loaded += AyDateBoxYear_Loaded;
            Cursor = Cursors.Hand;
            Unloaded += AyDateBoxYear_Unloaded;
            AutoSelected = Enums.AutoSelectBehavior.OnFocus;
        }
        public void ClosePopup()
        {
            if (_PopupContent.IsNotNull())
            {
                _PopupContent.IsOpen = false;
            }
        }
        private void AyDateBoxYear_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyDateBoxYear_Loaded;
            this.KeyDown += AyDateBoxYear_KeyDown;
            this.GotKeyboardFocus += AyDateBoxYear_GotKeyboardFocus;
        }

        private void AyDateBoxYear_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= AyDateBoxYear_Unloaded;
            this.KeyDown -= AyDateBoxYear_KeyDown;
        }

        private void AyDateBoxYear_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && _PopupContent != null)
            {
                WhenBoxLostFocus();
            }
        }

        private void AyDateBoxYear_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
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
        /// <summary>
        /// 这是验证后，修复文本框值后，触发的文本框的值
        /// </summary>
        public event EventHandler<EventArgs> OnAyBoxLostFocus;


        public bool setOpposite = false;
        bool isCreateLessMinValue = false;
        bool isCreateLessMaxValue = false;
        public Button CreateButtons(int content)
        {
            Button btn = new Button();
            btn.ClickMode = ClickMode.Press;
            btn.Content = content.ToString();
            btn.SetResourceReference(Button.StyleProperty, "AyDateBoxCalendar.Button");
            AyCalendarService.SetClickYearButtonsEnabled(btn, content, MinDateTime, MaxDateTime, DisplayDatesStrings, setOpposite, SelectedDateTime);
            if (content < MinValue)
            {
                isCreateLessMinValue = true;
                btn.IsEnabled = false;
            }
            if (content > MaxValue)
            {
                btn.IsEnabled = false;
                isCreateLessMaxValue = true;
            }
            btn.Tag = content;
            btn.Click += Btn_Click;
            return btn;
        }


        public List<DateTime?> MinDateTime
        {
            get { return (List<DateTime?>)GetValue(MinDateTimeProperty); }
            set { SetValue(MinDateTimeProperty, value); }
        }

        public static readonly DependencyProperty MinDateTimeProperty =
            DependencyProperty.Register("MinDateTime", typeof(List<DateTime?>), typeof(AyDateBoxYear), new PropertyMetadata(null));




        public List<DateTime?> MaxDateTime
        {
            get { return (List<DateTime?>)GetValue(MaxDateTimeProperty); }
            set { SetValue(MaxDateTimeProperty, value); }
        }

        public static readonly DependencyProperty MaxDateTimeProperty =
            DependencyProperty.Register("MaxDateTime", typeof(List<DateTime?>), typeof(AyDateBoxYear), new PropertyMetadata(null));

        #region 用于DisabledDates  2017-3-8 15:52:28 AY设计
        public List<string> DisplayDatesStrings;
        #endregion



        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var _1 = btn.Tag.ToString();
            Text = _1;
            _PopupContent.IsOpen = false;
            OnAyBoxLostFocus?.Invoke(this, null);
        }

        public Button CreatePageButtons(string icon, string content)
        {
            Button btn = new Button();
            if (icon == null)
            {
                btn.Content = content;
                btn.SetResourceReference(Button.StyleProperty, "AyDateBoxCalendar.Button");
            }
            else
            {
                UIBase.SetIcon(btn, icon);
                btn.SetResourceReference(Button.StyleProperty, "AyDateBoxCalendar.Icon");
            }
            btn.ToolTip = content;
            btn.ClickMode = ClickMode.Press;
  
            return btn;
        }
  
        int beginYear = 0;
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


                    //yearListPopup.Bind<AyDateBoxYear>(Popup.StaysOpenProperty, x => x.IsKeyboardFocused);
                }
                fiveBefore = 0;
                endBefore = 0;
                prewtenbutton = null;
                nexttenbutton = null;
                _grid = null;
                beginYear = DateTime.Now.Year;
                if (!Text.IsNullAndTrimAndEmpty() && Text.Length == 4)
                {
                    beginYear = Text.ToInt();
                }
                CreateTenYearList(beginYear);
                AyPopupContentAdorner ap = new AyPopupContentAdorner();
                Binding b1 = new Binding();
                b1.Source = _PopupContent;
                b1.Path = new PropertyPath("IsOpen");
                ap.SetBinding(AyPopupContentAdorner.IsOpenProperty, b1);

                ap.Content = RootGrid;
                _PopupContent.Child = ap;
                isCreateLessMinValue = false;
                isCreateLessMaxValue = false;

                return _PopupContent;

            }
            set { _PopupContent = value; }
        }

        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(AyDateBoxYear), new PropertyMetadata(null));

        int fiveBefore = 0;
        int endBefore = 0;
        Button prewtenbutton = null;
        Button nexttenbutton = null;
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
        //<control:Button Margin = "5,0,0,0" RenderMode="{Binding RenderMode}"  IsEnabled="{Binding IsEnabled}" Padding="16,8"
        //                                 Content="默认主题风格按钮"  Icon="fa_paint_brush"   Style="{DynamicResource Button.AY.Outline3}"></control:Button>
        public void CreateTenYearList(int beginYear)
        {
            RootGrid.Children.Clear();
            GridService.SetColumns(RootGrid, "? ?");
            GridService.SetRows(RootGrid, "? ? ? ? ? ?");
            fiveBefore = beginYear - 5;
            endBefore = beginYear + 5;
            int _1 = 0;
            for (int i = fiveBefore; i < endBefore; i++)
            {
                //创建按钮
                var _11 = CreateButtons(i);
                if (_1 > 4)
                {
                    GridService.SetRowColumn(_11, (_1 - 5) + " 1");
                }
                else
                {
                    GridService.SetRowColumn(_11, _1 + " 0");
                }
                _1++;
                RootGrid.Children.Add(_11);
            }

            Grid gridbottom = new Grid();
            Grid.SetColumnSpan(gridbottom, 2);
            Grid.SetRow(gridbottom, 5);
            RootGrid.Children.Add(gridbottom);

            GridService.SetColumns(gridbottom, "? ? ?");
            prewtenbutton = CreatePageButtons("more_ay_yearLeft", Langs.ay_Last10.Lang());
            if (isCreateLessMinValue)
            {
                prewtenbutton.IsEnabled = false;
            }
            prewtenbutton.Click += _2_Click;
            Grid.SetColumn(prewtenbutton, 0);

            var _3 = CreatePageButtons("path_ay_yearClose", Langs.share_close.Lang());
            _3.Click += _3_Click;
            Grid.SetColumn(_3, 1);


            nexttenbutton = CreatePageButtons("more_ay_yearRight", Langs.ay_Next10.Lang());
            if (isCreateLessMaxValue)
            {
                nexttenbutton.IsEnabled = false;
            }
            nexttenbutton.Click += _4_Click;


            Grid.SetColumn(nexttenbutton, 2);
            gridbottom.Children.Add(prewtenbutton);
            gridbottom.Children.Add(_3);
            gridbottom.Children.Add(nexttenbutton);
        }

        private void _3_Click(object sender, RoutedEventArgs e)
        {
            _PopupContent.IsOpen = false;
        }

        private void _4_Click(object sender, RoutedEventArgs e)
        {
            beginYear = fiveBefore + 15;
            fiveBefore = beginYear - 5;
            endBefore = beginYear + 5;
            YearNavigationOperation();
            return;
        }

        private void YearNavigationOperation()
        {
            isCreateLessMinValue = false;
            isCreateLessMaxValue = false;
            int _1 = 0;
            for (int i = fiveBefore; i < endBefore; i++)
            {
                var _11 = RootGrid.Children[_1] as Button;
                if (_11.IsNotNull())
                {
                    _11.Content = i;
                    _11.Tag = i;
                    _11.IsEnabled = true;
                    if (i < MinValue)
                    {
                        isCreateLessMinValue = true;
                        _11.IsEnabled = false;
                    }
                    if (i > MaxValue)
                    {
                        _11.IsEnabled = false;
                        isCreateLessMaxValue = true;
                    }
                    AyCalendarService.SetClickYearButtonsEnabled(_11, i, MinDateTime, MaxDateTime, DisplayDatesStrings, setOpposite, SelectedDateTime);
                }
                _1++;
            }
            if (!isCreateLessMinValue)
            {
                prewtenbutton.IsEnabled = true;
            }
            else
            {
                prewtenbutton.IsEnabled = false;
            }

            if (!isCreateLessMaxValue)
            {
                nexttenbutton.IsEnabled = true;
            }
            else
            {
                nexttenbutton.IsEnabled = false;
            }
            this.Focus();
            this.SelectAll();
        }

        private void _2_Click(object sender, RoutedEventArgs e)
        {

            beginYear = fiveBefore - 5;
            fiveBefore = beginYear - 5;
            endBefore = beginYear + 5;
            YearNavigationOperation();
            return;
        }
    }
}
