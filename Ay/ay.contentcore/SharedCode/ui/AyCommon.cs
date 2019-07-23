using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

public static class AyCommon
{
    private static void WriteObject(object sp)
    {
        PropertyInfo[] propertys = sp.GetType().GetProperties();
        StringBuilder sb=new StringBuilder();
        foreach (PropertyInfo pinfo in propertys)
        {
            sb.Append(pinfo.Name + ":" + pinfo.GetValue(sp, null));
        }
    }

    #region wpf级别的通用转换
    /// <summary>
    /// 将字符串转PackUri的格式字符串
    /// </summary>
    /// <param name="filePath">PackUri的component/后面的路径</param>
    /// <returns>格式化为PackUri后的的字符串</returns>
    public static string ToPackUri(this string filePath)
    {
        return string.Format(@"pack://application:,,,/{0};component/{1}", Application.ResourceAssembly.GetName().Name, filePath);
    }
    /// <summary>
    /// 将字符串转PackUri的格式字符串
    /// </summary>
    /// <param name="filePath">PackUri的component/后面的路径</param>
    /// <param name="assemblyName">程序集名字</param>
    /// <returns></returns>
    public static string ToPackUri(this string filePath, string assemblyName)
    {
        return string.Format(@"pack://application:,,,/{0};component/{1}", assemblyName, filePath);
    }

    /// <summary>
    /// 转换packrui为 资源字典
    /// </summary>
    /// <param name="uri">完整的packuri路径</param>
    /// <returns></returns>
    public static System.Windows.ResourceDictionary ToResourceDictionary(this Uri uri)
    {
        return new System.Windows.ResourceDictionary() { Source = uri };
    }
    /// <summary>
    /// 转换packrui为 资源字典
    /// </summary>
    /// <param name="uri">完整的packuri路径</param>
    /// <returns></returns>
    public static System.Windows.ResourceDictionary ToResourceDictionary(this string uri)
    {
        return new System.Windows.ResourceDictionary() { Source = uri.ToUri() };
    }
    /// <summary>
    /// 转换packrui为 资源字典
    /// </summary>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="filePath">后面路径</param>
    /// <returns></returns>
    public static System.Windows.ResourceDictionary ToResourceDictionary(this string assemblyName, string filePath)
    {
        return new System.Windows.ResourceDictionary() { Source = string.Format(@"pack://application:,,,/{0};component/{1}", assemblyName, filePath).ToUri() };
    }
    public static System.Windows.ResourceDictionary ToApplicationCurrentResourceDictionary(this string filePath)
    {
        return new System.Windows.ResourceDictionary() { Source = string.Format(@"pack://application:,,,/{0};component/{1}", Application.ResourceAssembly.GetName().Name, filePath).ToUri() };
    }


    /// <summary>
    /// ay 2016-8-11 10:00:49
    /// 字符串转 gridlength
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static GridLength ToGridLength(this string str)
    {
        if (str.IndexOf("*") == 0)
        {
            return new GridLength(1, GridUnitType.Star);
        }
        if (str.ToLower().Equals("auto"))
        {
            return GridLength.Auto;
        }
        if (str.IndexOf("*") > -1)
        {
            string _a1 = str.Substring(0, (str.Length - 1));
            return new GridLength(_a1.ToDouble(), GridUnitType.Star);
        }
        return new GridLength(str.ToDouble(), GridUnitType.Pixel);
    }

