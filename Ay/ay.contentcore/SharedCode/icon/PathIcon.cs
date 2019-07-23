using ay.contentcore;
using ay.contentcore.Mgr;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace ay.contentcore
{
    public class PathIcon
    {
        private static PathIcon _Singleton = null;
        private static object _Lock = new object();
        private static PathIcon CreateInstance()
        {
            if (_Singleton == null)
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new PathIcon();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static PathIcon Instance
        {
            get
            { 
               var _1=  CreateInstance();
                _1.InitXmlToStream();
                return _1;
            }
        }
        public XDocument xmlDoc = null;
        public bool _Init = false;
        public void InitXmlToStream()
        {

            if (!_Init)
            {
                string dllPath = null;
                string _ad = null;
                if (WpfDesign.IsInDesignMode)
                {
                    var _a = Application.Current.Resources["design"] as DesignDevSupport;
                    _ad = System.IO.Path.Combine(_a.ContentDirectory, @"Icon\Path");
                    //_ad = System.IO.Path.Combine(@"E:\新建文件夹\AYUI7\AyWpfProject\bin\Debug\Content", @"Icon\Path");
                }
                else
                {
                    dllPath = Uri.UnescapeDataString(System.IO.Path.GetDirectoryName(new Uri(this.GetType().Assembly.CodeBase).AbsolutePath));
                    _ad = System.IO.Path.Combine(dllPath, @"Content\Icon\Path");
                }
        
                var files = System.IO.Directory.GetFiles(_ad, "*.ayicon", System.IO.SearchOption.AllDirectories);
                if (files.Length > 1)
                {
                    xmlDoc = XDocument.Load(files[0]);
                }
                for (int i = 1; i < files.Length; i++)
                {
                    var xmlDoc1 = XDocument.Load(files[i]);
                    xmlDoc.Root.Add(xmlDoc1.Root.Elements());
                }
                _Init = false;
            }
        }
        /// <summary>  
        /// 返回XMl文件指定元素的指定属性值  
        /// </summary>  
        /// <param name="xmlElement">指定元素</param>  
        /// <param name="xmlAttribute">指定属性</param>  
        /// <param name="reloadXml">是否重新加载XML</param>  
        /// <returns></returns>  
        public string GetIconFromXml(string xmlElement, string xmlAttribute = "value", bool reloadXml = false)
        {
            InitXmlToStream();
            var results = from c in xmlDoc.Descendants(xmlElement)
                          select c;
            string s = null;
            foreach (var result in results)
            {
                s = result.Attribute(xmlAttribute).Value.ToString();
            }
            return s;
        }


        public static string GetIcon(DependencyObject obj)
        {
            return (string)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, string value)
        {
            obj.SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(string), typeof(IIconSupport), new PropertyMetadata(string.Empty, new PropertyChangedCallback(d)));

        private static void d(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IIconSupport p)
            {
                p.LoadIcon();
            }
        }
    }



}
