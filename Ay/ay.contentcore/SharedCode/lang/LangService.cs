using ay.contents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

public static class LangService
{
    public static string splitStr = " :: ";

    /// <summary>
    /// 加载一个目录下所有aylang文件到应用程序的资源中，请先确保App.xaml里面第一个合并的资源字典是语言包
    /// </summary>
    /// <param name="languageDir">aylang语言文件的目录</param>
    public static void AddLanguage(Application app, string languageDir)
    {
        if (app.Resources.MergedDictionaries.Count > 0)
        {
            app.Resources.MergedDictionaries.Remove(app.Resources.MergedDictionaries[0]);
        }
        app.Resources.MergedDictionaries.Insert(0, ConvertLangsToResourceDictionary(languageDir));
    }
    /// <summary>
    /// 加载一个目录下所有aylang文件到应用程序的资源中
    /// </summary>
    /// <param name="languageDir">aylang语言文件的目录</param>
    public static void UpdateLangage(Application app, string languageDir)
    {
        List<DicItem> Lists = ConvertLangs(languageDir);
        foreach (var item in Lists)
        {
            app.Resources[item.NamePrex + item.Key] = item.TargetValue;
        }
    }
    /// <summary>
    ///加载一个目录下所有aylang文件到 一个资源字典
    /// </summary>
    /// <param name="language">aylang语言文件的目录</param>
    /// <returns></returns>
    public static List<DicItem> ConvertLangs(string languageDir)
    {
        List<DicItem> Lists = null;
        if (Lists == null)
        {
            Lists = new List<DicItem>();
        }
        else
        {
            Lists.Clear();
        }
        var files = Directory.GetFiles(languageDir, "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith("aylang")).OrderByDescending(x => x);
        foreach (var item in files)
        {
            ReadLangFile(item, Lists);
        }
        return Lists;
    
    }
    public static ResourceDictionary ConvertLangsToResourceDictionary(string languageDir)
    {
        List<DicItem> Lists = ConvertLangs(languageDir);
        ResourceDictionary languageChanged = new ResourceDictionary();
        foreach (var item in Lists)
        {
            languageChanged[item.NamePrex + item.Key] = item.TargetValue;
        }
        return languageChanged;
    }
    static void ReadLangFile(string filePath, List<DicItem> Lists)
    {
        if (Lists == null)
        {
            Lists = new List<DicItem>();
        }
        var file = File.Open(filePath, FileMode.Open);
        List<string> txt = new List<string>();
        LoadTextData(file, txt);
        file.Close();
        file.Dispose();
        string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath) + "_";
        foreach (var item in txt)
        {
            string[] resultString = Regex.Split(item, splitStr, RegexOptions.IgnoreCase);
            DicItem d = new DicItem();
            d.Key = resultString[0].Replace("\\r\\n", Environment.NewLine).Trim('\"');
            d.TargetValue = resultString[1].Replace("\\r\\n", Environment.NewLine).Trim('\"');
            d.NamePrex = fileName;
            Lists.Add(d);
        }
    }
    static void LoadTextData(FileStream file, List<string> txt)
    {
        using (var stream = new StreamReader(file))
        {
            while (!stream.EndOfStream)
            {
                string a = stream.ReadLine();
                if (string.IsNullOrEmpty(a))
                {
                    continue;
                }
                if (!a.Contains(splitStr))
                {
                    continue;
                }
                txt.Add(a);
            }
        }
    }

    /// <summary>
    /// 获得多国语言字符串
    /// </summary>
    /// <param name="langkey">Ay多国语言key,请使用Langs类中的字符传</param>
    /// <returns></returns>
    public static string Lang(this string langkey)
    {
        //从资源字典中获得值
        return Application.Current.Resources[langkey] as string;
    }
}

