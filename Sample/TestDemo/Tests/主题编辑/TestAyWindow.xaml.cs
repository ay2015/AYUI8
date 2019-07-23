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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ay.Controls;
using Ay.Framework.WPF.Controls;


namespace TestDemo
{

    public partial class TestAyWindow : AyWindow
    {
        public TestAyWindow()
        {
            InitializeComponent();
        }
        int id = 1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            id++;
            if (id % 2 == 0)
            {
                this.IsCoverTaskBar = true;
            }
            else
            {
                this.IsCoverTaskBar = false;
            }

        }
    }





}
