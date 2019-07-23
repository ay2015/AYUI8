using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ay.FuncFactory
{
    /// <summary>
    /// 作用：转换
    /// </summary>
    public partial class AyFuncImage : AyFuncBase
    {
        private static AyFuncImage _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncImage CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncImage();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncImage Instance
        {
            get
            {
                return CreateInstance();
            }
        }


        #region 生成缩略图
        public AyFuncImage()
        {

        }
        /// <summary>
        /// 类的构造函数
        /// </summary>
        /// <param name="ImageFileName">图片文件的全路径名称</param>
        public AyFuncImage(string ImageFileName)
        {
            ResourceImage = Image.FromFile(ImageFileName);
            ErrMessage = "";
        }
        /// <summary>
        /// 初始化需要被处理的对象
        /// </summary>
        /// <param name="ImageFileName">图片文件的全路径名称</param>
        public void SetConstructer(string ImageFileName)
        {
            ResourceImage = Image.FromFile(ImageFileName);
            ErrMessage = "";
        }
        public Image ResourceImage;
        private int ImageWidth;
        private int ImageHeight;

        public string ErrMessage;
        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 生成缩略图重载方法1，返回缩略图的Image对象
        /// 必须先调用SetConstructer
        /// </summary>
        /// <param name="Width">缩略图的宽度</param>
        /// <param name="Height">缩略图的高度</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetReducedImage(int Width, int Height)
        {
            try
            {
                Image ReducedImage;

                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ReducedImage = ResourceImage.GetThumbnailImage(Width, Height, callb, IntPtr.Zero);

                return ReducedImage;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法2，将缩略图文件保存到指定的路径
        /// 必须先调用SetConstructer
        /// </summary>
        /// <param name="Width">缩略图的宽度</param>
        /// <param name="Height">缩略图的高度</param>
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:\Images\filename.jpg</param>
        /// <returns>成功返回true，否则返回false</returns>
        public bool GetReducedImage(int Width, int Height, string targetFilePath)
        {
            try
            {
                Image ReducedImage;

                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ReducedImage = ResourceImage.GetThumbnailImage(Width, Height, callb, IntPtr.Zero);
                string fExt = System.IO.Path.GetExtension(targetFilePath).ToLower();
                ImageFormat imagef = ImageFormat.Jpeg;
                switch (fExt)
                {
                    case ".jpg":
                    case ".jpeg":
                        imagef = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        imagef = ImageFormat.Bmp;
                        break;
                    case ".png":
                        imagef = ImageFormat.Png;
                        break;
                    default:
                        break;
                }
                ReducedImage.Save(@targetFilePath, imagef);

                ReducedImage.Dispose();

                return true;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法3，返回缩略图的Image对象
        /// 必须先调用SetConstructer
        /// </summary>
        /// <param name="Percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>  
        /// <returns>缩略图的Image对象</returns>
        public Image GetReducedImage(double Percent)
        {
            try
            {
                Image ReducedImage;

                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ImageWidth = Convert.ToInt32(ResourceImage.Width * Percent);
                ImageHeight = Convert.ToInt32(ResourceImage.Width * Percent);

                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

                return ReducedImage;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法4，返回缩略图的Image对象
        /// 必须先调用SetConstructer
        /// </summary>
        /// <param name="Percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>  
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:\Images\filename.jpg</param>
        /// <returns>成功返回true,否则返回false</returns>
        public bool GetReducedImage(double Percent, string targetFilePath)
        {
            try
            {
                Image ReducedImage;

                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ImageWidth = Convert.ToInt32(ResourceImage.Width * Percent);
                ImageHeight = Convert.ToInt32(ResourceImage.Width * Percent);

                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);

                ReducedImage.Dispose();

                return true;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 将图片修改到固定尺寸
        /// </summary>
        /// <param name="OriginalFile"></param>
        /// <param name="NewFile"></param>
        /// <param name="NewWidth"></param>
        /// <param name="MaxHeight"></param>
        /// <param name="OnlyResizeIfWider"></param>
        public void ResizeImage(string OriginalFile, string NewFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
        {
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFile);

            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (OnlyResizeIfWider)
            {
                if (FullsizeImage.Width <= NewWidth)
                {
                    NewWidth = FullsizeImage.Width;
                }
            }

            int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
            if (NewHeight > MaxHeight)
            {
                // Resize with height instead
                NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
                NewHeight = MaxHeight;
            }

            System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            FullsizeImage.Dispose();

            // Save resized picture
            NewImage.Save(NewFile);
        }
    }
}
