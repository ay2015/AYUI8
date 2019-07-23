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

    public partial class TestsPopupWindow : Window
    {
        public TestsPopupWindow()
        {
            InitializeComponent();
        }

        private void Ee_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            p.IsOpen = true;
        }
        private void Ee_GotKeyboardFocus2(object sender, KeyboardFocusChangedEventArgs e)
        {
            p2.IsOpen = true;
        }

        private void BtnShowExample3_Click(object sender, RoutedEventArgs e)
        {
            p3.ShowPopup();
        }

        private void BtnHideExample3_Click(object sender, RoutedEventArgs e)
        {
            p3.HidePopup();
        }


        #region 动画测试代码区域
        #region 动画1 测试


        private AyAniBounce _Bounce;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounce Bounce
        {
            get
            {
                if (_Bounce == null)
                {
                    _Bounce = new AyAniBounce(rectMenuTestArea);
                    _Bounce.AnimateSpeed = 3000;
                }
                return _Bounce;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Bounce.Begin();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Bounce.Pauze();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Bounce.Resume();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Bounce.Stop();
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Bounce.Destroy();
        }
        #endregion
        #region 动画2 测试
        private AyAniBounce _Bounce2;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounce Bounce2
        {
            get
            {
                if (_Bounce2 == null)
                {
                    _Bounce2 = new AyAniBounce(rectMenuTestArea);
                    _Bounce2.Completed += () =>
                    {
                        tbAniResult.Text = "动画2 完成";
                    };
                }
                return _Bounce2;
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (Bounce2.Story != null)
            {
                tbAniResult.Text = "动画2 开始";
            }
            Bounce2.Begin();
        }

        private void Button2_Click_1(object sender, RoutedEventArgs e)
        {
            if (Bounce2.Story != null)
            {
                tbAniResult.Text = "动画2 暂停";
                Bounce2.Pauze();
            }
        }
        private void Button2_Click_2(object sender, RoutedEventArgs e)
        {
            if (Bounce2.Story != null)
            {
                tbAniResult.Text = "动画2 继续";
                Bounce2.Resume();
            }
        }
        private void Button2_Click_3(object sender, RoutedEventArgs e)
        {
            if (Bounce2.Story != null)
            {
                tbAniResult.Text = "动画2 停止";
                Bounce2.Stop();
            }
        }
        private void Button2_Click_4(object sender, RoutedEventArgs e)
        {
            Bounce2.Destroy();
            if (Bounce2.Story == null)
            {
                tbAniResult.Text = "动画2 已经销毁";
            }

        }

        #endregion

        #region 动画3 测试
        private AyAniBounce _Bounce3;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounce Bounce3
        {
            get
            {
                if (_Bounce3 == null)
                {
                    _Bounce3 = new AyAniBounce(rectMenuTestArea, true);
                    _Bounce3.Completed += () =>
                    {
                        tbAniResult3.Text = "动画3 完成,如果结束后再次单击没反应，则销毁了";
                    };
                }
                return _Bounce3;
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (Bounce3.Story != null)
            {
                tbAniResult3.Text = "动画3 开始";
            }
            Bounce3.Begin();
        }
        private void Button3_Click_1(object sender, RoutedEventArgs e)
        {
            if (Bounce3.Story == null)
            {
                tbAniResult3.Text = "动画3 重新初始化了";
                Bounce3.IsInitialized = false;
            }
        }
        #endregion
        #region 动画4 测试
        private AyAniBounce _Bounce4;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounce Bounce4
        {
            get
            {
                if (_Bounce4 == null)
                {
                    _Bounce4 = new AyAniBounce(rectMenuTestArea);
                    _Bounce4.Completed += () =>
                    {
                        tbAniResult4.Text = "动画4 完成";
                    };
                }
                return _Bounce4;
            }
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            if (Bounce4.Story != null)
            {
                tbAniResult4.Text = "动画4 开始";
            }
            Bounce4.Begin();
        }
        #endregion

        #endregion

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            GC.Collect(0, GCCollectionMode.Forced);
        }

        #region 动画测试区域
        private AyAniBounceIn _BounceIn;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceIn BounceIn
        {
            get
            {
                if (_BounceIn == null)
                {
                    _BounceIn = new AyAniBounceIn(rect2);
                }
                return _BounceIn;
            }
        }

        private void AniBounceIn_Click(object sender, RoutedEventArgs e)
        {
            BounceIn.Begin();
        }

        private AyAniBounceInDown _BounceInDown;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceInDown BounceInDown
        {
            get
            {
                if (_BounceInDown == null)
                {
                    _BounceInDown = new AyAniBounceInDown(rect2);
                }
                return _BounceInDown;
            }
        }

        private void AniBounceInDown_Click(object sender, RoutedEventArgs e)
        {
            BounceInDown.Begin();
        }

        private AyAniBounceInUp _BounceInUp;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceInUp BounceInUp
        {
            get
            {
                if (_BounceInUp == null)
                {
                    _BounceInUp = new AyAniBounceInUp(rect2);
                }
                return _BounceInUp;
            }
        }

        private void AniBounceInUp_Click(object sender, RoutedEventArgs e)
        {
            BounceInUp.Begin();
        }
        private AyAniBounceInLeft _BounceInLeft;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceInLeft BounceInLeft
        {
            get
            {
                if (_BounceInLeft == null)
                {
                    _BounceInLeft = new AyAniBounceInLeft(rect2);
                }
                return _BounceInLeft;
            }
        }

        private void AniBounceInLeft_Click(object sender, RoutedEventArgs e)
        {
            BounceInLeft.Begin();
        }
        private AyAniBounceInRight _BounceInRight;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceInRight BounceInRight
        {
            get
            {
                if (_BounceInRight == null)
                {
                    _BounceInRight = new AyAniBounceInRight(rect2);
                }
                return _BounceInRight;
            }
        }

        private void AniBounceInRight_Click(object sender, RoutedEventArgs e)
        {
            BounceInRight.Begin();
        }

        private AyAniSlideInDown _SlideInDown;
        public AyAniSlideInDown SlideInDown
        {
            get
            {
                if (_SlideInDown == null)
                {
                    _SlideInDown = new AyAniSlideInDown(rect2);
                }
                return _SlideInDown;
            }
        }

        private void AyAniSlideInDown_Click(object sender, RoutedEventArgs e)
        {
            SlideInDown.Begin();
        }


        private AyAniSlideInUp _SlideInUp;
        public AyAniSlideInUp SlideInUp
        {
            get
            {
                if (_SlideInUp == null)
                {
                    _SlideInUp = new AyAniSlideInUp(rect2);
                }
                return _SlideInUp;
            }
        }

        private void AyAniSlideInUp_Click(object sender, RoutedEventArgs e)
        {
            SlideInUp.Begin();
        }


        private AyAniSlideInLeft _SlideInLeft;
        public AyAniSlideInLeft SlideInLeft
        {
            get
            {
                if (_SlideInLeft == null)
                {
                    _SlideInLeft = new AyAniSlideInLeft(rect2);
                }
                return _SlideInLeft;
            }
        }

        private void AyAniSlideInLeft_Click(object sender, RoutedEventArgs e)
        {
            SlideInLeft.Begin();
        }

        private AyAniSlideInRight _SlideInRight;
        public AyAniSlideInRight SlideInRight
        {
            get
            {
                if (_SlideInRight == null)
                {
                    _SlideInRight = new AyAniSlideInRight(rect2);
                }
                return _SlideInRight;
            }
        }

        private void AyAniSlideInRight_Click(object sender, RoutedEventArgs e)
        {
            SlideInRight.Begin();
        }








        private AyAniSlideOutDown _SlideOutDown;
        public AyAniSlideOutDown SlideOutDown
        {
            get
            {
                if (_SlideOutDown == null)
                {
                    _SlideOutDown = new AyAniSlideOutDown(rect2);
                }
                return _SlideOutDown;
            }
        }

        private void AyAniSlideOutDown_Click(object sender, RoutedEventArgs e)
        {
            SlideOutDown.Begin();
        }


        private AyAniSlideOutUp _SlideOutUp;
        public AyAniSlideOutUp SlideOutUp
        {
            get
            {
                if (_SlideOutUp == null)
                {
                    _SlideOutUp = new AyAniSlideOutUp(rect2);
                }
                return _SlideOutUp;
            }
        }

        private void AyAniSlideOutUp_Click(object sender, RoutedEventArgs e)
        {
            SlideOutUp.Begin();
        }


        private AyAniSlideOutLeft _SlideOutLeft;
        public AyAniSlideOutLeft SlideOutLeft
        {
            get
            {
                if (_SlideOutLeft == null)
                {
                    _SlideOutLeft = new AyAniSlideOutLeft(rect2);
                }
                return _SlideOutLeft;
            }
        }

        private void AyAniSlideOutLeft_Click(object sender, RoutedEventArgs e)
        {
            SlideOutLeft.Begin();
        }

        private AyAniSlideOutRight _SlideOutRight;
        public AyAniSlideOutRight SlideOutRight
        {
            get
            {
                if (_SlideOutRight == null)
                {
                    _SlideOutRight = new AyAniSlideOutRight(rect2);
                }
                return _SlideOutRight;
            }
        }

        private void AyAniSlideOutRight_Click(object sender, RoutedEventArgs e)
        {
            SlideOutRight.Begin();
        }



        private AyAniZoomIn _ZoomIn;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomIn ZoomIn
        {
            get
            {
                if (_ZoomIn == null)
                {
                    _ZoomIn = new AyAniZoomIn(rect2);
                }
                return _ZoomIn;
            }
        }

        private void AniZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ZoomIn.Begin();
        }

        private AyAniZoomInDown _ZoomInDown;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomInDown ZoomInDown
        {
            get
            {
                if (_ZoomInDown == null)
                {
                    _ZoomInDown = new AyAniZoomInDown(rect2);
                }
                return _ZoomInDown;
            }
        }

        private void AniZoomInDown_Click(object sender, RoutedEventArgs e)
        {
            ZoomInDown.Begin();
        }

        private AyAniZoomInUp _ZoomInUp;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomInUp ZoomInUp
        {
            get
            {
                if (_ZoomInUp == null)
                {
                    _ZoomInUp = new AyAniZoomInUp(rect2);
                }
                return _ZoomInUp;
            }
        }

        private void AniZoomInUp_Click(object sender, RoutedEventArgs e)
        {
            ZoomInUp.Begin();
        }
        private AyAniZoomInLeft _ZoomInLeft;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomInLeft ZoomInLeft
        {
            get
            {
                if (_ZoomInLeft == null)
                {
                    _ZoomInLeft = new AyAniZoomInLeft(rect2);
                }
                return _ZoomInLeft;
            }
        }

        private void AniZoomInLeft_Click(object sender, RoutedEventArgs e)
        {
            ZoomInLeft.Begin();
        }
        private AyAniZoomInRight _ZoomInRight;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomInRight ZoomInRight
        {
            get
            {
                if (_ZoomInRight == null)
                {
                    _ZoomInRight = new AyAniZoomInRight(rect2);
                }
                return _ZoomInRight;
            }
        }

        private void AniZoomInRight_Click(object sender, RoutedEventArgs e)
        {
            ZoomInRight.Begin();
        }

        private AyAniZoomBounceIn _ZoomBounceIn;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomBounceIn ZoomBounceIn
        {
            get
            {
                if (_ZoomBounceIn == null)
                {
                    _ZoomBounceIn = new AyAniZoomBounceIn(rect2);
                }
                return _ZoomBounceIn;
            }
        }

        private void AniZoomBounceIn_Click(object sender, RoutedEventArgs e)
        {
            ZoomBounceIn.Begin();
        }


        private AyAniBounceOut _BounceOut;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceOut BounceOut
        {
            get
            {
                if (_BounceOut == null)
                {
                    _BounceOut = new AyAniBounceOut(rect2);
                }
                return _BounceOut;
            }
        }

        private void AniBounceOut_Click(object sender, RoutedEventArgs e)
        {
            BounceOut.Begin();
        }
        private AyAniBounceOutDown _BounceOutDown;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceOutDown BounceOutDown
        {
            get
            {
                if (_BounceOutDown == null)
                {
                    _BounceOutDown = new AyAniBounceOutDown(rect2);
                }
                return _BounceOutDown;
            }
        }

        private void AniBounceOutDown_Click(object sender, RoutedEventArgs e)
        {
            BounceOutDown.Begin();
        }

        private AyAniBounceOutUp _BounceOutUp;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceOutUp BounceOutUp
        {
            get
            {
                if (_BounceOutUp == null)
                {
                    _BounceOutUp = new AyAniBounceOutUp(rect2);
                }
                return _BounceOutUp;
            }
        }

        private void AniBounceOutUp_Click(object sender, RoutedEventArgs e)
        {
            BounceOutUp.Begin();
        }


        private AyAniBounceOutLeft _BounceOutLeft;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceOutLeft BounceOutLeft
        {
            get
            {
                if (_BounceOutLeft == null)
                {
                    _BounceOutLeft = new AyAniBounceOutLeft(rect2);
                }
                return _BounceOutLeft;
            }
        }

        private void AniBounceOutLeft_Click(object sender, RoutedEventArgs e)
        {
            BounceOutLeft.Begin();
        }


        private AyAniBounceOutRight _BounceOutRight;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniBounceOutRight BounceOutRight
        {
            get
            {
                if (_BounceOutRight == null)
                {
                    _BounceOutRight = new AyAniBounceOutRight(rect2);
                }
                return _BounceOutRight;
            }
        }

        private void AniBounceOutRight_Click(object sender, RoutedEventArgs e)
        {
            BounceOutRight.Begin();
        }

        private AyAniZoomBounceOut _ZoomBounceOut;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomBounceOut ZoomBounceOut
        {
            get
            {
                if (_ZoomBounceOut == null)
                {
                    _ZoomBounceOut = new AyAniZoomBounceOut(rect2);
                }
                return _ZoomBounceOut;
            }
        }

        private void AyAniZoomBounceOut_Click(object sender, RoutedEventArgs e)
        {
            ZoomBounceOut.Begin();
        }





























        private AyAniZoomOut _ZoomOut;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomOut ZoomOut
        {
            get
            {
                if (_ZoomOut == null)
                {
                    _ZoomOut = new AyAniZoomOut(rect2);
                }
                return _ZoomOut;
            }
        }

        private void AniZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ZoomOut.Begin();
        }

        private AyAniZoomOutDown _ZoomOutDown;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomOutDown ZoomOutDown
        {
            get
            {
                if (_ZoomOutDown == null)
                {
                    _ZoomOutDown = new AyAniZoomOutDown(rect2);
                }
                return _ZoomOutDown;
            }
        }

        private void AniZoomOutDown_Click(object sender, RoutedEventArgs e)
        {
            ZoomOutDown.Begin();
        }

        private AyAniZoomOutUp _ZoomOutUp;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomOutUp ZoomOutUp
        {
            get
            {
                if (_ZoomOutUp == null)
                {
                    _ZoomOutUp = new AyAniZoomOutUp(rect2);
                }
                return _ZoomOutUp;
            }
        }

        private void AniZoomOutUp_Click(object sender, RoutedEventArgs e)
        {
            ZoomOutUp.Begin();
        }
        private AyAniZoomOutLeft _ZoomOutLeft;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomOutLeft ZoomOutLeft
        {
            get
            {
                if (_ZoomOutLeft == null)
                {
                    _ZoomOutLeft = new AyAniZoomOutLeft(rect2);
                }
                return _ZoomOutLeft;
            }
        }

        private void AniZoomOutLeft_Click(object sender, RoutedEventArgs e)
        {
            ZoomOutLeft.Begin();
        }
        private AyAniZoomOutRight _ZoomOutRight;
        /// <summary>
        /// 跳跃
        /// </summary>
        public AyAniZoomOutRight ZoomOutRight
        {
            get
            {
                if (_ZoomOutRight == null)
                {
                    _ZoomOutRight = new AyAniZoomOutRight(rect2);
                }
                return _ZoomOutRight;
            }
        }

        private void AniZoomOutRight_Click(object sender, RoutedEventArgs e)
        {
            ZoomOutRight.Begin();
        }




        private AyAniRotateIn _RotateIn;
        public AyAniRotateIn RotateIn
        {
            get
            {
                if (_RotateIn == null)
                {
                    _RotateIn = new AyAniRotateIn(rect2);
                }
                return _RotateIn;
            }
        }

        private void AyAniRotateIn_Click(object sender, RoutedEventArgs e)
        {
            RotateIn.Begin();
        }



        private AyAniRotateOut _RotateOut;
        public AyAniRotateOut RotateOut
        {
            get
            {
                if (_RotateOut == null)
                {
                    _RotateOut = new AyAniRotateOut(rect2);
                }
                return _RotateOut;
            }
        }

        private void AyAniRotateOut_Click(object sender, RoutedEventArgs e)
        {
            RotateOut.Begin();
        }




        private AyAniFlash _AniFlash;
        public AyAniFlash AniFlash
        {
            get
            {
                if (_AniFlash == null)
                {
                    _AniFlash = new AyAniFlash(rect2);
                }
                return _AniFlash;
            }
        }

        private void AniFlash_Click(object sender, RoutedEventArgs e)
        {
            AniFlash.Begin();
        }


        private AyAniHinge _AniHinge;
        public AyAniHinge AniHinge
        {
            get
            {
                if (_AniHinge == null)
                {
                    _AniHinge = new AyAniHinge(rect2);
                }
                return _AniHinge;
            }
        }

        private void AniHinge_Click(object sender, RoutedEventArgs e)
        {
            AniHinge.Begin();
        }


        private AyAniJello _AyAniJello;
        public AyAniJello AyAniJello
        {
            get
            {
                if (_AyAniJello == null)
                {
                    _AyAniJello = new AyAniJello(rect2);
                }
                return _AyAniJello;
            }
        }

        private void AyAniJello_Click(object sender, RoutedEventArgs e)
        {
            AyAniJello.Begin();
        }


        private AyAniPulse _AyAniPulse;
        public AyAniPulse AyAniPulse
        {
            get
            {
                if (_AyAniPulse == null)
                {
                    _AyAniPulse = new AyAniPulse(rect2);
                }
                return _AyAniPulse;
            }
        }

        private void AyAniPulse_Click(object sender, RoutedEventArgs e)
        {
            AyAniPulse.Begin();
        }
        private AyAniRubberBand _AyAniRubberBand;
        public AyAniRubberBand AyAniRubberBand
        {
            get
            {
                if (_AyAniRubberBand == null)
                {
                    _AyAniRubberBand = new AyAniRubberBand(rect2);
                }
                return _AyAniRubberBand;
            }
        }

        private void AyAniRubberBand_Click(object sender, RoutedEventArgs e)
        {
            AyAniRubberBand.Begin();
        }

        private AyAniShake _AyAniShake;
        public AyAniShake AyAniShake
        {
            get
            {
                if (_AyAniShake == null)
                {
                    _AyAniShake = new AyAniShake(rect2);
                }
                return _AyAniShake;
            }
        }

        private void AyAniShake_Click(object sender, RoutedEventArgs e)
        {
            AyAniShake.Begin();
        }
        private AyAniSwing _AyAniSwing;
        public AyAniSwing AyAniSwing
        {
            get
            {
                if (_AyAniSwing == null)
                {
                    _AyAniSwing = new AyAniSwing(rect2);
                }
                return _AyAniSwing;
            }
        }

        private void AyAniSwing_Click(object sender, RoutedEventArgs e)
        {
            AyAniSwing.Begin();
        }


        private AyAniTada _AyAniTada;
        public AyAniTada AyAniTada
        {
            get
            {
                if (_AyAniTada == null)
                {
                    _AyAniTada = new AyAniTada(rect2);
                }
                return _AyAniTada;
            }
        }

        private void AyAniTada_Click(object sender, RoutedEventArgs e)
        {
            AyAniTada.Begin();
        }

        private AyAniWobble _AyAniWobble;
        public AyAniWobble AyAniWobble
        {
            get
            {
                if (_AyAniWobble == null)
                {
                    _AyAniWobble = new AyAniWobble(rect2);
                }
                return _AyAniWobble;
            }
        }

        private void AyAniWobble_Click(object sender, RoutedEventArgs e)
        {
            AyAniWobble.Begin();
        }



        private void AyAniCanvas_Click(object sender, RoutedEventArgs e)
        {
            (new Window1()).Show();
        }

        IEasingFunction d = new CubicEase { EasingMode = EasingMode.EaseOut };
        private AyAniRotate _AyRotate1;
        public AyAniRotate AyRotate1
        {
            get
            {
                if (_AyRotate1 == null)
                {
                    _AyRotate1 = new AyAniRotate(rect000);
                    _AyRotate1.AniRepeatBehavior = RepeatBehavior.Forever;
                    _AyRotate1.AnimateSpeed = 1200;
                    _AyRotate1.RotateAngleAdd = 220;
                    _AyRotate1.EasingFunction = d;
                }
                return _AyRotate1;
            }
        }
        private AyAniRotate _AyRotate2;
        public AyAniRotate AyRotate2
        {
            get
            {
                if (_AyRotate2 == null)
                {
                    _AyRotate2 = new AyAniRotate(rect001);
                    _AyRotate2.AnimateSpeed = 1200;
                    _AyRotate2.RotateAngleAdd = 500;
                    _AyRotate2.EasingFunction = d;
                }
                return _AyRotate2;
            }
        }
        private void RotateTest_Click(object sender, RoutedEventArgs e)
        {
            AyRotate1.Begin();
            AyRotate2.Begin();
        }
        private void StopRotateTest_Click(object sender, RoutedEventArgs e)
        {
            AyRotate1.Stop();
            AyRotate2.Stop();
        }
        private void PauseRotateTest_Click(object sender, RoutedEventArgs e)
        {
            AyRotate1.Pauze();
            AyRotate2.Pauze();
        }
        private void DestoryRotateTest_Click(object sender, RoutedEventArgs e)
        {
            AyRotate1.Destroy();
            AyRotate2.Destroy();
        }
        private void ResumeRotateTest_Click(object sender, RoutedEventArgs e)
        {
            AyRotate1.Resume();
            AyRotate2.Resume();
        }
        private void ResetRotateTest_Click(object sender, RoutedEventArgs e)
        {
            AyRotate1.ReInitialize();
            AyRotate2.ReInitialize();
            MessageBox.Show("重置成功！点击旋转按钮吧");
        }

        private AyAniScale _AyScale1;
        public AyAniScale AyScale1
        {
            get
            {
                if (_AyScale1 == null)
                {
                    _AyScale1 = new AyAniScale(rect000);
                    _AyScale1.AnimateSpeed = 1000;
                    _AyScale1.ScaleXTo = 0.2;
                    _AyScale1.ScaleYTo = 0.2;
                    _AyScale1.EasingFunction = d;
                }
                return _AyScale1;
            }
        }
        private void ScaleTest1_Click(object sender, RoutedEventArgs e)
        {
            AyScale1.Begin();
        }
        private AyAniScale _AyScale2;
        public AyAniScale AyScale2
        {
            get
            {
                if (_AyScale2 == null)
                {
                    _AyScale2 = new AyAniScale(rect001);
                    _AyScale2.AnimateSpeed = 1000;
                    _AyScale2.ScaleXAdd = 0.5;
                    _AyScale2.ScaleYAdd = 0.5;
                    _AyScale2.EasingFunction = d;
                }
                return _AyScale2;
            }
        }
        private void ScaleTest2_Click(object sender, RoutedEventArgs e)
        {
            AyScale2.Begin();
        }

        private AyAniScale _AyScale3;
        public AyAniScale AyScale3
        {
            get
            {
                if (_AyScale3 == null)
                {
                    _AyScale3 = new AyAniScale(rect002);
                    _AyScale3.AnimateSpeed = 1000;
                    _AyScale3.ScaleXFrom = 3.0;
                    _AyScale3.ScaleYFrom = 3.0;
                    _AyScale3.ScaleXTo = 0.6;
                    _AyScale3.ScaleYTo = 0.6;
                    _AyScale3.EasingFunction = d;
                }
                return _AyScale3;
            }
        }
        private void ScaleTest3_Click(object sender, RoutedEventArgs e)
        {

            AyScale3.Begin();
        }

        private void StopScaleTest_Click(object sender, RoutedEventArgs e)
        {
            AyScale1.Stop();
            AyScale2.Stop();
            AyScale3.Stop();
        }
        private void PauseScaleTest_Click(object sender, RoutedEventArgs e)
        {
            AyScale1.Pauze();
            AyScale2.Pauze();
            AyScale3.Pauze();
        }
        private void DestoryScaleTest_Click(object sender, RoutedEventArgs e)
        {
            AyScale1.Destroy();
            AyScale2.Destroy();
            AyScale3.Destroy();
        }
        private void ResumeScaleTest_Click(object sender, RoutedEventArgs e)
        {
            AyScale1.Resume();
            AyScale2.Resume();
            AyScale3.Resume();
        }
        private void ResetScaleTest_Click(object sender, RoutedEventArgs e)
        {
            AyScale1.ReInitialize();
            AyScale2.ReInitialize();
            AyScale3.ReInitialize();
            MessageBox.Show("重置成功！");
        }

        private AyAniTranslate _AyMove;
        public AyAniTranslate AyMove
        {
            get
            {
                if (_AyMove == null)
                {
                    _AyMove = new AyAniTranslate(rect000);
                    _AyMove.AnimateSpeed = 1200;
                    _AyMove.TranslateXFrom = -400.8;
                    _AyMove.TranslateYFrom = -700.5;
                    _AyMove.TranslateXTo = 0;
                    _AyMove.TranslateYTo = 100;
                    _AyMove.EasingFunction = d;
                }
                return _AyMove;
            }
        }
        private void MoveTest1_Click(object sender, RoutedEventArgs e)
        {
            AyMove.Begin();
        }

        private AyAniTranslate _AyMove1;
        public AyAniTranslate AyMove1
        {
            get
            {
                if (_AyMove1 == null)
                {
                    _AyMove1 = new AyAniTranslate(rect001);
                    _AyMove1.AnimateSpeed = 300;
                    _AyMove1.TranslateXAdd = 100;
                    _AyMove1.TranslateYAdd = 100;
                    _AyMove1.EasingFunction = d;
                }
                return _AyMove1;
            }
        }
        private void MoveTest2_Click(object sender, RoutedEventArgs e)
        {
            AyMove1.Begin();
        }
        private void StopMoveTest_Click(object sender, RoutedEventArgs e)
        {
            AyMove.Stop();
            AyMove1.Stop();
        }
        private void TestMatrix_Click(object sender, RoutedEventArgs e)
        {
            Matrix matrix = Matrix.Identity;
            matrix.Translate(10, 20);
            matrix.Rotate(50);
            MatrixAnimation ma = new MatrixAnimation(Matrix.Identity, matrix, TimeSpan.FromMilliseconds(1000));
            mt.BeginAnimation(MatrixTransform.MatrixProperty, ma);
        }
        private void TestMatrix1_Click(object sender, RoutedEventArgs e)
        {
            MatrixAnimation ma = new MatrixAnimation(Matrix.Identity, TimeSpan.FromMilliseconds(1000));
            mt.BeginAnimation(MatrixTransform.MatrixProperty, ma);
        }

        #endregion
        #region 动画树测试



 


        private void TestAllBegin_Click(object sender, RoutedEventArgs e)
        {
            tballplaytime1.Text = AniTree1.AllPlayTime.ToString()+"毫秒";
            AniTree1.Begin();
        }

        private void StopAllBeginTest_Click(object sender, RoutedEventArgs e)
        {
            AniTree1.Stop();
        }
        private void PauseAllBeginTest_Click(object sender, RoutedEventArgs e)
        {
            AniTree1.Pauze();
        }
        private void DestoryAllBeginTest_Click(object sender, RoutedEventArgs e)
        {
            AniTree1.Destroy();
        }
        private void ResumeAllBeginTest_Click(object sender, RoutedEventArgs e)
        {
            AniTree1.Resume();
        }
        private void ResetAllBeginTest_Click(object sender, RoutedEventArgs e)
        {
            AniTree1.Initialize();
        }
        private AyAnimateTreePad _AniTree1;
        /// <summary>
        /// 动画树1
        /// </summary>
        public AyAnimateTreePad AniTree1
        {
            get
            {
                if (_AniTree1 == null)
                {
                    _AniTree1 = new AyAnimateTreePad();
                    _AniTree1.AddSameBegin(AyRotate1, AyRotate2, AyScale1, AyScale2, AyScale3, AyMove1);
                }
                return _AniTree1;
            }
        }
        private AyAnimateTreePad _AniTree2;
        /// <summary>
        /// 动画树2
        /// </summary>
        public AyAnimateTreePad AniTree2
        {
            get
            {
                if (_AniTree2 == null)
                {
                    _AniTree2 = new AyAnimateTreePad();
                    _AniTree2.Add(AyScale3).DelayAddSameBegin(2000, AyRotate2, AyMove1, AyScale2).Add(AyScale1).DelayAdd(3000, AyRotate1);
                }
                return _AniTree2;
            }
        }


        private void TestAllBegin1_Click(object sender, RoutedEventArgs e)
        {
            tballplaytime2.Text = AniTree2.AllPlayTime.ToString() + "毫秒";
            AniTree2.Begin();
        }

        private void StopAllBeginTest1_Click(object sender, RoutedEventArgs e)
        {
            AniTree2.Stop();
        }
        private void PauseAllBeginTest1_Click(object sender, RoutedEventArgs e)
        {
            AniTree2.Pauze();
        }
        private void DestoryAllBeginTest1_Click(object sender, RoutedEventArgs e)
        {
            AniTree2.Destroy();
        }
        private void ResumeAllBeginTest1_Click(object sender, RoutedEventArgs e)
        {
            AniTree2.Resume();
        }
        private void ResetAllBeginTest1_Click(object sender, RoutedEventArgs e)
        {
            AniTree2.Initialize();
        }

        #endregion

    }

}
