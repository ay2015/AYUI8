using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using ay.contentcore;
using ay.AyExpression;

namespace ay.Controls
{
    /// <summary>
    /// 默认普通文本框
    /// 文本框：设置Rule，支持实时验证，设置MaskExpression，输入放大
    /// 文本框提示默认跟随window移动的
    /// 多行文本 设置：IsMultiply="True" 设置水印换行，在Mask属性输入&#x0a;
    /// 密码框使用和绑定：设置 IsPasswordBox="True" PasswordStr="{Binding Password}"
    /// </summary>
    [TemplatePart(Name = "PART_ROOT", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_PlusRepeatButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_MinusRepeatButton", Type = typeof(RepeatButton))]
    public partial class AyFormInput : AyTextBox, IAyValidate, IAyHighlight
    {
        public AyPopKeyBoard PopKeyBoard
        {
            get { return (AyPopKeyBoard)GetValue(PopKeyBoardProperty); }
            set { SetValue(PopKeyBoardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopKeyBoard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopKeyBoardProperty =
            DependencyProperty.Register("PopKeyBoard", typeof(AyPopKeyBoard), typeof(AyFormInput), new PropertyMetadata(null, new PropertyChangedCallback(OnPopKeyBoard)));

        private static void OnPopKeyBoard(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = d as AyFormInput;
            if (_1.IsNotNull())
            {
                AyPopKeyBoard _2 = e.NewValue as AyPopKeyBoard;
                if (_2 != null)
                {
                    _2.InitInputChild(_1);

                }
            }
        }

        public override bool Validate()
        {
            ValdiateTextBox(this);
            return !this.IsError;
        }
        public override bool ValidateButNotShowError()
        {
            ValdiateTextBoxNotShowError(this);
            return !this.IsError;
        }
        public override void ShowError()
        {
            apErrorToolTip.IsOpen = true;
        }

        public override void HighlightElement()
        {
            IsHighlight = true;
        }

        public new string ControlID { get { return ControlGUID.AyFormInput; } }
        //public AyPopKeyBoard PopKeyBoard
        //{
        //    get { return (AyPopKeyBoard)GetValue(PopKeyBoardProperty); }
        //    set { SetValue(PopKeyBoardProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PopKeyBoard.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PopKeyBoardProperty =
        //    DependencyProperty.Register("PopKeyBoard", typeof(AyPopKeyBoard), typeof(AyFormInput), new PropertyMetadata(null, new PropertyChangedCallback(OnPopKeyBoard)));

        //private static void OnPopKeyBoard(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var _1 = d as AyFormInput;
        //    if (_1.IsNotNull())
        //    {
        //        AyPopKeyBoard _2 = e.NewValue as AyPopKeyBoard;
        //        if (_2 != null)
        //        {
        //            _2.InitInputChild(_1);

        //        }
        //    }
        //}
        ContentPresenter mask = null;
        public AyFormInput()
        {
            Loaded += AyFormInput_Loaded;
            //if (!DesignerProperties.GetIsInDesignMode(this))
            //{
            //    SetResourceReference(AyTextBox.BorderBrushProperty, "Ay.Brush14");
            //    //this.BorderThickness = new Thickness(1);
            //    //SetResourceReference(AyTextBox.CaretBrushProperty, "Ay.Brush5");
            //    if (this.FontSize == double.NaN)
            //        this.FontSize = 14;

            //    this.Padding = new Thickness(2, 0, 0, 0);
            //}
            //else
            //{
            //    this.BorderBrush = new SolidColorBrush(Colors.Blue);
            //    this.AyTextCorner = new CornerRadius(0);
            //    this.BorderThickness = new Thickness(1);
            //    this.FontSize = 14;
            //}
        }

        Grid targetTextBox = null;
        RepeatButton plusRepeatButton = null;
        RepeatButton minusRepeatButton = null;
        public override void OnApplyTemplate()
        {
            targetTextBox = Template.FindName("PART_ROOT", this) as Grid;
            plusRepeatButton = Template.FindName("PART_PlusRepeatButton", this) as RepeatButton;
            minusRepeatButton = Template.FindName("PART_MinusRepeatButton", this) as RepeatButton;
            mask = Template.FindName("PART_PlaceholderHost", this) as ContentPresenter;

            if (minusRepeatButton != null)
            {
                if (IsNumberBox || IsIntegerBox)
                {
                    minusRepeatButton.Click += Rb2_Click;
                }
            }
            if (plusRepeatButton != null)
            {
                if (IsNumberBox || IsIntegerBox)
                {
                    plusRepeatButton.Click += Rb_Click;
                }
            }
            base.OnApplyTemplate();
        }

        ToolTip _apMaskToolTip = null;
        bool bindParentMask = false;
        public ToolTip apMaskToolTip
        {
            get
            {
                if (_apMaskToolTip.IsNull())
                {
                    CreateZoomPopupEx();
                }
                if (!bindParentMask)
                {
                    Window w = Window.GetWindow(this);
                    if (null != w && !bindParentMask)
                    {
                        bindParentMask = true;
                        w.LocationChanged += delegate (object sender2, EventArgs args)
                        {
                            var offset = _apMaskToolTip.HorizontalOffset;
                            _apMaskToolTip.HorizontalOffset = offset + 1;
                            _apMaskToolTip.HorizontalOffset = offset;

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
                                    _apMaskToolTip.IsOpen = false;
                                }
                            };
                            _isInPage.Unloaded += (esd, earg) =>
                            {
                                _apMaskToolTip.IsOpen = false;
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
                                        _apMaskToolTip.IsOpen = false;

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
                                    _apMaskToolTip.IsOpen = false;
                                }
                            };
                            w.Unloaded += (esd, earg) =>
                            {
                                _apMaskToolTip.IsOpen = false;
                            };
                        }
                        w.SizeChanged += delegate (object sender3, SizeChangedEventArgs e2)
                        {
                            var offset = _apMaskToolTip.HorizontalOffset;
                            _apMaskToolTip.HorizontalOffset = offset + 1;
                            _apMaskToolTip.HorizontalOffset = offset;
                        };
                    }

                }
                return _apMaskToolTip;
            }
            set { _apMaskToolTip = value; }
        }

        ToolTip _apErrorToolTip = null;
        bool bindapErrorToolTip = false;
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


        /// <summary>
        /// 2018-1-25 13:19:43 AY 修复 移动 mask的bug
        /// </summary>
        public void DragTitleBarWhen()
        {
            if (this._apErrorToolTip.IsNotNull())
                this.apErrorToolTip.IsOpen = false;
            if (this._apMaskToolTip.IsNotNull())
                this.apMaskToolTip.IsOpen = false;
        }

        AyTooltip at = null;

        #region 创建1个popupex  2016年8月4日10:12:03 ayjs.net  杨洋    AY
        private void CreatePopupEx()
        {
            if (_apErrorToolTip.IsNull())
            {
                _apErrorToolTip = new ToolTip();
                this.IsVisibleChanged += AyFormInput_IsVisibleChanged;
                _apErrorToolTip.MinHeight = this.Height == double.NaN ? this.Height : 0;
                _apErrorToolTip.BorderThickness = new Thickness(0);
                _apErrorToolTip.Background = new SolidColorBrush(Colors.Transparent);

                _apErrorToolTip.Padding = new Thickness(0);
                _apErrorToolTip.Placement = PlacementMode.Right;
                _apErrorToolTip.Padding = new Thickness(10, 0, 0, 0);
                _apErrorToolTip.HorizontalOffset = 0;
                _apErrorToolTip.VerticalOffset = 0;
                _apErrorToolTip.Opened += popup_Opened;
                _apErrorToolTip.PlacementTarget = this;

                _apErrorToolTip.VerticalContentAlignment = VerticalAlignment.Center;

                this.LostKeyboardFocus += this_LostKeyboardFocus;
                //this.GotKeyboardFocus += this_GotKeyboardFocus;
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

        private void AyFormInput_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AyFormInput a = sender as AyFormInput;
            var _1 = (bool)e.NewValue;
            if (!_1)
            {
                apErrorToolTip.IsOpen = false;
            }
        }

        AyText tb = null;
        private void CreateZoomPopupEx()
        {
            if (_apMaskToolTip.IsNull())
            {

                _apMaskToolTip = new ToolTip();
                _apMaskToolTip.Background = new SolidColorBrush(Colors.Transparent);


                _apMaskToolTip.BorderThickness = new Thickness(0);
                _apMaskToolTip.Padding = new Thickness(0);
                _apMaskToolTip.Placement = PlacementMode.Top;
                _apMaskToolTip.Padding = new Thickness(0);
                _apMaskToolTip.HorizontalOffset = 0;
                _apMaskToolTip.VerticalOffset = -6;

                _apMaskToolTip.MinHeight = 38;
                Binding _at1 = new Binding { Path = new PropertyPath("AyToolTipForeground"), Source = this, Mode = BindingMode.TwoWay };
                Binding _at2 = new Binding { Path = new PropertyPath("AyToolTipBackground"), Source = this, Mode = BindingMode.TwoWay };
                Binding _at3 = new Binding { Path = new PropertyPath("AyToolTipBorderBrush"), Source = this, Mode = BindingMode.TwoWay };

                tb = new AyText();
                tb.SetBinding(TextBlock.ForegroundProperty, _at1);
                Binding _at4 = new Binding { Path = new PropertyPath("MaskExpressionFontSize"), Source = this, Mode = BindingMode.TwoWay };
                tb.SetBinding(TextBlock.FontSizeProperty, _at4);
                tb.FontWeight = FontWeights.Bold;
                Border bd = new Border();
                bd.Child = tb;
                bd.SetBinding(Border.BorderBrushProperty, _at3);
                bd.BorderThickness = new Thickness(1);
                bd.Padding = new Thickness(10, 5, 10, 5);
                bd.CornerRadius = new CornerRadius(3);
                bd.SetBinding(Border.BackgroundProperty, _at2);
                _apMaskToolTip.Content = bd;

                this.GotKeyboardFocus += this_MaskGotKeyboardFocus;

            }
        }

        private void this_MaskGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ShowMaskToolTip(sender);
        }

        private void ShowMaskToolTip(object sender)
        {
            AyFormInput _d = sender as AyFormInput;
            if (_d != null)
            {
                string text = IsPasswordBox ? _d.Password : _d.Text;
                if (!text.IsNullOrWhiteSpace())
                {
                    var _r = text.ToAyExpressionValue(this.MaskExpression);
                    tb.Text = _r;
                    if (this.IsKeyboardFocused || this.IsFocused)
                    {
                        apMaskToolTip.IsOpen = true;
                    }
                }
                else
                {
                    tb.Text = null;
                    apMaskToolTip.IsOpen = false;
                }
            }
        }

        //private void this_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    BeginTextBoxValidate(sender);
        //}

        private void this_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            AyTextBox _d = sender as AyTextBox;
            if (_d != null)
            {
                if (!_d.IsKeyboardFocused)
                {
                    _d.IsError = false;
                    apErrorToolTip.IsOpen = false;
                    if (!MaskExpression.IsNullOrWhiteSpace())
                    {
                        apMaskToolTip.IsOpen = false;
                    }
                }
                FormatNumberWhenIsNumberBox();
            }
        }

        public void FormatNumberWhenIsNumberBox()
        {
            if (IsNumberBox && FormatNumber != null)
            {
                var _33 = Text.ToDouble();
                Text = string.Format("{0:" + FormatNumber + "}", _33);
            }
        }

        void popup_Opened(object sender, EventArgs e)
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
        #endregion

        #region AyToolTip设置


        public Brush AyToolTipBackground
        {
            get { return (Brush)GetValue(AyToolTipBackgroundProperty); }
            set { SetValue(AyToolTipBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipBackgroundProperty =
            DependencyProperty.Register("AyToolTipBackground", typeof(Brush), typeof(AyFormInput), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFFFF4AC")));



        public Brush AyToolTipForeground
        {
            get { return (Brush)GetValue(AyToolTipForegroundProperty); }
            set { SetValue(AyToolTipForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipForegroundProperty =
            DependencyProperty.Register("AyToolTipForeground", typeof(Brush), typeof(AyFormInput), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFE25D5D")));



        public Brush AyToolTipBorderBrush
        {
            get { return (Brush)GetValue(AyToolTipBorderBrushProperty); }
            set { SetValue(AyToolTipBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipBorderBrushProperty =
            DependencyProperty.Register("AyToolTipBorderBrush", typeof(Brush), typeof(AyFormInput), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFECA50D")));


        #endregion

        #region 输入时候 掩码大提示
        /// <summary>
        /// 字号
        /// </summary>
        public double MaskExpressionFontSize
        {
            get { return (double)GetValue(MaskExpressionFontSizeProperty); }
            set { SetValue(MaskExpressionFontSizeProperty, value); }
        }

        public static readonly DependencyProperty MaskExpressionFontSizeProperty =
            DependencyProperty.Register("MaskExpressionFontSize", typeof(double), typeof(AyFormInput), new PropertyMetadata(24.00));


        /// <summary>
        /// 输入时候 掩码大提示的表达式
        /// </summary>
        public string MaskExpression
        {
            get { return (string)GetValue(MaskExpressionProperty); }
            set { SetValue(MaskExpressionProperty, value); }
        }

        public static readonly DependencyProperty MaskExpressionProperty =
            DependencyProperty.Register("MaskExpression", typeof(string), typeof(AyFormInput), new PropertyMetadata(null, onMaskExpressionChanged));

        private static void onMaskExpressionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AyFormInput _1 = d as AyFormInput;
            if (_1.IsNotNull())
            {
                _1.InitMaskExpression();

            }

        }

        #endregion
        /// <summary>
        /// 规则：AY表达式
        /// </summary>
        public string Rule
        {
            get { return (string)GetValue(RuleProperty); }
            set { SetValue(RuleProperty, value); }
        }

        public static readonly DependencyProperty RuleProperty =
            DependencyProperty.Register("Rule", typeof(string), typeof(AyFormInput), new PropertyMetadata(null, onRuleChanged));

        private static void onRuleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AyFormInput _1 = d as AyFormInput;
            if (_1.IsNotNull())
                _1.InitAyValidate();
        }


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
            DependencyProperty.Register("ErrorInfo", typeof(string), typeof(AyFormInput), ErrorInfoMetadata);

        public bool IsMultiply
        {
            get { return (bool)GetValue(IsMultiplyProperty); }
            set { SetValue(IsMultiplyProperty, value); }
        }



        private static FrameworkPropertyMetadata IsMultiplyMetadata =
  new FrameworkPropertyMetadata(
    false);

        //private static void multiPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    AyFormInput layer = d as AyFormInput;
        //    if (layer.IsNotNull())
        //    {
        //        if (layer.IsLoaded)
        //        {
        //            //bool nValue = (bool)e.NewValue;
        //            layer.OnMultiChanged(layer);
        //        }
        //        else
        //        {
        //            layer.Loaded += layer.Layer_Loaded;
        //        }
        //    }

        //}

        //private void Layer_Loaded(object sender, RoutedEventArgs e)
        //{
        //    AyFormInput layer = sender as AyFormInput;
        //    layer.Loaded -= layer.Layer_Loaded;
        //    layer.OnMultiChanged(layer);
        //}

        //private void OnMultiChanged(AyFormInput layer)
        //{
        //    if (layer.IsNotNull())
        //    {
        //        if (layer.IsMultiply)
        //        {
        //            layer.TextWrapping = TextWrapping.Wrap;
        //            layer.AcceptsReturn = true;
        //            layer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

        //            layer.VerticalContentAlignment = VerticalAlignment.Top;
        //            layer.HorizontalContentAlignment = HorizontalAlignment.Left;

        //            layer.Padding = new Thickness(6,6,6,6);
        //            //if (layer.MaskTextBlock == null)
        //            //{
        //            //    return;
        //            //}
        //            //layer.MaskTextBlock.Margin = new Thickness(12, 6, 6, 6);
        //            //layer.MaskTextBlock.TextWrapping = TextWrapping.Wrap;
        //            //layer.MaskTextBlock.TextTrimming = TextTrimming.None;
        //            //layer.MaskTextBlock.VerticalAlignment = VerticalAlignment.Top;
        //            mask.Margin = new Thickness(16,12,12,12);
        //            double d = layer.FontSize + 10;
        //            //layer.MaskTextBlock.LineHeight = d;
        //            layer.SetValue(TextBlock.LineHeightProperty, d);
        //        }
        //        else
        //        {
        //            layer.TextWrapping = TextWrapping.NoWrap;
        //            layer.AcceptsReturn = false;
        //            layer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        //            layer.VerticalContentAlignment = VerticalAlignment.Center;
        //            layer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        //            layer.Padding = new Thickness(0);
        //            //if (layer.MaskTextBlock == null)
        //            //{
        //            //    return;
        //            //}
        //            //layer.MaskTextBlock.Margin = new Thickness(6, 0, 0, 0);
        //            //layer.MaskTextBlock.TextWrapping = TextWrapping.NoWrap;
        //            //layer.MaskTextBlock.TextTrimming = TextTrimming.CharacterEllipsis;
        //            //layer.MaskTextBlock.VerticalAlignment = VerticalAlignment.Center;

        //            //layer.MaskTextBlock.LineHeight = double.NaN;
        //            layer.SetValue(TextBlock.LineHeightProperty, double.NaN);
        //        }
        //    }
        //}




        // Using a DependencyProperty as the backing store for IsMultiply.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMultiplyProperty =
            DependencyProperty.Register("IsMultiply", typeof(bool), typeof(AyFormInput), IsMultiplyMetadata);




        private void Txt_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //var _1isview = FormHelper.GetIsViewMode(this);
            //if (!_1isview)
            //{
            AyTextBox _d = sender as AyTextBox;
            if (_d != null)
            {
                if (!_d.IsKeyboardFocused)
                {
                    _d.IsError = false;
                    apErrorToolTip.IsOpen = false;
                    e.Handled = true;
                }
            }
            //}
        }

        //private void Txt_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    AyFormInput _d = sender as AyFormInput;
        //    if (_d != null)
        //    {
        //        if (!_d.IsKeyboardFocused)
        //        {
        //            ValdiateTextBox(_d);
        //            e.Handled = true;
        //        }
        //    }
        //}






        private void AyFormInput_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyFormInput_Loaded;
            DataObject.AddPastingHandler(this, OnPaste);
            if (targetTextBox.IsNotNull())
            {
                apErrorToolTip.PlacementTarget = targetTextBox;

            }

            if (MaskExpression.IsNotNull())
            {
                apMaskToolTip.PlacementTarget = targetTextBox;
            }
            FormatNumberWhenIsNumberBox();

            //InitAyValidate();
            //InitMaskExpression();
            InitIsPasswordBox();


            var _isInAyLayer = this.GetVisualAncestor<AyLayer>();
            if (_isInAyLayer.IsNotNull())
            {
                _isInAyLayer.DragTitleBarStart += DragTitleBarWhen;
                this.Unloaded += (fe, er) =>
                {
                    _isInAyLayer.DragTitleBarStart -= DragTitleBarWhen;
                };
            }
        }



        private void InitAyValidate()
        {
            if (!WpfTreeHelper.IsInDesignMode)
            {
                if (!Rule.IsNullAndTrimAndEmpty())
                {
                    CreatePopupEx();
                    this.TextChanged += TextBox_TextChanged;
                    //this.MouseEnter += Txt_MouseEnter;
                    this.MouseLeave += Txt_MouseLeave;
                }
            }
        }

        private void InitMaskExpression()
        {
            if (!WpfTreeHelper.IsInDesignMode)
            {
                if (!MaskExpression.IsNullOrWhiteSpace())
                {
                    CreateZoomPopupEx();
                    apMaskToolTip.PlacementTarget = targetTextBox;
                    this.TextChanged += AyFormInput_MaskTextChanged;
                }
            }

        }

        private void InitIsPasswordBox()
        {
            if (IsPasswordBox)
            {
                IsResponseChange = false;
                Text = ConvertToPasswordChar(Text.ToObjectString().Length);
                IsResponseChange = true;
            }
        }

        private void AyFormInput_MaskTextChanged(object sender, TextChangedEventArgs e)
        {
            ShowMaskToolTip(sender);
        }

        #region 拓展 时间控件公用属性 2017-2-13 14:53:52

        public virtual void UpdateTextWhenDateChange()
        {

        }

        public DateTime? PickedDate
        {
            get { return (DateTime?)GetValue(PickedDateProperty); }
            set { SetValue(PickedDateProperty, value); }
        }
        public static readonly DependencyProperty PickedDateProperty =
            DependencyProperty.Register("PickedDate", typeof(DateTime?), typeof(AyFormInput), new UIPropertyMetadata(null, onPickedDateChanged));

        private static void onPickedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = (d as AyFormInput);
            if (_1 != null)
            {
                _1.UpdateTextWhenDateChange();
            }

        }


        #endregion

        /// <summary>
        /// 验证 提示控制
        /// </summary>
        /// <param name="_d"></param>
        public void ValdiateTextBox(AyFormInput _d)
        {
            string text = IsPasswordBox ? _d.Password : _d.Text;
            var _r = text.ToAyExpressionFormResult(this.Rule);

            if (_r.Result)
            {
                if (!EqualValidateValue.IsNullOrWhiteSpace())
                {
                    if (text != EqualValidateValue)
                    {
                        ErrorInfo = EqualValidateErrorInfo;
                        apErrorToolTip.IsOpen = true;
                        _d.IsError = true;
                        return;
                    }
                }

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

        public void ValdiateTextBoxNotShowError(AyFormInput _d)
        {
            string text = IsPasswordBox ? _d.Password : _d.Text;
            var _r = text.ToAyExpressionFormResult(this.Rule);

            if (_r.Result)
            {
                if (!EqualValidateValue.IsNullOrWhiteSpace())
                {
                    if (text != EqualValidateValue)
                    {
                        ErrorInfo = EqualValidateErrorInfo;
                        _d.IsError = true;
                        return;
                    }
                }

                _d.IsError = false;
                ErrorInfo = string.Empty;
            }
            else
            {
                ErrorInfo = _r.Error;
                _d.IsError = true;
            }
        }
        public double NumberBoxSpinWidth
        {
            get { return (double)GetValue(NumberBoxSpinWidthProperty); }
            set { SetValue(NumberBoxSpinWidthProperty, value); }
        }

        public static readonly DependencyProperty NumberBoxSpinWidthProperty =
            DependencyProperty.Register("NumberBoxSpinWidth", typeof(double), typeof(AyFormInput), new PropertyMetadata(40.00));


        public string NumberBoxSpinAdd
        {
            get { return (string)GetValue(NumberBoxSpinAddProperty); }
            set { SetValue(NumberBoxSpinAddProperty, value); }
        }

        public static readonly DependencyProperty NumberBoxSpinAddProperty =
            DependencyProperty.Register("NumberBoxSpinAdd", typeof(string), typeof(AyFormInput), new UIPropertyMetadata("more_element_Int_Add"));



        public string NumberBoxSpinMinus
        {
            get { return (string)GetValue(NumberBoxSpinMinusProperty); }
            set { SetValue(NumberBoxSpinMinusProperty, value); }
        }

        public static readonly DependencyProperty NumberBoxSpinMinusProperty =
            DependencyProperty.Register("NumberBoxSpinMinus", typeof(string), typeof(AyFormInput), new UIPropertyMetadata("path_element_Int_Minus"));



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BeginTextBoxValidate(sender);
        }

        private void BeginTextBoxValidate(object sender)
        {
            AyFormInput _d = sender as AyFormInput;
            if (_d != null)
            {
                ValdiateTextBox(_d);
            }
        }



        /// <summary>
        /// 是否弹出中文键盘
        /// </summary>
        public bool IsContainChineseKeyboard
        {
            get { return (bool)GetValue(IsContainChineseKeyboardProperty); }
            set { SetValue(IsContainChineseKeyboardProperty, value); }
        }

        public static readonly DependencyProperty IsContainChineseKeyboardProperty =
            DependencyProperty.Register("IsContainChineseKeyboard", typeof(bool), typeof(AyFormInput), new PropertyMetadata(false));

        /// <summary>
        /// 是否是身份证
        /// </summary>
        public bool IsIDCard
        {
            get { return (bool)GetValue(IsIDCardProperty); }
            set { SetValue(IsIDCardProperty, value); }
        }

        public static readonly DependencyProperty IsIDCardProperty =
            DependencyProperty.Register("IsIDCard", typeof(bool), typeof(AyFormInput), new PropertyMetadata(false));


        #region 密码框需求

        /// <summary>
        /// 按照指定的长度生成密码字符
        /// </summary>
        /// <param name="length">字符长度</param>
        /// <returns></returns>
        private string ConvertToPasswordChar(int length)
        {
            return String.Concat(Enumerable.Repeat(PasswordChar, length));
        }

        public static decimal GetNumber(string str)
        {
            decimal result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = decimal.Parse(str);
                }
            }
            return result;
        }
        ///
        /// 获取字符串中的数字
        ///
        /// 字符串
        /// 数字
        public static int GetNumberInt(string str)
        {
            int result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = int.Parse(str);
                }
            }
            return result;
        }

        #region 2016-11-26 14:04:01 控制 当是纯数字模式下的，是否显示加减按钮

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText) return;
            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
            if (IsNumberBox)
            {
                e.CancelCommand();

                FormatNumberBoxElse(GetNumber(text).ToDouble());

                this.CaretIndex = this.Text.Length;
            }
            else if (IsIntegerBox)
            {
                e.CancelCommand();
                this.Text = GetNumberInt(text).ToString();
                this.CaretIndex = this.Text.Length;
            }
            else
            {

            }

        }
        public bool IsShowAddMinusButton
        {
            get { return (bool)GetValue(IsShowAddMinusButtonProperty); }
            set { SetValue(IsShowAddMinusButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowAddMinusButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowAddMinusButtonProperty =
            DependencyProperty.Register("IsShowAddMinusButton", typeof(bool), typeof(AyFormInput), new PropertyMetadata(true));


        #endregion

        public bool IsIntegerBox
        {
            get { return (bool)GetValue(IsDoubleBoxProperty); }
            set { SetValue(IsDoubleBoxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDoubleBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDoubleBoxProperty =
            DependencyProperty.Register("IsIntegerBox", typeof(bool), typeof(AyFormInput), new PropertyMetadata(false, OnIntegerChanged));

        private static void OnIntegerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as AyFormInput).SetIntegerEvent();
        }
        private void SetIntegerEvent()
        {
            if (IsIntegerBox)
            {
                this.PreviewTextInput += AyFormInput_PreviewTextInput;

                InputMethod.SetIsInputMethodEnabled(this, false);


                this.PreviewMouseWheel += AyFormInput_PreviewMouseWheel;
                this.PreviewKeyDown += AyFormInput_KeyDown;
            }
            else
            {
                this.PreviewTextInput -= AyFormInput_PreviewTextInput;
                this.PreviewMouseWheel -= AyFormInput_PreviewMouseWheel;
                this.PreviewKeyDown -= AyFormInput_KeyDown;
                InputMethod.SetIsInputMethodEnabled(this, true);
                this.RightContent = null;

            }

        }

        public bool IsNumberBox
        {
            get { return (bool)GetValue(IsNumberBoxProperty); }
            set { SetValue(IsNumberBoxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsNumberBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNumberBoxProperty =
            DependencyProperty.Register("IsNumberBox", typeof(bool), typeof(AyFormInput), new PropertyMetadata(false, OnNumberChanged));

        private static void OnNumberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as AyFormInput).SetNumberEvent();
        }

        private void SetNumberEvent()
        {
            if (IsNumberBox)
            {
                this.PreviewTextInput += AyFormInput_PreviewTextInput2;
                InputMethod.SetIsInputMethodEnabled(this, false);

                this.PreviewMouseWheel += AyFormInput_PreviewMouseWheel;
                this.PreviewKeyDown += AyFormInput_KeyDown;
            }
            else
            {
                this.PreviewTextInput -= AyFormInput_PreviewTextInput2;
                this.PreviewMouseWheel -= AyFormInput_PreviewMouseWheel;
                this.PreviewKeyDown -= AyFormInput_KeyDown;
                InputMethod.SetIsInputMethodEnabled(this, true);
                this.RightContent = null;

            }

        }

        private void AyFormInput_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up)
            {
                var _1 = Text.ToDouble() + Increment;
                if (MaxValue.HasValue && _1 > MaxValue.Value)
                {
                    if (IsNumberBox)
                    {
                        FormatNumberMax();
                    }
                    else if (IsIntegerBox)
                    {
                        Text = MaxValue.ToInt().ToObjectString();
                    }
                }
                else
                {
                    FormatNumberBoxElse(_1);
                }


                this.SelectionStart = Text.Length + 1;
            }
            else if (e.Key == Key.Down)
            {
                var _1 = Text.ToDouble() - Increment;
                if (MinValue.HasValue && _1 < MinValue.Value)
                {
                    if (IsNumberBox)
                    {
                        FormatNumberMin();
                    }
                    else if (IsIntegerBox)
                    {
                        Text = MinValue.ToInt().ToObjectString();
                    }
                }
                else
                {
                    FormatNumberBoxElse(_1);
                }
                this.SelectionStart = Text.Length + 1;
            }
        }
        #region 数字框的时候，鼠标滚轮滚动时候触发
        public event EventHandler<EventArgs> OnAyBoxMouseWheeled;
        #endregion


        private void AyFormInput_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                var _1 = Text.ToDouble() + Increment;
                if (MaxValue.HasValue && _1 > MaxValue.Value)
                {

                    if (IsNumberBox)
                    {
                        FormatNumberMax();
                    }
                    else if (IsIntegerBox)
                    {
                        Text = MaxValue.ToInt().ToObjectString();
                    }

                }
                else
                {
                    FormatNumberBoxElse(_1);
                }
            }
            else
            {
                var _1 = Text.ToDouble() - Increment;
                if (MinValue.HasValue && _1 < MinValue.Value)
                {
                    if (IsNumberBox)
                    {
                        FormatNumberMin();
                    }
                    else if (IsIntegerBox)
                    {
                        Text = MinValue.ToInt().ToObjectString();
                    }
                }
                else
                {
                    FormatNumberBoxElse(_1);
                }


            }
            if (OnAyBoxMouseWheeled.IsNotNull())
            {
                OnAyBoxMouseWheeled(this, null);
            }
            this.Validate();
            this.SelectionStart = Text.Length + 1;
            e.Handled = true;
        }

        private void FormatNumberMin()
        {
            if (FormatNumber != null)
            {
                Text = string.Format("{0:" + FormatNumber + "}", MinValue);
            }
            else
            {
                Text = MinValue.ToObjectString();
            }
        }

        private void FormatNumberBoxElse(double _1)
        {
            if (IsNumberBox && FormatNumber != null)
            {
                var _22 = "{0:" + FormatNumber + "}";

                string ttt = string.Format(_22, _1);
                string ttt3 = string.Format(_22, 100);
                Text = ttt;
            }
            else
            {
                Text = _1.ToObjectString();
            }
        }

        private void FormatNumberMax()
        {
            if (FormatNumber != null)
            {
                Text = string.Format("{0:" + FormatNumber + "}", MaxValue);
            }
            else
            {
                Text = MaxValue.ToObjectString();
            }
        }

        private void Rb_Click(object sender, RoutedEventArgs e)
        {
            double _1 = Text.ToDouble() + Increment;
            if (MaxValue.HasValue && _1 > MaxValue.Value)
            {
                if (IsNumberBox)
                {
                    FormatNumberMax();
                }
                else if (IsIntegerBox)
                {
                    Text = MaxValue.ToInt().ToObjectString();
                }
            }
            else
            {
                FormatNumberBoxElse(_1);
            }
            this.SelectionStart = Text.Length + 1;
            this.Validate();
        }
        private void Rb2_Click(object sender, RoutedEventArgs e)
        {
            double _1 = Text.ToDouble() - Increment;
            if (MinValue.HasValue && _1 < MinValue.Value)
            {
                if (IsNumberBox)
                {
                    FormatNumberMin();
                }
                else if (IsIntegerBox)
                {
                    Text = MinValue.ToInt().ToObjectString();
                }
            }
            else
            {
                FormatNumberBoxElse(_1);
            }
            this.SelectionStart = Text.Length + 1;
            this.Validate();
        }
        #region 2019-5-7 15:56:41 补充0显示


        public string FormatNumber
        {
            get { return (string)GetValue(FormatNumberProperty); }
            set { SetValue(FormatNumberProperty, value); }
        }

        public static readonly DependencyProperty FormatNumberProperty =
            DependencyProperty.Register("FormatNumber", typeof(string), typeof(AyFormInput), new PropertyMetadata(null));


        #endregion
        #region 增加数字框，加减号

        /// <summary>
        /// 2016-9-26 20:10:47
        /// 当IsNumberBox=true时候，设置此项有效，每次增加或者减少的幅度
        /// </summary>
        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        /// <summary>
        /// 2016-9-26 20:10:47
        /// 当IsNumberBox=true时候，设置此项有效，每次增加或者减少的幅度
        /// </summary>
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(double), typeof(AyFormInput), new PropertyMetadata(1.00));



        public double? MinValue
        {
            get { return (double?)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double?), typeof(AyFormInput), new PropertyMetadata(null, OnMinValueChanged));

        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyFormInput).SetMinValueChanged();
        }

        private void SetMinValueChanged()
        {
            this.LostKeyboardFocus += AyFormInput_LostKeyboardFocus;
        }

        private void AyFormInput_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Rule.IsNull() && Text.IsNullAndTrimAndEmpty())
            {
                return;
            }
            if (Rule.IsNotNull() && Rule.IndexOf("required") == -1 && Text.IsNullAndTrimAndEmpty())
            {
                return;
            }

            var _33 = Text.ToDouble();
            if (MinValue.HasValue && _33 < MinValue.Value)
            {
                if (IsNumberBox)
                {
                    if (FormatNumber != null)
                    {
                        Text = string.Format("{0:" + FormatNumber + "}", _33);
                    }
                }
                else
                {
                    Text = MinValue.ToObjectString();
                }
            }


        }

