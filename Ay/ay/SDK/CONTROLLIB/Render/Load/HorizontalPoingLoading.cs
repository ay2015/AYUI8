using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ay.Controls
{
	public class HorizontalPoingLoading : UserControl
	{
		public static readonly DependencyProperty EllipseDiameterProperty;

		public static readonly DependencyProperty FillBrushProperty;

		public double EllipseDiameter
		{
			get
			{
				return (double)GetValue(EllipseDiameterProperty);
			}
			set
			{
				SetValue(EllipseDiameterProperty, value);
			}
		}

		public Brush FillBrush
		{
			get
			{
				return (Brush)GetValue(FillBrushProperty);
			}
			set
			{
				SetValue(FillBrushProperty, value);
			}
		}

		private static void OnEllipseDiameterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			HorizontalPoingLoading horizontalPoingLoading;
			if ((horizontalPoingLoading = (sender as HorizontalPoingLoading)) != null)
			{
				horizontalPoingLoading.EllipseDiameter = (double)e.NewValue;
			}
		}

		static HorizontalPoingLoading()
		{
			EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(HorizontalPoingLoading), new PropertyMetadata(5.0, OnEllipseDiameterChanged));
			FillBrushProperty = DependencyProperty.Register("FillBrush", typeof(Brush), typeof(HorizontalPoingLoading), new PropertyMetadata(Brushes.Blue));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalPoingLoading), new FrameworkPropertyMetadata(typeof(HorizontalPoingLoading)));
		}
	}
}
