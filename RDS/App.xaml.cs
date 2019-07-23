using ay.contentcore;
using ay.Wpf.Theme.Element;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace RDS
{
    public partial class App : Application
    {
        public ElementThemeResourceDictionaryBase _theme = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ContentManager.Instance.ApplySetting();
            _theme = new ElementBlueThemeResourceDictionary();
            _theme.AddTheme(this);
            AyThread.Instance.InitDispatcher(Application.Current.Dispatcher);
            DataFactory.Instance.Init();
            //System.Windows.Forms.Application.EnableVisualStyles();//winform控件启动样式
        }
    }
    /// <summary>
    /// 静态资源App
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 主页面
        /// </summary>
        public static _ViewStart IndexWindow { get; set; }

        public static string IndexView = "pack://application:,,,/RDS;component/Views/Index/IndexView.xaml";
    }
}
