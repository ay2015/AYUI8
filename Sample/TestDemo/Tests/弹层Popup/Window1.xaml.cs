using ay.Animate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TestDemo
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
         
        }


        private AyAniThickness _AyAniThickness;
        public AyAniThickness AyAniThickness
        {
            get
            {
                if (_AyAniThickness == null)
                {
                    _AyAniThickness = new AyAniThickness(rec2);
                    _AyAniThickness.ToThickness = new Thickness(39, 200, 0, 0);
                    _AyAniThickness.AniEasingMode = 2;
                    _AyAniThickness.AnimateSpeed = 800;
                }
                return _AyAniThickness;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            msg.Text = "动画演示中";
            AyAniThickness.Begin();
        }

        private AyAniCanvas _AyAniCanvas;
        public AyAniCanvas AyAniCanvas
        {
            get
            {
                if (_AyAniCanvas == null)
                {
                    _AyAniCanvas = new AyAniCanvas(rec1);
                    _AyAniCanvas.ToCanvasLeft = 200;
                    _AyAniCanvas.AniEasingMode = 2;
                    _AyAniCanvas.ToCanvasTop = 10;
                    _AyAniCanvas.AnimateSpeed = 500;
                }
                return _AyAniCanvas;
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            msg.Text = "动画演示中";
            AyAniCanvas.Begin();
            //var bn = new AyAniCanvas(rec1, () =>
            //{
            //    var bn2 = new AyAniCanvas(rec1, () =>
            //    {
            //        var bn3 = new AyAniCanvas(rec1, () =>
            //        {
            //            msg.Text = "动画完成";
            //        });
            //        bn3.ToCanvasLeft = 10;
            //        bn3.AniEasingMode = 2;
            //        bn3.ToCanvasTop = 10;
            //        bn3.AnimateSpeed = 600;
            //        bn3.
            //    });
            //    bn2.ToCanvasLeft = 300;
            //    bn2.AniEasingMode = 2;
            //    bn2.ToCanvasTop = 300;
            //    bn2.AnimateSpeed = 1000;
            //    bn2.Animate().End();

            //});
            //bn.ToCanvasLeft = 200;
            //bn.AniEasingMode = 2;
            //bn.ToCanvasTop = 10;
            //bn.AnimateSpeed = 1200;
            //bn.Animate().End();

        }


        private AyAniColor _AyAniColor;
        public AyAniColor AyAniColor
        {
            get
            {
                if (_AyAniColor == null)
                {
                    _AyAniColor = new AyAniColor(rec2);
                    _AyAniColor.ToColor = Colors.Green;
                    _AyAniColor.AniPropertyPath = _AyAniColor.SampleFillPropertyPath;
                    _AyAniColor.AnimateSpeed = 2000;
                    _AyAniColor.AniEasingMode = 2;
                }
                return _AyAniColor;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            msg.Text = "动画演示中";
            AyAniColor.Begin();
        }



    }
}

