using Ay.Framework.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ay.Controls
{
    /*
     在需要键盘的地方xaml加上
            <AyPopKeyBoard  x:Name="keyboard" HorizontalAlignment="Stretch" VerticalAlignment="Bottom" Margin="0,0,0,0" Visibility="Collapsed" />
    在   <AyFormInput IsShowAddMinusButton="False" Margin="0,0,10,0" PopKeyBoard="{Binding ElementName=keyboard}" 绑定这个键盘，键盘根据ayforminput的属性加载对应的键盘
   后台 通过  keyboard.HideKeyboard(); 隐藏键盘
     */
    /// <summary>
    // AY 
    //  www.ayjs.net
    /// </summary>
    public partial class AyPopKeyBoard : UserControl
    {
        public AyPopKeyBoard()
        {
            InitializeComponent();
            Loaded += AyPopKeyBoard_Loaded;
        }



        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(AyPopKeyBoard), new PropertyMetadata(false, new PropertyChangedCallback(OnIsOpenChanged)));

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = d as AyPopKeyBoard;
            if (_1 != null)
            {
                _1.akb.Width = SystemParameters.WorkArea.Width;
            }
        }



        public AyFormInput ElementName
        {
            get { return (AyFormInput)GetValue(ElementNameProperty); }
            set { SetValue(ElementNameProperty, value); }
        }

        public static readonly DependencyProperty ElementNameProperty =
            DependencyProperty.Register("ElementName", typeof(AyFormInput), typeof(AyPopKeyBoard), new PropertyMetadata(null, OnElementNameChanged));

        private static void OnElementNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AyPopKeyBoard _ownerclass)
            {
                _ownerclass.akb.ShowPP();
            }
        }


        private void AyPopKeyBoard_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyPopKeyBoard_Loaded;
            this.keypop.CustomPopupPlacementCallback = GetPopupPlacement;


        }

        public void InitInputChild(AyFormInput _1)
        {
            if (_1 != null)
            {
                _1.GotFocus -= txtUN_GotFocus;
                _1.PreviewMouseDown -= txtUN_PreviewMouseDown;
                _1.PreviewTouchDown -= txtUN_PreviewTouchDown;

                _1.GotFocus += txtUN_GotFocus;
                _1.PreviewMouseDown += txtUN_PreviewMouseDown;
                _1.PreviewTouchDown += txtUN_PreviewTouchDown;
            }
        }

        private void txtUN_GotFocus(object sender, RoutedEventArgs e)
        {
            ShowP(sender);
        }

        private void ShowP(object sender)
        {
            keypop.IsOpen = false;
            var _g = sender as AyFormInput;
            if (_g.IsNotNull())
            {
                akb.ElementName = _g;
            }
            if (!keypop.IsOpen)
            {
                keypop.IsOpen = true;
            }
            if (_g.PopKeyBoard!= null)
            {
                _g.PopKeyBoard.akb.ShowPP();
            }
 
        }

        public void HideKeyboard()
        {
            akb.HideKeyboard();
        }

        private void txtUN_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowP(sender);
        }

        private void txtUN_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            ShowP(sender);

        }
        private static CustomPopupPlacement[] GetPopupPlacement(Size popupSize, Size targetSize, Point offset)
        {
            var point = SystemParameters.WorkArea.BottomRight;
            point.Y = point.Y - popupSize.Height;
            return new[] { new CustomPopupPlacement(point, PopupPrimaryAxis.Horizontal) };
        }



    }
}
