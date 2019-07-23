using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
	public class TwoPointLoading : UserControl
	{
		public static readonly DependencyProperty EllipseWidthProperty;

		public double EllipseWidth
		{
			get
			{
				return (double)GetValue(EllipseWidthProperty);
			}
			set
			{
				SetValue(EllipseWidthProperty, value);
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

		static TwoPointLoading()
		{
			EllipseWidthProperty = DependencyProperty.Register("EllipseWidth", typeof(double), typeof(HorizontalPoingLoading), new PropertyMetadata(5.0, OnEllipseDiameterChanged));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TwoPointLoading), new FrameworkPropertyMetadata(typeof(TwoPointLoading)));
		}
	}
}
