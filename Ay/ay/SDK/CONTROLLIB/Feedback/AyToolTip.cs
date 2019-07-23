using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ay.Controls
{
    /// <summary>
    /// AyTooltip.xaml 的交互逻辑
    /// </summary>
    [ContentProperty("TooltipContent")]
    [TemplatePart(Name = "PART_ToolTipHost", Type = typeof(ContentPresenter))]
    public  class AyTooltip : ContentControl
    { 
        /// <summary>
        /// 重写背景色 2015-10-8 15:10:58
        /// ay 杨洋
        /// </summary>

        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(AyTooltip), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));




        public Thickness RectangleMargin
        {
            get { return (Thickness)GetValue(RectangleMarginProperty); }
            set { SetValue(RectangleMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RectangleMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectangleMarginProperty =
            DependencyProperty.Register("RectangleMargin", typeof(Thickness), typeof(AyTooltip), new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 8), FrameworkPropertyMetadataOptions.AffectsRender));



        public double SanJiaoAngle
        {
            get { return (double)GetValue(SanJiaoAngleProperty); }
            set { SetValue(SanJiaoAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SanJiaoAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SanJiaoAngleProperty =
            DependencyProperty.Register("SanJiaoAngle", typeof(double), typeof(AyTooltip), new FrameworkPropertyMetadata(180.00, FrameworkPropertyMetadataOptions.AffectsRender));



        public Thickness SanJiaoMargin
        {
            get { return (Thickness)GetValue(SanJiaoMarginProperty); }
            set { SetValue(SanJiaoMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SanJiaoMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SanJiaoMarginProperty =
            DependencyProperty.Register("SanJiaoMargin", typeof(Thickness), typeof(AyTooltip), new FrameworkPropertyMetadata(new Thickness(17, 0, 0, -4.0), FrameworkPropertyMetadataOptions.AffectsRender));




        public VerticalAlignment SanJiaoVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(SanJiaoVerticalAlignmentProperty); }
            set { SetValue(SanJiaoVerticalAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SanJiaoVerticalAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SanJiaoVerticalAlignmentProperty =
            DependencyProperty.Register("SanJiaoVerticalAlignment", typeof(VerticalAlignment), typeof(AyTooltip), new FrameworkPropertyMetadata(VerticalAlignment.Bottom, FrameworkPropertyMetadataOptions.AffectsRender));



        public HorizontalAlignment SanJiaoHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(SanJiaoHorizontalAlignmentProperty); }
            set { SetValue(SanJiaoHorizontalAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SanJiaoHorizontalAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SanJiaoHorizontalAlignmentProperty =
            DependencyProperty.Register("SanJiaoHorizontalAlignment", typeof(HorizontalAlignment), typeof(AyTooltip), new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.AffectsRender));




        public double SanJiaoWidth
        {
            get { return (double)GetValue(SanJiaoWidthProperty); }
            set { SetValue(SanJiaoWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SanJiaoWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SanJiaoWidthProperty =
            DependencyProperty.Register("SanJiaoWidth", typeof(double), typeof(AyTooltip), new FrameworkPropertyMetadata(20.25, FrameworkPropertyMetadataOptions.AffectsRender));



        public double SanJiaoHeight
        {
            get { return (double)GetValue(SanJiaoHeightProperty); }
            set { SetValue(SanJiaoHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SanJiaoHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SanJiaoHeightProperty =
            DependencyProperty.Register("SanJiaoHeight", typeof(double), typeof(AyTooltip), new FrameworkPropertyMetadata(12.29, FrameworkPropertyMetadataOptions.AffectsRender));



        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentMarginProperty =
            DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(AyTooltip), new FrameworkPropertyMetadata(new Thickness(8, 8, 8, 16), FrameworkPropertyMetadataOptions.AffectsRender));




        public DataTemplate ToolTipDataTemplate
        {
            get { return (DataTemplate)GetValue(ToolTipDataTemplateProperty); }
            set { SetValue(ToolTipDataTemplateProperty, value); }
        }

        public static readonly DependencyProperty ToolTipDataTemplateProperty =
            DependencyProperty.Register("ToolTipDataTemplate", typeof(DataTemplate), typeof(AyTooltip), new PropertyMetadata(null));


        /// <summary>
        /// 三角形位置
        /// ay 杨洋
        /// </summary>
        public Dock Placement
        {
            get { return (Dock)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(Dock), typeof(AyTooltip), new PropertyMetadata(Dock.Bottom, new PropertyChangedCallback(DockChanged)));


        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RadiusX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusXProperty =
            DependencyProperty.Register("RadiusX", typeof(double), typeof(AyTooltip), new PropertyMetadata(0.00));



        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RadiusY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusYProperty =
            DependencyProperty.Register("RadiusY", typeof(double), typeof(AyTooltip), new PropertyMetadata(0.00));


        private static void DockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AyTooltip t = d as AyTooltip;
            if (t != null)
            {
                switch (t.Placement)
                {
                    case Dock.Left:
                        t.RectangleMargin = new Thickness(0, 0, 0, 0);
                        t.SanJiaoAngle = 270;
                        t.SanJiaoMargin = new Thickness(-11, 0, 0, 0);
                        t.ContentMargin = new Thickness(12, 8, 16, 8);
                        t.SanJiaoVerticalAlignment = VerticalAlignment.Center;
                        t.SanJiaoHorizontalAlignment = HorizontalAlignment.Left;
                        t.SanJiaoWidth = 14;
                        t.SanJiaoHeight = 9;
                        break;
                    case Dock.Top:
                        t.RectangleMargin = new Thickness(0, 8, 0, 0);
                        t.SanJiaoAngle = 0;
                        t.SanJiaoMargin = new Thickness(17, -4, 0, 0);
                        t.ContentMargin = new Thickness(8, 16, 8, 8);
                        t.SanJiaoVerticalAlignment = VerticalAlignment.Top;
                        t.SanJiaoHorizontalAlignment = HorizontalAlignment.Left;
                        t.SanJiaoWidth = 20.25;
                        t.SanJiaoHeight = 12.29;

                        break;
                    case Dock.Right:
                        t.RectangleMargin = new Thickness(0, 0, 0, 0);
                        t.SanJiaoAngle = 90;
                        t.SanJiaoMargin = new Thickness(0, 0, -11, 0);
                        t.ContentMargin = new Thickness(12, 8, 16, 8);
                        t.SanJiaoVerticalAlignment = VerticalAlignment.Center;
                        t.SanJiaoHorizontalAlignment = HorizontalAlignment.Right;
                        t.SanJiaoWidth = 14;
                        t.SanJiaoHeight = 9;
                        break;
                    case Dock.Bottom:
                        t.RectangleMargin = new Thickness(0, 0, 0, 8);
                        t.SanJiaoAngle = 180.00;
                        t.SanJiaoMargin = new Thickness(17, 0, 0, -4.0);
                        t.ContentMargin = new Thickness(8, 8, 8, 16);
                        t.SanJiaoVerticalAlignment = VerticalAlignment.Bottom;
                        t.SanJiaoHorizontalAlignment = HorizontalAlignment.Left;
                        t.SanJiaoWidth = 20.25;
                        t.SanJiaoHeight = 12.29;

                        break;
                    default:

                        break;
                }
            }
        }

        public object TooltipContent
        {
            get { return (object)GetValue(TooltipContentProperty); }
            set { SetValue(TooltipContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TooltipContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TooltipContentProperty =
            DependencyProperty.Register("TooltipContent", typeof(object), typeof(AyTooltip), new PropertyMetadata(null));








    }
}
