using ay.AyExpression;
using ay.Controls.Args;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;


namespace ay.Controls
{

#pragma warning disable 1634, 1691

    [StyleTypedProperty(Property = "ItemStyle", StyleTargetType = typeof(RadioButton))]
    [TemplatePart(Name = "item_host", Type = typeof(AyItemsControlAll))]
    public class AyRadioBoxList : Control, IAyValidate, IAyHighlight
    {
        public AyRadioBoxList()
        {
            GroupName = Guid.NewGuid().ToGuidStringNoSplit("x2");
            Loaded += AyRadioBoxList_Loaded;
            this.SetResourceReference(AyRadioBoxList.ItemStyleProperty, "RadioButtonStyle");
        }

        public event EventHandler<AyBoxListEventArgs> Click;
        public void RaiseClick()
        {
            if (Click.IsNotNull())
            {
                Click(this, new AyBoxListEventArgs(SelectedObject, SelectedText, SelectedValue));
            }
        }

        public Style ItemStyle
        {
            get { return (Style)GetValue(ItemStyleProperty); }
            set { SetValue(ItemStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemStyleProperty =
            DependencyProperty.Register("ItemStyle", typeof(Style), typeof(AyRadioBoxList), new PropertyMetadata(null));


        private void AyRadioBoxList_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyRadioBoxList_Loaded;
            if (WpfTreeHelper.IsInDesignMode)
            {
                return;
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

        }

        public void RaiseSelectedValueChanged(object oldValue, object newValue)
        {
            if (CanSelectedValue)
            {
                IEnumerable<IAyCheckedItem> items = ItemsSource as IEnumerable<IAyCheckedItem>;
                if (items == null)
                {
                    return;
                }
                //var _10 = items.FirstOrDefault(item => item.ItemValue == oldValue.ToObjectString());
                //if (_10.IsNotNull())
                //{
                //    _10.IsChecked = false;
                //}
                var _1 = items.FirstOrDefault(item => item.ItemValue == newValue.ToObjectString());
                if (_1.IsNotNull())
                {
                    SelectedObject = _1;
                    SelectedText = _1.ItemText;
                    _1.IsChecked = true;
                }
                else
                {
                    var checkitem = items.FirstOrDefault(item => item.IsChecked);
                    if (checkitem == null) return;
                    checkitem.IsChecked = false;
                    SelectedObject = null;
                    SelectedText = null;
                    CanSelectedValue = false;
                    SelectedValue = "";
                    CanSelectedValue = true;
                }
            }
        }
        bool CanSelectedValue = true;
        public void UpdateCheckedState()
        {
            IEnumerable<IAyCheckedItem> items = ItemsSource as IEnumerable<IAyCheckedItem>;
            if (items == null)
            {
                return;
            }
            if (items != null)
            {
                var _1 = items.FirstOrDefault(item => item.IsChecked);
                if (_1.IsNotNull())
                {
                    SelectedObject = _1;
                    CanSelectedValue = false;
                    SelectedValue = _1.ItemValue;
                    CanSelectedValue = true;
                    SelectedText = _1.ItemText;

                }
                else
                {
                    SelectedObject = null;
                    CanSelectedValue = false;
                    SelectedValue = null;
                    CanSelectedValue = true;
                    SelectedText = null;
                }

            }

            IsHighlight = false;
            apErrorToolTip.IsOpen = false;
            RaiseClick();
        }

        public bool Validate()
        {
            if (Rule.IsNotNull())
            {
                if (Rule.ToLower().IndexOf("required") > -1)
                {
                    IEnumerable<IAyCheckedItem> items = ItemsSource as IEnumerable<IAyCheckedItem>;
                    if (items == null)
                    {
                        apErrorToolTip.IsOpen = true;
                        at.TooltipContent = AyFormErrorTemplate.Required;
                        return false;
                    }

                    int count = items.Where(item => item.IsChecked).Count();
                    if (count == 0)
                    {
                        apErrorToolTip.IsOpen = true;
                        at.TooltipContent = AyFormErrorTemplate.Required;
                        return false;
                    }
                    else
                    {
                        apErrorToolTip.IsOpen = false;
                        at.TooltipContent = "";
                        return true;
                    }
                }
            }

            return true;
        }

        public bool ValidateButNotShowError()
        {
            if (Rule.IsNotNull())
            {
                if (Rule.ToLower().IndexOf("required") > -1)
                {
                    IEnumerable<IAyCheckedItem> items = ItemsSource as IEnumerable<IAyCheckedItem>;
                    if (items == null)
                    {
                        at.TooltipContent = AyFormErrorTemplate.Required;
                        return false;
                    }

                    int count = items.Where(item => item.IsChecked).Count();
                    if (count == 0)
                    {
                        at.TooltipContent = AyFormErrorTemplate.Required;
                        return false;
                    }
                    else
                    {
                        at.TooltipContent = "";
                        return true;
                    }
                }
            }
            return true;
        }

        public void ShowError()
        {
            apErrorToolTip.IsOpen = true;
        }


        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(AyRadioBoxList), new PropertyMetadata(new Thickness(5, 2, 5, 2)));



        static AyRadioBoxList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AyRadioBoxList), new FrameworkPropertyMetadata(typeof(AyRadioBoxList)));

        }

