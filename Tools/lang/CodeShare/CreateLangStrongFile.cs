using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AyLangManage.SDK
{
    /// <summary>
    /// AY国际化支持共用类
    /// 2019-6-21 13:41:25
    /// </summary>
    public class CreateLangStrongFile
    {
        public static string splitStr = " :: ";
        /// <summary>
        /// 文件夹路径
        /// </summary>
        /// <param name="dirPath">文件夹目录，会读取多个</param>
        public void ReadLangFiles(string dirPath)
        {
            if (Lists == null)
            {
                Lists = new ObservableCollection<DicItem>();
            }
            else
            {
                Lists.Clear();
            }
            var files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith("aylang")).OrderByDescending(x => x);
            foreach (var item in files)
            {
                ReadLangFile(item);
            }
        }
        /// <summary>
        /// 从aylang文件读取
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadLangFile(string filePath)
        {
            if (Lists == null)
            {
                Lists = new ObservableCollection<DicItem>();
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

        void LoadTextData(FileStream file, List<string> txt)
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
                    if (!a.Contains(CreateLangStrongFile.splitStr))
                    {
                        continue;
                    }
                    txt.Add(a);
                }
            }
        }

        public string templateResource = "<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:s=\"clr-namespace:System;assembly=mscorlib\">\r\n{0}\r\n</ResourceDictionary>";
        public string templateItemResource = @"<s:String x:Key=""{0}""  xml:space=""preserve"">{1}</s:String>";

        string templateCsResource = "public partial class Langs \r\n{{\r\n{0}\r\n}}";
        string templateItemCsResource = "\r\n\t///<summary>\r\n\t///{1}\r\n\t///</summary>\r\n\tpublic const string {0} =@\"{0}\";";

        public ObservableCollection<DicItem> Lists { get; set; }
        /// <summary>
        /// 转换成资源字典
        /// </summary>
        /// <param name="resouceDir"></param>
        /// <returns></returns>
        public bool ConvertResource(string resouceDir, bool ismulti = false)
        {

            try
            {
                if (ismulti)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in Lists)
                    {
                        sb.AppendLine(string.Format(templateItemResource, item.NamePrex + item.Key, item.TargetValue));
                    }
                    System.IO.File.WriteAllText(resouceDir, string.Format(templateResource, sb.ToString()), Encoding.UTF8);
                }
                else
                {

                    string fileName = System.IO.Path.GetFileNameWithoutExtension(resouceDir) + "_";
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in Lists)
                    {
                        sb.AppendLine(string.Format(templateItemResource, fileName + item.Key, item.TargetValue));
                    }
                    System.IO.File.WriteAllText(resouceDir, string.Format(templateResource, sb.ToString()), Encoding.UTF8);
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 转换成强类型C#类
        /// </summary>
        /// <param name="resouceCsDir"></param>
        /// <returns></returns>
        public bool ConvertCsResource(string resouceCsDir, bool ismulti = false)
        {

            try
            {
                if (ismulti)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in Lists)
                    {
                        sb.AppendLine(string.Format(templateItemCsResource, item.NamePrex + item.Key, item.TargetValue.Replace("\r\n", "\\r\\n")));
                    }
                    System.IO.File.WriteAllText(resouceCsDir, string.Format(templateCsResource, sb.ToString()), Encoding.UTF8);
                }
                else
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(resouceCsDir) + "_";
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in Lists)
                    {
                        sb.AppendLine(string.Format(templateItemCsResource, fileName + item.Key, item.TargetValue.Replace("\r\n", "\\r\\n")));
                    }
                    System.IO.File.WriteAllText(resouceCsDir, string.Format(templateCsResource, sb.ToString()), Encoding.UTF8);
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ConvertAllResource(string resouceDir, string resouceCsDir,bool multi=false)
        {
            ConvertResource(resouceDir, multi);
            ConvertCsResource(resouceCsDir, multi);
        }
        public static void OpenPlaceAndSelectFile(string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                System.Diagnostics.Process.Start("Explorer.exe", @"/select," + filename);
            }

        }
    }
}
