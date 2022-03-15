
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ay.contentcore
{
    public class AyLangComboBox : ComboBox, ISaveSupport
    {

        public AyLangComboBox()
        {
            Loaded += AyLangComboBox_Loaded;
        }

        public string LangDir { get; set; }
        public void Update()
        {
            Items.Clear();
            LangDir = System.IO.Path.Combine(ContentManager.Instance.ContentFolder, "Lang");
            //初始化下拉集合
            string[] dirs = Directory.GetDirectories(LangDir);
            foreach (var item in dirs)
            {
                ComboBoxItem cboItem = new ComboBoxItem();

                cboItem.Content = Path.GetFileNameWithoutExtension(item);
                this.Items.Add(cboItem);
            }
        }

        private void AyLangComboBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //List<string> result = new List<string>();
            //XmlDocument doc = new XmlDocument();
            //XmlNode node = doc.SelectSingleNode("Application");
            //XmlNodeList childs = node.ChildNodes;
            ////string absolutePath = Host.ResolvePath("pathicon.xml");
            ////doc.Load(absolutePath);
            //foreach (XmlNode ass in childs)
            //{
            //    result.Add(ass.Name);
            //}
            Loaded -= AyLangComboBox_Loaded;
            if (WpfDesign.IsInDesignMode)
            {
                return;
            }
            var _curLang = AyGlobalConfig.ACM["CurrentLang"];
            if (_curLang == "")
            {
                _curLang = "zh-CN";
            }
            Update();
            foreach (var item in Items)
            {
                var _1 = (item as ComboBoxItem);

                if (_1.Content.ToString() == _curLang)
                {
                    _1.IsSelected = true;
                    break;
                }
            }
            SelectionChanged += AyLangComboBox_SelectionChanged;
        }

        private void AyLangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsAutoSave)
            {
                SaveSettingAndAppy();
            }
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        public void SaveSetting()
        {
            if (SelectedItem == null) return;
            var _1 = (SelectedItem as ComboBoxItem).Content as string;
            var _p = System.IO.Path.Combine(LangDir, _1);
            AyGlobalConfig.ACM["CurrentLang"] = _p;

            //Settings.Default.CurrentLang = _1;
            //Settings.Default.Save();
        }
        /// <summary>
        /// 保存并且应用
        /// </summary>
        public void SaveSettingAndAppy()
        {
            if (SelectedItem == null) return;
            var _1 = (SelectedItem as ComboBoxItem).Content as string;
            var _p = System.IO.Path.Combine(LangDir, _1);
            LangService.UpdateLangage(Application.Current, _p);
            //Settings.Default.CurrentLang = _1;
            //Settings.Default.Save();
            AyGlobalConfig.ACM["CurrentLang"] = _p;
        }

        /// <summary>
        /// 是否自动保存并且应用
        /// </summary>
        public bool IsAutoSave
        {
            get { return (bool)GetValue(IsAutoSaveProperty); }
            set { SetValue(IsAutoSaveProperty, value); }
        }

        public static readonly DependencyProperty IsAutoSaveProperty =
            DependencyProperty.Register("IsAutoSave", typeof(bool), typeof(AyLangComboBox), new PropertyMetadata(true));


        /// <summary>
        /// 应用设置
        /// </summary>
        public void AppySetting()
        {
            if (SelectedItem == null) return;
            var _1 = (SelectedItem as ComboBoxItem).Content as string;
            var _p = System.IO.Path.Combine(LangDir, _1);
            LangService.UpdateLangage(Application.Current, _p);
        }
    }
}
