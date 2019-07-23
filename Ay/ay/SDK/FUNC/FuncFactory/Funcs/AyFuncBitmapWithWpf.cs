using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ay.FuncFactory
{
    public class AyFuncBitmapWithWpf:AyFuncBase
    {
        private static AyFuncBitmapWithWpf _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncBitmapWithWpf CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncBitmapWithWpf();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncBitmapWithWpf Instance
        {
            get
            {
                return CreateInstance();
            }
        }

        #region 2015年11月30日09:43:18 ay增加，用来处理bitmap和sourcesource

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool DeleteObject(IntPtr hObject);

        public  BitmapSource GetBitmapSource(Bitmap _bitmap)
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

        /// <summary>  
        /// 从bitmap转换成ImageSource  
        /// </summary>  
        /// <param name="icon"></param>  
        /// <returns></returns>  
        public ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
               IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;

        }

        /// <summary>
        /// 将图片Image转换成Byte[]
        /// </summary>
        /// <param name="Image">image对象</param>
        /// <param name="imageFormat">后缀名</param>
        /// <returns></returns>
        public byte[] ImageToBytes(Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {

            if (Image == null) { return null; }
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(Image))
                {
                    Bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return data;
        }

        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    System.Drawing.Color pixelColor = bitmap.GetPixel(i, j);
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        /**/
        /// <summary>
        /// 保存JPG时用
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns>得到指定mimeType的ImageCodecInfo</returns>
        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /**/
        /// <summary>
        /// 保存为JPEG格式，支持压缩质量选项
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="FileName"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        public bool SaveAsJPEG(Bitmap bmp, string FileName, int Qty = 100)
        {
            try
            {
                EncoderParameter p;
                EncoderParameters ps;
                ps = new EncoderParameters(1);
                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Qty);
                ps.Param[0] = p;
                bmp.Save(FileName, GetCodecInfo("image/jpeg"), ps);
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion


        public enum AyImageFormat { JPG, BMP, PNG, GIF, TIF }

        public void GenerateImage(BitmapSource bitmap, AyImageFormat format, Stream destStream)
        {
            BitmapEncoder encoder = null;

            switch (format)
            {
                case AyImageFormat.JPG:
                    encoder = new JpegBitmapEncoder();
                    break;
                case AyImageFormat.PNG:
                    encoder = new PngBitmapEncoder();
                    break;
                case AyImageFormat.BMP:
                    encoder = new BmpBitmapEncoder();
                    break;
                case AyImageFormat.GIF:
                    encoder = new GifBitmapEncoder();
                    break;
                case AyImageFormat.TIF:
                    encoder = new TiffBitmapEncoder();
                    break;
                default:
                    throw new InvalidOperationException();
            }

            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(destStream);
        }

        public ImageSource ConvertIconToImageSource(Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
              hBitmap,
              IntPtr.Zero,
              Int32Rect.Empty,
              BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }

            return wpfBitmap.GetAsFrozen() as ImageSource;
        }

        /// <summary>
        /// 从一个Image对象得到一个BitmapSource对象
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        public System.Windows.Media.Imaging.BitmapSource ConvertScannedImage(System.Drawing.Image imageFile)
        {
            if (imageFile == null)
                return null;

            // save the image out to a temp file
            string fileName = Path.GetTempFileName();

            // this is pretty hokey, but since SaveFile won't overwrite, we
            // need to do something to both guarantee a unique name and
            // also allow SaveFile to write the file
            File.Delete(fileName);

            // now save using the same filename
            imageFile.Save(fileName);
            imageFile.Dispose();

            System.Windows.Media.Imaging.BitmapFrame img;

            // load the file back in to a WPF type, this is just
            // to get around size issues with large scans
            using (FileStream stream = File.OpenRead(fileName))
            {
                img = System.Windows.Media.Imaging.BitmapFrame.Create(stream, System.Windows.Media.Imaging.BitmapCreateOptions.None, System.Windows.Media.Imaging.BitmapCacheOption.OnLoad);

                stream.Close();
            }

            // clean up
            File.Delete(fileName);

            return img;
        }

        /// <summary>
        /// 从一个ImageFilePath对象得到一个BitmapSource对象
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <returns></returns>
        public System.Windows.Media.Imaging.BitmapSource ConvertScannedImage(String imageFilePath)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imageFilePath);
            return ConvertScannedImage(image);
        }

        /// <summary>
        /// 将一个图像资源使用指定的格式压缩。
        /// </summary>
        /// <param name="imageToConvert"></param>
        /// <param name="formatOfImage"></param>
        /// <returns></returns>
        private byte[] ConvertBitmapSourceToByteArray(System.Windows.Media.Imaging.BitmapSource imageToConvert, System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] buffer;
            try
            {
                using (var ms = new MemoryStream())
                {
                    if (System.Drawing.Imaging.ImageFormat.Png == formatOfImage)
                    {
                        var bencoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
                        bencoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(imageToConvert));
                        bencoder.Save(ms);
                    }
                    else if (System.Drawing.Imaging.ImageFormat.Tiff == formatOfImage)
                    {
                        var tencoder = new System.Windows.Media.Imaging.TiffBitmapEncoder();
                        tencoder.Compression = System.Windows.Media.Imaging.TiffCompressOption.Ccitt4;
                        tencoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(imageToConvert));
                        tencoder.Save(ms);
                    }
                    else
                    {

                    }
                    ms.Flush();
                    buffer = ms.GetBuffer();
                }
            }
            catch (Exception) { throw; }

            return buffer;
        }

    }

    
}