        public static readonly DependencyProperty IsSelectAllCheckedProperty =
         DependencyProperty.Register("IsSelectAllChecked", typeof(bool?), typeof(AyRadioBoxList), new PropertyMetadata(false));


        /// <summary>
        /// 是否开启SelectedValue，Text，Object功能，开启后，这3个对象会有值
        /// 2016-12-23 10:32:02
        /// AY
        /// </summary>
        //public bool IsOpenSelectedFunc
        //{
        //    get { return (bool)GetValue(IsOpenSelectedFuncProperty); }
        //    set { SetValue(IsOpenSelectedFuncProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for IsOpenSelectedFunc.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty IsOpenSelectedFuncProperty =
        //    DependencyProperty.Register("IsOpenSelectedFunc", typeof(bool), typeof(AyRadioBoxList), new PropertyMetadata(true));


        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(AyRadioBoxList), new PropertyMetadata(""));


        /// <summary>
        /// 规则：AY表达式 , 只支持required
        /// </summary>
        public string Rule
        {
            get { return (string)GetValue(RuleProperty); }
            set { SetValue(RuleProperty, value); }
        }

        public static readonly DependencyProperty RuleProperty =
            DependencyProperty.Register("Rule", typeof(string), typeof(AyRadioBoxList), new PropertyMetadata(null));


        #region 提供快速读取



        /// <summary>
        /// 选中的Value
        /// </summary>
        public string SelectedValue
        {
            get { return (string)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(string), typeof(AyRadioBoxList), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedValueChanged)));

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyRadioBoxList).RaiseSelectedValueChanged(e.OldValue, e.NewValue);
        }


        /// <summary>
        /// 选中的Text
        /// </summary>
        public string SelectedText
        {
            get { return (string)GetValue(SelectedTextProperty); }
            set { SetValue(SelectedTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTextProperty =
            DependencyProperty.Register("SelectedText", typeof(string), typeof(AyRadioBoxList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));







        public IAyCheckedItem SelectedObject
        {
            get { return (IAyCheckedItem)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(IAyCheckedItem), typeof(AyRadioBoxList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));




        #endregion

        /// <summary>
        /// 是否高亮，默认不是
        /// </summary>
        public bool IsHighlight
        {
            get { return (bool)GetValue(IsHighlightProperty); }
            set { SetValue(IsHighlightProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightProperty =
            DependencyProperty.Register("IsHighlight", typeof(bool), typeof(AyRadioBoxList), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));



        public AyPanelAllPanelType PanelType
        {
            get { return (AyPanelAllPanelType)GetValue(PanelTypeProperty); }
            set { SetValue(PanelTypeProperty, value); }
        }
        public static readonly DependencyProperty PanelTypeProperty =
            DependencyProperty.Register("PanelType", typeof(AyPanelAllPanelType), typeof(AyRadioBoxList), new FrameworkPropertyMetadata(AyPanelAllPanelType.WrapPanel_H, FrameworkPropertyMetadataOptions.AffectsRender));



        /// <summary>
        /// 生成CheckBox的数据源
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(AyRadioBoxList), new PropertyMetadata(null));




        public void HighlightElement()
        {
            IsHighlight = true;
        }

        #region 错误提示

        public Brush AyToolTipBackground
        {
            get { return (Brush)GetValue(AyToolTipBackgroundProperty); }
            set { SetValue(AyToolTipBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipBackgroundProperty =
            DependencyProperty.Register("AyToolTipBackground", typeof(Brush), typeof(AyRadioBoxList), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFFFF4AC")));



        public Brush AyToolTipForeground
        {
            get { return (Brush)GetValue(AyToolTipForegroundProperty); }
            set { SetValue(AyToolTipForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipForegroundProperty =
            DependencyProperty.Register("AyToolTipForeground", typeof(Brush), typeof(AyRadioBoxList), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFE25D5D")));



        public Brush AyToolTipBorderBrush
        {
            get { return (Brush)GetValue(AyToolTipBorderBrushProperty); }
            set { SetValue(AyToolTipBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipBorderBrushProperty =
            DependencyProperty.Register("AyToolTipBorderBrush", typeof(Brush), typeof(AyRadioBoxList), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFECA50D")));



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


        AyTooltip at = null;

        TextBlock _tb = null;
        private void CreatePopupEx()
        {
            if (_apErrorToolTip.IsNull())
            {
                _apErrorToolTip = new ToolTip();
                _apErrorToolTip.BorderThickness = new Thickness(0);
                _apErrorToolTip.Background = new SolidColorBrush(Colors.Transparent);
                _apErrorToolTip.Padding = new Thickness(0);

                _apErrorToolTip.Placement = PlacementMode.Right;
                _apErrorToolTip.Padding = new Thickness(0, 0, 0, 10);
                _apErrorToolTip.HorizontalOffset = 0;
                _apErrorToolTip.VerticalOffset = 0;
                _apErrorToolTip.Opened += popup_Opened;
                _apErrorToolTip.PlacementTarget = this;
                _apErrorToolTip.VerticalContentAlignment = VerticalAlignment.Center;

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

                    _apErrorToolTip.Content = at;


                }
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
        public void DragTitleBarWhen()
        {
            this.apErrorToolTip.IsOpen = false;
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
    }
}
