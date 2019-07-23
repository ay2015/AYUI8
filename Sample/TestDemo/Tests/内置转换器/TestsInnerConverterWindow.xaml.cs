using ay.Animate;
using System;
using System.Collections.Generic;
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

    public partial class TestsInnerConverterWindow : Window
    {
        public TestsInnerConverterWindow()
        {
           
            InitializeComponent();
            Loaded += TestsInnerConverterWindow_Loaded;
        }

        private void TestsInnerConverterWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= TestsInnerConverterWindow_Loaded;
           txt5.Text= System.AppDomain.CurrentDomain.BaseDirectory + @"TestResource\TestsInnerConverterWindow_1.png";
           spByteArray.DataContext=ImageResources.GetImageByteByUri(System.AppDomain.CurrentDomain.BaseDirectory + @"TestResource\TestsInnerConverterWindow_1.png");
        }
    }

}
