using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

/// <summary>
/// 定义一批图像处理的方法
/// </summary>
public class ImageResources
{
    /// <summary>
    /// 先获取属性中的资源，然后转换格式返回
    /// </summary>
    /// <returns></returns>
    public static BitmapSource GetBitmapImage(Bitmap bmap)
    {
        BitmapSource returnSource;
        try
        {
            //直接获取资源
            //Bitmap bmap = TestResource.Properties.Resources.I_001;
            //转换格式
            returnSource = Imaging.CreateBitmapSourceFromHBitmap(bmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bmap.Dispose();
        }
        catch
        {
            returnSource = null;
        }
        return returnSource;
    }


    /// <summary>
    /// 从文件流读取，而后转换为图片格式
    /// </summary>
    /// <param name="qianruresource">资源先设置 嵌入的文件，然后程序集名字.(文件夹名字路径).文件名</param>
    /// <returns></returns>
    public static BitmapSource GetBitmapImage(string qianruresource)
    {
        //获取文件流
        Assembly myAssembly = Assembly.GetExecutingAssembly();
        //格式为：项目名称-文件夹地址-文件名称
        Stream myStream = myAssembly.GetManifestResourceStream(qianruresource);
        //图片格式
        BitmapImage image = new BitmapImage();
        image.BeginInit();
        image.StreamSource = myStream;
        image.EndInit();
        myStream.Dispose();
        myStream.Close();
        return image;
    }
    /// <summary>
    /// ay 2018-6-28 09:51:49
    /// 支持jpg jpeg png gif bmp
    /// </summary>
    /// <param name="by">图片文件二进制</param>
    /// <param name="fileName">文件名，要被存储到的地方 绝对路径</param>
    public static void SaveBitmapImageIntoFile(byte[] by, string fileName)
    {
        BitmapImage bitmapImage = null;
        bitmapImage = ImageResources.ByteArrayToBitmapImage(by, null, null);
        BitmapEncoder encoder = null;
        int index = fileName.LastIndexOf('.');
        string extion = fileName.Substring(
            index + 1, fileName.Length - index - 1);
        extion = extion.ToLower();

        switch (extion)
        {
            case "jpg":
            case "jpeg":
                encoder = new JpegBitmapEncoder();
                break;
            case "png":
                encoder = new PngBitmapEncoder();
                break;
            case "gif":
                encoder = new GifBitmapEncoder();
                break;
            case "bmp":
                encoder = new BmpBitmapEncoder();
                break;
        }
        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
        using (var fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
        {
            encoder.Save(fileStream);
        }
    }

    public static string SaveLocal(string saveName, BitmapSource bitmap)
    {
        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        System.IO.FileStream fileStream = new System.IO.FileStream(saveName, FileMode.Create, FileAccess.ReadWrite);
        encoder.Save(fileStream);
        fileStream.Close();
        return saveName;
    }

    static long maxlong = (long)int.MaxValue;
    /// <summary>
    /// 本地图片非资源的，转换为二进制
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static byte[] GetImageByteByUri(string uri)
    {
        using (BinaryReader binReader = new BinaryReader(File.Open(uri, FileMode.Open)))
        {
            FileInfo fileInfo = new FileInfo(uri);

            if (maxlong > fileInfo.Length)
            {
                byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                return bytes;
            }
            else
            {
                int leng = 1024;
                byte[] bytes = new byte[fileInfo.Length];
                for (long j = 0; j < (fileInfo.Length / (long)leng + (long)1); j++)
                {
                    byte[] b = binReader.ReadBytes(leng);
                    if (b == null || b.Length < 1)
                    {
                        break;
                    }
                    for (long jj = j * leng; jj < (j + 1) * leng; jj++)
                    {
                        bytes[jj] = b[jj % leng];
                    }
                }
                return bytes;
            }
        }
    }


    public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray, int? width, int? height)
    {
        BitmapImage bmp = null;

        try
        {
            bmp = new BitmapImage();
            bmp.BeginInit();
            Stream myStream = new MemoryStream(byteArray);
            bmp.StreamSource = myStream;
            if (width.HasValue)
            {
                bmp.DecodePixelWidth = width.Value;//设置图像的宽度
            }
            if (height.HasValue)
            {
                bmp.DecodePixelHeight = height.Value;//设置图像的高度
            }
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.EndInit();
            myStream.Dispose();
            myStream.Close();
        }
        catch { bmp = null; }
        return bmp;
    }

    /// <summary>
    /// 本地路径的图片打开
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static BitmapImage GetBitmapByUri(string path)
    {
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();//开始更新状态
                                //指定BitmapImage的StreamSource为按指定路径打开的文件流
        bitmapImage.StreamSource = new FileStream(path, FileMode.Open, FileAccess.Read);
        //加载Image后以便立即释放流
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();//结束更新
                              //清除流以避免在尝试删除图像时出现文件访问异常
        bitmapImage.StreamSource.Dispose();
        return bitmapImage;//返回BitmapImage
    }
    /// <summary>
    /// 本地路径的图片打开，同GetBitmapByUri方法，只是不释放Dispose，StreamSource
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static BitmapImage GetBitmapByUri2(string path)
    {
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();//开始更新状态
                                //指定BitmapImage的StreamSource为按指定路径打开的文件流
        bitmapImage.StreamSource = new FileStream(path, FileMode.Open, FileAccess.Read);
        //加载Image后以便立即释放流
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();//结束更新
        return bitmapImage;//返回BitmapImage
    }

    public static byte[] BitmapImageToByteArray(BitmapImage bmp)
    {
        byte[] byteArray = null;

        try
        {
            Stream sMarket = bmp.StreamSource;
            if (sMarket != null && sMarket.Length > 0)     //很重要，因为Position经常位于Stream的末尾，导致下面读取到的长度为0。   
            {
                sMarket.Position = 0;
                using (BinaryReader br = new BinaryReader(sMarket))
                {
                    byteArray = br.ReadBytes((int)sMarket.Length);
                }
            }
        }
        catch
        {

        }
        return byteArray;
    }


    //public  DrawingVisual CreatObjects()
    //{
    //    DrawingVisual visual = new DrawingVisual();

    //    using (DrawingContext dc = visual.RenderOpen())

    //    {
    //        dc.DrawRectangle(System.Windows.Media.Brushes.Brown, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, 1), new Rect(0, 0, 200, 200));
    //        // dc.DrawRectangle(Brushes.Brown, new Pen(Brushes.Black, 1), new Rect(i, i, 200, 200));

    //        BitmapImage image = new BitmapImage();
    //        // image.StreamSource 

    //        image.BeginInit();

    //        image.UriSource = new Uri("..\\..\\Images\\1.png", UriKind.Relative);

    //        image.DecodePixelWidth = 100;

    //        image.DecodePixelHeight = 100;

    //        image.EndInit();

    //        image.Freeze();


    //        dc.DrawImage(image, new Rect(i, 10, 100, 100));
    //    }
    //    return visual;
    //}
}