        public double? MaxValue
        {
            get { return (double?)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double?), typeof(AyFormInput), new PropertyMetadata(null, OnMaxValueChanged));

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyFormInput).SetMaxValueChanged();
        }

        private void SetMaxValueChanged()
        {
            this.LostKeyboardFocus += Maxvalue_LostKeyboardFocus;
        }

        private void Maxvalue_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Rule.IsNull() && Text.IsNullAndTrimAndEmpty())
            {
                return;
            }
            if (Rule.IsNotNull() && Rule.IndexOf("required") == -1 && Text.IsNullAndTrimAndEmpty())
            {
                return;
            }
            var _33 = Text.ToDouble();
            if (MaxValue.HasValue && _33 > MaxValue.Value)
            {
                if (IsNumberBox)
                {
                    if (FormatNumber != null)
                    {
                        Text = string.Format("{0:" + FormatNumber + "}", _33);
                    }
                }
                else
                {
                    Text = MaxValue.ToObjectString();
                }
            }

        }

        #endregion

        private void AyFormInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0)
            {
                e.Handled = !Char.IsDigit(e.Text[0]);
            }
        }

        private void AyFormInput_PreviewTextInput2(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0)
            {
                var aChar = e.Text[0];
                if ((aChar >= '0' && aChar <= '9') || aChar == '.')
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }

            }
        }

        public bool IsResponseChange = false;
        int lastOffset = 0;

        private void AyFormPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsResponseChange) //响应事件标识，替换字符时，不处理后续逻辑
                return;
            //Console.WriteLine(string.Format("------{0}------", e.Changes.Count));
            foreach (TextChange c in e.Changes)
            {
                //Console.WriteLine(string.Format("addLength:{0} removeLenth:{1} offSet:{2}", c.AddedLength, c.RemovedLength, c.Offset));
                Password = Password.Remove(c.Offset, c.RemovedLength); //从密码文中根据本次Change对象的索引和长度删除对应个数的字符
                Password = Password.Insert(c.Offset, Text.Substring(c.Offset, c.AddedLength));   //将Text新增的部分记录给密码文
                lastOffset = c.Offset;
            }
            //Console.WriteLine(PasswordStr);
            /*将文本转换为密码字符*/
            IsResponseChange = false;  //设置响应标识为不响应
            this.Text = ConvertToPasswordChar(Text.Length);  //将输入的字符替换为密码字符
            IsResponseChange = true;   //回复响应标识
            this.SelectionStart = lastOffset + 1; //设置光标索引
            //this.SelectionStart = lastOffset ; //设置光标索引
            //Console.WriteLine(string.Format("SelectionStar:{0}", this.SelectionStart));
        }

        /// <summary>
        /// 是否为密码框
        /// </summary>
        public bool IsPasswordBox
        {
            get { return (bool)GetValue(IsPasswordBoxProperty); }
            set { SetValue(IsPasswordBoxProperty, value); }
        }

        public static DependencyProperty IsPasswordBoxProperty =
       DependencyProperty.Register("IsPasswordBox", typeof(bool), typeof(AyFormInput), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsPasswordBoxChanged)));

        private static void OnIsPasswordBoxChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as AyFormInput).SetEvent();
        }

        /// <summary>
        /// 定义TextChange事件
        /// </summary>
        private void SetEvent()
        {
            if (IsPasswordBox)
            {
                InputMethod.SetIsInputMethodEnabled(this, false);
                this.TextChanged += AyFormPassword_TextChanged;
            }
            else
            {
                InputMethod.SetIsInputMethodEnabled(this, true);
                this.TextChanged -= AyFormPassword_TextChanged;
            }
        }
        /// <summary>
        /// 替换明文的单个密码字符
        /// </summary>
        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }


        public static DependencyProperty PasswordCharProperty =
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(AyFormInput), new FrameworkPropertyMetadata('●'));



        public bool isFirstInitPassword
        {
            get { return (bool)GetValue(isFirstInitPasswordProperty); }
            set { SetValue(isFirstInitPasswordProperty, value); }
        }

        public static readonly DependencyProperty isFirstInitPasswordProperty =
            DependencyProperty.Register("isFirstInitPassword", typeof(bool), typeof(AyFormInput), new PropertyMetadata(true));


        /// <summary>
        /// 密码字符串
        /// </summary>
        public string Password
        {
            get { return GetValue(PasswordProperty).ToObjectString(); }
            set { SetValue(PasswordProperty, value); }
        }

        public static DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(AyFormInput), new PropertyMetadata(string.Empty, OnPasswordChanged));

        private static void OnPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var _1 = (sender as AyFormInput);
            if (_1.isFirstInitPassword && _1.IsPasswordBox)
            {
                _1.IsResponseChange = false;
                var _2 = e.NewValue.ToObjectString();
                _1.Text = _1.ConvertToPasswordChar(_2.Length);
                _1.IsResponseChange = true;
                _1.isFirstInitPassword = false;
            }
        }



        #region 比较验证


        public string EqualValidateValue
        {
            get { return (string)GetValue(EqualValidateValueProperty); }
            set { SetValue(EqualValidateValueProperty, value); }
        }

        public static readonly DependencyProperty EqualValidateValueProperty =
            DependencyProperty.Register("EqualValidateValue", typeof(string), typeof(AyFormInput), new PropertyMetadata(string.Empty));





        public string EqualValidateErrorInfo
        {
            get { return (string)GetValue(EqualValidateErrorInfoProperty); }
            set { SetValue(EqualValidateErrorInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EqualValidateErrorInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EqualValidateErrorInfoProperty =
            DependencyProperty.Register("EqualValidateErrorInfo", typeof(string), typeof(AyFormInput), new PropertyMetadata("两次输入的不一致"));




        #endregion


        #endregion

    }

}