    public static FontWeight ToFontWeight(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return FontWeights.Normal;
        }
        else
        {
            switch (str)
            {
                case "bold":
                    return FontWeights.Bold;
                case "normal":
                    return FontWeights.Normal;
                case "thin":
                    return FontWeights.Thin;
                case "light":
                    return FontWeights.Light;
                case "regular":
                    return FontWeights.Regular;
                default:
                    return FontWeights.Normal;
            }
        }
    }

    public static Thickness ToThickness(this string str)
    {
        var _1 = str.Split(',');
        return new Thickness(_1[0].ToDouble(), _1[1].ToDouble(), _1[2].ToDouble(), _1[3].ToDouble());
    }

    public static int[] ToIntArray(this string[] region)
    {
        ArrayList aList = new ArrayList();
        foreach (string i in region)
            aList.Add(i.ToInt());
        return (int[])aList.ToArray(typeof(int));
    }

    public static HorizontalAlignment ToHorAlign(this string str)
    {
        if (str.IsNull()) return HorizontalAlignment.Center;
        var _d = str.ToObjectString().ToLower();
        switch (_d)
        {
            case "center":
            case "c":
                return HorizontalAlignment.Center;
            case "left":
            case "l":
                return HorizontalAlignment.Left;
            case "right":
            case "r":
                return HorizontalAlignment.Right;
            case "stretch":
            case "s":
                return HorizontalAlignment.Stretch;
            default:
                return HorizontalAlignment.Stretch;
        }
    }

    public static VerticalAlignment ToVerAlign(this string str)
    {
        if (str.IsNull()) return VerticalAlignment.Center;
        var _d = str.ToObjectString().ToLower();
        switch (_d)
        {
            case "c":
            case "center":
                return VerticalAlignment.Center;
            case "t":
            case "top":
                return VerticalAlignment.Top;
            case "b":
            case "bottom":
                return VerticalAlignment.Bottom;
            case "s":
            case "stretch":
                return VerticalAlignment.Stretch;
            default:
                return VerticalAlignment.Stretch;

        }
    }

    /// <summary>
    /// 计算合并单元格后的宽度
    /// ay
    /// 2016-8-11 10:44:55
    /// </summary>
    /// <param name="str">字符串1 例如 1*</param>
    /// <param name="str2">字符串2 例如 2*</param>
    /// <returns></returns>
    public static GridLength Add(this string str, string str2)
    {
        var _1 = str.ToLower();
        if (_1.ToLower().Equals("auto"))
        {
            return str2.ToGridLength();
        }
        else if (str2.ToLower().Equals("auto"))
        {
            return str.ToGridLength();
        }
        else
        {
            var st1 = str.ToGridLength();
            var st2 = str2.ToGridLength();
            if (st1.GridUnitType == st2.GridUnitType)
            {
                if (st1.GridUnitType == GridUnitType.Star)
                {
                    return new GridLength(st1.Value + st2.Value, GridUnitType.Star);
                }
                else
                {
                    return new GridLength(st1.Value + st2.Value, GridUnitType.Pixel);
                }
            }
            else
            {
                if (st1.GridUnitType == GridUnitType.Star)
                {
                    return st1;
                }
                else
                {
                    return st2;
                }
            }
        }
    }

    #endregion
    #region 功能性质
    /// <summary>
    /// 计算年龄
    /// </summary>
    /// <param name="birthDate">出生日期</param>
    /// <param name="now">现在日期</param>
    /// <returns></returns>
    [Pure]
    public static int CalculateAgeCorrect(DateTime birthDate, DateTime now)
    {
        int age = now.Year - birthDate.Year;
        if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
        return age;
    }


    /// <summary>
    /// 自动生成订单编号  201008251145409865
    /// </summary>
    /// <returns></returns>
    [Pure]
    public static string CreateOrderId()
    {
        string strRandom = AyCommon.Rnd.Next(1000, 10000).ToString(); //生成编号 
        string code = DateTime.Now.ToString("yyyyMMddHHmmss") + strRandom;//形如
        return code;
    }

    /// <summary>
    /// 内存回收
    /// </summary>
    [Pure]
    public static void MemoryGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    [Pure]
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// using包裹source，func处理内容
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="source"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static TResponse UseAndDispose<T, TResponse>(this T source, Func<T, TResponse> func) where T : IDisposable
    {
        using (source)
        {
            return func(source);
        }
    }
    #endregion






    #region "全球唯一码GUID"
    /// <summary>
    /// 获取一个全球唯一码GUID字符串,默认全部转换为小写
    /// 作者：AY
    /// 时间:2016-6-20 00:17:40
    /// </summary>
    [Pure]
    public static string GetGuid
    {
        get
        {
            return Guid.NewGuid().ToString().ToLower();
        }
    }
    /// <summary>
    /// 创建一个GUID，没有"-"分隔符的
    /// 作者：AY
    /// 时间:2016-6-20 00:17:40
    /// </summary>
    [Pure]
    public static string GetGuidNoSplit
    {
        get
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }
    }


    #endregion



    #region 随机数
    private static readonly WeakReference s_random = new WeakReference(null);

    public static Random Rnd
    {
        get
        {
            Contract.Ensures(Contract.Result<Random>() != null);
            var r = (Random)s_random.Target;
            if (r == null)
            {
                s_random.Target = r = new Random();
            }
            return r;
        }
    }
    /// <summary>
    /// 生成指定位数，由0-9组成的随机数
    /// </summary>
    /// <param name="codeNum">位数</param>
    /// <returns></returns>
    [Pure]
    public static string RndNum(int codeNum)
    {
        StringBuilder sb = new StringBuilder(codeNum);
        for (int i = 1; i < codeNum + 1; i++)
        {
            int t = Rnd.Next(9);
            sb.AppendFormat("{0}", t);
        }
        return sb.ToString();

    }


    public static bool NextBool(this Random rnd)
    {
        Contract.Requires(rnd != null);
        return rnd.Next() % 2 == 0;
    }

    public static float NextFloat(this Random rnd, float min = 0, float max = 1)
    {
        Contract.Requires(rnd != null);
        Contract.Requires(max >= min);
        var delta = max - min;
        return (float)rnd.NextDouble() * delta + min;
    }

    /// <summary>
    /// 生成设置范围内的Double的随机数
    /// 例子:_random.AyNextDouble(1.5, 2.5)
    /// 作者：AY
    /// 添加时间：2016-06-19 21:28:24
    /// </summary>
    /// <param name="random">Random对象</param>
    /// <param name="miniDouble">生成随机数的最大值</param>
    /// <param name="maxiDouble">生成随机数的最小值</param>
    /// <returns>当Random等于NULL的时候返回0;</returns>
    public static double NextDouble(this Random random, double miniDouble, double maxiDouble)
    {
        if (random != null)
        {
            return random.NextDouble() * (maxiDouble - miniDouble) + miniDouble;
        }
        else
        {
            return 0.0d;
        }
    }
    #endregion


    #region 系统级别判断
    public static bool ISXP = false;
    public static bool IsXP_OS()
    {
        OperatingSystem os = Environment.OSVersion;
        return (PlatformID.Win32NT == os.Platform && os.Version.Major == 5 && os.Version.Minor == 1) || IsWindows2003;

    }
    //C#判断操作系统是否为Windows2003
    public static bool IsWindows2003
    {
        get
        {
            return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor == 2);
        }
    }
    public static bool IsWindowsVista
    {
        get
        {
            return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor == 0);
        }
    }
    #endregion

    #region 本程序集级别的使用到的
    public static string GetDateString(DateTime? datetime)
    {
        if (datetime.HasValue)
        {
            DateTime dtn = DateTime.Now;
            TimeSpan ts = dtn - datetime.Value;
            if (ts.Days == 0)
            {
                return datetime.Value.ToString("HH:mm");
            }
            else
            if (ts.Days == 1)
            {
                return "昨天";
            }
            else
            if (ts.Days == 2)
            {
                return "前天";
            }
            else
            {
                if (dtn.Year == datetime.Value.Year)
                {
                    return datetime.Value.ToString("MM-dd");
                }
                else
                {
                    return datetime.Value.ToString("yyyy-MM-dd");
                }
            }
        }
        else
        {
            return null;
        }
    }
    public static string GetChatDateString(DateTime? datetime)
    {
        if (datetime.HasValue)
        {
            DateTime dtn = DateTime.Now;
            TimeSpan ts = dtn - datetime.Value;
            if (ts.Days == 0)
            {
                return datetime.Value.ToString("H:mm:ss");
            }
            else
            {
                return datetime.Value.ToString("yyyy-MM-dd H:mm:ss");
            }
        }
        else
        {
            return null;
        }
    }

    #endregion
    #region 进程

    public static Process StartProcess(string filename, string[] args)
    {
        try
        {
            string s = "";
            if (args.IsNotNull())
            {
                foreach (string arg in args)
                {
                    s = s + arg + " ";
                }
            }

            s = s.Trim();
            Process myprocess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo(filename, s);
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            myprocess.StartInfo = startInfo;
            myprocess.StartInfo.UseShellExecute = false;
            myprocess.Start();
            return myprocess;
        }
        catch (Exception ex)
        {
            MessageBox.Show("启动应用程序时出错！原因：" + ex.Message);
        }
        return null;
    }
    public static void GetEmbedResource(string filepath, string resourcename)
    {
        String projectName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();
        using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(projectName + resourcename))
        {
            Byte[] b = new Byte[stream.Length];
            stream.Read(b, 0, b.Length);
            if (File.Exists(filepath))
                File.Delete(filepath);
            if (!File.Exists(filepath))
            {
                using (FileStream f = File.Create(filepath))
                {
                    f.Write(b, 0, b.Length);
                }
            }

        }
    }
    #endregion






}







