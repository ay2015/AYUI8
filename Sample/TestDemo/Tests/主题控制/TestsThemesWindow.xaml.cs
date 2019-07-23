using ay.Wpf.Theme.Element;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace TestDemo
{

    public partial class TestsThemesWindow : Window
    {
        public TestsThemesWindow()
        {
            InitializeComponent();
            Loaded += TestsThemesWindow_Loaded;
        }

        private void TestsThemesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= TestsThemesWindow_Loaded;
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
            dg1.ItemsSource = b;

            
        }



    }

    public class AyBlog
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public int ReadCount { get; set; }
    }

    public class BlogCollection : ObservableCollection<AyBlog>
    {

    }

}
