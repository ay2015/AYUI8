
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Collections;
using System.Collections.Generic;
using ay.contents;
using System.Windows.Shapes;

namespace ay.Controls
{

    /// <summary>
    /// AY 2015版本窗口
    /// </summary>
    public class AyWindow : AyWindowBase,IAyLayerSupport
    {
        static AyWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AyWindow),
             new FrameworkPropertyMetadata(typeof(AyWindow)));
        }

        #region 方便窗体快速获得Session
        public Hashtable Session
        {
            get { return AYUI.Session; }
        }
        #endregion

        public double WindowIconWidth
        {
            get { return (double)GetValue(WindowIconWidthProperty); }
            set { SetValue(WindowIconWidthProperty, value); }
        }

        public static readonly DependencyProperty WindowIconWidthProperty =
            DependencyProperty.Register("WindowIconWidth", typeof(double), typeof(AyWindow), new PropertyMetadata(16.00));



        public double WindowIconHeight
        {
            get { return (double)GetValue(WindowIconHeightProperty); }
            set { SetValue(WindowIconHeightProperty, value); }
        }

        public static readonly DependencyProperty WindowIconHeightProperty =
            DependencyProperty.Register("WindowIconHeight", typeof(double), typeof(AyWindow), new PropertyMetadata(16.00));



        public Visibility WindowIconVisibility
        {
            get { return (Visibility)GetValue(WindowIconVisibilityProperty); }
            set { SetValue(WindowIconVisibilityProperty, value); }
        }

        public static readonly DependencyProperty WindowIconVisibilityProperty =
            DependencyProperty.Register("WindowIconVisibility", typeof(Visibility), typeof(AyWindow), new PropertyMetadata(Visibility.Visible));



        public Visibility WindowRightButtonGroupVisibility
        {
            get { return (Visibility)GetValue(WindowRightButtonGroupVisibilityProperty); }
            set { SetValue(WindowRightButtonGroupVisibilityProperty, value); }
        }

        public static readonly DependencyProperty WindowRightButtonGroupVisibilityProperty =
            DependencyProperty.Register("WindowRightButtonGroupVisibility", typeof(Visibility), typeof(AyWindow), new PropertyMetadata(Visibility.Visible));


        #region 拓展,右上角按钮显示


        public Visibility SkinButtonVisibility
        {
            get { return (Visibility)GetValue(SkinButtonVisibilityProperty); }
            set { SetValue(SkinButtonVisibilityProperty, value); }
        }


        public static readonly DependencyProperty SkinButtonVisibilityProperty =
            DependencyProperty.Register("SkinButtonVisibility", typeof(Visibility), typeof(AyWindow), new PropertyMetadata(Visibility.Visible));

        #endregion


        public Border AyBackgroundBehindLayer { get; set; }
        public Border AyBackgroundLayer { get; set; }

        private static double shad = 14.00;//控制默认阴影大小

        private double dragClip = 14.00;//控制拖拽的clip
        /// <summary>
        /// 窗体入场模式
        ///  0 默认 啥都有
        ///  1 啥都没
        ///  2 没有阴影
        ///  3 没有背景，方便自定义固定背景
        ///  4 没有背景，方便自定义固定背景，同时也没有顶部右侧自带的按钮
        ///  5 没有背景，方便自定义固定背景，有顶部右侧自带的按钮
        ///  </summary>
        public int WindowEntranceBackgroundMode
        {
            get { return (int)GetValue(WindowEntranceBackgroundModeProperty); }
            set { SetValue(WindowEntranceBackgroundModeProperty, value); }
        }

        public static readonly DependencyProperty WindowEntranceBackgroundModeProperty =
            DependencyProperty.Register("WindowEntranceBackgroundMode", typeof(int), typeof(AyWindow), new PropertyMetadata(0));

        public CornerRadius CloseButtonCornerRadius
        {
            get { return (CornerRadius)GetValue(CloseButtonCornerRadiusProperty); }
            set { SetValue(CloseButtonCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonCornerRadiusProperty =
            DependencyProperty.Register("CloseButtonCornerRadius", typeof(CornerRadius), typeof(AyWindow), new PropertyMetadata(new CornerRadius(0)));




        IntPtr currentWindowIntPtr = IntPtr.Zero;

        public AyWindow()
        {
            //this.AllowsTransparency = true;
            //this.WindowStyle = WindowStyle.None;
            //this.ResizeMode = ResizeMode.NoResize;
            if (!WpfTreeHelper.IsInDesignMode)
            {
                this.SourceInitialized += new EventHandler(AyWindow_SourceInitialized);
                currentWindowIntPtr = new WindowInteropHelper(this).Handle;
            }
        }

        public Stretch BackgroundStretch
        {
            get { return (Stretch)GetValue(BackgroundStretchProperty); }
            set { SetValue(BackgroundStretchProperty, value); }
        }

        public static readonly DependencyProperty BackgroundStretchProperty =
            DependencyProperty.Register("BackgroundStretch", typeof(Stretch), typeof(AyWindow), new PropertyMetadata(Stretch.Fill));



        #region 2016-3-16 21:28:58 

        /// <summary>
        /// 是否遮盖任务栏
        /// </summary>
        public bool IsCoverTaskBar
        {
            get { return (bool)GetValue(IsCoverTaskBarProperty); }
            set { SetValue(IsCoverTaskBarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxCoverTaskBar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCoverTaskBarProperty =
            DependencyProperty.Register("IsCoverTaskBar", typeof(bool), typeof(AyWindow), new PropertyMetadata(false));


        #endregion

        #region resize ay 2016-5-22 15:23:03 
        private void TopThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            TopDrag(e);
        }

        private void TopDrag(System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Height > this.MinHeight)
            {
                double yadjust = this.Height - e.VerticalChange;
                if (yadjust > 0)
                {
                    this.Height = yadjust;
                    this.Top += e.VerticalChange;
                    CommonThumbResize();
                }
            }
            else if (this.Height == this.MinHeight)
            {
                if (e.VerticalChange < 0)
                {
                    this.Top += e.VerticalChange;
                    double yadjust = this.Height - e.VerticalChange;
                    if (yadjust > 0)
                    {
                        this.Height = yadjust;
                        CommonThumbResize();
                    }
                }
            }
            else
            {
                this.Height = this.MinHeight;
            }
        }

        private void BtmThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            BottomDrag(e);
        }

        private void BottomDrag(System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Height > this.MinHeight)
            {
                double yadjust = this.Height + e.VerticalChange;
                if (yadjust > 0)
                {
                    this.Height = yadjust;
                    CommonThumbResize();
                }
            }
            else if (this.Height == this.MinHeight)
            {
                if (e.VerticalChange > 0)
                {
                    double yadjust = this.Height + e.VerticalChange;
                    if (yadjust > 0)
                    {
                        this.Height = yadjust;
                        CommonThumbResize();
                    }
                }
            }
            else
            {
                this.Height = this.MinHeight;
            }
        }

        private void RgtThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            RightDrag(e);
        }

        private void RightDrag(System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Width > this.MinWidth)
            {
                double xadjust = this.Width + e.HorizontalChange;
                if (xadjust > 0)
                {
                    this.Width = xadjust;
                    CommonThumbResize();
                }
            }
            else if (this.Width == this.MinWidth)
            {
                if (e.HorizontalChange > 0)
                {
                    double xadjust = this.Width + e.HorizontalChange;
                    if (xadjust > 0)
                    {
                        this.Width = xadjust;
                        CommonThumbResize();
                    }
                }
            }
            else
            {
                this.Width = this.MinWidth;
            }
        }

        private void LftThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            LeftDrag(e);
        }

        private void LeftDrag(System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Width > this.MinWidth)
            {
                double xadjust = this.Width - e.HorizontalChange;
                if (xadjust > 0)
                {
                    this.Width = xadjust;
                    this.Left += e.HorizontalChange;
                    CommonThumbResize();
                }
            }
            else if (this.Width == this.MinWidth)
            {
                if (e.HorizontalChange < 0)
                {
                    this.Left += e.HorizontalChange;
                    double xadjust = this.Width - e.HorizontalChange;
                    if (xadjust > 0)
                    {
                        this.Width = xadjust;
                        CommonThumbResize();
                    }
                }
            }
            else
            {
                this.Width = this.MinWidth;
            }
        }

        private void LeftTopThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            LeftDrag(e);
            TopDrag(e);
        }
        public void CommonThumbResize()
        {
            if (ResizeWindowInvokeMethod != null)
            {
                ResizeWindowInvokeMethod();
            }

        }


        private void RightBottomThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            RightDrag(e);
            BottomDrag(e);
        }

        private void RightTopThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            RightDrag(e);
            TopDrag(e);
        }

        private void LeftBottomThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            BottomDrag(e);
            LeftDrag(e);
        }
        #endregion



        #region 依赖属性
        /// <summary>
        /// window窗体的标题栏目的背景画刷
        /// </summary>
        public Brush WindowTitleBarBg
        {
            get { return (Brush)GetValue(WindowTitleBarBgProperty); }
            set { SetValue(WindowTitleBarBgProperty, value); }
        }

        public static readonly DependencyProperty WindowTitleBarBgProperty =
            DependencyProperty.Register("WindowTitleBarBg", typeof(Brush), typeof(AyWindow), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// 工具栏内容
        /// </summary>
        public object ToolBarContent
        {
            get { return (object)GetValue(ToolBarContentProperty); }
            set { SetValue(ToolBarContentProperty, value); }
        }

        public static readonly DependencyProperty ToolBarContentProperty =
            DependencyProperty.Register("ToolBarContent", typeof(object), typeof(AyWindow), new PropertyMetadata(null));

        /// <summary>
        /// 标题栏控制
        /// 1 代表移动，双击最大化
        /// 2 代表移动
        /// 0 代表 不做任何操作
        /// </summary>
        private int titleBarClickMode = 1;

        public int TitleBarClickMode
        {
            get { return titleBarClickMode; }
            set { titleBarClickMode = value; }
        }

        private Action titleBarDoubleClickAction = null;

        public Action TitleBarDoubleClickAction
        {
            get { return titleBarDoubleClickAction; }
            set { titleBarDoubleClickAction = value; }
        }




        /// <summary>
        /// 生日 2016-10-14 00:52:19
        /// 增加工具栏 数据模板
        /// </summary>
        public DataTemplate ToolBarContentDataTemplate
        {
            get { return (DataTemplate)GetValue(ToolBarContentDataTemplateProperty); }
            set { SetValue(ToolBarContentDataTemplateProperty, value); }
        }

        public static readonly DependencyProperty ToolBarContentDataTemplateProperty =
            DependencyProperty.Register("ToolBarContentDataTemplate", typeof(DataTemplate), typeof(AyWindow), new PropertyMetadata(null));


        public Thickness CloseButtonMargin
        {
            get { return (Thickness)GetValue(CloseButtonMarginProperty); }
            set { SetValue(CloseButtonMarginProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonMarginProperty =
            DependencyProperty.Register("CloseButtonMargin", typeof(Thickness), typeof(AyWindow), new PropertyMetadata(new Thickness(0)));

        #endregion

        public double TitleBarHeight
        {
            get { return (double)GetValue(TitleBarHeightProperty); }
            set { SetValue(TitleBarHeightProperty, value); }
        }

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register("TitleBarHeight", typeof(double), typeof(AyWindow), new PropertyMetadata(40.00));



        public double RightButtonWidth
        {
            get { return (double)GetValue(RightButtonWidthProperty); }
            set { SetValue(RightButtonWidthProperty, value); }
        }
        public static readonly DependencyProperty RightButtonWidthProperty =
            DependencyProperty.Register("RightButtonWidth", typeof(double), typeof(AyWindow), new PropertyMetadata(40.00));


        private HwndSource hs;
        private void AyWindow_SourceInitialized(object sender, EventArgs e)
        {
            hs = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            hs.AddHook(new HwndSourceHook(WndProc));
        }



        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();//获得当前活动窗体

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:/* WM_GETMINMAXINFO */
                    if (!IsCoverTaskBar)
                    {
                        WmGetMinMaxInfo(hwnd, lParam);
                        handled = true;
                    }
                    break;
                //case 0x0202:
                //    Console.WriteLine("释放鼠标左键");
                //    break;
                //case 28:
                //    Console.WriteLine(msg + ";wParam=>" + wParam.ToString() + ";lParam=>" + lParam.ToString());
                //Console.WriteLine(this.ShowActivated);
                //var _1 = GetActiveWindow() == currentWindowIntPtr;
                //Console.WriteLine(_1);
                //if (!_1)
                //{
                //if (wParam.ToInt() == 0)
                //{
                //    AyTime.setTimeout(300, () =>
                //    {
                //        var _1 = GetActiveWindow() == currentWindowIntPtr;
                //        Console.WriteLine(_1);
                //        //if (_1)
                //        //    DoMinWindow();
                //    });
                //}
                //}
                //return (IntPtr)0;

                //Console.WriteLine(GetAsyncKeyState(0x01));
                //if (wParam.ToInt() == 0)
                //{
                //    AyTime.setTimeout(300, () =>
                //    {
                //        DoMinWindow();
                //    });

                //    return (IntPtr)0;
                //}
                //if ()
                //{

                //}
                //if (Mouse.LeftButton == MouseButtonState.Released)
                //{
                //    if (wParam.ToInt() == 0)
                //    {
                //        DoMinWindow();
                //        return (IntPtr)0;
                //    }
                //}
                // Console.WriteLine(msg + ";wParam=>" + wParam.ToString() + ";lParam=>" + lParam.ToString());
                //if (wParam.ToInt() == 0 )
                //{
                //    DoMinWindow();
                //}
                //if (wParam.ToInt() == 1)
                //{
                //    return (IntPtr)1;
                //}
                //break;
                //case 28:
                //    //Console.WriteLine("左键单击");
                //    Console.WriteLine(msg + ";wParam=>" + wParam.ToString() + ";lParam=>" + lParam.ToString());
                //    if (wParam.ToInt() == 0 )
                //    {
                //        //this.WindowState = WindowState.Minimized;
                //        DoMinWindow();
                //        handled = true;
                //    }
                //    //handled = true;
                //    return (IntPtr)1;
                //    break;
                //case WM_SYSCOMMAND:
                //        if (wParam.ToInt32() == SC_MINIMIZE)
                //        {
                //            this.WindowState = WindowState.Minimized;
                //            return IntPtr.Zero;
                //        }
                //        break;
                //if (m.Msg == WM_SYSCOMMAND)
                //{

                //case 0x0313:
                //    Console.WriteLine("右击了");
                //    break;
                //case 0x0086:
                //    Console.WriteLine("wParam"+wParam.ToString()+";lParam"+lParam.ToString());
                //    Console.WriteLine("左击了");
                //    break;

                //case 0x0086:
                //    if (wParam == (IntPtr)0)
                //    {
                //        return (IntPtr)1;
                //    }
                //    break;
                //case 0x0085:
                //    return (System.IntPtr)0;

                //case 0x0084:
                //    if (!this.AllowsTransparency && this.AyResizeMode != ResizeMode.NoResize)
                //    {
                //        this.mousePoint.X = (int)(short)(lParam.ToInt32() & 0xFFFF);
                //        this.mousePoint.Y = (int)(short)(lParam.ToInt32() >> 16);

                //        handled = true;
                //        if (Math.Abs(this.mousePoint.Y - this.Top) <= this.cornerWidth
                //            && Math.Abs(this.mousePoint.X - this.Left) <= this.cornerWidth)
                //        { // Top-Left  
                //            return new IntPtr((int)WindowWin32.HTTOPLEFT);
                //        }
                //        else if (Math.Abs(this.ActualHeight + this.Top - this.mousePoint.Y) <= this.cornerWidth
                //            && Math.Abs(this.mousePoint.X - this.Left) <= this.cornerWidth)
                //        { // Bottom-Left  
                //            return new IntPtr((int)WindowWin32.HTBOTTOMLEFT);
                //        }
                //        else if (Math.Abs(this.mousePoint.Y - this.Top) <= this.cornerWidth
                //            && Math.Abs(this.ActualWidth + this.Left - this.mousePoint.X) <= this.cornerWidth)
                //        { // Top-Right  
                //            return new IntPtr((int)WindowWin32.HTTOPRIGHT);
                //        }
                //        else if (Math.Abs(this.ActualWidth + this.Left - this.mousePoint.X) <= this.cornerWidth
                //            && Math.Abs(this.ActualHeight + this.Top - this.mousePoint.Y) <= this.cornerWidth)
                //        { // Bottom-Right  
                //            return new IntPtr((int)WindowWin32.HTBOTTOMRIGHT);
                //        }
                //        else if (Math.Abs(this.mousePoint.X - this.Left) <= dborder)
                //        { // Left  
                //            return new IntPtr((int)WindowWin32.HTLEFT);
                //        }
                //        else if (Math.Abs(this.ActualWidth + this.Left - this.mousePoint.X) <= dborder)
                //        { // Right  
                //            return new IntPtr((int)WindowWin32.HTRIGHT);
                //        }
                //        else if (Math.Abs(this.mousePoint.Y - this.Top) <= dborder)
                //        { // Top  
                //            return new IntPtr((int)WindowWin32.HTTOP);
                //        }
                //        else if (Math.Abs(this.ActualHeight + this.Top - this.mousePoint.Y) <= dborder)
                //        { // Bottom  
                //            return new IntPtr((int)WindowWin32.HTBOTTOM);
                //        }
                //        else
                //        {
                //            handled = false;
                //            return IntPtr.Zero;
                //        }
                //    }

                //    break;
                //case 0x0083:
                //    return (System.IntPtr)0;
                //case 0x112:
                //    if (wParam.ToLong() == 0xf120)
                //    {
                //        this.Activate();
                //        return (System.IntPtr)0;
                //    }
                //    if (wParam.ToLong() == 0xF020)
                //    {
                //        DoMinWindow();
                //        return (System.IntPtr)0;
                //    }
                //    if (wParam.ToLong() == 0xF060)
                //    {
                //        DoCloseWindow();
                //        return (System.IntPtr)0;
                //    }


                default: break;
            }

            return System.IntPtr.Zero;
        }


        public Thickness ShadowMargin
        {
            get { return (Thickness)GetValue(ShadowMarginProperty); }
            set { SetValue(ShadowMarginProperty, value); }
        }
        public static readonly DependencyProperty ShadowMarginProperty =
            DependencyProperty.Register("ShadowMargin", typeof(Thickness), typeof(AyWindow), new PropertyMetadata(new Thickness(shad)));


        private double shadowRadius;

        public double ShadowRadius
        {
            get
            {
                return ShadowMargin.Left;
            }
            set
            {
                shadowRadius = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShadowRadius"));
            }
        }

        private Thickness shadowBorderThickness;

        public Thickness ShadowBorderThickness
        {
            get
            {
                return new Thickness(ShadowMargin.Left / 2);
            }
            set
            {
                shadowBorderThickness = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShadowBorderThickness"));
            }
        }



        private Thickness oldShadowMargin = new Thickness(shad);

        #region 这一部分用于最大化时不遮蔽任务栏
        private void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {

            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);


            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }


        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion

        #region 这一部分用于得到元素相对于窗体的位置
        //public Size GetElementPixelSize(UIElement element)
        //{
        //    Matrix transformToDevice;
        //    var source = PresentationSource.FromVisual(element);
        //    if (source != null)
        //        transformToDevice = source.CompositionTarget.TransformToDevice;
        //    else
        //        using (var source1 = new HwndSource(new HwndSourceParameters()))
        //            transformToDevice = source1.CompositionTarget.TransformToDevice;

        //    if (element.DesiredSize == new Size())
        //        element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

        //    return (Size)transformToDevice.Transform((Vector)element.DesiredSize);
        //}
        #endregion

        /// <summary>
        /// wpf window的 resizemode有bug，会产生白线和窗体遮罩错位,很多bug
        /// 2016-10-15 05:32:00
        /// AY
        /// </summary>
        public ResizeMode AyResizeMode
        {
            get { return (ResizeMode)GetValue(AyResizeModeProperty); }
            set { SetValue(AyResizeModeProperty, value); }
        }

        public static readonly DependencyProperty AyResizeModeProperty =
            DependencyProperty.Register("AyResizeMode", typeof(ResizeMode), typeof(AyWindow), new PropertyMetadata(ResizeMode.CanResize));


        #region 2016-4-7 07:33:43 用于控制窗体默认右侧按钮样式，触发事件
        public Action SkinWindowMethodOverride;
        //2016-6-3 11:20:46添加 用于调整窗体大小时候触发委托
        public Action ResizeWindowInvokeMethod;
        #endregion


        #region Resize窗体的控件
        Thumb leftTopThumb = null;
        Thumb rightBottomThumb = null;
        Thumb RightTopThumb = null;
        Thumb leftBottomThumb = null;
        Thumb TopThumb = null;
        Thumb BtmThumb = null;
        Thumb LftThumb = null;
        Thumb RgtThumb = null;
        #endregion
        public void DoHiddenResizeThumb()
        {
            if (this.AyResizeMode == ResizeMode.NoResize) { }
            else
            {
                if (leftTopThumb.IsNotNull())
                {
                    leftTopThumb.Visibility = Visibility.Collapsed;
                }
                if (rightBottomThumb.IsNotNull())
                {
                    rightBottomThumb.Visibility = Visibility.Collapsed;
                }
                if (RightTopThumb.IsNotNull())
                {
                    RightTopThumb.Visibility = Visibility.Collapsed;
                }
                if (leftBottomThumb.IsNotNull())
                {
                    leftBottomThumb.Visibility = Visibility.Collapsed;
                }
                if (TopThumb.IsNotNull())
                {
                    TopThumb.Visibility = Visibility.Collapsed;
                }
                if (BtmThumb.IsNotNull())
                {
                    BtmThumb.Visibility = Visibility.Collapsed;
                }
                if (LftThumb.IsNotNull())
                {
                    LftThumb.Visibility = Visibility.Collapsed;
                }
                if (RgtThumb.IsNotNull())
                {
                    RgtThumb.Visibility = Visibility.Collapsed;
                }
                if (outerDragBorder.IsNotNull())
                {
                    outerDragBorder.Visibility = Visibility.Collapsed;
                }


            }
        }
        public void DoShowResizeThumb()
        {
            if (this.AyResizeMode == ResizeMode.NoResize) { }
            else
            {
                if (outerDragBorder.IsNotNull())
                {
                    outerDragBorder.Visibility = Visibility.Visible;
                }
                if (leftTopThumb.IsNotNull())
                {
                    leftTopThumb.Visibility = Visibility.Visible;
                }
                if (rightBottomThumb.IsNotNull())
                {
                    rightBottomThumb.Visibility = Visibility.Visible;
                }
                if (RightTopThumb.IsNotNull())
                {
                    RightTopThumb.Visibility = Visibility.Visible;
                }
                if (leftBottomThumb.IsNotNull())
                {
                    leftBottomThumb.Visibility = Visibility.Visible;
                }
                if (TopThumb.IsNotNull())
                {
                    TopThumb.Visibility = Visibility.Visible;
                }
                if (BtmThumb.IsNotNull())
                {
                    BtmThumb.Visibility = Visibility.Visible;
                }
                if (LftThumb.IsNotNull())
                {
                    LftThumb.Visibility = Visibility.Visible;
                }
                if (RgtThumb.IsNotNull())
                {
                    RgtThumb.Visibility = Visibility.Visible;
                }

            }

        }


     



     

        public override void OnApplyTemplate()
        {
            ControlTemplate baseWindowTemplate = this.Template;
            ayLayerArea = (Grid)baseWindowTemplate.FindName("AyLayerArea", this);
            ayLayerAboveArea = (Grid)baseWindowTemplate.FindName("AyLayerAboveArea", this);
            AyWindowMaskArea = GetTemplateChild("AyWindowMaskArea") as Rectangle;
            Button closeBtn = (Button)baseWindowTemplate.FindName("PART_CLOSE", this);
            if (closeBtn != null)
            {
                closeBtn.Click += delegate
                {
                    try
                    {
                        if (CloseWindowMethodOverride != null)
                        {
                            CloseWindowMethodOverride();
                        }
                        else
                        {
                            DoCloseWindow();
                        }
                    }
                    catch
                    {

                    }
                };
            }


            Button minBtn = (Button)baseWindowTemplate.FindName("PART_MIN", this);
            if (minBtn != null)
            {
                minBtn.Click += delegate
                {
                    if (MinWindowMethodOverride != null)
                    {
                        MinWindowMethodOverride();
                    }
                    else
                    {
                        DoMinWindow();
                    }

                };
            }

            Button maxBtn = (Button)baseWindowTemplate.FindName("PART_MAX", this);
            if (maxBtn != null)
            {
                maxBtn.Click += delegate
                {
                    if (MaxWindowMethodOverride != null)
                    {
                        MaxWindowMethodOverride();
                    }
                    else
                    {
                        DoRestoreOrMax();
                    }

                };
            }

            Button menuWindow = (Button)baseWindowTemplate.FindName("PART_MENU", this);
            if (menuWindow != null)
            {
                if (WindowMenu != null)
                {
                    WindowMenuVisibility = Visibility.Visible;
                    WindowMenu.Placement = PlacementMode.Bottom;
                    WindowMenu.PlacementTarget = menuWindow;
                }

                menuWindow.Click += delegate
                {
                    if (MenuWindowMethodOverride != null)
                    {
                        MenuWindowMethodOverride();
                    }
                    else
                    {
                        ShowWindowMenu();
                    }

                };
            }

            Button restoreBtn = (Button)baseWindowTemplate.FindName("PART_RESTORE", this);
            if (restoreBtn != null)
            {
                restoreBtn.Click += delegate
                {
                    if (MaxWindowMethodOverride != null)
                    {
                        MaxWindowMethodOverride();
                    }
                    else
                    {
                        DoRestoreOrMax();
                    }

                };
            }
            if (TitleBarClickMode > 0)
            {
                DockPanel topBar = (DockPanel)baseWindowTemplate.FindName("top", this);
                if (topBar != null)
                {

                    if (TitleBarClickMode == 1)
                    {
                        AyExtension.SetAyWindowMouseLeftButtonCommonClick(this, topBar, TitleBarDoubleClickAction);
                    }
                    else if (TitleBarClickMode == 2)
                    {
                        AyExtension.SetAyWindowMouseLeftButtonMove(this, topBar);
                    }

                }
            }

            borderClip = (Grid)baseWindowTemplate.FindName("AyWindowDragBorder", this);
            outerDragBorder = (Grid)baseWindowTemplate.FindName("AyWindowOuterDragBorder", this);


            if (this.WindowState == WindowState.Normal)
            {
                if (this.AllowsTransparency)
                {

                    DoShowResizeThumb();
                }
                else
                {
                    if (IsCoverTaskBar)
                    {
                        ShadowMargin = new Thickness(0);
                        ShadowRadius = 0;
                    }
                    else
                    {
                        ShadowMargin = new Thickness(0);
                        ShadowRadius = 0;

                    }
                }
                restoreWindowVisibility = Visibility.Collapsed;
                maxWindowVisibility = Visibility.Visible;
            }
            else
            {
                restoreWindowVisibility = Visibility.Visible;
                maxWindowVisibility = Visibility.Collapsed;
                DoHiddenResizeThumb();
            }


            if (borderClip != null && this.AyResizeMode != ResizeMode.NoResize)
            {
                if (!this.AllowsTransparency)
                {
                    CreateDragBorderNoRec(outerDragBorder);
                }
                else
                {
                    CreateDragBorder(borderClip);
                }

            }


            if (WindowState == WindowState.Maximized)
            {
                if (IsCoverTaskBar)
                {
                    ShadowMargin = new Thickness(ShadowMargin.Left / 2);
                }
                else
                {
                    ShadowMargin = new Thickness(0);
                }


            }
            AllCP = (ContentPresenter)baseWindowTemplate.FindName("AllCP", this);

            AyBackgroundBehindLayer = (Border)baseWindowTemplate.FindName("AyBackgroundBehindLayer", this);
            AyBackgroundLayer = (Border)baseWindowTemplate.FindName("AyBackgroundLayer", this);

            base.OnApplyTemplate();
        }
        public ContentPresenter AllCP { get; set; }

        Grid borderClip = null;
        Grid outerDragBorder = null;
        #region 为元素注册事件
        private void InitializeEvent()
        {
 

        }


        private void CreateDragBorderNoRec(Grid borderClip)
        {
            borderClip.Children.Clear();
            bool shadow = WindowEntranceBackgroundMode == 1 || WindowEntranceBackgroundMode == 5 || WindowEntranceBackgroundMode == 2;

            leftTopThumb = new Thumb();
            rightBottomThumb = new Thumb();
            RightTopThumb = new Thumb();
            leftBottomThumb = new Thumb();
            TopThumb = new Thumb();
            BtmThumb = new Thumb();
            LftThumb = new Thumb();
            RgtThumb = new Thumb();
            double o = 0.01;
            double lvalue = 12;
            double lrvalue = 12;
            leftTopThumb.Width = leftTopThumb.Height = lrvalue;
            double _tm = 0;
            leftTopThumb.VerticalAlignment = VerticalAlignment.Top;

            leftTopThumb.HorizontalAlignment = HorizontalAlignment.Left;
            leftTopThumb.Cursor = Cursors.SizeNWSE;

            leftTopThumb.Margin = new Thickness(_tm, _tm, 0, 0);
            leftTopThumb.Opacity = o;

            leftTopThumb.DragDelta += LeftTopThumb_DragDelta;

            borderClip.Children.Add(leftTopThumb);





            RightTopThumb.Margin = new Thickness(0, _tm, _tm, 0);
            RightTopThumb.Opacity = o;

            RightTopThumb.Width = RightTopThumb.Height = lrvalue;
            RightTopThumb.VerticalAlignment = VerticalAlignment.Top;
            RightTopThumb.HorizontalAlignment = HorizontalAlignment.Right;
            RightTopThumb.Cursor = Cursors.SizeNESW;
            RightTopThumb.DragDelta += RightTopThumb_DragDelta;
            borderClip.Children.Add(RightTopThumb);




            rightBottomThumb.Margin = new Thickness(0, 0, _tm, _tm);
            rightBottomThumb.Opacity = o;

            rightBottomThumb.Width = rightBottomThumb.Height = lrvalue;
            rightBottomThumb.VerticalAlignment = VerticalAlignment.Bottom;
            rightBottomThumb.HorizontalAlignment = HorizontalAlignment.Right;
            rightBottomThumb.Cursor = Cursors.SizeNWSE;

            rightBottomThumb.DragDelta += RightBottomThumb_DragDelta;

            borderClip.Children.Add(rightBottomThumb);



            rightBottomThumb.Margin = new Thickness(_tm, 0, 0, _tm);
            leftBottomThumb.Opacity = o;

            leftBottomThumb.Width = leftBottomThumb.Height = lrvalue;
            leftBottomThumb.VerticalAlignment = VerticalAlignment.Bottom;
            leftBottomThumb.HorizontalAlignment = HorizontalAlignment.Left;
            leftBottomThumb.Cursor = Cursors.SizeNESW;

            leftBottomThumb.DragDelta += LeftBottomThumb_DragDelta;

            borderClip.Children.Add(leftBottomThumb);


            TopThumb.Margin = new Thickness(lrvalue, -7, lrvalue, 0);
            TopThumb.Opacity = o;

            TopThumb.Height = lvalue;
            TopThumb.VerticalAlignment = VerticalAlignment.Top;
            TopThumb.Cursor = Cursors.SizeNS;

            TopThumb.DragDelta += TopThumb_DragDelta;

            borderClip.Children.Add(TopThumb);




            BtmThumb.Margin = new Thickness(lrvalue, 0, lrvalue, -7);
            BtmThumb.Opacity = o;

            BtmThumb.Height = lvalue;
            BtmThumb.VerticalAlignment = VerticalAlignment.Bottom;
            BtmThumb.Cursor = Cursors.SizeNS;

            BtmThumb.DragDelta += BtmThumb_DragDelta;

            borderClip.Children.Add(BtmThumb);




            LftThumb.Margin = new Thickness(-7, lrvalue, 0, lrvalue);
            LftThumb.Opacity = o;

            LftThumb.Width = lvalue;
            LftThumb.HorizontalAlignment = HorizontalAlignment.Left;
            LftThumb.Cursor = Cursors.SizeWE;

            LftThumb.DragDelta += LftThumb_DragDelta;

            borderClip.Children.Add(LftThumb);


            RgtThumb.Margin = new Thickness(0, lrvalue, -7, lrvalue);
            RgtThumb.Opacity = o;

            RgtThumb.Width = lvalue;
            RgtThumb.HorizontalAlignment = HorizontalAlignment.Right;
            RgtThumb.Cursor = Cursors.SizeWE;

            RgtThumb.DragDelta += RgtThumb_DragDelta;

            borderClip.Children.Add(RgtThumb);
        }

        private void CreateDragBorder(Grid borderClip)
        {
            borderClip.Children.Clear();
            bool shadow = WindowEntranceBackgroundMode == 1 || WindowEntranceBackgroundMode == 5 || WindowEntranceBackgroundMode == 2;

            leftTopThumb = new Thumb();
            rightBottomThumb = new Thumb();
            RightTopThumb = new Thumb();
            leftBottomThumb = new Thumb();
            TopThumb = new Thumb();
            BtmThumb = new Thumb();
            LftThumb = new Thumb();
            RgtThumb = new Thumb();
            double o = 0.01;
            leftTopThumb.Width = leftTopThumb.Height = dragClip;
            double _tm = dragClip / 2;
            leftTopThumb.VerticalAlignment = VerticalAlignment.Top;

            leftTopThumb.HorizontalAlignment = HorizontalAlignment.Left;
            leftTopThumb.Cursor = Cursors.SizeNWSE;
            if (shadow)
            {
                leftTopThumb.Margin = new Thickness(_tm, _tm, 0, 0);
                leftTopThumb.Opacity = o;
            }
            else
            {
                leftTopThumb.Opacity = 0;
            }
            leftTopThumb.DragDelta += LeftTopThumb_DragDelta;

            borderClip.Children.Add(leftTopThumb);




            if (shadow)
            {
                RightTopThumb.Margin = new Thickness(0, _tm, _tm, 0);
                RightTopThumb.Opacity = o;
            }
            else
            {
                RightTopThumb.Opacity = 0;
            }
            RightTopThumb.Width = RightTopThumb.Height = dragClip;
            RightTopThumb.VerticalAlignment = VerticalAlignment.Top;
            RightTopThumb.HorizontalAlignment = HorizontalAlignment.Right;
            RightTopThumb.Cursor = Cursors.SizeNESW;
            RightTopThumb.DragDelta += RightTopThumb_DragDelta;
            borderClip.Children.Add(RightTopThumb);



            if (shadow)
            {
                rightBottomThumb.Margin = new Thickness(0, 0, _tm, _tm);
                rightBottomThumb.Opacity = o;
            }
            else
            {
                rightBottomThumb.Opacity = 0;
            }
            rightBottomThumb.Width = rightBottomThumb.Height = dragClip;
            rightBottomThumb.VerticalAlignment = VerticalAlignment.Bottom;
            rightBottomThumb.HorizontalAlignment = HorizontalAlignment.Right;
            rightBottomThumb.Cursor = Cursors.SizeNWSE;

            rightBottomThumb.DragDelta += RightBottomThumb_DragDelta;

            borderClip.Children.Add(rightBottomThumb);


            if (shadow)
            {
                rightBottomThumb.Margin = new Thickness(_tm, 0, 0, _tm);
                leftBottomThumb.Opacity = o;
            }
            else
            {
                leftBottomThumb.Opacity = 0;
            }
            leftBottomThumb.Width = leftBottomThumb.Height = dragClip;
            leftBottomThumb.VerticalAlignment = VerticalAlignment.Bottom;
            leftBottomThumb.HorizontalAlignment = HorizontalAlignment.Left;
            leftBottomThumb.Cursor = Cursors.SizeNESW;

            leftBottomThumb.DragDelta += LeftBottomThumb_DragDelta;

            borderClip.Children.Add(leftBottomThumb);

            if (shadow)
            {
                var dragClip1 = dragClip + _tm;
                TopThumb.Margin = new Thickness(dragClip1, _tm, dragClip1, 0);
                TopThumb.Opacity = o;
            }
            else
            {
                TopThumb.Margin = new Thickness(dragClip, 0, dragClip, 0);
                TopThumb.Opacity = 0;
            }
            TopThumb.Height = dragClip;
            TopThumb.VerticalAlignment = VerticalAlignment.Top;
            TopThumb.Cursor = Cursors.SizeNS;

            TopThumb.DragDelta += TopThumb_DragDelta;

            borderClip.Children.Add(TopThumb);



            if (shadow)
            {
                var dragClip1 = dragClip + _tm;
                BtmThumb.Margin = new Thickness(dragClip1, 0, dragClip1, _tm);
                BtmThumb.Opacity = o;
            }
            else
            {
                BtmThumb.Margin = new Thickness(dragClip, 0, dragClip, 0);
                BtmThumb.Opacity = 0;
            }

            BtmThumb.Height = dragClip;
            BtmThumb.VerticalAlignment = VerticalAlignment.Bottom;
            BtmThumb.Cursor = Cursors.SizeNS;

            BtmThumb.DragDelta += BtmThumb_DragDelta;

            borderClip.Children.Add(BtmThumb);



            if (shadow)
            {
                var dragClip1 = dragClip + _tm;
                LftThumb.Margin = new Thickness(_tm, dragClip1, 0, dragClip1);
                LftThumb.Opacity = o;
            }
            else
            {
                LftThumb.Margin = new Thickness(0, dragClip, 0, dragClip);
                LftThumb.Opacity = 0;
            }
            LftThumb.Width = dragClip;
            LftThumb.HorizontalAlignment = HorizontalAlignment.Left;
            LftThumb.Cursor = Cursors.SizeWE;

            LftThumb.DragDelta += LftThumb_DragDelta;

            borderClip.Children.Add(LftThumb);

            if (shadow)
            {
                var dragClip1 = dragClip + _tm;
                RgtThumb.Margin = new Thickness(0, dragClip1, _tm, dragClip1);
                RgtThumb.Opacity = o;
            }
            else
            {
                RgtThumb.Margin = new Thickness(0, dragClip, 0, dragClip);
                RgtThumb.Opacity = 0;
            }
            RgtThumb.Width = dragClip;
            RgtThumb.HorizontalAlignment = HorizontalAlignment.Right;
            RgtThumb.Cursor = Cursors.SizeWE;

            RgtThumb.DragDelta += RgtThumb_DragDelta;

            borderClip.Children.Add(RgtThumb);
        }



        public override void DoCloseWindow()
        {
            if (CloseIsHideWindow)
            {
                this.Hide();
            }
            else
            {
                try
                {
                    if (!ComfirmBeforeClose)
                    {
                        this.Close();
                    }
                    else
                    {
                        if (MessageBoxResult.OK == AyMessageBox.ShowQuestionOkCancel(Langs.ay_ConfirmWhenExitApp.Lang(), Langs.share_remind.Lang()))
                        {
                            this.Close();
                        }
                    }

                }
                catch
                {


                }

            }

        }

        public override void DoMinWindow()
        {
            //2017-6-7 13:56:39 AY 如果任务栏没有图标，建议最好托盘图标，或者别的方式调用Show方法
            if (this.ShowInTaskbar)
            {
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                this.Hide();
            }

        }



        public void ShowWindowMenu()
        {
            if (WindowMenu != null)
            {
                WindowMenu.IsOpen = true;
            }
        }



        public override void DoRestoreOrMax()
        {
            if (this.WindowState == WindowState.Normal)
            {

                this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
                //if (IsCoverTaskBar)
                //{
                //    double currentshadow = ShadowMargin.Left / 2;
                //    currentshadow = currentshadow - BorderThickness.Left;
                //    ShadowMargin = new Thickness(currentshadow);
                //    ShadowRadius = 0;
                //}
                //else
                //{
                ShadowMargin = new Thickness(0);
                ShadowRadius = 0;

                //}
                restoreWindowVisibility = Visibility.Visible;
                maxWindowVisibility = Visibility.Collapsed;
                DoHiddenResizeThumb();

            }
            else
            {

                this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
                if (this.AllowsTransparency)
                {
                    ShadowMargin = oldShadowMargin;
                    ShadowRadius = ShadowMargin.Left;


                }
                else
                {
                    //if (IsCoverTaskBar)
                    //{
                    //    double currentshadow = ShadowMargin.Left / 2;
                    //    currentshadow = currentshadow - BorderThickness.Left;
                    //    ShadowMargin = new Thickness(currentshadow);
                    //    ShadowRadius = 0;
                    //}
                    //else
                    //{
                    ShadowMargin = new Thickness(0);
                    ShadowRadius = 0;

                    //}
                }
                restoreWindowVisibility = Visibility.Collapsed;
                maxWindowVisibility = Visibility.Visible;
                DoShowResizeThumb();

            }

        }


        #endregion





        #region 2016-6-20 00:20:22修改 只对当前程序集使用，以前是public的
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int x;
            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            /// Construct a point of coordinates (x,y).
            /// </summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public POINT(Point pt)
            {
                x = Convert.ToInt32(pt.X);
                y = Convert.ToInt32(pt.Y);
            }


        }

        /// <summary>
        /// 窗体大小信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };
        /// <summary> Win32 </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal struct RECT
        {
            /// <summary> Win32 </summary>
            public int left;
            /// <summary> Win32 </summary>
            public int top;
            /// <summary> Win32 </summary>
            public int right;
            /// <summary> Win32 </summary>
            public int bottom;

            /// <summary> Win32 </summary>
            public static readonly RECT Empty = new RECT();

            /// <summary> Win32 </summary>
            public int Width
            {
                get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
            }
            /// <summary> Win32 </summary>
            public int Height
            {
                get { return bottom - top; }
            }

            /// <summary> Win32 </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }


            /// <summary> Win32 </summary>
            public RECT(RECT rcSrc)
            {
                this.left = rcSrc.left;
                this.top = rcSrc.top;
                this.right = rcSrc.right;
                this.bottom = rcSrc.bottom;
            }

            /// <summary> Win32 </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return left >= right || top >= bottom;
                }
            }
            /// <summary> Return a user friendly representation of this struct </summary>
            public override string ToString()
            {
                if (this == RECT.Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }

            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }


            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }

            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MONITORINFO
        {
            /// <summary>
            /// </summary>            
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            /// <summary>
            /// </summary>            
            public RECT rcMonitor = new RECT();

            /// <summary>
            /// </summary>            
            public RECT rcWork = new RECT();

            /// <summary>
            /// </summary>            
            public int dwFlags = 0;
        }
        #endregion


    }
}

