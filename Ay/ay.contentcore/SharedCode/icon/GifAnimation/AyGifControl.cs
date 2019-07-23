
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ay.contentcore.Mgr
{
    public class BitmapWithWpf
    {
        private static BitmapWithWpf _Instance;
        public static BitmapWithWpf Instance
        {
            get {
                if (_Instance == null)
                    _Instance = new BitmapWithWpf();
                return _Instance;
            }
       

        }
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool DeleteObject(IntPtr hObject);

        public BitmapSource GetBitmapSource(Bitmap _bitmap)
        {
            BitmapSource _bitmapSource;
            IntPtr handle = IntPtr.Zero;

            try
            {
                handle = _bitmap.GetHbitmap();
                _bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                if (handle != IntPtr.Zero)
                    DeleteObject(handle);
            }

            return _bitmapSource;
        }
    }
    /// <summary>
    /// 创建实例后，设置Image的基本属性
    /// 设置InitControl设置路径
    /// StartAnimation
    /// </summary>
    public class AyGifControl : System.Windows.Controls.Image
    {


        private Bitmap _bitmap; // Local bitmap member to cache image resource

        private BitmapSource _bitmapSource;
        public delegate void FrameUpdatedEventHandler();


        /// <summary>
        /// Delete local bitmap resource
        /// Reference: http://msdn.microsoft.com/en-us/library/dd183539(VS.85).aspx
        /// </summary>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Override the OnInitialized method
        /// </summary>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Loaded += new RoutedEventHandler(AnimatedGIFControl_Loaded);
            this.Unloaded += new RoutedEventHandler(AnimatedGIFControl_Unloaded);
        }

        /// <summary>
        /// Load the embedded image for the Image.Source
        /// </summary>
        public void InitControl(string Icon)
        {
            //var bImage = new BitmapImage();
            //bImage.BeginInit();
            //if (Icon.IndexOf("pack://") == 0)
            //{

            //}
            //else if (Icon.IndexOf(":") < 0)
            //{
            //    Icon = System.IO.Directory.GetCurrentDirectory() + Icon;
            //}
            //bImage.UriSource = new Uri(Icon, UriKind.RelativeOrAbsolute);
            //bImage.EndInit();
            if (Icon.IndexOf("pack://") == 0)
            {
                var bImage = new BitmapImage();
                bImage.BeginInit();
                bImage.UriSource = new Uri(Icon, UriKind.RelativeOrAbsolute);
                bImage.EndInit();
                //_bitmap = (Image)bImage;
            }
            if (Icon.IndexOf(":") < 0)
            {
                Icon = System.IO.Directory.GetCurrentDirectory() + Icon;
            }
            _bitmap = new Bitmap(Icon, false);
        }

        void AnimatedGIFControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Get GIF image from Resources
            if (_bitmap != null)
            {
                //Width = _bitmap.Width;
                //Height = _bitmap.Height;

                _bitmapSource = BitmapWithWpf.Instance.GetBitmapSource(_bitmap);
                Source = _bitmapSource;
            }
        }

        /// <summary>
        /// Close the FileStream to unlock the GIF file
        /// </summary>
        private void AnimatedGIFControl_Unloaded(object sender, RoutedEventArgs e)
        {
            StopAnimate();
        }

        /// <summary>
        /// Start animation
        /// </summary>
        public void StartAnimate()
        {
            ImageAnimator.Animate(_bitmap, OnFrameChanged);
        }

        /// <summary>
        /// Stop animation
        /// </summary>
        public void StopAnimate()
        {
            ImageAnimator.StopAnimate(_bitmap, OnFrameChanged);
        }

        /// <summary>
        /// Event handler for the frame changed
        /// </summary>
        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                   new FrameUpdatedEventHandler(FrameUpdatedCallback));
        }

        private void FrameUpdatedCallback()
        {
            ImageAnimator.UpdateFrames();

            if (_bitmapSource != null)
                _bitmapSource.Freeze();

            _bitmapSource = BitmapWithWpf.Instance.GetBitmapSource(_bitmap);
            Source = _bitmapSource;
            InvalidateVisual();

        }


    }
}
