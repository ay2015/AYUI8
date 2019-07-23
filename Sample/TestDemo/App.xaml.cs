using ay.contentcore;
using ay.Wpf.Theme.Element;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace TestDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public ElementThemeResourceDictionaryBase _theme = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ContentManager.Instance.ApplySetting();
            AyGlobalConfig.AYUI_ConfigFileNamePath = AyGlobalConfig.ReturnCurrentFolderCombinePath2("Content/application.xml");

            _theme = new ElementBlueThemeResourceDictionary();
            _theme.AccentBrush = SolidColorBrushConverter.From16JinZhi("#D80A0A");
            _theme.AddTheme(this);
            //下次切换，应该从merge移除，重新add

        }


    }
}
