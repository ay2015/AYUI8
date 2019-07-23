/**-----------------------------------------------
 * ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权=========
 * 作者：ay
 * 联系QQ：875556003
 * 时间2016-6-24 15:27:39
 * 最后修改：2017-9-7 10:19:16
 * -----------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ay.Controls
{

    public class AySplitView:ContentControl
    {

        public AySplitView()
        {
            Loaded += AySplitView_Loaded;
        }
        static AySplitView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AySplitView), new FrameworkPropertyMetadata(typeof(AySplitView)));
        }

        public SplitViewDisplayMode DisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
       public Grid LeftArea = null;
        Grid ContentArea = null;
        ColumnDefinition colLeft = null;
        ColumnDefinition colContent = null;
        RowDefinition rowTop = null;
        RowDefinition rowBottom = null;
        ContentControl cc = null;
        public override void OnApplyTemplate()
        {
            if (!WpfTreeHelper.IsInDesignMode)
            {
                LeftArea = Template.FindName("LeftArea", this) as Grid;
                ContentArea = Template.FindName("ContentArea", this) as Grid;
                colLeft = Template.FindName("colLeft", this) as ColumnDefinition;
                colContent = Template.FindName("colContent", this) as ColumnDefinition;
                rowTop = Template.FindName("rowTop", this) as RowDefinition;
                rowBottom = Template.FindName("rowBottom", this) as RowDefinition;
                cc = Template.FindName("cc", this) as ContentControl;
            }

            base.OnApplyTemplate();
        }

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(SplitViewDisplayMode), typeof(AySplitView), new FrameworkPropertyMetadata(SplitViewDisplayMode.Inline, OnDisplayModePropertyChanged));
        private static void OnDisplayModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AySplitView sv = d as AySplitView;
            if (sv != null)
            {
                if (sv.LeftArea.IsNull()) return;
                DisplayModeOrPlacementChanged(sv);
            }
        }



        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }

        public static readonly DependencyProperty IsPaneOpenProperty =
            DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(AySplitView), new FrameworkPropertyMetadata(false, OnIsPaneOpenPropertyChanged));

        private static void OnIsPaneOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AySplitView sv = d as AySplitView;

            if (sv != null)
            {
                if (sv.LeftArea.IsNull()) return;
                bool isOpenValue = (bool)e.NewValue;
                if (sv.PanePlacement == SplitViewPanePlacement.Left)
                {
                    Grid.SetRow(sv.LeftArea, 0);
                    Grid.SetRow(sv.ContentArea, 0);
                    sv.colContent.Width = new GridLength(1, GridUnitType.Star);
                    sv.colLeft.Width = GridLength.Auto;
                    sv.LeftArea.HorizontalAlignment = HorizontalAlignment.Left;
                    //inline效果
                    if (sv.DisplayMode == SplitViewDisplayMode.Inline)
                    {
                        Grid.SetColumn(sv.LeftArea, 0);
                        Grid.SetColumn(sv.ContentArea, 1);
                        Grid.SetColumnSpan(sv.LeftArea, 1);
                        Grid.SetColumnSpan(sv.ContentArea, 1);

                        InlineOrOverlay(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {
                        Grid.SetColumn(sv.LeftArea, 0);
                        Grid.SetColumn(sv.ContentArea, 0);
                        Grid.SetColumnSpan(sv.LeftArea, 2);
                        Grid.SetColumnSpan(sv.ContentArea, 2);
                        InlineOrOverlay(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.CompactInline)//应该设置OpenPaneLength
                    {
                        Grid.SetColumn(sv.LeftArea, 0);
                        Grid.SetColumn(sv.ContentArea, 1);
                        Grid.SetColumnSpan(sv.LeftArea, 1);
                        Grid.SetColumnSpan(sv.ContentArea, 1);

                        CompactInlineOrCompactOverlay(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                    {
                        Grid.SetColumn(sv.LeftArea, 0);
                        Grid.SetColumn(sv.ContentArea, 0);
                        Grid.SetColumnSpan(sv.LeftArea, 2);
                        Grid.SetColumnSpan(sv.ContentArea, 2);
                        CompactInlineOrCompactOverlay(sv, isOpenValue);
                    }

                }
                else if (sv.PanePlacement == SplitViewPanePlacement.Right)
                {
                    Grid.SetRow(sv.LeftArea, 0);
                    Grid.SetRow(sv.ContentArea, 0);
                    sv.colContent.Width = GridLength.Auto;
                    sv.colLeft.Width = new GridLength(1, GridUnitType.Star);
                    sv.LeftArea.HorizontalAlignment = HorizontalAlignment.Right;
                    //inline效果
                    if (sv.DisplayMode == SplitViewDisplayMode.Inline)
                    {
                        Grid.SetColumn(sv.LeftArea, 1);
                        Grid.SetColumn(sv.ContentArea, 0);
                        Grid.SetColumnSpan(sv.LeftArea, 1);
                        Grid.SetColumnSpan(sv.ContentArea, 1);

                        InlineOrOverlayRight(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {
                        Grid.SetColumn(sv.LeftArea, 0);
                        Grid.SetColumn(sv.ContentArea, 0);
                        Grid.SetColumnSpan(sv.LeftArea, 2);
                        Grid.SetColumnSpan(sv.ContentArea, 2);
                        InlineOrOverlayRight(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.CompactInline)//应该设置OpenPaneLength
                    {
                        Grid.SetColumn(sv.LeftArea, 1);
                        Grid.SetColumn(sv.ContentArea, 0);
                        Grid.SetColumnSpan(sv.LeftArea, 1);
                        Grid.SetColumnSpan(sv.ContentArea, 1);
                        CompactInlineOrCompactOverlay(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                    {
                        Grid.SetColumn(sv.LeftArea, 0);
                        Grid.SetColumn(sv.ContentArea, 0);
                        Grid.SetColumnSpan(sv.LeftArea, 2);
                        Grid.SetColumnSpan(sv.ContentArea, 2);
                        CompactInlineOrCompactOverlay(sv, isOpenValue);
                    }
                }
                else if (sv.PanePlacement == SplitViewPanePlacement.Top) //www.ayjs.net 六安杨洋（AY）拓展
                {
                    Grid.SetColumn(sv.LeftArea, 0);
                    Grid.SetColumn(sv.ContentArea, 0);
                    sv.rowTop.Height = GridLength.Auto;
                    sv.rowBottom.Height = new GridLength(1, GridUnitType.Star);
                    sv.LeftArea.VerticalAlignment = VerticalAlignment.Top;
                    //inline效果
                    if (sv.DisplayMode == SplitViewDisplayMode.Inline)
                    {
                        Grid.SetRow(sv.LeftArea, 0);
                        Grid.SetRow(sv.ContentArea, 1);
                        Grid.SetRowSpan(sv.LeftArea, 1);
                        Grid.SetRowSpan(sv.ContentArea, 1);


                        InlineOrOverlayTop(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {
                        Grid.SetRow(sv.LeftArea, 0);
                        Grid.SetRow(sv.ContentArea, 0);
                        Grid.SetRowSpan(sv.LeftArea, 2);
                        Grid.SetRowSpan(sv.ContentArea, 2);

                        InlineOrOverlayTop(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.CompactInline)//应该设置OpenPaneLength
                    {
                        Grid.SetRow(sv.LeftArea, 0);
                        Grid.SetRow(sv.ContentArea, 1);
                        Grid.SetRowSpan(sv.LeftArea, 1);
                        Grid.SetRowSpan(sv.ContentArea, 1);
                        CompactInlineOrCompactOverlayTop(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                    {
                        Grid.SetRow(sv.LeftArea, 0);
                        Grid.SetRow(sv.ContentArea, 0);
                        Grid.SetRowSpan(sv.LeftArea, 2);
                        Grid.SetRowSpan(sv.ContentArea, 2);
                        CompactInlineOrCompactOverlayTop(sv, isOpenValue);
                    }
                }
                else if (sv.PanePlacement == SplitViewPanePlacement.Bottom) //www.ayjs.net 六安杨洋（AY）拓展
                {
                    Grid.SetColumn(sv.LeftArea, 0);
                    Grid.SetColumn(sv.ContentArea, 0);
                    sv.rowBottom.Height = GridLength.Auto;
                    sv.rowTop.Height = new GridLength(1, GridUnitType.Star);
                    sv.LeftArea.VerticalAlignment = VerticalAlignment.Bottom;
                    //inline效果
                    if (sv.DisplayMode == SplitViewDisplayMode.Inline)
                    {
                        Grid.SetRow(sv.LeftArea, 1);
                        Grid.SetRow(sv.ContentArea, 0);
                        Grid.SetRowSpan(sv.LeftArea, 1);
                        Grid.SetRowSpan(sv.ContentArea, 1);

                        InlineOrOverlayBottom(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {
                        Grid.SetRow(sv.LeftArea, 0);
                        Grid.SetRow(sv.ContentArea, 0);
                        Grid.SetRowSpan(sv.LeftArea, 2);
                        Grid.SetRowSpan(sv.ContentArea, 2);

                        InlineOrOverlayBottom(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.CompactInline)//应该设置OpenPaneLength
                    {
                        Grid.SetRow(sv.LeftArea, 1);
                        Grid.SetRow(sv.ContentArea, 0);
                        Grid.SetRowSpan(sv.LeftArea, 1);
                        Grid.SetRowSpan(sv.ContentArea, 1);
                        CompactInlineOrCompactOverlayTop(sv, isOpenValue);
                    }
                    else if (sv.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                    {
                        Grid.SetRow(sv.LeftArea, 0);
                        Grid.SetRow(sv.ContentArea, 0);
                        Grid.SetRowSpan(sv.LeftArea, 2);
                        Grid.SetRowSpan(sv.ContentArea, 2);
                        CompactInlineOrCompactOverlayTop(sv, isOpenValue);
                    }
                }
                #region 触发客户端定义的事件
                bool oldBool = (bool)e.OldValue;
                sv.IsPaneOpenValueChanged(oldBool, isOpenValue);
                #endregion

            }
        }


        #region IsPaneOpenEvent ay 2016-6-24 14:54:30用于展开关闭事件暴露给客户端。
        private static readonly RoutedEvent IsPaneOpenChangedEvent =
            EventManager.RegisterRoutedEvent("IsPaneOpenChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<bool>), typeof(AySplitView));


        public event RoutedPropertyChangedEventHandler<bool> IsPaneOpenChanged
        {
            add { AddHandler(IsPaneOpenChangedEvent, value); }
            remove { RemoveHandler(IsPaneOpenChangedEvent, value); }
        }

        private void IsPaneOpenValueChanged(bool oldIsPaneOpen, bool newIsPaneOpen)
        {
            RoutedPropertyChangedEventArgs<bool> args = new RoutedPropertyChangedEventArgs<bool>(oldIsPaneOpen, newIsPaneOpen);
            args.RoutedEvent = AySplitView.IsPaneOpenChangedEvent;
            RaiseEvent(args);
        }

        private static readonly RoutedEvent PanePlacementChangedEvent =
          EventManager.RegisterRoutedEvent("PanePlacementChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<SplitViewPanePlacement>), typeof(AySplitView));


        public event RoutedPropertyChangedEventHandler<SplitViewPanePlacement> PanePlacementChanged
        {
            add { AddHandler(PanePlacementChangedEvent, value); }
            remove { RemoveHandler(PanePlacementChangedEvent, value); }
        }

        private void PanePlacementValueChanged(SplitViewPanePlacement oldSplitViewPanePlacement, SplitViewPanePlacement newSplitViewPanePlacement)
        {
            RoutedPropertyChangedEventArgs<SplitViewPanePlacement> args = new RoutedPropertyChangedEventArgs<SplitViewPanePlacement>(oldSplitViewPanePlacement, newSplitViewPanePlacement);
            args.RoutedEvent = AySplitView.PanePlacementChangedEvent;
            RaiseEvent(args);
        }

        //添加Open时候动画完成后事件

        private static readonly RoutedEvent OpenPaneCompletedEvent =
          EventManager.RegisterRoutedEvent("OpenPaneCompleted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AySplitView));


        public event RoutedEventHandler OpenPaneCompleted
        {
            add { AddHandler(OpenPaneCompletedEvent, value); }
            remove { RemoveHandler(OpenPaneCompletedEvent, value); }
        }

        private void OpenPaneCompletedChanged()
        {
            RoutedEventArgs newEvent = new RoutedEventArgs(AySplitView.OpenPaneCompletedEvent, this);
            this.RaiseEvent(newEvent);

        }
        //添加关闭Pane时候，动画完成后事件
        private static readonly RoutedEvent ClosePaneCompletedEvent =
   EventManager.RegisterRoutedEvent("ClosePaneCompleted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AySplitView));


        public event RoutedEventHandler ClosePaneCompleted
        {
            add { AddHandler(ClosePaneCompletedEvent, value); }
            remove { RemoveHandler(ClosePaneCompletedEvent, value); }
        }

        private void ClosePaneCompletedChanged()
        {
            RoutedEventArgs newEvent = new RoutedEventArgs(AySplitView.ClosePaneCompletedEvent, this);
            this.RaiseEvent(newEvent);

        }
        #endregion


        private static void InlineOrOverlayBottom(AySplitView sv, bool isOpenValue)
        {
            if (sv.LeftArea.IsNull()) return;
            if (isOpenValue) //从隐藏到显示
            {
                Thickness thickOld = new Thickness(0, 0, 0, sv.LeftArea.Height * (-1));

                if (double.IsNaN(sv.OpenPaneLength))
                {
                    thickOld = new Thickness(0, 0, 0, sv.cc.RenderSize.Height * (-1));
                }
                sv.LeftArea.Margin = thickOld;
                sv.LeftArea.Visibility = Visibility.Visible;
                Thickness thicnew = new Thickness(0, 0, 0, 0);
                ThicknessAnimationUsingKeyFrames taShow = new ThicknessAnimationUsingKeyFrames();
                taShow.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                taShow.FillBehavior = FillBehavior.Stop;
                taShow.Completed += (sender, ev) =>
                {
                    sv.LeftArea.Margin = thicnew;
                    taShow = null;
                    #region 触发客户端定义的事件
                    sv.OpenPaneCompletedChanged();
                    #endregion
                };

                EasingThicknessKeyFrame taFrame = new EasingThicknessKeyFrame(thicnew, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                taShow.KeyFrames.Add(taFrame);
                sv.LeftArea.BeginAnimation(Grid.MarginProperty, taShow);

            }
            else //显示到隐藏
            {

                Thickness thickOld = new Thickness(0, 0, 0, 0);
                sv.LeftArea.Margin = thickOld;
                Thickness thicnew = new Thickness(0, 0, 0, sv.cc.RenderSize.Height * (-1));
                if (!double.IsNaN(sv.OpenPaneLength))
                {
                    thicnew = new Thickness(0, 0, 0, sv.OpenPaneLength * (-1));
                }

                ThicknessAnimationUsingKeyFrames taShow = new ThicknessAnimationUsingKeyFrames();
                taShow.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                taShow.FillBehavior = FillBehavior.Stop;
                taShow.Completed += (sender, ev) =>
                {
                    sv.LeftArea.Margin = thicnew;
                    sv.LeftArea.Visibility = Visibility.Collapsed;
                    taShow = null;
                    #region 触发客户端定义的事件 ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang=======盗窃代码请注意=========
                    sv.ClosePaneCompletedChanged();
                    #endregion
                };

                EasingThicknessKeyFrame taFrame = new EasingThicknessKeyFrame(thicnew, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                taShow.KeyFrames.Add(taFrame);
                sv.LeftArea.BeginAnimation(Grid.MarginProperty, taShow);

            }
        }

        private static void CompactInlineOrCompactOverlayTop(AySplitView sv, bool isOpenValue)
        {
            if (sv.LeftArea.IsNull()) return;
            if (isOpenValue) //从隐藏到显示
            {
                sv.LeftArea.Margin = new Thickness(0, 0, 0, 0);
                sv.LeftArea.Visibility = Visibility.Visible;
                DoubleAnimationUsingKeyFrames AyTopWidthKey = new DoubleAnimationUsingKeyFrames();
                AyTopWidthKey.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                AyTopWidthKey.FillBehavior = FillBehavior.Stop;
                double endWidth = sv.OpenPaneLength;
                EasingDoubleKeyFrame LeftWidthKey0 = new EasingDoubleKeyFrame(endWidth, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                AyTopWidthKey.KeyFrames.Add(LeftWidthKey0);
                AyTopWidthKey.Completed += (a, ed) =>
                {
                    sv.LeftArea.Height = endWidth;
                    AyTopWidthKey = null;
                    #region 触发客户端定义的事件
                    sv.OpenPaneCompletedChanged();
                    #endregion
                };
                sv.LeftArea.BeginAnimation(ContentControl.HeightProperty, AyTopWidthKey);

            }
            else //显示到隐藏
            {

                sv.LeftArea.Margin = new Thickness(0, 0, 0, 0);
                sv.LeftArea.Visibility = Visibility.Visible;
                DoubleAnimationUsingKeyFrames TopWidthKey = new DoubleAnimationUsingKeyFrames();
                TopWidthKey.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                TopWidthKey.FillBehavior = FillBehavior.Stop;
                double endWidth = sv.CompactPaneLength;
                EasingDoubleKeyFrame LeftWidthKey0 = new EasingDoubleKeyFrame(endWidth, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                TopWidthKey.KeyFrames.Add(LeftWidthKey0);
                TopWidthKey.Completed += (a, ed) =>
                {
                    sv.LeftArea.Height = endWidth;
                    TopWidthKey = null;
                    #region 触发客户端定义的事件
                    sv.ClosePaneCompletedChanged();
                    #endregion
                };
                sv.LeftArea.BeginAnimation(ContentControl.HeightProperty, TopWidthKey);

            }
        }

        private static void InlineOrOverlayTop(AySplitView sv, bool isOpenValue)
        {
            if (sv.LeftArea.IsNull()) return;
            if (isOpenValue) //从隐藏到显示
            {

                Thickness thickOld = new Thickness(0, sv.LeftArea.Height * (-1), 0, 0);

                if (double.IsNaN(sv.OpenPaneLength))
                {
                    thickOld = new Thickness(0, 0, sv.cc.RenderSize.Height * (-1), 0);
                }
                sv.LeftArea.Margin = thickOld;
                sv.LeftArea.Visibility = Visibility.Visible;
                Thickness thicnew = new Thickness(0, 0, 0, 0);
                ThicknessAnimationUsingKeyFrames taShow = new ThicknessAnimationUsingKeyFrames();
                taShow.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                taShow.FillBehavior = FillBehavior.Stop;
                taShow.Completed += (sender, ev) =>
                {
                    sv.LeftArea.Margin = thicnew;
                    taShow = null;
                    #region 触发客户端定义的事件
                    sv.OpenPaneCompletedChanged();
                    #endregion
                };

                EasingThicknessKeyFrame taFrame = new EasingThicknessKeyFrame(thicnew, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                taShow.KeyFrames.Add(taFrame);
                sv.LeftArea.BeginAnimation(Grid.MarginProperty, taShow);

            }
            else //显示到隐藏
            {

                Thickness thickOld = new Thickness(0, 0, 0, 0);
                sv.LeftArea.Margin = thickOld;
                Thickness thicnew = new Thickness(0, sv.cc.RenderSize.Height * (-1), 0, 0);
                if (!double.IsNaN(sv.OpenPaneLength))
                {
                    thicnew = new Thickness(0, sv.OpenPaneLength * (-1), 0, 0);
                }

                ThicknessAnimationUsingKeyFrames taShow = new ThicknessAnimationUsingKeyFrames();
                taShow.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                taShow.FillBehavior = FillBehavior.Stop;
                taShow.Completed += (sender, ev) =>
                {
                    sv.LeftArea.Margin = thicnew;
                    sv.LeftArea.Visibility = Visibility.Collapsed;
                    taShow = null;
                    #region 触发客户端定义的事件
                    sv.ClosePaneCompletedChanged();
                    #endregion
                };

                EasingThicknessKeyFrame taFrame = new EasingThicknessKeyFrame(thicnew, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                taShow.KeyFrames.Add(taFrame);
                sv.LeftArea.BeginAnimation(Grid.MarginProperty, taShow);

            }
        }

        private static void InlineOrOverlayRight(AySplitView sv, bool isOpenValue)
        {
            if (sv.LeftArea.IsNull()) return;
            if (isOpenValue) //从隐藏到显示
            {
                Thickness thickOld = new Thickness(0);
                if (sv.LeftArea.ActualWidth != 0)
                {
                    thickOld = new Thickness(0, 0, sv.LeftArea.ActualWidth * (-1), 0);

                }
                else
                {
                    thickOld = new Thickness(0, 0, sv.LeftArea.Width * (-1), 0);
                }
             

                if (double.IsNaN(sv.OpenPaneLength))
                {
                    thickOld = new Thickness(0, 0, sv.cc.RenderSize.Width * (-1), 0);
                }
                sv.LeftArea.Margin = thickOld;
                sv.LeftArea.Visibility = Visibility.Visible;
                Thickness thicnew = new Thickness(0, 0, 0, 0);
                ThicknessAnimationUsingKeyFrames taShow = new ThicknessAnimationUsingKeyFrames();
                taShow.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                taShow.FillBehavior = FillBehavior.Stop;
                taShow.Completed += (sender, ev) =>
                {
                    sv.LeftArea.Margin = thicnew;
                    taShow = null;
                    #region 触发客户端定义的事件
                    sv.OpenPaneCompletedChanged();
                    #endregion
                };

                EasingThicknessKeyFrame taFrame = new EasingThicknessKeyFrame(thicnew, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                taShow.KeyFrames.Add(taFrame);
                sv.LeftArea.BeginAnimation(Grid.MarginProperty, taShow);

            }
            else //显示到隐藏
            {

                Thickness thickOld = new Thickness(0, 0, 0, 0);
                sv.LeftArea.Margin = thickOld;
                Thickness thicnew = new Thickness(0, 0, sv.cc.RenderSize.Width * (-1), 0);
                if (!double.IsNaN(sv.OpenPaneLength))
                {
                    thicnew = new Thickness(0, 0, sv.OpenPaneLength * (-1), 0);
                }

                ThicknessAnimationUsingKeyFrames taShow = new ThicknessAnimationUsingKeyFrames();
                taShow.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                taShow.FillBehavior = FillBehavior.Stop;
                taShow.Completed += (sender, ev) =>
                {
                    sv.LeftArea.Margin = thicnew;
                    sv.LeftArea.Visibility = Visibility.Collapsed;
                    taShow = null;
                    #region 触发客户端定义的事件
                    sv.ClosePaneCompletedChanged();
                    #endregion
                };

                EasingThicknessKeyFrame taFrame = new EasingThicknessKeyFrame(thicnew, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                taShow.KeyFrames.Add(taFrame);
                sv.LeftArea.BeginAnimation(Grid.MarginProperty, taShow);

            }
        }

        private static void CompactInlineOrCompactOverlay(AySplitView sv, bool isOpenValue)
        {
            if (sv.LeftArea.IsNull()) return;
            if (isOpenValue) //从隐藏到显示
            {
                sv.LeftArea.Margin = new Thickness(0, 0, 0, 0);
                sv.LeftArea.Visibility = Visibility.Visible;
                DoubleAnimationUsingKeyFrames LeftWidthKey = new DoubleAnimationUsingKeyFrames();
                LeftWidthKey.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                LeftWidthKey.FillBehavior = FillBehavior.Stop;
                double endWidth = sv.OpenPaneLength;
                EasingDoubleKeyFrame LeftWidthKey0 = new EasingDoubleKeyFrame(endWidth, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                LeftWidthKey.KeyFrames.Add(LeftWidthKey0);
                LeftWidthKey.Completed += (a, ed) =>
                {
                    sv.LeftArea.Width = endWidth;
                    LeftWidthKey = null;
                    #region 触发客户端定义的事件
                    sv.OpenPaneCompletedChanged();
                    #endregion
                };
                sv.LeftArea.BeginAnimation(ContentControl.WidthProperty, LeftWidthKey);

            }
            else //显示到隐藏
            {

                sv.LeftArea.Margin = new Thickness(0, 0, 0, 0);
                sv.LeftArea.Visibility = Visibility.Visible;
                DoubleAnimationUsingKeyFrames LeftWidthKey = new DoubleAnimationUsingKeyFrames();
                LeftWidthKey.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                LeftWidthKey.FillBehavior = FillBehavior.Stop;
                double endWidth = sv.CompactPaneLength;
                EasingDoubleKeyFrame LeftWidthKey0 = new EasingDoubleKeyFrame(endWidth, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                LeftWidthKey.KeyFrames.Add(LeftWidthKey0);
                LeftWidthKey.Completed += (a, ed) =>
                {
                    sv.LeftArea.Width = endWidth;
                    LeftWidthKey = null;
                    #region 触发客户端定义的事件
                    sv.ClosePaneCompletedChanged();
                    #endregion
                };
                sv.LeftArea.BeginAnimation(ContentControl.WidthProperty, LeftWidthKey);

            }
        }


        private static void InlineOrOverlay(AySplitView sv, bool isOpenValue)
        {
            if (isOpenValue) //从隐藏到显示
            {
                if (sv.LeftArea.IsNull()) return;
                {
                    Thickness thickOld = new Thickness(sv.LeftArea.Width * (-1), 0, 0, 0);
                    if (double.IsNaN(sv.OpenPaneLength))
                    {
                        thickOld = new Thickness(sv.cc.RenderSize.Width * (-1), 0, 0, 0);
                    }
                    sv.LeftArea.Margin = thickOld;
                    sv.LeftArea.Visibility = Visibility.Visible;
                    Thickness thicnew = new Thickness(0, 0, 0, 0);
                    ThicknessAnimationUsingKeyFrames taShow = new ThicknessAnimationUsingKeyFrames();
                    taShow.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                    taShow.FillBehavior = FillBehavior.Stop;
                    taShow.Completed += (sender, ev) =>
                    {
                        sv.LeftArea.Margin = thicnew;
                        taShow = null;

                        #region 触发客户端定义的事件
                        sv.OpenPaneCompletedChanged();
                        #endregion
                    };
                    EasingThicknessKeyFrame taFrame = new EasingThicknessKeyFrame(thicnew, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                    taShow.KeyFrames.Add(taFrame);
                    sv.LeftArea.BeginAnimation(Grid.MarginProperty, taShow);
                }
                //if (sv.DisplayMode == SplitViewDisplayMode.Inline)
                //{
                //    {
                //        Thickness thickOld = new Thickness(0 , 0, 0, 0);                   
                //        sv.ContentArea.Margin = thickOld;
                //        Thickness thicnew = new Thickness(sv.LeftArea.Width, 0, 0, 0);
                //        if (double.IsNaN(sv.OpenPaneLength))
                //        {
                //            thicnew = new Thickness(sv.cc.RenderSize.Width, 0, 0, 0);
                //        }
                //        sv.ContentArea.Margin = thicnew;
                //        //ThicknessAnimationUsingKeyFrames taShow = new ThicknessAnimationUsingKeyFrames();
                //        //taShow.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                //        //taShow.FillBehavior = FillBehavior.Stop;
                //        //taShow.Completed += (sender, ev) =>
                //        //{
                //        //    sv.ContentArea.Margin = thicnew;
                //        //    taShow = null;
                //        //};
                //        //EasingThicknessKeyFrame taFrame = new EasingThicknessKeyFrame(thicnew, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                //        //taShow.KeyFrames.Add(taFrame);
                //        //sv.LeftArea.BeginAnimation(Grid.MarginProperty, taShow);
                //    }
                //}

            }
            else //显示到隐藏
            {
                Thickness thickOld = new Thickness(0, 0, 0, 0);
                sv.LeftArea.Margin = thickOld;
                Thickness thicnew = new Thickness(sv.cc.RenderSize.Width * (-1), 0, 0, 0);
                if (!double.IsNaN(sv.OpenPaneLength))
                {
                    thicnew = new Thickness(sv.OpenPaneLength * (-1), 0, 0, 0);
                }

                ThicknessAnimationUsingKeyFrames taShow = new ThicknessAnimationUsingKeyFrames();
                taShow.Duration = new Duration(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime));
                taShow.FillBehavior = FillBehavior.Stop;
                taShow.Completed += (sender, ev) =>
                {
                    sv.LeftArea.Margin = thicnew;
                    sv.LeftArea.Visibility = Visibility.Collapsed;
                    taShow = null;
                    #region 触发客户端定义的事件
                    sv.ClosePaneCompletedChanged();
                    #endregion
                };

                EasingThicknessKeyFrame taFrame = new EasingThicknessKeyFrame(thicnew, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(sv.OpenMillisecondTime)));
                taShow.KeyFrames.Add(taFrame);
                sv.LeftArea.BeginAnimation(Grid.MarginProperty, taShow);

            }
        }

        public double OpenPaneLength
        {
            get { return (double)GetValue(OpenPaneLengthProperty); }
            set { SetValue(OpenPaneLengthProperty, value); }
        }

        // Summary:
        //     Identifies the OpenPaneLength dependency property.
        //
        // Returns:
        //     The identifier for the OpenPaneLength dependency property.
        public static readonly DependencyProperty OpenPaneLengthProperty =
            DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(AySplitView), new FrameworkPropertyMetadata(double.NaN, OnOpenPaneLengthPropertyChanged));

        private static void OnOpenPaneLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AySplitView sv = d as AySplitView;
            if (sv != null)
            {
                if (sv.LeftArea.IsNull()) return;
                OnLengthChanged(sv);
            }
        }


        // Summary:
        //     Gets or sets the Brush to apply to the background of the Pane area of the control.
        //
        // Returns:
        //     The Brush to apply to the background of the Pane area of the control.
        public Brush PaneBackground
        {
            get { return (Brush)GetValue(PaneBackgroundProperty); }
            set { SetValue(PaneBackgroundProperty, value); }
        }

        // Summary:
        //     Identifies the PaneBackground dependency property.
        //
        // Returns:
        //     The identifier for the PaneBackground dependency property.
        public static readonly DependencyProperty PaneBackgroundProperty =
            DependencyProperty.Register("PaneBackground", typeof(Brush), typeof(AySplitView), new FrameworkPropertyMetadata(null));

        // Summary:
        //     Gets or sets a value that specifies whether the pane is shown on the right or
        //     left side of the SplitView.
        //
        // Returns:
        //     A value of the enumeration that specifies whether the pane is shown on the right
        //     or left side of the SplitView. The default is Left.
        public SplitViewPanePlacement PanePlacement
        {
            get { return (SplitViewPanePlacement)GetValue(PanePlacementProperty); }
            set { SetValue(PanePlacementProperty, value); }
        }

        //
        // Summary:
        //     Identifies the PanePlacement dependency property.
        //
        // Returns:
        //     The identifier for the PanePlacement dependency property.
        public static readonly DependencyProperty PanePlacementProperty =
            DependencyProperty.Register("PanePlacement", typeof(SplitViewPanePlacement), typeof(AySplitView), new FrameworkPropertyMetadata(SplitViewPanePlacement.Left, OnPanePlacementPropertyChanged));

        private static void OnPanePlacementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AySplitView sv = d as AySplitView;
            if (sv != null)
            {
                if (sv.LeftArea.IsNull()) return;
                DisplayModeOrPlacementChanged(sv);
                #region 触发客户端定义的事件
                SplitViewPanePlacement newValue = (SplitViewPanePlacement)e.NewValue;
                SplitViewPanePlacement oldValue = (SplitViewPanePlacement)e.OldValue;
                sv.PanePlacementValueChanged(oldValue, newValue);
                #endregion
            }
        }

        public object Pane
        {
            get { return (object)GetValue(PaneProperty); }
            set { SetValue(PaneProperty, value); }
        }

        /// <summary>
        ///  Identifies the <see cref="Pane"/> dependency property.
        /// </summary>
        /// <value>The identifier for the Pane dependency property.</value>
        public static readonly DependencyProperty PaneProperty =
            DependencyProperty.Register("Pane", typeof(object), typeof(AySplitView), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the width of the SplitView pane in its compact display mode.
        /// </summary>
        /// <value>
        /// The width of the pane in it's compact display mode. The default is 48 device-independent
        /// pixel (DIP) (defined by the SplitViewCompactPaneThemeLength resource).
        /// </value>
        public double CompactPaneLength
        {
            get { return (double)GetValue(CompactPaneLengthProperty); }
            set { SetValue(CompactPaneLengthProperty, value); }
        }

        /// <summary>
        /// Identifies the CompactPaneLength dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the CompactPaneLength dependency property.
        /// </value>
        public static readonly DependencyProperty CompactPaneLengthProperty =
            DependencyProperty.Register("CompactPaneLength", typeof(double), typeof(AySplitView), new FrameworkPropertyMetadata(48d, OnCompactPaneLengthPropertyChanged));

        private static void OnCompactPaneLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AySplitView sv = d as AySplitView;
            if (sv != null)
            {
                if (sv.LeftArea.IsNull()) return;
                OnLengthChanged(sv);
            }
        }

        private static void OnLengthChanged(AySplitView sv)
        {
            if (sv.LeftArea.IsNull()) return;
            if (sv.DisplayMode == SplitViewDisplayMode.CompactOverlay || sv.DisplayMode == SplitViewDisplayMode.CompactInline)
            {
         
                    sv.LeftArea.Visibility = Visibility.Visible;
            }
            else
            {
                if (sv.IsPaneOpen)
                {
                    
                        sv.LeftArea.Visibility = Visibility.Visible;
                }
                else
                {

                    sv.LeftArea.Visibility = Visibility.Collapsed;
                }
            }

            if (sv.PanePlacement == SplitViewPanePlacement.Left || sv.PanePlacement == SplitViewPanePlacement.Right)
            {
            
                    sv.LeftArea.Height = double.NaN;
                if (sv.IsPaneOpen)
                {
                    if (sv.DisplayMode == SplitViewDisplayMode.Inline || sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {

                            sv.LeftArea.Width = sv.OpenPaneLength;
                    }
                }
                else
                {
                    if (sv.DisplayMode == SplitViewDisplayMode.CompactInline || sv.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                    {
             
                            sv.LeftArea.Width = sv.CompactPaneLength;
                    }
                    else
                    {
                     
                            sv.LeftArea.Width = sv.OpenPaneLength;
                    }

                }
            }
            else
            {
      
                    sv.LeftArea.Width = double.NaN;
                if (sv.IsPaneOpen)
                {
                    if (sv.DisplayMode == SplitViewDisplayMode.Inline || sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {
                    
                            sv.LeftArea.Height = sv.OpenPaneLength;
                    }
                }
                else
                {
                    if (sv.DisplayMode == SplitViewDisplayMode.CompactInline || sv.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                    {

                            sv.LeftArea.Height = sv.CompactPaneLength;
                    }
                    else
                    {
                       
                            sv.LeftArea.Height = sv.OpenPaneLength;
                    }

                }
            }

        }


        public int OpenMillisecondTime
        {
            get { return (int)GetValue(OpenMillisecondTimeProperty); }
            set { SetValue(OpenMillisecondTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SpreadMillisecondTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenMillisecondTimeProperty =
            DependencyProperty.Register("OpenMillisecondTime", typeof(int), typeof(AySplitView), new FrameworkPropertyMetadata(200));

        private void AySplitView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AySplitView_Loaded;

            //if (PanePlacement == SplitViewPanePlacement.Left)//因为默认值是左，所以要处理，其他方向会触发依赖属性的回调函数，所以不用写ay
            //{
                DisplayModeOrPlacementChanged(this);
            //}
        }
        private static void DisplayModeOrPlacementChanged(AySplitView sv)
        {
            if (sv.LeftArea.IsNull())
            {
                return;
            }
                if (sv.PanePlacement == SplitViewPanePlacement.Left || sv.PanePlacement == SplitViewPanePlacement.Right)
            {
                sv.rowTop.Height = new GridLength(1, GridUnitType.Star);
                sv.rowBottom.Height = new GridLength(1, GridUnitType.Star);
                Grid.SetRowSpan(sv.LeftArea, 2);
                Grid.SetRowSpan(sv.ContentArea, 2);
                Grid.SetRow(sv.LeftArea, 0);
                Grid.SetRow(sv.ContentArea, 0);

                if (sv.PanePlacement == SplitViewPanePlacement.Left)
                {
                    sv.colContent.Width = new GridLength(1, GridUnitType.Star);
                    sv.colLeft.Width = GridLength.Auto;
                    sv.LeftArea.HorizontalAlignment = HorizontalAlignment.Left;
                    sv.LeftArea.VerticalAlignment = VerticalAlignment.Stretch;

                    if (sv.DisplayMode == SplitViewDisplayMode.CompactOverlay || sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {
                        Grid.SetColumn(sv.LeftArea, 0);
                        Grid.SetColumn(sv.ContentArea, 0);
                        Grid.SetColumnSpan(sv.LeftArea, 2);
                        Grid.SetColumnSpan(sv.ContentArea, 2);
                        sv.LeftArea.Margin = new Thickness(0);
                    }
                    else
                    {
                        Grid.SetColumn(sv.LeftArea, 0);
                        Grid.SetColumn(sv.ContentArea, 1);
                        Grid.SetColumnSpan(sv.LeftArea, 1);
                        Grid.SetColumnSpan(sv.ContentArea, 1);
                    }
                }
                else if (sv.PanePlacement == SplitViewPanePlacement.Right)
                {
                    sv.colLeft.Width = new GridLength(1, GridUnitType.Star);
                    sv.colContent.Width = GridLength.Auto;
                    sv.LeftArea.HorizontalAlignment = HorizontalAlignment.Right;
                    sv.LeftArea.VerticalAlignment = VerticalAlignment.Stretch;

                    if (sv.DisplayMode == SplitViewDisplayMode.CompactOverlay || sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {
                        Grid.SetColumn(sv.LeftArea, 0);
                        Grid.SetColumn(sv.ContentArea, 0);
                        Grid.SetColumnSpan(sv.LeftArea, 2);
                        Grid.SetColumnSpan(sv.ContentArea, 2);
                        sv.LeftArea.Margin = new Thickness(0);
                    }
                    else
                    {
                        Grid.SetColumn(sv.LeftArea, 1);
                        Grid.SetColumn(sv.ContentArea, 0);
                        Grid.SetColumnSpan(sv.LeftArea, 1);
                        Grid.SetColumnSpan(sv.ContentArea, 1);

                    }
                }

                OnLengthChanged(sv);
            }
            else //上下的处理www.ayjs.net 六安杨洋（AY）拓展 2016-6-24 11:33:54 ay花费2天
            {
                sv.colContent.Width = new GridLength(1, GridUnitType.Star);
                sv.colLeft.Width = new GridLength(1, GridUnitType.Star);
                Grid.SetColumnSpan(sv.LeftArea, 2);
                Grid.SetColumnSpan(sv.ContentArea, 2);
                Grid.SetColumn(sv.LeftArea, 0);
                Grid.SetColumn(sv.ContentArea, 0);
                if (sv.PanePlacement == SplitViewPanePlacement.Top)
                {
                    sv.rowTop.Height = GridLength.Auto;
                    sv.rowBottom.Height = new GridLength(1, GridUnitType.Star);
                    sv.LeftArea.VerticalAlignment = VerticalAlignment.Top;
                    sv.LeftArea.HorizontalAlignment = HorizontalAlignment.Stretch;

                    if (sv.DisplayMode == SplitViewDisplayMode.CompactOverlay || sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {
                        Grid.SetRow(sv.LeftArea, 0);
                        Grid.SetRow(sv.ContentArea, 0);
                        Grid.SetRowSpan(sv.LeftArea, 2);
                        Grid.SetRowSpan(sv.ContentArea, 2);
                        sv.LeftArea.Margin = new Thickness(0);
                    }
                    else
                    {
                        Grid.SetRow(sv.LeftArea, 0);
                        Grid.SetRow(sv.ContentArea, 1);
                        Grid.SetRowSpan(sv.LeftArea, 1);
                        Grid.SetRowSpan(sv.ContentArea, 1);
                    }
                }
                else if (sv.PanePlacement == SplitViewPanePlacement.Bottom)
                {
                    sv.rowBottom.Height = GridLength.Auto;
                    sv.rowTop.Height = new GridLength(1, GridUnitType.Star);

                    sv.LeftArea.VerticalAlignment = VerticalAlignment.Bottom;
                    sv.LeftArea.HorizontalAlignment = HorizontalAlignment.Stretch;


                    if (sv.DisplayMode == SplitViewDisplayMode.CompactOverlay || sv.DisplayMode == SplitViewDisplayMode.Overlay)
                    {
                        Grid.SetRow(sv.LeftArea, 0);
                        Grid.SetRow(sv.ContentArea, 0);
                        Grid.SetRowSpan(sv.LeftArea, 2);
                        Grid.SetRowSpan(sv.ContentArea, 2);
                        sv.LeftArea.Margin = new Thickness(0);
                    }
                    else
                    {
                        Grid.SetRow(sv.LeftArea, 1);
                        Grid.SetRow(sv.ContentArea, 0);
                        Grid.SetRowSpan(sv.LeftArea, 1);
                        Grid.SetRowSpan(sv.ContentArea, 1);
                    }
                }

                OnLengthChanged(sv);

            }
        }

    }
}
