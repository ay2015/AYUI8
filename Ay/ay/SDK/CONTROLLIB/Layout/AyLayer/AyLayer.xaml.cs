
using ay.Animate;
using ay.contentcore;
using ay.contents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;


namespace ay.Controls
{
    /// <summary>
    /// AyLayer.xaml 的交互逻辑
    /// </summary>
    public partial class AyLayer : UserControl
    {
        public System.Action DragTitleBarStart = null;

        public AyLayer()
        {
            InitializeComponent();

        }
        public object ItemContent { get; set; }
        private Grid Owner = null;
        /// <summary>
        /// 根据ID关闭一个aylayer
        /// </summary>
        /// <param name="layerId"></param>
        public static void Close(string layerId)
        {
            if (AYUI.Session.ContainsKey(layerId))
            {
                var t = AYUI.Session[layerId] as AyLayer;
                if (t != null)
                {
                    AYUI.Session.Remove(layerId);
                    t.CloseAyLayer(layerId, t);
                }
            }
        }



        public void SetDragMove(UIElement ui)
        {
            DragInGridBehavior m = new DragInGridBehavior();
            //m.ConstrainToParentBounds = true;
            m.Attach(ui);
        }
        public AyLayerOptions _options;

        private void SetAyLayerBase(object owner, object content, string title, AyLayerOptions options, bool isDiaglog)
        {
            if (options == null)
            {
                options = AyLayerOptions.DefaultAyLayerOptions;
            }

            _options = options;
            //this.Topmost = true;
            //this.ShowInTaskbar = true;
            //if (owner != null)
            //{
            //    Owner = owner;
            //}

            if (owner == null)
            {
                if (Application.Current.MainWindow is AyWindowBase)
                {
                    AyWindowBase mainWindow = Application.Current.MainWindow as AyWindowBase;
                    Owner = mainWindow.ayLayerArea;
                }
            }
            else
            {
                var _Owner = owner as Grid;
                if (_Owner != null)
                {
                    Owner = _Owner;
                }
                else
                {
                    var _Wn = owner as AyWindowBase;
                    if (_Wn.IsNotNull())
                        Owner = _Wn.ayLayerArea;
                }
            }

            userPresenter.Content = content;
            ItemContent = content;

            if (options.LayerId.IsNotNull())
            {
                AYUI.Session[options.LayerId] = this;
            }
            if (options.Direction.HasValue)
            {
                switch (options.Direction.Value)
                {
                    case AyLayerDockDirect.LT:
                        body.HorizontalAlignment = HorizontalAlignment.Left;
                        body.VerticalAlignment = VerticalAlignment.Top;
                        break;
                    case AyLayerDockDirect.CT:
                        //中间位置计算
                        body.HorizontalAlignment = HorizontalAlignment.Center;
                        body.VerticalAlignment = VerticalAlignment.Top;
                        break;
                    case AyLayerDockDirect.RT:
                        //中间位置计算
                        body.HorizontalAlignment = HorizontalAlignment.Right;
                        body.VerticalAlignment = VerticalAlignment.Top;
                        break;
                    case AyLayerDockDirect.LC:
                        body.HorizontalAlignment = HorizontalAlignment.Left;
                        body.VerticalAlignment = VerticalAlignment.Center;
                        break;
                    case AyLayerDockDirect.CC:
                        body.HorizontalAlignment = HorizontalAlignment.Center;
                        body.VerticalAlignment = VerticalAlignment.Center;
                        break;
                    case AyLayerDockDirect.RC:
                        body.HorizontalAlignment = HorizontalAlignment.Right;
                        body.VerticalAlignment = VerticalAlignment.Center;
                        break;
                    case AyLayerDockDirect.LB:
                        body.HorizontalAlignment = HorizontalAlignment.Left;
                        body.VerticalAlignment = VerticalAlignment.Bottom;
                        break;
                    case AyLayerDockDirect.CB:
                        body.HorizontalAlignment = HorizontalAlignment.Center;
                        body.VerticalAlignment = VerticalAlignment.Bottom;
                        break;
                    case AyLayerDockDirect.RB:
                        body.HorizontalAlignment = HorizontalAlignment.Right;
                        body.VerticalAlignment = VerticalAlignment.Bottom;
                        break;
                    default:
                        break;
                }
            }

            if (options.IsContainsTitleBar)
            {

                d.Height = "40.00".ToGridLength();
                Border b = new Border();
                b.SetResourceReference(Border.BorderBrushProperty, "bordercolorlight");
                b.BorderThickness = options.TitleBarBorderThickness;
                if (options.CanDrag)
                {
                    SetDragMove(b);
                }
                if (options.LayerCornerRadius.HasValue)
                {
                    b.CornerRadius = new CornerRadius(options.LayerCornerRadius.Value.TopLeft, options.LayerCornerRadius.Value.TopRight, 0, 0);
                }

                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                Grid g = new Grid();
                if (!title.IsNullOrWhiteSpace())
                {
                    AyText tb = new AyText();
                    tb.Width = 200;
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.HorizontalAlignment = HorizontalAlignment.Left;
                    tb.Margin = new Thickness(16, 0, 0, 0);
                    tb.TextWrapping = TextWrapping.NoWrap;
                    tb.SetResourceReference(AyText.FontSizeProperty, "WindowTitleFontSize");
                    tb.TextTrimming = TextTrimming.CharacterEllipsis;
                    tb.Text = title;
                    g.Children.Add(tb);
                }

                if (options.CanClose)
                {
                    Button ab = new Button();
                    ab.SetResourceReference(Button.StyleProperty, "AYWin_CLOSE");
                    ab.Click += closewindow_Click;
                    ab.VerticalAlignment = VerticalAlignment.Center;
                    ab.HorizontalAlignment = HorizontalAlignment.Right;
                    ab.Content = Langs.share_close.Lang();
                    ab.Width = 32;
                    ab.Height = 32;
                    ab.Margin = new Thickness(0, 0, 5, 0);
                    g.Children.Add(ab);
                }
                b.Child = g;
                b.SetResourceReference(Border.BackgroundProperty, "colorwhite");
                bodyConent.Children.Add(b);
            }
            else
            {
                if (options.TitleBar.IsNotNull() && options.CanDrag)
                {
                    SetDragMove(options.TitleBar);
                }
            }

            if (options.MaskBrush.IsNotNull())
            {
                layoutMain.Background = options.MaskBrush;
            }
            if (isDiaglog)
            {
                if (options.MaskBrush.IsNull())
                {
                    layoutMain.Background = SolidColorBrushConverter.From16JinZhi("#8C000000");
                }
                if (options.WhenShowDialogNeedShake)
                    layoutMain.MouseLeftButtonDown += LayoutMain_MouseLeftButtonDown;
            }

            if (options.IsShowLayerBorder)
            {
                body.SetResourceReference(Border.BorderBrushProperty, "bordercolorlight");

                if (options.LayerBorderThickness.HasValue)
                {
                    body.BorderThickness = options.LayerBorderThickness.Value;
                }
                else
                {
                    body.BorderThickness = new Thickness(1);
                }
            }
            if (options.LayerCornerRadius.HasValue)
            {
                body.CornerRadius = options.LayerCornerRadius.Value;
            }
            options.LayerBackground.Freeze();
            body.Background = options.LayerBackground;

            if (options.ShowAnimateIndex == 0)
            {
                body.Visibility = Visibility.Visible;
                if (options.Opened.IsNotNull())
                {
                    options.Opened();
                }
                body.Loaded += Body_Loaded;

                DropShadowEffect de = new DropShadowEffect();
                de.Color = options.ShadowColor;
                de.ShadowDepth = options.ShadowDepth;
                de.Opacity = 0.1;
                body.Effect = de;
                de.BlurRadius = options.ShadowRadius;
                SetRealPoint(options);

            }
            else if (options.ShowAnimateIndex == 1)
            {
                var sc = new AyAniScale(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 450;
                sc.ScaleXFrom = 0;

                sc.ScaleYFrom = 0;
                sc.ScaleXTo = 1;
                sc.ScaleYTo = 1;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Begin();


            }
            else if (options.ShowAnimateIndex == 2)
            {
                var sc = new AyAniSlideInDown(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 750;
                sc.FromDistance = -4000;
                sc.OpacityNeed = false;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 3)
            {
                var sc = new AyAniSlideInUp(body, () =>
                {

                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 750;
                sc.FromDistance = 4000;
                sc.OpacityNeed = false;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 4)
            {
                var sc = new AyAniSlideInLeft(body, () =>
                {

                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 750;
                sc.FromDistance = -4000;
                sc.OpacityNeed = false;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 5)
            {
                var sc = new AyAniSlideInRight(body, () =>
                {

                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 750;
                sc.FromDistance = 4000;
                sc.OpacityNeed = false;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 6)
            {
                var sc = new AyAniBounceInDown(body, () =>
                {

                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 7)
            {
                var sc = new AyAniBounceInUp(body, () =>
                {

                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 8)
            {
                var sc = new AyAniBounceInLeft(body, () =>
                {

                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 9)
            {
                var sc = new AyAniBounceInRight(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 10)
            {
                var sc = new AyAniRotateIn(body, () =>
                {
                    ShowShadow(options);

                });
                sc.AnimateSpeed = 750;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 11)
            {
                var sc = new AyAniBounceIn(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 750;
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 12)
            {
                var sc = new AyAniBounceInLeft(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 750;
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 13)
            {
                var sc = new AyAniBounceInRight(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 750;
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 14)
            {
                var sc = new AyAniBounceInDown(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 750;
                sc.Begin();
            }
            else if (options.ShowAnimateIndex == 15)
            {
                var sc = new AyAniBounceInUp(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AutoDestory = true;
                sc.AnimateSpeed = 750;
                sc.Begin();
            }


        }

        private void Body_Loaded(object sender, RoutedEventArgs e)
        {
            body.Loaded -= Body_Loaded;
            SetRealPoint(_options);
        }

        public void SetRealPoint(AyLayerOptions options)
        {
            if (options.Direction.HasValue)
            {
                //var scre = AyWindow.GetScreen(Window.GetWindow(this));
                double dh = Owner.ActualHeight;
                double dw = Owner.ActualWidth;
                //if (dh > scre.Bounds.Height)
                //{
                //    dh = scre.Bounds.Height;
                //}
                //if (dw> scre.Bounds.Width)
                //{
                //    dw = scre.Bounds.Width;
                //}
                body.HorizontalAlignment = HorizontalAlignment.Left;
                body.VerticalAlignment = VerticalAlignment.Top;
                switch (options.Direction.Value)
                {
                    case AyLayerDockDirect.LT:
                        body.Margin = new Thickness(0, 0, 0, 0);
                        break;
                    case AyLayerDockDirect.CT:
                        //中间位置计算
                        var _left = (dw / 2.0) - (body.ActualWidth / 2.0);
                        body.Margin = new Thickness(_left, 0, 0, 0);
                        break;
                    case AyLayerDockDirect.RT:
                        //中间位置计算
                        var _RT = dw - body.ActualWidth;
                        body.Margin = new Thickness(_RT, 0, 0, 0);
                        break;
                    case AyLayerDockDirect.LC:
                        var _LC = (dh / 2.0) - (body.ActualHeight / 2.0);
                        //var _LC = dh - body.ActualHeight;
                        body.Margin = new Thickness(0, _LC, 0, 0);
                        break;
                    case AyLayerDockDirect.CC:
                        var CC = (dh / 2.0) - (body.ActualHeight / 2.0);
                        var _leftd = (dw / 2.0) - (body.ActualWidth / 2.0);
                        body.Margin = new Thickness(_leftd, CC, 0, 0);
                        break;
                    case AyLayerDockDirect.RC:
                        var CC2 = (dh / 2.0) - (body.ActualHeight / 2.0);
                        var _RT1 = dw - body.ActualWidth;
                        body.Margin = new Thickness(_RT1, CC2, 0, 0);
                        break;
                    case AyLayerDockDirect.LB:
                        var _RT2 = dh - body.ActualHeight;
                        body.Margin = new Thickness(0, _RT2, 0, 0);
                        break;
                    case AyLayerDockDirect.CB:
                        var _RT3 = dh - body.ActualHeight;
                        var _leftd1 = (dw / 2.0) - (body.ActualWidth / 2.0);
                        body.Margin = new Thickness(_leftd1, _RT3, 0, 0);
                        break;
                    case AyLayerDockDirect.RB:
                        var _RT4 = dh - body.ActualHeight;
                        var _RT5 = dw - body.ActualWidth;
                        body.Margin = new Thickness(_RT5, _RT4, 0, 0);
                        break;
                    default:
                        break;
                }
            }
        }
        private void ShowShadow(AyLayerOptions options)
        {
            if (options.HasShadow)
            {
                DropShadowEffect de = new DropShadowEffect();
                //de.BlurRadius = options.ShadowRadius;
                de.Color = options.ShadowColor;
                de.ShadowDepth = options.ShadowDepth;
                body.Effect = de;
                body.Opacity = 0.1;
                de.BlurRadius = options.ShadowRadius;
                AyAniDouble _1 = new AyAniDouble(body);
                _1.AutoDestory = true;
                _1.AniPropertyPath = new PropertyPath("(FrameworkElement.Effect).(DropShadowEffect.BlurRadius)");
                _1.FromDouble = 0;
                _1.ToDouble = options.ShadowRadius;
                _1.AniEasingMode = 2;
                _1.AnimateSpeed = 200;
                _1.Begin();
                _1.Completed += () =>
                {
                    SetRealPoint(options);
                };
            }
            else
            {
                SetRealPoint(options);
            }
            if (options.Opened.IsNotNull())
            {
                options.Opened();
            }
        }

        AyAniPulse ani = null;
        private void LayoutMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var _ayhit = e.OriginalSource as Grid;
            if (_ayhit != null)
            {
                if (_ayhit.Name != null && _ayhit.Name.Equals("layoutMain"))
                {
                    if (ani == null)
                    {
                        ani = new AyAniPulse(body);
                        ani.AnimateSpeed = 300;
                        ani.ScaleXDiff = 0.05;
                        ani.ScaleYDiff = 0.05;
                        ani.Begin();
                    }
                    else if (ani.IsCompleted)
                    {
                        ani.Begin();
                    }
                }
            }


            e.Handled = true;
        }

        public AyLayer(object owner, object content, string title, AyLayerOptions options, bool isdialog)
        {
            InitializeComponent();

            SetAyLayerBase(owner, content, title, options, isdialog);
        }

        public static AyLayer Show(object content)
        {
            return Show(null, content);
        }

        public static AyLayer Show(object owner, object content)
        {
            return Show(owner, content, null);
        }
        public static AyLayer Show(object owner, object content, string title)
        {
            return Show(owner, content, title, null);
        }
        public static AyLayer Show(object owner, object content, string title, AyLayerOptions options)
        {
            var messageBox = new AyLayer(owner, content, title, options, false);
            messageBox.Show();
            return messageBox;
        }

        public void Show()
        {
            var _1 = Owner.Children.Count;
            var zIndex = _1 + 1;
            Panel.SetZIndex(this, zIndex);
            this.Visibility = Visibility.Visible;
            this.Opacity = 1;
            Owner.Children.Add(this);
        }


        public static AyLayer ShowDialog(object content)
        {
            return ShowDialog(null, content);
        }
        public static AyLayer ShowDialog(object owner, object content)
        {
            return ShowDialog(owner, content, null);
        }
        public static AyLayer ShowDialog(object owner, object content, string title)
        {
            return ShowDialog(owner, content, title, null);
        }
        public static AyLayer ShowDialog(object owner, object content, string title, AyLayerOptions options)
        {
            var messageBox = new AyLayer(owner, content, title, options, true);
            messageBox.Show();
            return messageBox;
        }


        private void closewindow_Click(object sender, RoutedEventArgs e)
        {
            var _layerId = _options.LayerId;


            if (_layerId.IsNotNull() && AYUI.Session.ContainsKey(_layerId))
            {
                var t = AYUI.Session[_layerId] as AyLayer;
                if (t != null)
                {
                    CloseAyLayerNotTriggerClosed(_layerId, t);
                }
            }
            else
            {
                CloseAyLayerTop();
            }
        }
        /// <summary>
        /// 关闭弹层
        /// </summary>
        private void CloseAyLayerTop()
        {
            if (_options.HasCloseAnimation)
            {
                var bn = new AyAniZoomBounceOut(body, () =>
                {
                    this.Visibility = Visibility.Collapsed;
                    this.Opacity = 0;

                    Owner.Children.Remove(this);
                });
                bn.AutoDestory = true;
                bn.Begin();
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
                this.Opacity = 0;
                Owner.Children.Remove(this);
            }
       
        }
        /// <summary>
        /// 动画方式，关闭时候不触发用户定义的Closed回调
        /// </summary>
        /// <param name="_layerId"></param>
        /// <param name="t"></param>
        public void CloseAyLayerNotTriggerClosed(string _layerId, AyLayer t)
        {
            AYUI.Session.Remove(_layerId);
            t.CloseAyLayerTop();
        }
        /// <summary>
        /// 动画方式关闭弹层
        /// </summary>
        /// <param name="_layerId">弹层id</param>
        /// <param name="t"></param>
        public void CloseAyLayer(string _layerId, AyLayer t)
        {
            if (_options.Closed.IsNotNull())
            {
                _options.Closed();
            }
            AYUI.Session.Remove(_layerId);
            t.CloseAyLayerTop();
        }
        /// <summary>
        /// 非动画方式关闭弹层，直接关闭
        /// </summary>
        /// <param name="_layerId">弹层id</param>
        /// <param name="t"></param>
        public void CloseLIAyLayer(string _layerId, AyLayer t)
        {
            if (_options.Closed.IsNotNull())
            {
                _options.Closed();
            }
            AYUI.Session.Remove(_layerId);
            this.Visibility = Visibility.Collapsed;
            this.Opacity = 0;

            Owner.Children.Remove(t);
        }

        /// <summary>
        /// 非动画方式关闭弹层，直接关闭，不触发用户定义的Closed
        /// </summary>
        /// <param name="_layerId">弹层id</param>
        /// <param name="t"></param>
        public void CloseLIAyLayerNotTriggerClosed(string _layerId, AyLayer t)
        {
            AYUI.Session.Remove(_layerId);
            this.Visibility = Visibility.Collapsed;
            this.Opacity = 0;
            Owner.Children.Remove(t);
        }

    }
}
