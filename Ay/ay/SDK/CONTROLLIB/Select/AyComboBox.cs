
using ay.AyExpression;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;


namespace ay.Controls
{
    /// <summary>
    /// Ay基本下拉框
    /// </summary>
    [TemplatePart(Name = "PART_SearchTextBox", Type = typeof(AyTextBox))]
    [TemplatePart(Name = "PART_ROOT", Type = typeof(Border))]
    //[StyleTypedProperty(Property = "AyFormInputStyle", StyleTargetType = typeof(AyFormInput))]
    public class AyComboBox : ComboBox, IAyValidate, IAyHighlight, IAyControl, IControlPlaceholder
    {
        public AyComboBox()
        {
            Loaded += AyComboBox_Loaded;
        }
        public static readonly DependencyProperty IsKeepPlaceholderProperty;

        public static readonly DependencyProperty PlaceholderProperty;

        public static readonly DependencyProperty PlaceholderTemplateProperty;

        /// <summary>
        /// 当获得键盘焦点时候，是否保持水印
        /// </summary>
        public bool IsKeepPlaceholder
        {
            get
            {
                return (bool)GetValue(IsKeepPlaceholderProperty);
            }
            set
            {
                SetValue(IsKeepPlaceholderProperty, value);
            }
        }
        /// <summary>
        /// 水印
        /// </summary>
		public object Placeholder
        {
            get
            {
                return GetValue(PlaceholderProperty);
            }
            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }
        /// <summary>
        /// 水印模板
        /// </summary>
		public DataTemplate PlaceholderTemplate
        {
            get
            {
                return (DataTemplate)GetValue(PlaceholderTemplateProperty);
            }
            set
            {
                SetValue(PlaceholderTemplateProperty, value);
            }
        }

        /// <summary>
        /// 左侧内容
        /// </summary>
        public object LeftContent
        {
            get { return (object)GetValue(LeftContentProperty); }
            set { SetValue(LeftContentProperty, value); }
        }

        public static readonly DependencyProperty LeftContentProperty =
            DependencyProperty.Register("LeftContent", typeof(object), typeof(AyComboBox), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// 右侧内容
        /// </summary>
        public object RightContent
        {
            get { return (object)GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register("RightContent", typeof(object), typeof(AyComboBox), new FrameworkPropertyMetadata(null));


        static AyComboBox()
        {
            IsKeepPlaceholderProperty = DependencyProperty.Register("IsKeepPlaceholder", typeof(bool), typeof(AyComboBox), new UIPropertyMetadata(true));
            PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(object), typeof(AyComboBox), new UIPropertyMetadata(null));
            PlaceholderTemplateProperty = DependencyProperty.Register("PlaceholderTemplate", typeof(DataTemplate), typeof(AyComboBox), new UIPropertyMetadata(null));
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AyComboBox), new FrameworkPropertyMetadata(typeof(AyComboBox)));
        }

        public string ControlID { get { return ControlGUID.AyComboBox; } }

