using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Windows.Shapes;

namespace ay.Controls
{
    [TemplatePart(Name = "PART_MENU", Type = typeof(Button))]
    [TemplatePart(Name = "PART_MIN", Type = typeof(Button))]
    [TemplatePart(Name = "PART_MAX", Type = typeof(Button))]
    [TemplatePart(Name = "PART_RESTORE", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CLOSE", Type = typeof(Button))]
    [TemplatePart(Name = "contentBorder", Type = typeof(Border))]
    public class AyWindowBase : Window, INotifyPropertyChanged
    {
        #region 提供虚方法
        public virtual void DoRestoreOrMax() { }
        public virtual void DoMinWindow() { }
        public virtual void DoCloseWindow() { }
        public virtual void DoShowWindowMenu() { }

        #endregion

        public Visibility TitleVisibility
        {
            get { return (Visibility)GetValue(TitleVisibilityProperty); }
            set { SetValue(TitleVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TitleVisibilityProperty =
            DependencyProperty.Register("TitleVisibility", typeof(Visibility), typeof(AyWindowBase), new PropertyMetadata(Visibility.Visible));

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        static AyWindowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AyWindowBase),
                new FrameworkPropertyMetadata(typeof(AyWindowBase)));
        }
        public AyWindowBase()
        {
            this.Loaded += AyWindowBase_Loaded;
        }

        public Rectangle AyWindowMaskArea = null;
        private ay.Animate.AyAniColor _MaskColorAnimate;
        /// <summary>
        /// 无注释
        /// </summary>
        public ay.Animate.AyAniColor MaskColorAnimate
        {
            get
            {
                if (_MaskColorAnimate == null)
                {
                    _MaskColorAnimate = new Animate.AyAniColor(AyWindowMaskArea);
                    _MaskColorAnimate.AniPropertyPath = _MaskColorAnimate.SampleFillPropertyPath;
                    _MaskColorAnimate.ToColor = HexToBrush.ToColor("#8C000000");

                    _MaskColorAnimate.AnimateSpeed = 150;
                }
                return _MaskColorAnimate;
            }
        }
        public void ResetMaskColor()
        {
            AyWindowMaskArea.Fill = Brushes.Transparent;
        }
        //private ay.Animate.AyAniColor _MaskColorAnimateEnd;
        ///// <summary>
        ///// 无注释
        ///// </summary>
        //public ay.Animate.AyAniColor MaskColorAnimateEnd
        //{
        //    get
        //    {
        //        if (_MaskColorAnimateEnd == null)
        //        {
        //            _MaskColorAnimateEnd = new Animate.AyAniColor(AyWindowMaskArea);
        //            _MaskColorAnimateEnd.AniPropertyPath = _MaskColorAnimateEnd.SampleFillPropertyPath;

        //            _MaskColorAnimateEnd.ToColor = HexToBrush.ToColor("#00000000");
        //            _MaskColorAnimateEnd.AnimateSpeed = 150;
        //        }
        //        return _MaskColorAnimateEnd;
        //    }
        //}


        public bool ComfirmBeforeClose
        {
            get { return (bool)GetValue(ComfirmBeforeCloseProperty); }
            set { SetValue(ComfirmBeforeCloseProperty, value); }
        }

        public static readonly DependencyProperty ComfirmBeforeCloseProperty =
            DependencyProperty.Register("ComfirmBeforeClose", typeof(bool), typeof(AyWindowBase), new PropertyMetadata(false));

        public Grid ayLayerArea = null;

        public Grid ayLayerAboveArea = null;

        public bool CloseIsHideWindow
        {
            get { return (bool)GetValue(CloseIsHideWindowProperty); }
            set { SetValue(CloseIsHideWindowProperty, value); }
        }

        public static readonly DependencyProperty CloseIsHideWindowProperty =
          DependencyProperty.Register("CloseIsHideWindow", typeof(bool), typeof(AyWindowBase), new PropertyMetadata(false));
        private void AyWindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= AyWindowBase_Loaded;
            if (this.WindowState == WindowState.Maximized)
            {
                maxWindowVisibility = Visibility.Collapsed;
                restoreWindowVisibility = Visibility.Visible;
            }
            this.StateChanged += AyWindowBase_StateChanged;
        }
        private void AyWindowBase_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                restoreWindowVisibility = Visibility.Collapsed;
                maxWindowVisibility = Visibility.Visible;
            }
            else
            {
                restoreWindowVisibility = Visibility.Visible;
                maxWindowVisibility = Visibility.Collapsed;

            }
        }


        public Thickness RightButtonGroupMargin
        {
            get { return (Thickness)GetValue(RightButtonGroupMarginProperty); }
            set { SetValue(RightButtonGroupMarginProperty, value); }
        }

        public static readonly DependencyProperty RightButtonGroupMarginProperty =
            DependencyProperty.Register("RightButtonGroupMargin", typeof(Thickness), typeof(AyWindowBase), new PropertyMetadata(new Thickness(0, 4, 4, 0)));


