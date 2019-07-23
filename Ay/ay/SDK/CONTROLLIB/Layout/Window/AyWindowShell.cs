using ay.contents;
using ay.Controls.Helper;
using ay.FuncFactory;
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ay.Controls
{

    public class AyWindowShell : AyWindowBase
    {
        static AyWindowShell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AyWindowShell),
             new FrameworkPropertyMetadata(typeof(AyWindowShell)));
        }
        #region 依赖属性


        /// <summary>
        /// 是否固定背景图片
        /// </summary>
        public bool IsPinBackground
        {
            get { return (bool)GetValue(IsPinBackgroundProperty); }
            set { SetValue(IsPinBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IsPinBackgroundProperty =
            DependencyProperty.Register("IsPinBackground", typeof(bool), typeof(AyWindowShell), new PropertyMetadata(false));


        public double WindowIconWidth
        {
            get { return (double)GetValue(WindowIconWidthProperty); }
            set { SetValue(WindowIconWidthProperty, value); }
        }

        public static readonly DependencyProperty WindowIconWidthProperty =
            DependencyProperty.Register("WindowIconWidth", typeof(double), typeof(AyWindowShell), new PropertyMetadata(16.00));



        public double WindowIconHeight
        {
            get { return (double)GetValue(WindowIconHeightProperty); }
            set { SetValue(WindowIconHeightProperty, value); }
        }

        public static readonly DependencyProperty WindowIconHeightProperty =
            DependencyProperty.Register("WindowIconHeight", typeof(double), typeof(AyWindowShell), new PropertyMetadata(16.00));



        public Visibility WindowIconVisibility
        {
            get { return (Visibility)GetValue(WindowIconVisibilityProperty); }
            set { SetValue(WindowIconVisibilityProperty, value); }
        }

        public static readonly DependencyProperty WindowIconVisibilityProperty =
            DependencyProperty.Register("WindowIconVisibility", typeof(Visibility), typeof(AyWindowShell), new PropertyMetadata(Visibility.Visible));



        public Visibility WindowRightButtonGroupVisibility
        {
            get { return (Visibility)GetValue(WindowRightButtonGroupVisibilityProperty); }
            set { SetValue(WindowRightButtonGroupVisibilityProperty, value); }
        }

        public static readonly DependencyProperty WindowRightButtonGroupVisibilityProperty =
            DependencyProperty.Register("WindowRightButtonGroupVisibility", typeof(Visibility), typeof(AyWindowShell), new PropertyMetadata(Visibility.Visible));


        public Visibility SkinButtonVisibility
        {
            get { return (Visibility)GetValue(SkinButtonVisibilityProperty); }
            set { SetValue(SkinButtonVisibilityProperty, value); }
        }


        public static readonly DependencyProperty SkinButtonVisibilityProperty =
            DependencyProperty.Register("SkinButtonVisibility", typeof(Visibility), typeof(AyWindowShell), new PropertyMetadata(Visibility.Visible));



        /// <summary>
        /// 窗体入场模式
        ///  0 默认 啥都有
        ///  1  没有背景,没有右侧按钮组
        ///  2  没有背景
        ///  3 没有右侧自带的按钮
        ///  </summary>
        public int WindowEntranceBackgroundMode
        {
            get { return (int)GetValue(WindowEntranceBackgroundModeProperty); }
            set { SetValue(WindowEntranceBackgroundModeProperty, value); }
        }

        public static readonly DependencyProperty WindowEntranceBackgroundModeProperty =
            DependencyProperty.Register("WindowEntranceBackgroundMode", typeof(int), typeof(AyWindowShell), new FrameworkPropertyMetadata(0));

        public CornerRadius CloseButtonCornerRadius
        {
            get { return (CornerRadius)GetValue(CloseButtonCornerRadiusProperty); }
            set { SetValue(CloseButtonCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonCornerRadiusProperty =
            DependencyProperty.Register("CloseButtonCornerRadius", typeof(CornerRadius), typeof(AyWindowShell), new PropertyMetadata(new CornerRadius(0)));



        private bool isLoadedDefaultBackground = true;

        public bool IsLoadedDefaultBackground
        {
            get { return isLoadedDefaultBackground; }
            set { isLoadedDefaultBackground = value; }
        }

        #endregion
        #region 方便窗体快速获得Session
        public Hashtable Session
        {
            get { return AYUI.Session; }
        }
        #endregion

        public Border AyBackgroundBehindLayer;
        public Border AyBackgroundLayer;
        public ContentPresenter AllCP;
  
        //private WindowResizer wr = null;
        public AyWindowShell()
        {
            //this.AllowsTransparency = true;

            //this.WindowStyle = WindowStyle.None;


            if (!WpfTreeHelper.IsInDesignMode)
            {
                this.Loaded += delegate
                {
                    if (IsLoadedDefaultBackground)
                    {
                        GetWindowBackgroundConfig();
                    }
                };
                ReloadColorfulConfig();
            }
        }


        private AyTransition bgData;

        public AyTransition BgData
        {
            get { return bgData; }
            set
            {
                bgData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BgData"));
            }
        }

        private double gaoSiRadius = 0.00;
        /// <summary>
        /// 增加高斯模糊 
        /// 2015-06-10 14:35:20
        /// 作者 ay
        /// 目的 控制模糊的程度
        /// 
        /// 最后修改:2015年11月30日13:43:01 
        /// 主要通过gdiplus进行模糊设定，等于xp用户，忽略模糊
        /// </summary>
        public double GaoSiRadius
        {
            get { return gaoSiRadius; }
            set
            {
                if (value != GaoSiRadius)
                {
                    gaoSiRadius = value;
                    //OnPropertyChanged(new PropertyChangedEventArgs("GaoSiRadius"));
                    //处理bitmap
                    UpdateImage();
                }
            }
        }

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = true)]
        private static extern void CopyMemory(IntPtr Dest, IntPtr src, int Length);

        private System.Drawing.Bitmap Bmp;
        private IntPtr ImageCopyPointer, ImagePointer;
        private int DataLength;

        public void ReLoadImageBmp(string path)
        {

            if (AyCommon.ISXP)
            {
                ImageBrush ib = new ImageBrush(new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)));
                if (this.BgData != null)
                {
                    ib.Stretch = BackgroundStretch;
                    ib.AlignmentX = AlignmentX.Left;
                    ib.AlignmentY = AlignmentY.Top;
                    BgData.ImBrush = ib;
                }

            }
            else
            {
                if (Bmp != null)
                {
                    Bmp.Dispose();
                    Marshal.FreeHGlobal(ImageCopyPointer);
                }
                try
                {
                    if (path.IndexOf("pack://") == 0)
                    {
                        BitmapSource bitp = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                        Bmp = AyFuncBitmapWithWpf.Instance.GetBitmap(bitp);
                    }
                    else
                    {
                        Bmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(path);
                    }

                    Bmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(path);
                    System.Drawing.Imaging.BitmapData BmpData = new System.Drawing.Imaging.BitmapData();
                    Bmp.LockBits(new System.Drawing.Rectangle(0, 0, Bmp.Width, Bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Bmp.PixelFormat, BmpData);    // 用原始格式LockBits,得到图像在内存中真正地址，这个地址在图像的大小，色深等未发生变化时，每次Lock返回的Scan0值都是相同的。
                    ImagePointer = BmpData.Scan0;                            //  记录图像在内存中的真正地址
                    DataLength = BmpData.Stride * BmpData.Height;           //  记录整幅图像占用的内存大小
                    ImageCopyPointer = Marshal.AllocHGlobal(DataLength);    //  直接用内存数据来做备份，AllocHGlobal在内部调用的是LocalAlloc函数
                    CopyMemory(ImageCopyPointer, ImagePointer, DataLength); //  这里当然也可以用Bitmap的Clone方式来处理，但是我总认为直接处理内存数据比用对象的方式速度快。
                    Bmp.UnlockBits(BmpData);
                    UpdateImage();
                }
                catch (Exception d)
                {
                    MessageBox.Show(d.Message);
                }
            }
        }


        public Stretch BackgroundStretch
        {
            get { return (Stretch)GetValue(BackgroundStretchProperty); }
            set { SetValue(BackgroundStretchProperty, value); }
        }

        public static readonly DependencyProperty BackgroundStretchProperty =
            DependencyProperty.Register("BackgroundStretch", typeof(Stretch), typeof(AyWindowShell), new PropertyMetadata(Stretch.Fill));




        private void UpdateImage()
        {
            string thememode = AyWindowShellConfigSetting.GetXmlValue("themeMode");
            if (thememode == "img")
            {
                if (AyCommon.ISXP)
                {

                }
                else
                {
                    if (Bmp != null)
                    {
                        CopyMemory(ImagePointer, ImageCopyPointer, DataLength);             // 需要恢复原始的图像数据，不然模糊就会叠加了。
                        System.Drawing.Rectangle Rect = new System.Drawing.Rectangle(0, 0, Bmp.Width, Bmp.Height);
                        Bmp.GaussianBlur(ref Rect, (float)GaoSiRadius, false);
                        //Bmp.UsmSharpen(ref Rect,(float)GaoSiRadius, (float)usmSlider.Value);//暂时不支持
                        BitmapSource bs = AyFuncBitmapWithWpf.Instance.GetBitmapSource(Bmp);
                        ImageBrush ib = new ImageBrush(bs);
                        if (this.BgData != null)
                        {
                            ib.Stretch = BackgroundStretch;
                            ib.AlignmentX = AlignmentX.Left;
                            ib.AlignmentY = AlignmentY.Top;
                            BgData.ImBrush = ib;
                        }
                    }

                }

            }
        }

        /// <summary>
        /// GetWindowBackgroundConfig必须要执行完，p才有值，然后updateimage，才能给picture换 imagebrush
        /// </summary>
        AyTransition p;
        public void GetWindowBackgroundConfig()
        {
            string thememode = AyWindowShellConfigSetting.GetXmlValue("themeMode");
            var bgFolder = AyWindowShellConfigSetting.GetXmlValue("bgFolder", "value");//图片文件夹
            if (thememode == "img")
            {
                var path = AyWindowShellConfigSetting.GetXmlValue("skinBg", "value");
                if (path.IndexOf(":") < 0)
                {
                    path = System.IO.Path.Combine(bgFolder, path);
                }

                p = new AyTransitionPicture(path);
                p.Radius = CornerRadius.TopLeft;
                p.StrokeThickness = BorderThickness.Top;
                p.Stroke = BorderBrush;
                this.BgData = p;
                ReLoadImageBmp(path);
            }
            else if (thememode == "color")
            {
                var path = AyWindowShellConfigSetting.GetXmlValue("skinColorBg", "value");
                p = new AyTransitionColor(path);
                p.Radius = CornerRadius.TopLeft;
                p.StrokeThickness = BorderThickness.Top;
                p.Stroke = BorderBrush;
                this.BgData = p;
            }
        }


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
            DependencyProperty.Register("WindowTitleBarBg", typeof(Brush), typeof(AyWindowShell), new PropertyMetadata(null));


        public double TitleBarHeight
        {
            get { return (double)GetValue(TitleBarHeightProperty); }
            set { SetValue(TitleBarHeightProperty, value); }
        }

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register("TitleBarHeight", typeof(double), typeof(AyWindowShell), new PropertyMetadata(40.00));



        public double RightButtonWidth
        {
            get { return (double)GetValue(RightButtonWidthProperty); }
            set { SetValue(RightButtonWidthProperty, value); }
        }

        public static readonly DependencyProperty RightButtonWidthProperty =
            DependencyProperty.Register("RightButtonWidth", typeof(double), typeof(AyWindowShell), new PropertyMetadata(40.00));



        public double RightButtonHeight
        {
            get { return (double)GetValue(RightButtonHeightProperty); }
            set { SetValue(RightButtonHeightProperty, value); }
        }

        public static readonly DependencyProperty RightButtonHeightProperty =
            DependencyProperty.Register("RightButtonHeight", typeof(double), typeof(AyWindowShell), new PropertyMetadata(40.00));



        /// <summary>
        /// 工具栏内容
        /// </summary>
        public object ToolBarContent
        {
            get { return (object)GetValue(ToolBarContentProperty); }
            set { SetValue(ToolBarContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToolBarContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToolBarContentProperty =
            DependencyProperty.Register("ToolBarContent", typeof(object), typeof(AyWindowShell), new PropertyMetadata(null));


        /// <summary>
        /// 生日 2016-10-14 00:52:19
        /// 增加工具栏 数据模板
        /// </summary>
        public DataTemplate ToolBarContentDataTemplate
        {
            get { return (DataTemplate)GetValue(ToolBarContentDataTemplateProperty); }
            set { SetValue(ToolBarContentDataTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToolBarContentDataTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToolBarContentDataTemplateProperty =
            DependencyProperty.Register("ToolBarContentDataTemplate", typeof(DataTemplate), typeof(AyWindowShell), new PropertyMetadata(null));

        public Thickness CloseButtonMargin
        {
            get { return (Thickness)GetValue(CloseButtonMarginProperty); }
            set { SetValue(CloseButtonMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonMarginProperty =
            DependencyProperty.Register("CloseButtonMargin", typeof(Thickness), typeof(AyWindowShell), new PropertyMetadata(new Thickness(0)));

        #endregion

        public Action SkinWindowMethodOverride;
        public Grid ayLayerArea = null;
        //public Grid ayLayerAboveArea = null;

        #region 为元素注册事件
        public override void OnApplyTemplate()
        {
            ControlTemplate baseWindowTemplate = this.Template;
            this.StateChanged += AyWindowShellNew_StateChanged;
            TransitionPresenter tp = (TransitionPresenter)baseWindowTemplate.FindName("tpMainImage", this);
            if (tp != null)
            {
                int a;
                if (int.TryParse(AyWindowShellConfigSetting.GetXmlValue("skinSwitchAnimation"), out a))
                {
                    tp.Transition = AyTransitionGetter.AyTransitionOneWay()[a];
                }
                else
                {
                    tp.Transition = AyTransitionGetter.AyTransitionOneWay()[16];

                }
            }

            Button skinBtn = (Button)baseWindowTemplate.FindName("PART_SKIN", this);
            if (skinBtn != null)
            {
                skinBtn.Click += delegate
                {
                    if (SkinWindowMethodOverride != null)
                    {
                        SkinWindowMethodOverride();
                    }
                    else
                    {
                        OpenSkinWindowShow();
                    }

                };
            }
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

            Button restoreBtn = (Button)baseWindowTemplate.FindName("restoreWindow", this);
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
            AllCP = (ContentPresenter)baseWindowTemplate.FindName("AllCP", this);
            
            ayLayerArea = (Grid)baseWindowTemplate.FindName("AyLayerArea", this);
            ayLayerAboveArea = (Grid)baseWindowTemplate.FindName("AyLayerAboveArea", this);
            AyWindowMaskArea = GetTemplateChild("AyWindowMaskArea") as Rectangle;

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



            if (WindowState == WindowState.Maximized)
            {

            }
            if (p != null)
            {
                p.Radius = CornerRadius.TopLeft;
                p.StrokeThickness = BorderThickness.Top;
                p.Stroke = BorderBrush;
                this.BgData = p;
            }

            AyBackgroundBehindLayer = (Border)baseWindowTemplate.FindName("AyBackgroundBehindLayer", this);
            AyBackgroundLayer = (Border)baseWindowTemplate.FindName("AyBackgroundLayer", this);
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
            if (this.ShowInTaskbar)
            {
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                this.Hide();
            }
        }

        public void OpenSkinWindowShow()
        {
            ShowSkinPop();
        }

     
        public void ShowWindowMenu()
        {
            if (WindowMenu != null)
            {
                WindowMenu.IsOpen = true;
            }
        }

        private void AyWindowShellNew_StateChanged(object sender, EventArgs e)
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
        /// <summary>
        /// 获得窗体所在的屏幕
        /// 2017-10-17 16:27:44
        /// AY
        /// </summary>
        /// <param name="win">窗体</param>
        /// <returns></returns>
        public static System.Windows.Forms.Screen GetScreen(Window win)
        {
            var screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(win).Handle);
            return screen;
        }
        public override void DoRestoreOrMax()
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
            }
            else
            {
                this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);         
            }
        }

        private void ShowSkinPop()
        {
            //AyWindowShellManager<SkinSetWindow>.Show();
        }

        #endregion

        #region 2015-6-11 10:05:00 拓展4块遮罩Rectangle 透明度
        private double rectangleOpacity1 = 1;
        /// <summary>
        /// 左侧菜单
        /// </summary>
        public double RectangleOpacity1
        {
            get { return rectangleOpacity1; }
            set
            {
                rectangleOpacity1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RectangleOpacity1"));
            }
        }
        /// <summary>
        /// 内容区
        /// </summary>
        private double rectangleOpacity2 = 1;

        public double RectangleOpacity2
        {
            get { return rectangleOpacity2; }
            set
            {
                rectangleOpacity2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RectangleOpacity2"));
            }
        }

        /// <summary>
        /// 主界面
        /// </summary>
        private double rectangleOpacity3 = 1;

        public double RectangleOpacity3
        {
            get { return rectangleOpacity3; }
            set
            {
                rectangleOpacity3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RectangleOpacity3"));
            }
        }
        private double rectangleOpacity4 = 1;
        /// <summary>
        /// 待定
        /// </summary>
        public double RectangleOpacity4
        {
            get { return rectangleOpacity4; }
            set
            {
                rectangleOpacity4 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RectangleOpacity4"));
            }
        }

        #endregion


        public void ReloadColorfulConfig()
        {
            GaoSiRadius = AyWindowShellConfigSetting.GetXmlValue("Gaosi").ToDouble();
            RectangleOpacity1 = AyWindowShellConfigSetting.GetXmlValue("RecOpa1").ToDouble();
            RectangleOpacity2 = AyWindowShellConfigSetting.GetXmlValue("RecOpa2").ToDouble();
            RectangleOpacity3 = AyWindowShellConfigSetting.GetXmlValue("RecOpa3").ToDouble();
        }


        #region ayui4.2 2016-7-6 10:11:51增加 给客户端修改背景的权利
        /// <summary>
        /// 给客户端修改背景的权利,但是不做同步，只是临时，同步需要自己额外的处理
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="isNeedGaosi">是否授高斯模糊度影响</param>
        public void SetBackgroundFromName(string path, bool isNeedGaosi = false)
        {
            AyTransitionPicture ap = new AyTransitionPicture(path);
            ap.Radius = this.CornerRadius.TopLeft;
            ap.StrokeThickness = this.BorderThickness.Top;
            this.BgData = ap;
            if (isNeedGaosi)
            {
                this.ReLoadImageBmp(path);
            }
            else
            {
                ImageBrush ib = new ImageBrush(new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)));
                if (this.BgData != null)
                {
                    ib.Stretch = BackgroundStretch;
                    ib.AlignmentX = AlignmentX.Left;
                    ib.AlignmentY = AlignmentY.Top;
                    BgData.ImBrush = ib;
                }
            }
     
        }
        #endregion
    }

}


