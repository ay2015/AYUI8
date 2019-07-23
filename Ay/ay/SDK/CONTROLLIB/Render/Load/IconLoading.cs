using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
	public class IconLoading : UserControl
	{
		public static readonly DependencyProperty IconProperty;

		public static readonly DependencyProperty IconSizeProperty;

		public string Icon
		{
			get
			{
				return (string)GetValue(IconProperty);
			}
			set
			{
				SetValue(IconProperty, value);
			}
		}

		public double IconSize
		{
			get
			{
				return (double)GetValue(IconSizeProperty);
			}
			set
			{
				SetValue(IconSizeProperty, value);
			}
		}

		static IconLoading()
		{
			IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(IconLoading), new PropertyMetadata(""));
			IconSizeProperty = DependencyProperty.Register("IconSize", typeof(double), typeof(IconLoading), new PropertyMetadata(20.0));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(IconLoading), new FrameworkPropertyMetadata(typeof(IconLoading)));
		}
	}
}
