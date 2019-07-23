using ay.AyExpression;
using ay.contentcore;
using ay.Controls.Args;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;


namespace ay.Controls
{

    /// <summary>
    /// checkbox列表控件 AY 2017-10-20 17:45:33
    /// </summary>
    [StyleTypedProperty(Property = "ItemStyle", StyleTargetType = typeof(CheckBox))]
    public class AyCheckBoxList : Control, IAyValidate, IAyHighlight
    {
        public AyCheckBoxList()
        {
            Loaded += AyCheckBoxList_Loaded;
            //Style style = new Style { TargetType = typeof(ToolTip) };

            //Setter setter = new Setter();
            //setter.Property = FrameworkElement.LayoutTransformProperty;
            //setter.Value = FindResource("scaler");

            //style.Setters.Add(setter);
            this.SetResourceReference(AyCheckBoxList.ItemStyleProperty, "CheckBoxStyle");
            //Resources.Add(typeof(ToolTip), style);
        }

        private void AyCheckBoxList_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyCheckBoxList_Loaded;
            //UpdateSelectResult();
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

        /// <summary>
        /// 多个值分组的依据，默认是英文的逗号
        /// </summary>
        public char SplitChar
        {
            get { return (char)GetValue(SplitCharProperty); }
            set { SetValue(SplitCharProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SplitChar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SplitCharProperty =
            DependencyProperty.Register("SplitChar", typeof(char), typeof(AyCheckBoxList), new PropertyMetadata(','));
        /// <summary>
        /// selectedvalue改变后，触发ischecked的变化
        /// </summary>
        /// <param name="oldValue">旧selectedvalue值</param>
        /// <param name="newValue"></param>
        public void RaiseSelectedValueChanged(object oldValue, object newValue)
        {
            if (CanSelectedValue)
            {
                IEnumerable<IAyCheckedItem> items = ItemsSource as IEnumerable<IAyCheckedItem>;
                if (items == null)
                {
                    return;
                }
                var ovalues = oldValue.ToObjectString().Split(SplitChar);
                if (ovalues.Length > 0)
                {
                    var _ov = items.Where(item => ovalues.Contains(item.ItemValue));
                    _ov.ForEach(x =>
                    {
                        x.IsChecked = false;
                    });
                }
                var nvalues = newValue.ToObjectString().Split(SplitChar);
                var _1 = items.Where(item => nvalues.Contains(item.ItemValue));
                if (_1.IsNotNull() && _1.Count() > 0)
                {
                    SelectedObject = _1;
                    StringBuilder sbText = new StringBuilder();
                    foreach (var item in _1)
                    {
                        sbText.Append(item.ItemText + SplitChar);
                        item.IsChecked = true;
                    }
                    SelectedText = sbText.ToString().TrimEnd(SplitChar);
                }


                if (nvalues.Length == items.Count())
                {
                    IsSelectAllChecked = true;
                }
                else if (nvalues.Count() == 0)
                {
                    IsSelectAllChecked = false;
                }
                else
                {
                    IsSelectAllChecked = null;
                }

            }
        }
        bool CanSelectedValue = true;

        static AyCheckBoxList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AyCheckBoxList), new FrameworkPropertyMetadata(typeof(AyCheckBoxList)));
        }

        public static readonly DependencyProperty IsSelectAllCheckedProperty =
         DependencyProperty.Register("IsSelectAllChecked", typeof(bool?), typeof(AyCheckBoxList), new PropertyMetadata(false));

        /// <summary>
        /// 返回或设置全选复选框的选中状态
        /// </summary>
        public bool? IsSelectAllChecked
        {
            get { return (bool?)GetValue(IsSelectAllCheckedProperty); }
            set { SetValue(IsSelectAllCheckedProperty, value); }
        }

        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(AyCheckBoxList), new PropertyMetadata(new Thickness(5,2,5,2)));


