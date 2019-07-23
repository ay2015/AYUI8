using ay.contentcore.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ay.contentcore
{
    public class ContentManager
    {
        private static ContentManager _Singleton = null;
        private static object _Lock = new object();
        internal static ContentManager CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new ContentManager();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static ContentManager Instance
        {
            get
            {
                return CreateInstance();
            }
        }

     

        private string _ContentFolder = null;
        /// <summary>
        /// 内容文件夹目录，到Content这个文件夹级别的
        /// </summary>
        public string ContentFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_ContentFolder))
                {
                    var _d = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                    _ContentFolder = System.IO.Path.Combine(_d, "Content");
                }
                return _ContentFolder;
            }
            set
            {
                _ContentFolder = value;
            }
        }
        /// <summary>
        /// 应用设置
        /// </summary>
        public void ApplySetting()
        {
            //读取语言
            var _curLang = Settings.Default.CurrentLang as string;
            string LangDir = System.IO.Path.Combine(ContentManager.Instance.ContentFolder, "Lang");
            if (System.IO.Directory.Exists(LangDir))
            {
                var _p = System.IO.Path.Combine(LangDir, _curLang);
                LangService.UpdateLangage(Application.Current, _p);
            }


            //读取字体设置
            string fontFamilyName = Properties.Settings.Default.LastFontFamily;
            string fontFamilyStretch = Properties.Settings.Default.LastFontStretch;
            string fontFamilyStyle = Properties.Settings.Default.LastFontStyle;
            string fontFamilyWeight = Properties.Settings.Default.LastFontWeight;
            if (!string.IsNullOrWhiteSpace(fontFamilyName))
            {
                Application.Current.Resources["NormalFontFamily"] = new FontFamily(fontFamilyName);
            }
            if (string.IsNullOrWhiteSpace(fontFamilyStretch))
            {
                Application.Current.Resources["NormalFontStretch"] = fontFamilyStretch.ToFontStretch();
            }
            if (string.IsNullOrWhiteSpace(fontFamilyStyle))
            {
                Application.Current.Resources["NormalFontStyle"] = fontFamilyStyle.ToFontStyle();
            }
            if (string.IsNullOrWhiteSpace(fontFamilyWeight))
            {
                Application.Current.Resources["NormalFontWeight"] = fontFamilyWeight.ToFontWeight();
            }

        }
    }
}
