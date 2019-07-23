using ay.FuncFactory;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ay.Controls.Helper;

namespace ay.Controls
{
    /// <summary>
    /// AyGaosiBackgroundLayer.xaml 的交互逻辑
    /// </summary>
    public partial class AyGaosiBackgroundLayer : UserControl, INotifyPropertyChanged
    {
        static AyGaosiBackgroundLayer()
        {
            BorderBrushProperty.OverrideMetadata(
             typeof(AyGaosiBackgroundLayer),
             new FrameworkPropertyMetadata(new PropertyChangedCallback(BorderBrushPropertyChanged)));

            BorderThicknessProperty.OverrideMetadata(
 typeof(AyGaosiBackgroundLayer),
 new FrameworkPropertyMetadata(new PropertyChangedCallback(BorderThicknessPropertyChanged)));
        }
     
        public AyGaosiBackgroundLayer()
        {
            InitializeComponent();

            Loaded += AyGaosiBackgroundLayer_Loaded;
        }

        private static void BorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AyGaosiBackgroundLayer layer = d as AyGaosiBackgroundLayer;

            if (layer != null)
            {
                Thickness nValue = (Thickness)e.NewValue;
                if (nValue != null)
                {
                    layer.SetBorderThicknessProperty(nValue.Top);
                }
            }
        }
        public void SetBorderThicknessProperty(double ti)
        {
            if (p != null)
                p.StrokeThickness = ti;
        }

        private static void BorderBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AyGaosiBackgroundLayer layer = d as AyGaosiBackgroundLayer;

            if (layer != null)
            {
                Brush nValue = e.NewValue as Brush;
                if (nValue != null)
                {
                    layer.SetBorderBrushProperty(nValue);
                }
            }
        }
        public void SetBorderBrushProperty(Brush brush)
        {
            if(p!=null)
            p.Stroke = brush;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(AyGaosiBackgroundLayer), new PropertyMetadata(null, SourceChanged));

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AyGaosiBackgroundLayer layer = d as AyGaosiBackgroundLayer;

            if (layer != null)
            {
                string nValue = e.NewValue as string;
                if (nValue != null)
                {
                    if (layer != null)
                        layer.SetSource(nValue);
                }
            }
        }



        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(AyGaosiBackgroundLayer), new PropertyMetadata(new CornerRadius(0), CornerRadiusChanged));
        private static void CornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AyGaosiBackgroundLayer layer = d as AyGaosiBackgroundLayer;

            if (layer != null)
            {
                CornerRadius nValue = (CornerRadius)e.NewValue;
                if (nValue != null)
                {
                    layer.SetCornerRadius(nValue);
                }


            }
        }
        public void SetCornerRadius(CornerRadius radius)
        {
            p.Radius = radius.TopLeft;
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





        public double GaoSiRadius
        {
            get { return (double)GetValue(GaoSiRadiusProperty); }
            set { SetValue(GaoSiRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for gaoSiRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GaoSiRadiusProperty =
            DependencyProperty.Register("GaoSiRadius", typeof(double), typeof(AyGaosiBackgroundLayer), new PropertyMetadata(0.00, GaoSiChanged));


        private static void GaoSiChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AyGaosiBackgroundLayer layer = d as AyGaosiBackgroundLayer;

            if (layer != null)
            {
                layer.UpdateImage();

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
            DependencyProperty.Register("BackgroundStretch", typeof(Stretch), typeof(AyGaosiBackgroundLayer), new PropertyMetadata(Stretch.Fill));


        private void UpdateImage()
        {
            if (AyCommon.ISXP)
            {

            }
            else
            {
                if (Bmp != null)
                {
                    CopyMemory(ImagePointer, ImageCopyPointer, DataLength);
                    System.Drawing.Rectangle Rect = new System.Drawing.Rectangle(0, 0, Bmp.Width, Bmp.Height);
                    Bmp.GaussianBlur(ref Rect, (float)GaoSiRadius, false);
                    //Bmp.UsmSharpen(ref Rect,(float)GaoSiRadius, (float)usmSlider.Value);//暂时不支持
                    BitmapSource bs = AyFuncBitmapWithWpf.Instance.GetBitmapSource(Bmp);
                    ImageBrush ib = new ImageBrush(bs);
                    if (this.BgData != null)
                    {
                        ib.Stretch = BackgroundStretch;
                        BgData.ImBrush = ib;
                    }
                }

            }
        }

        AyTransition p;
        void AyGaosiBackgroundLayer_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyGaosiBackgroundLayer_Loaded;

            tpMainImage.Transition = AyTransitionGetter.AyTransitionOneWay()[2];
        }

        private void SetSource(string source)
        {
            if (source.IndexOf(":") < 0)
            {
                source = System.IO.Directory.GetCurrentDirectory() + source;
            }
            if (source.IndexOf("#") > -1)
            {
                p = new AyTransitionColor(source);
                p.Radius = CornerRadius.TopLeft;
                p.StrokeThickness = BorderThickness.Top;
                p.Stroke = BorderBrush;
                this.BgData = p;
         
            }
            else
            {
                p = new AyTransitionPicture(source);
                p.Radius = CornerRadius.TopLeft;
                p.StrokeThickness = BorderThickness.Top;
                p.Stroke = BorderBrush;
                this.BgData = p;
                ReLoadImageBmp(source);
            }
         
        }



    }
}