        public Brush PanelBackground
        {
            get { return (Brush)GetValue(PanelBackgroundProperty); }
            set { SetValue(PanelBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PanelBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanelBackgroundProperty =
            DependencyProperty.Register("PanelBackground", typeof(Brush), typeof(AyComboBox), new PropertyMetadata(Brushes.White));


        public Brush PanelBorderBrush
        {
            get { return (Brush)GetValue(PanelBorderBrushProperty); }
            set { SetValue(PanelBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PanelBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanelBorderBrushProperty =
            DependencyProperty.Register("PanelBorderBrush", typeof(Brush), typeof(AyComboBox), new PropertyMetadata(Brushes.Transparent));



        public Brush CaretBrush
        {
            get { return (Brush)GetValue(CaretBrushProperty); }
            set { SetValue(CaretBrushProperty, value); }
        }

        public static readonly DependencyProperty CaretBrushProperty =
            DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(AyComboBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));


        private void AyComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyComboBox_Loaded;

            if (!WpfTreeHelper.IsInDesignMode)
            {
                if (!Rule.IsNullAndTrimAndEmpty())
                {
                    CreatePopupEx();

                    if (editBox.IsNotNull())
                    {

                        editBox.TextChanged += TextBox_TextChanged;
               
                        //this.MouseLeave += Txt_MouseLeave;
                    }

                }

                if (editBox.IsNotNull())
                {
                    editBox.KeyUp += (sender2, arg2) =>
                   {
                        if ((arg2.Key == Key.Back && editBox.Text.Length == 0) || (arg2.Key == Key.Left && editBox.CaretIndex ==0))
                        {
                            IsDropDownOpen = true;
                            return;
                        }
                        arg2.Handled = true;

                    };
                }
                //if (searchBox.IsNotNull() && SearchBoxVisibility == Visibility.Visible)
                //{
                //    searchBox.TextChanged += SearchBox_TextChanged;
                //}
                this.LostKeyboardFocus += AyComboBox_LostKeyboardFocus;
                if (targetTextBox.IsNotNull())
                {
                    apErrorToolTip.PlacementTarget = targetTextBox;
                }
            }

            var _isInAyLayer = this.GetVisualAncestor<AyLayer>();
            if (_isInAyLayer.IsNotNull())
            {
                _isInAyLayer.DragTitleBarStart += DragTitleBarWhen;
                this.Unloaded += (fe, er) =>
                {
                    _isInAyLayer.DragTitleBarStart -= DragTitleBarWhen;
                };
            }
            this.KeyDown -= AyComboBox_KeyDown;
            this.KeyDown += AyComboBox_KeyDown;
        }

        private void AyComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                IsDropDownOpen = !IsDropDownOpen;
            }
        }

        private void AyComboBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            IsHighlight = false;
        }

        //private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var _filterView = CollectionViewSource.GetDefaultView(this.ItemsSource);
        //    _filterView.Filter = new Predicate<object>((itemObj) =>
        //    {
        //        IAyComboBoxSupportSearch d = itemObj as IAyComboBoxSupportSearch;
        //        return d.SearchText.IndexOf(searchBox.Text) >= 0;
        //    });
        //}

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BeginTextBoxValidate(sender);
        }
        //private void Txt_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    var _1isview = FormHelper.GetIsViewMode(this);
        //    if (!_1isview)
        //    {
        //        if (editBox.IsNotNull() && !editBox.IsKeyboardFocused)
        //        {
        //            editBox.IsError = false;
        //            apErrorToolTip.IsOpen = false;
        //            e.Handled = true;
        //        }
        //    }
        //}



        public AyTextBox searchBox = null;

    

        public AyTextBox editBox = null;

        Border targetTextBox = null;

        public override void OnApplyTemplate()
        {
            targetTextBox = Template.FindName("PART_ROOT", this) as Border;
            //if (popup != null)
            //{
            //    popup.Opened += popup_Opened;
            //}

            editBox = Template.FindName("PART_EditableTextBox", this) as AyTextBox;
            searchBox = Template.FindName("PART_SearchTextBox", this) as AyTextBox;

            if (AfterSelectBehavior == ComboAfterSelectBehavior.UnSelectAll)
            {
                this.SelectionChanged += AyComboBox_SelectionChanged;
                //this.DropDownClosed += AyComboBox_DropDownClosed;

            }
            base.OnApplyTemplate();

        }
        #region 2016-9-19 21:52:17 增加复杂下拉，选中模板


        //public DataTemplate AySelectionBoxItemTemplate
        //{
        //    get { return (DataTemplate)GetValue(AySelectionBoxItemTemplateProperty); }
        //    set { SetValue(AySelectionBoxItemTemplateProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for AySelectionBoxItemTemplate.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty AySelectionBoxItemTemplateProperty =
        //    DependencyProperty.Register("AySelectionBoxItemTemplate", typeof(DataTemplate), typeof(AyComboBox), new PropertyMetadata(null));



