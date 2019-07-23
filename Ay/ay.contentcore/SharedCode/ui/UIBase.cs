using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ay.contentcore
{

    /// <summary>
    /// 定义UI控件基本附加属性
    /// </summary>
    public class UIBase : DependencyObject
    {



        public static TextAlignment GetTextAlignment(DependencyObject obj)
        {
            return (TextAlignment)obj.GetValue(TextAlignmentProperty);
        }

        public static void SetTextAlignment(DependencyObject obj, TextAlignment value)
        {
            obj.SetValue(TextAlignmentProperty, value);
        }

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.RegisterAttached("TextAlignment", typeof(TextAlignment), typeof(UIBase), new PropertyMetadata(TextAlignment.Left));



        public static bool GetHasUnderLine(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasUnderLineProperty);
        }

        public static void SetHasUnderLine(DependencyObject obj, bool value)
        {
            obj.SetValue(HasUnderLineProperty, value);
        }

        public static readonly DependencyProperty HasUnderLineProperty =
            DependencyProperty.RegisterAttached("HasUnderLine", typeof(bool), typeof(UIBase), new PropertyMetadata(true));


        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }
        /// <summary>
        /// 基本圆角
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(UIBase), new PropertyMetadata(new CornerRadius(4)));




        public static string GetIcon(DependencyObject obj)
        {
            return (string)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, string value)
        {
            obj.SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(string), typeof(UIBase), new PropertyMetadata(null));



        public static double GetIconWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(IconWidthProperty);
        }

        public static void SetIconWidth(DependencyObject obj, double value)
        {
            obj.SetValue(IconWidthProperty, value);
        }

        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.RegisterAttached("IconWidth", typeof(double), typeof(UIBase), new PropertyMetadata(12.00));



        public static double GetIconHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(IconHeightProperty);
        }

        public static void SetIconHeight(DependencyObject obj, double value)
        {
            obj.SetValue(IconHeightProperty, value);
        }

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.RegisterAttached("IconHeight", typeof(double), typeof(UIBase), new PropertyMetadata(12.00));



        public static Thickness GetBorderThickness(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(BorderThicknessProperty);
        }

        public static void SetBorderThickness(DependencyObject obj, Thickness value)
        {
            obj.SetValue(BorderThicknessProperty, value);
        }

        /// <summary>
        /// 边框的粗细
        /// </summary>
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(UIBase), new PropertyMetadata(new Thickness(1, 1, 1, 1)));




        public static Brush GetBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BorderBrushProperty);
        }

        public static void SetBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(BorderBrushProperty, value);
        }
        /// <summary>
        /// 边框颜色
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.RegisterAttached("BorderBrush", typeof(Brush), typeof(UIBase), new PropertyMetadata(Brushes.Transparent));

        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }
        /// <summary>
        /// 水印文本
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(UIBase), new PropertyMetadata(string.Empty));




        public static HorizontalAlignment GetPlaceholderHorizontalAlignment(DependencyObject obj)
        {
            return (HorizontalAlignment)obj.GetValue(PlaceholderHorizontalAlignmentProperty);
        }

        public static void SetPlaceholderHorizontalAlignment(DependencyObject obj, HorizontalAlignment value)
        {
            obj.SetValue(PlaceholderHorizontalAlignmentProperty, value);
        }


        public static readonly DependencyProperty PlaceholderHorizontalAlignmentProperty =
            DependencyProperty.RegisterAttached("PlaceholderHorizontalAlignment", typeof(HorizontalAlignment), typeof(UIBase), new PropertyMetadata(HorizontalAlignment.Left));




        public static VerticalAlignment GetPlaceholderVerticalAlignment(DependencyObject obj)
        {
            return (VerticalAlignment)obj.GetValue(PlaceholderVerticalAlignmentProperty);
        }

        public static void SetPlaceholderVerticalAlignment(DependencyObject obj, VerticalAlignment value)
        {
            obj.SetValue(PlaceholderVerticalAlignmentProperty, value);
        }

        public static readonly DependencyProperty PlaceholderVerticalAlignmentProperty =
            DependencyProperty.RegisterAttached("PlaceholderVerticalAlignment", typeof(VerticalAlignment), typeof(UIBase), new PropertyMetadata(VerticalAlignment.Center));




        public static Thickness GetPlaceholderMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(PlaceholderMarginProperty);
        }

        public static void SetPlaceholderMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(PlaceholderMarginProperty, value);
        }
        /// <summary>
        /// 水印边距
        /// </summary>
        public static readonly DependencyProperty PlaceholderMarginProperty =
            DependencyProperty.RegisterAttached("PlaceholderMargin", typeof(Thickness), typeof(UIBase), new PropertyMetadata(new Thickness(16, 0, 0, 0)));


        public static bool GetIsRequired(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsRequiredProperty);
        }

        public static void SetIsRequired(DependencyObject obj, bool value)
        {
            obj.SetValue(IsRequiredProperty, value);
        }


        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.RegisterAttached("IsRequired", typeof(bool), typeof(UIBase), new PropertyMetadata(false));





        public static UIStretch GetUIStretch(DependencyObject obj)
        {
            return (UIStretch)obj.GetValue(UIStretchProperty);
        }

        public static void SetUIStretch(DependencyObject obj, UIStretch value)
        {
            obj.SetValue(UIStretchProperty, value);
        }

        public static readonly DependencyProperty UIStretchProperty =
            DependencyProperty.RegisterAttached("UIStretch", typeof(UIStretch), typeof(UIBase), new UIPropertyMetadata(UIStretch.None, new PropertyChangedCallback(OnUIStretchChanged)));

        private static void OnUIStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement f)
            {
                switch ((UIStretch)e.NewValue)
                {
                    case UIStretch.None:
                        break;
                    case UIStretch.HorizonStretch:
                        f.ClearValue(FrameworkElement.MinWidthProperty);
                        f.ClearValue(FrameworkElement.MaxWidthProperty);

                        f.Width = double.NaN;
                        f.HorizontalAlignment = HorizontalAlignment.Stretch;
                        break;
                    case UIStretch.VerticalStretch:
                        f.ClearValue(FrameworkElement.MinHeightProperty);
                        f.ClearValue(FrameworkElement.MaxHeightProperty);
                        f.Height = double.NaN;
                        f.VerticalAlignment = VerticalAlignment.Stretch;
                        break;
                    case UIStretch.HorizonStretchVerticalStretch:

                        f.Width = double.NaN;
                        f.ClearValue(FrameworkElement.MinHeightProperty);
                        f.ClearValue(FrameworkElement.MaxHeightProperty);
                        f.HorizontalAlignment = HorizontalAlignment.Stretch;
                        f.ClearValue(FrameworkElement.MinWidthProperty);
                        f.ClearValue(FrameworkElement.MaxWidthProperty);
                        f.Height = double.NaN;
                        f.VerticalAlignment = VerticalAlignment.Stretch;
                        break;
                    default:
                        break;
                }



            }
        }
    }
    public enum UIStretch
    {
        None,
        HorizonStretch,
        VerticalStretch,
        HorizonStretchVerticalStretch
    }
}
