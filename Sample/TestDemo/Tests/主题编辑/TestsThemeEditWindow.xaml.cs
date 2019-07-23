using ay.Animate;
using ay.Controls;
using ay.Wpf.Theme.Element;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestDemo
{

    public partial class TestsThemeEditWindow : AyWindowSimple
    {
        public TestsThemeEditWindow()
        {
            InitializeComponent();
            Loaded += TestsThemeEditWindow_Loaded;
        }

        private void TestsThemeEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= TestsThemeEditWindow_Loaded;
            //创建数据源
            DateTime dt = DateTime.Now.AddHours(-12);
            BlogCollection b = new BlogCollection();
            Random rand = new Random();
            for (int i = 1; i <= 100; i++)
            {
                AyBlog ay = new AyBlog();
                ay.Name = "wpf" + i;
                ay.Content = "正在更新,已经过去了第" + i + "天";
                ay.CreateTime = dt.AddMinutes(i);
                ay.ReadCount = rand.Next(100, 1000000);
                b.Add(ay);
            }
    
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("dd");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            root.Resources["OnStatic"] = Colors.Pink;
            root.Resources["OnHover"] = Colors.Yellow;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ((App)App.Current)._theme.AccentBrush = HexToBrush.FromHex("#D18023");
        }

        private void BtnGetWindow_Click(object sender, RoutedEventArgs e)
        {
            TestAyWindowSimple t = new TestAyWindowSimple();
            t.Show();
        }
        private void BtnGetWindow1_Click(object sender, RoutedEventArgs e)
        {
            TestAyWindow t = new TestAyWindow();
            t.Show();
        }
        private void btnShellWindow_Click(object sender, RoutedEventArgs e)
        {
            TestAyWindowShell t = new TestAyWindowShell();
            t.Show();
        }

        private void BtnMessageBox_Click(object sender, RoutedEventArgs e)
        {
            AyMessageBox.ShowError(owner: this, "ayjs.net","测试错误");
        }

    }


}