        public string SearchBoxMask
        {
            get { return (string)GetValue(SearchBoxMaskProperty); }
            set { SetValue(SearchBoxMaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchBoxMask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchBoxMaskProperty =
            DependencyProperty.Register("SearchBoxMask", typeof(string), typeof(AyComboBox), new PropertyMetadata("查找..."));


        /// <summary>
        /// 下拉框中的SearchBox是否可见
        /// </summary>
        public Visibility SearchBoxVisibility
        {
            get { return (Visibility)GetValue(SearchBoxVisibilityProperty); }
            set { SetValue(SearchBoxVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchBoxVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchBoxVisibilityProperty =
            DependencyProperty.Register("SearchBoxVisibility", typeof(Visibility), typeof(AyComboBox), new PropertyMetadata(Visibility.Collapsed));




        public new bool IsInputMethodEnabled
        {
            get { return (bool)GetValue(IsInputMethodEnabledProperty); }
            set { SetValue(IsInputMethodEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsInputMethodEnabledProperty =
            DependencyProperty.Register("IsInputMethodEnabled", typeof(bool), typeof(AyComboBox), new PropertyMetadata(true));


        #endregion
        private void AyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AyComboBox cb = sender as AyComboBox;
            if (cb != null && cb.IsEditable == true && cb.IsDropDownOpen)
            {
                //cb.IsEditable = false;
                //cb.IsEditable = true;
                if (editBox != null)
                {
                    if (editBox.IsFocused)
                    {
                        cb.IsEditable = false;
                        cb.IsEditable = true;
                        cb.Focus();
                    }

                }

            }
        }


        public ComboAfterSelectBehavior AfterSelectBehavior
        {
            get { return (ComboAfterSelectBehavior)GetValue(AfterSelectProperty); }
            set { SetValue(AfterSelectProperty, value); }
        }

        public static readonly DependencyProperty AfterSelectProperty =
            DependencyProperty.Register("AfterSelectBehavior", typeof(ComboAfterSelectBehavior), typeof(AyComboBox), new PropertyMetadata(ComboAfterSelectBehavior.SelectAll));


        /// <summary>
        /// 下拉区域最小高度
        /// </summary>
        public double PanelMinHeight
        {
            get { return (double)GetValue(PanelMinHeightProperty); }
            set { SetValue(PanelMinHeightProperty, value); }
        }

        public static readonly DependencyProperty PanelMinHeightProperty =
            DependencyProperty.Register("PanelMinHeight", typeof(double), typeof(AyComboBox), new PropertyMetadata(0.00));


        //void popup_Opened(object sender, EventArgs e)
        //{
        //    var p = sender as Popup;
        //    if (p != null)
        //    {
        //        popup.Placement = PlacementMode.Bottom;
        //        UIElement container = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(p)) as UIElement;
        //        Point relativeLocation = p.Child.TranslatePoint(new Point(0, 0), container);
        //        if (relativeLocation.X < 0.5 && relativeLocation.X >= 0)
        //        {
        //            if (relativeLocation.Y < 0)
        //            {
        //                popup.Placement = PlacementMode.Top;
        //            }
        //            else
        //            {
        //                popup.Placement = PlacementMode.Bottom;
        //            }

        //        }
        //        else if (relativeLocation.X < 0)
        //        {
        //            popup.Placement = PlacementMode.Left;
        //        }
        //        else if (relativeLocation.X > 0.5)
        //        {

        //            popup.Placement = PlacementMode.Right;
        //        }

        //        //Console.WriteLine(relativeLocation.X + " , " + relativeLocation.Y);

        //    }
        //}


        #region 拓展验证
        public bool IsTextValidate()
        {
            //var _1isview = FormHelper.GetIsViewMode(this);
            //if (!_1isview)
            //{
            if (editBox.IsNotNull())
            {
                if (editBox.Text.Length > 0)
                {
                    return true;
                }

            }
            else
            {
                if (IsFirstItemInvalid && this.SelectedIndex <= 0)
                {
                    return false;
                }
                return true;
                //if (!IsFirstItemInvalid && this.SelectedIndex < 0)
                //{
                //    return true;
                //}
            }
            //}
            return false;
        }


        /// <summary>
        /// 验证统一接口
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            //var _1isview = FormHelper.GetIsViewMode(this);
            //if (!_1isview)
            //{
            if (editBox.IsNotNull())
            {
                ValdiateTextBox(editBox);
                return !editBox.IsError;
            }
            else
            {
                if (IsFirstItemInvalid && this.SelectedIndex <= 0)
                {
                    ErrorInfo = AyFormErrorTemplate.Required;
                    apErrorToolTip.IsOpen = true;
                    return false;
                }
                apErrorToolTip.IsOpen = false;
                return true;

            }
            //}
            //return false;
        }

        public bool ValidateButNotShowError()
        {
            if (editBox.IsNotNull())
            {
                ValdiateTextBoxNotTip(editBox);
                return !editBox.IsError;
            }
            else
            {
                if (IsFirstItemInvalid && this.SelectedIndex <= 0)
                {
                    ErrorInfo = AyFormErrorTemplate.Required;
                    return false;
                }
                return true;
            }
        }
        public void ShowError()
        {
            apErrorToolTip.IsOpen = true;
        }

        /// <summary>
        /// 规则：AY表达式
        /// </summary>
        public string Rule
        {
            get { return (string)GetValue(RuleProperty); }
            set { SetValue(RuleProperty, value); }
        }

        public static readonly DependencyProperty RuleProperty =
            DependencyProperty.Register("Rule", typeof(string), typeof(AyComboBox), new PropertyMetadata(null));


        private static FrameworkPropertyMetadata ErrorInfoMetadata =
                new FrameworkPropertyMetadata(
                  "",     // Default value
                  FrameworkPropertyMetadataOptions.AffectsRender,
                  null,    // Property changed callback
                  null);
        public string ErrorInfo
        {
            get { return (string)GetValue(ErrorInfoProperty); }
            set { SetValue(ErrorInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorInfoProperty =
            DependencyProperty.Register("ErrorInfo", typeof(string), typeof(AyComboBox), ErrorInfoMetadata);
        bool bindapErrorToolTip = false;
        ToolTip _apErrorToolTip = null;
        public ToolTip apErrorToolTip
        {
            get
            {
                if (_apErrorToolTip.IsNull())
                {
                    CreatePopupEx();
                }
                if (!bindapErrorToolTip)
                {
                    Window w = Window.GetWindow(this);

                    if (null != w && !bindapErrorToolTip)
                    {
                        bindapErrorToolTip = true;
                        w.LocationChanged += delegate (object sender2, EventArgs args)
                        {
                            var offset = _apErrorToolTip.HorizontalOffset;
                            _apErrorToolTip.HorizontalOffset = offset + 1;
                            _apErrorToolTip.HorizontalOffset = offset;
                            UpdateToolTipStyle();

                        };
                        var _isInPage = this.GetVisualAncestor<Page>();
                        if (_isInPage.IsNotNull())
                        {
                            _isInPage.IsVisibleChanged += (esd, earg) =>
                            {
                                if (((bool)earg.NewValue))
                                {

                                }
                                else
                                {
                                    _apErrorToolTip.IsOpen = false;
                                }
                            };
                            _isInPage.Unloaded += (esd, earg) =>
                            {
                                _apErrorToolTip.IsOpen = false;
                            };
                        }
                        else
                        {
                            var _2 = w.GetVisualAncestor<ContentPresenter>();
                            if (_2.IsNotNull())
                            {
                                _2.IsVisibleChanged += (btnSender, ew) =>
                                {
                                    if ((bool)ew.NewValue)
                                    {

                                    }
                                    else
                                    {
                                        _apErrorToolTip.IsOpen = false;

                                    }
                                };
                            }

                            w.IsVisibleChanged += (esd, earg) =>
                            {
                                if (((bool)earg.NewValue))
                                {

                                }
                                else
                                {
                                    _apErrorToolTip.IsOpen = false;
                                }
                            };
                            w.Unloaded += (esd, earg) =>
                            {
                                _apErrorToolTip.IsOpen = false;
                            };
                        }

                        w.SizeChanged += delegate (object sender3, SizeChangedEventArgs e2)
                        {
                            var offset = _apErrorToolTip.HorizontalOffset;
                            _apErrorToolTip.HorizontalOffset = offset + 1;
                            _apErrorToolTip.HorizontalOffset = offset;
                        };
                    }
                }
                return _apErrorToolTip;
            }
            set { _apErrorToolTip = value; }
        }

        AyTooltip at = null;


        private void CreatePopupEx()
        {
            if (_apErrorToolTip.IsNull())
            {
                _apErrorToolTip = new ToolTip();
                _apErrorToolTip.MinHeight = this.Height == double.NaN ? this.Height : 0;
                _apErrorToolTip.BorderThickness = new Thickness(0);
                _apErrorToolTip.Background = new SolidColorBrush(Colors.Transparent);
                _apErrorToolTip.Padding = new Thickness(0);
                _apErrorToolTip.Placement = PlacementMode.Right;
                _apErrorToolTip.Padding = new Thickness(10, 0, 0, 0);
                _apErrorToolTip.HorizontalOffset = 0;
                _apErrorToolTip.VerticalOffset = 0;
                _apErrorToolTip.Opened += ErrorPopup_Opened;
                _apErrorToolTip.PlacementTarget = this;
                _apErrorToolTip.VerticalContentAlignment = VerticalAlignment.Center;

                this.LostKeyboardFocus += this_LostKeyboardFocus;
                this.GotKeyboardFocus += this_GotKeyboardFocus;
                if (at.IsNull())
                {
                    at = new AyTooltip();

                    Binding _at1 = new Binding { Path = new PropertyPath("AyToolTipForeground"), Source = this, Mode = BindingMode.TwoWay };
                    Binding _at2 = new Binding { Path = new PropertyPath("AyToolTipBackground"), Source = this, Mode = BindingMode.TwoWay };
                    Binding _at3 = new Binding { Path = new PropertyPath("AyToolTipBorderBrush"), Source = this, Mode = BindingMode.TwoWay };
                    at.SetBinding(AyTooltip.BorderBrushProperty, _at3);
                    at.SetBinding(AyTooltip.ForegroundProperty, _at1);
                    at.SetBinding(AyTooltip.BackgroundProperty, _at2);
                    at.Placement = Dock.Left;
                    at.SetBinding(AyTooltip.TooltipContentProperty, new Binding { Path = new PropertyPath("ErrorInfo"), Source = this, Mode = BindingMode.TwoWay });
                    _apErrorToolTip.Content = at;


                }
            }
        }

        public void ErrorPopup_Opened(object sender, EventArgs e)
        {
            var p = sender as ToolTip;
            if (p != null)
            {
                UpdateToolTipStyle();
            }
        }

        public void UpdateToolTipStyle()
        {
            Point relativeLocation = at.TranslatePoint(new Point(0, 0), this);
            if (relativeLocation.X < 0)
            {
                at.Placement = Dock.Right;
                apErrorToolTip.Padding = new Thickness(0, 0, 10, 0);
            }
            else if (relativeLocation.X > 0)
            {
                at.Placement = Dock.Left;
                apErrorToolTip.Padding = new Thickness(10, 0, 0, 0);
            }
        }
        private void this_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            AyComboBox _d1 = sender as AyComboBox;
            var _d = _d1.editBox;
            BeginTextBoxValidate(_d);
            e.Handled = true;
        }

        private void this_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            AyComboBox _d1 = sender as AyComboBox;
            var _d = _d1.editBox;
            if (_d != null)
            {
                if (!_d.IsKeyboardFocused)
                {
                    _d.IsError = false;
                    apErrorToolTip.IsOpen = false;


                    e.Handled = true;
                }
            }
        }

        private void BeginTextBoxValidate(object sender)
        {
            AyTextBox _d = sender as AyTextBox;
            if (_d != null)
            {
                ValdiateTextBox(_d);
            }
        }

        /// <summary>
        /// 验证 提示控制
        /// </summary>
        /// <param name="_d"></param>
        private void ValdiateTextBox(AyTextBox _d)
        {

            if (IsTextMustInDropDown)
            {
                if (!_d.Text.IsNullOrWhiteSpace() && this.SelectedItem.IsNull())
                {
                    this.ErrorInfo = AyFormErrorTemplate.ComboBoxInvalidItem;
                    this.apErrorToolTip.IsOpen = true;
                    _d.IsError = true;
                    return;
                }
            }

            if (IsFirstItemInvalid && this.SelectedIndex <= 0)
            {
                ErrorInfo = AyFormErrorTemplate.Required;
                apErrorToolTip.IsOpen = true;
                _d.IsError = true;
                return;
            }


            var _r = _d.Text.ToAyExpressionFormResult(this.Rule);

            if (_r.Result)
            {
                _d.IsError = false;
                apErrorToolTip.IsOpen = false;
                ErrorInfo = string.Empty;
            }
            else
            {
                ErrorInfo = _r.Error;
                apErrorToolTip.IsOpen = true;
                _d.IsError = true;
            }
        }

        private void ValdiateTextBoxNotTip(AyTextBox _d)
        {

            if (IsTextMustInDropDown)
            {
                if (!_d.Text.IsNullOrWhiteSpace() && this.SelectedItem.IsNull())
                {
                    this.ErrorInfo = AyFormErrorTemplate.ComboBoxInvalidItem;
                    _d.IsError = true;
                    return;
                }
            }

            if (IsFirstItemInvalid && this.SelectedIndex <= 0)
            {
                ErrorInfo = AyFormErrorTemplate.Required;
                _d.IsError = true;
                return;
            }


            var _r = _d.Text.ToAyExpressionFormResult(this.Rule);

            if (_r.Result)
            {
                _d.IsError = false;
                ErrorInfo = string.Empty;
            }
            else
            {
                ErrorInfo = _r.Error;
                _d.IsError = true;
            }
        }


        #endregion
        #region 增加高亮需求 2016-11-25 14:53:17

        /// <summary>
        /// 是否高亮，默认不是
        /// </summary>
        public bool IsHighlight
        {
            get { return (bool)GetValue(IsHighlightProperty); }
            set { SetValue(IsHighlightProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightProperty =
            DependencyProperty.Register("IsHighlight", typeof(bool), typeof(AyComboBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnIsHighlightChanged)));

        private static void OnIsHighlightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyComboBox).SetOnIsHighlightChanged((bool)e.OldValue, (bool)e.NewValue);
        }
        //高亮以后，必须先获得焦点，然后失去焦点，才能自动IsHighlight=false
        bool isOpenHighlightThenFocus = false;
        public void SetOnIsHighlightChanged(bool oldv, bool newv)
        {
            if (newv)
            {
                if (editBox.IsNotNull())
                {
                    editBox.GotKeyboardFocus -= This_Highlight_GotKeyboardFocus;
                    editBox.GotKeyboardFocus += This_Highlight_GotKeyboardFocus;
                    editBox.LostKeyboardFocus -= This_Highlight_LostKeyboardFocus;
                    editBox.LostKeyboardFocus += This_Highlight_LostKeyboardFocus;
                }
                else
                {
                    this.GotFocus -= AyComboBox_GotFocus;
                    this.GotFocus += AyComboBox_GotFocus;
                    this.LostFocus -= AyComboBox_LostFocus;
                    this.LostFocus += AyComboBox_LostFocus;

                }
            }
            else
            {
                if (editBox.IsNotNull())
                {
                    editBox.GotKeyboardFocus -= This_Highlight_GotKeyboardFocus;
                    editBox.LostKeyboardFocus -= This_Highlight_LostKeyboardFocus;
                }
                else
                {
                    this.GotFocus -= AyComboBox_GotFocus;
                    this.LostFocus -= AyComboBox_LostFocus;
                }

            }
        }

        private void AyComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (isOpenHighlightThenFocus)
            {
                IsHighlight = false;
                isOpenHighlightThenFocus = false;
            }
        }

        private void AyComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IsHighlight)
            {
                isOpenHighlightThenFocus = true;
            }
        }

        private void This_Highlight_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (isOpenHighlightThenFocus)
            {
                IsHighlight = false;
                isOpenHighlightThenFocus = false;
            }
        }

        private void This_Highlight_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (IsHighlight)
            {
                isOpenHighlightThenFocus = true;
            }
        }
        public void HighlightElement()
        {
            IsHighlight = true;
        }
        #endregion

        #region AyToolTip设置
        public void DragTitleBarWhen()
        {
            this.apErrorToolTip.IsOpen = false;
        }

        /// <summary>
        /// 输入的值是不是必须在下拉框里面的，这里不考虑选中后，把文字删掉几个，combobox取值的时候是选中的对象，不要相信Text中的
        /// 作者：A Y
        /// 作者：Aaronyang
        /// 2016-10-07 17:13:50
        /// </summary>
        public bool IsTextMustInDropDown
        {
            get { return (bool)GetValue(IsTextMustInDropDownProperty); }
            set { SetValue(IsTextMustInDropDownProperty, value); }
        }

        public static readonly DependencyProperty IsTextMustInDropDownProperty =
            DependencyProperty.Register("IsTextMustInDropDown", typeof(bool), typeof(AyComboBox), new PropertyMetadata(false));


        /// <summary>
        /// 第一项是否是空项目,如果是空，表示如果用户是required的，如果用户选了第一项，会验证失败的。提示不能为空
        /// 2016-10-03 19:48:01
        /// </summary>
        public bool IsFirstItemInvalid
        {
            get { return (bool)GetValue(IsFirstItemInvalidProperty); }
            set { SetValue(IsFirstItemInvalidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFirstItemIsInvalidItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFirstItemInvalidProperty =
            DependencyProperty.Register("IsFirstItemInvalid", typeof(bool), typeof(AyComboBox), new PropertyMetadata(false));




        public Brush AyToolTipBackground
        {
            get { return (Brush)GetValue(AyToolTipBackgroundProperty); }
            set { SetValue(AyToolTipBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipBackgroundProperty =
            DependencyProperty.Register("AyToolTipBackground", typeof(Brush), typeof(AyComboBox), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFFFF4AC")));



        public Brush AyToolTipForeground
        {
            get { return (Brush)GetValue(AyToolTipForegroundProperty); }
            set { SetValue(AyToolTipForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipForegroundProperty =
            DependencyProperty.Register("AyToolTipForeground", typeof(Brush), typeof(AyComboBox), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFE25D5D")));



        public Brush AyToolTipBorderBrush
        {
            get { return (Brush)GetValue(AyToolTipBorderBrushProperty); }
            set { SetValue(AyToolTipBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipBorderBrushProperty =
            DependencyProperty.Register("AyToolTipBorderBrush", typeof(Brush), typeof(AyComboBox), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFECA50D")));


        #endregion




        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(AyComboBox), new PropertyMetadata(int.MaxValue));


    }

}
