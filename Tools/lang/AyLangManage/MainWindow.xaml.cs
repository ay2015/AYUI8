using AyLangManage.Properties;
using AyLangManage.SDK;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AyLangManage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string title = "工具-Ay多国语言字典管理190620";
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded += MainWindow_Loaded;
            Listss = new ObservableCollection<DicItem>();
            this.DataContext = Listss;
            this.Title = title;
            resouceDir.Text = Settings.Default.ResourcePathDefault;
            resouceCsDir.Text = Settings.Default.CsResourceDefaultPath;

            if (!string.IsNullOrWhiteSpace(Settings.Default.LastFilePath) && File.Exists(Settings.Default.LastFilePath))
            {
                
                LoadFile(Settings.Default.LastFilePath);
            }
        }
        private ObservableCollection<DicItem> Lists;

        public ObservableCollection<DicItem> Listss
        {
            get { return Lists; }
            set { Lists = value; }
        }


        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Ay多国语言文件 (*.aylang)|*.aylang";
            //openFile.InitialDirectory = Environment.CurrentDirectory + "\\lang";
            if (openFile.ShowDialog() == true)
            {
                try
                {
                    LoadFile(openFile.FileName);
                }
                catch
                {
                    MessageBox.Show("文件错误!", "文件打开失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void LoadFile(string filePath)
        {
            Listss.Clear();
            //
            var file = File.Open(filePath, FileMode.Open);
            List<string> txt = new List<string>();
            LoadTextData(file, txt);
            file.Close();
            file.Dispose();

            foreach (var item in txt)
            {
                string[] resultString = Regex.Split(item, CreateLangStrongFile.splitStr, RegexOptions.IgnoreCase);
                DicItem d = new DicItem();
                d.Key = resultString[0].Replace("\\r\\n", Environment.NewLine).Trim('\"');
                d.TargetValue = resultString[1].Replace("\\r\\n", Environment.NewLine).Trim('\"');
                Lists.Add(d);
            }

            CurrentFileName = filePath;
            this.Title = title + "- 正在编辑：" + filePath;
        }

        private void LoadTextData(FileStream file, List<string> txt)
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


        private void btnResetFile_Click(object sender, RoutedEventArgs e)
        {
            Listss.Clear();
            CurrentFileName = "";
            this.Title = title;
        }
        private string _CurrentFileName;

        public string CurrentFileName
        {
            get { return _CurrentFileName; }
            set
            {

                if (_CurrentFileName != value)
                {
                    _CurrentFileName = value;
                    Settings.Default.LastFilePath = value;
                    Settings.Default.Save();
                }


            }
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Lists)
            {
                sb.AppendLine(string.Format("\"{0}\"{2}\"{1}\"", item.Key.Trim(), item.TargetValue, CreateLangStrongFile.splitStr).Replace("\r\n", "\\r\\n"));
            }
            if (string.IsNullOrEmpty(CurrentFileName))
            {
                var dlg = new SaveFileDialog()
                {
                    Title = "Ay多国语言文件-另存为",
                    DefaultExt = "aylang",
                    Filter = "Ay多国语言文件(*.aylang) | *.aylang"
                };
                if (dlg.ShowDialog() == true)
                {
                    System.IO.File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                    CurrentFileName = dlg.FileName;
                    this.Title = title + "- 正在编辑：" + dlg.FileName;
                }
            }
            else
            {
                System.IO.File.WriteAllText(CurrentFileName, sb.ToString(), Encoding.UTF8);
            }

        }
        private void btnSaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Lists)
            {
                sb.AppendLine(string.Format("\"{0}\"{2}\"{1}\"", item.Key, item.TargetValue, CreateLangStrongFile.splitStr).Replace("\r\n", "\\r\\n"));
            }
            var dlg = new SaveFileDialog()
            {
                Title = "Ay多国语言文件-另存为",
                DefaultExt = "aylang",
                Filter = "Ay多国语言文件(*.aylang) | *.aylang"
            };
            if (dlg.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                CurrentFileName = dlg.FileName;
                this.Title = title + "- 正在编辑：" + dlg.FileName;
            }
        }

        private void DgSceneRecord_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void BtnNewTemplateFile_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Ay多国语言文件 (*.aylang)|*.aylang";
            //openFile.InitialDirectory = Environment.CurrentDirectory + "\\lang";
            if (openFile.ShowDialog() == true)
            {
                try
                {
                    //打开文件参考
                    Listss.Clear();
                    //
                    var file = File.Open(openFile.FileName, FileMode.Open);
                    List<string> txt = new List<string>();
                    LoadTextData(file, txt);
                    file.Close();
                    file.Dispose();

                    foreach (var item in txt)
                    {
                        string[] resultString = Regex.Split(item, CreateLangStrongFile.splitStr, RegexOptions.IgnoreCase);
                        DicItem d = new DicItem();
                        d.Key = resultString[0].Replace("\\r\\n", Environment.NewLine).Trim('\"');
                        d.SampleValue = resultString[1].Replace("\\r\\n", Environment.NewLine).Trim('\"');
                        Lists.Add(d);
                    }
                }
                catch
                {
                    MessageBox.Show("文件错误!", "文件打开失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void BtnMatchFile_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Ay多国语言文件 (*.aylang)|*.aylang";
            //openFile.InitialDirectory = Environment.CurrentDirectory + "\\lang";
            if (openFile.ShowDialog() == true)
            {
                try
                {

                    //
                    var file = File.Open(openFile.FileName, FileMode.Open);
                    List<string> txt = new List<string>();
                    LoadTextData(file, txt);
                    file.Close();
                    file.Dispose();
                    List<DicItem> _tem = new List<DicItem>();
                    foreach (var item in txt)
                    {
                        string[] resultString = Regex.Split(item, CreateLangStrongFile.splitStr, RegexOptions.IgnoreCase);
                        DicItem d = new DicItem();
                        d.Key = resultString[0].Replace("\\r\\n", Environment.NewLine).Trim('\"');
                        d.TargetValue = resultString[1].Replace("\\r\\n", Environment.NewLine).Trim('\"');
                        _tem.Add(d);
                    }
                    foreach (var item in Lists)
                    {
                        var _1 = _tem.FirstOrDefault(x => x.Key == item.Key);
                        if (_1 != null)
                        {
                            item.TargetValue = _1.TargetValue;
                        }
                    }
                    CurrentFileName = openFile.FileName;
                    this.Title = title + "- 正在编辑：" + openFile.FileName;

                }
                catch
                {
                    MessageBox.Show("文件错误!", "文件打开失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnShowSourceColumn_Checked(object sender, RoutedEventArgs e)
        {
            if (dg != null)
                dg.Columns[2].Visibility = Visibility.Visible;
        }

        private void BtnShowSourceColumn_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dg != null)
                dg.Columns[2].Visibility = Visibility.Collapsed;
        }

        private void BtnOpenDir_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", Environment.CurrentDirectory + "\\lang");
        }
        CreateLangStrongFile FileOutPut = new CreateLangStrongFile();
        private void BtnConvertResource_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(resouceDir.Text))
            {
                var dlg = new SaveFileDialog()
                {
                    Title = "另存为",
                    DefaultExt = ".xaml",
                    Filter = "WPF资源字典文件(*.xaml) | *.xaml"
                };
                if (dlg.ShowDialog() == true)
                {
                    FileOutPut.Lists = Lists;
                    bool a = FileOutPut.ConvertResource(dlg.FileName);
                    if (a)
                    {
                        resouceDir.Text = dlg.FileName;
                        Settings.Default.ResourcePathDefault = dlg.FileName;
                        Settings.Default.Save();
                        CreateLangStrongFile.OpenPlaceAndSelectFile(dlg.FileName);
                    }
                }
            }
            else
            {
                FileOutPut.Lists = Lists;
                FileOutPut.ConvertResource(resouceDir.Text);
                CreateLangStrongFile.OpenPlaceAndSelectFile(resouceDir.Text);
            }
        }


        private void BtnConvertCsResource_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(resouceDir.Text))
            {
                var dlg = new SaveFileDialog()
                {
                    Title = "另存为",
                    DefaultExt = ".xaml",
                    Filter = "WPF资源字典文件(*.xaml) | *.xaml"
                };
                if (dlg.ShowDialog() == true)
                {
                    FileOutPut.Lists = Lists;
                    bool a = FileOutPut.ConvertCsResource(dlg.FileName);
                    if (a)
                    {
                        resouceCsDir.Text = dlg.FileName;
                        Settings.Default.CsResourceDefaultPath = dlg.FileName;
                        Settings.Default.Save();
                        CreateLangStrongFile.OpenPlaceAndSelectFile(dlg.FileName);
                    }
                }
            }
            else
            {
                FileOutPut.Lists = Lists;
                FileOutPut.ConvertCsResource(resouceCsDir.Text);
                CreateLangStrongFile.OpenPlaceAndSelectFile(resouceCsDir.Text);
            }

        }


        private void BtnOpenResourceDir_Click(object sender, RoutedEventArgs e)
        {
            CreateLangStrongFile.OpenPlaceAndSelectFile(resouceDir.Text);
        }

        private void BtnOpenCsResourceDir_Click(object sender, RoutedEventArgs e)
        {
            CreateLangStrongFile.OpenPlaceAndSelectFile(resouceCsDir.Text);
        }

        private void BtnConvertAllFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(resouceDir.Text) || string.IsNullOrEmpty(resouceCsDir.Text))
            {
                MessageBox.Show("请先设置资源文件名和C#文件名（包含路径的）");
                return;
            }
            try
            {
                lblCreateStatus.Content = "生成中";
                FileOutPut.Lists = Lists;
                FileOutPut.ConvertAllResource(resouceDir.Text, resouceCsDir.Text);
                lblCreateStatus.Content = "生成成功";

            }
            catch (Exception ex)
            {
                lblCreateStatus.Content = "生成失败";
                MessageBox.Show(ex.Message);
            }

        }
    }
}
