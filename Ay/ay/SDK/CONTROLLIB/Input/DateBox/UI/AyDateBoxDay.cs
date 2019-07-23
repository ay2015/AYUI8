using ay.SDK.CONTROLLIB.Primitive;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;


namespace ay.Controls
{
    public class AyDateBoxDay : AyFormInput
    {
        public AyDateBoxDay()
        {
            IsShowAddMinusButton = false;
            IsIntegerBox = true;
            MaxValue = 31;
            MaxLength = 2;
            MinValue = 1;
            Loaded += AyDateBoxDay_Loaded;
            AutoSelected = Enums.AutoSelectBehavior.OnFocus;
            Unloaded += AyDateBoxMonth_Unloaded;
        }

        public void ClosePopup()
        {
            if (_PopupContent.IsNotNull())
            {
                _PopupContent.IsOpen = false;
            }
        }
        private void AyDateBoxMonth_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= AyDateBoxMonth_Unloaded;
            this.KeyDown -= AyDateBox_KeyDown;
        }

        private void AyDateBoxDay_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyDateBoxDay_Loaded;
            this.KeyDown += AyDateBox_KeyDown;
            this.GotKeyboardFocus += AyDateBoxDay_GotKeyboardFocus;
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

        private void AyDateBoxDay_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
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


        public event EventHandler<EventArgs> OnAyBoxLostFocus;

        public Button CreateButtons(string content, int ctag)
        {
            Button btn = new Button();
            btn.ClickMode = ClickMode.Press;
            btn.Content = content.ToString();
            btn.SetResourceReference(Button.StyleProperty, "AyDateBoxCalendar.Button");
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

            GridService.SetColumns(RootGrid, "? ? ? ? ? ? ?");
            GridService.SetRows(RootGrid, "? ? ? ? ?");
            int _1 = 1;
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (_1 < 32)
                    {
                        var _11 = CreateButtons(_1.ToString(), _1);
                        GridService.SetRowColumn(_11, j + " " + i);
                        _1++;
                        RootGrid.Children.Add(_11);
                    }
                }
            }
        }


    }
}