        public Style ItemStyle
        {
            get { return (Style)GetValue(ItemStyleProperty); }
            set { SetValue(ItemStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckBoxStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemStyleProperty =
            DependencyProperty.Register("ItemStyle", typeof(Style), typeof(AyCheckBoxList), new PropertyMetadata(null));




        public AyPanelAllPanelType PanelType
        {
            get { return (AyPanelAllPanelType)GetValue(PanelTypeProperty); }
            set { SetValue(PanelTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PanelType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanelTypeProperty =
            DependencyProperty.Register("PanelType", typeof(AyPanelAllPanelType), typeof(AyCheckBoxList), new PropertyMetadata(AyPanelAllPanelType.WrapPanel_H));


        /// <summary>
        /// 是否高亮，默认不是
        /// </summary>
        public bool IsHighlight
        {
            get { return (bool)GetValue(IsHighlightProperty); }
            set { SetValue(IsHighlightProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightProperty =
            DependencyProperty.Register("IsHighlight", typeof(bool), typeof(AyCheckBoxList), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));



        /// <summary>
        /// 规则：AY表达式 , 支持required
        /// </summary>
        public string Rule
        {
            get { return (string)GetValue(RuleProperty); }
            set { SetValue(RuleProperty, value); }
        }

        public static readonly DependencyProperty RuleProperty =
            DependencyProperty.Register("Rule", typeof(string), typeof(AyCheckBoxList), new PropertyMetadata(null));

        /// <summary>
        /// 生成CheckBox的数据源
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(AyCheckBoxList), new PropertyMetadata(null));


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
            DependencyProperty.Register("SelectedValue", typeof(string), typeof(AyCheckBoxList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedValueChanged)));
        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyCheckBoxList).RaiseSelectedValueChanged(e.OldValue, e.NewValue);
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
            DependencyProperty.Register("SelectedText", typeof(string), typeof(AyCheckBoxList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        /// <summary>
        /// 选中的对象的结合
        /// </summary>
        public IEnumerable SelectedObject
        {
            get { return (IEnumerable)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(IEnumerable), typeof(AyCheckBoxList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        /// <summary>
        /// 全选或清空所用选择
        /// </summary>
        public void CheckAll()
        {
            foreach (IAyCheckedItem item in ItemsSource as IList<IAyCheckedItem>)
            {
                item.IsChecked = IsSelectAllChecked.HasValue ? IsSelectAllChecked.Value : false;
            }
            IEnumerable<IAyCheckedItem> items = ItemsSource as IEnumerable<IAyCheckedItem>;
            if (items == null)
            {
                return;
            }

            var _1 = items.Where(item => item.IsChecked);
            if (_1.IsNotNull() && _1.Count() > 0)
            {
                SelectedObject = _1;
                StringBuilder sbValue = new StringBuilder();
                StringBuilder sbText = new StringBuilder();
                foreach (var item in _1)
                {
                    sbValue.Append(item.ItemValue + SplitChar);
                    sbText.Append(item.ItemText + SplitChar);
                }
                CanSelectedValue = false;
                SelectedValue = sbValue.ToString().TrimEnd(SplitChar);
                CanSelectedValue = true;
                SelectedText = sbText.ToString().TrimEnd(SplitChar);
            }
            else
            {
                SelectedObject = null;
                CanSelectedValue = false;
                SelectedValue = null;
                CanSelectedValue = true;
                SelectedText = null;
            }

            int count = _1.Count();
            if (count == items.Count())
            {
                IsSelectAllChecked = true;
            }
            else if (count == 0)
            {
                IsSelectAllChecked = false;
            }
            else
            {
                IsSelectAllChecked = null;
            }
            IsHighlight = false;
            apErrorToolTip.IsOpen = false;
        }

        public event EventHandler<AyBoxListEventArgs> Click;
        public void RaiseClick()
        {
            if (Click.IsNotNull())
            {
                Click(this, new AyBoxListEventArgs(SelectedObject,SelectedText,SelectedValue));
            }
        }

        /// <summary>
        /// 根据当前选择的个数来更新全选框的状态
        /// </summary>
        public void UpdateCheckedState()
        {
            IEnumerable<IAyCheckedItem> items = ItemsSource as IEnumerable<IAyCheckedItem>;
            if (items == null)
            {
                return;
            }

            var _1 = items.Where(item => item.IsChecked);
            if (_1.IsNotNull() && _1.Count() > 0)
            {
                SelectedObject = _1;
                StringBuilder sbValue = new StringBuilder();
                StringBuilder sbText = new StringBuilder();
                foreach (var item in _1)
                {
                    sbValue.Append(item.ItemValue + SplitChar);
                    sbText.Append(item.ItemText + SplitChar);
                }
                CanSelectedValue = false;
                SelectedValue = sbValue.ToString().TrimEnd(SplitChar);
                CanSelectedValue = true;
                SelectedText = sbText.ToString().TrimEnd(SplitChar);
            }
            else
            {
                SelectedObject = null;
                CanSelectedValue = false;
                SelectedValue = null;
                CanSelectedValue = true;
                SelectedText = null;
            }

            int count = _1.Count();
            if (count == items.Count())
            {
                IsSelectAllChecked = true;
            }
            else if (count == 0)
            {
                IsSelectAllChecked = false;
            }
            else
            {
                IsSelectAllChecked = null;
            }
            IsHighlight = false;
            apErrorToolTip.IsOpen = false;
            RaiseClick();
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


        /// <summary>
        /// 验证是否有选中的
        /// </summary>
        /// <returns></returns>
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
            DependencyProperty.Register("AyToolTipBackground", typeof(Brush), typeof(AyCheckBoxList), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFFFF4AC")));



        public Brush AyToolTipForeground
        {
            get { return (Brush)GetValue(AyToolTipForegroundProperty); }
            set { SetValue(AyToolTipForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipForegroundProperty =
            DependencyProperty.Register("AyToolTipForeground", typeof(Brush), typeof(AyCheckBoxList), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFE25D5D")));



        public Brush AyToolTipBorderBrush
        {
            get { return (Brush)GetValue(AyToolTipBorderBrushProperty); }
            set { SetValue(AyToolTipBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AyToolTipBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AyToolTipBorderBrushProperty =
            DependencyProperty.Register("AyToolTipBorderBrush", typeof(Brush), typeof(AyCheckBoxList), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#FFECA50D")));



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

        public void DragTitleBarWhen()
        {
            this.apErrorToolTip.IsOpen = false;
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

    }
}