        #region 依赖属性拓展
        public ContextMenu WindowMenu
        {
            get { return (ContextMenu)GetValue(WindowMenuProperty); }
            set { SetValue(WindowMenuProperty, value); }
        }

        public static readonly DependencyProperty WindowMenuProperty =
            DependencyProperty.Register("WindowMenu", typeof(ContextMenu), typeof(AyWindowBase), new PropertyMetadata(null));



        public Visibility WindowMenuVisibility
        {
            get { return (Visibility)GetValue(WindowMenuVisibilityProperty); }
            set { SetValue(WindowMenuVisibilityProperty, value); }
        }

        public static readonly DependencyProperty WindowMenuVisibilityProperty =
            DependencyProperty.Register("WindowMenuVisibility", typeof(Visibility), typeof(AyWindowBase), new PropertyMetadata(Visibility.Collapsed));

        //最大化 需要圆角为0
        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(AyWindowBase), new PropertyMetadata(new CornerRadius(3)));


        /// <summary>
        /// 控制还原图标的可见性，用户无心关心
        /// </summary>
        public Visibility maxWindowVisibility
        {
            get { return (Visibility)GetValue(maxWindowVisibilityProperty); }
            set { SetValue(maxWindowVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for maxWindowVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty maxWindowVisibilityProperty =
            DependencyProperty.Register("maxWindowVisibility", typeof(Visibility), typeof(AyWindowBase), new PropertyMetadata(Visibility.Visible));


        /// <summary>
        /// 控制还原图标的可见性，用户无心关心
        /// </summary>
        public Visibility restoreWindowVisibility
        {
            get { return (Visibility)GetValue(restoreWindowVisibilityProperty); }
            set { SetValue(restoreWindowVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for restoreWindowVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty restoreWindowVisibilityProperty =
            DependencyProperty.Register("restoreWindowVisibility", typeof(Visibility), typeof(AyWindowBase), new PropertyMetadata(Visibility.Collapsed));


        /// <summary>
        /// 最小化按钮是否显示
        /// </summary>
        public Visibility CloseButtonVisibility
        {
            get { return (Visibility)GetValue(CloseButtonVisibilityProperty); }
            set { SetValue(CloseButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonVisibilityProperty =
            DependencyProperty.Register("CloseButtonVisibility", typeof(Visibility), typeof(AyWindowBase), new PropertyMetadata(Visibility.Visible));

        public Visibility MinButtonVisibility
        {
            get { return (Visibility)GetValue(MinButtonVisibilityProperty); }
            set { SetValue(MinButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinButtonVisibilityProperty =
            DependencyProperty.Register("MinButtonVisibility", typeof(Visibility), typeof(AyWindowBase), new PropertyMetadata(Visibility.Visible));



        public Visibility MaxButtonVisibility
        {
            get { return (Visibility)GetValue(MaxButtonVisibilityProperty); }
            set { SetValue(MaxButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxButtonVisibilityProperty =
            DependencyProperty.Register("MaxButtonVisibility", typeof(Visibility), typeof(AyWindowBase), new PropertyMetadata(Visibility.Visible));

        #endregion

        #region 委托事件
        public Action CloseWindowMethodOverride;
        public Action MinWindowMethodOverride;
        public Action MaxWindowMethodOverride;
        //public Action SkinWindowMethodOverride;
        public Action MenuWindowMethodOverride;
        #endregion


        public static ElementMoveMode GetSetElementMove(DependencyObject obj)
        {
            return (ElementMoveMode)obj.GetValue(SetElementMoveProperty);
        }

        public static void SetSetElementMove(DependencyObject obj, ElementMoveMode value)
        {
            obj.SetValue(SetElementMoveProperty, value);
        }
        /// <summary>
        /// 设置元素在aywindow的行为，是左键单击移动，还是也支持 双击最大化窗体
        /// </summary>
        public static readonly DependencyProperty SetElementMoveProperty =
            DependencyProperty.RegisterAttached("SetElementMove", typeof(ElementMoveMode), typeof(AyWindowBase), new PropertyMetadata(ElementMoveMode.None, new PropertyChangedCallback(OnElementMove)));

        private static void OnElementMove(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _2 = d as UIElement;
            var _win = Window.GetWindow(_2) as AyWindowBase;
            if (_win.IsNotNull())
            {
                if (_2.IsNotNull())
                {
                    ElementMoveMode newValue = (ElementMoveMode)e.NewValue;
                    switch (newValue)
                    {
                        case ElementMoveMode.None:
                            break;
                        case ElementMoveMode.Move:
                            AyExtension.SetAyWindowMouseLeftButtonMove(_win, _2);
                            break;
                        case ElementMoveMode.MoveAndDoubleClickMax:
                            AyExtension.SetAyWindowMouseLeftButtonCommonClick(_win, _2);
                            break;
                    }
                }
            }
            else
            {
                if (!WpfTreeHelper.IsInDesignMode)
                {
                    throw new Exception("your window is not inherit from aywindow");
                }

            }


        }
    }
}